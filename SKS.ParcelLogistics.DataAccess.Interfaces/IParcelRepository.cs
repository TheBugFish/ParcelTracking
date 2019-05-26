using SKS.ParcelLogistics.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SKS.ParcelLogistics.DataAccess.Interfaces
{
    public interface IParcelRepository : IRepository<ParcelDTO>
    {
        ParcelDTO GetByTrackingCode(string code);
    }
}
