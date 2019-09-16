using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    /// <summary>
    /// Notification level for order fill emails.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TradeConfirmEmail
    {
        /// <summary>
        /// Never send email notification for order fills.
        /// </summary>
        [EnumMember(Value = "none")]
        None,

        /// <summary>
        /// Send email notification for all order fills.
        /// </summary>
        [EnumMember(Value = "all")]
        All
    }
}
