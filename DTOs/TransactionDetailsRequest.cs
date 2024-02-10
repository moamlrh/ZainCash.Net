namespace ZainCash.Net.DTOs;

/// <summary>
///  Represents the request of the transaction details API call.
/// </summary>
public class TransactionDetailsRequest 
{
    public string TransactionId { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public string Msisdn { get; set; } = string.Empty;
    public string MerchantId { get; set; } = string.Empty;

    protected void Validate()
    {
        if (string.IsNullOrWhiteSpace(TransactionId))
        {
            throw new ArgumentException("TransactionId is required", nameof(TransactionId));
        }
        if (string.IsNullOrWhiteSpace(Secret))
        {
            throw new ArgumentException("Secret is required", nameof(Secret));
        }
        if (string.IsNullOrWhiteSpace(Msisdn))
        {
            throw new ArgumentException("Msisdn is required", nameof(Msisdn));
        }
        if (string.IsNullOrWhiteSpace(MerchantId))
        {
            throw new ArgumentException("MerchantId is required", nameof(MerchantId));
        }
    }

    public Dictionary<string, string> GetData()
    {
        Validate();
        return new Dictionary<string, string>
        {
            { "id", TransactionId },
            { "msisdn", Msisdn },
            { "iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() },
            { "exp", (DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 60 * 60 * 4).ToString() }
        };
    }
}
