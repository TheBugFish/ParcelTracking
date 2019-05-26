using SKS.ParcelLogistics.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SKS.ParcelLogistics.DataAccess.Interfaces
{
    public interface IWarehouseRepository : IRepository<WarehouseDTO>
    {
        WarehouseDTO GetByCode(string code);
        List<WarehouseDTO> FindByParent(WarehouseDTO parent);
        WarehouseDTO ParentOf(string code);
    }
}
