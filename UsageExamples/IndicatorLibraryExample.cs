using Alpaca.Markets;
using System.Diagnostics.CodeAnalysis;
using OoplesFinance.StockIndicators.Builder;
using OoplesFinance.StockIndicators.Models;

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
        var bars = page.Items.ToList();

        var dates = bars.Select(x => x.TimeUtc).ToList();

        var stockData = new StockData(
            bars.Select(x => (Double)x.Open), bars.Select(x => (Double)x.High),
            bars.Select(x => (Double)x.Low), bars.Select(x => (Double)x.Close),
            bars.Select(x => (Double)x.Volume), dates);

        var source = IndicatorDataSource.FromBatch(stockData);

        SeriesHandle defaultUpper = default, defaultMiddle = default, defaultLower = default;
        SeriesHandle customUpper = default, customMiddle = default, customLower = default;
        SeriesHandle rsi = default;
        SeriesHandle macdOfRsiPrimary = default, macdOfRsiSignal = default, macdOfRsiHistogram = default;

        var builder = new StockIndicatorBuilder(source)
            .ConfigureIndicators(indicators =>
            {
                // can do typical indicators such as using typical bollinger band settings
                var defaultBollingerBands = indicators.BollingerBands();
                defaultUpper = defaultBollingerBands.Upper;
                defaultMiddle = defaultBollingerBands.Middle;
                defaultLower = defaultBollingerBands.Lower;

                // we can also do completely custom indicators with very little effort such as below
                var customBollingerBands = indicators.BollingerBands(50, 3);
                customUpper = customBollingerBands.Upper;
                customMiddle = customBollingerBands.Middle;
                customLower = customBollingerBands.Lower;

                // can also create indicators out of any other indicators such as a MACD of an RSI
                rsi = indicators.Rsi();
                var macdOfRsi = indicators.Then(rsi).Macd();
                macdOfRsiPrimary = macdOfRsi.Primary;
                macdOfRsiSignal = macdOfRsi.Signal;
                macdOfRsiHistogram = macdOfRsi.Histogram;
            });

        using var runtime = builder.Build();
        runtime.Start();

        PrintResults("Bollinger Bands (default)", dates,
            ("Upper", runtime.GetSeries(defaultUpper)),
            ("Middle", runtime.GetSeries(defaultMiddle)),
            ("Lower", runtime.GetSeries(defaultLower)));

        PrintResults("Bollinger Bands (length 50, 3 std dev)", dates,
            ("Upper", runtime.GetSeries(customUpper)),
            ("Middle", runtime.GetSeries(customMiddle)),
            ("Lower", runtime.GetSeries(customLower)));

        PrintResults("MACD of RSI", dates,
            ("Rsi", runtime.GetSeries(rsi)),
            ("Macd", runtime.GetSeries(macdOfRsiPrimary)),
            ("Signal", runtime.GetSeries(macdOfRsiSignal)),
            ("Histogram", runtime.GetSeries(macdOfRsiHistogram)));
    }

    public void Dispose() => alpacaDataClient?.Dispose();

    private static void PrintResults(
        String title,
        List<DateTime> dates,
        params (String Name, IndicatorBuffer<Double> Values)[] series)
    {
        if (series.Length == 0 || dates.Count == 0)
        {
            return;
        }

        Console.WriteLine($"Printing {title} results for {symbol}:");
        Console.WriteLine();

        Console.WriteLine("Date\t\t" + String.Join("\t",
            series.Select(item => item.Name.PadRight(8))));

        for (var row = 0; row < dates.Count; row++)
        {
            var date = dates[row].ToShortDateString();
            var values = String.Join("\t\t",
                // ReSharper disable once AccessToModifiedClosure
                series.Select(item => item.Values[row].ToString("F2")));

            Console.WriteLine($"{date}\t{values}");
        }

        Console.WriteLine();
    }
}
