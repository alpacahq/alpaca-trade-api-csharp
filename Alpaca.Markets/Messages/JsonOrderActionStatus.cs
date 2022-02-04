namespace Alpaca.Markets;

internal sealed class JsonOrderActionStatus : IOrderActionStatus
{
    [JsonProperty(PropertyName = "id", Required = Required.Always)]
    public Guid OrderId { get; set; }

    [JsonIgnore]
    public Boolean IsSuccess => StatusCode.IsSuccessHttpStatusCode();

    [JsonProperty(PropertyName = "status", Required = Required.Always)]
    public Int64 StatusCode { get; set; }
}
