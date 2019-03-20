using System;

namespace Alpaca.Markets
{
    internal interface IThrottler
    {
        /// <summary>
        /// Gets flag indicating we are currently being rate limited.
        /// </summary>
        Int32 MaxAttempts { get; set;  }

        /// <summary>
        /// Blocks the current thread indefinitely until allowed to proceed.
        /// </summary>
        void WaitToProceed();

        /// <summary>
        /// Block all further requests until the specified milliseconds have elapsed
        /// </summary>
        /// <param name="milliseconds">Block for milliseconds</param>
        void AllStop(Int32 milliseconds);

    }
}