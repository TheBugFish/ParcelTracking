using FluentValidation;
using FluentValidation.Attributes;
using System.Collections.Generic;

namespace SKS.ParcelLogistics.BusinessLogic.Entities
{
    [Validator(typeof(TrackingInformationValidator))]
    public class TrackingInformationModel
    {
        public StateEnum State;
        public List<HopArrivalModel> VisitedHops;
        public List<HopArrivalModel> FutureHops;
    }

    public enum StateEnum
    {
        InTransportEnum,
        InTruckDeliveryEnum,
        DeliveredEnum
    }

    public class TrackingInformationValidator : AbstractValidator<TrackingInformationModel>
    {
        public TrackingInformationValidator()
        {
            RuleFor(x => x.VisitedHops).NotNull();
            RuleFor(x => x.FutureHops).NotNull();
        }
    }
}