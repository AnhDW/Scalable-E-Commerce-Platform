using Contracts.DTOs.Payment.Gateway;
using PaymentService.Entities;
using PaymentService.GatewayRepository.IGatewayRepository;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PaymentService.GatewayRepository
{
    public class MomoGateway : IPaymentGateway
    {
        private readonly IConfiguration _cfg;
        public string Name => "Momo";
        public MomoGateway(IConfiguration cfg) { _cfg = cfg; }

        public async Task<PaymentRedirectResultDto> CreatePaymentAsync(Payment payment, CancellationToken ct = default)
        {
            var endpoint = _cfg["Momo:CreateUrl"]!;
            var partnerCode = _cfg["Momo:PartnerCode"]!;
            var accessKey = _cfg["Momo:AccessKey"]!;
            var secret = _cfg["Momo:Secret"]!;

            var requestId = Guid.NewGuid().ToString();
            var orderId = payment.Id.ToString();
            var amount = ((long)(payment.TotalAmount)).ToString(); // depends on momo doc

            var body = new Dictionary<string, string>
        {
            {"partnerCode", partnerCode},
            {"accessKey", accessKey},
            {"requestId", requestId},
            {"amount", amount},
            {"orderId", orderId},
            {"orderInfo", $"Payment for {payment.Id}"},
            {"returnUrl", _cfg["Momo:ReturnUrl"]!},
            {"notifyUrl", _cfg["Momo:NotifyUrl"]!},
            {"extraData", "" }
        };

            // raw signature creation (see momo docs)
            var raw = string.Join("&", body.Select(kv => $"{kv.Key}={kv.Value}"));
            var signature = ToHmacSha256(secret, raw);
            body.Add("signature", signature);

            using var client = new HttpClient();
            var resp = await client.PostAsync(endpoint, new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json"), ct);
            var text = await resp.Content.ReadAsStringAsync(ct);
            using var doc = JsonDocument.Parse(text);
            var payUrl = doc.RootElement.GetProperty("payUrl").GetString() ?? string.Empty;
            return new PaymentRedirectResultDto { RedirectUrl = payUrl, Provider = Name };
        }

        public async Task<(bool Valid, string GatewayTransactionId, decimal Amount, IDictionary<string, string> Raw)> VerifyCallbackAsync(HttpRequest request, CancellationToken ct = default)
        {
            // Momo sends JSON via callback (IPN). Read body
            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;
            var doc = JsonDocument.Parse(body);
            var root = doc.RootElement;
            // build raw dictionary for audit
            var raw = root.EnumerateObject().ToDictionary(p => p.Name, p => p.Value.ToString());

            // verify signature
            var signature = root.GetProperty("signature").GetString();
            // build raw signature string the same as request
            var secret = _cfg["Momo:Secret"]!;
            var rawData = string.Join("&", root.EnumerateObject().Where(p => p.Name != "signature").Select(p => $"{p.Name}={p.Value.ToString()}"));
            var computed = ToHmacSha256(secret, rawData);
            var valid = string.Equals(computed, signature, StringComparison.OrdinalIgnoreCase);

            var txnId = root.TryGetProperty("transId", out var t) ? t.GetString() ?? "" : Guid.NewGuid().ToString();
            var amount = root.TryGetProperty("amount", out var a) && a.TryGetDecimal(out var am) ? am : 0m;

            return (valid, txnId, amount, raw);
        }

        private static string ToHmacSha256(string key, string data)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var dataBytes = Encoding.UTF8.GetBytes(data);
            using var hmac = new HMACSHA256(keyBytes);
            var hash = hmac.ComputeHash(dataBytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
