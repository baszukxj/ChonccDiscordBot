using System;
using System.Threading;
using MarketDataMonitorAPI;

namespace Example
{
    public class GetSecurityPriceTechnicalsSmaExample
    {
        
        public static void Main()
        {
            string tickerSymbol = "AAPL";

            #region Test Code/Classes
            ////initializes method from SMADayAverage class and sets it to a variable
            //SMADayAverage RealTimeSMA = new SMADayAverage();

            ////calls method from SMADayAverage class
            //var SMAAverageDay_20 = RealTimeSMA.SMACurrentAverage(20, tickerSymbol);
            //var SMAAverageDay_10 = RealTimeSMA.SMACurrentAverage(10, tickerSymbol);
            //var SMAAverageDay_5 = RealTimeSMA.SMACurrentAverage(5, tickerSymbol);

            //Console.WriteLine($"20SMA: { SMAAverageDay_20}");
            //Console.WriteLine($"10SMA: {SMAAverageDay_10}");
            //Console.WriteLine($"5SMA: {SMAAverageDay_5}");

            ////initializes method from GetLatestPrice class and sets it to a variable
            //GetLatestPrice RealTimePrice = new GetLatestPrice();

            ////call method from GetLatestPrice class and return the decemial value
            //var currentPrice = RealTimePrice.TickerCurrentPrice(tickerSymbol);

            //Console.WriteLine($"LatestPrice: {currentTickerPrice}");

            ////initializes method from BuyShares class and then executes buy order for ticker
            //BuyShares buy = new BuyShares();
            //var buyTicker = buy.ExecuteOrder("AAPL");

            ////initializes method from SellShares class and then executes sell order for ticker
            //SellShares sell = new SellShares();
            //var sellTicker = sell.ExecuteOrder("AAPL");

            ////initializes method from Account Summary class then displays gain/loss on account for the intraday
            //AccountSummary ViewAccountSummary = new AccountSummary();
            //var portfolioBalance = ViewAccountSummary.ExecuteAccountEquity();

            //beginning of the Loop---------------------------------------------------------------------------------------------------------------------------------------------------
            #endregion

            var waitFiveSeconds = TimeSpan.FromSeconds(5);
            int i = 0;
            while (i < 5399)
            {
                Thread.Sleep(waitFiveSeconds);
                i++;

                //initializes method from GetLatestPrice class and sets it to a variable
                GetLatestPrice RealTimePrice = new GetLatestPrice();

                //call method from GetLatestPrice class and return the decemial value
                var currentTickerPrice = RealTimePrice.TickerCurrentPrice(tickerSymbol);
                double currentPrice = Convert.ToDouble(currentTickerPrice);
                decimal _CurrentPrice = Convert.ToDecimal(currentTickerPrice);

                //Console.WriteLine($"LatestPrice: {currentTickerPrice}");

                //initializes method from Account Summary class then displays amount in account and sets 5% of the account to be traded
                AccountSummary ViewAccountSummary = new AccountSummary();
                var portfolioBalance = ViewAccountSummary.ExecuteAccountEquity();
                //Console.WriteLine(portfolioBalance.Result);

                //converts portfolio balance to a double so it can be divided to get 5% of account
                var results = portfolioBalance.Result;
                double doublevalue = Convert.ToDouble(results);
                var fivePercentOfAccount = doublevalue * 0.05;
                //Console.WriteLine(fivePercentOfAccount);

                //sets the maximum gain and loss the account is willing to gin and or loss then converts those values to decimals
                var Loss = fivePercentOfAccount * -.07;
                decimal maximumLoss = Convert.ToDecimal(Loss);
                var Gain = fivePercentOfAccount * .1;
                decimal maximumGain = Convert.ToDecimal(Gain);

                //figures out how many share you can buy with 5% of account then convert value from double to int and round to the nearest whole number
                var shareCountDouble = fivePercentOfAccount / currentPrice;
                int shareCount = Convert.ToInt32(Math.Floor(shareCountDouble));
                //Console.WriteLine(shareCount);

                //initializes method from OpenPositions class and pings back how many shares are open currently
                OpenPositions ViewOpenPositions = new OpenPositions();
                var quantity = ViewOpenPositions.ViewOpenSharesQuantity(tickerSymbol);
                var sharesOpen = quantity.Result;
                //Console.WriteLine(sharesOpen);

                //calls task method and returns value for symbol
                var symbol = ViewOpenPositions.ViewPositionSymbol(tickerSymbol);
                var tickerName = symbol.Result;
                //Console.WriteLine(tickerName);

                //calls task method and returns value for gain or loss
                var change = ViewOpenPositions.ViewPositionUnrealizedProfitLoss(tickerSymbol);
                var gainloss = change.Result;
                //Console.WriteLine(gainloss);


                //initializes method from SMADayAverage class and sets it to a variable
                SMADayAverage RealTimeSMA = new SMADayAverage();

                //calls method from SMADayAverage class
                var SMAAverageDay_20 = RealTimeSMA.SMACurrentAverage(20, tickerSymbol);
                var SMAAverageDay_10 = RealTimeSMA.SMACurrentAverage(10, tickerSymbol);
                var SMAAverageDay_5 = RealTimeSMA.SMACurrentAverage(5, tickerSymbol);

                Console.WriteLine($"20SMA: { SMAAverageDay_20}");
                Console.WriteLine($"10SMA: {SMAAverageDay_10}");
                Console.WriteLine($"5SMA: {SMAAverageDay_5}");

                if (tickerName != tickerSymbol || string.IsNullOrEmpty(tickerName))
                {
                    if (SMAAverageDay_5 >= SMAAverageDay_10 && _CurrentPrice > SMAAverageDay_20)
                    {
                        //initializes method from BuyShares class and then executes buy order for ticker
                        BuyShares buy = new BuyShares();
                        var buyTicker = buy.ExecuteOrder(tickerSymbol, shareCount);
                        continue;

                    }
                    else
                    {
                        continue;
                    }
                }
                else if (tickerName == tickerSymbol)
                {
                    if (SMAAverageDay_5 <= SMAAverageDay_10 && _CurrentPrice <= SMAAverageDay_20)
                    {
                        //initializes method from SellShares class and then executes sell order for ticker
                        SellShares sell = new SellShares();
                        var sellTicker = sell.ExecuteOrder(tickerSymbol, sharesOpen);
                        continue;
                    }
                    else if (gainloss <= maximumLoss)
                    {
                        //initializes method from SellShares class and then executes sell order for ticker
                        SellShares sell = new SellShares();
                        var sellTicker = sell.ExecuteOrder(tickerSymbol, sharesOpen);
                        continue;
                    }
                    else if (gainloss <= maximumGain)
                    {
                        //initializes method from SellShares class and then executes sell order for ticker
                        SellShares sell = new SellShares();
                        var sellTicker = sell.ExecuteOrder(tickerSymbol, sharesOpen);
                        continue;
                    }

                }

            }

            AccountSummary AccountSummary = new AccountSummary();
            var totalGainLoss = AccountSummary.ExecuteBalanceView();
            Console.WriteLine(totalGainLoss);

            Console.ReadLine();


        }

    }
 }
