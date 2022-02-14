namespace Alpaca.Markets.Tests;

public sealed class RequestValidationTest
{
    private static readonly Interval<DateTime> _interval = new ();

    private const String Symbol = "AAPL";

    [Fact]
    public void HistoricalRequestBaseNoSymbolsValidationWorks() =>
        validate(new HistoricalCryptoBarsRequest(Array.Empty<String>(), _interval, BarTimeFrame.Day));

    [Fact]
    public void HistoricalRequestBaseEmptySymbolValidationWorks() =>
        validate(new HistoricalCryptoBarsRequest(new [] { String.Empty }, _interval, BarTimeFrame.Day));

    [Fact]
    public void HistoricalRequestBaseEmptyPageValidationWorks() =>
        validate(new HistoricalCryptoBarsRequest(Symbol, BarTimeFrame.Minute, _interval)
                .WithPageSize(UInt32.MinValue));

    [Fact]
    public void NewsArticlesRequestEmptySymbolValidationWorks() =>
        validate(new NewsArticlesRequest(new [] { String.Empty }));

    [Fact]
    public void NewsArticlesRequestBigPageValidationWorks() =>
        validate(new NewsArticlesRequest().WithPageSize(Pagination.MaxNewsPageSize + 1));

    [Fact]
    public void HistoricalRequestBaseHugePageValidationWorks() =>
        validate(new HistoricalCryptoBarsRequest(Symbol, BarTimeFrame.Minute, _interval)
            .WithPageSize(UInt32.MaxValue));

    [Fact]
    public void SnapshotDataRequestEmptySymbolValidationWorks() =>
        validate(new SnapshotDataRequest(String.Empty, CryptoExchange.Ersx));

    [Fact]
    public void LatestDataRequestEmptySymbolValidationWorks() =>
        validate(new LatestDataRequest(String.Empty, CryptoExchange.Cbse));

    [Fact]
    public void LatestBestBidOfferRequestEmptySymbolValidationWorks() =>
        validate(new LatestBestBidOfferRequest(String.Empty, CryptoExchange.Gnss));

    [Fact]
    public void DeletePositionRequestEmptySymbolValidationWorks() =>
        validate(new DeletePositionRequest(String.Empty));

    [Fact]
    public void NewWatchListRequestEmptyNameValidationWorks() =>
        validate(new NewWatchListRequest(String.Empty, Array.Empty<String>()));

    [Fact]
    public void NewWatchListRequestEmptySymbolValidationWorks() =>
        validate(new NewWatchListRequest(Guid.NewGuid().ToString("D"), new [] { String.Empty}));

    [Fact]
    public void UpdateWatchListRequestEmptyNameValidationWorks() =>
        validate(new UpdateWatchListRequest(Guid.NewGuid(), String.Empty, Array.Empty<String>()));

    [Fact]
    public void UpdateWatchListRequestEmptySymbolValidationWorks() =>
        validate(new UpdateWatchListRequest(Guid.NewGuid(),
            Guid.NewGuid().ToString("D"), new [] { String.Empty}));

    [Fact]
    public void ChangeWatchListRequestEmptyNameValidationWorks() =>
        validate(new ChangeWatchListRequest<String>(String.Empty, Symbol));

    [Fact]
    public void ChangeWatchListRequestEmptySymbolValidationWorks() =>
        validate(new ChangeWatchListRequest<Guid>(Guid.NewGuid(), String.Empty));

    [Fact]
    public void NewOrderRequestEmptySymbolValidationWorks() =>
        validate(new NewOrderRequest(String.Empty, OrderQuantity.Fractional(12.34M),
            OrderSide.Buy, OrderType.Market, TimeInForce.Gtc));

    [Fact]
    public void NewOrderRequestNegativeQuantityValidationWorks() =>
        validate(new NewOrderRequest(Symbol, -42L,
            OrderSide.Buy, OrderType.Market, TimeInForce.Gtc));

    [Fact]
    public void ChangeOrderRequestNegativeQuantityValidationWorks() =>
        validate(new ChangeOrderRequest(Guid.NewGuid()) { Quantity = -42L });

    [Fact]
    public void OrderBaseEmptySymbolValidationWorks() =>
        validate(new MarketOrder(String.Empty, OrderQuantity.Fractional(42M), OrderSide.Buy));

    [Fact]
    public void OrderBaseNegativeQuantityValidationWorks() =>
        validate(new MarketOrder(Symbol, OrderQuantity.Fractional(-42M), OrderSide.Buy));

    [Fact]
    public void ListOrdersRequestEmptySymbolValidationWorks() =>
        validate(new ListOrdersRequest().WithSymbol(String.Empty));

    [Fact]
    public void CalendarRequestConstructorWorks()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var yesterday = today.AddDays(-1);

        var request1 = new CalendarRequest().WithInterval(new Interval<DateOnly>(yesterday, today));
        var request2 = new CalendarRequest(yesterday, today);

        Assert.Equal(request1.DateInterval, request2.DateInterval);
    }

    [Fact]
    public void AccountActivitiesRequestConstructorWorks()
    {
        var request1 = new AccountActivitiesRequest().WithInterval(new Interval<DateTime>());
        var request2 = new AccountActivitiesRequest(Enum.GetValues<AccountActivityType>());

        Assert.NotNull(request1.ActivityTypes);
        Assert.NotNull(request2.ActivityTypes);

        Assert.NotEmpty(request2.ActivityTypes);
        Assert.Empty(request1.ActivityTypes);

        Assert.Equal(request1.TimeInterval, request2.TimeInterval);
    }

    [Fact]
    public void ListOrdersRequestWithSymbolsWorks()
    {
        var request = new ListOrdersRequest();

        Assert.NotNull(request.Symbols);
        Assert.Empty(request.Symbols);

        request.WithSymbols(new[] { Symbol });

        Assert.NotNull(request.Symbols);
        Assert.NotEmpty(request.Symbols);
        Assert.Contains(Symbol, request.Symbols);
    }

    private static void validate(Validation.IRequest request) =>
        Assert.NotNull(Assert.Throws<RequestValidationException>(request.Validate).PropertyName);
}
