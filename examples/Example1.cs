using ZainCash.Net.DTOs;

namespace ZainCash.Net.examples;

internal class Example1
{
    public static async Task GetTransactionId_Pay_GetToken_CheckResponse()
    {
        var initRequest = new InitZainCashApiRequest
        {
            Msisdn = "9647835077893",
            Amount = "1000",
            Language = "en",
            OrderId = "123456",
            MerchantId = "5ffacf6612b5777c6d44266f",
            Secret = "$2y$10$hBbAZo2GfSSvyqAyV2SaqOfYewgYpfR1O19gIh4SqyGWdmySZYPuS",
            ServiceType = "ozone service",
            RedirectionUrl = "https://www.google.com"
        };

        // - Create new Transaction [BE]
        var URL = await ZainCashInitializer.Init(initRequest);

        // - User will pay using the URL returned from the previous step. [FE]

        // - After the user pays, the user will be redirected to the RedirectionUrl. [FE]

        // - The RedirectionUrl will contain the token in the query string. [FE] [BE]

        // the token will be decoded to get the transaction details [BE]
        var decodedToke = ZainCashInitializer.DecodeToken("TOKEN_FROM_QUERY_STRING", initRequest.Secret);
        Console.WriteLine(decodedToke.Id);
        Console.WriteLine(decodedToke.Msg);
        Console.WriteLine(decodedToke.Status);
        Console.WriteLine(decodedToke.OrderId);
    }
}
