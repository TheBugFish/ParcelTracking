using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Rest.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using SKS.ParcelLogistics.BusinessLogic;
using SKS.ParcelLogistics.BusinessLogic.Entities;
using SKS.ParcelLogistics.BusinessLogic.Interfaces;
using SKS.ParcelLogistics.WebSerice.Controllers;
using SKS.ParcelLogistics.WebService.Controllers;
using SKS.ParcelLogistics.WebService.DTOs;

namespace SKS.ParcelLogistics.Tests
{
    [TestClass]
    public class ControllerUnitTest
    {
        private Mock<IBusinessLogic> _bl;
        private ParcelLogisticsController controller;

        public ControllerUnitTest()
        {
            Mapper.Reset();
            Mapper.Initialize(config =>
            {
                config.AddProfile<MappingProfile>();
                config.AddProfile<ToBLProfile>();
            });

            Mapper.AssertConfigurationIsValid();
            _bl = new Mock<IBusinessLogic>();
            controller = new ParcelLogisticsController(_bl.Object);
        }

        [TestMethod]
        public void ParcelPost_Returns_200_When_Valid()
        {
            _bl.Setup(input => input.OnBoardParcel(It.IsNotNull<ParcelModel>())).Returns("ASDA123F");

            var recipient = new Recipient
            {
                FirstName = "Rudi",
                LastName = "Recipient",
                Street = "Poststraße",
                PostalCode = "A-1070",
                City = "Poststadt"
            };
            var parcel = new Parcel
            {
                Recipient = recipient,
                Weight = 1.0f
            };

            var result = controller.ParcelPost(parcel);
            var obj = result as ObjectResult;

            Assert.IsInstanceOfType(result, typeof(ObjectResult));
           Assert.AreEqual(StatusCodes.Status200OK, obj.StatusCode);
        }

        [TestMethod]
        public void ParcelPost_Returns_500_When_Not_Valid()
        {
            _bl.Setup(input => input.OnBoardParcel(It.IsAny<ParcelModel>())).Throws(new BLException("", null));

            var recipient = new Recipient();
            var parcel = new Parcel
            {
                Recipient = recipient,
                Weight = 1.0f
            };

            var result = controller.ParcelPost(parcel);
            var obj = result as ObjectResult;

            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.AreEqual(StatusCodes.Status500InternalServerError, obj.StatusCode);
        }

        [TestMethod]
        public void ParcelTrackingIdGet_Returns_Parcel_When_Valid()
        {
            var recipient = new RecipientModel
            {
                FirstName = "Rudi",
                LastName = "Recipient",
                Street = "Poststraße",
                PostalCode = "A-1070",
                City = "Poststadt"
            };
            var parcel = new ParcelModel
            {
                Recipient = recipient,
                Weight = 1.0f,
                TrackingCode = "12345568"
            };
            _bl.Setup(input => input.GetParcelByCode(It.IsAny<string>())).Returns(parcel);
            _bl.Setup(input => input.TrackParcel(It.IsAny<string>())).Returns(new TrackingInformationModel());

            var result = controller.ParcelTrackingIdGet("1234");
            var obj = result as ObjectResult;
            var val = JsonConvert.DeserializeObject<TrackingInformation>(obj.Value.ToString());

            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.AreEqual(StatusCodes.Status200OK, obj.StatusCode);
            Assert.IsInstanceOfType(val, typeof(TrackingInformation));

        }

        public void ParcelTrackingIdGet_Returns_500_When_Not_Valid()
        {

            _bl.Setup(input => input.GetParcelByCode(It.IsAny<string>())).Throws(new BLException("", null));

            var result = controller.ParcelTrackingIdGet("1234");
            var obj = result as ObjectResult;
            var val = JsonConvert.DeserializeObject<TrackingInformation>(obj.Value.ToString());

        }

        [TestMethod]
        public void ParcelTrackingIdGet_Returns_404_WHen_Parcel_Not_Found()
        {
            var parcel = new ParcelModel();
            parcel = null;
            _bl.Setup(input => input.GetParcelByCode(It.IsAny<string>())).Returns(parcel);
            _bl.Setup(input => input.TrackParcel(It.IsAny<string>())).Returns(new TrackingInformationModel());

            var result = controller.ParcelTrackingIdGet("1234");
            var obj = result as ObjectResult;

            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.AreEqual(StatusCodes.Status404NotFound, obj.StatusCode);
        }

        [TestMethod]
        public void ParcelTrackingIdReportHopCodePost_Returns_200_When_Successful()
        {
            _bl.Setup(input => input.ReportParcelHop(It.IsAny<string>(), It.IsAny<string>()));

            var result = controller.ParcelTrackingIdReportHopCodePost("79AG90XZ", "17AX");
            var obj = result as ObjectResult;

            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.AreEqual(StatusCodes.Status200OK, obj.StatusCode);
        }

        [TestMethod]
        public void ParcelTrackingIdReportHopCodePost_Returns_500_With_Failure()
        {
            _bl.Setup(input => input.ReportParcelHop(It.IsRegex(@"^[A-Z0-9]{8}\b"), It.IsRegex(@"^[A-Z0-9]{4}\b"))).Throws(new BLException("exception", new Exception()));

            var result = controller.ParcelTrackingIdReportHopCodePost("79AG90XZ", "");
            var obj = result as ObjectResult;

            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.AreEqual(StatusCodes.Status500InternalServerError, obj.StatusCode);
        }

        [TestMethod]
        public void WarehouseGet_Returns_Warehouse()
        {
            var warehouse = new WarehouseModel
            {
                Code = "17AX",
                Duration = 12,
                Description = "Root warehouse",

            };
            _bl.Setup(input => input.GetWarehouseHierarchy()).Returns(warehouse);

            var result = controller.WarehouseGet();
            var obj = result as ObjectResult;

            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.AreEqual(StatusCodes.Status200OK, obj.StatusCode);
        }

        [TestMethod]
        public void WarehouseGet_Returns_500_With_Failure()
        {
            var warehouse = new WarehouseModel();
            _bl.Setup(input => input.GetWarehouseHierarchy()).Throws(new BLException("", new Exception()));

            var result = controller.WarehouseGet();
            var obj = result as ObjectResult;

            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.AreEqual(StatusCodes.Status500InternalServerError, obj.StatusCode);
        }

        [TestMethod]
        public void WarehousePost_Returns_200()
        {
            var warehouse = new Warehouse
            {
                Code = "17AX",
                Duration = 12,
                Description = "Root warehouse",

            };
            _bl.Setup(input => input.ImportWarehouses(It.IsAny<WarehouseModel>())).Returns(true);

            var result = controller.WarehousePost(warehouse);
            var obj = result as ObjectResult;

            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.AreEqual(StatusCodes.Status200OK, obj.StatusCode);
        }
        
        
        [TestMethod]
        public void WarehousePost_Returns_500_With_Failure()
        {
            var warehouse = new Warehouse();
            warehouse = null;
            _bl.Setup(input => input.ImportWarehouses(It.IsNotNull<WarehouseModel>())).Returns(true);

            var result = controller.WarehousePost(warehouse);
            var obj = result as ObjectResult;

            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.AreEqual(StatusCodes.Status500InternalServerError, obj.StatusCode);
        }
    }
}
