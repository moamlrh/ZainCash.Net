using System.Net.Http.Json;
using ZainCash.Net.DTOs;
using ZainCash.Net.Utils;

namespace ZainCash.Net.Services;

public abstract class BaseZainCashService : IZainCashService
{
    private readonly ZainCashAPIConfig _config;
    public BaseZainCashService(ZainCashAPIConfig config)
    {
        _config = config;
    }

    /// <summary>
    /// Initialize a new transaction and return the URL which used by the client to redirect to ZainCash payment page.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<InitTransactionResponse> InitTransactionAsync(InitTransactionRequest request, CancellationToken cancellationToken = default)
    {
        var data = request.GetData(_config);
        var token = TokenHelper.GenerateToken(data, _config.Secret);
        var tUrl = GetInitTransactionUrl();
        var rUrl = GetPayTransactionUrl();

        using var client = new HttpClient();
        try
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "token", token },
                { "lang", _config.Language },
                { "merchantId", _config.MerchantId },
            });
            var response = await client.PostAsync(tUrl, content, cancellationToken);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("ZainCash API with status code: " + response.StatusCode);
            }
            if (response.Content == null)
            {
                throw new Exception("ZainCash API response is null");
            }
            var zainCashAPIResponse = await response.Content.ReadFromJsonAsync<InitTransactionResponse>(cancellationToken);
            if (zainCashAPIResponse == null)
            {
                throw new Exception("ZainCash API response is null");
            }
            return zainCashAPIResponse;
        }
        catch (Exception ex)
        {
            throw new Exception("ZainCash API error: " + ex.Message);
        }
        finally
        {
            client.Dispose();
        }
    }

    /// <summary>
    ///  Get the transaction details based on the transaction id.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="isDevelopment"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TransactionDetailsResponse> GetTransactionDetailsAsync(TransactionDetailsRequest request, CancellationToken cancellationToken = default)
    {
        var data = request.GetData();
        var token = TokenHelper.GenerateToken(data, request.Secret);
        var rUrl = GetTransactionUrl();

        using var client = new HttpClient();
        try
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "token", token },
                { "merchantId", request.MerchantId },
            });
            var response = await client.PostAsync(rUrl, content, cancellationToken);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("ZainCash API with status code: " + response.StatusCode);
            }
            if (response.Content == null)
            {
                throw new Exception("ZainCash API response is null");
            }

            var transactionDetails = await response.Content.ReadFromJsonAsync<TransactionDetailsResponse>(cancellationToken);
            if (transactionDetails == null)
            {
                throw new Exception("ZainCash API response is null");
            }
            return transactionDetails;
        }
        catch (Exception ex)
        {
            throw new Exception("ZainCash API error: " + ex.Message);
        }
        finally
        {
            client.Dispose();
        }
    }

    /// <summary>
    ///  Get the status of the transaction based on the token and the secret.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="secret"></param>
    /// <returns></returns>
    public PaymentStatus GetStatusFromToken(string token, string secret)
    {
        return TokenHelper.DecodeToken(token, secret).Status;
    }

    /// <summary>
    ///  Decode the token and return the result.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="secret"></param>
    /// <returns></returns>
    public TokenResult DecodeToken(string token, string secret)
    {
        return TokenHelper.DecodeToken(token, secret);
    }

    /// <summary>
    ///  Generate a token for ZainCash API based on the init request data.
    /// </summary>
    /// <param name="initZainRequest"></param>
    /// <returns></returns>
    public string GenerateToken(InitTransactionRequest initZainRequest)
    {
        return TokenHelper.GenerateToken(initZainRequest, _config);
    }

    /// <summary>
    ///  Extract the transaction id from the URL.
    /// </summary>
    /// <param name="URL"></param>
    /// <returns></returns>
    public string ExtractTransactionId(string URL)
    {
        var array = URL.Split("id=");
        if (array.Length > 1)
        {
            return array[1];
        }
        return string.Empty;
    }
    protected abstract string GetInitTransactionUrl();
    protected abstract string GetPayTransactionUrl();
    protected abstract string GetTransactionUrl();
}
