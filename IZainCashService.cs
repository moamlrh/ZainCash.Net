using ZainCash.Net.DTOs;

namespace ZainCash.Net;

public interface IZainCashService
{
    string GetPaymentUrl(string transactionId);
    string GenerateToken(InitTransactionRequest initRequest);

    TokenResult DecodeToken(string token);
    Task<TransactionResponse> GetTransactionAsync(string transactionId, CancellationToken cancellationToken = default);
    Task<InitTransactionResponse> InitTransactionAsync(InitTransactionRequest initRequest, CancellationToken cancellationToken = default);
}
