using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates the basic quote information from Alpaca APIs.
    /// </summary>
    /// <typeparam name="TExchange">Type of bid/ask exchange properties.</typeparam>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [CLSCompliant(false)]
    public interface IQuote<out TExchange>
    {
        /// <summary>
        /// Gets quote timestamp in UTC time zone.
        /// </summary>
        DateTime TimestampUtc { get; }

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

        /// <summary>
        /// Gets tape where trade occurred.
        /// </summary>
        String Tape { get; }
    }
}
