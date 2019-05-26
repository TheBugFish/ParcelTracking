using AutoMapper;
using FluentValidation.Results;
using SKS.ParcelLogistics.BusinessLogic.Entities;
using SKS.ParcelLogistics.BusinessLogic.Interfaces;
using SKS.ParcelLogistics.DataAccess.Entities;
using SKS.ParcelLogistics.DataAccess.Interfaces;
using System;
using System.Collections.Generic;

namespace SKS.ParcelLogistics.BusinessLogic.Mock
{
    public class MockTrackingLogic : ITrackingLogic
    {
        private readonly IParcelRepository _parcelRepository;
        private readonly IHopRepository _hopRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly ITruckRepository _truckRepository;

        public MockTrackingLogic(IParcelRepository parcelRepository, IHopRepository hopRepository, IWarehouseRepository warehouseRepository, ITruckRepository truckRepository)
        {
            _parcelRepository = parcelRepository;
            _hopRepository = hopRepository;
            _warehouseRepository = warehouseRepository;
            _truckRepository = truckRepository;
        }

        public ParcelModel GetParcelByCode(string ParcelID)
        {
            var recipient = new RecipientModel() { City = "MockCity", FirstName = "MockMartin", LastName = "MockMuster", PostalCode = "A-0000", Street = "Mockstraße 1337" };
            return new ParcelModel() { Recipient = recipient, Latitude = 48.2166205m, Longitude = 16.3958889m, TrackingCode = "MOCKMOCK", Weight = 0.5f };
        }

        public bool AddParcel(ParcelModel parcel)
        {
            return true;
        }

        public bool ReportHop(HopArrivalModel hopModel)
        {
            return true;
        }

        public string GenerateTrackingCode()
        {
            return "MOCKCODE";
        }

        public IList<HopArrivalModel> GetAllPastHops(ParcelModel parcelModel)
        {
            ValidateParcel(parcelModel);
            return new List<HopArrivalModel>();
            //return Mapper.Map<List<HopArrivalDAO>, List<HopArrivalModel>>(_hopRepository.GetPrevHopsForParcel(parcelModel.TrackingCode));
        }

        public bool ValidateParcel(ParcelModel parcel)
        {
            return true;
        }

        public bool ValidateHop(HopArrivalModel hop)
        {
            return true;
        }
    }
}
