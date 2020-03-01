using Intrinio.SDK.Api;
using Intrinio.SDK.Client;


namespace MarketDataMonitorAPI
{
    public class GetLatestPrice
    {

        public decimal TickerCurrentPrice(string ticker)
        {
            Configuration.Default.AddApiKey("api_key", "OjI0MmFhYmZmODViODJjNDhkODZhNjZmOGE1NmZkNTFh");

            var securityApi = new SecurityApi();
            var identifier = ticker;  // string | A Security identifier (Ticker, FIGI, ISIN, CUSIP, Intrinio ID)

            var realtimePrice = securityApi.GetSecurityRealtimePrice(identifier);
            var latestPrice = realtimePrice.LastPrice;

            //converts nullable decemal to decemcial
            decimal? a = latestPrice;
            decimal b = a ?? -1;
           

            return b;
        }



    }
}
