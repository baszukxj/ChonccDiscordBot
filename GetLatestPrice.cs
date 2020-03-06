using Intrinio.SDK.Api;
using Intrinio.SDK.Client;
using System;

namespace MarketDataMonitorAPI
{
    public class GetLatestPrice
    {

        public decimal TickerCurrentPrice(string ticker)
        {
            Configuration.Default.AddApiKey("api_key", "OjI0MmFhYmZmODViODJjNDhkODZhNjZmOGE1NmZkNTFh");

            var securityApi = new SecurityApi();
            var identifier = ticker;  // string | A Security identifier (Ticker, FIGI, ISIN, CUSIP, Intrinio ID)
            try
            {
                var realtimePrice = securityApi.GetSecurityRealtimePrice(identifier);
                var latestPrice = realtimePrice.LastPrice;

                //converts nullable decemal to decemcial
                decimal? a = latestPrice;
                decimal b = a ?? -1;

                //return latestPrice;
                return b;
            }
            catch (Exception e)
            {
                var b = 1;
                Console.WriteLine("Exception when calling SecurityApi.GetSecurityRealtimePrice: " + e.Message);
                return b;
            }
        }



    }
}
