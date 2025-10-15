using AuthService.Entities;
using AutoMapper;
using Contracts.DTOs.Auth;

namespace AuthService.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDto, ApplicationUser>().ReverseMap();
            CreateMap<ApplicationRole, RoleDto>().ReverseMap();

        }

    }
}
