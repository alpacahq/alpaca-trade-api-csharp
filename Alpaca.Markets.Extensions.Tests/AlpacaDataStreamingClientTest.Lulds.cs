using Moq;
using Xunit;

namespace Alpaca.Markets.Extensions.Tests;

public sealed partial class AlpacaDataStreamingClientTest
{
    [Fact]
    public void GetLimitUpLimitDownSubscriptionWorks()
    {
        var client = createMockClient(
            _ => _.GetLimitUpLimitDownSubscription(It.IsAny<String>()));

        var subscriptionOne = client.Object.GetLimitUpLimitDownSubscription(_symbols);
        var subscriptionTwo = client.Object.GetLimitUpLimitDownSubscription(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscriptionOne, 4);

        client.VerifyAll();
    }

    [Fact]
    public async Task SubscribeLimitUpLimitDownAsyncWorks()
    {
        var client = createMockClient(
            _ => _.GetLimitUpLimitDownSubscription(It.IsAny<String>()));

        await using var subscription = await client.Object.SubscribeLimitUpLimitDownAsync(Stock);
        await using var subscriptionOne = await client.Object.SubscribeLimitUpLimitDownAsync(_symbols);
        // ReSharper disable once UseAwaitUsing
        using var subscriptionTwo = await client.Object.SubscribeLimitUpLimitDownAsync(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscription, 2);

        await subscriptionOne.DisposeAsync();
        client.VerifyAll();
    }
}
