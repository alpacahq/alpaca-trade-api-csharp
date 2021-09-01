using System.Runtime.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    /// <summary>
    /// Supported order durations in Alpaca REST API.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TimeInForce
    {
        /// <summary>
        /// The order is good for the day, and it will be canceled automatically at the end of market hours.
        /// </summary>
        [EnumMember(Value = "day")]
        Day,

        /// <summary>
        /// The order is good until canceled.
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "gtc")]
        Gtc,

        /// <summary>
        /// The order is placed at the time the market opens.
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "opg")]
        Opg,

        /// <summary>
        /// The order is immediately filled or canceled after being placed (may partial fill).
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "ioc")]
        Ioc,

        /// <summary>
        /// The order is immediately filled or canceled after being placed (may not partial fill).
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "fok")]
        Fok,

        /// <summary>
        /// The order will become a limit order if a limit price is specified or a market order otherwise at market close.
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "cls")]
        Cls
    }
}
