using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    /// <summary>
    /// Order statuses sorting direction for <see cref="RestClient.ListOrdersAsync"/> call from Alpaca REST API.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OrderListSorting
    {
        /// <summary>
        /// Returns only open orders.
        /// </summary>
        [EnumMember(Value = "desc")]
        Descending,

        /// <summary>
        /// Returns only closed orders.
        /// </summary>
        [EnumMember(Value = "asc")]
        Ascending
    }
}
