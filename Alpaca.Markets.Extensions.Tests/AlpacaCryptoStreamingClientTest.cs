using System.Linq.Expressions;

namespace Alpaca.Markets.Extensions.Tests;

public sealed class AlpacaCryptoStreamingClientTest
{
    [Fact]
    public async Task WithReconnectWorks()
    {
        var client = createMockClient(
            _ => _.GetTradeSubscription(It.IsAny<String>()));

        using var wrapped = client.Object.WithReconnect();
        var result = await wrapped.ConnectAndAuthenticateAsync();

        await wrapped.DisconnectAsync();

        Assert.Equal(AuthStatus.Authorized, result);
    }

    private static Mock<IAlpacaCryptoStreamingClient> createMockClient<TItem>(
        Expression<Func<IAlpacaCryptoStreamingClient, IAlpacaDataSubscription<TItem>>> subscriptionFactory)
        where TItem : class =>
        subscriptionFactory.CreateMockClient();
}
