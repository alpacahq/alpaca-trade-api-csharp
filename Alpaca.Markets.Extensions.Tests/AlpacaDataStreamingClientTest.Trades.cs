namespace Alpaca.Markets.Extensions.Tests;

public sealed partial class AlpacaDataStreamingClientTest
{
    [Fact]
    public void GetTradesSubscriptionWorks()
    {
        var client = createMockClient(
            client => client.GetTradeSubscription(It.IsAny<String>()));

        var subscriptionOne = client.Object.GetTradeSubscription(_symbols);
        var subscriptionTwo = client.Object.GetTradeSubscription(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscriptionOne, ExpectedNumberOfEventsForAllSymbols);

        client.VerifyAll();
    }

    [Fact]
    public async Task SubscribeTradeAsyncWorks()
    {
        var client = createMockClient(
            client => client.GetTradeSubscription(It.IsAny<String>()));

        await using var subscription = await client.Object.SubscribeTradeAsync(Stock);
        await using var subscriptionOne = await client.Object.SubscribeTradeAsync(_symbols);
        // ReSharper disable once UseAwaitUsing
        using var subscriptionTwo = await client.Object.SubscribeTradeAsync(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscription, ExpectedNumberOfEventsForOneSymbol);

        client.VerifyAll();
    }
}
