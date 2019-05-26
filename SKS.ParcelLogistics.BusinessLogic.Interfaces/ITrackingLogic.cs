using System;
using System.Collections.Generic;
using System.Text;
using SKS.ParcelLogistics.BusinessLogic.Entities;

namespace SKS.ParcelLogistics.BusinessLogic.Interfaces
{
    public interface ITrackingLogic
    {
        ParcelModel GetParcelByCode(string trackingID);
        bool AddParcel(ParcelModel parcel);
        bool ReportHop(HopArrivalModel hop);
        string GenerateTrackingCode();

        bool ValidateParcel(ParcelModel parcel);
        bool ValidateHop(HopArrivalModel hop);
        IList<HopArrivalModel> GetAllPastHops(ParcelModel parcel);
    }
}
