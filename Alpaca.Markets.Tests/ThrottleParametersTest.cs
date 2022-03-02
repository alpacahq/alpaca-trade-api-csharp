namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed class ThrottleParametersTest
{
    private readonly MockClientsFactoryFixture _mockClientsFactory;

    public ThrottleParametersTest(
        MockClientsFactoryFixture mockClientsFactory) =>
        _mockClientsFactory = mockClientsFactory;

    [Fact]
    public async Task ThrottlingWithErrorMessageWorks()
    {
        var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        var errorMessage = new JObject(
            new JProperty("message", "Fahrenheit"),
            new JProperty("code", 451)).ToString();

        mock.AddGet("/v2/clock", errorMessage, HttpStatusCode.TooManyRequests);
        mock.AddGet("/v2/clock", errorMessage, HttpStatusCode.TooManyRequests);
        mock.AddGet("/v2/clock", errorMessage, HttpStatusCode.TooManyRequests);
        mock.AddGet("/v2/clock", errorMessage, HttpStatusCode.TooManyRequests);
        mock.AddGet("/v2/clock", errorMessage, HttpStatusCode.TooManyRequests);
        mock.AddGet("/v2/clock", errorMessage, HttpStatusCode.TooManyRequests);

        var exception = await Assert.ThrowsAsync<RestClientErrorException>(
            () => mock.Client.GetClockAsync());

        Assert.Equal("Fahrenheit",exception.Message);
        Assert.Equal(451, exception.ErrorCode);
    }

    [Fact]
    public async Task ThrottlingWithoutErrorMessageWorks()
    {
        var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        const String errorMessage = "<html><body>HTTP 500: Unknown server error.</body></html>";

        mock.AddDelete("/v2/orders/**", errorMessage, HttpStatusCode.InternalServerError);

        var exception = await Assert.ThrowsAsync<RestClientErrorException>(
            () => mock.Client.DeleteOrderAsync(Guid.NewGuid()));

        Assert.NotEqual("HTTP 500: Unknown server error.",exception.Message);
        Assert.Equal(500, exception.ErrorCode);
    }
}
