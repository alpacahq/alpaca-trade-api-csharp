using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates order action status information from Alpaca REST API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface IOrderActionStatus
    {
        /// <summary>
        /// Gets unique server-side order identifier.
        /// </summary>
        Guid OrderId { get; }

        /// <summary>
        /// Returns <c>true</c> if requested action completed successfully.
        /// </summary>
        Boolean IsSuccess { get; }
    }
}
