using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SKS.ParcelLogistics.DataAccess.Entities
{
    public class ParcelDTO
    {
        [Key]
        public int Id { get; set; }
        public float Weight { get; set; }
        public RecipientDTO Recipient { get; set; }
        public string TrackingCode { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}