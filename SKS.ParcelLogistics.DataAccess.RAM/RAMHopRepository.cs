using SKS.ParcelLogistics.DataAccess.Entities;
using SKS.ParcelLogistics.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SKS.ParcelLogistics.DataAccess.RAM
{
    public class RAMHopRepository : IHopRepository
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(RAMTruckRepository));
        private int _autoinc = 0;
        private readonly List<HopArrivalDAO> _hops = new List<HopArrivalDAO>();
        public void Create(HopArrivalDAO t)
        {
            t.Id = _autoinc++;
            _hops.Add(t);
        }

        public void Update(HopArrivalDAO t)
        {
            throw new NotImplementedException();
        }

        public void Delete(HopArrivalDAO t)
        {
            _hops.Remove(t);
        }

        public IEnumerable<HopArrivalDAO> GetAll()
        {
            return _hops;
        }

        public void DeleteAll()
        {
            _autoinc = 0;
            _hops.Clear();
        }

        public HopArrivalDAO Get(int id)
        {
            return _hops.FirstOrDefault(w => w.Id == id);
        }

        public List<HopArrivalDAO> GetPrevHopsForParcel(string trackingCode)
        {
            var hops = _hops.Where(h => h.TrackingId == trackingCode).OrderBy(h => h.DateTime).ToList();
            return hops;
        }

        public HopArrivalDAO GetByCodeAndTrackingId(string code, string trackingCode)
        {
            var hop = _hops.Where(h => (h.Code == code && h.TrackingId == trackingCode)).FirstOrDefault();
            return hop;
        }
    }
}