using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets.Tests;

[SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Global")]
internal static class MessageDataHelpers
{
    private static readonly String _exchange = CryptoExchange.Cbse.ToString();

    public const String StreamingMessageTypeTag = "T";

    private const Int64 IntegerQuantity = 123L;

    private const Decimal Quantity = 123.45M;

    private const Decimal Price = 100M;

    private const Decimal Size = 10M;

    public static JToken CreateMarketOrder(
        this String symbol) =>
        new JObject(
            new JProperty("status", OrderStatus.PartiallyFilled),
            new JProperty("asset_class", AssetClass.UsEquity),
            new JProperty("time_in_force", TimeInForce.Day),
            new JProperty("order_class", OrderClass.Simple),
            new JProperty("asset_id", Guid.NewGuid()),
            new JProperty("type", OrderType.Market),
            new JProperty("side", OrderSide.Sell),
            new JProperty("filled_qty", Quantity),
            new JProperty("id", Guid.NewGuid()),
            new JProperty("symbol", symbol),
            new JProperty("qty", Quantity),
            new JProperty("legs"));

    public static JObject CreateNewsArticle(
        this String symbol) =>
        new(
            new JProperty("images", new JArray(
                new JObject(
                    new JProperty("url", new Uri("https://www.google.com")),
                    new JProperty("size", "large")))),
            new JProperty("headline", Guid.NewGuid().ToString("D")),
            new JProperty("content", Guid.NewGuid().ToString("D")),
            new JProperty("summary", Guid.NewGuid().ToString("D")),
            new JProperty("source", Guid.NewGuid().ToString("D")),
            new JProperty("author", Guid.NewGuid().ToString("D")),
            new JProperty("url", new Uri("https://www.google.com")),
            new JProperty("id", Random.Shared.NextInt64()),
            new JProperty("created_at", DateTime.UtcNow),
            new JProperty("updated_at", DateTime.UtcNow),
            new JProperty("symbols", new JArray(symbol)));

    public static JObject CreateOrderBook(
        this String symbol) =>
        new(
            new JProperty("a", new JArray(createOrderBookEntry())),
            new JProperty("b", new JArray(createOrderBookEntry())),
            new JProperty(StreamingMessageTypeTag, "o"),
            new JProperty("t", DateTime.UtcNow),
            new JProperty("x", _exchange),
            new JProperty("S", symbol),
            new JProperty("r", true));

    public static JObject CreateStreamingNewsArticle(
        this String symbol) =>
        CreateNewsArticle(symbol)
            .addStreamingProperties("n");

    public static JObject CreateStreamingBar(
        this String symbol,
        String channel) =>
        HistoricalDataHelpers.CreateBar()
            .addStreamingProperties(channel, symbol);

    public static JObject CreateStreamingTrade(
        this String symbol,
        String channel) =>
        HistoricalDataHelpers.CreateTrade()
            .addStreamingProperties(channel, symbol);

    public static JObject CreateStreamingQuote(
        this String symbol) =>
        HistoricalDataHelpers.CreateQuote()
            .addStreamingProperties("q", symbol);

    public static void Validate(
        this IOrder order,
        String symbol)
    {
        Assert.NotNull(order);

        Assert.NotEqual(Guid.Empty, order.AssetId);
        Assert.NotEqual(Guid.Empty, order.OrderId);
        Assert.Equal(symbol, order.Symbol);

        Assert.Equal(IntegerQuantity, order.IntegerFilledQuantity);
        Assert.Equal(IntegerQuantity, order.IntegerQuantity);
        Assert.True(order.GetOrderQuantity().IsInShares);

        Assert.Null(order.TrailOffsetInPercent);
        Assert.Null(order.TrailOffsetInDollars);
        Assert.Null(order.ReplacedByOrderId);
        Assert.Null(order.AverageFillPrice);
        Assert.Null(order.ReplacesOrderId);
        Assert.Null(order.ClientOrderId);
        Assert.Null(order.HighWaterMark);
        Assert.Null(order.LimitPrice);
        Assert.Null(order.StopPrice);
        Assert.Null(order.Notional);

        Assert.Null(order.SubmittedAtUtc);
        Assert.Null(order.CancelledAtUtc);
        Assert.Null(order.ReplacedAtUtc);
        Assert.Null(order.CreatedAtUtc);
        Assert.Null(order.UpdatedAtUtc);
        Assert.Null(order.ExpiredAtUtc);
        Assert.Null(order.FilledAtUtc);
        Assert.Null(order.FailedAtUtc);

        Assert.Empty(order.Legs);
    }

    public static void Validate(
        this INewsArticle article,
        String symbol)
    {
        Assert.NotNull(article);
        Assert.NotEqual(0L, article.Id);

        Assert.Equal(symbol, article.Symbols.Single());

        Assert.NotNull(article.ArticleUrl);
        Assert.NotNull(article.Headline);
        Assert.NotNull(article.Content);
        Assert.NotNull(article.Summary);
        Assert.NotNull(article.Author);
        Assert.NotNull(article.Source);

        Assert.NotNull(article.LargeImageUrl);
        Assert.Null(article.SmallImageUrl);
        Assert.Null(article.ThumbImageUrl);
    }

    public static void Validate(
        this IOrderBook orderBook,
        String symbol)
    {
        Assert.NotNull(orderBook);
        Assert.True(orderBook.IsReset);
        Assert.Equal(symbol, orderBook.Symbol);
        Assert.Equal(_exchange, orderBook.Exchange);

        Assert.NotNull(orderBook.Asks);
        Assert.NotNull(orderBook.Bids);

        var ask = orderBook.Asks.Single();
        var bid = orderBook.Asks.Single();
        Assert.Equal(ask.Price, bid.Price);
        Assert.Equal(ask.Size, bid.Size);

        Assert.Equal(Price, bid.Price);
        Assert.Equal(Size, ask.Size);
    }

    private static JObject createOrderBookEntry() =>
        new(
            new JProperty("p", Price),
            new JProperty("s", Size));

    private static JObject addStreamingProperties(
        this JObject message,
        String channel,
        String? symbol = null)
    {
        message.Add(StreamingMessageTypeTag, channel);
        if (symbol is not null)
        {
            message.Add("S", symbol);
        }
        return message;
    }
}
