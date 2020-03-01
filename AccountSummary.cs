using Alpaca.Markets;
using System;
using System.Threading.Tasks;

namespace MarketDataMonitorAPI
{
     public class AccountSummary
     {
        private static string API_KEY = "PK43DG0LFRIX11TF9LF8";

        private static string API_SECRET = "VbqwOhfIf8HWU5XngW/mp40A7kOhcHkeg4Km7CcL";

        public async Task ExecuteBalanceView()
        {
           // First, open the API connection
            var client = Environments.Paper
                .GetAlpacaTradingClient(API_KEY, new SecretKey(API_SECRET));

            // Get account info
            var account = await client.GetAccountAsync();

            //# Check our current balance vs. our balance at the last market close
            var balance_change = account.Equity - account.LastEquity;
            Console.WriteLine($"Today's portfolio balance change: ${balance_change}");

        }
     }
}
