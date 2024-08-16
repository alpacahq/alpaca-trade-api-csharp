using MessagePack;

namespace Alpaca.Markets;

[MessagePackObject]
[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IQuote))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
public sealed class JsonRealTimeQuote : JsonRealTimeBase, IQuote
{
    [Key("bx")]
    [JsonProperty(PropertyName = "bx", Required = Required.Always)]
    public String BidExchange { get; set; } = String.Empty;

    [Key("ax")]
    [JsonProperty(PropertyName = "ax", Required = Required.Always)]
    public String AskExchange { get; set; } = String.Empty;

    [Key("bp")]
    [JsonProperty(PropertyName = "bp", Required = Required.Always)]
    public Decimal BidPrice { get; set; }

    [Key("ap")]
    [JsonProperty(PropertyName = "ap", Required = Required.Always)]
    public Decimal AskPrice { get; set; }

    [Key("bs")]
    [JsonProperty(PropertyName = "bs", Required = Required.Always)]
    public Decimal BidSize { get; set; }

    [Key("as")]
    [JsonProperty(PropertyName = "as", Required = Required.Always)]
    public Decimal AskSize { get; set; }

    [Key("c")]
    [JsonProperty(PropertyName = "c", Required = Required.Default)]
    [JsonConverter(typeof(SafeListJsonConverter))]
    public List<String> ConditionsList { get; } = [];

    [JsonProperty(PropertyName = "z", Required = Required.Default)]
    public String Tape { get; set; } = String.Empty;

    [JsonIgnore]
    public IReadOnlyList<String> Conditions =>
        ConditionsList.EmptyIfNull();

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        this.ToDebuggerDisplayString();
}
