using System;

namespace SKS.ParcelLogistics.BusinessLogic.Domain
{
    public class GeoPoint
    {
        public decimal Lat { get; set; }
        public decimal Lon { get; set; }

        public GeoPoint(decimal latitude, decimal longitude)
        {
            Lat = latitude;
            Lon = longitude;
        }

        private double ToDeg(decimal radians)
        {
            return (double)radians * (Math.PI / 180);
        }

        public double DistToOtherInMeters(GeoPoint other)
        {
            decimal lat1 = this.Lat;
            decimal lon1 = this.Lon;

            decimal lat2 = other.Lat;
            decimal lon2 = other.Lon;

            return Math.Acos(Math.Sin(ToDeg(lat1)) * Math.Sin(ToDeg(lat2)) + Math.Cos(ToDeg(lat1)) * Math.Cos(ToDeg(lat2)) * Math.Cos(ToDeg(lon2) - ToDeg(lon1))) * 6371000;
        }

        public double DistToOtherInKm(GeoPoint other)
        {
            return DistToOtherInMeters(other) / 1000;
        }
    }
}