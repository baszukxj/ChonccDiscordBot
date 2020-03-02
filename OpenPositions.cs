using Alpaca.Markets;
using System;
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
            var tickerPosition = await client.GetPositionAsync(ticker);

            var quantity = tickerPosition.Quantity;

            //Console.WriteLine(quantity);
            return quantity;

        }

        public async Task<string> ViewPositionSymbol(string ticker)
        {
            var client = Environments.Paper
                .GetAlpacaTradingClient(API_KEY, new SecretKey(API_SECRET));

            // Get our position in AAPL.
            var tickerPosition = await client.GetPositionAsync(ticker);

            var symbol = tickerPosition.Symbol;

           // Console.WriteLine(symbol);
            return symbol;

        }

        public async Task<decimal> ViewPositionUnrealizedProfitLoss(string ticker)
        {
            var client = Environments.Paper
                .GetAlpacaTradingClient(API_KEY, new SecretKey(API_SECRET));

            // Get our position in AAPL.
            var tickerPosition = await client.GetPositionAsync(ticker);

            var change = tickerPosition.IntradayUnrealizedProfitLoss;

           // Console.WriteLine(change);
            return change;

        }
    }
}
