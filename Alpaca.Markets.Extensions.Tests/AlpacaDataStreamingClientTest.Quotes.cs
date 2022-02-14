using Moq;
using Xunit;

namespace Alpaca.Markets.Extensions.Tests;

public sealed partial class AlpacaDataStreamingClientTest
{
    [Fact]
    public void GetQuoteSubscriptionWorks()
    {
        var client = createMockClient(
            _ => _.GetQuoteSubscription(It.IsAny<String>()));

        var subscriptionOne = client.Object.GetQuoteSubscription(_symbols);
        var subscriptionTwo = client.Object.GetQuoteSubscription(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscriptionOne, 4);

        client.VerifyAll();
    }

    [Fact]
    public async Task SubscribeQuoteAsyncWorks()
    {
        var client = createMockClient(
            _ => _.GetQuoteSubscription(It.IsAny<String>()));

        await using var subscription = await client.Object.SubscribeQuoteAsync(Stock);
        await using var subscriptionOne = await client.Object.SubscribeQuoteAsync(_symbols);
        // ReSharper disable once UseAwaitUsing
        using var subscriptionTwo = await client.Object.SubscribeQuoteAsync(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscription, 2);

        await subscriptionOne.DisposeAsync();
        client.VerifyAll();
    }
}
