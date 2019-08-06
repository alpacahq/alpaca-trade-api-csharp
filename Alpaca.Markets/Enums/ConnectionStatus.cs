using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    /// <summary>
    /// Authorization status for Alpaca streaming API.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ConnectionStatus
    {
        /// <summary>
        /// Client successfully connected.
        /// </summary>
        [EnumMember(Value = "connected")]
        Connected,

        /// <summary>
        /// Client successfully authorized.
        /// </summary>
        [EnumMember(Value = "auth_success")]
        Success,

        /// <summary>
        /// Client authentication required.
        /// </summary>
        [EnumMember(Value = "auth_required")]
        AuthenticationRequired,

        /// <summary>
        /// Client authentication failed.
        /// </summary>
        [EnumMember(Value = "auth_failed")]
        AuthenticationFailed
    }
}