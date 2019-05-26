using AutoMapper;
using FluentValidation.Results;
using SKS.ParcelLogistics.BusinessLogic.Entities;
using SKS.ParcelLogistics.BusinessLogic.Interfaces;
using SKS.ParcelLogistics.DataAccess.Entities;
using SKS.ParcelLogistics.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SKS.ParcelLogistics.BusinessLogic
{
    public class WarehouseLogic : IWarehouseLogic
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(WarehouseLogic));
        private WarehouseValidator _wVal;
        private TruckValidator _tVal;

        private readonly IWarehouseRepository _warehouseRepository;
        private readonly ITruckRepository _truckRepository;
        private readonly IHopRepository _hopRepository;
        private readonly IParcelRepository _parcelRepository;
        public WarehouseLogic(IWarehouseRepository warehouseRepository, ITruckRepository truckRepository, IHopRepository hopRepository, IParcelRepository parcelRepository)
        {
            _warehouseRepository = warehouseRepository;
            _truckRepository = truckRepository;
            _hopRepository = hopRepository;
            _parcelRepository = parcelRepository;
            _wVal = new WarehouseValidator();
            _tVal = new TruckValidator();
        }

        public bool AddWarehouseRoot(WarehouseModel warehouse)
        {
            try
            {
               // bool isOK = true;
                if (ValidateWarehouseModel(warehouse))
                {
                    var mapped = Mapper.Map<WarehouseModel,WarehouseDTO>(warehouse);
                    _warehouseRepository.Create(mapped);
                    

                    return true;
                   /* foreach (WarehouseModel wh in warehouse.NextHops)
                    {
                        wh.Parent = warehouse;
                        isOK = isOK && AddWarehouseRecursive(wh);
                    }

                    var warehouseDAO = Mapper.Map<WarehouseModel, WarehouseDAO>(warehouse);
                    _warehouseRepository.Create(warehouseDAO);
                    _logger.Info("WarehouseRoot added: " + warehouseDAO.Code);
                    return isOK;*/
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new BLException("Error setting up warehouses", ex);
            }
        }

       /* private bool AddWarehouseRecursive(WarehouseModel warehouse)
        {
            try
            {
                bool isOK = true;
                if (ValidateWarehouseModel(warehouse))
                {

                    foreach (WarehouseModel wh in warehouse.NextHops)
                    {
                        wh.Parent = warehouse;
                        isOK = isOK && AddWarehouseRecursive(wh);
                    }
                    foreach (TruckModel truck in warehouse.Trucks)
                    {
                        truck.Parent = warehouse;
                        isOK = isOK && AddTruck(truck);
                    }

                    if (isOK)
                    {

                        var warehouseDAO = Mapper.Map<WarehouseModel, WarehouseDAO>(warehouse);
                        _warehouseRepository.Create(warehouseDAO);
                        _logger.Info("Warehouse added: " + warehouseDAO.Code);
                    }
                    return isOK;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new BLException("Error setting up warehouses (recursion)", ex);
            }
            
        }*/

        private bool AddTruck(TruckModel truckModel)
        {
            try
            {
                if (ValidateTruckModel(truckModel))
                {
                    var truckDAO = Mapper.Map<TruckModel, TruckDTO>(truckModel);
                    _truckRepository.Create(truckDAO);
                    _logger.Info("Truck added: " + truckDAO.Code + " [" + truckDAO.NumberPlate + "]");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new BLException("Error adding truck: "+ex.Message+", Inner: "+ex.InnerException.Message, ex);
            }
        }


        public TruckModel GetTruckByCode(string code)
        {
            try
            {
                _logger.Info(string.Format("Getting truck by code {0}", code));
                var truckDAO = _warehouseRepository.GetByCode(code);
                var truckModel = Mapper.Map<WarehouseDTO, TruckModel>(truckDAO);
                ValidateTruckModel(truckModel);

                return truckModel;
            }
            catch (Exception ex)
            {
                throw new BLException("Error getting truck", ex);
            }
        }

        public IList<TruckModel> GetAllTrucks()
        {
            try
            {
                List<TruckDTO> truckDAOs = _truckRepository.GetAll().ToList();
                var truckModels = Mapper.Map<List<TruckDTO>, List<TruckModel>>(truckDAOs);

                return truckModels;
            }
            catch (Exception ex)
            {
                throw new BLException("Error getting trucks", ex);
            }
        }

        public bool DeleteWarehouseTree()
        {
            try
            {
                _warehouseRepository.DeleteAll();
                _truckRepository.DeleteAll();
                _hopRepository.DeleteAll(); // TODO maybe hops don't have to be deleted for archiving reasons?
                _parcelRepository.DeleteAll();
                _logger.Info("Deleted all warehouses, trucks and hops.");
                return true;
            }
            catch (Exception ex)
            {
                throw new BLException("Error deleting warehouses & trucks & hops: "+ex.Message+", Inner: "+ex.InnerException, ex);
            }
        }

        public WarehouseModel GetWarehouseByCode(string code)
        {
            try
            {
                var warehouse = _warehouseRepository.GetByCode(code);
                var warehouseModel = Mapper.Map<WarehouseDTO, WarehouseModel>(warehouse);
                ValidateWarehouseModel(warehouseModel);

                return warehouseModel;
            }
            catch (Exception ex)
            {
                throw new BLException("Error loading warehouses", ex);
            }
        }

        public bool ValidateWarehouseModel(WarehouseModel warehouse)
        {
            try
            {
                ValidationResult valRes = _wVal.Validate(warehouse);

                if (!valRes.IsValid)
                {
                    throw new BLException(valRes.Errors, new ArgumentException());
                }
                return true;
            }
            catch (BLException ex)
            {
                _logger.Error(ex.Message);
                throw ex;
            }
        }

        public bool ValidateTruckModel(TruckModel truck)
        {
            try
            {
                ValidationResult valRes = _tVal.Validate(truck);

                if (!valRes.IsValid)
                {
                    throw new BLException(valRes.Errors, new ArgumentException());
                }
                return true;
            }
            catch (BLException ex)
            {
                _logger.Error(ex.Message);
                throw ex;
            }
        }

        private WarehouseModel GetRootWh()
        {
            return Mapper.Map<WarehouseDTO, WarehouseModel>(_warehouseRepository.FindByParent(null).FirstOrDefault());
        }

        public WarehouseModel GetWarehouseHierarchy()
        {
            try
            {
                WarehouseModel rootWh = GetRootWh();

                if (rootWh != null)
                {
                    AddRecursiveWarehouseHierarchy(rootWh);
                }

                _logger.Debug("warehouse root is: " + rootWh);
                return rootWh;
            }
            catch (Exception ex)
            {
                throw new BLException("Error loading warehouse hierarchy", ex);
            }
        }

        private WarehouseModel AddRecursiveWarehouseHierarchy(WarehouseModel warehouse)
        {
            try
            {
                var warehouseModel = Mapper.Map<WarehouseModel, WarehouseDTO>(warehouse);

                warehouse.Trucks = Mapper.Map<List<TruckDTO>, List<TruckModel>>(_truckRepository.FindByParent(warehouseModel));
                warehouse.NextHops = Mapper.Map<List<WarehouseDTO>, List<WarehouseModel>>(_warehouseRepository.FindByParent(warehouseModel));

                foreach (WarehouseModel wh in warehouse.NextHops)
                {
                    AddRecursiveWarehouseHierarchy(wh);
                }

                return warehouse;
            }
            catch (Exception ex)
            {
                throw new BLException("Error loading warehouse (recursion)", ex);
            }
        }


        public IList<HopArrivalModel> GetAllFutureHops(HopArrivalModel latestHop, TruckModel closestTruck, ParcelModel parcelModel)
        {
            _logger.Debug(string.Format("WarehouseLogic.GetAllFutureHops: latestHop: {0}, closestTruck: {1}, parcelModel: {2}", (latestHop != null ? latestHop.Code : "null"), (closestTruck != null ? closestTruck.Code : "null"), (parcelModel != null ? parcelModel.TrackingCode : "null")));
            //Wenn es keinen Truck in Reichweite gibt, wirf eine Exception
            if (closestTruck == null)
                throw new Exception("No truck in range!");

            //Sonst: bereite die Datencontainer vor
            HopArrivalModel closestTruckHop = new HopArrivalModel() { Code = closestTruck.Code, TrackingId = parcelModel.TrackingCode, DateTime = DateTime.Now /*TODO!!*/};
            List<HopArrivalModel> futureHops = new List<HopArrivalModel>();

            //Wenn der letzte HopReport schon der Truck war, dann gibt es keine weiteren Reports
            //Shortcut evaluation, kann nicht auf null.Code zugreifen!
            // -> leere Liste
            if (latestHop != null && latestHop.Code == closestTruck.Code)
                return futureHops;


            //Wenn der letzte Report kein Truck war, dann füge den Truck in den Container
            futureHops.Add(closestTruckHop);

            //Wenn der Truck keinen Parent hat, wirf eine exception
            if (closestTruck.Parent == null)
                throw new Exception("Closest truck has no parent!");

            //Sonst: setze den Parent auf den Parent des Zieltrucks
            WarehouseModel parent = closestTruck.Parent;


            //Schleife, bis parent null (Root gefunden)
            int iter = 10; //Forcierter Schleifenabbruch
            while (parent != null && iter-- > 0)
            {
                _logger.Debug(string.Format("comparing parentCode '{0}' to lastHopCode '{1}'", parent.Code, latestHop != null ? latestHop.Code : "null"));
                if (latestHop == null || parent.Code != latestHop.Code)
                {
                    _logger.Debug(string.Format("save parent '{0}' to list", parent.Code));
                    futureHops.Add(new HopArrivalModel()
                    {
                        Code = parent.Code,
                        TrackingId = parcelModel.TrackingCode,
                        DateTime = DateTime.Now
                    });
                }
                else
                {
                    _logger.Debug(string.Format("break loop: parent '{0}' ", parent.Code));
                    break;
                }
                parent = Mapper.Map<WarehouseDTO, WarehouseModel>(_warehouseRepository.ParentOf(parent.Code));
            }

            //gib sortierte (TODO) liste an futurehops zurück
            return futureHops.OrderByDescending(h => h.DateTime).ToList();
        }
    }
}
