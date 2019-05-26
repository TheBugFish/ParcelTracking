using SKS.ParcelLogistics.BusinessLogic.Domain;
using SKS.ParcelLogistics.ServiceAgents.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SKS.ParcelLogistics.ServiceAgents
{
    public class MockGeoEncodingAgent : IGeoEncodingAgent
    {
        public GeoPoint EncodeAddress(string address)
        {
            return new GeoPoint((decimal)48.2166205, (decimal)16.3958889); //Riesenrad
        }
    }
}
