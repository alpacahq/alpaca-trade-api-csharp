using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates base data for any order types, never used directly by any code.
    /// </summary>
    public abstract class OrderBase : Validation.IRequest
    {
        /// <summary>
        /// Creates new instance of the <see cref="OrderBase"/> class.
        /// </summary>
        /// <param name="symbol">Alpaca symbol for order.</param>
        /// <param name="quantity">Order quantity (absolute value).</param>
        /// <param name="side">Order side (buy or sell).</param>
        /// <param name="type">Order type (market, limit, stop, stop-limit).</param>
        protected internal OrderBase(
            String symbol,
            Int64 quantity,
            OrderSide side,
            OrderType type)
        {
            Symbol = symbol ?? throw new ArgumentException(
                "Symbol name cannot be null.", nameof(symbol));
            Quantity = quantity;
            Side = side;
            Type = type;
        }

        /// <summary>
        /// Creates new instance of the <see cref="OrderBase"/> class.
        /// </summary>
        /// <param name="baseOrder">Base order for getting parameters.</param>
        protected internal OrderBase(
            OrderBase baseOrder)
            : this(
                baseOrder.EnsureNotNull(nameof(baseOrder)).Symbol,
                baseOrder.Quantity,
                baseOrder.Side,
                baseOrder.Type)
        {
            Duration = baseOrder.Duration;
            ClientOrderId = baseOrder.ClientOrderId;
            ExtendedHours = baseOrder.ExtendedHours;
        }

        /// <summary>
        /// Gets the new order asset name.
        /// </summary>
        public String Symbol { get; }

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
        public TimeInForce Duration { get; set; } = TimeInForce.Day;

        /// <summary>
        /// Gets or sets the client order ID.
        /// </summary>
        public String? ClientOrderId { get; set; }

        /// <summary>
        /// Gets or sets flag indicating that order should be allowed to execute during extended hours trading.
        /// </summary>
        public Boolean? ExtendedHours { get; set; }

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

        internal virtual JsonNewOrder GetJsonRequest() =>
            new JsonNewOrder
            {
                Symbol = Symbol,
                Quantity = Quantity,
                OrderSide = Side,
                OrderType = Type,
                TimeInForce = Duration,
                ExtendedHours = ExtendedHours,
                ClientOrderId = ClientOrderId
            };
    }
}
