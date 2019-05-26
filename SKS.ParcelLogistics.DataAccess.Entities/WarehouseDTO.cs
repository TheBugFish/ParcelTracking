using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SKS.ParcelLogistics.DataAccess.Entities
{
    public class WarehouseDTO
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Duration { get; set; }
        public List<WarehouseDTO> NextHops { get; set; }
        public List<TruckDTO> Trucks { get; set; }

        public WarehouseDTO Parent { get; set; }
    }
}