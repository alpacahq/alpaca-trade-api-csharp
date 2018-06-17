using System;

namespace Alpaca.Markets
{
    public interface IAccount
    {
        Guid Id { get; }

        AccountStatus Status { get; }

        String Currency { get; }

        Decimal TradableCash { get;  }

        Decimal WithdrawableCash { get;  }

        Decimal PortfolioValue { get;  }

        Boolean IsDayPatternTrader { get;  }

        Boolean IsTradingBlocked { get; }

        Boolean IsTransfersBlocked { get; }

        Boolean IsAccountBlocked { get; }

        DateTime CreatedAt { get; }
    }
}