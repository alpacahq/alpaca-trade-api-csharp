using System.Threading.Channels;

namespace Alpaca.Markets.Extensions;

/// <summary>
/// Set of extension methods for the <see cref="IAlpacaDataSubscription{TApi}"/> interface.
/// </summary>
public static class AlpacaDataSubscriptionExtensions
{
    /// <summary>
    /// Converts <see cref="IAlpacaDataSubscription{TItem}.Received"/> event into
    /// asynchronous enumerable (reactive event consuming style) using unbound channel.
    /// </summary>
    /// <param name="subscription">Original subscription object for wrapping.</param>
    /// <typeparam name="TItem">Type of streaming item provided via <paramref name="subscription"/> object.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="subscription"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>Stream of items received from server in form of async enumerable.</returns>
    [UsedImplicitly]
    public static IAsyncEnumerable<TItem> AsAsyncEnumerable<TItem>(
        this IAlpacaDataSubscription<TItem> subscription) =>
        AsAsyncEnumerable(subscription, CancellationToken.None);

    /// <summary>
    /// Converts <see cref="IAlpacaDataSubscription{TItem}.Received"/> event into
    /// asynchronous enumerable (reactive event consuming style) using unbound channel.
    /// </summary>
    /// <param name="subscription">Original subscription object for wrapping.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="TItem">Type of streaming item provided via <paramref name="subscription"/> object.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="subscription"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>Stream of items received from server in form of async enumerable.</returns>
    [UsedImplicitly]
    [SuppressMessage("Design", "CA1062:Validate arguments of public methods",
        Justification = "First validation using the EnsureNotNull call is enough.")]
    public static async IAsyncEnumerable<TItem> AsAsyncEnumerable<TItem>(
        this IAlpacaDataSubscription<TItem> subscription,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        subscription.EnsureNotNull();

        var buffer = Channel.CreateUnbounded<TItem>();
        subscription.Received += HandleReceived;

        try
        {
            while (await buffer.Reader
                .WaitToReadAsync(cancellationToken)
                .ConfigureAwait(false))
            {
                yield return await buffer.Reader
                    .ReadAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        finally
        {
            subscription.Received -= HandleReceived;
            buffer.Writer.TryComplete();
        }

        yield break;

        void HandleReceived(TItem item) => buffer.Writer.TryWrite(item);
    }
}
