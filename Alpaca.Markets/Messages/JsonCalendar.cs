﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonCalendar : ICalendar
    {
        [JsonConverter(typeof(DateConverter))]
        [JsonProperty(PropertyName = "date", Required = Required.Always)]
        public DateTime TradingDate { get; set; }

        [JsonConverter(typeof(TimeConverter))]
        [JsonProperty(PropertyName = "open", Required = Required.Always)]
        public DateTime TradingOpenTime { get; set; }

        [JsonConverter(typeof(TimeConverter))]
        [JsonProperty(PropertyName = "close", Required = Required.Always)]
        public DateTime TradingCloseTime { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(
            StreamingContext context)
        {
            TradingDate = DateTime.SpecifyKind(
                TradingDate.Date, DateTimeKind.Unspecified).Date;

            TradingOpenTime = CustomTimeZone.ConvertFromEstToUtc(
                TradingDate, TradingOpenTime);
            TradingCloseTime = CustomTimeZone.ConvertFromEstToUtc(
                TradingDate, TradingCloseTime);

            TradingDate = DateTime.SpecifyKind(
                TradingDate.Date, DateTimeKind.Utc);
        }
    }
}
