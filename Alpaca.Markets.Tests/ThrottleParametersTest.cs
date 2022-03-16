namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed class ThrottleParametersTest
{
    private readonly MockClientsFactoryFixture _mockClientsFactory;

    private const String OrdersUrl = "/v2/orders/**";

    private const String ClockUrl = "/v2/clock";

    private const String Message = "Fahrenheit";

    public ThrottleParametersTest(
        MockClientsFactoryFixture mockClientsFactory) =>
        _mockClientsFactory = mockClientsFactory;

    [Fact]
    public async Task ThrottlingWithErrorMessageWorks()
    {
        var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        var errorMessage = new JObject(
            new JProperty("message", Message),
            new JProperty("code", 451)).ToString();

        mock.AddGet(ClockUrl, errorMessage, HttpStatusCode.TooManyRequests);
        mock.AddGet(ClockUrl, errorMessage, HttpStatusCode.TooManyRequests);
        mock.AddGet(ClockUrl, errorMessage, HttpStatusCode.TooManyRequests);
        mock.AddGet(ClockUrl, errorMessage, HttpStatusCode.TooManyRequests);
        mock.AddGet(ClockUrl, errorMessage, HttpStatusCode.TooManyRequests);
        mock.AddGet(ClockUrl, errorMessage, HttpStatusCode.TooManyRequests);

        var exception = await Assert.ThrowsAsync<RestClientErrorException>(
            () => mock.Client.GetClockAsync());

        Assert.Equal(Message,exception.Message);
        Assert.Equal(451, exception.ErrorCode);
    }

    [Fact]
    public async Task ThrottlingWithoutErrorMessageWorks()
    {
        var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        const String errorMessage = "<html><body>HTTP 500: Unknown server error.</body></html>";

        mock.AddDelete(OrdersUrl, errorMessage, HttpStatusCode.InternalServerError);

        var exception = await Assert.ThrowsAsync<RestClientErrorException>(
            () => mock.Client.DeleteOrderAsync(Guid.NewGuid()));

        Assert.NotEqual("HTTP 500: Unknown server error.",exception.Message);
        Assert.Equal((Int32)HttpStatusCode.InternalServerError, exception.ErrorCode);
    }

    [Fact]
    public async Task ThrottlingWithInvalidErrorMessageWorks()
    {
        var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        var errorMessage = new JObject(
            new JProperty("msg", Message),
            new JProperty("id", 451)).ToString();

        mock.AddDelete(OrdersUrl, errorMessage, HttpStatusCode.InternalServerError);

        var exception = await Assert.ThrowsAsync<RestClientErrorException>(
            () => mock.Client.DeleteOrderAsync(Guid.NewGuid()));

        Assert.NotEqual(Message,exception.Message);
        Assert.Equal((Int32)HttpStatusCode.InternalServerError, exception.ErrorCode);
    }
}
