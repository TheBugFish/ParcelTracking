using SKS.ParcelLogistics.BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SKS.ParcelLogistics.BusinessLogic.Interfaces
{
    public interface IBusinessLogic
    {
        bool AddParcel(ParcelModel parcel);
        ParcelModel GetParcelByCode(string trackingCode);
        WarehouseModel GetWarehouseHierarchy();
        string GetNewTrackingID();
        bool DeleteAllWarehouses();

        bool ReportParcelHop(string parcelCode, string code);
        string OnBoardParcel(ParcelModel parcel);
        bool ImportWarehouses(WarehouseModel warehouseRoot);
        TrackingInformationModel TrackParcel(string parcelCode);
    }
}
