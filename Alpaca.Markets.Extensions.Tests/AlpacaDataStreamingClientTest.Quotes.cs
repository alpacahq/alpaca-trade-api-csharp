namespace Alpaca.Markets.Extensions.Tests;

public sealed partial class AlpacaDataStreamingClientTest
{
    [Fact]
    public void GetQuoteSubscriptionWorks()
    {
        var client = createMockClient(
            client => client.GetQuoteSubscription(It.IsAny<String>()));

        var subscriptionOne = client.Object.GetQuoteSubscription(_symbols);
        var subscriptionTwo = client.Object.GetQuoteSubscription(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscriptionOne, ExpectedNumberOfEventsForAllSymbols);

        client.VerifyAll();
    }

    [Fact]
    public async Task SubscribeQuoteAsyncWorks()
    {
        var client = createMockClient(
            client => client.GetQuoteSubscription(It.IsAny<String>()));

        await using var subscription = await client.Object.SubscribeQuoteAsync(Stock);
        await using var subscriptionOne = await client.Object.SubscribeQuoteAsync(_symbols);
        // ReSharper disable once UseAwaitUsing
        using var subscriptionTwo = await client.Object.SubscribeQuoteAsync(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscription, ExpectedNumberOfEventsForOneSymbol);

        client.VerifyAll();
    }
}
