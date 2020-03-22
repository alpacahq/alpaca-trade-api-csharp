using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    /// <summary>
    /// Single asset status in Alpaca REST API.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum AssetStatus
    {
        /// <summary>
        /// Active asset.
        /// </summary>
        [EnumMember(Value = "active")]
        Active,

        /// <summary>
        /// Inactive asset.
        /// </summary>
        [EnumMember(Value = "inactive")]
        Inactive,

        /// <summary>
        /// Delisted asset.
        /// </summary>
        [EnumMember(Value = "delisted")]
        Delisted
    }
}
