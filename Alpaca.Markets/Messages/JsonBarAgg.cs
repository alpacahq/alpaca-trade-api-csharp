﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonBarAgg : IAgg
    {
        [JsonProperty(PropertyName = "o", Required = Required.Always)]
        public Decimal Open { get; set; }

        [JsonProperty(PropertyName = "c", Required = Required.Always)]
        public Decimal Close { get; set; }

        [JsonProperty(PropertyName = "l", Required = Required.Always)]
        public Decimal Low { get; set; }

        [JsonProperty(PropertyName = "h", Required = Required.Always)]
        public Decimal High { get; set; }

        [JsonProperty(PropertyName = "v", Required = Required.Always)]
        public Int64 Volume { get; set; }

        [JsonProperty(PropertyName = "t", Required = Required.Default)]
        public Int64 TimeOffset { get; set; }

        [JsonProperty(PropertyName = "n", Required = Required.Default)]
        public Int32 ItemsInWindow { get; set; }

        [JsonIgnore]
        public DateTime Time { get; private set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(
            StreamingContext context) =>
            Time = DateTimeHelper.FromUnixTimeSeconds(TimeOffset);
    }
}

