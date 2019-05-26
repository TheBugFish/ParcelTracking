using SKS.ParcelLogistics.BusinessLogic.Domain;
using SKS.ParcelLogistics.BusinessLogic.Entities;
using SKS.ParcelLogistics.BusinessLogic.Interfaces;
using SKS.ParcelLogistics.ServiceAgents.Interfaces;
using System;
using System.Collections.Generic;

namespace SKS.ParcelLogistics.BusinessLogic
{
    public class BL : IBusinessLogic
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(BL));

        private readonly IWarehouseLogic _warehouseLogic;
        private readonly ITrackingLogic _trackingLogic;
        private readonly IGeoEncodingAgent _geoAgent;

        public BL(ITrackingLogic trackingLogic, IWarehouseLogic warehouseLogic, IGeoEncodingAgent geoAgent)
        {
            _warehouseLogic = warehouseLogic;
            _trackingLogic = trackingLogic;
            _geoAgent = geoAgent;
        }

        public bool AddParcel(ParcelModel parcel)
        {
            return _trackingLogic.AddParcel(parcel);
        }

        public ParcelModel GetParcelByCode(string ID)
        {
            try
            {
                _logger.Info("executing BL.GetParcelByCode: " + ID);
                return _trackingLogic.GetParcelByCode(ID);
            }
            catch (Exception e)
            {
                throw new BLException("Error Getting Parcel", e);
            }
           
        }

        public string GetNewTrackingID()
        {
            return _trackingLogic.GenerateTrackingCode();
        }

        public bool DeleteAllWarehouses()
        {
            return _warehouseLogic.DeleteWarehouseTree();
        }

        public string OnBoardParcel(ParcelModel parcel)
        {
            try
            {
                // i.   Validation of parcel data
                _trackingLogic.ValidateParcel(parcel);

                // ii.  Create new Tracking Code
                string trackingCode = _trackingLogic.GenerateTrackingCode();
                parcel.TrackingCode = trackingCode;

                // iii. Get GPS coordinates for package address (using Geo Encoding Agent) 
                // TODO save geo to DB, maybe save in parcel domain obj?
                GeoPoint geo = _geoAgent.EncodeAddress(string.Format("{0}, {1} {2}", parcel.Recipient.Street, parcel.Recipient.PostalCode, parcel.Recipient.City));
                parcel.Latitude = geo.Lat;
                parcel.Longitude = geo.Lon;

                // iv.  Write data to database
                _trackingLogic.AddParcel(parcel);

                // v.   Return TrackingCode
                return trackingCode;
            }
            catch (Exception ex)
            {
                throw new BLException("", ex);
            }
        }

        public bool ImportWarehouses(WarehouseModel warehouse)
        {
            try
            {
                // i.   Validation of data
                _warehouseLogic.ValidateWarehouseModel(warehouse);
                // ii.  Clear the existing DB(because of warehouses)
                _warehouseLogic.DeleteWarehouseTree();
                // iii. Write warehouse hierarchy to DB
                return _warehouseLogic.AddWarehouseRoot(warehouse);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ReportParcelHop(string parcelTrackingCode, string locationCode)
        {
            try
            {
                // i.   Validation of data
                HopArrivalModel hop = new HopArrivalModel() { TrackingId = parcelTrackingCode, Code = locationCode, DateTime = DateTime.Now };
                _trackingLogic.ValidateHop(hop);

                // ii.  Write new hop for parcel journey to DB
                return _trackingLogic.ReportHop(hop);
            }
            catch (Exception ex)
            {
                throw new BLException("BL error reporting hop: "+ex.Message, ex);
            }
        }

        public TrackingInformationModel TrackParcel(string parcelCode)
        {
            try
            {
                TrackingInformationModel result = new TrackingInformationModel();

                // i.   Validation of Tracking Code 
                ParcelModel parcel = _trackingLogic.GetParcelByCode(parcelCode);
                _trackingLogic.ValidateParcel(parcel);

                // ii.  Get all previously visited hops for package from DB
                result.VisitedHops = _trackingLogic.GetAllPastHops(parcel) as List<HopArrivalModel>;

                // iii. Predict future hops to final destination
                //      1.Do a GPS distance search e.g.
                //        https://dotnet-snippets.de/snippet/entfernung-zwischen-zwei-geografischen-koordinaten-berechnen/828
                //      2.Find the closest Truck
                //      3.Find the truck’s warehouse, the warehouses parent warehouse, etc.
                //        until you reach the last PAST hop.

                List<TruckModel> trucks = _warehouseLogic.GetAllTrucks() as List<TruckModel>;
                TruckModel closestTruck = null;
                decimal minDistance = decimal.MaxValue;

                for (int i = 0; i < trucks.Count; i++)
                {
                    if (trucks[i] == null)
                        continue;
                    GeoPoint parcelCoords = new GeoPoint(parcel.Latitude, parcel.Longitude);
                    var distance = (decimal)parcelCoords.DistToOtherInKm(new GeoPoint(trucks[i].Latitude, trucks[i].Longitude));
                    if (distance < minDistance && trucks[i].Radius >= distance)
                    {
                        closestTruck = trucks[i];
                        minDistance = distance;
                    }
                }

                /*NOT finished, just testing */
                HopArrivalModel latestHop;
                if (result.VisitedHops.Count > 0)
                {
                    latestHop = result.VisitedHops[result.VisitedHops.Count-1];
                }
                else
                {
                    latestHop = null;
                }
                result.FutureHops = _warehouseLogic.GetAllFutureHops(latestHop, closestTruck, parcel) as List<HopArrivalModel>;
                
                /* end testing */
                switch (result.FutureHops.Count)
                {
                    case 0:
                        result.State = StateEnum.DeliveredEnum;
                        break;
                    case 1:
                        result.State = StateEnum.InTruckDeliveryEnum;
                        break;
                    default:
                        result.State = StateEnum.InTransportEnum;
                        break;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw new BLException("BL error tracking parcels: " + ex.Message, ex);
            }
        }

        public WarehouseModel GetWarehouseHierarchy()
        {
            return _warehouseLogic.GetWarehouseHierarchy();
        }
    }
}