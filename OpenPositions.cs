using Alpaca.Markets;
using System;
using System.Threading.Tasks;

namespace MarketDataMonitorAPI
{
    public class OpenPositions
    {
        private static string API_KEY = "PK43DG0LFRIX11TF9LF8";

        private static string API_SECRET = "VbqwOhfIf8HWU5XngW/mp40A7kOhcHkeg4Km7CcL";

        public async Task ViewOpenAccountPositions(string ticker)
        {
            var client = Environments.Paper
                .GetAlpacaTradingClient(API_KEY, new SecretKey(API_SECRET));

            // Get our position in AAPL.
            var tickerPosition = await client.GetPositionAsync(ticker);

            // Get a list of all of our positions.
            var positions = await client.ListPositionsAsync();

            // Print the quantity of shares for each position.
            foreach (var position in positions)
            {
                Console.WriteLine($"{position.Quantity} shares of {position.Symbol}.");
            }

        }
    }
}
