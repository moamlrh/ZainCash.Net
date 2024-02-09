
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ZainCash.Net.DTOs;

namespace ZainCash.Net;

public class ZainCashInitializer
{
    private string URL = string.Empty;
    private static string rUrl = string.Empty;
    private static string tUrl = string.Empty;
    public static bool IsDevelopment = false;

    public ZainCashInitializer(InitZainCashApiRequest initRequest, bool isDevelopment = false)
    {
        IsDevelopment = isDevelopment;
        this.URL = Init(initRequest).Result;
    }

    /// <summary>
    /// Init (create) new transaction and return the URL to redirect the user to ZainCash payment page.
    /// </summary>
    /// <param name="initRequest"></param>
    /// <returns></returns>
    public static async Task<string> Init(InitZainCashApiRequest initRequest)
    {
        var data = new Dictionary<string, string>
        {
            { "msisdn", initRequest.Msisdn },
            { "amount", initRequest.Amount },
            { "lang", initRequest.Language },
            { "orderId", initRequest.OrderId },
            { "merchantId", initRequest.MerchantId },
            { "serviceType", initRequest.ServiceType },
            { "redirectUrl", initRequest.RedirectionUrl },
            { "iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() },
            { "exp", (DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 60 * 60 * 4).ToString() }
        };

        var token = GenerateToken(data, initRequest.Secret);

        if (IsDevelopment)
        {
            tUrl = "https://test.zaincash.iq/transaction/init";
            rUrl = "https://test.zaincash.iq/transaction/pay?id=";
        }
        else
        {
            tUrl = "https://api.zaincash.iq/transaction/init";
            rUrl = "https://api.zaincash.iq/transaction/pay?id=";
        }

        using var client = new HttpClient();
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "token", token },
                { "lang", initRequest.Language },
                { "merchantId", initRequest.MerchantId },
            });
        try
        {
            var response = await client.PostAsync(tUrl, content);
            var zainCashAPIResponse = await response.Content.ReadFromJsonAsync<ZainCashAPIResponse>();
            if (zainCashAPIResponse == null)
            {
                throw new Exception("ZainCash API response is null");
            }
            var APIUrlWithTransactionId = rUrl + zainCashAPIResponse.Id;
            return APIUrlWithTransactionId;
        }
        catch (Exception e)
        {
            throw new Exception("Error while calling ZainCash API: " + e.Message);
        }
    }

    /// <summary>
    ///  This method is used to generate a token for ZainCash API based on the data and the secret.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="secret"></param>
    /// <returns></returns>
    public static string GenerateToken(Dictionary<string, string> data, string secret)
    {
        var claims = new List<Claim>();
        foreach (var item in data)
        {
            claims.Add(new Claim(item.Key, item.Value));
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(4),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    ///  This method is used to decode the token and return the name of the user.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="secret"></param>
    /// <returns></returns>
    public static TokenResponse DecodeToken(string token, string secret)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        var secretKey = Encoding.UTF8.GetBytes(secret);
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero,

        };
        var principal = handler.ValidateToken(token, validationParameters, out var validatedToken);
        var id = principal.Claims.First(c => c.Type.Equals("id", StringComparison.CurrentCultureIgnoreCase)).Value;
        var msg = principal.Claims.First(c => c.Type.Equals("msg", StringComparison.CurrentCultureIgnoreCase)).Value;
        var status = principal.Claims.First(c => c.Type.Equals("status", StringComparison.CurrentCultureIgnoreCase)).Value;
        var orderId = principal.Claims.First(c => c.Type.Equals("orderId", StringComparison.CurrentCultureIgnoreCase)).Value;
        var response = new TokenResponse
        {
            Id = id,
            Msg = msg,
            OrderId = orderId,
            Status = (Status)Enum.Parse(typeof(Status), status)
        };
        return response;
    }

    public string GetURL() => URL;
}
