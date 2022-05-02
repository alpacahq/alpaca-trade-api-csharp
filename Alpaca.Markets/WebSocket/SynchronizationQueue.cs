using System.Collections.Concurrent;

namespace Alpaca.Markets;

internal sealed class SynchronizationQueue : IDisposable
{
    private readonly BlockingCollection<Action> _actions =
        new(new ConcurrentQueue<Action>());

    private readonly CancellationTokenSource _cancellationTokenSource = new();

    private readonly Thread _processingThread;

    public SynchronizationQueue()
    {
        _processingThread = new Thread(processingTask)
        {
            IsBackground = true
        };
        _processingThread.Start();
    }

    public event Action<Exception>? OnError;

    public void Enqueue(Action action) =>
        _actions.Add(action, _cancellationTokenSource.Token);

    [SuppressMessage(
        "Design", "CA1031:Do not catch general exception types",
        Justification = "Expected behavior - we report exceptions via OnError event.")]
    private void processingTask()
    {
        try
        {
            foreach (var action in _actions
                .GetConsumingEnumerable(_cancellationTokenSource.Token))
            {
                try
                {
                    action();
                }
                catch (Exception exception)
                {
                    OnError?.Invoke(exception);
                }
            }
        }
        catch (ObjectDisposedException exception)
        {
            Trace.TraceInformation(exception.Message);
        }
        catch (OperationCanceledException exception)
        {
            Trace.TraceInformation(exception.Message);
        }
    }

    [SuppressMessage("Design", "CA1031:Do not catch general exception types",
        Justification = "Double disposing should be safe - just ignore any exception.")]
    public void Dispose()
    {
        try
        {
            _cancellationTokenSource.Cancel(false);
            _processingThread.Join(TimeSpan.FromSeconds(5));

            _cancellationTokenSource.Dispose();
            _actions.Dispose();
        }
        catch (Exception exception)
        {
            Trace.TraceInformation(exception.Message);
        }
    }
}
