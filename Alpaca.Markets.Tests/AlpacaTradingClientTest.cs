using System.Globalization;

namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed partial class AlpacaTradingClientTest(
    MockClientsFactoryFixture mockClientsFactory)
{
    private const String Crypto = "BTCUSD";

    private const String Stock = "AAPL";

    [Fact]
    public void AlpacaTradingClientConfigurationValidationWorks()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var nullSecurityId = new AlpacaTradingClientConfiguration { SecurityId = null };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<InvalidOperationException>(() =>
            mockClientsFactory.GetAlpacaTradingClientMock(Environments.Paper, nullSecurityId));

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var nullApiEndpoint = new AlpacaTradingClientConfiguration { ApiEndpoint = null };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<InvalidOperationException>(() =>
            mockClientsFactory.GetAlpacaTradingClientMock(Environments.Paper, nullApiEndpoint));

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var nullThrottleParameters = new AlpacaTradingClientConfiguration { ThrottleParameters = null };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<InvalidOperationException>(() =>
            mockClientsFactory.GetAlpacaTradingClientMock(Environments.Paper, nullThrottleParameters));
    }

    [Fact]
    public async Task ListIntervalCalendarAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock(Environments.Paper);

        var today = DateOnly.FromDateTime(DateTime.Today);

        mock.AddGet("/v2/calendar", new JArray(
            new JObject(
                new JProperty("session_close", new TimeOnly(20, 00).ToString("O", CultureInfo.InvariantCulture)),
                new JProperty("session_open", new TimeOnly(08, 00).ToString("O", CultureInfo.InvariantCulture)),
                new JProperty("close", new TimeOnly(18, 00).ToString("O", CultureInfo.InvariantCulture)),
                new JProperty("open", new TimeOnly(10, 30).ToString("O", CultureInfo.InvariantCulture)),
                new JProperty("date", today.ToString("O", CultureInfo.InvariantCulture)))));

        var calendars = await mock.Client
            .ListIntervalCalendarAsync(CalendarRequest.GetForSingleDay(today));

        var calendar = calendars.Single();
        Assert.Equal(today, calendar.GetTradingDate());

        Assert.InRange(calendar.GetTradingOpenTimeUtc(),
            calendar.GetSessionOpenTimeUtc(), calendar.GetSessionCloseTimeUtc());
        Assert.InRange(calendar.GetTradingOpenTimeEst(),
            calendar.GetSessionOpenTimeEst(), calendar.GetSessionCloseTimeEst());
        Assert.InRange(calendar.GetTradingCloseTimeUtc(),
            calendar.GetSessionOpenTimeUtc(), calendar.GetSessionCloseTimeUtc());
        Assert.InRange(calendar.GetTradingCloseTimeEst(),
            calendar.GetSessionOpenTimeEst(), calendar.GetSessionCloseTimeEst());

        Interval<DateTime> tradingInterval = calendar.Trading;
        var (open, close) = calendar.Session.ToInterval();

        Assert.False(tradingInterval.IsOpen());
        Assert.False(tradingInterval.IsEmpty());

        Assert.InRange(tradingInterval.From!.Value, open!.Value, close!.Value);
        Assert.InRange(tradingInterval.Into!.Value, open.Value, close.Value);

        Assert.NotNull(JsonConvert.SerializeObject(calendar));
    }

    [Fact]
    public async Task GetClockAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet("/v2/clock", new JObject(
            new JProperty("next_close", DateTime.Today.AddDays(2)),
            new JProperty("next_open", DateTime.Today.AddDays(1)),
            new JProperty("timestamp", DateTime.UtcNow),
            new JProperty("is_open", true)));

        var clock = await mock.Client.GetClockAsync();

        Assert.True(clock.IsOpen);
        Assert.True(clock.TimestampUtc <= DateTime.UtcNow);
        Assert.True(clock.NextOpenUtc < clock.NextCloseUtc);

        Assert.NotNull(JsonConvert.SerializeObject(clock));
    }

    [Fact]
    public async Task GetRateLimitValuesWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        var oldLimits = mock.Client.GetRateLimitValues();

        Assert.Equal(0, oldLimits.Limit);
        Assert.Equal(0, oldLimits.Remaining);
        Assert.Equal(new DateTime(), oldLimits.ResetTimeUtc);

        var resetTime = DateTimeOffset.FromUnixTimeSeconds(
            new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds());

        mock.AddGet("/v2/clock", new JObject(
            new JProperty("next_close", DateTime.Today.AddDays(2)),
            new JProperty("next_open", DateTime.Today.AddDays(1)),
            new JProperty("timestamp", DateTime.UtcNow),
            new JProperty("is_open", true)).ToString(),
            HttpStatusCode.OK, GetHeaders().ToArray());

        await mock.Client.GetClockAsync();

        var newLimits = mock.Client.GetRateLimitValues();

        Assert.Equal(100, newLimits.Limit);
        Assert.Equal(99, newLimits.Remaining);
        Assert.Equal(resetTime.UtcDateTime, newLimits.ResetTimeUtc);

        return;

        IEnumerable<KeyValuePair<String, String>> GetHeaders() =>
            new KeyValuePair<String, String>[]
            {
                new("X-Ratelimit-Limit", "100"),
                new("X-Ratelimit-Remaining", "99"),
                new("X-Ratelimit-Reset",
                    resetTime.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture))
            };
    }
}
