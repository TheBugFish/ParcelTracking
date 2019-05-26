using SKS.ParcelLogistics.DataAccess.Entities;
using SKS.ParcelLogistics.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SKS.ParcelLogistics.DataAccess.RAM
{
    public class RAMParcelRepository : IParcelRepository
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(RAMParcelRepository));
        private readonly List<ParcelDAO> _parcels = new List<ParcelDAO>();
        private int _autoinc = 0;
        public void Create(ParcelDAO t)
        {
            t.Id = _autoinc++;
            _logger.Info("MockParcelRepository.Create (Parcel with tracking code): " + t.TrackingCode);
            _parcels.Add(t);
        }

        public void Update(ParcelDAO t)
        {
            throw new NotImplementedException();
        }

        public ParcelDAO Get(int d)
        {
            return _parcels.FirstOrDefault(p => p.Id == d);
        }

        public void Delete(ParcelDAO t)
        {
            _parcels.Remove(t);
        }

        public IEnumerable<ParcelDAO> GetAll()
        {
            return _parcels;
        }

        public ParcelDAO GetByTrackingCode(string code)
        {
            _logger.Info("MockParcelRepository.GetByTrackingCode: " + code);
            return _parcels.FirstOrDefault(p => p.TrackingCode == code);
        }

        public void DeleteAll()
        {
            _autoinc = 0;
            _parcels.Clear();
        }
    }
}