namespace Alpaca.Markets;

/// <summary>
/// Connection and authentication status for Alpaca streaming API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
internal enum ConnectionStatus
{
    /// <summary>
    /// Client successfully connected.
    /// </summary>
    [EnumMember(Value = "connected")]
    Connected,

    /// <summary>
    /// Client successfully authorized.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "auth_success")]
    AuthenticationSuccess,

    /// <summary>
    /// Client authentication required.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "auth_required")]
    AuthenticationRequired,

    /// <summary>
    /// Client authentication failed.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "auth_failed")]
    AuthenticationFailed,

    /// <summary>
    /// Requested operation successfully completed.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "success")]
    Success,

    /// <summary>
    /// Requested operation failed.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "failed")]
    Failed,

    /// <summary>
    /// Client successfully authorized.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "authorized")]
    AlpacaDataStreamingAuthorized,

    /// <summary>
    /// Client authentication failed.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "unauthorized")]
    AlpacaDataStreamingUnauthorized,

    /// <summary>
    /// Client authentication completed.
    /// </summary>
    [EnumMember(Value = "authenticated")]
    Authenticated
}
