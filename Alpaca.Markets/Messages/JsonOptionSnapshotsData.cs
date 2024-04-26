namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IDictionaryPage<IOptionSnapshot>) + "<" + nameof(IOptionSnapshot) + ">")]
internal sealed class JsonOptionsSnapshotData : IDictionaryPage<IOptionSnapshot>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [JsonProperty(PropertyName = "snapshots", Required = Required.Default)]
    public Dictionary<String, JsonOptionSnapshot> ItemsList { get; [ExcludeFromCodeCoverage] set; } = new();

    [JsonProperty(PropertyName = "next_page_token", Required = Required.Default)]
    public String? NextPageToken { get; set; }

    [JsonIgnore]
    public IReadOnlyDictionary<String, IOptionSnapshot> Items { get; [ExcludeFromCodeCoverage] private set; }
        = new Dictionary<String, IOptionSnapshot>();

    [OnDeserialized]
    [UsedImplicitly]
    internal void OnDeserializedMethod(
        StreamingContext _) =>
        Items = ItemsList.ToDictionary(
            kvp => kvp.Key,
            withSymbol<IOptionSnapshot, JsonOptionSnapshot>,
            StringComparer.Ordinal);

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        this.ToDebuggerDisplayString();

    private static TApi withSymbol<TApi, TJson>(
        KeyValuePair<String, TJson> kvp)
        where TJson : TApi, ISymbolMutable
    {
        kvp.Value.SetSymbol(kvp.Key);
        return kvp.Value;
    }
}
