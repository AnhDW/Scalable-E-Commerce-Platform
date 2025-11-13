using Common.Enums;
using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Relationship;
using RelationshipService.Entities;

namespace RelationshipService.Repositories.IRepositories
{
    public interface IPaymentOrderRelationRepository
    {
        Task<PagedList<PaymentOrderRelationDto>> GetAll(PaymentOrderRelationParams paymentOrderRelationParams);
        Task<List<PaymentOrderRelation>> GetAll();
        Task<List<Guid>> GetPaymentIdsByOrderId(Guid orderId);
        Task<List<Guid>> GetOrderIdsByPaymentId(Guid paymentId);
        Task<PaymentOrderRelation> GetById(Guid paymentId, Guid orderId);
        void Add(PaymentOrderRelation paymentOrderRelation);
        void Update(PaymentOrderRelation paymentOrderRelation);
        void Delete(PaymentOrderRelation paymentOrderRelation);
    }
}
