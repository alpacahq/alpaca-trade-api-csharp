using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaTradingClient.PostOrderAsync(NewOrderRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    public sealed class NewOrderRequest : Validation.IRequest
    {
        /// <summary>
        /// Creates new instance of <see cref="NewOrderRequest"/> object.
        /// </summary>
        /// <param name="symbol">Order asset name.</param>
        /// <param name="quantity">Order quantity.</param>
        /// <param name="side">Order side (buy or sell).</param>
        /// <param name="type">Order type.</param>
        /// <param name="duration">Order duration.</param>
        public NewOrderRequest(
            String symbol,
            Int64 quantity,
            OrderSide side,
            OrderType type,
            TimeInForce duration)
        {
            Symbol = symbol ?? throw new ArgumentException(
                "Symbol name cannot be null.", nameof(symbol));
            Quantity = quantity;
            Side = side;
            Type = type;
            Duration = duration;
        }

        /// <summary>
        /// Gets the new order asset name.
        /// </summary>
        public String Symbol { get;  }

        /// <summary>
        /// Gets the new order quantity.
        /// </summary>
        public Int64 Quantity { get; }

        /// <summary>
        /// Gets the new order side (buy or sell).
        /// </summary>
        public OrderSide Side { get; }

        /// <summary>
        /// Gets the new order type.
        /// </summary>
        public OrderType Type { get; }

        /// <summary>
        /// Gets the new order duration.
        /// </summary>
        public TimeInForce Duration { get; }

        /// <summary>
        /// Gets or sets the new order limit price.
        /// </summary>
        public Decimal? LimitPrice { get; set; }

        /// <summary>
        /// Gets or sets the new order stop price.
        /// </summary>
        public Decimal? StopPrice { get; set; }

        /// <summary>
        /// Gets or sets the new trailing order trail price offset in dollars.
        /// </summary>
        public Decimal? TrailOffsetInDollars { get; set; }

        /// <summary>
        /// Gets or sets the new trailing order trail price offset in percent.
        /// </summary>
        public Decimal? TrailOffsetInPercent { get; set; }

        /// <summary>
        /// Gets or sets the client order ID.
        /// </summary>
        public String? ClientOrderId { get; set; }

        /// <summary>
        /// Gets or sets flag indicating that order should be allowed to execute during extended hours trading.
        /// </summary>
        public Boolean? ExtendedHours { get; set; }

        /// <summary>
        /// Gets or sets the order class for advanced order types.
        /// </summary>
        public OrderClass? OrderClass { get; set; }

        /// <summary>
        /// Gets or sets the profit taking limit price for advanced order types.
        /// </summary>
        public Decimal? TakeProfitLimitPrice { get; set; }

        /// <summary>
        /// Gets or sets the stop loss stop price for advanced order types.
        /// </summary>
        public Decimal? StopLossStopPrice { get; set; }

        /// <summary>
        /// Gets or sets the stop loss limit price for advanced order types.
        /// </summary>
        public Decimal? StopLossLimitPrice { get; set; }
        
        IEnumerable<RequestValidationException> Validation.IRequest.GetExceptions()
        {
            ClientOrderId = ClientOrderId?.ValidateClientOrderId();

            if (String.IsNullOrEmpty(Symbol))
            {
                yield return new RequestValidationException(
                    "Symbols shouldn't be empty.", nameof(Symbol));
            }

            if (Quantity <= 0)
            {
                yield return new RequestValidationException(
                    "Order quantity should be positive value.", nameof(Quantity));
            }
        }

        internal JsonNewOrder GetJsonRequest() =>
            new JsonNewOrder
            {
                Symbol = Symbol,
                Quantity = Quantity,
                OrderSide = Side,
                OrderType = Type,
                TimeInForce = Duration,
                LimitPrice = LimitPrice,
                StopPrice = StopPrice,
                TrailOffsetInDollars = TrailOffsetInDollars,
                TrailOffsetInPercent = TrailOffsetInPercent,
                ClientOrderId = ClientOrderId,
                ExtendedHours = ExtendedHours,
                OrderClass = OrderClass,
                TakeProfit = TakeProfitLimitPrice != null
                    ? new JsonNewOrderAdvancedAttributes
                    {
                        LimitPrice = TakeProfitLimitPrice
                    }
                    : null,
                StopLoss = StopLossStopPrice != null ||
                           StopLossLimitPrice != null
                    ? new JsonNewOrderAdvancedAttributes
                    {
                        StopPrice = StopLossStopPrice,
                        LimitPrice = StopLossLimitPrice
                    }
                    : null
            };
    }
}
