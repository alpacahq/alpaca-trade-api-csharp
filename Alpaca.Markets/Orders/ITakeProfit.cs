using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITakeProfit
    {
        /// <summary>
        /// Gets or sets the profit taking limit price for advanced order types.
        /// </summary>
        Decimal LimitPrice { get; }
    }
}
