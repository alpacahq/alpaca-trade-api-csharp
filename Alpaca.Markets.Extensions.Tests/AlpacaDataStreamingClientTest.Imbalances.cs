namespace Alpaca.Markets.Extensions.Tests;

public sealed partial class AlpacaDataStreamingClientTest
{
    [Fact]
    public void GetOrderImbalanceSubscriptionWorks()
    {
        var client = createMockClient(
            client => client.GetOrderImbalanceSubscription(It.IsAny<String>()));

        var subscriptionOne = client.Object.GetOrderImbalanceSubscription(_symbols);
        var subscriptionTwo = client.Object.GetOrderImbalanceSubscription(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscriptionOne, ExpectedNumberOfEventsForAllSymbols);

        client.VerifyAll();
    }

    [Fact]
    public async Task SubscribeOrderImbalanceAsyncWorks()
    {
        var client = createMockClient(
            client => client.GetOrderImbalanceSubscription(It.IsAny<String>()));

        await using var subscription = await client.Object.SubscribeOrderImbalanceAsync(Stock);
        await using var subscriptionOne = await client.Object.SubscribeOrderImbalanceAsync(_symbols);
        // ReSharper disable once UseAwaitUsing
        using var subscriptionTwo = await client.Object.SubscribeOrderImbalanceAsync(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscription, ExpectedNumberOfEventsForOneSymbol);

        client.VerifyAll();
    }
}