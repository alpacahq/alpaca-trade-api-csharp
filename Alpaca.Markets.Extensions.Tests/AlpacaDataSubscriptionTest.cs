namespace Alpaca.Markets.Extensions.Tests;

public sealed class AlpacaDataSubscriptionTest
{
    private static readonly TimeSpan _delay = TimeSpan.FromMilliseconds(500);

    private const Int32 MinItems = 1;

    private const Int32 MaxItems = 3;

    [Fact]
    public async Task AsAsyncEnumerableWorks()
    {
        var cts = new CancellationTokenSource();
        var subscription = new Mock<IAlpacaDataSubscription<IBar>>();

        subscription.SetupAdd(s => s.Received += It.IsAny<Action<IBar>>());
        subscription.SetupRemove(s => s.Received -= It.IsAny<Action<IBar>>());

        // ReSharper disable once MethodSupportsCancellation
        var eventRaisingTask = Task.Run(async () =>
        {
            for (var index = MinItems; index <= MaxItems; ++index)
            {
                // ReSharper disable once MethodSupportsCancellation
                await Task.Delay(_delay);
                // ReSharper disable once MethodHasAsyncOverload
                subscription.Raise(s => s.Received += null, new Mock<IBar>().Object);
            }

            // ReSharper disable once MethodSupportsCancellation
            await Task.Delay(_delay);
            cts.Cancel(false);

            // We raise event after cancellation for pushing the async enumerator
            // ReSharper disable once MethodHasAsyncOverload
            subscription.Raise(s => s.Received += null, new Mock<IBar>().Object);
        });

        var count = 0;
        try
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            // ReSharper disable once MethodSupportsCancellation
            await foreach (var _ in subscription.Object.AsAsyncEnumerable())
            {
                // We need manual break here for testing empty token behavior
                if (cts.IsCancellationRequested)
                {
                    break;
                }

                ++count;
            }

            await eventRaisingTask;
        }
        catch (OperationCanceledException)
        {
            Assert.True(cts.IsCancellationRequested);
        }

        Assert.InRange(count, MinItems,  MaxItems);
        subscription.VerifyAll();
    }

}
