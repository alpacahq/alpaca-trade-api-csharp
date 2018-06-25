using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    public sealed partial class RestClient
    {
        public Task<IEnumerable<IExchange>> GetExchangesAsync()
        {
            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = "v1/meta/exchanges",
                Query = new QueryBuilder()
                    .AddParameter("apiKey", _polygonApiKey)
            };

            return getObjectsListAsync<IExchange, JsonExchange>(_polygonHttpClient, builder);
        }

        public Task<IDictionary<String, String>> GetSymbolTypeMapAsync()
        {
            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = "v1/meta/exchanges",
                Query = new QueryBuilder()
                    .AddParameter("apiKey", _polygonApiKey)
            };

            return getSingleObjectAsync
                <IDictionary<String,String>,Dictionary <String, String>>(
                    _polygonHttpClient, builder);
        }

        public Task<IHistoricalItems<IHistoricalTrade>> GetHistoricalTradesAsync(
            String symbol,
            DateTime date,
            Int64? offset = null,
            Int32? limit = null)
        {
            var dateAsString = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = $"v1//v1/historic/trades/{symbol}/{dateAsString}",
                Query = new QueryBuilder()
                    .AddParameter("apiKey", _polygonApiKey)
                    .AddParameter("offset", offset)
                    .AddParameter("limit", limit)
            };

            return getSingleObjectAsync
                <IHistoricalItems<IHistoricalTrade>, JsonHistoricalItems<IHistoricalTrade, JsonHistoricalTrade>>(
                _polygonHttpClient, builder);
        }

        public Task<IHistoricalItems<IHistoricalQuote>> GetHistoricalQuotesAsync(
            String symbol,
            DateTime date,
            Int64? offset = null,
            Int32? limit = null)
        {
            var dateAsString = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = $"v1//v1/historic/quotes/{symbol}/{dateAsString}",
                Query = new QueryBuilder()
                    .AddParameter("apiKey", _polygonApiKey)
                    .AddParameter("offset", offset)
                    .AddParameter("limit", limit)
            };

            return getSingleObjectAsync
                <IHistoricalItems<IHistoricalQuote>, JsonHistoricalItems<IHistoricalQuote, JsonHistoricalQuote>>(
                    _polygonHttpClient, builder);
        }
    }
}