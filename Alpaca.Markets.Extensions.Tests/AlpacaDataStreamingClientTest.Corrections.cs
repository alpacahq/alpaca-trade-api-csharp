namespace Alpaca.Markets.Extensions.Tests;

public sealed partial class AlpacaDataStreamingClientTest
{
    [Fact]
    public void GetCorrectionSubscriptionWorks()
    {
        var client = createMockClient(
            _ => _.GetCorrectionSubscription(It.IsAny<String>()));

        var subscriptionOne = client.Object.GetCorrectionSubscription(_symbols);
        var subscriptionTwo = client.Object.GetCorrectionSubscription(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscriptionOne, 4);

        client.VerifyAll();
    }

    [Fact]
    public async Task SubscribeCorrectionAsyncWorks()
    {
        var client = createMockClient(
            _ => _.GetCorrectionSubscription(It.IsAny<String>()));

        await using var subscription = await client.Object.SubscribeCorrectionAsync(Stock);
        await using var subscriptionOne = await client.Object.SubscribeCorrectionAsync(_symbols);
        // ReSharper disable once UseAwaitUsing
        using var subscriptionTwo = await client.Object.SubscribeCorrectionAsync(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscription, 2);

        await subscriptionOne.DisposeAsync();
        client.VerifyAll();
    }
}
