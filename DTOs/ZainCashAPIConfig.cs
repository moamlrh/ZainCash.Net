namespace ZainCash.Net.DTOs;

/// <summary>
///  Could be used to configure the ZainCash API settings and credentials.
/// </summary>
public class ZainCashAPIConfig
{
    public string Language { get; set; } = "en";
    public string Secret { get; set; } = string.Empty;
    public string Msisdn { get; set; } = string.Empty;
    public string MerchantId { get; set; } = string.Empty;
    public string RedirectUrl { get; set; } = string.Empty;
    public string serviceType { get; set; } = "ozone service";
}
