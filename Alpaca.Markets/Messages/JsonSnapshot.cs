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
        internal sealed class JsonTrade : IStreamTrade
        {
            [JsonProperty(PropertyName = "i", Required = Required.Default)]
            public String TradeId { get; set; } = String.Empty;

            [JsonProperty(PropertyName = "x", Required = Required.Always)]
            public String Exchange { get; set; } = String.Empty;

            [JsonProperty(PropertyName = "p", Required = Required.Always)]
            public Decimal Price { get; set; }

            [JsonProperty(PropertyName = "s", Required = Required.Always)]
            public Int64 Size { get; set; }

            [JsonProperty(PropertyName = "t", Required = Required.Always)]
            public DateTime TimeUtc { get; set; }

            [JsonIgnore]
            public String Symbol { get; set; } = String.Empty;
        }

        internal sealed class JsonQuote : IStreamQuote
        {
            [JsonProperty(PropertyName = "bx", Required = Required.Always)]
            public String BidExchange { get; set; } = String.Empty;

            [JsonProperty(PropertyName = "ax", Required = Required.Always)]
            public String AskExchange { get; set; } = String.Empty;

            [JsonProperty(PropertyName = "bp", Required = Required.Always)]
            public Decimal BidPrice { get; set; }

            [JsonProperty(PropertyName = "ap", Required = Required.Always)]
            public Decimal AskPrice { get; set; }

            [JsonProperty(PropertyName = "bs", Required = Required.Always)]
            public Int64 BidSize { get; set; }

            [JsonProperty(PropertyName = "as", Required = Required.Always)]
            public Int64 AskSize { get; set; }

            [JsonProperty(PropertyName = "t", Required = Required.Always)]
            public DateTime TimeUtc { get; set; }

            [JsonIgnore]
            public String Symbol { get; set; } = String.Empty;

            [JsonIgnore]
            Int64 IQuoteBase<Int64>.BidExchange => throw new NotImplementedException();

            [JsonIgnore]
            Int64 IQuoteBase<Int64>.AskExchange => throw new NotImplementedException();
        }

        [JsonProperty(PropertyName = "latestQuote", Required = Required.Always)]
        public JsonQuote QuoteObject { get; set; } = new ();

        [JsonProperty(PropertyName = "latestTrade", Required = Required.Always)]
        public JsonTrade TradeObject { get; set; } = new ();

        [JsonProperty(PropertyName = "minuteBar", Required = Required.Always)]
        public JsonAlpacaHistoricalBar JsonMinuteBar { get; set; } = new ();

        [JsonProperty(PropertyName = "dailyBar", Required = Required.Always)]
        public JsonAlpacaHistoricalBar JsonCurrentDailyBar { get; set; } = new ();

        [JsonProperty(PropertyName = "prevDailyBar", Required = Required.Always)]
        public JsonAlpacaHistoricalBar JsonPreviousDailyBar { get; set; } = new ();

        [JsonProperty(PropertyName = "symbol", Required = Required.Default)]
        public String Symbol { get; set; } = String.Empty;

        [JsonIgnore]
        public IStreamQuote Quote => QuoteObject;

        [JsonIgnore]
        public IStreamTrade Trade => TradeObject;

        [JsonIgnore]
        public IAgg MinuteBar => JsonMinuteBar;

        [JsonIgnore]
        public IAgg CurrentDailyBar => JsonCurrentDailyBar;

        [JsonIgnore]
        public IAgg PreviousDailyBar => JsonPreviousDailyBar;

        [OnDeserialized]
        internal void OnDeserializedMethod(
            StreamingContext context) =>
            WithSymbol(Symbol);

        public ISnapshot WithSymbol(
            String symbol)
        {
            Symbol = symbol;
            TradeObject.Symbol = Symbol;
            QuoteObject.Symbol = Symbol;
            return this;
        }
    }
}
