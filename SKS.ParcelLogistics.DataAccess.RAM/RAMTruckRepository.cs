using SKS.ParcelLogistics.DataAccess.Entities;
using SKS.ParcelLogistics.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SKS.ParcelLogistics.DataAccess.RAM
{
    public class RAMTruckRepository : ITruckRepository
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(RAMTruckRepository));
        private int _autoinc = 0;
        private readonly List<TruckDAO> _trucks = new List<TruckDAO>();
        public void Create(TruckDAO t)
        {
            t.Id = _autoinc++;
            _trucks.Add(t);
        }

        public void Update(TruckDAO t)
        {
            throw new NotImplementedException();
        }

        public void Delete(TruckDAO t)
        {
            _trucks.Remove(t);
        }

        public IEnumerable<TruckDAO> GetAll()
        {
            return _trucks;
        }

        public void DeleteAll()
        {
            _autoinc = 0;
            _trucks.Clear();
        }

        public TruckDAO Get(int id)
        {
            return _trucks.FirstOrDefault(w => w.Id == id);
        }

        public TruckDAO GetByCode(string code)
        {
            return _trucks.FirstOrDefault(w => w.Code == code);
        }

        public List<TruckDAO> FindByParent(WarehouseDAO parent)
        {
            List<TruckDAO> result;
            if (parent == null)
                return null;

            _logger.Debug("Searching trucks of parent '" + parent.Code + "'");

            result = _trucks.Where(t => t.Parent.Code == parent.Code).ToList();

            return result;
        }

        public string FindParentCode(string code)
        {
            return _trucks.Where(w => w.Code == code).FirstOrDefault().Parent.Code;
        }
    }
}