using SKS.ParcelLogistics.DataAccess.Entities;
using SKS.ParcelLogistics.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SKS.ParcelLogistics.DataAccess.Mock
{
    public class MockParcelRepository : IParcelRepository
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(MockParcelRepository));
        private readonly List<ParcelDTO> _parcels = new List<ParcelDTO>();
        private int _autoinc = 0;
        public void Create(ParcelDTO t)
        {
            t.Id = _autoinc++;
            _logger.Info("MockParcelRepository.Create (Parcel with tracking code): " + t.TrackingCode);
            _parcels.Add(t);
        }

        public void Update(ParcelDTO t)
        {
            throw new NotImplementedException();
        }

        public ParcelDTO Get(int d)
        {
            return _parcels.FirstOrDefault(p => p.Id == d);
        }

        public void Delete(ParcelDTO t)
        {
            _parcels.Remove(t);
        }

        public IEnumerable<ParcelDTO> GetAll()
        {
            return _parcels;
        }

        public ParcelDTO GetByTrackingCode(string code)
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