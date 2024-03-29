﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZainCash.Net.DTOs;
using ZainCash.Net.Extensions;
using ZainCash.Net.Services;
using ZainCash.Net.Utils;

namespace ZainCash.Net.examples;

internal class Example1
{
    public static async Task First_Example(WebApplicationBuilder builder, IConfiguration configuration)
    {
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

        // - Now you can access zain cash config everywhere using DI.
        var config = builder.Services.BuildServiceProvider().GetRequiredService<ZainCashAPIConfig>();

        // - You can now access the service using the IZainCashService interface 
        // - The type of the service will be determined by the environment type on the service registration.
        IZainCashService service = builder.Services.BuildServiceProvider().GetRequiredService<IZainCashService>();

        // - OR by creating an instance of the service directly.
        IZainCashService productionService = new ZainCashService(config);
        IZainCashService developmentService = new TestZainCashService(config);

        // - To create a new transaction, you need to call the InitAsync method. [BE]
        var initRequest = new InitTransactionRequest
        {
            Amount = 1500,          // at least 1000 IQD 
            OrderId = "123456",
        };
        InitTransactionResponse response = await service.InitTransactionAsync(initRequest, CancellationToken.None);
        Console.WriteLine(response.Id); // Transaction Id

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
        Console.WriteLine(transactionDetails.CurrencyConversion); // Transaction currency conversion
    }
}
