using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKS.ParcelLogistics.DataAccess.Entities;
using SKS.ParcelLogistics.DataAccess.Interfaces;

namespace SKS.ParcelLogistics.DataAccess.RAM
{
    public class RAMWarehouseRepository : IWarehouseRepository
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(RAMTruckRepository));
        private int _autoinc = 0;
        private readonly List<WarehouseDAO> _warehouses = new List<WarehouseDAO>();
        public void Create(WarehouseDAO t)
        {
            t.Id = _autoinc++;
            _warehouses.Add(t);
        }

        public void Update(WarehouseDAO t)
        {
            throw new NotImplementedException();
        }

        public void Delete(WarehouseDAO t)
        {
            _warehouses.Remove(t);
        }

        public IEnumerable<WarehouseDAO> GetAll()
        {
            return _warehouses;
        }

        public void DeleteAll()
        {
            _autoinc = 0;
            _warehouses.Clear();
        }

        public WarehouseDAO Get(int id)
        {
            return _warehouses.FirstOrDefault(w => w.Id == id);
        }

        public WarehouseDAO GetByCode(string code)
        {
            return _warehouses.FirstOrDefault(w => w.Code == code);
        }

        public List<WarehouseDAO> FindByParent(WarehouseDAO parent)
        {
            List<WarehouseDAO> result;
            if (parent == null)
                return _warehouses.Where(w => w.Parent == null).ToList();
            else
            {
                _logger.Debug(string.Format("Searching for Warehouse with parent '{0}'", parent == null ? "" : parent.Code));

                result = _warehouses.Where(w => (w.Parent != null ? w.Parent.Code == parent.Code : false)).ToList();
            }
            return result;
        }

        public WarehouseDAO ParentOf(string code)
        {
            var result = _warehouses.Where(w => w.Code == code).FirstOrDefault();

            if (result == null || result.Parent == null)
                return null;

            result = result.Parent;
            return result;
        }
    }
}