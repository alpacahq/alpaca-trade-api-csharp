using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates basic quote information any REST API.
    /// </summary>
    /// <typeparam name="TExchange">Type of bid/ask exchange properties.</typeparam>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
#pragma warning disable 618
    public interface IQuoteBase<out TExchange> : IQuoteBase
#pragma warning restore 618
    {
        /// <summary>
        /// Gets identifier of bid source exchange.
        /// </summary>
        TExchange BidExchange { get; }

        /// <summary>
        /// Gets identifier of ask source exchange.
        /// </summary>
        TExchange AskExchange { get; }
    }

    /// <summary>
    /// Encapsulates basic quote information any REST API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [Obsolete("This interface will be merged with its generic version in the next major SDK release.", false)]
    public interface IQuoteBase
    {
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
