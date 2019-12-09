using System;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    internal sealed class JsonNewOrderAttributes
    {
        [JsonProperty(PropertyName = "take_profit_limit_price", Required = Required.Default)]
        public Decimal? TakeProfitLimitPrice { get; set; }

        [JsonProperty(PropertyName = "stop_loss_stop_price", Required = Required.Default)]
        public Decimal? StopLossStopPrice { get; set; }

        [JsonProperty(PropertyName = "stop_loss_limit_price", Required = Required.Default)]
        public Decimal? StopLossLimitPrice { get; set; }
    }
}
