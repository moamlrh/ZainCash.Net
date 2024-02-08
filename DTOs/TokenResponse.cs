namespace ZainCash.Net.DTOs;

public class TokenResponse
{
    public Status Status { get; set; }
    public string Msg { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
}

public enum Status
{
    failed,
    success,
    pending,
    completed,
}
