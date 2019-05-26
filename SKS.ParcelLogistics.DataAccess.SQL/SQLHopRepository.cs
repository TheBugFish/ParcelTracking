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
    public class SQLHopRepository : IHopRepository
    {
        private readonly ParcelLogisticsContext _dbContext;
        private ILog _logger = LogManager.GetLogger(typeof(SQLHopRepository));

        public SQLHopRepository(DbContext dbContext)
        {
            _dbContext = (ParcelLogisticsContext)dbContext;
            _dbContext.Database.EnsureCreated();
        }

        public void Create(HopArrivalDTO t)
        {
            try
            {
                _dbContext.HopArrivals.Add(t);
                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new DalException("DAL error creating hop", ex);
            }
            
        }

        public void Delete(HopArrivalDTO t)
        {
            try
            {
                _dbContext.HopArrivals.Remove(t);
                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new DalException("DAL error deleting hop", ex);
            }
          
        }

        public void DeleteAll()
        {
            try
            {
                _dbContext.RemoveRange(_dbContext.HopArrivals);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("DAL error deleting all hops", ex);
            }

        }
        public HopArrivalDTO Get(int id)
        {
            try
            {
                return _dbContext.HopArrivals.Where(h => h.Id == id).FirstOrDefault();
            }
            catch(Exception ex)
            {
                throw new DalException("DAL error getting hop", ex);
            }
            
        }

        public IEnumerable<HopArrivalDTO> GetAll()
        {
            try
            {
                return _dbContext.HopArrivals;
            }
            catch(Exception ex)
            {
                throw new DalException("DAL error getting all hops", ex);
            }        
        }

        public HopArrivalDTO GetByCodeAndTrackingId(string code, string trackingCode)
        {
            try
            {
                return _dbContext.HopArrivals.Where(h => h.Code == code && h.TrackingId == trackingCode).FirstOrDefault();
            }
            catch(Exception ex)
            {
                throw new DalException("DAL error getting hop by code + trackID", ex);
            }
           
        }

        public List<HopArrivalDTO> GetPrevHopsForParcel(string trackingCode)
        {
            try
            {
                return _dbContext.HopArrivals.Where(h => h.TrackingId == trackingCode).ToList();
            }
            catch(Exception ex)
            {
                throw new DalException("DAL error getting previous hops", ex);
            }
           
        }

        public void Update(HopArrivalDTO t)
        {
            try
            {
                _dbContext.HopArrivals.Update(t);
                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new DalException("DAL error updating hop", ex);
            }

        }
    }
}
