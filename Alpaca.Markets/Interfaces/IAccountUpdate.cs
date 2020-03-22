using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates account update information from Alpaca streaming API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface IAccountUpdate : IAccountBase
    {
        /// <summary>
        /// Gets timestamp of last account update event.
        /// </summary>
        DateTime UpdatedAt { get; }

        /// <summary>
        /// Gets timestamp of account deletion event.
        /// </summary>
        DateTime? DeletedAt { get; }
    }
}
