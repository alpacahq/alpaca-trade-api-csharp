using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    /// <summary>
    /// Period values for portfolio history in the Alpaca REST API.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum HistoryPeriod
    {
        /// <summary>
        /// 1 month
        /// </summary>
        [EnumMember(Value = "1M")]
        Month,

        /// <summary>
        /// 3 month
        /// </summary>
        [EnumMember(Value = "3M")]
        ThreeMonth,

        /// <summary>
        /// 6 month
        /// </summary>
        [EnumMember(Value = "6M")]
        SixMonth,

        /// <summary>
        /// 1 Year
        /// </summary>
        [EnumMember(Value = "1A")]
        Year,

        /// <summary>
        /// All
        /// </summary>
        [EnumMember(Value = "all")]
        All,

        /// <summary>
        /// Intraday
        /// </summary>
        [EnumMember(Value = "intraday")]
        Intraday,
    }
}
