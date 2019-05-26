using SKS.ParcelLogistics.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SKS.ParcelLogistics.DataAccess.Interfaces
{
    public interface IHopRepository : IRepository<HopArrivalDTO>
    {
        List<HopArrivalDTO> GetPrevHopsForParcel(string trackingCode);
        HopArrivalDTO GetByCodeAndTrackingId(string code, string trackingCode);
    }
}
