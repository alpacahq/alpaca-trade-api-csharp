using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates basic quote information any REST API.
    /// </summary>
    /// <typeparam name="TExchange">Type of bid/ask exchange properties.</typeparam>
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
        Int64 BidSize { get; }

        /// <summary>
        /// Gets ask quantity.
        /// </summary>
        Int64 AskSize { get; }
    }
}
