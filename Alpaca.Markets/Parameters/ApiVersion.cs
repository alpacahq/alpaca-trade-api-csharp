using System.Runtime.Serialization;

namespace Alpaca.Markets
{
    /// <summary>
    /// 
    /// </summary>
    public enum ApiVersion
    {
        /// <summary>
        /// 
        /// </summary>
        [EnumMember(Value = "v1")]
        V1 = 1,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember(Value = "v2")]
        V2 = 2
    }
}
