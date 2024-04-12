namespace Alpaca.Markets.Extensions.Tests;

public sealed partial class AlpacaDataStreamingClientTest
{
    [Fact]
    public void GetCancellationSubscriptionWorks()
    {
        var client = createMockClient(
            client => client.GetCancellationSubscription(It.IsAny<String>()));

        var subscriptionOne = client.Object.GetCancellationSubscription(_symbols);
        var subscriptionTwo = client.Object.GetCancellationSubscription(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscriptionOne, ExpectedNumberOfEventsForAllSymbols);

        client.VerifyAll();
    }

    [Fact]
    public async Task SubscribeCancellationAsyncWorks()
    {
        var client = createMockClient(
            client => client.GetCancellationSubscription(It.IsAny<String>()));

        await using var subscription = await client.Object.SubscribeCancellationAsync(Stock);
        await using var subscriptionOne = await client.Object.SubscribeCancellationAsync(_symbols);
        // ReSharper disable once UseAwaitUsing
        using var subscriptionTwo = await client.Object.SubscribeCancellationAsync(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscription, ExpectedNumberOfEventsForOneSymbol);

        client.VerifyAll();
    }
}
