using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    /// <summary>
    /// Exchanges supported by Alpaca REST API.
    /// </summary>
    [JsonConverter(typeof(ExchangeEnumConverter))]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public enum Exchange
    {
        /// <summary>
        /// Unknown exchange (not supported by this version of SDK).
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "UNKNOWN")]
        Unknown,

        /// <summary>
        /// NYSE American Stock Exchange.
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "NYSEMKT")]
        NyseMkt,

        /// <summary>
        /// NYSE Arca Stock Exchange.
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "NYSEARCA")]
        NyseArca,

        /// <summary>
        /// New York Stock Exchange (NYSE)
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "NYSE")]
        Nyse,

        /// <summary>
        /// Nasdaq Stock Market.
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "NASDAQ")]
        Nasdaq,

        /// <summary>
        /// BATS Global Market.
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "BATS")]
        Bats,

        /// <summary>
        /// American Stock Exchange (AMEX)
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "AMEX")]
        Amex,

        /// <summary>
        /// Archipelago Stock Exchange (ARCA).
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "ARCA")]
        Arca,

        /// <summary>
        /// International Exchange (IEX).
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "IEX")]
        Iex
    }
}
