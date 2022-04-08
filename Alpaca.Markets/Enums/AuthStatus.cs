﻿using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets;

/// <summary>
/// Authorization status for Alpaca streaming API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public enum AuthStatus
{
    /// <summary>
    /// Client successfully authorized.
    /// </summary>
    [EnumMember(Value = "authorized")]
    Authorized,

    /// <summary>
    /// Client is not authorized.
    /// </summary>
    [EnumMember(Value = "unauthorized")]
    Unauthorized
}
