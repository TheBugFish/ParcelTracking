using log4net;
using Microsoft.EntityFrameworkCore;
using SKS.ParcelLogistics.DataAccess.Entities;
using SKS.ParcelLogistics.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SKS.ParcelLogistics.DataAccess.SQL
{
    public class SQLWarehouseRepository : IWarehouseRepository
    {
        private readonly ParcelLogisticsContext _dbContext;
        private ILog _logger = LogManager.GetLogger(typeof(SQLWarehouseRepository));

        public SQLWarehouseRepository(DbContext dbContext)
        {
            _dbContext = (ParcelLogisticsContext)dbContext;
            _dbContext.Database.EnsureCreated();

            /*if (_dbContext.Warehouses.Any()) return;

            var warehouses = new WarehouseDAO[]
            {
                new WarehouseDAO{Code="TEST",Description="My Warehouse", Duration=0.5m, Parent=null},
          
            };
            foreach (WarehouseDAO w in warehouses)
            {
                _dbContext.Warehouses.Add(w);
            }
            _dbContext.SaveChanges();*/
        }

        public void Create(WarehouseDTO warehouse)
        {
            try
            {
                _dbContext.Warehouses.Add(warehouse);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DalException("DAL error creating warehouse", ex);
            }
        }

        public void Update(WarehouseDTO warehouse)
        {
            try
            {
                _dbContext.Warehouses.Update(warehouse);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DalException("DAL error updating warehouse", ex);
            }
        }

        public void Delete(WarehouseDTO warehouse)
        {
            try
            {
                _dbContext.Warehouses.Remove(warehouse);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DalException("DAL error deleting warehouse", ex);
            }
        }

        public IEnumerable<WarehouseDTO> GetAll()
        {
            try
            {
                return _dbContext.Warehouses;
            }
            catch (Exception ex)
            {
                throw new DalException("DAL error getting warehouses", ex);
            }
        }

        public void DeleteAll()
        {
            try
            {
                _dbContext.Warehouses.RemoveRange(GetAll());
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DalException("DAL error deleting all warehouses", ex);
            }
        }

        public WarehouseDTO Get(int id)
        {
            try
            {
                return _dbContext.Warehouses.Where(w => w.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new DalException("DAL error getting warehouse by id", ex);
            }
        }

        public WarehouseDTO GetByCode(string code)
        {
            try
            {
                return _dbContext.Warehouses.Where(w => w.Code == code).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new DalException("DAL error getting warehouse by code", ex);
            }
        }

        public List<WarehouseDTO> FindByParent(WarehouseDTO parent)
        {
            try
            {
                return _dbContext.Warehouses.Where(w => w.Parent.Code == parent.Code).ToList();
            }
            catch (Exception ex)
            {
                throw new DalException("DAL error finding warehouse by parent", ex);
            }
        }

        public WarehouseDTO ParentOf(string code)
        {
            try
            {
                return _dbContext.Warehouses.Where(w => w.Parent.Code == code).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new DalException("DAL error finding parent of warehouse", ex);
            }
        }
    }
}