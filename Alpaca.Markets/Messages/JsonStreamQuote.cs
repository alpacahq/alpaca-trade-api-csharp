using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
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

        /// <summary>
        /// Called when the json message is deserialized.
        /// We adjust the milliseconds by deviding it by 10,000 to reduce the time stamp
        /// </summary>
        /// <param name="context">The context.</param>
        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
#if NET45
            Time = DateTimeHelper.FromUnixTimeMilliseconds((Int64)(Timestamp / 10000));
#else
            Time = DateTime.SpecifyKind(DateTimeOffset.FromUnixTimeMilliseconds((Int64)(Timestamp / 10000)).DateTime, DateTimeKind.Utc);
#endif
        }
    }
}