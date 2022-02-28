using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets.Tests;

[SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Global")]
internal static class HistoricalDataHelpers
{
    private static readonly String _condition = Guid.NewGuid().ToString("D");

    private static readonly String _exchange = CryptoExchange.Ersx.ToString();

    private static readonly String _tape = Guid.NewGuid().ToString("D");

    public static void AddSingleBarsPageExpectation(
        this IMock mock, String pathPrefix, String symbol) =>
        mock.addSinglePageExpectation(pathPrefix, symbol, "bars", createBar);

    public static void AddMultiBarsPageExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.addMultiPageExpectation(pathPrefix, symbols, "bars", createBar);

    public static void AddLatestBarExpectation(
        this IMock mock, String pathPrefix, String symbol) =>
        mock.addLatestExpectation(pathPrefix, symbol, "bars", createBar);

    public static void AddLatestBarsExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.addLatestExpectation(pathPrefix, symbols, "bars", createBar);

    public static void AddSingleTradesPageExpectation(
        this IMock mock, String pathPrefix, String symbol) =>
        mock.addSinglePageExpectation(pathPrefix, symbol, "trades", createTrade);

    public static void AddMultiTradesPageExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.addMultiPageExpectation(pathPrefix, symbols, "trades", createTrade);

    public static void AddLatestTradeExpectation(
        this IMock mock, String pathPrefix, String symbol) =>
        mock.addLatestExpectation(pathPrefix, symbol, "trades", createTrade);

    public static void AddLatestTradesExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.addLatestExpectation(pathPrefix, symbols, "trades", createTrade);

    public static void AddSingleQuotesPageExpectation(
        this IMock mock, String pathPrefix, String symbol) =>
        mock.addSinglePageExpectation(pathPrefix, symbol, "quotes", createQuote);

    public static void AddMultiQuotesPageExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.addMultiPageExpectation(pathPrefix, symbols, "quotes", createQuote);

    public static void AddLatestQuoteExpectation(
        this IMock mock, String pathPrefix, String symbol) =>
        mock.addLatestExpectation(pathPrefix, symbol, "quotes", createQuote);

    public static void AddLatestQuotesExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.addLatestExpectation(pathPrefix, symbols, "quotes", createQuote);

    public static void AddSnapshotExpectation(
        this IMock mock, String pathPrefix, String symbol) =>
        mock.AddGet($"{pathPrefix}/{symbol}/snapshot", createSnapshot(symbol));

    public static void AddSnapshotsExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.AddGet($"{pathPrefix}/snapshots", new JObject(
            symbols.Select(_ => new JProperty(_, createSnapshot()))) );

    public static void AddXbboExpectation(
        this IMock mock, String pathPrefix, String symbol) =>
        mock.AddGet($"{pathPrefix}/{symbol}/xbbo/latest", new JObject(
            new JProperty("xbbo", createQuote()),
            new JProperty("symbol", symbol)));

    private static void addSinglePageExpectation(
        this IMock mock, String pathPrefix, String symbol,
        String items, Func<JObject> createItem) =>
        mock.AddGet($"{pathPrefix}/{symbol}/{items}", new JObject(
            new JProperty(items, createItemsList(createItem)),
            new JProperty("symbol", symbol)));

    private static void addMultiPageExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols,
        String items, Func<JObject> createItem) =>
        mock.AddGet($"{pathPrefix}/{items}", new JObject(
            new JProperty(items, new JObject(
                symbols.Select(_ => new JProperty(_, createItemsList(createItem)))))));

    private static void addLatestExpectation(
        this IMock mock, String pathPrefix, String symbol,
        String items, Func<JObject> createItem) =>
        mock.AddGet($"{pathPrefix}/{symbol}/{items}/latest", new JObject(
            new JProperty(items[..^1], createItem()), // Without last `s` character
            new JProperty("symbol", symbol)));

    private static void addLatestExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols,
        String items, Func<JObject> createItem) =>
        mock.AddGet($"{pathPrefix}/{items}/latest", new JObject(
            new JProperty(items, new JObject(
                symbols.Select(_ => new JProperty(_, createItem()))))));

    public static void Validate<TItem>(
        this IEnumerable<TItem> items,
        String symbol) =>
        Assert.True(items.All(_ => _ switch
        {
            ITrade trade => trade.Validate(symbol),
            IQuote quote => quote.Validate(symbol),
            IBar bar => bar.Validate(symbol),
            _ => throw new NotSupportedException()
        }));

    public static Boolean Validate(
        this IBar bar,
        String symbol)
    {
        Assert.True(bar.TimeUtc <= DateTime.UtcNow);
        Assert.Equal(symbol, bar.Symbol);

        Assert.InRange(bar.Close, bar.Low, bar.High);
        Assert.InRange(bar.Open, bar.Low, bar.High);

        Assert.True(bar.TimeUtc <= DateTime.UtcNow);
        Assert.True(bar.TradeCount != 0);
        Assert.True(bar.Volume != 0M);
        Assert.True(bar.Vwap != 0M);

        return true;
    }
    
    public static Boolean Validate(
        this ITrade trade,
        String symbol)
    {
        Assert.True(trade.TimestampUtc <= DateTime.UtcNow);
        Assert.Equal(symbol, trade.Symbol);

        Assert.NotEmpty(trade.Conditions);
        Assert.Equal(_condition, trade.Conditions.Single());

        Assert.Equal(_exchange, trade.Exchange);
        Assert.Equal(_tape, trade.Tape);

        Assert.Equal(TakerSide.Unknown, trade.TakerSide);
        Assert.Equal(12345UL, trade.TradeId);
        Assert.Equal(100M, trade.Price);
        Assert.Equal(42M, trade.Size);

        return true;
    }

    public static Boolean Validate(
        this IQuote quote,
        String symbol)
    {
        Assert.True(quote.TimestampUtc <= DateTime.UtcNow);
        Assert.Equal(symbol, quote.Symbol);

        if (String.IsNullOrEmpty(quote.Tape)) // Crypto quote
        {
            Assert.Equal(String.Empty, quote.Tape);
            Assert.Empty(quote.Conditions);
        }
        else
        {
            Assert.Equal(_condition, quote.Conditions.Single());
            Assert.Equal(_tape, quote.Tape);
        }

        Assert.Equal(_exchange, quote.AskExchange);
        Assert.Equal(_exchange, quote.BidExchange);

        Assert.Equal(100M, quote.AskPrice);
        Assert.Equal(200M, quote.BidPrice);
        Assert.Equal(42M, quote.AskSize);
        Assert.Equal(42M, quote.BidSize);

        return true;
    }

    public static Boolean Validate(
        this ISnapshot snapshot,
        String symbol) =>
        snapshot.PreviousDailyBar!.Validate(symbol) &
        snapshot.CurrentDailyBar!.Validate(symbol) &
        snapshot.MinuteBar!.Validate(symbol) &
        snapshot.Quote!.Validate(symbol) &
        snapshot.Trade!.Validate(symbol);

    private static JArray createItemsList(
        Func<JObject> createItem) => new (createItem(), createItem());

    private static JObject createBar() =>
        new (
            new JProperty("t", DateTime.UtcNow),
            new JProperty("vw", 1234.56M),
            new JProperty("n", 100),
            new JProperty("v", 1000M),
            new JProperty("o", 100M),
            new JProperty("l", 90M),
            new JProperty("h", 120M),
            new JProperty("c", 110M));

    private static JObject createTrade() =>
        new (
            new JProperty("c", new JArray(_condition)),
            new JProperty("t", DateTime.UtcNow),
            new JProperty("x", _exchange),
            new JProperty("i", 12345UL),
            new JProperty("z", _tape),
            new JProperty("p", 100M),
            new JProperty("s", 42));

    private static JObject createQuote() =>
        new (
            new JProperty("c", new JArray(_condition)),
            new JProperty("t", DateTime.UtcNow),
            new JProperty("ax", _exchange),
            new JProperty("bx", _exchange),
            new JProperty("ap", 100M),
            new JProperty("bp", 200M),
            new JProperty("as", 42M),
            new JProperty("bs", 42M),
            new JProperty("x", _exchange),
            new JProperty("z", _tape));

    private static JObject createSnapshot(
        String symbol = "") =>
        new (
            new JProperty("latestQuote", createQuote()),
            new JProperty("latestTrade", createTrade()),
            new JProperty("prevDailyBar", createBar()),
            new JProperty("minuteBar", createBar()),
            new JProperty("dailyBar", createBar()),
            new JProperty("symbol", symbol));
}
