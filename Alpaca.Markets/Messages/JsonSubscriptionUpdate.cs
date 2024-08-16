using MessagePack;

namespace Alpaca.Markets;

[MessagePackObject]
public sealed class JsonSubscriptionUpdate
{
    [Key("action")]
    [JsonProperty(PropertyName = "action", Required = Required.Default)]
    public JsonAction Action { get; set; }

    [Key("trades")]
    [JsonProperty(PropertyName = "trades", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public List<String>? Trades { get; set; }

    [Key("quotes")]
    [JsonProperty(PropertyName = "quotes", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public List<String>? Quotes { get; set; }

    [Key("bars")]
    [JsonProperty(PropertyName = "bars", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public List<String>? MinuteBars { get; set; }

    [Key("dailyBars")]
    [JsonProperty(PropertyName = "dailyBars", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public List<String>? DailyBars { get; set; }

    [Key("statuses")]
    [JsonProperty(PropertyName = "statuses", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public List<String>? Statuses { get; set; }

    [Key("lulds")]
    [JsonProperty(PropertyName = "lulds", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public List<String>? Lulds { get; set; }

    [Key("news")]
    [JsonProperty(PropertyName = "news", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public List<String>? News { get; set; }

    [Key("updatedBars")]
    [JsonProperty(PropertyName = "updatedBars", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public List<String>? UpdatedBars { get; set; }

    [Key("orderbooks")]
    [JsonProperty(PropertyName = "orderbooks", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public List<String>? OrderBooks { get; set; }
}
