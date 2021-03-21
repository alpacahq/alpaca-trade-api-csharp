using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates exchange information from Polygon REST API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [CLSCompliant(false)]
    public interface IExchange
    {
        /// <summary>
        /// Gets exchange unique identifier.
        /// </summary>
        UInt32 ExchangeId { get; }

        /// <summary>
        /// Gets exchange type.
        /// </summary>
        ExchangeType ExchangeType { get; }

        /// <summary>
        /// Gets market data type.
        /// </summary>
        MarketDataType MarketDataType { get; }

        /// <summary>
        /// Gets exchange market identification code.
        /// </summary>
        String? MarketIdentificationCode { get; }

        /// <summary>
        /// Gets exchange name.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Gets exchange tape ID.
        /// </summary>
        String? TapeId { get; }
    }
}
