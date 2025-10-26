using AutoMapper;
using Contracts.DTOs.Relationship;
using Contracts.DTOs.ShoppingCart;
using ShoppingCartService.Entities;

namespace ShoppingCartService.Profiles
{
    public class MappingProfile : Profile
    {
        protected MappingProfile()
        {
            CreateMap<CartItem, CartItemDto>().ReverseMap();
        }
    }
}
