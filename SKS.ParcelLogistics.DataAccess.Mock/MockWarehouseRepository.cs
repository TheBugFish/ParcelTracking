using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKS.ParcelLogistics.DataAccess.Entities;
using SKS.ParcelLogistics.DataAccess.Interfaces;

namespace SKS.ParcelLogistics.DataAccess.Mock
{
    public class MockWarehouseRepository : IWarehouseRepository
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(MockWarehouseRepository));
        private int _autoinc = 0;
        private readonly List<WarehouseDTO> _warehouses = new List<WarehouseDTO>();
        private readonly MockTruckRepository _truckRepo = new MockTruckRepository();

        public void Create(WarehouseDTO w)
        {
            w.Id = _autoinc++;
            _warehouses.Add(w);
            foreach (var tr in w.Trucks)
            {
                _truckRepo.Create(tr);
            }

        }

        public void Update(WarehouseDTO w)
        {
            throw new NotImplementedException();
        }

        public void Delete(WarehouseDTO w)
        {
            _warehouses.Remove(w);
        }

        public IEnumerable<WarehouseDTO> GetAll()
        {
            return _warehouses;
        }

        public void DeleteAll()
        {
            _autoinc = 0;
            _warehouses.Clear();
        }

        public WarehouseDTO Get(int id)
        {
            return _warehouses.FirstOrDefault(w => w.Id == id);
        }

        public WarehouseDTO GetByCode(string code)
        {
            var wh = _warehouses.FirstOrDefault(w => w.Code == code);
            if(wh == null)
            {
                wh = _truckRepo.GetByCode(code);
            }
            return wh;
        }

        public List<WarehouseDTO> FindByParent(WarehouseDTO parent)
        {
            List<WarehouseDTO> result;
            if (parent == null)
                return _warehouses.Where(w => w.Parent == null).ToList();
            else
            {
                _logger.Debug(string.Format("Searching for Warehouse with parent '{0}'", parent == null ? "" : parent.Code));

                result = _warehouses.Where(w => (w.Parent != null ? w.Parent.Code == parent.Code : false)).ToList();
            }
            return result;
        }

        public WarehouseDTO ParentOf(string code)
        {
            var result = _warehouses.Where(w => w.Code == code).FirstOrDefault();

            if (result == null || result.Parent == null)
                return null;

            var result2 = _warehouses.Where(w => w.Code == result.Parent.Code).FirstOrDefault();
            
            return result2;
        }
    }
}