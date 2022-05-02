namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IClock))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonClock : IClock
{
    [JsonProperty(PropertyName = "timestamp", Required = Required.Always)]
    [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
    public DateTime TimestampUtc { get; set; }

    [JsonProperty(PropertyName = "is_open", Required = Required.Always)]
    public Boolean IsOpen { get; set; }

    [JsonProperty(PropertyName = "next_open", Required = Required.Always)]
    [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
    public DateTime NextOpenUtc { get; set; }

    [JsonProperty(PropertyName = "next_close", Required = Required.Always)]
    [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
    public DateTime NextCloseUtc { get; set; }

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(IClock)} {{ Timestamp = {TimestampUtc:O}, IsOpen = {IsOpen}, NextOpen = {NextOpenUtc:0}, NextClose = {NextCloseUtc:0} }}";
}
