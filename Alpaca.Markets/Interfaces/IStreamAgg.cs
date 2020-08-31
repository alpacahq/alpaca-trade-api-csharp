using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates bar information from Polygon streaming API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface IStreamAgg : IAggBase, IStreamBase
    {
        /// <summary>
        /// Gets bar average price.
        /// </summary>
        Decimal Average { get; }

        /// <summary>
        /// Gets bar opening timestamp.
        /// </summary>
        [Obsolete("This property will be removed in the next major release. Use the StartTimeUtc property instead.", false)]
        DateTime StartTime { get; }

        /// <summary>
        /// Gets bar opening timestamp in UTC time zone.
        /// </summary>
        DateTime StartTimeUtc { get; }

        /// <summary>
        /// Gets bar closing timestamp.
        /// </summary>
        [Obsolete("This property will be removed in the next major release. Use the EndTimeUtc property instead.", false)]
        DateTime EndTime { get; }

        /// <summary>
        /// Gets bar closing timestamp in UTC time zone.
        /// </summary>
        DateTime EndTimeUtc { get; }
    }
}
