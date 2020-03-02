using Alpaca.Markets;
using System;
using System.Configuration;
using System.Threading.Tasks;


namespace MarketDataMonitorAPI
{
     public class SellShares
     {
        private static string API_KEY = ConfigurationManager.AppSettings["AlpacaAPIKey"];
        private static string API_SECRET = ConfigurationManager.AppSettings["AlpacaAPISecret"];


        public async Task ExecuteOrder(string ticker)
        {
            // First, open the API connection
            var client = Environments.Paper
                .GetAlpacaTradingClient(API_KEY, new SecretKey(API_SECRET));

            // Submit a market order to attempt to sell 1 share of APPL
           var order = await client.PostOrderAsync(ticker, 1, OrderSide.Sell, OrderType.Market, TimeInForce.Day);

            Console.Read();
        }

    }
}
