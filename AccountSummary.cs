using Alpaca.Markets;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace MarketDataMonitorAPI
{
     public class AccountSummary
     {

        private static string API_KEY = ConfigurationManager.AppSettings["AlpacaAPIKey"];
        private static string API_SECRET = ConfigurationManager.AppSettings["AlpacaAPISecret"];

        //private static string API_KEY = "PKTBQ6VVXSS6QH0A6R9G";

        //private static string API_SECRET = "Okl9rNweJLh2bgwBIJdZzja1FPjvTiEWI/X7wdrz";

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

        public async Task<decimal> ExecuteAccountEquity()
        {
            // First, open the API connection
            var client = Environments.Paper
                .GetAlpacaTradingClient(API_KEY, new SecretKey(API_SECRET));

            // Get account info
            var account = await client.GetAccountAsync();

            //# Check our current balance vs. our balance at the last market close
            decimal balance = account.Equity;
            
            return balance;  

        }
    }
}
