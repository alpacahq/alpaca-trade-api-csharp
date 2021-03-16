using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Set of extension methods for the <see cref="IAlpacaDataSubscription{TApi}"/> interface.
    /// </summary>
    [CLSCompliant(false)]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class AlpacaDataSubscriptionExtensions
    {
        /// <summary>
        /// Converts <see cref="IAlpacaDataSubscription{TItem}.Received"/> event into
        /// asynchronous enumerable (reactive event consuming style) using unbound channel.
        /// </summary>
        /// <param name="subscription">Original subscription object for wrapping.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <typeparam name="TItem">Type of streaming item provided via <paramref name="subscription"/> object.</typeparam>
        /// <returns>Stream of items received from server in form of async enumerable.</returns>
        public static async IAsyncEnumerable<TItem> AsAsyncEnumerable<TItem>(
            this IAlpacaDataSubscription<TItem> subscription,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            subscription.EnsureNotNull(nameof(subscription));

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

            void HandleReceived(TItem item) => buffer.Writer.TryWrite(item);
        }
    }
}
