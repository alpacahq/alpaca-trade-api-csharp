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
        [Obsolete("This property will be removed in the next major release. Use the UpdatedAtUtc property instead.", true)]
        DateTime UpdatedAt { get; }

        /// <summary>
        /// Gets timestamp of last account update event in the UTC.
        /// </summary>
        DateTime UpdatedAtUtc { get; }

        /// <summary>
        /// Gets timestamp of account deletion event.
        /// </summary>
        [Obsolete("This property will be removed in the next major release. Use the DeletedAtUtc property instead.", true)]
        DateTime? DeletedAt { get; }

        /// <summary>
        /// Gets timestamp of account deletion event in the UTC.
        /// </summary>
        DateTime? DeletedAtUtc { get; }
    }
}
