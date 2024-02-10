# ZainCash.Net

### Install :

`dotnet add package ZainCash.Net --version 2.0.0`

### Usage :

```csharp
var initRequest = new InitTransactionRequest
{
    OrderId = "123456",
    ServiceType = "book_service",
    Amount = 1500, // at lease 1000 IQD
    Msisdn = "9647835077893", // your wallet phone number
    MerchantId = "5ffacf6612b5777c6d44266f", // your merchant id from ZainCash support
    RedirectionUrl = "https://www.your-website.com/", // which will handle the response from ZainCash with the token as a query string
    Secret = "$2y$10$hBbAZo2GfSSvyqAyV2SaqOfYewgYpfR1O19gIh4SqyGWdmySZYPuS", // your secret from ZainCash support
};

// - Create new Transaction [BE]
IZainCashService service = new ZainCashService(); // OR by using DI (Dependency Injection).

// - For initiating a new transaction.
string URL = await service.InitTransactionAsync(initRequest, true, CancellationToken.None);

// - To generate the token without creating (init) a new transaction.
string token = service.GenerateToken(initRequest);

// - User will pay using the URL returned from the previous step which contains the transaction id.
// - After the user completes the payment, ZainCash will redirect the user to the RedirectionUrl.
// - The RedirectionUrl will contain the token in the query string
// - Will be something like: https://www.your-website.com/?token=THE_TOKEN

// - To decode the token and get the transaction details.
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


// - To get the transaction details by the transaction id.
TransactionDetailsResponse transactionDetails = await service.GetTransactionDetailsAsync(new TransactionDetailsRequest {
    TransactionId = tokenResult.Id,
    MerchantId = initRequest.MerchantId,
    Secret = initRequest.Secret
});


// - TransactionDetailsResponse contains more details as well.
Console.WriteLine(transactionDetails.Id); // Transaction id
Console.WriteLine(transactionDetails.Type); // Transaction type
Console.WriteLine(transactionDetails.Amount); // Transaction amount
Console.WriteLine(transactionDetails.Source); // Transaction source
Console.WriteLine(transactionDetails.Credit); // Transaction credit
Console.WriteLine(transactionDetails.Status); // Transaction status
Console.WriteLine(transactionDetails.OrderId); // Transaction order id
Console.WriteLine(transactionDetails.Reversed); // Transaction reversed
Console.WriteLine(transactionDetails.CreatedAt); // Transaction created at
Console.WriteLine(transactionDetails.UpdatedAt); // Transaction updated at
Console.WriteLine(transactionDetails.RedirectUrl); // Transaction redirect url
Console.WriteLine(transactionDetails.ServiceType); // Transaction service type
Console.WriteLine(transactionDetails.ReferenceNumber); // Transaction reference number
Console.WriteLine(transactionDetails.CurrencyConversion); // Transaction currency conversion
```
