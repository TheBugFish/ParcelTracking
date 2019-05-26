using AutoMapper;
using SKS.ParcelLogistics.BusinessLogic.Entities;
using SKS.ParcelLogistics.WebService.DTOs;

namespace SKS.ParcelLogistics.BusinessLogic
{
    public class ToBLProfile : Profile
    {
        public ToBLProfile()
        {
            CreateMap<Parcel, ParcelModel>().
                ForMember(x => x.TrackingCode, opt => opt.Ignore()).
                ForMember(x => x.Latitude, opt => opt.Ignore()).
                ForMember(x => x.Longitude, opt => opt.Ignore()).
                ReverseMap();

            CreateMap<Recipient, RecipientModel>().
                ReverseMap();

            CreateMap<Warehouse, WarehouseModel>().
                ForMember(x => x.Parent, opt => opt.Ignore()).
                ReverseMap();

            CreateMap<Truck, TruckModel>().
                ForMember(x => x.Parent, opt => opt.Ignore()).
                ReverseMap();
        }
    }
}