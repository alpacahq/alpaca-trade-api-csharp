using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class AlpacaDataSubscriptionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TItem"></typeparam>
        /// <returns></returns>
        public static async IAsyncEnumerable<TItem> AsAsyncEnumerable<TItem>(
            this IAlpacaDataSubscription<TItem> subscription,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
            where TItem : IStreamBase
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
