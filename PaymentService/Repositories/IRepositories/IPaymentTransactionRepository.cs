using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Payment;
using PaymentService.Entities;

namespace PaymentService.Repositories.IRepositories
{
    public interface IPaymentTransactionRepository
    {
        Task<PagedList<PaymentTransactionDto>> GetAll(PaymentTransactionParams paymentTransactionParams);
        Task<List<PaymentTransaction>> GetAll();
        Task<PaymentTransaction> GetById(Guid paymentTransactionId);
        void Add(PaymentTransaction paymentTransaction);
        void Update(PaymentTransaction paymentTransaction);
        void Delete(PaymentTransaction paymentTransaction);
    }
}
