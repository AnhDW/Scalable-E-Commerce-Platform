using AutoMapper;
using Contracts.DTOs.Relationship;
using RelationshipService.Entities;

namespace RelationshipService.Profiles
{
    public class MappingProfile : Profile
    {
        protected MappingProfile()
        {
            CreateMap<UserStoreRelation, UserStoreRelationDto>().ReverseMap();
            CreateMap<PaymentOrderRelation, PaymentOrderRelationDto>().ReverseMap();
        }
    }
}
