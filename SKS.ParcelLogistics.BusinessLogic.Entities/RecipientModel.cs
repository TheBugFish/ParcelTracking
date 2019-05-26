using FluentValidation;
using FluentValidation.Attributes;

namespace SKS.ParcelLogistics.BusinessLogic.Entities
{
    [Validator(typeof(RecipientValidator))]
    public class RecipientModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
    }

    public class RecipientValidator : AbstractValidator<RecipientModel>
    {
        public RecipientValidator()
        {
            RuleFor(x => x.FirstName).NotNull().Matches(@"[A-ZÄÖÜ][a-zA-Z\s-äöüÄÖÜ]*");
            RuleFor(x => x.LastName).NotNull().Matches(@"[A-ZÄÖÜ][a-zA-Z\s-äöüÄÖÜ]*");
            RuleFor(x => x.City).NotNull().Matches(@"[A-ZÄÖÜ][a-zA-Z\s-äöüÄÖÜ]*");
            RuleFor(x => x.PostalCode).NotNull().Matches(@"^A-[0-9]{4}\b"); //A-0000 A-9999
            RuleFor(x => x.Street).NotNull().Matches(@"[A-ZÄÖÜ][a-zA-Z\s-ß\.äöüÄÖÜ]*[a-zA-Z/0-9]*");
        }
    }
}
