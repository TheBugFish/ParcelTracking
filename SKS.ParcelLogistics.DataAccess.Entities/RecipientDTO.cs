using System.ComponentModel.DataAnnotations;

namespace SKS.ParcelLogistics.DataAccess.Entities
{
    public class RecipientDTO
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
    }
}