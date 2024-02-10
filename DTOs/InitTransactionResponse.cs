namespace ZainCash.Net.DTOs;

/// <summary>
///  Represents the response from the ZainCash API on initiating a new transaction.
/// </summary>
public class InitTransactionResponse
{
    public int Amount { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
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
