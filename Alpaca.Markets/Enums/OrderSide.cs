﻿using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    /// <summary>
    /// Order side in Alpaca REST API.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OrderSide
    {
        /// <summary>
        /// Buy order.
        /// </summary>
        [EnumMember(Value = "buy")]
        Buy,

        /// <summary>
        /// Sell order.
        /// </summary>
        [EnumMember(Value = "sell")]
        Sell,

        /// <summary>
        /// Sell order.
        /// </summary>
        [EnumMember(Value = "short_sell")]
        ShortSell
    }
}
