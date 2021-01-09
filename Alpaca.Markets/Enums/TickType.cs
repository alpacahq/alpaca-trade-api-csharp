using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    /// <summary>
    /// Conditions map type for <see cref="PolygonDataClient.GetConditionMapAsync(System.Threading.CancellationToken)"/> call form Polygon REST API.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum TickType
    {
        /// <summary>
        /// Method <see cref="PolygonDataClient.GetConditionMapAsync(System.Threading.CancellationToken)"/> returns trades conditions.
        /// </summary>
        [EnumMember(Value = "trades")]
        Trades,

        /// <summary>
        /// Method <see cref="PolygonDataClient.GetConditionMapAsync(System.Threading.CancellationToken)"/> returns quotes conditions.
        /// </summary>
        [EnumMember(Value = "quotes")]
        Quotes
    }
}
