﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonDayAgg : IAgg
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

        [JsonProperty(PropertyName = "n", Required = Required.Default)]
        public Int32 ItemsInWindow { get; set; }

        [JsonConverter(typeof(DateConverter), "yyyy-M-d")]
        [JsonProperty(PropertyName = "d", Required = Required.Default)]
        public DateTime Time { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(
            StreamingContext context) =>
            Time = DateTime.SpecifyKind(
                Time.Date, DateTimeKind.Utc);
    }
}
