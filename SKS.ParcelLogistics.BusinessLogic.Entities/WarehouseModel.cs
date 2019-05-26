using FluentValidation;
using FluentValidation.Attributes;
using System.Collections.Generic;

namespace SKS.ParcelLogistics.BusinessLogic.Entities
{
    [Validator(typeof(WarehouseValidator))]
    public class WarehouseModel
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Duration { get; set; }
        public List<WarehouseModel> NextHops { get; set; }
        public List<TruckModel> Trucks { get; set; }

        public WarehouseModel Parent { get; set; }
    }

    public class WarehouseValidator : AbstractValidator<WarehouseModel>
    {
        public WarehouseValidator()
        {
            RuleFor(x => x.Code).NotNull().Matches(@"[A-Z0-9]{4}\b");
            RuleFor(x => x.Description).NotNull();
            RuleFor(x => x.Duration).NotNull().GreaterThan(0);
            RuleForEach(x => x.NextHops).SetValidator(this);
            RuleForEach(x => x.Trucks).SetValidator(new TruckValidator());
        }
    }
}