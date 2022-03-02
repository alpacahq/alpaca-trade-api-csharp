using Alpaca.Markets;
using OoplesFinance.StockIndicators.Models;
using static OoplesFinance.StockIndicators.Calculations;
using System.Text;

namespace UsageExamples
{
    internal sealed class OoplesIndicatorLibraryExample
    {
        const string paperApiKey = "REPLACEME";
        const string paperApiSecret = "REPLACEME";
        const string symbol = "AAPL";

        public static async Task RunExample()
        {
            var startDate = new DateTime(2021, 01, 01);
            var endDate = new DateTime(2021, 12, 31);

            using var client = Environments.Paper.GetAlpacaDataClient(new SecretKey(paperApiKey, paperApiSecret));
            var bars = (await client.ListHistoricalBarsAsync(new HistoricalBarsRequest(symbol, startDate, endDate, BarTimeFrame.Day)).ConfigureAwait(false)).Items;
            var stockData = new StockData(bars.Select(x => x.Open), bars.Select(x => x.High), bars.Select(x => x.Low), bars.Select(x => x.Close), bars.Select(x => x.Volume), bars.Select(x => x.TimeUtc));

            // can do typical indicators such as using typical bollinger band settings
            UseDefaultBollingerBands(stockData);

            // we can also do completely custom indicators with very little effort such as below
            UseCustomBollingerBands(stockData);

            // can also create indicators out of any other indicators such as a macd of a rsi
            UseRelativeStrengthIndexMacd(stockData);
        }

        private static void UseDefaultBollingerBands(StockData stockData)
        {
            var bbResults = stockData.CalculateBollingerBands();
            PrintResults(bbResults);
        }

        private static void UseCustomBollingerBands(StockData stockData)
        {
            var bbResults = stockData.CalculateBollingerBands(MovingAvgType.Ehlers3PoleSuperSmootherFilter, 50, 3);
            PrintResults(bbResults);
        }

        private static void UseRelativeStrengthIndexMacd(StockData stockData)
        {
            var macdResults = stockData.CalculateRelativeStrengthIndex().CalculateMovingAverageConvergenceDivergence();
            PrintResults(macdResults);
        }

        private static void PrintResults(StockData stockData)
        {
            var count = stockData.OutputValues.Count;

            if (count > 0)
            {
                Console.WriteLine($"Printing {stockData.IndicatorName} results for {symbol}:");
                Console.WriteLine();
                var headerString = "";

                for (int h = 0; h < stockData.OutputValues.First().Value.Count; h++)
                {
                    var sb = new StringBuilder();
                    for (int i = 0; i < count; i++)
                    {
                        var outputValue = stockData.OutputValues.ElementAt(i).Value.ElementAt(h);
                        var date = stockData.Dates.ElementAt(h).ToShortDateString();

                        if (h == 0)
                        {
                            // do this once to display header values
                            var headerValue = stockData.OutputValues.ElementAt(i).Key;
                            if (i == 0)
                            {
                                headerString += $"Date | {headerValue} | ";
                            }
                            else if (i == count - 1)
                            {
                                headerString += headerValue;
                            }
                            else
                            {
                                headerString += $"{headerValue} | ";
                            }
                        }

                        // display the output values
                        if (i == 0)
                        {
                            sb.Append($"{date} | {outputValue} | ");
                        }
                        else if (i == count - 1)
                        {
                            sb.Append(outputValue);

                            if (h == 0)
                            {
                                Console.WriteLine(headerString);
                            }

                            Console.WriteLine(sb.ToString());
                        }
                        else
                        {
                            sb.Append($"{outputValue} | ");
                        }
                    }
                }
            }
        }
    }
}
