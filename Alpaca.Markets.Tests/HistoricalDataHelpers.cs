using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets.Tests;

[SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Global")]
[SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
[SuppressMessage("Usage", "xUnit1047:Avoid using TheoryDataRow arguments that might not be serializable")]
internal static class HistoricalDataHelpers
{
    private static readonly String _condition = Guid.NewGuid().ToString("D");

    private static readonly String _exchange = CryptoExchange.Ersx.ToString();

    private static readonly String _tape = Guid.NewGuid().ToString("D");

    private const Decimal MidPrice = (AskPrice + BidPrice) / 2;

    private const String Auctions = "auctions";

    private const UInt64 TradeId = 12_345UL;

    private const String Trades = "trades";

    private const String Quotes = "quotes";

    private const Int32 TradesNumber = 100;

    private const Decimal Wvap = 1_234.56M;

    private const Decimal AskPrice = 100M;

    private const Decimal BidPrice = 200M;

    private const Decimal Volume = 1_000M;

    private const Decimal Greeks = 0.01M;

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

    internal static void AddLatestCryptoBarsExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.addLatestCryptoExpectation(pathPrefix, symbols, Bars, CreateBar);

    internal static void AddSingleTradesPageExpectation(
        this IMock mock, String pathPrefix, String symbol) =>
        mock.addSinglePageExpectation(pathPrefix, symbol, Trades, CreateTrade);

    internal static void AddMultiTradesPageExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.addMultiPageExpectation(pathPrefix, symbols, Trades, CreateTrade);

    internal static void AddLatestTradeExpectation(
        this IMock mock, String pathPrefix, String symbol) =>
        mock.addLatestExpectation(pathPrefix, symbol, Trades, CreateTrade);

    internal static void AddLatestTradesExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.addLatestExpectation(pathPrefix, symbols, Trades, CreateTrade);

    internal static void AddLatestCryptoTradesExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.addLatestCryptoExpectation(pathPrefix, symbols, Trades, CreateTrade);

    internal static void AddSingleQuotesPageExpectation(
        this IMock mock, String pathPrefix, String symbol) =>
        mock.addSinglePageExpectation(pathPrefix, symbol, Quotes, CreateQuote);

    internal static void AddMultiQuotesPageExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.addMultiPageExpectation(pathPrefix, symbols, Quotes, CreateQuote);

    internal static void AddLatestQuoteExpectation(
        this IMock mock, String pathPrefix, String symbol) =>
        mock.addLatestExpectation(pathPrefix, symbol, Quotes, CreateQuote);

    internal static void AddLatestQuotesExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.addLatestExpectation(pathPrefix, symbols, Quotes, CreateQuote);

    internal static void AddLatestCryptoQuotesExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.addLatestCryptoExpectation(pathPrefix, symbols, Quotes, CreateQuote);

    internal static void AddSnapshotExpectation(
        this IMock mock, String pathPrefix, String symbol) =>
        mock.AddGet($"{pathPrefix}/{symbol}/snapshot", createSnapshot(symbol));

    internal static void AddSnapshotsExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.AddGet($"{pathPrefix}/snapshots", new JObject(
            symbols.Select(name => new JProperty(name, createSnapshot()))) );

    internal static void AddCryptoSnapshotsExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.AddGet($"{pathPrefix}/snapshots", new JObject(
            new JProperty("snapshots", new JObject(
                symbols.Select(name => new JProperty(name, createSnapshot()))))));

    internal static void AddOptionSnapshotsExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.AddGet($"{pathPrefix}/snapshots", new JObject(
            new JProperty("snapshots", new JObject(
                symbols.Select(name => new JProperty(name, createOptionSnapshot()))))));

    internal static void AddOptionChainExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.AddGet($"{pathPrefix}/snapshots/*", new JObject(
            new JProperty("snapshots", new JObject(
                symbols.Select(name => new JProperty(name, createOptionSnapshot()))))));

    internal static void AddOrderBooksExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.addLatestCryptoExpectation(pathPrefix, symbols, "orderbooks", createOrderBook);

    internal static void AddSingleAuctionsPageExpectation(
        this IMock mock, String pathPrefix, String symbol) =>
        mock.addSinglePageExpectation(pathPrefix, symbol, Auctions, createAuction);

    internal static void AddMultiAuctionsPageExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols) =>
        mock.addMultiPageExpectation(pathPrefix, symbols, Auctions, createAuction);

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
                symbols.Select(name => new JProperty(name, createItemsList(createItem)))))));

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
                symbols.Select(name => new JProperty(name, createItem()))))));

    private static void addLatestCryptoExpectation(
        this IMock mock, String pathPrefix, IEnumerable<String> symbols,
        String items, Func<JObject> createItem) =>
        mock.AddGet($"{pathPrefix}/latest/{items}", new JObject(
            new JProperty(items, new JObject(
                symbols.Select(name => new JProperty(name, createItem()))))));

    public static void Validate<TItem>(
        this IEnumerable<TItem> items,
        String symbol) =>
        Assert.True(items.All(item => item switch
        {
            IBar bar => bar.Validate(symbol),
            ITrade trade => trade.Validate(symbol),
            IQuote quote => quote.Validate(symbol),
            IAuction auction => auction.validate(symbol),
            IMarketMover mover => mover.validate(symbol),
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

        Assert.Equal(String.Empty, trade.Update);
        Assert.Equal(_exchange, trade.Exchange);

        Assert.Equal(TakerSide.Unknown, trade.TakerSide);
        Assert.Equal(MidPrice, trade.Price);
        Assert.Equal(Size, trade.Size);

        if (trade.TradeId != 0)
        {
            Assert.Equal(TradeId, trade.TradeId);
            Assert.Equal(_tape, trade.Tape);
        }
        else
        {
            Assert.Equal(String.Empty, trade.Tape);
        }

        return true;
    }

    public static Boolean Validate(
        this IQuote quote,
        String symbol)
    {
        Assert.True(quote.TimestampUtc <= DateTime.UtcNow);
        Assert.Equal(symbol, quote.Symbol);

        if (String.IsNullOrEmpty(quote.Tape)) // Crypto or option quote
        {
            Assert.Equal(String.Empty, quote.Tape);
            if (quote.Conditions.Any()) // Option quote
            {
                Assert.Equal(_condition, quote.Conditions.Single());
            }
            else
            {
                Assert.Empty(quote.Conditions);
            }
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
        this IOrderBook orderBook,
        String symbol)
    {
        Assert.NotNull(orderBook);
        Assert.Equal(String.Empty, orderBook.Exchange);
        Assert.True(orderBook.TimestampUtc <= DateTime.UtcNow);
        Assert.Equal(symbol, orderBook.Symbol);
        Assert.False(orderBook.IsReset);

        Assert.NotNull(orderBook.Asks);
        Assert.NotNull(orderBook.Bids);

        var ask = orderBook.Asks.Single();
        var bid = orderBook.Bids.Single();
        Assert.Equal(ask.Price, bid.Price);
        Assert.Equal(ask.Size, bid.Size);

        Assert.Equal(MidPrice, bid.Price);
        Assert.Equal(Size, ask.Size);

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

    public static JObject CreateBar() =>
        new(
            new JProperty("t", DateTime.UtcNow),
            new JProperty("n", TradesNumber),
            new JProperty("o", MidPrice),
            new JProperty("l", AskPrice),
            new JProperty("h", BidPrice),
            new JProperty("c", MidPrice),
            new JProperty("v", Volume),
            new JProperty("vw", Wvap));
    
    public static JObject CreateTrade() =>
        new(
            new JProperty("c", new JArray(_condition)),
            new JProperty("t", DateTime.UtcNow),
            new JProperty("u", String.Empty),
            new JProperty("x", _exchange),
            new JProperty("p", MidPrice),
            new JProperty("i", TradeId),
            new JProperty("z", _tape),
            new JProperty("tks", "-"),
            new JProperty("s", Size));

    public static JObject CreateQuote() =>
        new(
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

    public static JObject CreateCorrection(
        this String symbol) =>
        new(
            new JProperty(MessageDataHelpers.StreamingMessageTypeTag, "c"),
            new JProperty("oc", new JArray(_condition)),
            new JProperty("cc", new JArray(_condition)),
            new JProperty("t", DateTime.UtcNow),
            new JProperty("u", String.Empty),
            new JProperty("x", _exchange),
            new JProperty("op", MidPrice),
            new JProperty("cp", MidPrice),
            new JProperty("oi", TradeId),
            new JProperty("ci", TradeId),
            new JProperty("S", symbol),
            new JProperty("tks", "-"),
            new JProperty("z", _tape),
            new JProperty("cs", Size),
            new JProperty("os", Size));
    
    public static JObject CreateStockMover(
        this String symbol) =>
        new(
            new JProperty("percent_change", MidPrice),
            new JProperty("change", AskPrice),
            new JProperty("price", BidPrice),
            new JProperty("symbol", symbol));

    private static Boolean validate(
        this IAuction auction,
        String symbol)
    {
        Assert.NotNull(auction);
        Assert.True(auction.Date <= DateOnly.FromDateTime(DateTime.Today));
        Assert.Equal(symbol, auction.Symbol);

        Assert.NotNull(auction.Openings);
        Assert.NotNull(auction.Closings);

        var opening = auction.Openings.Single();
        var closing = auction.Closings.Single();
        Assert.Equal(opening.Price, closing.Price);
        Assert.Equal(opening.Size, closing.Size);

        Assert.Equal(MidPrice, closing.Price);
        Assert.Equal(Size, opening.Size);

        Assert.Equal(_exchange, opening.Exchange);
        Assert.Equal(_condition, closing.Condition);

        Assert.True(opening.TimestampUtc <= DateTime.UtcNow);
        return true;
    }

    private static Boolean validate(
        this IMarketMover mover,
        String symbol)
    {
        Assert.Equal(MidPrice, mover.PercentChange);
        Assert.Equal(AskPrice, mover.Change);
        Assert.Equal(BidPrice, mover.Price);
        Assert.Equal(symbol, mover.Symbol);

        return true;
    }

    private static JArray createItemsList(
        Func<JObject> createItem) => new(createItem(), createItem());

    private static JObject createOrderBook() =>
        new(
            new JProperty("a", new JArray(createOrderBookEntry())),
            new JProperty("b", new JArray(createOrderBookEntry())),
            new JProperty("t", DateTime.UtcNow),
            new JProperty("x", _exchange));
    
    private static JObject createOrderBookEntry() =>
        new(
            new JProperty("p", MidPrice),
            new JProperty("s", Size));

    private static JObject createAuction() =>
        new(
            new JProperty("o", new JArray(createAuctionEntry())),
            new JProperty("c", new JArray(createAuctionEntry())),
            new JProperty("d", DateTime.UtcNow));
    
    private static JObject createAuctionEntry() =>
        new(
            new JProperty("t", DateTime.UtcNow),
            new JProperty("c", _condition),
            new JProperty("x", _exchange),
            new JProperty("p", MidPrice),
            new JProperty("s", Size));

    private static JObject createSnapshot() =>
        createSnapshot(String.Empty);

    private static JObject createSnapshot(
        String symbol) =>
        new(
            new JProperty("latestQuote", CreateQuote()),
            new JProperty("latestTrade", CreateTrade()),
            new JProperty("prevDailyBar", CreateBar()),
            new JProperty("minuteBar", CreateBar()),
            new JProperty("dailyBar", CreateBar()),
            new JProperty("symbol", symbol));

    private static JObject createOptionSnapshot() =>
        new(
            new JProperty("latestQuote", CreateQuote()),
            new JProperty("latestTrade", CreateTrade()),
            new JProperty("impliedVolatility", Volume),
            new JProperty("greeks", createGreeks()),
            new JProperty("symbol", String.Empty));

    private static JObject createGreeks() =>
        new(
            new JProperty("delta", Greeks),
            new JProperty("gamma", Greeks),
            new JProperty("theta", Greeks),
            new JProperty("vega", Greeks),
            new JProperty("rho", Greeks));
}
