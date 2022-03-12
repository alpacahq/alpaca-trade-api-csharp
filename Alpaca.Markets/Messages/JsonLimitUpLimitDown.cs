using System.Diagnostics;

namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(ILimitUpLimitDown))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonLimitUpLimitDown : JsonRealTimeBase, ILimitUpLimitDown
{
    [JsonProperty(PropertyName = "u", Required = Required.Default)]
    public Decimal LimitUpPrice { get; }

    [JsonProperty(PropertyName = "d", Required = Required.Default)]
    public Decimal LimitDownPrice { get; }

    [JsonProperty(PropertyName = "i", Required = Required.Default)]
    public String Indicator { get; } = String.Empty;

    [JsonProperty(PropertyName = "z", Required = Required.Default)]
    public String Tape { get; } = String.Empty;

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(IAccountActivity)} {{ LimitUpPrice = {LimitUpPrice}, LimitDownPrice = {LimitDownPrice}, Tape = \"{Tape}\" }}";
}
