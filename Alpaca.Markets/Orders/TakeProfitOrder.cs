﻿using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates data required for placing take profit order on the Alpaca REST API.
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
