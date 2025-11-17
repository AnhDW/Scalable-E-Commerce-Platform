using Contracts.DTOs.Payment.Gateway;
using PaymentService.Entities;

namespace PaymentService.GatewayRepository.IGatewayRepository
{
    public interface IPaymentGateway
    {
        string Name { get; }
        Task<PaymentRedirectResultDto> CreatePaymentAsync(Payment payment, CancellationToken ct = default);
        Task<(bool Valid, string GatewayTransactionId, decimal Amount, IDictionary<string, string> Raw)> VerifyCallbackAsync(HttpRequest request, CancellationToken ct = default);
    }
}
