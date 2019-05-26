using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SKS.ParcelLogistics.BusinessLogic.Entities;
using SKS.ParcelLogistics.WebService.DTOs;

namespace SKS.ParcelLogistics.Tests
{
    [TestClass]
    public class ValidationTest
    {
        private ParcelValidator _parcelV;
        private RecipientValidator _recipientV;
        private TruckValidator _truckV;
        private HopArrivalValidator _hopArrivalV;
        private TrackingInformationValidator _trackingInfoV;
        private ErrorValidator _errorV;

        public ValidationTest()
        {
            _parcelV = new ParcelValidator();
            _recipientV = new RecipientValidator();
            _truckV = new TruckValidator();
            _hopArrivalV = new HopArrivalValidator();
            _trackingInfoV = new TrackingInformationValidator();
            _errorV = new ErrorValidator();
        }

        [TestMethod]
        public void ShouldHaveValidationErrorWhenWeightIsLTZero()
        {
            ValidationResult validationResults = _parcelV.Validate(new ParcelModel { Weight = -1f, Recipient = new RecipientModel { City = "A", FirstName = "B", LastName = "C", PostalCode = "A-1234", Street = "E" } });
            Assert.IsTrue(validationResults.Errors.Count == 1);
        }

        [TestMethod]
        public void ShouldHaveValidationErrorWhenRecipientNull()
        {
            ValidationResult validationResults = _parcelV.Validate(new ParcelModel { Weight = 1f, Recipient = null });
            Assert.IsTrue(validationResults.Errors.Count == 1);
        }

        [TestMethod]
        public void ShouldHaveValidationErrorsWhenRecipientHasAnyNulls()
        {
            ValidationResult validationResults = _recipientV.Validate(new RecipientModel { City = null, FirstName = null, LastName = null, Street = null, PostalCode = null });
            Assert.IsTrue(validationResults.Errors.Count == 5);
        }

        [TestMethod]
        public void ShouldHaveValidationErrorsWhenTruckHasAnyNulls()
        {
            ValidationResult validationResults = _truckV.Validate(new TruckModel { Code = null, NumberPlate = null, Duration = 0.1M, Latitude = 0, Longitude = 0, Radius = 0.1M });
            Assert.IsTrue(validationResults.Errors.Count == 2);
        }

        [TestMethod]
        public void ShouldHaveValidationErrorWhenTruckRadiusLTZero()
        {
            ValidationResult validationResults = _truckV.Validate(new TruckModel { Code = "XT12", NumberPlate = "b", Duration = 1, Latitude = 1, Longitude = 1, Radius = -1 });
            Assert.IsTrue(validationResults.Errors.Count == 1);
        }

        [TestMethod]
        public void ShouldHaveValidationErrorWhenTruckDurationLTZero()
        {
            ValidationResult validationResults = _truckV.Validate(new TruckModel { Code = "XT12", NumberPlate = "b", Duration = -1, Latitude = 1, Longitude = 1, Radius = 1 });

            Assert.IsTrue(validationResults.Errors.Count == 1);
        }

        [TestMethod]
        public void ShouldHaveValidationErrorsWhenHopArrivalHasAnyNulls()
        {
            ValidationResult validationResults = _hopArrivalV.Validate(new HopArrivalModel { TrackingId = null, Code = null, DateTime = null });
            Assert.IsTrue(validationResults.Errors.Count == 3);
        }

        [TestMethod]
        public void ShouldHaveValidationErrorsWhenTrackingInformationHasAnyNulls()
        {
            ValidationResult validationResults = _trackingInfoV.Validate(new TrackingInformationModel() {FutureHops = null, VisitedHops = null });
            Assert.IsTrue(validationResults.Errors.Count == 2);
        }

        [TestMethod]
        public void ShouldHaveValidationErrorsWhenErrorMessageIsNull()
        {
            ValidationResult validationResults = _errorV.Validate(new Error(null));
            Assert.IsTrue(validationResults.Errors.Count == 1);
        }
    }
}