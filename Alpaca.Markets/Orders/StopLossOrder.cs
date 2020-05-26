﻿using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates data required for placing stop loss order on the Alpaca REST API.
    /// </summary>
    public sealed class StopLossOrder : AdvancedOrderBase, IStopLoss
    {
        internal StopLossOrder(
            OrderBase order,
            Decimal stopPrice,
            Decimal? limitPrice)
            : base(
                order, 
                OrderClass.OneTriggersOther)
        {
            LimitPrice = limitPrice;
            StopPrice = stopPrice;
        }

        /// <inheritdoc />
        public Decimal StopPrice { get; }

        /// <inheritdoc />
        public Decimal? LimitPrice { get; }
        
        internal override JsonNewOrder GetJsonRequest() =>
            base.GetJsonRequest()
                .WithStopLoss(this);
    }
}
