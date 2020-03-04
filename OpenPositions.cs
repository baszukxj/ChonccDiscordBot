using Alpaca.Markets;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace MarketDataMonitorAPI
{
    public class OpenPositions
    {
        private static string API_KEY = ConfigurationManager.AppSettings["AlpacaAPIKey"];
        private static string API_SECRET = ConfigurationManager.AppSettings["AlpacaAPISecret"];

        
        public async Task<int> ViewOpenSharesQuantity(string ticker)
        {
            var client = Environments.Paper
                .GetAlpacaTradingClient(API_KEY, new SecretKey(API_SECRET));

            // Get our position in AAPL.
            try
            {
                var tickerPosition = await client.GetPositionAsync(ticker);

                var quantity = tickerPosition.Quantity;

                return quantity;
            }
            catch (Exception)
            {
                return 0;
            }
           
        }

        public async Task<string> ViewPositionSymbol(string ticker)
        {
            var client = Environments.Paper
                .GetAlpacaTradingClient(API_KEY, new SecretKey(API_SECRET));

            try
            {
                // Get our position in AAPL.
                var tickerPosition = await client.GetPositionAsync(ticker);

                var symbol = tickerPosition.Symbol;

                return symbol;
            }
            catch (Exception) 
            {
                return null;
            }
            // Console.WriteLine(symbol);
           

        }

        public async Task<decimal> ViewPositionUnrealizedProfitLoss(string ticker)
        {
            var client = Environments.Paper
                .GetAlpacaTradingClient(API_KEY, new SecretKey(API_SECRET));


            try
            {
                // Get our position in AAPL.
                var tickerPosition = await client.GetPositionAsync(ticker);

                var change = tickerPosition.IntradayUnrealizedProfitLoss;

                return change;
            }
            catch (Exception) 
            {
                return 0;
            }
        }


    }
}
