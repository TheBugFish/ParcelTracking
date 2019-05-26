using SKS.ParcelLogistics.WebService.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SKS.ParcelLogistics.Tests
{
    [TestClass]
    public class IntegrationTest
    {
        private HttpClient _client;
        

        public IntegrationTest()
        {
            _client = new HttpClient { BaseAddress = new Uri("http://localhost:2621/") };
        }
        
        [TestMethod]
        public async Task ParcelPostIntegrationTest()
        {
            Recipient recipient = new Recipient("Rudi", "Recipient", "PostStraße", "A-9999", "PostStadt");
            Parcel p = new Parcel(10, recipient);

            var response = await _client.PostAsync("/api/parcel", new StringContent(p.ToJson(), Encoding.UTF8, "application/json") );

            response.EnsureSuccessStatusCode();
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
           
           // Assert.AreEqual(p, JsonConvert.DeserializeObject<Parcel> response.Content);

        }
        [TestMethod]
        public async Task ParcelGetIntegrationTest()
        {
            var response = await _client.GetAsync("/api/parcel/0");

            response.EnsureSuccessStatusCode();
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task PostWarehouseIntegrationTest()
        {
            List<Truck> Trucklist = new List<Truck>();
            List<Warehouse> Hoplist = new List<Warehouse>();
            Warehouse w = new Warehouse() { Code="AAAA", Description="Testhaus",Duration=1,Trucks = Trucklist, NextHops = Hoplist};

            var response = await _client.PostAsync("/api/warehouse", new StringContent(w.ToJson(), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        } 

        [TestMethod]
        public async Task GetWarehouseIntegrationtest()
        {
            var response = await _client.GetAsync("/api/warehouse");

            response.EnsureSuccessStatusCode();
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task ReportHopIntegrationTest()
        {
            var response = await _client.PostAsync("/api/parcel/{trackingId}/reportHop/{code}", new StringContent(""));

            response.EnsureSuccessStatusCode();
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }
    }
}
