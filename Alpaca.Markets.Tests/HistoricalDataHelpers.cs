using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets.Tests;

[SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Global")]
internal static class HistoricalDataHelpers
{
    private static readonly String _condition = Guid.NewGuid().ToString("D");

    private static readonly String _exchange = CryptoExchange.Ersx.ToString();

    private static readonly String _tape = Guid.NewGuid().ToString("D");

    private const Decimal MidPrice = (AskPrice + BidPrice) / 2;

    private const UInt64 TradeId = 12_345UL;

    private const String Trades = "trades";

    private const String Quotes = "quotes";

    private const Int32 TradesNumber = 100;

    private const Decimal Wvap = 1_234.56M;

    private const Decimal AskPrice = 100M;

    private const Decimal BidPrice = 200M;

    private const Decimal Volume = 1_000M;

    private const String Bars = "bars";

    private const Decimal Size = 42M;

    internal static void AddSingleBarsPageExpectation(
        this IMock mock, String pathPrefix, String symbol) =>
        mock.addSinglePageExpectation(pathPrefix, symbol, Bars, CreateBar);

    internal static void AddMultiBarsPageExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.addMultiPageExpectation(pathPrefix, symbols, Bars, CreateBar);

    internal static void AddLatestBarExpectation(
        this IMock mock, String pathPrefix, String symbol) =>
        mock.addLatestExpectation(pathPrefix, symbol, Bars, CreateBar);

    internal static void AddLatestBarsExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.addLatestExpectation(pathPrefix, symbols, Bars, CreateBar);

    internal static void AddSingleTradesPageExpectation(
        this IMock mock, String pathPrefix, String symbol) =>
        mock.addSinglePageExpectation(pathPrefix, symbol, Trades, createTrade);

    internal static void AddMultiTradesPageExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.addMultiPageExpectation(pathPrefix, symbols, Trades, createTrade);

    internal static void AddLatestTradeExpectation(
        this IMock mock, String pathPrefix, String symbol) =>
        mock.addLatestExpectation(pathPrefix, symbol, Trades, createTrade);

    internal static void AddLatestTradesExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.addLatestExpectation(pathPrefix, symbols, Trades, createTrade);

    internal static void AddSingleQuotesPageExpectation(
        this IMock mock, String pathPrefix, String symbol) =>
        mock.addSinglePageExpectation(pathPrefix, symbol, Quotes, createQuote);

    internal static void AddMultiQuotesPageExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.addMultiPageExpectation(pathPrefix, symbols, Quotes, createQuote);

    internal static void AddLatestQuoteExpectation(
        this IMock mock, String pathPrefix, String symbol) =>
        mock.addLatestExpectation(pathPrefix, symbol, Quotes, createQuote);

    internal static void AddLatestQuotesExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.addLatestExpectation(pathPrefix, symbols, Quotes, createQuote);

    internal static void AddSnapshotExpectation(
        this IMock mock, String pathPrefix, String symbol) =>
        mock.AddGet($"{pathPrefix}/{symbol}/snapshot", createSnapshot(symbol));

    internal static void AddSnapshotsExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.AddGet($"{pathPrefix}/snapshots", new JObject(
            symbols.Select(_ => new JProperty(_, createSnapshot()))) );

    internal static void AddCryptoSnapshotsExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.AddGet($"{pathPrefix}/snapshots", new JObject(
            new JProperty("snapshots", new JObject(
                symbols.Select(_ => new JProperty(_, createSnapshot()))))));

    internal static void AddXbboExpectation(
        this IMock mock, String pathPrefix, String symbol) =>
        mock.AddGet($"{pathPrefix}/{symbol}/xbbo/latest", new JObject(
            new JProperty("xbbo", createQuote()),
            new JProperty(nameof(symbol), symbol)));

    internal static void AddXbbosExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.addLatestExpectation(pathPrefix, symbols, "xbbos", createQuote);

    private static void addSinglePageExpectation(
        this IMock mock, String pathPrefix, String symbol,
        String items, Func<JObject> createItem) =>
        mock.AddGet($"{pathPrefix}/{symbol}/{items}", new JObject(
            new JProperty(items, createItemsList(createItem)),
            new JProperty(nameof(symbol), symbol)));

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
            new JProperty(nameof(symbol), symbol)));

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
            IBar bar => bar.Validate(symbol),
            ITrade trade => trade.Validate(symbol),
            IQuote quote => quote.Validate(symbol),
            _ => throw new NotSupportedException(
                $"The {typeof(TItem)} not supported yet!")
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
        Assert.Equal(TradeId, trade.TradeId);
        Assert.Equal(MidPrice, trade.Price);
        Assert.Equal(Size, trade.Size);

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

        Assert.Equal(AskPrice, quote.AskPrice);
        Assert.Equal(BidPrice, quote.BidPrice);
        Assert.Equal(Size, quote.AskSize);
        Assert.Equal(Size, quote.BidSize);

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

    public static JObject CreateBar() =>
        new (
            new JProperty("t", DateTime.UtcNow),
            new JProperty("n", TradesNumber),
            new JProperty("o", MidPrice),
            new JProperty("l", AskPrice),
            new JProperty("h", BidPrice),
            new JProperty("c", MidPrice),
            new JProperty("v", Volume),
            new JProperty("vw", Wvap));

    private static JObject createTrade() =>
        new (
            new JProperty("c", new JArray(_condition)),
            new JProperty("t", DateTime.UtcNow),
            new JProperty("x", _exchange),
            new JProperty("p", MidPrice),
            new JProperty("i", TradeId),
            new JProperty("z", _tape),
            new JProperty("s", Size));

    private static JObject createQuote() =>
        new (
            new JProperty("c", new JArray(_condition)),
            new JProperty("t", DateTime.UtcNow),
            new JProperty("ax", _exchange),
            new JProperty("bx", _exchange),
            new JProperty("ap", AskPrice),
            new JProperty("bp", BidPrice),
            new JProperty("x", _exchange),
            new JProperty("as", Size),
            new JProperty("bs", Size),
            new JProperty("z", _tape));

    private static JObject createSnapshot() =>
        createSnapshot(String.Empty);

    private static JObject createSnapshot(
        String symbol) =>
        new (
            new JProperty("latestQuote", createQuote()),
            new JProperty("latestTrade", createTrade()),
            new JProperty("prevDailyBar", CreateBar()),
            new JProperty("minuteBar", CreateBar()),
            new JProperty("dailyBar", CreateBar()),
            new JProperty("symbol", symbol));
}
