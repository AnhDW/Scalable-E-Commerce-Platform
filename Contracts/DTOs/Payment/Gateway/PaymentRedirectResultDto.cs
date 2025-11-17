namespace Contracts.DTOs.Payment.Gateway
{
    public class PaymentRedirectResultDto
    {
        public string RedirectUrl { get; set; } = string.Empty;
        public string Provider { get; set; } = string.Empty;
    }
}
