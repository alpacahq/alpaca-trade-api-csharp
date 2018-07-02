using System;

namespace Alpaca.Markets
{
    public interface IAccountUpdate
    {
        Guid AccountId { get; }

        AccountStatus Status { get; }

        String Currency { get; }

        Decimal TradableCash { get; }

        Decimal WithdrawableCash { get; }

        DateTime CreatedAt { get; }

        DateTime UpdatedAt { get; }

        DateTime? DeletedAt { get; }
    }
}