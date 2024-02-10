using ZainCash.Net.DTOs;

namespace ZainCash.Net.Interfaces;

public interface IZainCashService
{
    Task<string> InitTransactionAsync(InitTransactionRequest initRequest, bool isDevelopment = false, CancellationToken cancellationToken = default);
    Task<TransactionDetailsResponse> GetTransactionDetailsAsync(TransactionDetailsRequest request, bool isDevelopment = false, CancellationToken cancellationToken = default);
    TokenResult DecodeToken(string token, string secret);
    string GenerateToken(InitTransactionRequest initRequest);
    string ExtractTransactionId(string URL);
}
