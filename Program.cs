using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Intrinio.SDK.Api;
using Intrinio.SDK.Client;
using Intrinio.SDK.Model;
using MarketDataMonitorAPI;

namespace Example
{
    public class GetSecurityPriceTechnicalsSmaExample
    {


        public static void Main()
        {
            //initializes method from SMADayAverage class and sets it to a variable
            SMADayAverage RealTimeSMAAverage = new SMADayAverage();

            //calls method from SMADayAverage class
           var SMAAverageDay_20 = RealTimeSMAAverage.SMACurrentAverage(20);
           var SMAAverageDay_10 = RealTimeSMAAverage.SMACurrentAverage(10);
           var SMAAverageDay_5 = RealTimeSMAAverage.SMACurrentAverage(5);
            
            Console.WriteLine($"20SMA: { SMAAverageDay_20}");
            Console.WriteLine($"10SMA: {SMAAverageDay_10}");
            Console.WriteLine($"5SMA: {SMAAverageDay_5}");

            //initializes method from GetLatestPrice class and sets it to a variable
            GetLatestPrice RealTimePrice = new GetLatestPrice();

            //call method from GetLatestPrice class and return the decemial value
            var currentTickerPrice = RealTimePrice.TickerCurrentPrice("AAPL");

            Console.WriteLine($"LatestPrice: {currentTickerPrice}");

            Console.ReadLine();


          
        }

    }
 }
