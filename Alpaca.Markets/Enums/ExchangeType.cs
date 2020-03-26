﻿using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    /// <summary>
    /// Supported exchange types in Polygon REST API.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum ExchangeType
    {
        /// <summary>
        /// Ordinal exchange.
        /// </summary>
        [EnumMember(Value = "exchange")]
        Exchange,

        /// <summary>
        /// Banking organization.
        /// </summary>
        [EnumMember(Value = "banking")]
        Banking,

        /// <summary>
        /// Trade reporting facility.
        /// </summary>
        [EnumMember(Value = "TRF")]
        TradeReportingFacility
    }
}
