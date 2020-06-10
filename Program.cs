using System;
using System.Collections.Generic;
using System.Threading;
using MarketDataMonitorAPI;

namespace Example
{
    public class GetSecurityPriceTechnicalsSmaExample
    {
        
        public static void Main()
        {
            #region Test Code/Classes

            //string tickerSymbol = "AAPL";

            ////initializes method from SMADayAverage class and sets it to a variable
            //SMADayAverage RealTimeSMA = new SMADayAverage();

            ////calls method from SMADayAverage class
            //var SMAAverageDay_13 = RealTimeSMA.SMACurrentAverage(13, tickerSymbol);
            //var SMAAverageDay_8 = RealTimeSMA.SMACurrentAverage(8, tickerSymbol);
            //var SMAAverageDay_5 = RealTimeSMA.SMACurrentAverage(5, tickerSymbol);

            //Console.WriteLine($"20SMA: { SMAAverageDay_13}");
            //Console.WriteLine($"10SMA: {SMAAverageDay_8}");
            //Console.WriteLine($"5SMA: {SMAAverageDay_5}");

            //Console.ReadLine();

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
            #endregion

            #region Timer
            //timeer for starting the real loop at 9:30am or around that time
            var waitFiveMinutes = TimeSpan.FromMinutes(5);
            int x = 0;
            while (x < 18)
            {
                Thread.Sleep(waitFiveMinutes);
                x++;
                Console.WriteLine("tick-tock");
            }
            #endregion

            List<string> tickerList = new List<string> { "AAPL", "MSFT", "CSCO" };

            var waitFiveSeconds = TimeSpan.FromSeconds(5);
            int i = 0;
            
            while (i < 5399)
            {
                Thread.Sleep(waitFiveSeconds);
                i++;

                foreach (string tickerSymbol in tickerList)
                {

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
                    var tenPercentOfAccount = doublevalue * 0.10;
                    //Console.WriteLine(fivePercentOfAccount);

                    //sets the maximum gain and loss the account is willing to gin and or loss then converts those values to decimals
                    var Loss = tenPercentOfAccount * -.05;
                    decimal maximumLoss = Convert.ToDecimal(Loss);
                    var Gain = tenPercentOfAccount * .15;
                    decimal maximumGain = Convert.ToDecimal(Gain);

                    //figures out how many share you can buy with 5% of account then convert value from double to int and round to the nearest whole number
                    var shareCountDouble = tenPercentOfAccount / currentPrice;
                    int shareCount = Convert.ToInt32(Math.Floor(shareCountDouble));
                    //Console.WriteLine(shareCount);

                    //initializes method from OpenPositions class and pings back how many shares are open currently
                    OpenPositions ViewOpenPositions = new OpenPositions();
                    var quantity = ViewOpenPositions.ViewOpenSharesQuantity(tickerSymbol);
                    var sharesOpen = quantity.Result;

                    //calls task method and returns value for symbol
                    var symbol = ViewOpenPositions.ViewPositionSymbol(tickerSymbol);
                    var tickerName = symbol.Result;

                    //Console.WriteLine(tickerName);

                    //calls task method and returns value for gain or loss
                    var change = ViewOpenPositions.ViewPositionUnrealizedProfitLoss(tickerSymbol);
                    var gainOrLoss = change.Result;

                    //Console.WriteLine(gainloss);

                    //initializes method from SMADayAverage class and sets it to a variable
                    SMADayAverage RealTimeSMA = new SMADayAverage();

                    //calls method from SMADayAverage class
                    var SMAAverageDay_11 = RealTimeSMA.SMACurrentAverage(11, tickerSymbol);
                    var SMAAverageDay_8 = RealTimeSMA.SMACurrentAverage(8, tickerSymbol);
                    var SMAAverageDay_5 = RealTimeSMA.SMACurrentAverage(5, tickerSymbol);

                    Console.WriteLine(tickerSymbol);
                    Console.WriteLine($"11SMA: {SMAAverageDay_11}");
                    Console.WriteLine($"8SMA: {SMAAverageDay_8}");
                    Console.WriteLine($"5SMA: {SMAAverageDay_5}");
                    Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");


                    //safeguard for if the SMA API call encounters an exception and returns a 0 so it goes back to the beginning of loop
                    if (SMAAverageDay_11 == 0 ||SMAAverageDay_8 == 0 || SMAAverageDay_5 == 0) 
                    {
                        continue;
                    }
                    else if (tickerName != tickerSymbol || string.IsNullOrEmpty(tickerName))
                    {
                        if (SMAAverageDay_5 >= SMAAverageDay_8 && _CurrentPrice > SMAAverageDay_11)
                        {
                            //initializes method from BuyShares class and then executes buy order for ticker
                            BuyShares buy = new BuyShares();
                            var buyTicker = buy.ExecuteOrder(tickerSymbol, shareCount);
                            Console.WriteLine("-------------------------------------------------------------------------------------");
                            Console.WriteLine($"XELINA bought {shareCount} shares of {tickerSymbol} at ${_CurrentPrice}");
                            Console.WriteLine("-------------------------------------------------------------------------------------");
                            continue;

                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (tickerName == tickerSymbol)
                    {
                        if (SMAAverageDay_5 <= SMAAverageDay_8 && _CurrentPrice <= SMAAverageDay_11)
                        {
                            //initializes method from SellShares class and then executes sell order for ticker
                            SellShares sell = new SellShares();
                            var sellTicker = sell.ExecuteOrder(tickerSymbol, sharesOpen);
                            Console.WriteLine("-------------------------------------------------------------------------------------");
                            Console.WriteLine($"XELINA sold {sharesOpen} shares of {tickerSymbol} at ${_CurrentPrice}");
                            continue;
                        }
                        else if (gainOrLoss <= maximumLoss)
                        {
                            //initializes method from SellShares class and then executes sell order for ticker
                            SellShares sell = new SellShares();
                            var sellTicker = sell.ExecuteOrder(tickerSymbol, sharesOpen);
                            Console.WriteLine("-------------------------------------------------------------------------------------");
                            Console.WriteLine($"XELINA sold {sharesOpen} shares of {tickerSymbol} at ${_CurrentPrice}");
                            continue;
                        }
                        else if (gainOrLoss >= maximumGain)
                        {
                            //initializes method from SellShares class and then executes sell order for ticker
                            SellShares sell = new SellShares();
                            var sellTicker = sell.ExecuteOrder(tickerSymbol, sharesOpen);
                            Console.WriteLine("-------------------------------------------------------------------------------------");
                            Console.WriteLine($"XELINA sold {sharesOpen} shares of {tickerSymbol} at ${_CurrentPrice}");
                            continue;
                        }
                        else
                        {
                            continue;
                        }

                    }
                }
            }

            AccountSummary AccountSummary = new AccountSummary();
            var totalGainLoss = AccountSummary.ExecuteBalanceView();


           Console.ReadLine();


        }

    }
 }
