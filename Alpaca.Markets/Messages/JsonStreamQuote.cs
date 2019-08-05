﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal class JsonStreamQuote : IStreamQuote
    {
        [JsonProperty(PropertyName = "sym", Required = Required.Always)]
        public String Symbol { get; set; }

        [JsonProperty(PropertyName = "bx", Required = Required.Always)]
        public Int64 BidExchange { get; set; }

        [JsonProperty(PropertyName = "ax", Required = Required.Always)]
        public Int64 AskExchange { get; set; }

        [JsonProperty(PropertyName = "bp", Required = Required.Always)]
        public Decimal BidPrice { get; set; }

        [JsonProperty(PropertyName = "ap", Required = Required.Always)]
        public Decimal AskPrice { get; set; }

        [JsonProperty(PropertyName = "bs", Required = Required.Always)]
        public Int64 BidSize { get; set; }

        [JsonProperty(PropertyName = "as", Required = Required.Always)]
        public Int64 AskSize { get; set; }

        [JsonProperty(PropertyName = "t", Required = Required.Always)]
        public Int64 Timestamp { get; set; }

        [JsonIgnore]
        public DateTime Time { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(
            StreamingContext context)
        {
            Time = DateTimeHelper.FromUnixTimeNanoseconds(Timestamp);
        }
    }
}
