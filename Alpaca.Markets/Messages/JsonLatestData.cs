using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonLatestData<TQuote>
    {
        [JsonProperty(PropertyName = "quotes", Required = Required.Default)]
        public Dictionary<String, TQuote> Quotes { get; [ExcludeFromCodeCoverage] set; } = new();

        [JsonProperty(PropertyName = "bars", Required = Required.Default)]
        public Dictionary<String, JsonHistoricalBar> Bars { get; [ExcludeFromCodeCoverage] set; } = new();

        [JsonProperty(PropertyName = "trades", Required = Required.Default)]
        public Dictionary<String, JsonHistoricalTrade> Trades { get; [ExcludeFromCodeCoverage] set; } = new();

        [JsonProperty(PropertyName = "snapshots", Required = Required.Default)]
        public Dictionary<String, JsonCryptoSnapshot> Snapshots { get; [ExcludeFromCodeCoverage] set; } = new ();

        [JsonProperty(PropertyName = "xbbos", Required = Required.Default)]
        public Dictionary<String, JsonHistoricalCryptoQuote> LatestBestBidOffers { get; [ExcludeFromCodeCoverage] set; } = new ();
    }
}
