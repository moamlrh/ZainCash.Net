using System.Linq;
using ZainCash.Net.Utils;

namespace ZainCash.Net.DTOs;

/// <summary>
///  Represents the request to initiate a new transaction with the ZainCash API.
/// </summary>
public class InitTransactionRequest
{
    public int Amount { get; set; }
    public string OrderId { get; set; } = string.Empty;

    protected void Validate()
    {
        if (Amount < 1000)
        {
            throw new Exception("Amount must be at least 1000");
        }
        if (string.IsNullOrWhiteSpace(OrderId))
        {
            throw new Exception("OrderId is required");
        }
    }

    public Dictionary<string, string> GetData(ZainCashAPIConfig config)
    {
        Validate();
        var local = new Dictionary<string, string>
        {
            { "orderId", OrderId },
            { "amount", Amount.ToString() },
        };
        return local.Union(config.GetData()).ToDictionary(x => x.Key, x => x.Value);
    }
}