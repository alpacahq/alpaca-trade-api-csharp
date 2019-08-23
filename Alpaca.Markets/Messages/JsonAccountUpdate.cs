using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonAccountUpdate : IAccountUpdate
    {
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public Guid AccountId { get; set; }

        [JsonProperty(PropertyName = "status", Required = Required.Always)]
        public AccountStatus Status { get; set; }

        [JsonProperty(PropertyName = "currency", Required = Required.Default)]
        public String Currency { get; set; }

        [JsonProperty(PropertyName = "cash", Required = Required.Always)]
        public Decimal TradableCash { get; set; }

        [JsonProperty(PropertyName = "cash_withdrawable", Required = Required.Default)]
        public Decimal WithdrawableCash { get; set; }

        [JsonProperty(PropertyName = "created_at", Required = Required.Always)]
        public DateTime CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updated_at", Required = Required.Always)]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "deleted_at", Required = Required.AllowNull)]
        public DateTime? DeletedAt { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(
            StreamingContext context)
        {
            if (string.IsNullOrEmpty(Currency))
            {
                Currency = "USD";
            }
        }
    }
}
