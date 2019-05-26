using AutoMapper;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SKS.ParcelLogistics.BusinessLogic;
using SKS.ParcelLogistics.BusinessLogic.Entities;
using SKS.ParcelLogistics.BusinessLogic.Interfaces;
using SKS.ParcelLogistics.BusinessLogic.Mock;
using SKS.ParcelLogistics.DataAccess.Entities;
using SKS.ParcelLogistics.DataAccess.Interfaces;
using SKS.ParcelLogistics.DataAccess.Mock;
using SKS.ParcelLogistics.ServiceAgents;
using System;
using System.Collections.Generic;

namespace SKS.ParcelLogistics.Tests
{
    [TestClass]
    public class WarehouseLogicUnitTests
    {
        private WarehouseLogic warehouse;
        private Mock<IWarehouseRepository> warehouseRepMock;
        private Mock<ITruckRepository> truckRepMock;
        private Mock<IHopRepository> hopRepMock;
        private Mock<IParcelRepository> parcelRepMock;
        private IWarehouseLogic _wl;
        private static readonly ILog log = LogManager.GetLogger(typeof(WarehouseLogicUnitTests));

        public WarehouseLogicUnitTests()
        {
            Mapper.Reset();
            Mapper.Initialize(config =>
            {
                config.AddProfile<MappingProfile>();
                config.AddProfile<ToBLProfile>();
            });

            Mapper.AssertConfigurationIsValid();

            warehouseRepMock = new Mock<IWarehouseRepository>();
            truckRepMock = new Mock<ITruckRepository>();
            hopRepMock = new Mock<IHopRepository>();
            parcelRepMock = new Mock<IParcelRepository>();
            warehouse = new WarehouseLogic(warehouseRepMock.Object, truckRepMock.Object, hopRepMock.Object, parcelRepMock.Object);

            _wl =
                new WarehouseLogic(
                    new MockWarehouseRepository(),
                    new MockTruckRepository(),
                    new MockHopRepository(),
                    new MockParcelRepository());
        }

        [TestMethod]
        public void Add_warehouse_root_should_return_true()
        {         
            warehouseRepMock.Setup(input => input.Create(It.IsAny<WarehouseDTO>()));

            var wh = new WarehouseModel() { Code = "WMOC", Description = "Mock Warehouse", Duration = 0.5m, Parent = null };
  
            var result = warehouse.AddWarehouseRoot(wh);
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Delete_warehouse_tree_should_return_true()
        {
            warehouseRepMock.Setup(input => input.DeleteAll());
            truckRepMock.Setup(input => input.DeleteAll());
            hopRepMock.Setup(input => input.DeleteAll());
            parcelRepMock.Setup(input => input.DeleteAll());
            // Act
            var result = warehouse.DeleteWarehouseTree();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_all_future_hops_should_return_HopArrivalModel_list()
        {
            // Arrange
            HopArrivalModel latest = new HopArrivalModel() { };
            TruckModel closestTruck = new TruckModel() { };
            RecipientModel recipient = new RecipientModel
            {
                FirstName = "Rudi",
                LastName = "Recipient",
                Street = "Poststraße",
                PostalCode = "A-1070",
                City = "Poststadt"
            };
            ParcelModel parcel = new ParcelModel { Recipient = recipient, Weight = 1.0f };

            // Act
            var result = warehouse.GetAllFutureHops(latest, closestTruck, parcel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(IList<HopArrivalModel>));
        }

        [TestMethod]
        public void Get_all_trucks_should_return_list_of_trucks()
        {
            var trucks = (IEnumerable<TruckDTO>)new List<TruckDTO>();
            truckRepMock.Setup(input => input.GetAll()).Returns(trucks);
            // Arrange
            // -- Nothing to do

            // Act
            var result = warehouse.GetAllTrucks();

            // Assert
            Assert.IsInstanceOfType(result, typeof(IList<TruckModel>));
        }

        [TestMethod]
        public void Get_truck_by_code_should_return_truck()
        {
            var truckdto = new TruckDTO{
                Code = "TMOC",
                Parent = null,
                Duration = 0.5m,
                Latitude = 14,
                Longitude = 40,
                NumberPlate = "MOCKTRK-1",
                Radius = 10m
            };

            //warehouseRepository????
            warehouseRepMock.Setup(input => input.GetByCode(It.IsAny<string>())).Returns(truckdto);
            // Arrange
         //   TruckModel truck = new TruckModel() { Code = "TMOC", Parent = null, Duration = 0.5m, Latitude = 14, Longitude = 40, NumberPlate = "MOCKTRK-1", Radius = 10m };
          //  WarehouseModel root = new WarehouseModel() { Code = "ROOT", Description = "Root WH", Duration = 0.5m, Parent = null, Trucks = new List<TruckModel>() { truck } };
           // _wl.AddWarehouseRoot(root);

            // Act
            var result = warehouse.GetTruckByCode("TMOC");

            // Assert
            Assert.IsInstanceOfType(result, typeof(TruckModel));
        }

        [TestMethod]
        public void Get_warehouse_by_code_should_return_warehouse()
        {
            var ware = new WarehouseDTO() { Code = "ROOT", Description = "Root WH", Duration = 0.5m, Parent = null };
            warehouseRepMock.Setup(input => input.GetByCode(It.IsAny<string>())).Returns(ware);
            var result = warehouse.GetWarehouseByCode("ROOT");
            // Assert
            Assert.IsInstanceOfType(result, typeof(WarehouseModel));
        }

        [TestMethod]
        public void Get_warehouse_hierarchy_should_return_warehouse()
        {
            var warehouses = new List<WarehouseDTO>();
            var root = new WarehouseDTO() { Code = "ROOT", Description = "Root WH", Duration = 0.5m, Parent = null };
            warehouses.Add(root);
            var truck = new TruckDTO() { Code = "TMOC", Parent = null, Duration = 0.5m, Latitude = 14, Longitude = 40, NumberPlate = "MOCKTRK-1", Radius = 10m };
            var trucks = new List<TruckDTO>();
            trucks.Add(truck);
            warehouseRepMock.Setup(input => input.FindByParent(null)).Returns(warehouses);
            truckRepMock.Setup(input => input.FindByParent(It.IsAny<WarehouseDTO>())).Returns(trucks);

            // Act
            var result = warehouse.GetWarehouseHierarchy();

            // Assert
            Assert.IsInstanceOfType(result, typeof(WarehouseModel));
        }

        [TestMethod]
        public void Validate_truck_model_should_return_true()
        {
            // Arrange
            TruckModel truck = new TruckModel() { Code = "TMOC", Parent = null, Duration = 0.5m, Latitude = 14, Longitude = 40, NumberPlate = "MOCKTRK-1", Radius = 10m };

            // Act
            var result = _wl.ValidateTruckModel(truck);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Validate_warehouse_model_should_return_true()
        {
            // Arrange
            WarehouseModel wh = new WarehouseModel() { Code = "WMOC", Description = "Mock Warehouse", Duration = 0.5m, Parent = null };

            // Act
            var result = _wl.ValidateWarehouseModel(wh);

            // Assert
            Assert.IsTrue(result);
        }
    }
}