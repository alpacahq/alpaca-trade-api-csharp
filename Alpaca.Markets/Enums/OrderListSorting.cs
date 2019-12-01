using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    /// <summary>
    /// Order statuses sorting direction for old <see cref="RestClient.ListOrdersAsync"/> call from Alpaca REST API.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    [Obsolete("Use SortDirection enum instead of this one.", true)]
    public enum OrderListSorting
    {
        /// <summary>
        /// Descending sort order
        /// </summary>
        [EnumMember(Value = "desc")]
        Descending,

        /// <summary>
        /// Ascending sort order
        /// </summary>
        [EnumMember(Value = "asc")]
        Ascending,
    }
}
