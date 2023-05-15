using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonAccountActivity : IAccountActivity
    {
        private static readonly Char[] _activityIdSeparator = { ':' };

        [JsonProperty(PropertyName = "activity_type", Required = Required.Always)]
        public AccountActivityType ActivityType { get; set; }

        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public String ActivityId { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "symbol", Required = Required.Default)]
        public String? Symbol { get; set; }

        [JsonProperty(PropertyName = "date", Required = Required.Default)]
        public DateTime? ActivityDate { get; set; }

        [JsonProperty(PropertyName = "net_amount", Required = Required.Default)]
        public Decimal? NetAmount { get; set; }

        [JsonProperty(PropertyName = "per_share_amount", Required = Required.Default)]
        public Decimal? PerShareAmount { get; set; }

        [JsonProperty(PropertyName = "qty", Required = Required.Default)]
        public Decimal? Quantity { get; set; }

        [JsonProperty(PropertyName = "cum_qty", Required = Required.Default)]
        public Decimal? CumulativeQuantity { get; set; }

        [JsonProperty(PropertyName = "leaves_qty", Required = Required.Default)]
        public Decimal? LeavesQuantity { get; set; }

        [JsonIgnore]
        public Int64? IntegerQuantity => Quantity?.AsInteger();

        [JsonIgnore]
        public Int64? IntegerCumulativeQuantity => CumulativeQuantity?.AsInteger();

        [JsonIgnore]
        public Int64? IntegerLeavesQuantity => LeavesQuantity?.AsInteger();

        [JsonProperty(PropertyName = "price", Required = Required.Default)]
        public Decimal? Price { get; set; }

        [JsonProperty(PropertyName = "side", Required = Required.Default)]
        public OrderSide? Side { get; set; }

        [JsonProperty(PropertyName = "type", Required = Required.Default)]
        public TradeEvent? Type { get; set; }

        [JsonIgnore]
        public DateTime ActivityDateTimeUtc { get; private set; }

        [JsonProperty(PropertyName = "transaction_time", Required = Required.Default)]
        [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
        public DateTime? TransactionTimeUtc { get; set; }

        [JsonIgnore]
        public Guid ActivityGuid { get; private set; }

        [JsonProperty(PropertyName = "order_id", Required = Required.Default)]
        public Guid? OrderId { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(
            StreamingContext context)
        {
            var components = ActivityId.Split(_activityIdSeparator, StringSplitOptions.RemoveEmptyEntries);

            if (components.Length > 0 &&
                // ReSharper disable once StringLiteralTypo
                DateTime.TryParseExact(components[0], "yyyyMMddHHmmssfff",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
            {
                ActivityDateTimeUtc = CustomTimeZone.ConvertFromEstToUtc(dateTime);
            }

            if (components.Length > 1 &&
                Guid.TryParseExact(components[1], "D", out var guid))
            {
                ActivityGuid = guid;
            }

            ActivityDate = ActivityDate?.Date.AsUtcDateTime();
        }
    }
}
