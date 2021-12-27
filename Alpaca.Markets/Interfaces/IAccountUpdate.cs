using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates account update information from Alpaca streaming API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [Obsolete("This interface will be removed in the next major SDK release.", true)]
    public interface IAccountUpdate : IAccountBase
    {
        /// <summary>
        /// Gets timestamp of last account update event in the UTC.
        /// </summary>
        [UsedImplicitly]
        DateTime UpdatedAtUtc { get; }

        /// <summary>
        /// Gets timestamp of account deletion event in the UTC.
        /// </summary>
        [UsedImplicitly]
        DateTime? DeletedAtUtc { get; }
    }
}
