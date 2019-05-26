using System;
using System.Collections.Generic;
using System.Text;
using SKS.ParcelLogistics.BusinessLogic.Entities;

namespace SKS.ParcelLogistics.BusinessLogic.Interfaces
{
    public interface IWarehouseLogic
    {
        bool AddWarehouseRoot(WarehouseModel warehouse);
        WarehouseModel GetWarehouseByCode(string code);
        WarehouseModel GetWarehouseHierarchy();
        bool DeleteWarehouseTree();
        bool ValidateWarehouseModel(WarehouseModel warehouse);
        bool ValidateTruckModel(TruckModel warehouse);

        TruckModel GetTruckByCode(string code);
        IList<TruckModel> GetAllTrucks();
        IList<HopArrivalModel> GetAllFutureHops(HopArrivalModel latestHop, TruckModel closestTruck, ParcelModel parcel);
    }
}
