namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed class AlpacaNewsStreamingClientTest
{
    private readonly MockClientsFactoryFixture _mockClientsFactory;

    private const String Stock = "AAPL";

    public AlpacaNewsStreamingClientTest(
        MockClientsFactoryFixture mockClientsFactory) =>
        _mockClientsFactory = mockClientsFactory;

    [Fact]
    public async Task ConnectAndSubscribeWorks()
    {
        using var client = _mockClientsFactory.GetAlpacaNewsStreamingClientMock();

        await client.AddAuthentication();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<INewsArticle>.Create(
                         client.Client, _ => _.Validate(Stock),
                         _ => _.GetNewsSubscription(Stock),
                         _ => _.GetNewsSubscription()))
        {
            await client.AddMessageAsync(
                new JArray(Stock.CreateStreamingNewsArticle()));
            Assert.True(helper.WaitAll());
        }

        await client.Client.DisconnectAsync();
    }
}
