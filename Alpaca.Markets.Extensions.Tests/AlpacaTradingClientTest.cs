using Newtonsoft.Json.Linq;
using Xunit;

namespace Alpaca.Markets.Extensions.Tests;

[Collection("MockEnvironment")]
public sealed class AlpacaTradingClientTest
{
    private readonly MockClientsFactoryFixture _mockClientsFactory;

    private const Int32 Items = 5;

    public AlpacaTradingClientTest(
        MockClientsFactoryFixture mockClientsFactory) =>
        _mockClientsFactory = mockClientsFactory;

    [Fact]
    public async Task GetCalendarForSingleDayAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

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
    public async Task GetNewsArticlesAsAsyncEnumerableWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        addSinglePageExpectation(mock, Items);
        addSinglePageExpectation(mock);

        var counter = await validateList(
            mock.Client.ListAccountActivitiesAsAsyncEnumerable(
                new AccountActivitiesRequest()));

        Assert.NotEqual(0, counter);
    }

    private static void addSinglePageExpectation(
        MockClient<AlpacaTradingClientConfiguration, IAlpacaTradingClient> mock,
        Int32 count = 0) =>
        mock.AddGet("/v2/account/activities", new JArray(
            Enumerable.Repeat(createAccountActivity(), count)));

    private static JObject createAccountActivity() => new (
        new JProperty("activity_type", AccountActivityType.Fill),
        new JProperty("id", Guid.NewGuid().ToString("D")));

    private static async ValueTask<Int32> validateList<TItem>(
        IAsyncEnumerable<TItem> trades) =>
        await trades.CountAsync();
}
