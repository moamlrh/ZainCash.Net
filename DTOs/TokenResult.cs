namespace ZainCash.Net.DTOs;

/// <summary>
///  Represents the result of a token decoding.
/// </summary>
public class TokenResult
{
    public PaymentStatus Status { get; set; }
    public string Msg { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
}

public enum PaymentStatus
{
    failed,
    success,
    pending,
    completed,
}
