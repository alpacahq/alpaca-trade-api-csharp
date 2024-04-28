namespace Alpaca.Markets.Tests;

public sealed class RequestValidationTest
{
    private static readonly Interval<DateTime> _interval = new();

    private const Decimal DecimalQuery = 42M;

    private const Int64 IntegerQuery = 42L;

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
        validate(new NewsArticlesRequest().WithPageSize(101)); // Pagination.MaxNewsPageSize + 1

    [Fact]
    public void HistoricalRequestBaseHugePageValidationWorks() =>
        validate(new HistoricalCryptoBarsRequest(Symbol, BarTimeFrame.Minute, _interval)
            .WithPageSize(UInt32.MaxValue));

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
        validate(new NewOrderRequest(String.Empty, OrderQuantity.Fractional(DecimalQuery),
            OrderSide.Buy, OrderType.Market, TimeInForce.Gtc));

    [Fact]
    public void NewOrderRequestNegativeQuantityValidationWorks() =>
        validate(new NewOrderRequest(Symbol, -IntegerQuery,
            OrderSide.Buy, OrderType.Market, TimeInForce.Gtc));

    [Fact]
    public void ChangeOrderRequestNegativeQuantityValidationWorks() =>
        validate(new ChangeOrderRequest(Guid.NewGuid()) { Quantity = -IntegerQuery });

    [Fact]
    public void OrderBaseEmptySymbolValidationWorks() =>
        validate(MarketOrder.Buy(String.Empty, OrderQuantity.Fractional(DecimalQuery)));

    [Fact]
    public void OrderBaseNegativeQuantityValidationWorks() =>
        validate(MarketOrder.Buy(Symbol, OrderQuantity.Fractional(-DecimalQuery)));

    [Fact]
    public void ListOrdersRequestEmptySymbolValidationWorks() =>
        validate(new ListOrdersRequest().WithSymbol(String.Empty));

    [Fact]
    public void PortfolioHistoryRequestEmptySymbolValidationWorks() =>
        validate(new PortfolioHistoryRequest
            {
                Period = new HistoryPeriod(1, HistoryPeriodUnit.Week)
            }
            .WithInterval(new Interval<DateTime>(DateTime.Today, DateTime.Today)));

    [Fact]
    public void OptionContactsRequestExpirationDateValidationWorks() =>
        validate(new OptionContractsRequest
        {
            ExpirationDateGreaterThanOrEqualTo = DateOnly.FromDateTime(DateTime.Today),
            ExpirationDateLessThanOrEqualTo = DateOnly.FromDateTime(DateTime.Today),
            ExpirationDateEqualTo = DateOnly.FromDateTime(DateTime.Today)
        });

    [Fact]
    public void OptionChainRequestExpirationDateValidationWorks() =>
        validate(new OptionChainRequest(Symbol)
        {
            ExpirationDateGreaterThanOrEqualTo = DateOnly.FromDateTime(DateTime.Today),
            ExpirationDateLessThanOrEqualTo = DateOnly.FromDateTime(DateTime.Today),
            ExpirationDateEqualTo = DateOnly.FromDateTime(DateTime.Today)
        });

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

        request.WithSymbols([Symbol]);

        Assert.NotNull(request.Symbols);
        Assert.NotEmpty(request.Symbols);
        Assert.Contains(Symbol, request.Symbols);
    }

    private static void validate<TRequest>(TRequest request) =>
        Assert.NotNull(Assert.Throws<RequestValidationException>(() => forceValidation(request)).PropertyName);

    private static void forceValidation<TRequest>(TRequest request)
    {
        var method = typeof(TRequest).GetInterfaces()
            .SelectMany(@interface => @interface.GetMethods())
            .Single(method => method.Name == "GetExceptions");

        if (method.Invoke(request, null) is not
            IEnumerable<RequestValidationException?> exceptions)
        {
            return;
        }

        var exception = exceptions.OfType<RequestValidationException>().First() ??
                        throw new InvalidOperationException();
        throw exception;
    }
}
