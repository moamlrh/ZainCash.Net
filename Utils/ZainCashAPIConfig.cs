namespace ZainCash.Net.Utils;

/// <summary>
///  Could be used to configure the ZainCash API settings and credentials.
/// </summary>
public class ZainCashAPIConfig
{
    public string Language { get; set; } = "en";
    public string Secret { get; set; } = string.Empty;
    public string Msisdn { get; set; } = string.Empty;
    public string MerchantId { get; set; } = string.Empty;
    public string RedirectionUrl { get; set; } = string.Empty;
    public string ServiceType { get; set; } = "ozone service";

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Msisdn))
        {
            throw new Exception("Msisdn is required");
        }
        if (string.IsNullOrWhiteSpace(Language))
        {
            throw new Exception("Language is required");
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
            { "merchantId", MerchantId },
            { "serviceType", ServiceType },
            { "redirectUrl", RedirectionUrl },
            { "iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() },
            { "exp", (DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 60 * 60 * 4).ToString() }
        };
    }
}
