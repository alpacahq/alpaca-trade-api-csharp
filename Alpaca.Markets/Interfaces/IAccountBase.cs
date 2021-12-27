using System;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates basic account information from Alpaca streaming API.
    /// </summary>
    [Obsolete("All members of this interface will be pushed down into the IAccount interface in the next major SDK release.", false)]
    public interface IAccountBase
    {
        /// <summary>
        /// Gets unique account identifier.
        /// </summary>
        [UsedImplicitly]
        Guid AccountId { get; }

        /// <summary>
        /// Gets updated account status.
        /// </summary>
        [UsedImplicitly]
        AccountStatus Status { get; }

        /// <summary>
        /// Gets main account currency.
        /// </summary>
        [UsedImplicitly]
        String? Currency { get; }

        /// <summary>
        /// Gets amount of money available for trading.
        /// </summary>
        [UsedImplicitly]
        Decimal TradableCash { get; }

        /// <summary>
        /// Gets amount of money available for withdraw.
        /// </summary>
        [UsedImplicitly]
        [Obsolete("This property will be removed in the next major SDK release. Use TradableCash instead.", true)]
        Decimal WithdrawableCash { get; }

        /// <summary>
        /// Gets timestamp of account creation event in UTC.
        /// </summary>
        [UsedImplicitly]
        DateTime CreatedAtUtc { get; }
    }
}
