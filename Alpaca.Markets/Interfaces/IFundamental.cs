using System;

namespace Alpaca.Markets
{
    public interface IFundamental
    {
        Guid AssetId { get; }

        String Symbol { get; }

        String FullName { get; }

        String Industry { get; }

        String Sector { get; }

        Decimal pe_ratio { get; }

        Decimal peg_ratio { get; }

        Decimal beta { get; }

        Decimal eps { get; }

        Decimal market_cap { get; }

        Decimal shares_outstanding { get; }

        Decimal avg_vol { get; }

        Decimal div_rate { get; }

        Decimal roa { get; }

        Decimal roe { get; }

        Decimal ps { get; }

        Decimal pc { get; }

        Decimal gross_margin { get; }
    }
}