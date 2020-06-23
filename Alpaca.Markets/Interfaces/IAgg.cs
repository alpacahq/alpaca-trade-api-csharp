using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates bar information from Polygon REST API.
    /// </summary>
    public interface IAgg : IAggBase
    {
        /// <summary>
        /// Gets bar timestamp.
        /// </summary>
        [Obsolete("This property will be removed in the next major release. Use the TimeUtc property instead.", false)]
        DateTime Time { get; }

        /// <summary>
        /// Gets bar timestamp in the UTC.
        /// </summary>
        DateTime TimeUtc { get; }
    }
}
