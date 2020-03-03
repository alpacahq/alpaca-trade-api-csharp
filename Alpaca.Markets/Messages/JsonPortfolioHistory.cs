using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonPortfolioHistory : IPortfolioHistory
    {
        [JsonProperty(PropertyName = "equity", Required = Required.Always)]
        public List<Decimal?>? EquityList { get; set; }

        [JsonIgnore]
        public IReadOnlyList<Decimal?> Equity => EquityList.EmptyIfNull<Decimal?>();

        [JsonProperty(PropertyName = "profit_loss", Required = Required.Always)]
        public List<Decimal?>? ProfitLossList { get; set; }

        [JsonIgnore]
        public IReadOnlyList<Decimal?> ProfitLoss => ProfitLossList.EmptyIfNull<Decimal?>();

        [JsonProperty(PropertyName = "profit_loss_pct", Required = Required.Always)]
        public List<Decimal?>? ProfitLossPctList { get; set; }

        [JsonIgnore]
        public IReadOnlyList<Decimal?> ProfitLossPct => ProfitLossPctList.EmptyIfNull<Decimal?>();

        [JsonProperty(PropertyName = "timestamp", Required = Required.Always)]
        public List<Int64>? TimestampsList { get; set; }

        [JsonIgnore]
        public IReadOnlyList<DateTime> Timestamps => TimestampsList.EmptyIfNull<Int64>().Select(s => DateTimeHelper.FromUnixTimeMilliseconds(s)).ToList();

        [JsonProperty(PropertyName = "timeframe", Required = Required.Always)]
        public TimeFrame Timeframe { get; set; }

        [JsonProperty(PropertyName = "base_value", Required = Required.Always)]
        public Decimal BaseValue { get; set; }
    }
}
