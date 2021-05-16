using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonSnapshot : ISnapshot
    {
        [JsonProperty(PropertyName = "latestQuote", Required = Required.Always)]
        public JsonHistoricalQuote JsonQuote { get; set; } = new ();

        [JsonProperty(PropertyName = "latestTrade", Required = Required.Always)]
        public JsonHistoricalTrade JsonTrade { get; set; } = new ();

        [JsonProperty(PropertyName = "minuteBar", Required = Required.Always)]
        public JsonHistoricalBar.V2 JsonMinuteBar { get; set; } = new ();

        [JsonProperty(PropertyName = "dailyBar", Required = Required.Always)]
        public JsonHistoricalBar.V2 JsonCurrentDailyBar { get; set; } = new ();

        [JsonProperty(PropertyName = "prevDailyBar", Required = Required.Always)]
        public JsonHistoricalBar.V2 JsonPreviousDailyBar { get; set; } = new ();

        [JsonProperty(PropertyName = "symbol", Required = Required.Default)]
        public String Symbol { get; set; } = String.Empty;

        [JsonIgnore]
        public IQuote Quote => JsonQuote;

        [JsonIgnore]
        public ITrade Trade => JsonTrade;

        [JsonIgnore]
        public IBar MinuteBar => JsonMinuteBar;

        [JsonIgnore]
        public IBar CurrentDailyBar => JsonCurrentDailyBar;

        [JsonIgnore]
        public IBar PreviousDailyBar => JsonPreviousDailyBar;

        [OnDeserialized]
        internal void OnDeserializedMethod(
            StreamingContext context) =>
            WithSymbol(Symbol);

        public ISnapshot WithSymbol(
            String symbol)
        {
            Symbol = symbol;
            JsonTrade.Symbol = Symbol;
            JsonQuote.Symbol = Symbol;
            return this;
        }
    }
}
