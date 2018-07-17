using System;

namespace Alpaca.Markets
{
    public interface IFundamental
    {
        Guid AssetId { get; }

        String Symbol { get; }

        String FullName { get; }

        String Industry { get; }

        String IndustryGroup { get; }

        String Sector { get; }

        String ShortDescription { get; }

        String LongDescription { get; }

        Decimal pe_ratio { get; }

        Decimal peg_ratio { get; }

        Decimal beta { get; }

        Decimal eps { get; }

        Decimal MarketCapitalization { get; }

        Decimal SharesOutstanding { get; }

        Decimal AvgVolume { get; }

        Decimal FiftyTwoWeekHigh { get; }

        Decimal FiftyTwoWeekLow { get;  }

        Decimal DividentsRate { get; }

        Decimal roa { get; }

        Decimal roe { get; }

        Decimal ps { get; }

        Decimal pc { get; }

        Decimal GrossMargin { get; }
    }
}