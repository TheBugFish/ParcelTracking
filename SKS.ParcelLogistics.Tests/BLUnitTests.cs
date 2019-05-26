using AutoMapper;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SKS.ParcelLogistics.BusinessLogic;
using SKS.ParcelLogistics.BusinessLogic.Entities;
using SKS.ParcelLogistics.BusinessLogic.Interfaces;
using SKS.ParcelLogistics.BusinessLogic.Mock;
using SKS.ParcelLogistics.DataAccess.Mock;
using SKS.ParcelLogistics.ServiceAgents;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Formatters;
using Moq;
using SKS.ParcelLogistics.BusinessLogic.Domain;
using SKS.ParcelLogistics.ServiceAgents.Interfaces;

namespace SKS.ParcelLogistics.Tests
{
    [TestClass]
    public class BLUnitTests
    {
        private Mock<ITrackingLogic> tracking;
        private Mock<IWarehouseLogic> warehouse;
        private Mock<IGeoEncodingAgent> geoencoding;
        private static readonly ILog log = LogManager.GetLogger(typeof(BLUnitTests));
        private BL _business;

        public BLUnitTests()
        {
            Mapper.Reset();
            Mapper.Initialize(config =>
            {
                config.AddProfile<MappingProfile>();
                config.AddProfile<ToBLProfile>();
            });

            Mapper.AssertConfigurationIsValid();

             tracking = new Mock<ITrackingLogic>();
             warehouse = new Mock<IWarehouseLogic>();
             geoencoding = new Mock<IGeoEncodingAgent>();
            _business = new BL(tracking.Object, warehouse.Object, geoencoding.Object);
        }

        [TestMethod]
        public void Add_Parcel_should_return_true()
        {
            // Arrange
            tracking.Setup(input => input.AddParcel(It.IsAny<ParcelModel>())).Returns(true);
            
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
            var result = _business.AddParcel(parcel);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_Parcel_By_Code_should_return_Parcel()
        {
            var recipient = new RecipientModel
            {
                FirstName = "Rudi",
                LastName = "Recipient",
                Street = "Poststraße",
                PostalCode = "A-1070",
                City = "Poststadt"
            };
            var parcel = new ParcelModel { Recipient = recipient, Weight = 1.0f };


            tracking.Setup(input => input.GetParcelByCode(It.IsAny<string>())).Returns(parcel);
            // Arrange
            // -- Nothing to do

            // Act
            var result = _business.GetParcelByCode("MOCKCODE");

            // Assert
            Assert.IsInstanceOfType(result, typeof(ParcelModel));
        }

        [TestMethod]
        public void Get_TrackingID_should_return_string()
        {
            tracking.Setup(input => input.GenerateTrackingCode()).Returns("12345ABC");


            // Arrange
            // -- Nothing to do

            // Act
            var result = _business.GetNewTrackingID();

            // Assert
            Assert.AreEqual(result, "12345ABC");
        }

        [TestMethod]
        public void Delete_Warehouse_should_return_true()
        {
            warehouse.Setup(input => input.DeleteWarehouseTree()).Returns(true);

            // Act
            var result = _business.DeleteAllWarehouses();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void OnBoardParcel_should_return_TrackingId()
        {
            var recipient = new RecipientModel
            {
                FirstName = "Rudi",
                LastName = "Recipient",
                Street = "Poststraße",
                PostalCode = "A-1070",
                City = "Poststadt"
            };
            var parcel = new ParcelModel { Recipient = recipient, Weight = 1.0f };

            tracking.Setup(input => input.AddParcel(It.IsAny<ParcelModel>())).Returns(true);
            tracking.Setup(input => input.GenerateTrackingCode()).Returns("12345ABC");
            tracking.Setup(input => input.ValidateParcel(It.IsAny<ParcelModel>())).Returns(true);
            geoencoding.Setup(input => input.EncodeAddress(It.IsAny<string>())).Returns(new GeoPoint(21, 12));

            var result = _business.OnBoardParcel(parcel);

            Assert.AreEqual("12345ABC", result);
        }

        [TestMethod]
        public void Add_Warehouses_should_return_true()
        {
            var ware = new WarehouseModel
            {
                Code = "1234",
                Description = "root warehouse",
                Duration = 12
            };
            warehouse.Setup(input => input.ValidateWarehouseModel(It.IsAny<WarehouseModel>())).Returns(true);
            warehouse.Setup(input => input.DeleteWarehouseTree()).Returns(true);
            warehouse.Setup(input => input.AddWarehouseRoot(It.IsAny<WarehouseModel>())).Returns(true);

            var result = _business.ImportWarehouses(ware);
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Report_Hop_should_return_true()
        {
            tracking.Setup(input => input.ValidateParcel(It.IsAny<ParcelModel>())).Returns(true);
            tracking.Setup(input => input.ReportHop(It.IsAny<HopArrivalModel>())).Returns(true);

            var result = _business.ReportParcelHop("MOCKCODE", "TMOC");
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Get_WarehouseHierarchy_should_return_Warehouse()
        {
            var ware = new WarehouseModel
            {
                Code = "1234",
                Description = "root warehouse",
                Duration = 12
            };

            warehouse.Setup(input => input.GetWarehouseHierarchy()).Returns(ware);
          
            var result = _business.GetWarehouseHierarchy();

            // Assert
            Assert.IsInstanceOfType(result, typeof(WarehouseModel));
            Assert.AreEqual("1234", result.Code);
        }

        [TestMethod]
        public void Track_Parcel_should_return_TrackingInformationModel()
        {
            var recipient = new RecipientModel
            {
                FirstName = "Rudi",
                LastName = "Recipient",
                Street = "Poststraße",
                PostalCode = "A-1070",
                City = "Poststadt"
            };
            var parcel = new ParcelModel { Recipient = recipient, Weight = 1.0f };

            var hops = new List<HopArrivalModel>();
            var trucks = new List<TruckModel>();

            tracking.Setup(input => input.GetParcelByCode(It.IsAny<string>())).Returns(parcel);
            tracking.Setup(input => input.ValidateParcel(It.IsAny<ParcelModel>())).Returns(true);
            tracking.Setup(input => input.GetAllPastHops(It.IsAny<ParcelModel>())).Returns(hops);
            warehouse.Setup(input =>
                    input.GetAllFutureHops(It.IsAny<HopArrivalModel>(), It.IsAny<TruckModel>(),
                        It.IsAny<ParcelModel>()))
                .Returns(hops);
            warehouse.Setup(input => input.GetAllTrucks()).Returns(trucks);

            var result = _business.TrackParcel("12345ABC");

            // Assert
            Assert.IsInstanceOfType(result, typeof(TrackingInformationModel));
        }
    }
}