using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    /// <summary>
    /// Crypto exchanges supported by Alpaca REST API.
    /// </summary>
    [JsonConverter(typeof(CryptoExchangeEnumConverter))]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public enum CryptoExchange
    {
        /// <summary>
        /// ErisX Exchange.
        /// </summary>
        [UsedImplicitly] [EnumMember(Value = "UNKNOWN")]
        Unknown,

        /// <summary>
        /// ErisX Exchange.
        /// </summary>
        [UsedImplicitly] [EnumMember(Value = "ERSX")]
        Ersx,

        /// <summary>
        /// FTX Exchange.
        /// </summary>
        [UsedImplicitly] [EnumMember(Value = "FTXU")]
        Ftx,

        /// <summary>
        /// Coinbase Exchange.
        /// </summary>
        [UsedImplicitly] [EnumMember(Value = "CBSE")]
        Cbse,

        /// <summary>
        /// Not supported now.
        /// </summary>
        [UsedImplicitly]
        [Obsolete("This enum member is not obsolete and will be deleted in upcoming major release.", true)]
        [EnumMember(Value = "GNSS")]
        Gnss
    }
}
