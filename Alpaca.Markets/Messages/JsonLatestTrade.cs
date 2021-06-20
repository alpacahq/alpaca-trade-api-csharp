using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonLatestTrade : ITrade
    {
        [JsonProperty(PropertyName = "trade", Required = Required.Always)]
        public JsonHistoricalTrade Nested { get; set; } = new ();

        [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
        public String Symbol { get; set; } = String.Empty;

        [JsonIgnore]
        public DateTime TimestampUtc => Nested.TimestampUtc;

        [JsonIgnore]
        public UInt64 TradeId => Nested.TradeId;

        [JsonIgnore]
        public String Tape => Nested.Tape;

        [JsonIgnore]
        public String Exchange => Nested.Exchange;

        [JsonIgnore]
        public Decimal Price => Nested.Price;

        [JsonIgnore]
        public UInt64 Size => Nested.Size;

        [JsonIgnore]
        public IReadOnlyList<String> Conditions => Nested.Conditions;
    }
}
