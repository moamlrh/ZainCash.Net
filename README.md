# ZainCash.Net

### Install :

`dotnet add package ZainCash.Net --version 2.0.0`

### Usage :

```csharp
var initRequest = new InitZainCashApiRequest
{
    Amount = 1000, // at lease 1000 IQD
    OrderId = "123456",
    ServiceType = "book_service",
    Msisdn = "9647835077893", // your wallet phone number
    MerchantId = "5ffacf6612b5777c6d44266f", // your merchant id from ZainCash support
    RedirectionUrl = "https://www.your-website.com/", // which will handle the response from ZainCash with the token as a query string
    Secret = "$2y$10$hBbAZo2GfSSvyqAyV2SaqOfYewgYpfR1O19gIh4SqyGWdmySZYPuS", // your secret from ZainCash support
};

// - Create new Transaction [BE]
IZainCashService service = new ZainCashService(); // OR by using DI (Dependency Injection).

// - To create a new transaction, you need to call the InitAsync method. [BE]
string URL = await service.InitAsync(initRequest, true, CancellationToken.None);

// - To generate the token without creating a new transaction, you need to call the GenerateToken method. [BE]
string token = service.GenerateToken(initRequest);

// - User will pay using the URL returned from the previous step. [FE]

// - After the user pays, the user will be redirected to the RedirectionUrl. [FE]

// - The RedirectionUrl will contain the token in the query string. [FE] [BE]
// - Will be something like: https://www.your-website.com/?token=THE_TOKEN

// - To decode the token and get the transaction details and status.
TokenResult tokenResult = service.DecodeToken("THE_TOKEN", initRequest.Secret);

// - You can check the transaction status. e.g.
if (tokenResult.Status == PaymentStatus.success)
{
    // - The transaction is successful.
}
if (tokenResult.Status == PaymentStatus.failed)
{
    // - The transaction is failed.
}
if (tokenResult.Status == PaymentStatus.pending)
{
    // - The transaction is pending.
}
if (tokenResult.Status == PaymentStatus.completed)
{
    // - The transaction is completed.
}

// - TokenResult contains more details as well.
Console.WriteLine(tokenResult.Id); // Transaction Id
Console.WriteLine(tokenResult.Msg); // Response message
Console.WriteLine(tokenResult.Status); // Transaction status
Console.WriteLine(tokenResult.OrderId); // Order Id
```