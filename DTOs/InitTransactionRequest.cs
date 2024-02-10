namespace ZainCash.Net.DTOs;

/// <summary>
///  Represents the request to initiate a new transaction with the ZainCash API.
/// </summary>
public class InitTransactionRequest
{
    public int Amount { get; set; }
    public string Language { get; set; } = "en";
    public string Msisdn { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public string MerchantId { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty;
    public string RedirectionUrl { get; set; } = string.Empty;

    protected void Validate()
    {
        if (string.IsNullOrWhiteSpace(Msisdn))
        {
            throw new Exception("Msisdn is required");
        }
        if (Amount < 1000)
        {
            throw new Exception("Amount must be at least 1000");
        }
        if (string.IsNullOrWhiteSpace(Language))
        {
            throw new Exception("Language is required");
        }
        if (string.IsNullOrWhiteSpace(OrderId))
        {
            throw new Exception("OrderId is required");
        }
        if (string.IsNullOrWhiteSpace(MerchantId))
        {
            throw new Exception("MerchantId is required");
        }
        if (string.IsNullOrWhiteSpace(ServiceType))
        {
            throw new Exception("ServiceType is required");
        }
        if (string.IsNullOrWhiteSpace(RedirectionUrl))
        {
            throw new Exception("RedirectionUrl is required");
        }
        if (string.IsNullOrWhiteSpace(Secret))
        {
            throw new Exception("Secret is required");
        }
    }

    public Dictionary<string, string> GetData()
    {
        this.Validate();
        return new Dictionary<string, string>
        {
            { "msisdn", Msisdn },
            { "lang", Language },
            { "orderId", OrderId },
            { "merchantId", MerchantId },
            { "serviceType", ServiceType },
            { "amount", Amount.ToString() },
            { "redirectUrl", RedirectionUrl },
            { "iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() },
            { "exp", (DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 60 * 60 * 4).ToString() }
        };
    }
}