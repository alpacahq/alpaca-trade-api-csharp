using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates position action status information from Alpaca REST API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface IPositionActionStatus
    {
        /// <summary>
        /// Gets processed position asset name.
        /// </summary>
        String Symbol { get; }

        /// <summary>
        /// Returns <c>true</c> if requested action completed successfully.
        /// </summary>
        [UsedImplicitly]
        Boolean IsSuccess { get; }
    }
}
