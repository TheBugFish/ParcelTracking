using log4net;
using Microsoft.EntityFrameworkCore;
using SKS.ParcelLogistics.DataAccess.Entities;
using SKS.ParcelLogistics.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SKS.ParcelLogistics.DataAccess.SQL
{
    public class SQLParcelRepository : IParcelRepository
    {
        private readonly ParcelLogisticsContext _dbContext;
        private ILog _logger = LogManager.GetLogger(typeof(SQLParcelRepository));

        public SQLParcelRepository(DbContext dbContext)
        {
            _dbContext = (ParcelLogisticsContext)dbContext;
            _dbContext.Database.EnsureCreated();
        }

        public void Create(ParcelDTO parcel)
        {
            try
            {
                _dbContext.Parcels.Add(parcel);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DalException("DAL error creating parcel: " + ex.Message + ", Inner: " + ex.InnerException.Message, ex);
            }
        }

        public void Update(ParcelDTO parcel)
        {
            try
            {
                _dbContext.Parcels.Update(parcel);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DalException("DAL error updating parcel", ex);
            }

        }

        public ParcelDTO GetByTrackingCode(string d)
        {
            try
            {
                return _dbContext.Parcels.Include(b => b.Recipient).Where(b => b.TrackingCode == d).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new DalException("DAL error getting parcel by tracking code: " + ex.Message + ", Inner: " + ex.InnerException.Message, ex);
            }

        }

        public void Delete(ParcelDTO parcel)
        {
            try
            {
                _dbContext.Parcels.Remove(parcel);
                _dbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new DalException("DAL error deleting parcel", ex);
            }

        }

        public IEnumerable<ParcelDTO> GetAll()
        {
            try
            {
                return _dbContext.Parcels;
            }
            catch (Exception ex)
            {
                throw new DalException("DAL error getting all parcels", ex);
            }

        }

        public void DeleteAll()
        {
            try
            {
                _dbContext.RemoveRange(_dbContext.Parcels);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DalException("DAL error deleting all parcels", ex);
            }

        }

        public ParcelDTO Get(int id)
        {
            try
            {
                return _dbContext.Parcels.Where(b => b.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new DalException("DAL error getting parcel by id", ex);
            }
        }
    }
}
