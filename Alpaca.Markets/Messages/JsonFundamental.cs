using System;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    internal sealed class JsonFundamental : IFundamental
    {
        [JsonProperty(PropertyName = "asset_id", Required = Required.Always)]
        public Guid AssetId { get; set; }

        [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
        public String Symbol { get; set; }

        [JsonProperty(PropertyName = "full_name", Required = Required.Always)]
        public String FullName { get; set; }

        [JsonProperty(PropertyName = "industry_name", Required = Required.Always)]
        public String Industry { get; set; }

        [JsonProperty(PropertyName = "sector", Required = Required.Always)]
        public String Sector { get; set; }

        [JsonProperty(PropertyName = "pe_ratio", Required = Required.Always)]
        public Decimal pe_ratio { get; set; }

        [JsonProperty(PropertyName = "peg_ratio", Required = Required.Always)]
        public Decimal peg_ratio { get; set; }

        [JsonProperty(PropertyName = "beta", Required = Required.Always)]
        public Decimal beta { get; set; }

        [JsonProperty(PropertyName = "eps", Required = Required.Always)]
        public Decimal eps { get; set; }

        [JsonProperty(PropertyName = "market_cap", Required = Required.Always)]
        public Decimal market_cap { get; set; }

        [JsonProperty(PropertyName = "shares_outstanding", Required = Required.Always)]
        public Decimal shares_outstanding { get; set; }

        [JsonProperty(PropertyName = "avg_vol", Required = Required.Always)]
        public Decimal avg_vol { get; set; }

        [JsonProperty(PropertyName = "div_rate", Required = Required.Always)]
        public Decimal div_rate { get; set; }

        [JsonProperty(PropertyName = "roa", Required = Required.Always)]
        public Decimal roa { get; set; }

        [JsonProperty(PropertyName = "roe", Required = Required.Always)]
        public Decimal roe { get; set; }

        [JsonProperty(PropertyName = "ps", Required = Required.Always)]
        public Decimal ps { get; set; }

        [JsonProperty(PropertyName = "pc", Required = Required.Always)]
        public Decimal pc { get; set; }

        [JsonProperty(PropertyName = "gross_margin", Required = Required.Always)]
        public Decimal gross_margin { get; set; }
    }
}