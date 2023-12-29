using System.Linq.Expressions;

namespace Alpaca.Markets.Extensions.Tests;

public sealed partial class AlpacaDataStreamingClientTest
{
    private static readonly List<String> _symbols = [ Stock, Other ];

    private const Int32 ExpectedNumberOfEventsForAllSymbols = 4;

    private const Int32 ExpectedNumberOfEventsForOneSymbol = 2;

    private const String Stock = "AAPL";

    private const String Other = "MSFT";

    [Fact(Skip = "Temporary until Extensions package upgrade")]
    public async Task WithReconnectWorks()
    {
        var client = createMockClient(
            client => client.GetTradeSubscription(It.IsAny<String>()));

        using var wrapped = client.Object.WithReconnect();
        var result = await wrapped.ConnectAndAuthenticateAsync();

        await wrapped.DisconnectAsync();

        Assert.Equal(AuthStatus.Authorized, result);
    }

    [Fact]
    public void ReconnectionParametersWorks()
    {
        var parameters = new ReconnectionParameters
        {
            MaxReconnectionAttempts = ReconnectionParameters.Default.MaxReconnectionAttempts,
            MaxReconnectionDelay = ReconnectionParameters.Default.MaxReconnectionDelay,
            MinReconnectionDelay = ReconnectionParameters.Default.MinReconnectionDelay
        };

        Assert.NotEmpty(parameters.RetryWebSocketErrorCodes);
        Assert.NotEqual(parameters, ReconnectionParameters.Default);
    }

    private static Mock<IAlpacaDataStreamingClient> createMockClient<TItem>(
        Expression<Func<IAlpacaDataStreamingClient, IAlpacaDataSubscription<TItem>>> subscriptionFactory)
        where TItem : class =>
        subscriptionFactory.CreateMockClient();

    private static void verifySubscriptions<TItem>(
        IAlpacaDataSubscription<TItem> lhs,
        IAlpacaDataSubscription<TItem> rhs) =>
        _symbols.VerifySubscriptionsStreams(lhs, rhs);

    private static void verifySubscriptionEvents<TItem>(
        IAlpacaDataSubscription<TItem> subscription,
        Int32 expectedNumberOfEvents) =>
        subscription.VerifySubscriptionEventsNumber(expectedNumberOfEvents);
}
