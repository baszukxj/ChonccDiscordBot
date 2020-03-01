using Alpaca.Markets;
using System;
using System.Threading.Tasks;


namespace MarketDataMonitorAPI
{
     public class SellShares
     {

        private static string API_KEY = "PK43DG0LFRIX11TF9LF8";

        private static string API_SECRET = "VbqwOhfIf8HWU5XngW/mp40A7kOhcHkeg4Km7CcL";

        public async Task ExecuteOrder(string ticker)
        {
            // First, open the API connection
            var client = Environments.Paper
                .GetAlpacaTradingClient(API_KEY, new SecretKey(API_SECRET));

            // Submit a limit order to attempt to sell 1 share of AMD at a
            // particular price ($20.50) when the market opens
           var order = await client.PostOrderAsync(ticker, 1, OrderSide.Sell, OrderType.Market, TimeInForce.Day);

            Console.Read();
        }

    }
}
