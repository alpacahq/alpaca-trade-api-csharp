using JetBrains.Annotations;
using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates snapshot information from the Alpaca REST API.
    /// </summary>
    public interface ISnapshot
    {
        /// <summary>
        /// Gets the snapshot's asset name.
        /// </summary>
        [UsedImplicitly]
        String Symbol { get; }

        /// <summary>
        /// Gets the latest trade information.
        /// </summary>
        [UsedImplicitly]
        IStreamQuote? Quote { get; }

        /// <summary>
        /// Gets the latest quote information.
        /// </summary>
        [UsedImplicitly]
        IStreamTrade? Trade { get; }

        /// <summary>
        /// Gets the current minute bar information.
        /// </summary>
        [UsedImplicitly]
        IAgg? MinuteBar { get; }

        /// <summary>
        /// Gets the current daily bar information.
        /// </summary>
        [UsedImplicitly]
        IAgg? CurrentDailyBar { get; }

        /// <summary>
        /// Gets the previous minute bar information.
        /// </summary>
        [UsedImplicitly]
        IAgg? PreviousDailyBar { get; }
    }
}
