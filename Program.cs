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

            SMADayAverage RealTimeSMAAverage = new SMADayAverage();

            RealTimeSMAAverage.SMACurrentAverage(20);
            Console.ReadLine();


          
        }

    }
 }
