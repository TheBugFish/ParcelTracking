using FluentValidation;
using FluentValidation.Attributes;
using SKS.ParcelLogistics.BusinessLogic.Domain;

namespace SKS.ParcelLogistics.BusinessLogic.Entities
{
    [Validator(typeof(ParcelValidator))]
    public class ParcelModel
    {
        public float Weight { get; set; }
        public RecipientModel Recipient { get; set; }
        public string TrackingCode { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }

    public class ParcelValidator : AbstractValidator<ParcelModel>
    {
        public ParcelValidator()
        {
            RuleFor(x => x.Weight).NotNull().GreaterThan(0f);
            RuleFor(x => x.Recipient).NotNull();
            RuleFor(x => x.Recipient).SetValidator(new RecipientValidator());
        }
    }
}