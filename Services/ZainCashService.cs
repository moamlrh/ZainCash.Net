using System.Net.Http.Json;
using ZainCash.Net.DTOs;
using ZainCash.Net.Utils;

namespace ZainCash.Net.Services;

public class ZainCashService : BaseZainCashService
{
    public ZainCashService(ZainCashAPIConfig config) 
        : base(config)
    {
    }

    protected override string GetInitTransactionUrl()
        => "https://api.zaincash.iq/transaction/init";

    protected override string GetPayTransactionUrl()
        => "https://api.zaincash.iq/transaction/pay?id=";

    protected override string GetTransactionUrl()
        => "https://api.zaincash.iq/transaction/get";
}
