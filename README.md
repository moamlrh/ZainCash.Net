# ZainCash.Net

### Install : 
`dotnet add package ZainCash.Net --version 1.0.4`

### Usage :
```csharp
var initRequest = new InitZainCashApiRequest
{
  Amount = "1000",// at least 1000 IQD
  Language = "en",
  OrderId = "123456",
  ServiceType = "book_service",
  Msisdn = "9647835077893", // your wallet phone number
  MerchantId = "5ffacf6612b5777c6d44266f", // your merchant id from ZainCash support
  Secret = "$2y$10$hBbAZo2GfSSvyqAyV2SaqOfYewgYpfR1O19gIh4SqyGWdmySZYPuS", // your secret from ZainCash support
  RedirectionUrl = "https://www.your-website-endpoint.com" // which will handle the response from ZainCash with the token as a query string
};

// - Create new Transaction [BE]
var URL = await ZainCashInitializer.Init(initRequest);

// - The user will pay using the URL returned from the previous step. [FE]

// - After the user pays, the user will be redirected to the RedirectionUrl. [FE]

// - The RedirectionUrl will contain the token in the query string. [FE] [BE]

// - The token will be decoded to get the transaction details [BE]
var decodedToke = ZainCashInitializer.DecodeToken("TOKEN_FROM_QUERY_STRING", initRequest.Secret);
Console.WriteLine(decodedToke.Id);
Console.WriteLine(decodedToke.Msg);
Console.WriteLine(decodedToke.Status);
Console.WriteLine(decodedToke.OrderId);
```
