using log4net;
using Microsoft.EntityFrameworkCore;
using SKS.ParcelLogistics.DataAccess.Entities;
using SKS.ParcelLogistics.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKS.ParcelLogistics.DataAccess.SQL
{
    public class SQLTruckRepository : ITruckRepository
    {
        private readonly ParcelLogisticsContext _dbContext;
        private ILog _logger = LogManager.GetLogger(typeof(SQLTruckRepository));

        public SQLTruckRepository(DbContext dbContext)
        {
            _dbContext = (ParcelLogisticsContext)dbContext;
            _dbContext.Database.EnsureCreated();
        }

        public void Create(TruckDTO t)
        {
            try
            {
                _dbContext.Trucks.Add(t);
                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new DalException("DAL error creating truck", ex);
            }
           
        }

        public void Delete(TruckDTO t)
        {
            try
            {
                _dbContext.Trucks.Remove(t);
                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new DalException("DAL error deleting truck",ex);
            }
           
        }

        public void DeleteAll()
        {
            try
            {
                _dbContext.RemoveRange(_dbContext.Trucks);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DalException("DAL error deleting all Trucks", ex);
            }
        }

        public List<TruckDTO> FindByParent(WarehouseDTO warehouse)
        {
            try
            {
                return _dbContext.Trucks.Where(t => t.Parent == warehouse).ToList();
            }
            catch(Exception ex)
            {
                throw new DalException("DAL error finding trucks by parent", ex);
            }
            
        }

       /* public string FindParentCode(string code)
        {
            throw new NotImplementedException();
        }*/

        public TruckDTO Get(int id)
        {
            try
            {
                return _dbContext.Trucks.Include(t => t.Parent).Where(t => t.Id == id).FirstOrDefault();
            }
            catch(Exception ex)
            {
                throw new DalException("DAL error getting truck", ex);
            }        
        }

        public IEnumerable<TruckDTO> GetAll()
        {
            try
            {
                return _dbContext.Trucks.Include(t => t.Parent);
            }
            catch(Exception ex)
            {
                throw new DalException("DAL error getting all trucks", ex);
            }       
        }

        public TruckDTO GetByCode(string code)
        {
            try
            {
                return _dbContext.Trucks.Include(t => t.Parent).Where(t => t.Code == code).FirstOrDefault();
            }
            catch(Exception ex)
            {
                throw new DalException("DAL error getting truck by code", ex);
            }
          
        }

        public void Update(TruckDTO t)
        {
            try
            {
                _dbContext.Trucks.Update(t);
                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new DalException("DAL error updating truck", ex);
            }
           
        }
    }
}
