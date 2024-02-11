using System.Net.Http.Json;
using ZainCash.Net.DTOs;
using ZainCash.Net.Utils;

namespace ZainCash.Net.Services;

public abstract class BaseZainCashService : IZainCashService
{
    private readonly ZainCashAPIConfig _config;
    private readonly TokenHelper _tokenHelper;
    public BaseZainCashService(ZainCashAPIConfig config)
    {
        _config = config;
        _tokenHelper = new TokenHelper(config);
    }

    /// <summary>
    /// Initialize a new transaction and return the URL which used by the client to redirect to ZainCash payment page.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<InitTransactionResponse> InitTransactionAsync(InitTransactionRequest request, CancellationToken cancellationToken = default)
    {
        var data = request.GetData(_config);
        var token = _tokenHelper.GenerateToken(data);
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
    public async Task<TransactionResponse> GetTransactionAsync(string transactionId, CancellationToken cancellationToken = default)
    {
        var data = new Dictionary<string, string>
        {
            { "id", transactionId },
            { "msisdn", _config.Msisdn },
            { "iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() },
            { "exp", (DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 60 * 60 * 4).ToString() }
        };
        var token = _tokenHelper.GenerateToken(data);
        var rUrl = GetTransactionUrl();

        using var client = new HttpClient();
        try
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "token", token },
                { "merchantId", _config.MerchantId },
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

            var transactionDetails = await response.Content.ReadFromJsonAsync<TransactionResponse>(cancellationToken);
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
    public PaymentStatus GetStatusFromToken(string token)
    {
        return _tokenHelper.DecodeToken(token).Status;
    }

    /// <summary>
    ///  Decode the token and return the result.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="secret"></param>
    /// <returns></returns>
    public TokenResult DecodeToken(string token)
    {
        return _tokenHelper.DecodeToken(token);
    }

    /// <summary>
    ///  Generate a token for ZainCash API based on the init request data.
    /// </summary>
    /// <param name="initZainRequest"></param>
    /// <returns></returns>
    public string GenerateToken(InitTransactionRequest initZainRequest)
    {
        return _tokenHelper.GenerateToken(initZainRequest);
    }

    /// <summary>
    ///  Get the payment URL based on the transaction id.
    /// </summary>
    /// <param name="transactionId"></param>
    /// <returns></returns>
    public string GetPaymentUrl(string transactionId)
    {
        return GetPayTransactionUrl() + transactionId;
    }

    protected abstract string GetInitTransactionUrl();
    protected abstract string GetPayTransactionUrl();
    protected abstract string GetTransactionUrl();

}
