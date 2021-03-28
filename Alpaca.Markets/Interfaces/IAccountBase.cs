using System;

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
        Guid AccountId { get; }

        /// <summary>
        /// Gets updated account status.
        /// </summary>
        AccountStatus Status { get; }

        /// <summary>
        /// Gets main account currency.
        /// </summary>
        String? Currency { get; }

        /// <summary>
        /// Gets amount of money available for trading.
        /// </summary>
        Decimal TradableCash { get; }

        /// <summary>
        /// Gets amount of money available for withdraw.
        /// </summary>
        [Obsolete("This property will be removed in the next major SDK release. Use TradableCash instead.", false)]
        Decimal WithdrawableCash { get; }

        /// <summary>
        /// Gets timestamp of account creation event in UTC.
        /// </summary>
        DateTime CreatedAtUtc { get; }
    }
}
