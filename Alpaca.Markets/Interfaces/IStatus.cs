using System;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates the basic trading status update information from Alpaca APIs.
    /// </summary>
    public interface IStatus
    {
        /// <summary>
        /// Gets asset name.
        /// </summary>
        [UsedImplicitly]
        String Symbol { get; }

        /// <summary>
        /// Gets the beginning time of this bar in the UTC.
        /// </summary>
        [UsedImplicitly]
        DateTime TimeUtc { get; }

        /// <summary>
        /// Gets status code.
        /// </summary>
        [UsedImplicitly]
        String StatusCode { get; }

        /// <summary>
        /// Gets status message.
        /// </summary>
        [UsedImplicitly]
        String StatusMessage { get; }

        /// <summary>
        /// Gets reason code.
        /// </summary>
        [UsedImplicitly]
        String ReasonCode { get; }

        /// <summary>
        /// Gets reason message.
        /// </summary>
        [UsedImplicitly]
        String ReasonMessage { get; }

        /// <summary>
        /// Gets tape.
        /// </summary>
        [UsedImplicitly]
        String Tape { get; }
    }
}
