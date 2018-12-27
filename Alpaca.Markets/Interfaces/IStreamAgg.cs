using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates bar information from Polygon streaming API.
    /// </summary>
    public interface IStreamAgg : IAggBase
    {
        /// <summary>
        /// Gets asset name.
        /// </summary>
        String Symbol { get; } 

        /// <summary>
        /// Gets asset's exchange identifier.
        /// </summary>
        [Obsolete("Exchange is deprecated and will be removed in a future version of the SDK.")]
        Int64 Exchange { get; }
        
        /// <summary>
        /// Gets bar average price.
        /// </summary>
        Decimal Average { get; }

        /// <summary>
        /// Gets bar opening timestamp.
        /// </summary>
        DateTime StartTime { get; }

        /// <summary>
        /// Gets bar closing timestamp.
        /// </summary>
        DateTime EndTime { get; }
    }
}