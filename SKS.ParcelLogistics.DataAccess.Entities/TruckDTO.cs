using System.ComponentModel.DataAnnotations;

namespace SKS.ParcelLogistics.DataAccess.Entities
{
    public class TruckDTO : WarehouseDTO
    {
        public string NumberPlate { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Radius { get; set; }
    }
}