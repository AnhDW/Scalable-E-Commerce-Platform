using AutoMapper;
using Contracts.DTOs.Order;
using OrderService.Entities;

namespace OrderService.Profiles
{
    public class MappingProfile : Profile
    {
        protected MappingProfile()
        {
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<OrderStatusHistory, OrderStatusHistoryDto>().ReverseMap();
        }
    }
}
