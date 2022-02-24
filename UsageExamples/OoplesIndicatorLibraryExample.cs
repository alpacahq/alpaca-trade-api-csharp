using Alpaca.Markets;
using OoplesFinance.StockIndicators.Models;
using static OoplesFinance.StockIndicators.Calculations;

namespace UsageExamples
{
    public class OoplesIndicatorLibraryExample
    {
        const string paperApiKey = "REPLACEME";
        const string paperApiSecret = "REPLACEME";
        const string symbol = "AAPL";

        public async Task RunExample()
        {
            var startDate = new DateTime(2021, 01, 01);
            var endDate = new DateTime(2021, 12, 31);

            var client = Environments.Paper.GetAlpacaDataClient(new SecretKey(paperApiKey, paperApiSecret));
            var bars = (await client.ListHistoricalBarsAsync(new HistoricalBarsRequest(symbol, startDate, endDate, BarTimeFrame.Day)).ConfigureAwait(false)).Items;
            var stockData = new StockData(bars.Select(x => x.Open), bars.Select(x => x.High), bars.Select(x => x.Low), bars.Select(x => x.Close), bars.Select(x => x.Volume), bars.Select(x => x.TimeUtc));

            // can do typical indicators such as using typical bollinger band settings
            var results = stockData.CalculateBollingerBands();

            // we can also do completely custom indicators with very little effort such as below
            //var results2 = stockData.CalculateBollingerBands(MovingAvgType.Ehlers3PoleSuperSmootherFilter, 50, 3);

            // can also do indicators out of any indicators such as below
            // we are getting the macd of a typical rsi
            //var results3 = stockData.CalculateRelativeStrengthIndex().CalculateMovingAverageConvergenceDivergence();

            var upperBandList = results.OutputValues["UpperBand"];
            var middleBandList = results.OutputValues["MiddleBand"];
            var lowerBandList = results.OutputValues["LowerBand"];

            Console.WriteLine("Bollinger Band Results");
            for (int i = 0; i < stockData.Count; i++)
            {
                var upperBand = upperBandList[i];
                var middleBand = middleBandList[i];
                var lowerBand = lowerBandList[i];
                var close = stockData.ClosePrices[i];
                var signal = stockData.SignalsList[i];
                var date = stockData.Dates[i];

                Console.WriteLine($"{date.ToShortDateString()}: UB = {upperBand} MB = {middleBand} LB = {lowerBand} Price = {close} Signal = {signal} ");
            }
        }
    }
}
