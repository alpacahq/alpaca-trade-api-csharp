using System.Net.Http.Headers;
using System.Net.Sockets;

namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed class ThrottleParametersTest(
    MockClientsFactoryFixture mockClientsFactory)
{
    private const String OrdersUrl = "/v2/orders/**";

    private const String ClockUrl = "/v2/clock";

    private const String Message = "Fahrenheit";

    private const Int32 ErrorCode = 451;

    [Fact]
    public async Task ThrottlingWithErrorMessageWorks()
    {
        const String symbol = "AAPL";
        const Decimal money = 1_000M;
        const Int32 orders = 10;

        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        var errorMessage = new JObject(
            new JProperty("day_trading_buying_power", money),
            new JProperty("max_dtbp_used_so_far", money),
            new JProperty("open_orders", orders),
            new JProperty("max_dtbp_used", money),
            new JProperty("message", Message),
            new JProperty("symbol", symbol),
            new JProperty("code", ErrorCode)).ToString();

        var delay = TimeSpan.FromMilliseconds(100);

        mock.AddGet(ClockUrl, errorMessage, HttpStatusCode.TooManyRequests);
        mock.AddGet(ClockUrl, errorMessage, HttpStatusCode.TooManyRequests,
            AsHeader(new RetryConditionHeaderValue(DateTimeOffset.UtcNow)));
        mock.AddGet(ClockUrl, errorMessage, HttpStatusCode.TooManyRequests,
            AsHeader(new RetryConditionHeaderValue(delay)));
        mock.AddGet(ClockUrl, errorMessage, HttpStatusCode.BadGateway);
        mock.AddGet(ClockUrl, errorMessage, HttpStatusCode.GatewayTimeout);
        mock.AddGet(ClockUrl, errorMessage, HttpStatusCode.ServiceUnavailable);

        var exception = await Assert.ThrowsAsync<RestClientErrorException>(
            () => mock.Client.GetClockAsync());

        Assert.Equal(Message,exception.Message);
        Assert.Equal(ErrorCode, exception.ErrorCode);
        Assert.Equal(HttpStatusCode.ServiceUnavailable, exception.HttpStatusCode);

        var info = exception.ErrorInformation;

        Assert.NotNull(info);
        Assert.Equal(symbol, info.Symbol);
        Assert.Equal(orders, info.OpenOrdersCount);
        Assert.Equal(money, info.DayTradingBuyingPower);
        Assert.Equal(money, info.MaxDayTradingBuyingPowerUsed);
        Assert.Equal(money, info.MaxDayTradingBuyingPowerUsedSoFar);
        return;

        KeyValuePair<String, String> AsHeader(
            RetryConditionHeaderValue value) =>
            new("Retry-After", value.ToString());
    }

    [Fact]
    public async Task ThrottlingWithSocketErrorsWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet(ClockUrl, AsException(SocketError.TryAgain));
        mock.AddGet(ClockUrl, AsException(SocketError.TimedOut));
        mock.AddGet(ClockUrl, AsException(SocketError.HostNotFound));
        mock.AddGet(ClockUrl, AsException(SocketError.NotConnected));
        mock.AddGet(ClockUrl, AsException(SocketError.NotConnected));
        mock.AddGet(ClockUrl, AsException(SocketError.NotConnected));

        var exception = await Assert.ThrowsAsync<SocketException>(
            () => mock.Client.GetClockAsync());
        Assert.Equal(SocketError.NotConnected, exception.SocketErrorCode);
        return;

        SocketException AsException(
            SocketError socketError) =>
            new((Int32)socketError);
    }

    [Fact]
    public async Task ThrottlingWithoutErrorMessageWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        const String errorMessage = "<html><body>HTTP 500: Unknown server error.</body></html>";

        mock.AddDelete(OrdersUrl, errorMessage, HttpStatusCode.InternalServerError);

        var exception = await Assert.ThrowsAsync<RestClientErrorException>(
            () => mock.Client.CancelOrderAsync(Guid.NewGuid()));

        Assert.Null(exception.ErrorInformation);
        Assert.NotEqual("HTTP 500: Unknown server error.",exception.Message);
        Assert.Equal((Int32)HttpStatusCode.InternalServerError, exception.ErrorCode);
        Assert.Equal(HttpStatusCode.InternalServerError, exception.HttpStatusCode);
    }

    [Fact]
    public async Task ThrottlingWithInvalidErrorMessageWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        var errorMessage = new JObject(
            new JProperty("msg", Message),
            new JProperty("id", ErrorCode)).ToString();

        mock.AddDelete(OrdersUrl, errorMessage, HttpStatusCode.InternalServerError);

        var exception = await Assert.ThrowsAsync<RestClientErrorException>(
            () => mock.Client.CancelOrderAsync(Guid.NewGuid()));

        Assert.NotEqual(Message,exception.Message);
        Assert.Equal((Int32)HttpStatusCode.InternalServerError, exception.ErrorCode);
    }

    [Fact]
    public async Task CustomThrottleParametersWorks()
    {
        const UInt32 maxRetryParameters = 1;
        var timeout = TimeSpan.FromSeconds(1);

        var retrySocketErrorCodes = new[] { SocketError.AlreadyInProgress };
        var retryHttpStatuses = new[] { HttpStatusCode.FailedDependency };

        var throttleParameters = new ThrottleParameters(
            maxRetryParameters, retrySocketErrorCodes, retryHttpStatuses)
        {
            Timeout = timeout
        };

        Assert.Equal(retrySocketErrorCodes, throttleParameters.RetrySocketErrorCodes);
        Assert.Equal(retryHttpStatuses, throttleParameters.RetryHttpStatuses);
        Assert.Equal(maxRetryParameters, throttleParameters.MaxRetryAttempts);
        Assert.Equal(timeout, throttleParameters.Timeout);

        using var handler = throttleParameters.GetMessageHandler();
        Assert.NotNull(handler);

        var policy = throttleParameters.GetAsyncPolicy();
        Assert.NotNull(policy);

        using var client = new HttpClient(handler);
        await Assert.ThrowsAsync<TimeoutException>(
            () => client.GetAsync("https://httpbin.org/delay/10"));
    }

    [Fact]
    public async Task DefaultThrottleParametersWorks()
    {
        using var client = Environments.Paper.GetAlpacaTradingClient(
            new SecretKey(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N")));
        await Assert.ThrowsAsync<RestClientErrorException>(() => client.GetClockAsync());
    }
}
