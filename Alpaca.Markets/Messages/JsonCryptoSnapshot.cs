using System.Runtime.Serialization;

namespace Alpaca.Markets;

internal sealed class JsonCryptoSnapshot : ISnapshot
{
    [JsonProperty(PropertyName = "latestQuote", Required = Required.Default)]
    public JsonHistoricalCryptoQuote? JsonQuote { get; set; }

    [JsonProperty(PropertyName = "latestTrade", Required = Required.Default)]
    public JsonHistoricalTrade? JsonTrade { get; set; }

    [JsonProperty(PropertyName = "minuteBar", Required = Required.Default)]
    public JsonHistoricalBar? JsonMinuteBar { get; set; }

    [JsonProperty(PropertyName = "dailyBar", Required = Required.Default)]
    public JsonHistoricalBar? JsonCurrentDailyBar { get; set; }

    [JsonProperty(PropertyName = "prevDailyBar", Required = Required.Default)]
    public JsonHistoricalBar? JsonPreviousDailyBar { get; set; }

    [JsonProperty(PropertyName = "symbol", Required = Required.Default)]
    public String Symbol { get; set; } = String.Empty;

    [JsonIgnore]
    public IQuote? Quote => JsonQuote;

    [JsonIgnore]
    public ITrade? Trade => JsonTrade;

    [JsonIgnore]
    public IBar? MinuteBar => JsonMinuteBar;

    [JsonIgnore]
    public IBar? CurrentDailyBar => JsonCurrentDailyBar;

    [JsonIgnore]
    public IBar? PreviousDailyBar => JsonPreviousDailyBar;

    [OnDeserialized]
    [UsedImplicitly]
    internal void OnDeserializedMethod(
        StreamingContext _)
    {
        JsonTrade?.SetSymbol(Symbol);
        JsonQuote?.SetSymbol(Symbol);
        JsonMinuteBar?.SetSymbol(Symbol);
        JsonCurrentDailyBar?.SetSymbol(Symbol);
        JsonPreviousDailyBar?.SetSymbol(Symbol);
    }
}
