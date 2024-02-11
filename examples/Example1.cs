using ZainCash.Net.DTOs;
using ZainCash.Net.Services;
using ZainCash.Net.Utils;

namespace ZainCash.Net.examples;

internal class Example1
{
    public static async Task First_Example()
    {
        var initRequest = new InitTransactionRequest
        {
            OrderId = "123456",
            Amount = 1500, // at lease 1000 IQD 
        };

        var config = new ZainCashAPIConfig
        {
            ServiceType = "book_service",
            Language = "en",
            MerchantId = "5ffacf6612b5777c6d44266f",// your merchant id from ZainCash support
            RedirectionUrl = "https://www.your-website.com/", // which will handle the response from ZainCash with the token as a query string
            Secret = "$2y$10$hBbAZo2GfSSvyqAyV2SaqOfYewgYpfR1O19gIh4SqyGWdmySZYPuS", // your secret from ZainCash support
            Msisdn = "9647835077893", // your wallet phone number
        };

        // - Create new Transaction [BE]
        IZainCashService service = new TestZainCashService(config); // OR by using DI (Dependency Injection).

        // - To create a new transaction, you need to call the InitAsync method. [BE]
        InitTransactionResponse response = await service.InitTransactionAsync(initRequest, CancellationToken.None);

        // - To generate the token without creating a new transaction, you need to call the GenerateToken method. [BE]
        string token = service.GenerateToken(initRequest);

        // - User will pay using the URL returned from the previous step. [FE]

        // - After the user pays, the user will be redirected to the RedirectionUrl. [FE]

        // - The RedirectionUrl will contain the token in the query string. [FE] [BE]
        // - Will be something like: https://www.your-website.com/?token=THE_TOKEN

        // - To decode the token and get the transaction details and status.
        TokenResult tokenResult = service.DecodeToken("THE_TOKEN", config.Secret);

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


        // - To get the transaction details by the transaction id.
        TransactionDetailsResponse transactionDetails = await service.GetTransactionDetailsAsync(new TransactionDetailsRequest
        {
            TransactionId = tokenResult.Id,
            MerchantId = config.MerchantId,
            Secret = config.Secret
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
    }
}
