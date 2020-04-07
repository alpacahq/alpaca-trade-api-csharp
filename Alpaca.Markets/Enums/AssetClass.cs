using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    /// <summary>
    /// Supported asset classes for Alpaca REST API.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum AssetClass
    {
        /// <summary>
        /// US equity asset class.
        /// </summary>
        [EnumMember(Value = "us_equity")]
        UsEquity
    }
}
