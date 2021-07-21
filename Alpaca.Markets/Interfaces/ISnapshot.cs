using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates snapshot information from the Alpaca REST API.
    /// </summary>
    [CLSCompliant(false)]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface ISnapshot
    {
        /// <summary>
        /// Gets the snapshot's asset name.
        /// </summary>
        String Symbol { get; }

        /// <summary>
        /// Gets the latest trade information.
        /// </summary>
        IQuote Quote { get; }

        /// <summary>
        /// Gets the latest quote information.
        /// </summary>
        ITrade Trade { get; }

        /// <summary>
        /// Gets the current minute bar information.
        /// </summary>
        IBar? MinuteBar { get; }

        /// <summary>
        /// Gets the current daily bar information.
        /// </summary>
        IBar? CurrentDailyBar { get; }

        /// <summary>
        /// Gets the previous minute bar information.
        /// </summary>
        IBar? PreviousDailyBar { get; }
    }
}
