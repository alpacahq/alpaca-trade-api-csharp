namespace Alpaca.Markets.Extensions.Tests;

public sealed partial class AlpacaDataStreamingClientTest
{
    [Fact]
    public void GetCancellationSubscriptionWorks()
    {
        var client = createMockClient(
            _ => _.GetCancellationSubscription(It.IsAny<String>()));

        var subscriptionOne = client.Object.GetCancellationSubscription(_symbols);
        var subscriptionTwo = client.Object.GetCancellationSubscription(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscriptionOne, 4);

        client.VerifyAll();
    }

    [Fact]
    public async Task SubscribeCancellationAsyncWorks()
    {
        var client = createMockClient(
            _ => _.GetCancellationSubscription(It.IsAny<String>()));

        await using var subscription = await client.Object.SubscribeCancellationAsync(Stock);
        await using var subscriptionOne = await client.Object.SubscribeCancellationAsync(_symbols);
        // ReSharper disable once UseAwaitUsing
        using var subscriptionTwo = await client.Object.SubscribeCancellationAsync(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscription, 2);

        await subscriptionOne.DisposeAsync();
        client.VerifyAll();
    }
}
