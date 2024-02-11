using ZainCash.Net.DTOs;

namespace ZainCash.Net;

public interface IZainCashService
{
    Task<InitTransactionResponse> InitTransactionAsync(InitTransactionRequest initRequest, CancellationToken cancellationToken = default);
    Task<TransactionDetailsResponse> GetTransactionDetailsAsync(TransactionDetailsRequest request, CancellationToken cancellationToken = default);
    TokenResult DecodeToken(string token, string secret);
    string GenerateToken(InitTransactionRequest initRequest);
    string ExtractTransactionId(string URL);
}
