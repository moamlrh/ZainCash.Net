namespace ZainCash.Net.DTOs;

/// <summary>
///  Represents the response of the transaction details API call.
/// </summary>
public class TransactionResponse
{
    public int Amount { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public object? CurrencyConversion { get; set; }  
    public string ReferenceNumber { get; set; } = string.Empty;
    public string RedirectUrl { get; set; } = string.Empty;
    public bool Credit { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool Reversed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Id { get; set; } = string.Empty;
}

