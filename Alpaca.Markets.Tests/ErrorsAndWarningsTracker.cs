namespace Alpaca.Markets.Tests;

internal sealed class ErrorsAndWarningsTracker : IDisposable
{
    private static readonly TimeSpan _timeout = TimeSpan.FromMilliseconds(100);

    private readonly IStreamingClient _client;

    private readonly SemaphoreSlim _semaphore;

    private readonly Int32 _expectedWarnings;

    private readonly Int32 _expectedErrors;

    private Int32 _warnings;

    private Int32 _errors;

    public ErrorsAndWarningsTracker(
        IStreamingClient client,
        Int32 expectedWarnings,
        Int32 expectedErrors)
    {
        _expectedWarnings = expectedWarnings;
        _expectedErrors = expectedErrors;
        _client = client;

        _semaphore = new SemaphoreSlim(
            0, expectedErrors + expectedWarnings);

        _client.OnWarning += handleWarning;
        _client.OnError += handleError;
    }

    public async Task WaitAllEvents() =>
        Assert.True(await _semaphore.WaitAsync(_timeout));

    public void Dispose()
    {
        _client.OnWarning -= handleWarning;
        _client.OnError -= handleError;

        Assert.Equal(_expectedWarnings, _warnings);
        Assert.Equal(_expectedErrors, _errors);
    }

    private void handleError(Exception exception)
    {
        _semaphore.Release();
        ++_errors;
    }

    private void handleWarning(String message)
    {
        _semaphore.Release();
        ++_warnings;
    }
}
