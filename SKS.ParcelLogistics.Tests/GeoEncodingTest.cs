using AutoMapper;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SKS.ParcelLogistics.BusinessLogic;
using SKS.ParcelLogistics.BusinessLogic.Domain;
using SKS.ParcelLogistics.ServiceAgents;
using SKS.ParcelLogistics.ServiceAgents.Interfaces;

namespace SKS.ParcelLogistics.Tests
{
    [TestClass]
    public class GeoEncodingTest
    {
        private IGeoEncodingAgent _agent;
        private ILog _logger = LogManager.GetLogger(typeof(GeoEncodingTest));

        public GeoEncodingTest()
        {
            _agent = new GoogleGeoEncodingAgent();

            Mapper.Reset();
            Mapper.Initialize(config =>
            {
                config.AddProfile<MappingProfile>();
                config.AddProfile<ToBLProfile>();
            });
            Mapper.AssertConfigurationIsValid();
            // -- Configuration fails?
     
        }
        [TestMethod]
        public void DistanceUtilityTest()
        {
            GeoPoint g1 = new GeoPoint((decimal)48.2166205,(decimal)16.3958889); //Riesenrad
            GeoPoint g2 = new GeoPoint((decimal)48.2392831, (decimal)16.3773241); //Technikum

            double dist = g1.DistToOtherInMeters(g2);

            _logger.Info(string.Format("DistanceTest: {0}", dist));

            Assert.IsTrue(dist < 3000 && dist > 2000);
        }

        [TestMethod]
        public void GoogleAPITest()
        {

            GeoPoint g1 = new GeoPoint((decimal)48.2392831, (decimal)16.3773241); // Technikum
            GeoPoint g2 = _agent.EncodeAddress("Technikum Wien");

            Assert.IsTrue(g1.DistToOtherInMeters(g2) < 100);
        }
    }
}
