using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates bar information from Polygon REST API.
    /// </summary>
    [CLSCompliant(false)]
    public interface IAgg : IAggBase
    {
        /// <summary>
        /// Gets bar timestamp in the UTC.
        /// </summary>
        DateTime? TimeUtc { get; }
    }
}
