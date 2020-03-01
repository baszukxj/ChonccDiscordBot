using Alpaca.Markets;
using System;
using System.Threading.Tasks;


namespace MarketDataMonitorAPI
{
     public class BuyShares
     {
        private static string API_KEY = "PK43DG0LFRIX11TF9LF8";

        private static string API_SECRET = "VbqwOhfIf8HWU5XngW/mp40A7kOhcHkeg4Km7CcL";

        public async Task ExecuteOrder(string ticker)
        {
            // First, open the API connection
            var client = Environments.Paper
                .GetAlpacaTradingClient(API_KEY, new SecretKey(API_SECRET));

            // Submit a market order to buy 1 share of Apple at market price
            var order = await client.PostOrderAsync(ticker, 1, OrderSide.Buy, OrderType.Market, TimeInForce.Day);

            // Submit a limit order to attempt to sell 1 share of AMD at a
            // particular price ($20.50) when the market opens
            order = await client.PostOrderAsync(ticker, 1, OrderSide.Sell, OrderType.Market, TimeInForce.Day);

            Console.Read();
        }

     }
}
