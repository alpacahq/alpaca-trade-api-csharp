namespace Alpaca.Markets;

internal sealed class JsonPositionActionStatus : IPositionActionStatus
{
    [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
    public String Symbol { get; set; } = String.Empty;

    [JsonIgnore]
    public Boolean IsSuccess => StatusCode.IsSuccessHttpStatusCode();

    [JsonProperty(PropertyName = "status", Required = Required.Always)]
    public Int64 StatusCode { get; set; }
}
