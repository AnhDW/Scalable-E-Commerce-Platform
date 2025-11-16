using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Payment;
using PaymentService.Entities;

namespace PaymentService.Repositories.IRepositories
{
    public interface IPaymentRepository
    {
        Task<PagedList<PaymentDto>> GetAll(PaymentParams paymentParams);
        Task<List<Payment>> GetAll();
        Task<Payment> GetById(Guid paymentId);
        void Add(Payment payment);
        void Update(Payment payment);
        void Delete(Payment payment);

    }
}
