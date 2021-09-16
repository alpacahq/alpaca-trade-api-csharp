using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    /// <summary>
    /// Supported bar corporate action adjustment types for Alpaca Data API.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Adjustment
    {
        /// <summary>
        /// Raw data - no adjustment.
        /// </summary>
        [EnumMember(Value = "raw")]
        Nothing,

        /// <summary>
        /// Stock split adjustments.
        /// </summary>
        [EnumMember(Value = "split")]
        SplitsOnly,

        /// <summary>
        /// Stock dividend adjustments.
        /// </summary>
        [EnumMember(Value = "dividend")]
        DividendsOnly,

        /// <summary>
        /// Stock splits and dividend adjustments.
        /// </summary>
        [EnumMember(Value = "all")]
        SplitsAndDividends
    }
}
