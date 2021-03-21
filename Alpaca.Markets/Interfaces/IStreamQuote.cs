using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates quote information from Polygon streaming API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [CLSCompliant(false)]
    public interface IStreamQuote : IQuoteBase<String>
    {
        /// <summary>
        /// Gets quote timestamp in UTC time zone.
        /// </summary>
        DateTime TimeUtc { get; }

        /// <summary>
        /// Gets asset name.
        /// </summary>
        String Symbol { get; }
    }
}
