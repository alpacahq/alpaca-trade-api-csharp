using System.Runtime.Serialization;

namespace Alpaca.Markets
{
    /// <summary>
    /// REST API version number.
    /// </summary>
    public enum ApiVersion
    {
        /// <summary>
        /// First version number.
        /// </summary>
        [EnumMember(Value = "v1")]
        V1 = 1,

        /// <summary>
        /// Second version number.
        /// </summary>
        [EnumMember(Value = "v2")]
        V2 = 2
    }
}
