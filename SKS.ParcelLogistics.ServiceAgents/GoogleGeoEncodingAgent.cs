using SKS.ParcelLogistics.BusinessLogic.Domain;
using SKS.ParcelLogistics.ServiceAgents.Interfaces;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml;

namespace SKS.ParcelLogistics.ServiceAgents
{
    public class GoogleGeoEncodingAgent : IGeoEncodingAgent
    {
        private static IFormatProvider cult = new CultureInfo("en-US");

        public GeoPoint EncodeAddress(string address)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://maps.googleapis.com/maps/api/geocode/xml?address=" + address);
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string responseText = reader.ReadToEnd();

                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(responseText);

                    XmlNode statusNode = xml.SelectSingleNode("/GeocodeResponse/status");
                    if (statusNode.InnerText != "OK") throw new Exception("GoogleGeoEncodingAgent didn't get OK from Google - status: " + statusNode.InnerText);

                    XmlNode latNode = xml.SelectSingleNode("/GeocodeResponse/result/geometry/location/lat");
                    XmlNode lonNode = xml.SelectSingleNode("/GeocodeResponse/result/geometry/location/lng");

                    decimal lat = Convert.ToDecimal(latNode.InnerText, cult);
                    decimal lon = Convert.ToDecimal(lonNode.InnerText, cult);

                    return new GeoPoint(lat, lon);

                case HttpStatusCode.NotFound:
                    throw new Exception("Webrequest error 404 in GoogleGeoEncodingAgent.");
                default:
                    throw new Exception("Unexpected status code in GoogleGeoEncodingAgent.");
            }
        }
    }
}

