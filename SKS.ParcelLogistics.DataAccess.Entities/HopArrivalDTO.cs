using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SKS.ParcelLogistics.DataAccess.Entities
{
    public class HopArrivalDTO
    {
        [Key]
        public int Id { get; set; }
        public string TrackingId { get; set; }
        public string Code { get; set; }
        public DateTime? DateTime { get; set; }
    }
}