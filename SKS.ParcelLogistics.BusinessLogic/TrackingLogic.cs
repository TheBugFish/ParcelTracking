using AutoMapper;
using FluentValidation.Results;
using SKS.ParcelLogistics.BusinessLogic.Entities;
using SKS.ParcelLogistics.BusinessLogic.Interfaces;
using SKS.ParcelLogistics.DataAccess.Entities;
using SKS.ParcelLogistics.DataAccess.Interfaces;
using System;
using System.Collections.Generic;

namespace SKS.ParcelLogistics.BusinessLogic
{
    public class TrackingLogic : ITrackingLogic
    {
        private ParcelValidator _pVal;
        private HopArrivalValidator _hVal;

        private readonly IParcelRepository _parcelRepository;
        private readonly IHopRepository _hopRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly ITruckRepository _truckRepository;

        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(TrackingLogic));

        public TrackingLogic(IParcelRepository parcelRepository, IHopRepository hopRepository, IWarehouseRepository warehouseRepository , ITruckRepository truckRepository)
        {
            _pVal = new ParcelValidator();
            _hVal = new HopArrivalValidator();

            _parcelRepository = parcelRepository;
            _hopRepository = hopRepository;
            _warehouseRepository = warehouseRepository;
            _truckRepository = truckRepository;
        }

        public ParcelModel GetParcelByCode(string ParcelID)
        {
            try
            {
                var parcel = _parcelRepository.GetByTrackingCode(ParcelID);
                var parcelModel = Mapper.Map<ParcelDTO, ParcelModel>(parcel);

                return ValidateParcel(parcelModel) ? parcelModel : null;
            }
            catch (Exception ex)
            {
                throw new BLException("BL error getting parcel by code: "+ex.Message, ex);
            }
        }

        public bool AddParcel(ParcelModel parcel)
        {
            try
            {
                ValidateParcel(parcel);
                var parcelModel = Mapper.Map<ParcelModel, ParcelDTO>(parcel);

                _parcelRepository.Create(parcelModel);
                _logger.Info("ParcelModel saved to Repo");
                return true;
            }
            catch (Exception ex)
            {
                throw new BLException("BL error adding parcel: "+ex.Message, ex);
            }
        }

        public bool ReportHop(HopArrivalModel hopModel)
        {
            try
            {
                var truck = _truckRepository.GetByCode(hopModel.Code);
                var warehouse = _warehouseRepository.GetByCode(hopModel.Code);
                var parcel = _parcelRepository.GetByTrackingCode(hopModel.TrackingId);

                if (truck == null && warehouse == null)
                    throw new Exception("Location code invalid");
                if (parcel == null)
                    throw new Exception("Parcel trackingId invalid");

                _hopRepository.Create(Mapper.Map<HopArrivalModel, HopArrivalDTO>(hopModel));

                _logger.Info(string.Format("Parcel '{0}' has been scanned at LocationCode '{1}'. Timestamp: {2}", hopModel.TrackingId, hopModel.Code, hopModel.DateTime));
                return true;
            }
            catch (Exception ex)
            {
                throw new BLException("BL error reporting hop: "+ex.Message, ex);
            }
        }

        public string GenerateTrackingCode()
        {
            Guid g = Guid.NewGuid();
            string trackingID = g.ToString("N").Substring(0, 8).ToUpper();
            _logger.Info("Tracking ID Generated: " + trackingID);

            return trackingID;
        }

        public IList<HopArrivalModel> GetAllPastHops(ParcelModel parcelModel)
        {
            ValidateParcel(parcelModel);

            return Mapper.Map<List<HopArrivalDTO>, List<HopArrivalModel>>(_hopRepository.GetPrevHopsForParcel(parcelModel.TrackingCode));
        }

        public bool ValidateParcel(ParcelModel parcel)
        {
            try
            {
                if (parcel == null)
                    return false;

                ValidationResult valRes;
                valRes = _pVal.Validate(parcel);

                if (!valRes.IsValid)
                {
                    throw new BLException(valRes.Errors, new ArgumentException());
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw new BLException("BL error validating parcel: "+ex.Message, ex);
            }
        }

        public bool ValidateHop(HopArrivalModel hop)
        {
            try
            {
                if (hop == null)
                    return false;

                ValidationResult valRes;
                valRes = _hVal.Validate(hop);

                if (!valRes.IsValid)
                {
                    throw new BLException(valRes.Errors, new ArgumentException());
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw new BLException("BL error validating hop: "+ex.Message, ex);
            }
        }
    }
}