using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Intrinio.SDK.Api;
using Intrinio.SDK.Client;
using Intrinio.SDK.Model;

namespace MarketDataMonitorAPI
{
    public class SMADayAverage
    {

        public decimal SMACurrentAverage(int periodNum)
        {
            Configuration.Default.AddApiKey("api_key", "OjI0MmFhYmZmODViODJjNDhkODZhNjZmOGE1NmZkNTFh");

            var securityApi = new SecurityApi();
            var identifier = "AAPL";  // string | A Security identifier (Ticker, FIGI, ISIN, CUSIP, Intrinio ID)
            var period = periodNum;  // int? | The number of observations, per period, to calculate Simple Moving Average (optional)  (default to 20)
            var priceKey = "close";  // string | The Stock Price field to use when calculating Simple Moving Average (optional)  (default to close)
            var startDate = DateTime.Parse("2020-02-01");  // string | Return technical indicator values on or after the date (optional) 
            var endDate = DateTime.Now;  // string | Return technical indicator values on or before the date (optional) 
            var pageSize = 20;  // int? | The number of results to return (optional)  (default to 100)
            var nextPage = "";  // string | Gets the next page of data from a previous API call (optional) 

            //string startDateTime = startDate.ToString();
            //string endDateTime = endDate.ToString();
            var pageSizeNum = pageSize.ToString();

            //try
            //{
            //    ApiResponseSecuritySimpleMovingAverage result = securityApi.GetSecurityPriceTechnicalsSma(identifier, period, priceKey,  /*startDateTime, endDateTime,*/ pageSizeNum, nextPage);
            //    Console.WriteLine(result.ToJson());
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Exception when calling SecurityApi.GetSecurityPriceTechnicalsSma: " + e.Message);
            //}

            var smaAverage = securityApi.GetSecurityPriceTechnicalsSma(identifier, period, priceKey, /*startDateTime, endDateTime,*/ pageSizeNum, nextPage).Technicals;

        
            var x = new List<float>();

            //adds every line of the SMA to a new list
            foreach (var line in smaAverage)
            {
                var SMA = line.Sma.GetValueOrDefault();
                x.Add(SMA);
               // Console.WriteLine(SMA);
            }
            var sum = x.Sum();
            decimal newSum = Convert.ToDecimal(sum);
            //Console.WriteLine(sum);

            //calls api and gets the lastes price
            var realtimePrice = securityApi.GetSecurityRealtimePrice(identifier);
            var latestPrice = realtimePrice.LastPrice;

            //converts nullable decemal to decemcial
            decimal? a = latestPrice;
            decimal b = a ?? -1;
           

            //adds the sum of last 7 day x period sma plus current price then divides by all to get current average sma for x period
            var sumSMA = newSum + b;
            var CurrentAverageSMA = sumSMA / 8;

            
            //returns currentaverageSMA 
            return CurrentAverageSMA;
            
        }


    }
    
}
