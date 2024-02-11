using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ZainCash.Net.DTOs;

namespace ZainCash.Net.Utils;

internal class TokenHelper
{
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
    ///  To generate a token for ZainCash API based on the init request, without making an API call. 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static string GenerateToken(InitTransactionRequest request, ZainCashAPIConfig config)
    {
        var data = request.GetData(config);
        return GenerateToken(data, config.Secret);
    }

    /// <summary>
    ///  This method is used to decode the token and return the name of the user.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="secret"></param>
    /// <returns></returns>
    public static TokenResult DecodeToken(string token, string secret)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        var secretKey = Encoding.UTF8.GetBytes(secret);
        var validationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
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
        var response = new TokenResult
        {
            Id = id,
            Msg = msg,
            OrderId = orderId,
            Status = (PaymentStatus)Enum.Parse(typeof(PaymentStatus), status)
        };
        return response;
    }
}