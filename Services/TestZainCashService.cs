using ZainCash.Net.Utils;

namespace ZainCash.Net.Services;

public class TestZainCashService : BaseZainCashService
{
    public TestZainCashService(ZainCashAPIConfig config) 
        : base(config)
    {
    }

    protected override string GetInitTransactionUrl()
        => "https://test.zaincash.iq/transaction/init";

    protected override string GetPayTransactionUrl()
        => "https://test.zaincash.iq/transaction/pay?id=";

    protected override string GetTransactionUrl()
        => "https://test.zaincash.iq/transaction/get";
}
