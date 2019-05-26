using SKS.ParcelLogistics.BusinessLogic.Domain;

namespace SKS.ParcelLogistics.ServiceAgents.Interfaces
{
    public interface IGeoEncodingAgent
    {
        GeoPoint EncodeAddress(string address);
    }
}