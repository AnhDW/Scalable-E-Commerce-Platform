using Contracts.DTOs.Payment.Gateway;
using PaymentService.Entities;
using PaymentService.GatewayRepository.IGatewayRepository;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace PaymentService.GatewayRepository
{
    public class VnPayGateway : IPaymentGateway
    {
        private readonly IConfiguration _cfg;
        public string Name => "VNPay";

        public VnPayGateway(IConfiguration cfg)
        {
            _cfg = cfg;
        }


        public Task<PaymentRedirectResultDto> CreatePaymentAsync(Payment payment, CancellationToken ct = default)
        {
            // build params
            var vnpUrl = _cfg["VnPay:Url"]!;
            var vnpTmnCode = _cfg["VnPay:TmnCode"]!;
            var vnpHashSecret = _cfg["VnPay:Secret"]!;

            var vnpParams = new Dictionary<string, string>
        {
            {"vnp_Version","2.1.0"},
            {"vnp_Command","pay"},
            {"vnp_TmnCode", vnpTmnCode},
            {"vnp_Amount", ((long)(payment.TotalAmount * 100)).ToString()}, // VND in cents
            {"vnp_CurrCode","VND"},
            {"vnp_TxnRef", payment.Id.ToString()}, // our payment id
            {"vnp_OrderInfo", $"Payment for {payment.Id}"},
            {"vnp_ReturnUrl", _cfg["VnPay:ReturnUrl"]!},
            {"vnp_IpAddr", "0.0.0.0"},
            {"vnp_CreateDate", DateTime.UtcNow.ToString("yyyyMMddHHmmss")}
        };

            // sort & create query + hash
            var ordered = vnpParams.OrderBy(kv => kv.Key, StringComparer.Ordinal);
            var query = string.Join("&", ordered.Select(kv => $"{WebUtility.UrlEncode(kv.Key)}={WebUtility.UrlEncode(kv.Value)}"));
            var signData = string.Join("&", ordered.Select(kv => $"{kv.Key}={kv.Value}"));
            var secureHash = ToHmacSha512(vnpHashSecret, signData);
            var redirectUrl = $"{vnpUrl}?{query}&vnp_SecureHash={secureHash}";

            var result = new PaymentRedirectResultDto { RedirectUrl = redirectUrl, Provider = Name };
            return Task.FromResult(result);
        }

        public async Task<(bool Valid, string GatewayTransactionId, decimal Amount, IDictionary<string, string> Raw)> VerifyCallbackAsync(HttpRequest request, CancellationToken ct = default)
        {
            var secret = _cfg["VnPay:Secret"]!;
            // read all query params
            var q = request.Query.ToDictionary(k => k.Key, v => v.Value.ToString());
            q.TryGetValue("vnp_SecureHash", out var secureHash);
            // remove secure hash then compute
            var filtered = q.Where(kv => kv.Key != "vnp_SecureHash" && kv.Key != "vnp_SecureHashType")
                            .ToDictionary(kv => kv.Key, kv => kv.Value);
            var signData = string.Join("&", filtered.OrderBy(k => k.Key).Select(kv => $"{kv.Key}={kv.Value}"));
            var computed = ToHmacSha512(secret, signData);
            var valid = string.Equals(computed, secureHash, StringComparison.OrdinalIgnoreCase);

            filtered.TryGetValue("vnp_TransactionNo", out var txnNo);
            filtered.TryGetValue("vnp_TxnRef", out var txnRef);
            filtered.TryGetValue("vnp_Amount", out var amountStr);
            decimal amount = 0;
            if (!string.IsNullOrEmpty(amountStr) && long.TryParse(amountStr, out var a)) amount = a / 100m;

            return (valid, txnNo ?? Guid.NewGuid().ToString(), amount, filtered);
        }

        private static string ToHmacSha512(string key, string data)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var dataBytes = Encoding.UTF8.GetBytes(data);
            using var hmac = new HMACSHA512(keyBytes);
            var hash = hmac.ComputeHash(dataBytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
