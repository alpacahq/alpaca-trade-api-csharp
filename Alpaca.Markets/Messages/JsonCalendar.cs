using System;
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
        public DateTime TradingDateEst { get; set; }

        [JsonConverter(typeof(TimeConverter))]
        [JsonProperty(PropertyName = "open", Required = Required.Always)]
        public DateTime TradingOpenTimeEst { get; set; }

        [JsonConverter(typeof(TimeConverter))]
        [JsonProperty(PropertyName = "close", Required = Required.Always)]
        public DateTime TradingCloseTimeEst { get; set; }

        [JsonIgnore]
        public DateTime TradingDateUtc { get; private set; }

        [JsonIgnore]
        public DateTime TradingOpenTimeUtc { get; private set; }

        [JsonIgnore]
        public DateTime TradingCloseTimeUtc { get; private set; }

        [JsonIgnore] 
        public DateTime TradingDate => TradingDateUtc;

        [JsonIgnore]
        public DateTime TradingOpenTime => TradingOpenTimeUtc;

        [JsonIgnore] 
        public DateTime TradingCloseTime => TradingCloseTimeUtc;

        [OnDeserialized]
        internal void OnDeserializedMethod(
            StreamingContext context)
        {
            TradingDateEst = DateTime.SpecifyKind(
                TradingDate.Date, DateTimeKind.Unspecified);
            TradingOpenTimeEst = DateTime.SpecifyKind(
                TradingOpenTimeEst, DateTimeKind.Unspecified);
            TradingCloseTimeEst = DateTime.SpecifyKind(
                TradingCloseTimeEst, DateTimeKind.Unspecified);

            TradingOpenTimeUtc = CustomTimeZone.ConvertFromEstToUtc(
                TradingDateEst.Date.Add(TradingOpenTimeEst.TimeOfDay));
            TradingCloseTimeUtc = CustomTimeZone.ConvertFromEstToUtc(
                TradingDateEst.Date.Add(TradingCloseTime.TimeOfDay));

            TradingDateUtc = TradingDateEst.AsUtcDateTime();
        }
    }
}
