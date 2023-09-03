using Alpaca.Markets;
using System.Diagnostics.CodeAnalysis;
using OoplesFinance.StockIndicators.Enums;
using OoplesFinance.StockIndicators.Models;
using OoplesFinance.StockIndicators.Interfaces;
using static OoplesFinance.StockIndicators.Calculations;

namespace UsageExamples;

[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
internal sealed class IndicatorLibraryExample : IDisposable
{
    private const String API_KEY = "REPLACEME";

    private const String API_SECRET = "REPLACEME";

    private const String symbol = "AAPL";

    private IAlpacaDataClient alpacaDataClient;

    public async Task Run()
    {
        var startDate = new DateTime(2021, 01, 01);
        var endDate = new DateTime(2021, 12, 31);

        alpacaDataClient = Environments.Paper.GetAlpacaDataClient(new SecretKey(API_KEY, API_SECRET));

        var page = await alpacaDataClient.ListHistoricalBarsAsync(
            new HistoricalBarsRequest(symbol, startDate, endDate, BarTimeFrame.Day)).ConfigureAwait(false);
        var bars = page.Items;

        var stockData = new StockData(
            bars.Select(x => (Double)x.Open), bars.Select(x => (Double)x.High),
            bars.Select(x => (Double)x.Low), bars.Select(x => (Double)x.Close),
            bars.Select(x => (Double)x.Volume), bars.Select(x => x.TimeUtc));

        // can do typical indicators such as using typical bollinger band settings
        UseDefaultBollingerBands(stockData);

        stockData.SignalsList.Clear();
        stockData.CustomValuesList.Clear();
        // we can also do completely custom indicators with very little effort such as below
        UseCustomBollingerBands(stockData);

        stockData.SignalsList.Clear();
        stockData.CustomValuesList.Clear();
        // can also create indicators out of any other indicators such as a MACD of a RSI
        UseRelativeStrengthIndexMacd(stockData);
    }

    public void Dispose() => alpacaDataClient?.Dispose();

    private static void UseDefaultBollingerBands(StockData stockData) =>
        PrintResults(stockData.CalculateBollingerBands());

    private static void UseCustomBollingerBands(StockData stockData) =>
        PrintResults(stockData.CalculateBollingerBands(
            MovingAvgType.Ehlers3PoleSuperSmootherFilter, 50, 3));

    private static void UseRelativeStrengthIndexMacd(StockData stockData) =>
        PrintResults(stockData.CalculateRelativeStrengthIndex()
            .CalculateMovingAverageConvergenceDivergence());

    private static void PrintResults(IStockData stockData)
    {
        var seriesCount = stockData.OutputValues.Count;
        if (seriesCount <= 0)
        {
            return;
        }

        var rowsCount = stockData.OutputValues.First().Value.Count;
        if (rowsCount <= 0)
        {
            return;
        }

        Console.WriteLine($"Printing {stockData.IndicatorName} results for {symbol}:");
        Console.WriteLine();

        Console.WriteLine("Date\t\t" + String.Join("\t",
            stockData.OutputValues.Keys.Select(key => key.PadRight(8))));

        for (var row = 0; row < rowsCount; row++)
        {
            var date = stockData.Dates[row].ToShortDateString();
            var values = String.Join("\t\t", 
                stockData.OutputValues.Values
                    // ReSharper disable once AccessToModifiedClosure
                    .Select(value => value[row].ToString("F2")));

            Console.WriteLine($"{date}\t{values}");
        }

        Console.WriteLine();
    }
}
