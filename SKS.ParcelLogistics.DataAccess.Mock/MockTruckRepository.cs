using SKS.ParcelLogistics.DataAccess.Entities;
using SKS.ParcelLogistics.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SKS.ParcelLogistics.DataAccess.Mock
{
    public class MockTruckRepository : ITruckRepository
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(MockTruckRepository));
        private int _autoinc = 0;
        private readonly List<TruckDTO> _trucks = new List<TruckDTO>();
        public void Create(TruckDTO t)
        {
            t.Id = _autoinc++;
            _trucks.Add(t);
        }

        public void Update(TruckDTO t)
        {
            throw new NotImplementedException();
        }

        public void Delete(TruckDTO t)
        {
            _trucks.Remove(t);
        }

        public IEnumerable<TruckDTO> GetAll()
        {
            return _trucks;
        }

        public void DeleteAll()
        {
            _autoinc = 0;
            _trucks.Clear();
        }

        public TruckDTO Get(int id)
        {
            return _trucks.FirstOrDefault(w => w.Id == id);
        }

        public TruckDTO GetByCode(string code)
        {
            return _trucks.FirstOrDefault(w => w.Code == code);
        }

        public List<TruckDTO> FindByParent(WarehouseDTO parent)
        {
            List<TruckDTO> result;
            if (parent == null)
                return null;

            _logger.Debug("Searching trucks of parent '" + parent.Code + "'");

            result = _trucks.Where(t => t.Parent.Code == parent.Code).ToList();

            return result;
        }

       /* public string FindParentCode(string code)
        {
            return _trucks.Where(w => w.Code == code).FirstOrDefault().Parent.Code;
        }*/
    }
}