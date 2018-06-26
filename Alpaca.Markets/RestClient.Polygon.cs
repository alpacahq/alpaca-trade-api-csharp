using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Alpaca.Markets.Enums;

namespace Alpaca.Markets
{
    public sealed partial class RestClient
    {
        public Task<IEnumerable<IExchange>> GetExchangesAsync()
        {
            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = "v1/meta/exchanges",
                Query = getDefaultPolygonApiQueryBuilder()
            };

            return getObjectsListAsync<IExchange, JsonExchange>(_polygonHttpClient, builder);
        }

        public Task<IDictionary<String, String>> GetSymbolTypeMapAsync()
        {
            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = "v1/meta/symbol-types",
                Query = getDefaultPolygonApiQueryBuilder()
            };

            return getSingleObjectAsync
                <IDictionary<String,String>,Dictionary <String, String>>(
                    _polygonHttpClient, builder);
        }

        public Task<IDayHistoricalItems<IHistoricalTrade>> GetHistoricalTradesAsync(
            String symbol,
            DateTime date,
            Int64? offset = null,
            Int32? limit = null)
        {
            var dateAsString = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = $"v1/historic/trades/{symbol}/{dateAsString}",
                Query = getDefaultPolygonApiQueryBuilder()
                    .AddParameter("offset", offset)
                    .AddParameter("limit", limit)
            };

            return getSingleObjectAsync
                <IDayHistoricalItems<IHistoricalTrade>,
                    JsonDayHistoricalItems<IHistoricalTrade, JsonHistoricalTrade>>(
                _polygonHttpClient, builder);
        }

        public Task<IDayHistoricalItems<IHistoricalQuote>> GetHistoricalQuotesAsync(
            String symbol,
            DateTime date,
            Int64? offset = null,
            Int32? limit = null)
        {
            var dateAsString = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = $"v1/historic/quotes/{symbol}/{dateAsString}",
                Query = getDefaultPolygonApiQueryBuilder()
                    .AddParameter("offset", offset)
                    .AddParameter("limit", limit)
            };

            return getSingleObjectAsync
                <IDayHistoricalItems<IHistoricalQuote>,
                    JsonDayHistoricalItems<IHistoricalQuote, JsonHistoricalQuote>>(
                _polygonHttpClient, builder);
        }

        public Task<IAggHistoricalItems<IBar>> GetDayAggregatesAsync(
            String symbol,
            DateTime? dateFromInclusive = null,
            DateTime? dateIntoInclusive = null,
            Int32? limit = null)
        {
            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = $"v1/historic/agg/day/{symbol}",
                Query = getDefaultPolygonApiQueryBuilder()
                    .AddParameter("from", dateFromInclusive, "dd-MM-yyyy")
                    .AddParameter("to", dateIntoInclusive, "dd-MM-yyyy")
                    .AddParameter("limit", limit)
            };

            return getSingleObjectAsync
                <IAggHistoricalItems<IBar>,
                    JsonAggHistoricalItems<IBar, JsonDayBar>>(
                _polygonHttpClient, builder);
        }

        public Task<IAggHistoricalItems<IBar>> GetMinuteAggregatesAsync(
            String symbol,
            DateTime? dateFromInclusive = null,
            DateTime? dateIntoInclusive = null,
            Int32? limit = null)
        {
            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = $"v1/historic/agg/minute/{symbol}",
                Query = getDefaultPolygonApiQueryBuilder()
                    .AddParameter("from", dateFromInclusive, "dd-MM-yyyy")
                    .AddParameter("to", dateIntoInclusive, "dd-MM-yyyy")
                    .AddParameter("limit", limit)
            };

            return getSingleObjectAsync
            <IAggHistoricalItems<IBar>,
                JsonAggHistoricalItems<IBar, JsonMinuteBar>>(
                _polygonHttpClient, builder);
        }

        public Task<ILastTrade> GetLastTradeAsync(
            String symbol)
        {
            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = $"v1/last/stocks/{symbol}",
                Query = getDefaultPolygonApiQueryBuilder()
            };

            return getSingleObjectAsync<ILastTrade, JsonLastTrade>(
                _polygonHttpClient, builder);
        }

        public Task<ILastQuote> GetLastQuoteAsync(
            String symbol)
        {
            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = $"v1/last_quote/stocks/{symbol}",
                Query = getDefaultPolygonApiQueryBuilder()
            };

            return getSingleObjectAsync<ILastQuote, JsonLastQuote>(
                _polygonHttpClient, builder);
        }
        public async Task<IDictionary<Int64, String>> GetConditionMapAsync(
            TickType tickType = TickType.Trades)
        {
            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = $"v1/meta/conditions/{tickType.ToEnumString()}",
                Query = getDefaultPolygonApiQueryBuilder()
            };

            var dictionary = await getSingleObjectAsync
                <IDictionary<String, String>, Dictionary<String, String>>(
                    _polygonHttpClient, builder);

            return dictionary
                .ToDictionary(
                    kvp => Int64.Parse(kvp.Key,
                        NumberStyles.Integer, CultureInfo.InvariantCulture),
                    kvp => kvp.Value);
        }

        private QueryBuilder getDefaultPolygonApiQueryBuilder()
        {
            return new QueryBuilder()
                .AddParameter("apiKey", _polygonApiKey)
                .AddParameter("staging", "true");
        }
    }
}