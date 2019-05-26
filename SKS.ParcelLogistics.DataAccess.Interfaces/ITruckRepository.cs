using SKS.ParcelLogistics.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SKS.ParcelLogistics.DataAccess.Interfaces
{
    public interface ITruckRepository : IRepository<TruckDTO>
    {
        TruckDTO GetByCode(string code);
        List<TruckDTO> FindByParent(WarehouseDTO warehouse);
        //string FindParentCode(string code);
    }
}
