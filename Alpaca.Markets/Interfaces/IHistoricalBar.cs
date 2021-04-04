using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates bar information from Alpaca APIs.
    /// </summary>
    [CLSCompliant(false)]
    public interface IHistoricalBar : IBar
    {
        /// <summary>
        /// Gets bar timestamp in the UTC.
        /// </summary>
        DateTime? TimeUtc { get; }
    }
}
