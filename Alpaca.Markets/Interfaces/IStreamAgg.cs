using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates bar information from Polygon streaming API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [CLSCompliant(false)]
    public interface IStreamAgg : IAggBase
    {
        /// <summary>
        /// Gets bar average price.
        /// </summary>
        Decimal Average { get; }

        /// <summary>
        /// Gets bar opening timestamp in UTC time zone.
        /// </summary>
        DateTime StartTimeUtc { get; }

        /// <summary>
        /// Gets bar closing timestamp in UTC time zone.
        /// </summary>
        DateTime EndTimeUtc { get; }

        /// <summary>
        /// Gets asset name.
        /// </summary>
        String Symbol { get; }
    }
}
