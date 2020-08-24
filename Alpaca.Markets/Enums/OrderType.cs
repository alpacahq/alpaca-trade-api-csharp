﻿using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    /// <summary>
    /// Supported order types in Alpaca REST API.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum OrderType
    {
        /// <summary>
        /// Market order (no prices required).
        /// </summary>
        [EnumMember(Value = "market")]
        Market,

        /// <summary>
        /// Stop order (stop price required).
        /// </summary>
        [EnumMember(Value = "stop")]
        Stop,

        /// <summary>
        /// Limit order (limit price required).
        /// </summary>
        [EnumMember(Value = "limit")]
        Limit,

        /// <summary>
        /// Stop limit order (both stop and limit prices required).
        /// </summary>
        [EnumMember(Value = "stop_limit")]
        StopLimit,
        
        /// <summary>
        /// Trailing stop order (dollars or percent offset required).
        /// </summary>
        [EnumMember(Value = "trailing_stop")]
        TrailingStop
    }
}
