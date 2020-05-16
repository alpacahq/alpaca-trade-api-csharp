using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TakeProfitOrder : AdvancedOrderBase, ITakeProfit
    {
        internal TakeProfitOrder(
            OrderBase order,
            Decimal limitPrice)
            : base(
                order,
                OrderClass.OneTriggersOther) =>
            LimitPrice = limitPrice;

        /// <inheritdoc />
        public Decimal LimitPrice { get; }
        
        internal override JsonNewOrder GetJsonRequest() =>
            base.GetJsonRequest()
                .WithTakeProfit(this);
    }
}
