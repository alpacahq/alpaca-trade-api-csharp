using System.Runtime.Serialization;

namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonMultiQuotesPage<TQuote> : IMultiPageMutable<IQuote>
    where TQuote : IQuote, ISymbolMutable
{
    [JsonProperty(PropertyName = "quotes", Required = Required.Default)]
    public Dictionary<String, List<TQuote>?> ItemsDictionary { get; set; } = new();

    [JsonProperty(PropertyName = "next_page_token", Required = Required.Default)]
    public String? NextPageToken { get; set; }

    [JsonIgnore]
    public IReadOnlyDictionary<String, IReadOnlyList<IQuote>> Items { get; set; } =
        new Dictionary<String, IReadOnlyList<IQuote>>();

    [OnDeserialized]
    [UsedImplicitly]
    internal void OnDeserializedMethod(
        StreamingContext _) =>
        Items = ItemsDictionary.SetSymbol<IQuote, TQuote>();
}
