using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonLastTradePolygon : ILastTrade
    {
        internal struct Last : IEquatable<Last>
        {
            [JsonProperty(PropertyName = "exchange", Required = Required.Always)]
            public Int64 Exchange { get; set; }

            [JsonProperty(PropertyName = "price", Required = Required.Always)]
            public Decimal Price { get; set; }

            [JsonProperty(PropertyName = "size", Required = Required.Always)]
            public Int64 Size { get; set; }

            [JsonProperty(PropertyName = "timestamp", Required = Required.Always)]
            [JsonConverter(typeof(UnixMillisecondsDateTimeConverter))]
            public DateTime Timestamp { get; set; }

            public Boolean Equals(Last other) =>
                Timestamp.Equals(other.Timestamp) &&
                Exchange == other.Exchange &&
                Price == other.Price &&
                Size == other.Size;

            public override Boolean Equals(Object? obj) => 
                obj is Last other && Equals(other);

            [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
            public override Int32 GetHashCode()
            {
                unchecked
                {
                    var hashCode = Exchange.GetHashCode();
                    hashCode = (hashCode * 397) ^ Price.GetHashCode();
                    hashCode = (hashCode * 397) ^ Size.GetHashCode();
                    hashCode = (hashCode * 397) ^ Timestamp.GetHashCode();
                    return hashCode;
                }
            }
        }

        [JsonProperty(PropertyName = "last", Required = Required.Always)]
        public Last Nested { get; set; }

        [JsonProperty(PropertyName = "status", Required = Required.Always)]
        public String Status { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
        public String Symbol { get; set; } = String.Empty;

        [JsonIgnore]
        public Int64 Exchange => Nested.Exchange;

        [JsonIgnore]
        public Decimal Price => Nested.Price;

        [JsonIgnore]
        public Int64 Size => Nested.Size;

        [JsonIgnore]
        public DateTime TimeUtc => Nested.Timestamp;
    }
}
