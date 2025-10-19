using BusinessService.Entities;
using AutoMapper;
using Contracts.DTOs.Business;

namespace BusinessService.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Store, StoreDto>().ReverseMap();
            CreateMap<StoreCategory, StoreCategoryDto>().ReverseMap();
            CreateMap<StoreTag, StoreTagDto>().ReverseMap();
            CreateMap<StoreCategoryRelation, StoreCategoryRelationDto>().ReverseMap();
            CreateMap<StoreTagRelation, StoreTagRelationDto>().ReverseMap();
        }

    }
}
