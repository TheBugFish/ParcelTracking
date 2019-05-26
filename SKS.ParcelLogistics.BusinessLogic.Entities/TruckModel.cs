using FluentValidation;
using FluentValidation.Attributes;

namespace SKS.ParcelLogistics.BusinessLogic.Entities
{
    [Validator(typeof(TruckValidator))]
    public class TruckModel
    {
        public string Code { get; set; }
        public string NumberPlate { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Radius { get; set; }
        public decimal Duration { get; set; }

        public WarehouseModel Parent { get; set; }
    }

    public class TruckValidator : AbstractValidator<TruckModel>
    {
        public TruckValidator()
        {
            RuleFor(x => x.Code).NotNull().Matches(@"^[A-Z0-9]{4}\b");
            RuleFor(x => x.Duration).NotNull().GreaterThan(0);
            RuleFor(x => x.Latitude).NotNull();
            RuleFor(x => x.Longitude).NotNull();
            RuleFor(x => x.NumberPlate).NotNull();
            RuleFor(x => x.Radius).NotNull().GreaterThan(0);
        }
    }
}