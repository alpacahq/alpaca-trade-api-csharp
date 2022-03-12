using System.Diagnostics;

namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IAsset))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonAsset : IAsset
{
    [JsonProperty(PropertyName = "id", Required = Required.Always)]
    public Guid AssetId { get; set; }

    [JsonProperty(PropertyName = "class", Required = Required.Default)]
    public AssetClass Class { get; set; }

    [JsonProperty(PropertyName = "exchange", Required = Required.Always)]
    public Exchange Exchange { get; set; }

    [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
    public String Symbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "name", Required = Required.Always)]
    public String Name { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "status", Required = Required.Always)]
    public AssetStatus Status { get; set; }

    [JsonProperty(PropertyName = "tradable", Required = Required.Always)]
    public Boolean IsTradable { get; set; }

    [JsonProperty(PropertyName = "marginable", Required = Required.Default)]
    public Boolean Marginable { get; set; }

    [JsonProperty(PropertyName = "shortable", Required = Required.Default)]
    public Boolean Shortable { get; set; }

    [JsonProperty(PropertyName = "easy_to_borrow", Required = Required.Default)]
    public Boolean EasyToBorrow { get; set; }

    [JsonProperty(PropertyName = "fractionable", Required = Required.Default)]
    public Boolean Fractionable { get; set; }

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(IAsset)} {{ ID = {AssetId:B}, Symbol = \"{Symbol}\", Class = {Class}, Status = {Status}, Exchange = {Exchange} }}";
}
