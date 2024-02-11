# ZainCash.Net

### Install:
##### using CLI:
`dotnet add package ZainCash.Net --version 3.0.0`

##### using Package Manager:
`Install-Package ZainCash.Net -Version 3.0.0`

### Description:
ZainCash.Net is a simple and easy-to-use library for integrating ZainCash payment gateway with your .NET applications. It provides a simple and easy-to-use API for creating a new transaction, generating a token, decoding the token, and getting the transaction details.

### Environment:
- [x] Production.
- [x] Development.

### Tested on:
- [x] .NET 8

### Usage :

#### Register the service and the config DI (Dependency Injection):
```csharp
// [+] Register zain cash service using DI with environment type.
builder.Services.AddZainCashService(isDevelopment: true);

// [+] Register zain cash api config (could be one of the below approach).

// 1- Will use the default section name "ZainCashAPIConfig" from the *.json file.
builder.Services.AddZainCashConfig(configuration);

// 2- Or, you could specify the section name.
builder.Services.AddZainCashConfig(configuration.GetSection("ZainCashAPIConfig"));

// 3- Or, you could specify the config directly.
builder.Services.AddZainCashConfig(new ZainCashAPIConfig
{
    Language = "en",
    ServiceType = "book_service",
    Msisdn = "9647835077893",
    MerchantId = "5ffacf6612b5777c6d44266f",
    RedirectUrl = "https://www.your-website.com/",
    Secret = "$2y$10$hBbAZo2GfSSvyqAyV2SaqOfYewgYpfR1O19gIh4SqyGWdmySZYPuS",
});
```

#### Use the service from the DI:
```csharp
// - Now you can access zain cash config everywhere using DI.
var config = builder.Services.BuildServiceProvider().GetRequiredService<ZainCashAPIConfig>();

// - You can now access the service using the IZainCashService interface
// - The type of the service will be determined by the environment type on the service registration.
IZainCashService service = builder.Services.BuildServiceProvider().GetRequiredService<IZainCashService>();
```

#### Using without DI :
```csharp
// - OR by creating an instance of the service directly.
IZainCashService productionService = new ZainCashService(config);
IZainCashService developmentService = new TestZainCashService(config);
```

#### Init a new transaction (create transaction):
```csharp
// - To create a new transaction.
var initRequest = new InitTransactionRequest
{
    Amount = 1500,      // at least 1000 IQD
    OrderId = "123456",
};
InitTransactionResponse response = await service.InitTransactionAsync(initRequest, CancellationToken.None);
console.WriteLine(response.Id); // Transaction Id
console.WriteLine(response.Amount); // Transaction amount
console.WriteLine(response.OrderId); // Transaction order id
console.WriteLine(response.ServiceType); // Transaction service type
console.WriteLine(response.ReferenceNumber); // Transaction reference number
console.WriteLine(response.CurrencyConversion); // Transaction currency conversion
```

#### Token
```csharp
// - To generate the token without creating a new transaction.
string token = service.GenerateToken(initRequest);

// [+] The next steps will be on the client side (FrontEnd)
// - User will pay using the URL returned from the previous step. [FE]
// - After the user pays, the user will be redirected to the RedirectUrl. [FE]
// - The RedirectUrl will contain the token in the query string. [FE] [BE]
// - Will be something like: https://www.your-website.com/?token=THE_TOKEN


// [+] Back to the server side (BackEnd)
// - To decode the token and get the transaction details like status or id.
TokenResult tokenResult = service.DecodeToken("THE_TOKEN");

// - You can check the transaction status. e.g.
if (tokenResult.Status == PaymentStatus.Success)
{
    // - The transaction is successful.
}
if (tokenResult.Status == PaymentStatus.Failed)
{
    // - The transaction is failed.
}
if (tokenResult.Status == PaymentStatus.Pending)
{
    // - The transaction is pending.
}
if (tokenResult.Status == PaymentStatus.Completed)
{
    // - The transaction is completed.
}

// - TokenResult contains more details as well.
Console.WriteLine(tokenResult.Id); // Transaction Id
Console.WriteLine(tokenResult.Msg); // Response message
Console.WriteLine(tokenResult.Status); // Transaction status
Console.WriteLine(tokenResult.OrderId); // Order Id
```

#### Get transaction details by the transaction id
```csharp
// - To get the transaction details by the transaction id.
TransactionResponse transactionDetails = await service.GetTransactionAsync(tokenResult.Id, CancellationToken.None);

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
Console.WriteLine(transactionDetails.CurrencyConversion); // Transaction currency conversion``
```