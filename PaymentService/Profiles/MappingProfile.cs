using AutoMapper;
using Contracts.DTOs.Payment;
using PaymentService.Entities;

namespace PaymentService.Profiles
{
    public class MappingProfile : Profile
    {
        protected MappingProfile()
        {
            CreateMap<Payment, PaymentDto>().ReverseMap();
            CreateMap<PaymentTransaction, PaymentTransactionDto>().ReverseMap();
        }
    }
}
