﻿using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    /// <summary>
    /// Day trade margin call protection mode for account. See more information here:
    /// https://docs.alpaca.markets/user-protections/#day-trade-margin-call-dtmc-protection-at-alpaca
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DayTradeMarginCallProtection
    {
        /// <summary>
        /// Check rules on position entry.
        /// </summary>
        [EnumMember(Value = "entry")]
        Entry,

        /// <summary>
        /// Check rules on position exit.
        /// </summary>
        [EnumMember(Value = "exit")]
        Exit,

        /// <summary>
        /// Check rules on position entry and exit.
        /// </summary>
        [EnumMember(Value = "both")]
        Both,
    }
}
