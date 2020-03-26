﻿using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    /// <summary>
    /// Order statuses filter for <see cref="RestClient.ListOrdersAsync"/> call from Alpaca REST API.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum OrderStatusFilter
    {
        /// <summary>
        /// Returns only open orders.
        /// </summary>
        [EnumMember(Value = "open")]
        Open,

        /// <summary>
        /// Returns only closed orders.
        /// </summary>
        [EnumMember(Value = "closed")]
        Closed,

        /// <summary>
        /// Returns all orders.
        /// </summary>
        [EnumMember(Value = "all")]
        All
    }
}
