using AutoMapper;
using FluentValidation.Results;
using SKS.ParcelLogistics.BusinessLogic.Entities;
using SKS.ParcelLogistics.BusinessLogic.Interfaces;
using SKS.ParcelLogistics.DataAccess.Entities;
using SKS.ParcelLogistics.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SKS.ParcelLogistics.BusinessLogic.Mock
{
    public class MockWarehouseLogic : IWarehouseLogic
    {


        private readonly IWarehouseRepository _warehouseRepository;
        private readonly ITruckRepository _truckRepository;
        private readonly IHopRepository _hopRepository;
        private readonly IParcelRepository _parcelRepository;

        public MockWarehouseLogic(IWarehouseRepository warehouseRepository, ITruckRepository truckRepository, IHopRepository hopRepository, IParcelRepository parcelRepository)
        {
            _warehouseRepository = warehouseRepository;
            _truckRepository = truckRepository;
            _hopRepository = hopRepository;
            _parcelRepository = parcelRepository;
        }

        public bool AddWarehouseRoot(WarehouseModel warehouse)
        {
            return true;
        }



        private bool AddTruck(TruckModel truckModel)
        {
            return true;
        }


        public TruckModel GetTruckByCode(string code)
        {
            return new TruckModel() { Code = "TMOC", Duration = 0.2m, Latitude = 48.21662m, Longitude = 16.395888m, NumberPlate = "MOCKPLATE" };
        }

        public IList<TruckModel> GetAllTrucks()
        {
            return new List<TruckModel>();
        }

        public bool DeleteWarehouseTree()
        {
            return true;
        }

        public WarehouseModel GetWarehouseByCode(string code)
        {
            return new WarehouseModel() { Code = "WMOC", Description = "Mock Warehouse", Duration = 0.5m, Parent = null };
        }

        public bool ValidateWarehouseModel(WarehouseModel warehouse)
        {
            return true;
        }

        public bool ValidateTruckModel(TruckModel truck)
        {
            return true;
        }

        private WarehouseModel GetRootWh()
        {
            return new WarehouseModel();
        }

        public WarehouseModel GetWarehouseHierarchy()
        {
            return new WarehouseModel();
        }

        public IList<HopArrivalModel> GetAllFutureHops(HopArrivalModel latestHop, TruckModel closestTruck, ParcelModel parcelModel)
        {
            return new List<HopArrivalModel>();
        }
    }
}
