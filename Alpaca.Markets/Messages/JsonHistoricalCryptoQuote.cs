using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonHistoricalCryptoQuote : IQuote, ISymbolMutable
    {
        [JsonProperty(PropertyName = "t", Required = Required.Always)]
        public DateTime TimestampUtc { get; set; }

        [JsonProperty(PropertyName = "x", Required = Required.Always)]
        public String AskExchange { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "ap", Required = Required.Default)]
        public Decimal AskPrice { get; set; }

        [JsonProperty(PropertyName = "as", Required = Required.Default)]
        public Decimal AskSize { get; set; }

        [JsonProperty(PropertyName = "bp", Required = Required.Default)]
        public Decimal BidPrice { get; set; }

        [JsonProperty(PropertyName = "bs", Required = Required.Default)]
        public Decimal BidSize { get; set; }

        [JsonIgnore]
        public String Symbol { get; private set; } = String.Empty;

        [JsonIgnore]
        public IReadOnlyList<String> Conditions => Array.Empty<String>();

        [JsonIgnore]
        public String BidExchange => AskExchange;

        [JsonIgnore]
        public String Tape => String.Empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetSymbol(String symbol) => Symbol = symbol;
    }
}
