using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStopLoss
    {
        /// <summary>
        /// Gets or sets the stop loss stop price for advanced order types.
        /// </summary>
        Decimal StopPrice { get; }

        /// <summary>
        /// Gets or sets the stop loss limit price for advanced order types.
        /// </summary>
        Decimal? LimitPrice { get; }
    }
}
