using System;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates the basic LULD update information from Alpaca APIs.
    /// </summary>
    public interface ILimitUpLimitDown
    {
        /// <summary>
        /// Gets asset name.
        /// </summary>
        [UsedImplicitly]
        String Symbol { get; }

        /// <summary>
        /// Gets the LULD update timestamp in the UTC.
        /// </summary>
        [UsedImplicitly]
        [Obsolete("This property will be removed in the next major version of SDK. Use the TimestampUtc property instead.", false)]
        DateTime TimeUtc { get; }

        /// <summary>
        /// Gets the LULD update timestamp in the UTC.
        /// </summary>
        [UsedImplicitly]
        DateTime TimestampUtc { get; }

        /// <summary>
        /// Gets the current limit up price.
        /// </summary>
        [UsedImplicitly]
        Decimal LimitUpPrice { get; }

        /// <summary>
        /// Gets the current limit down price.
        /// </summary>
        [UsedImplicitly]
        Decimal LimitDownPrice{ get; }

        /// <summary>
        /// Gets the indicator name.
        /// </summary>
        [UsedImplicitly]
        String Indicator { get; }

        /// <summary>
        /// Gets tape.
        /// </summary>
        [UsedImplicitly]
        String Tape { get; }
    }
}
