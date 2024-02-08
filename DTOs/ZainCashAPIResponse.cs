namespace ZainCash.Net.DTOs;

public class ZainCashAPIResponse
{
    public string Source { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty;
    public string Lang { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public object? CurrencyConversion { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public bool Credit { get; set; } = false;
    public string Status { get; set; } = string.Empty;
    public bool Reversed { get; set; } = false;
    public string CreatedAt { get; set; } = string.Empty;
    public string UpdatedAt { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
}
