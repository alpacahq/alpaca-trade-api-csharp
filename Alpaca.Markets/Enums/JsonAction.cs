using MessagePack;
using MessagePack.Formatters;

namespace Alpaca.Markets;

[JsonConverter(typeof(StringEnumConverter))]
[MessagePackFormatter(typeof(EnumAsStringFormatter<JsonAction>))]
public enum JsonAction
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
