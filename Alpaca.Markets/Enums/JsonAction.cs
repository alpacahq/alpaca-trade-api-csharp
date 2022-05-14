namespace Alpaca.Markets;

[JsonConverter(typeof(StringEnumConverter))]
internal enum JsonAction
{
    [EnumMember(Value = "authenticate")]
    Authenticate,

    [EnumMember(Value = "listen")]
    Listen,

    [UsedImplicitly]
    [EnumMember(Value = "unlisten")]
    Unlisten,

    [EnumMember(Value = "auth")]
    StreamingAuthenticate,

    [EnumMember(Value = "subscribe")]
    Subscribe,

    [EnumMember(Value = "unsubscribe")]
    Unsubscribe
}
