namespace Alpaca.Markets.Extensions.Tests;

public sealed partial class AlpacaDataStreamingClientTest
{
    [Fact]
    public void GetStatusSubscriptionWorks()
    {
        var client = createMockClient(
            client => client.GetStatusSubscription(It.IsAny<String>()));

        var subscriptionOne = client.Object.GetStatusSubscription(_symbols);
        var subscriptionTwo = client.Object.GetStatusSubscription(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscriptionOne, ExpectedNumberOfEventsForAllSymbols);

        client.VerifyAll();
    }

    [Fact]
    public async Task SubscribeStatusAsyncWorks()
    {
        var client = createMockClient(
            client => client.GetStatusSubscription(It.IsAny<String>()));

        await using var subscription = await client.Object.SubscribeStatusAsync(Stock);
        await using var subscriptionOne = await client.Object.SubscribeStatusAsync(_symbols);
        // ReSharper disable once UseAwaitUsing
        using var subscriptionTwo = await client.Object.SubscribeStatusAsync(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscription, ExpectedNumberOfEventsForOneSymbol);

        client.VerifyAll();
    }
}
