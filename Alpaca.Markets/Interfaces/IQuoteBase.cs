using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates basic quote information any REST API.
    /// </summary>
    /// <typeparam name="TExchange">Type of bid/ask exchange properties.</typeparam>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [CLSCompliant(false)]
    public interface IQuoteBase<out TExchange>
    {
        /// <summary>
        /// Gets identifier of bid source exchange.
        /// </summary>
        TExchange BidExchange { get; }

        /// <summary>
        /// Gets identifier of ask source exchange.
        /// </summary>
        TExchange AskExchange { get; }

        /// <summary>
        /// Gets bid price level.
        /// </summary>
        Decimal BidPrice { get; }

        /// <summary>
        /// Gets ask price level.
        /// </summary>
        Decimal AskPrice { get; }

        /// <summary>
        /// Gets bid quantity.
        /// </summary>
        UInt64 BidSize { get; }

        /// <summary>
        /// Gets ask quantity.
        /// </summary>
        UInt64 AskSize { get; }
    }
}
