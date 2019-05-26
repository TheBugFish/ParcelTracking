using AutoMapper;
using SKS.ParcelLogistics.BusinessLogic.Entities;

namespace SKS.ParcelLogistics.BusinessLogic
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ParcelModel, DataAccess.Entities.ParcelDTO>().
                ForMember(x => x.Id, opt => opt.Ignore()).
                ReverseMap();

            CreateMap<RecipientModel, DataAccess.Entities.RecipientDTO>().
                ForMember(x => x.Id, opt => opt.Ignore()).
                ReverseMap();

            CreateMap<WarehouseModel, DataAccess.Entities.WarehouseDTO>().
                ForMember(x => x.Id, opt => opt.Ignore()).
                ReverseMap();

            CreateMap<TruckModel, DataAccess.Entities.TruckDTO>().
                ForMember(x => x.Id, opt => opt.Ignore()).
                ForMember(x => x.Description, opt => opt.Ignore()).
                ForMember(x => x.NextHops, opt => opt.Ignore()).
                ForMember(x => x.Trucks, opt => opt.Ignore()).
                ReverseMap();

            CreateMap<HopArrivalModel, DataAccess.Entities.HopArrivalDTO>().
                ForMember(x => x.Id, opt => opt.Ignore()).
                ReverseMap();
        }
    }
}