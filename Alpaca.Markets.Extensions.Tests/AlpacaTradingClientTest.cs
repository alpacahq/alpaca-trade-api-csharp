namespace Alpaca.Markets.Extensions.Tests;

[Collection("MockEnvironment")]
public sealed class AlpacaTradingClientTest(
    MockClientsFactoryFixture mockClientsFactory)
{
    private const Decimal Price = 12_345.67M;

    private const String Symbol = "AAPL";

    private const Int32 Items = 5;

    [Fact]
    public async Task GetCalendarForSingleDayAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        var today = DateOnly.FromDateTime(DateTime.Now);
        mock.AddGet("/v2/calendar", new JArray(
            new JObject(
                new JProperty("date", today.ToString("O")),
                new JProperty("open", "09:30"),
                new JProperty("close", "16:00"),
                new JProperty("session_open", "08:00"),
                new JProperty("session_close", "18:00"))));

        var calendar = await mock.Client
            .GetCalendarForSingleDayAsync(today);

        Assert.NotNull(calendar);
    }

    [Fact]
    public async Task ListAccountActivitiesAsAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        addSinglePageExpectationOfAccountActivities(mock, Items);
        addSinglePageExpectationOfAccountActivities(mock);

        var counter = await validateList(
            mock.Client.ListAccountActivitiesAsAsyncEnumerable(
                new AccountActivitiesRequest()));

        Assert.NotEqual(0, counter);
    }

    [Fact]
    public async Task ListOptionContractsAsAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        addSinglePageExpectationOfOptionContracts(mock, Guid.NewGuid().ToString("N"));
        addSinglePageExpectationOfOptionContracts(mock);

        var counter = await validateList(
            mock.Client.ListOptionContractsAsAsyncEnumerable(new OptionContractsRequest()));

        Assert.NotEqual(0, counter);
    }

    [Fact]
    public async Task IsMarketOpenAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        var tomorrow = DateTime.Today.AddDays(1);
        var dayAfterTomorrow = tomorrow.AddDays(1);

        addClock(mock, DateTime.UtcNow, tomorrow);
        addClock(mock, tomorrow, DateTime.UtcNow);
        addClock(mock, tomorrow, dayAfterTomorrow);

        Assert.True(await mock.Client.IsMarketOpenAsync());
        Assert.False(await mock.Client.IsMarketOpenAsync());
        Assert.True(await mock.Client.IsMarketOpenAsync());
        Assert.True(await mock.Client.IsMarketOpenAsync());

        Assert.NotNull(await mock.Client.GetClockCachedAsync());
    }

    private static void addSinglePageExpectationOfAccountActivities(
        MockClient<AlpacaTradingClientConfiguration, IAlpacaTradingClient> mock,
        Int32 count = 0) =>
        mock.AddGet("/v2/account/activities", new JArray(
            Enumerable.Repeat(createAccountActivity(), count)));

    private static JObject createAccountActivity() => new(
        new JProperty("activity_type", AccountActivityType.Fill),
        new JProperty("id", Guid.NewGuid().ToString("D")));

    private static void addSinglePageExpectationOfOptionContracts(
        MockClient<AlpacaTradingClientConfiguration, IAlpacaTradingClient> mock,
        String? token = null) =>
        mock.AddGet("/v2/options/contracts", new JObject(
            new JProperty("option_contracts", new JArray(
                Enumerable.Repeat(createOptionContract(Guid.NewGuid()), Items))),
            new JProperty("next_page_token", token)));

    private static JObject createOptionContract(
        Guid contractId) =>
        new(
            new JProperty("expiration_date", DateOnly.FromDateTime(DateTime.Today).ToString("O")),
            new JProperty("underlying_asset_id", contractId),
            new JProperty("style", OptionStyle.American),
            new JProperty("status", AssetStatus.Active),
            new JProperty("underlying_symbol", Symbol),
            new JProperty("type", OptionType.Call),
            new JProperty("root_symbol", Symbol),
            new JProperty("strike_price", Price),
            new JProperty("tradable", true),
            new JProperty("symbol", Symbol),
            new JProperty("id", contractId),
            new JProperty("name", Symbol),
            new JProperty("size", 100));

    private static async ValueTask<Int32> validateList<TItem>(
        IAsyncEnumerable<TItem> trades) =>
        await trades.CountAsync();

    private static void addClock(
        MockClient<AlpacaTradingClientConfiguration, IAlpacaTradingClient> mock,
        DateTime nextClose,
        DateTime nextOpen) =>
        mock.AddGet("/v2/clock", new JObject(
            new JProperty("is_open", nextClose < nextOpen),
            new JProperty("timestamp", DateTime.UtcNow),
            new JProperty("next_close", nextClose),
            new JProperty("next_open", nextOpen)));
}
