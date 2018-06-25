using System;

namespace Alpaca.Markets
{
    public interface IExchange
    {
        Int64 ExchangeId { get; }

        ExchangeType ExchangeType { get; }

        MarketDataType MarketDataType { get; }

        String MarketIdentificationCode { get; }

        String Name { get; }

        String TapeId { get; }
    }
}