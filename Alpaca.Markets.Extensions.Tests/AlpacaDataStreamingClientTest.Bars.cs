namespace Alpaca.Markets.Extensions.Tests;

public sealed partial class AlpacaDataStreamingClientTest
{
    [Fact]
    public void GetDailyBarSubscriptionWorks()
    {
        var client = createMockClient(
            client => client.GetDailyBarSubscription(It.IsAny<String>()));

        var subscriptionOne = client.Object.GetDailyBarSubscription(_symbols);
        var subscriptionTwo = client.Object.GetDailyBarSubscription(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscriptionOne, ExpectedNumberOfEventsForAllSymbols);

        client.VerifyAll();
    }

    [Fact]
    public async Task SubscribeDailyBarAsyncWorks()
    {
        var client = createMockClient(
            client => client.GetDailyBarSubscription(It.IsAny<String>()));

        await using var subscription = await client.Object.SubscribeDailyBarAsync(Stock);
        await using var subscriptionOne = await client.Object.SubscribeDailyBarAsync(_symbols);
        // ReSharper disable once UseAwaitUsing
        using var subscriptionTwo = await client.Object.SubscribeDailyBarAsync(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscription, ExpectedNumberOfEventsForOneSymbol);

        client.VerifyAll();
    }

    [Fact]
    public void GetMinuteBarSubscriptionWorks()
    {
        var client = createMockClient(
            client => client.GetMinuteBarSubscription(It.IsAny<String>()));

        var subscriptionOne = client.Object.GetMinuteBarSubscription(_symbols);
        var subscriptionTwo = client.Object.GetMinuteBarSubscription(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscriptionOne, ExpectedNumberOfEventsForAllSymbols);

        client.VerifyAll();
    }

    [Fact]
    public async Task SubscribeMinuteBarAsyncWorks()
    {
        var client = createMockClient(
            client => client.GetMinuteBarSubscription(It.IsAny<String>()));

        await using var subscription = await client.Object.SubscribeMinuteBarAsync(Stock);
        await using var subscriptionOne = await client.Object.SubscribeMinuteBarAsync(_symbols);
        // ReSharper disable once UseAwaitUsing
        using var subscriptionTwo = await client.Object.SubscribeMinuteBarAsync(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscription, ExpectedNumberOfEventsForOneSymbol);

        client.VerifyAll();
    }

    [Fact]
    public void GetUpdatedBarSubscriptionWorks()
    {
        var client = createMockClient(
            client => client.GetUpdatedBarSubscription(It.IsAny<String>()));

        var subscriptionOne = client.Object.GetUpdatedBarSubscription(_symbols);
        var subscriptionTwo = client.Object.GetUpdatedBarSubscription(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscriptionOne, ExpectedNumberOfEventsForAllSymbols);

        client.VerifyAll();
    }

    [Fact]
    public async Task SubscribeUpdatedBarAsyncWorks()
    {
        var client = createMockClient(
            client => client.GetUpdatedBarSubscription(It.IsAny<String>()));

        await using var subscription = await client.Object.SubscribeUpdatedBarAsync(Stock);
        await using var subscriptionOne = await client.Object.SubscribeUpdatedBarAsync(_symbols);
        // ReSharper disable once UseAwaitUsing
        using var subscriptionTwo = await client.Object.SubscribeUpdatedBarAsync(Stock, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscription, ExpectedNumberOfEventsForOneSymbol);

        client.VerifyAll();
    }
}
