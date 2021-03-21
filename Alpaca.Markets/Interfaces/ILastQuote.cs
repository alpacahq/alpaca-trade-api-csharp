using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates last quote information from Alpaca REST API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [CLSCompliant(false)]
    public interface ILastQuote : IQuoteBase<Int64>
    {
        /// <summary>
        /// Gets quote timestamp in UTC time zone.
        /// </summary>
        DateTime TimeUtc { get; }

        /// <summary>
        /// Gets quote response status.
        /// </summary>
        String Status { get; }

        /// <summary>
        /// Gets asset name.
        /// </summary>
        String Symbol { get; }
    }
}
