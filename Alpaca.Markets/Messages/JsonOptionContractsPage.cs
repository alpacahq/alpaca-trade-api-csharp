namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonOptionContractsPage : IPage<IOptionContract>
{
    [JsonProperty(PropertyName = "option_contracts", Required = Required.Always)]
    public List<JsonOptionContract> Contracts { get; [ExcludeFromCodeCoverage] set; } = [];

    [JsonProperty(PropertyName = "next_page_token", Required = Required.Default)]
    public String? NextPageToken { get; set; }

    [JsonIgnore]
    [ExcludeFromCodeCoverage]
    public String Symbol => String.Empty;

    public IReadOnlyList<IOptionContract> Items => Contracts.EmptyIfNull();
}
