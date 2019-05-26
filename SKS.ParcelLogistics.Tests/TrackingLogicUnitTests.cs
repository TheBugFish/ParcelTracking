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
using Moq;
using SKS.ParcelLogistics.DataAccess.Entities;
using SKS.ParcelLogistics.DataAccess.Interfaces;

namespace SKS.ParcelLogistics.Tests
{
    [TestClass]
    public class TrackingLogicUnitTests
    {
        private TrackingLogic tracking;
        private Mock<IParcelRepository> parcelMock;
        private Mock<IHopRepository> hopMock;
        private Mock<IWarehouseRepository> warehouseMock;
        private Mock<ITruckRepository> truckMock;

        private ITrackingLogic _tl;
        private static readonly ILog log = LogManager.GetLogger(typeof(TrackingLogicUnitTests));

        public TrackingLogicUnitTests()
        {
            Mapper.Reset();
            Mapper.Initialize(config =>
            {
                config.AddProfile<MappingProfile>();
                config.AddProfile<ToBLProfile>();
            });

            Mapper.AssertConfigurationIsValid();
            parcelMock = new Mock<IParcelRepository>();
            hopMock = new Mock<IHopRepository>();
            warehouseMock = new Mock<IWarehouseRepository>();
            truckMock = new Mock<ITruckRepository>();
            tracking = new TrackingLogic(parcelMock.Object, hopMock.Object, warehouseMock.Object, truckMock.Object);

            _tl =
                new TrackingLogic(
                    new MockParcelRepository(),
                    new MockHopRepository(),
                    new MockWarehouseRepository(),
                    new MockTruckRepository());
        }

        [TestMethod]
        public void Get_Parcel_by_code_should_return_Parcel()
        {
            // Arrange
            var recipient = new RecipientDTO()
            {
                FirstName = "Rudi",
                LastName = "Recipient",
                Street = "Poststraße",
                PostalCode = "A-1070",
                City = "Poststadt"
            };
            var parcel = new ParcelDTO() { Recipient = recipient, Weight = 1.0f, TrackingCode = "MOCKCODE" };
            parcelMock.Setup(input => input.GetByTrackingCode(It.IsAny<string>())).Returns(parcel);

            var result = tracking.GetParcelByCode("MOCKCODE");
            // Assert
            Assert.IsInstanceOfType(result, typeof(ParcelModel));
        }

        [TestMethod]
        public void Add_Parcel_should_return_true()
        {
            // Arrange
            RecipientModel recipient = new RecipientModel
            {
                FirstName = "Rudi",
                LastName = "Recipient",
                Street = "Poststraße",
                PostalCode = "A-1070",
                City = "Poststadt"
            };
            ParcelModel parcel = new ParcelModel { Recipient = recipient, Weight = 1.0f };

            parcelMock.Setup(input => input.Create(It.IsAny<ParcelDTO>()));
            parcel.TrackingCode = "MOCKCODE";

            var result = tracking.AddParcel(parcel);
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Report_hop_should_return_true()
        {
            var hop = new HopArrivalModel
            {
                Code = "1234",
                DateTime = DateTime.Now,
                TrackingId = "12345ABC"
            };

            var recipient = new RecipientDTO
            {
                FirstName = "Rudi",
                LastName = "Recipient",
                Street = "Poststraße",
                PostalCode = "A-1070",
                City = "Poststadt"
            };
            var parcel = new ParcelDTO { Recipient = recipient, Weight = 1.0f, TrackingCode = "MOCKCODE" };

            var ware = new WarehouseDTO
            {
                Code = "1234",
                Description = "root warehouse",
                Duration = 12,
                Id = 3
            };

            var truck = new TruckDTO
            {
                Code = "1234",
                Description = "root truck",
                Duration = 42,
                Id = 4
            };
            
            truckMock.Setup(input => input.GetByCode(It.IsAny<string>())).Returns(truck);
            warehouseMock.Setup(input => input.GetByCode(It.IsAny<string>())).Returns(ware);
            parcelMock.Setup(input => input.GetByTrackingCode(It.IsAny<string>())).Returns(parcel);
            var result = tracking.ReportHop(hop);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Generate_tracking_code_should_return_string()
        {
            // Arrange
            // -- Nothing to do

            // Act
            var result = _tl.GenerateTrackingCode();

            // Assert
            Assert.IsInstanceOfType(result, typeof(string));
        }

        [TestMethod]
        public void Get_all_past_hops_should_return_HopArrivalModel()
        {
            // Arrange
            RecipientModel recipient = new RecipientModel
            {
                FirstName = "Rudi",
                LastName = "Recipient",
                Street = "Poststraße",
                PostalCode = "A-1070",
                City = "Poststadt"
            };
            ParcelModel parcel = new ParcelModel { Recipient = recipient, Weight = 1.0f };

           var hops = new List<HopArrivalDTO>();
           hopMock.Setup(input => input.GetPrevHopsForParcel(It.IsAny<string>())).Returns(hops);

            // Act
            var result = tracking.GetAllPastHops(parcel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(IList<HopArrivalModel>));
        }

        [TestMethod]
        public void ValidateParcel_should_return_true()
        {
            // Arrange
            RecipientModel recipient = new RecipientModel
            {
                FirstName = "Rudi",
                LastName = "Recipient",
                Street = "Poststraße",
                PostalCode = "A-1070",
                City = "Poststadt"
            };
            ParcelModel parcel = new ParcelModel { Recipient = recipient, Weight = 1.0f };
            parcel.TrackingCode = "MOCKCODE";
            // Act
            var result = _tl.ValidateParcel(parcel);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateHop_should_return_true()
        {
            // Arrange
            HopArrivalModel hop = new HopArrivalModel() { Code = "MOCK", TrackingId = "MOCKCODE", DateTime = DateTime.Now };

            // Act
            var result = _tl.ValidateHop(hop);

            // Assert
            Assert.IsTrue(result);
        }

      
    }
}