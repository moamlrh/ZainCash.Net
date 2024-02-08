namespace ZainCash.Net.DTOs;

public class InitZainCashApiRequest
{
    public string Language { get; set; } = "en";
    public string Amount { get; set; } = string.Empty;
    public string Msisdn { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public string MerchantId { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty;
    public string RedirectionUrl { get; set; } = string.Empty;
}