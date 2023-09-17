using System.Globalization;
using System.Net.Http.Headers;

namespace Alpaca.Markets;

internal sealed class RateLimitHandler : IDisposable
{
    private readonly ReaderWriterLockSlim _lock = new(LockRecursionPolicy.NoRecursion);

    private RateLimitValues _current;

    public void TryUpdate(
        HttpHeaders headers)
    {
        var updated = new RateLimitValues(
            tryParseInt32(getHeaderValue(headers, "X-Ratelimit-Limit")),
            tryParseInt32(getHeaderValue(headers, "X-Ratelimit-Remaining")),
            tryParseInt64(getHeaderValue(headers, "X-Ratelimit-Reset")).FromUnixTimeSeconds());

        _lock.EnterWriteLock();
        try
        {
            var compareResult = updated.ResetTimeUtc.CompareTo(_current.ResetTimeUtc);
            if ((compareResult == 0 && updated.Remaining < _current.Remaining) ||
                compareResult > 0)
            {
                _current = updated;
            }
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public RateLimitValues GetCurrent()
    {
        _lock.EnterReadLock();
        try
        {
            return _current;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    public void Dispose() => _lock.Dispose();

    private static String? getHeaderValue(
        HttpHeaders headers,
        String headerName) =>
        headers.TryGetValues(headerName, out var values) ? values.FirstOrDefault() : null;

    private static Int32 tryParseInt32(
        String? headerValue) =>
        Int32.TryParse(headerValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value) ? value : 0;

    private static Int64 tryParseInt64(
        String? headerValue) =>
        Int64.TryParse(headerValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value) ? value : 0;
}
