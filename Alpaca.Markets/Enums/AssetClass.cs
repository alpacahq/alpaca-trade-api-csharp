using System.Runtime.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    /// <summary>
    /// Supported asset classes for Alpaca REST API.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AssetClass
    {
        /// <summary>
        /// US equity asset class.
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "us_equity")]
        UsEquity,

        /// <summary>
        /// Crypto currency asset class.
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "crypto")]
        Crypto
    }
}
