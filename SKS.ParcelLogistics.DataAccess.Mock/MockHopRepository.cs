using SKS.ParcelLogistics.DataAccess.Entities;
using SKS.ParcelLogistics.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SKS.ParcelLogistics.DataAccess.Mock
{
    public class MockHopRepository : IHopRepository
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(MockHopRepository));
        private int _autoinc = 0;
        private readonly List<HopArrivalDTO> _hops = new List<HopArrivalDTO>();
        public void Create(HopArrivalDTO t)
        {
            t.Id = _autoinc++;
            _hops.Add(t);
        }

        public void Update(HopArrivalDTO t)
        {
            throw new NotImplementedException();
        }

        public void Delete(HopArrivalDTO t)
        {
            _hops.Remove(t);
        }

        public IEnumerable<HopArrivalDTO> GetAll()
        {
            return _hops;
        }

        public void DeleteAll()
        {
            _autoinc = 0;
            _hops.Clear();
        }

        public HopArrivalDTO Get(int id)
        {
            return _hops.FirstOrDefault(w => w.Id == id);
        }

        public List<HopArrivalDTO> GetPrevHopsForParcel(string trackingCode)
        {
            var hops = _hops.Where(h => h.TrackingId == trackingCode).OrderBy(h => h.DateTime).ToList();
            return hops;
        }

        public HopArrivalDTO GetByCodeAndTrackingId(string code, string trackingCode)
        {
            var hop = _hops.Where(h => (h.Code == code && h.TrackingId == trackingCode)).FirstOrDefault();
            return hop;
        }
    }
}