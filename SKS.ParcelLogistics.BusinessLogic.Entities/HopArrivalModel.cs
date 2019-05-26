using FluentValidation;
using FluentValidation.Attributes;
using System;

namespace SKS.ParcelLogistics.BusinessLogic.Entities
{
    [Validator(typeof(HopArrivalValidator))]
    public class HopArrivalModel
    {
        public string TrackingId { get; set; }
        public string Code { get; set; }
        public DateTime? DateTime { get; set; }
    }

    public class HopArrivalValidator : AbstractValidator<HopArrivalModel>
    {
        public HopArrivalValidator()
        {
            RuleFor(x => x.Code).NotNull().Matches(@"^[A-Z0-9]{4}\b");
            RuleFor(x => x.TrackingId).NotNull().Matches(@"^[A-Z0-9]{8}\b");
            RuleFor(x => x.DateTime).NotNull();
        }
    }
}
