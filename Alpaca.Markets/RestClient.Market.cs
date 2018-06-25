using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alpaca.Markets.Helpers;

namespace Alpaca.Markets
{
    public sealed partial class RestClient
    {
        public Task<IEnumerable<IAssetBars>> GetBarsAsync(
            IEnumerable<String> symbol,
            BarDuration barDuration,
            DateTime? startTimeInclusive = null,
            DateTime? endTimeInclusive = null)
        {
            var builder = new UriBuilder(_alpacaHttpClient.BaseAddress)
            {
                Path = "v1/bars",
                Query = new QueryBuilder()
                    .AddParameter("symbols", String.Join(",", symbol))
                    .AddParameter("timeframe", barDuration.ToEnumString())
                    .AddParameter("start_dt", startTimeInclusive)
                    .AddParameter("end_dt", endTimeInclusive)
            };

            return getObjectsListAsync<IAssetBars, JsonAssetBars>(_alpacaHttpClient, builder);
        }

        public Task<IAssetBars> GetBarsAsync(
            String symbol,
            BarDuration barDuration,
            DateTime? startTimeInclusive = null,
            DateTime? endTimeInclusive = null)
        {
            var builder = new UriBuilder(_alpacaHttpClient.BaseAddress)
            {
                Path = $"v1/assets/{symbol}/bars",
                Query = new QueryBuilder()
                    .AddParameter("timeframe ", barDuration.ToEnumString())
                    .AddParameter("start_dt", startTimeInclusive)
                    .AddParameter("end_dt", endTimeInclusive)
            };

            return getSingleObjectAsync<IAssetBars, JsonAssetBars>(_alpacaHttpClient, builder);
        }

        public Task<IEnumerable<IQuote>> GetQuotesAsync(
            IEnumerable<String> symbol)
        {
            var builder = new UriBuilder(_alpacaHttpClient.BaseAddress)
            {
                Path = "v1/quotes",
                Query = new QueryBuilder()
                    .AddParameter("symbols", String.Join(",", symbol))
            };

            return getObjectsListAsync<IQuote, JsonQuote>(_alpacaHttpClient, builder);
        }

        public Task<IQuote> GetQuoteAsync(
            String symbol)
        {
            return getSingleObjectAsync<IQuote, JsonQuote>(
                _alpacaHttpClient, $"v1/assets/{symbol}/quote");
        }

        public Task<IEnumerable<IFundamental>> GetFundamentalsAsync(
            IEnumerable<String> symbol)
        {
            var builder = new UriBuilder(_alpacaHttpClient.BaseAddress)
            {
                Path = "v1/fundamentals",
                Query = new QueryBuilder()
                    .AddParameter("symbols", String.Join(",", symbol))
            };

            return getObjectsListAsync<IFundamental, JsonFundamental>(_alpacaHttpClient, builder);
        }

        public Task<IFundamental> GetFundamentalAsync(
            String symbol)
        {
            return getSingleObjectAsync<IFundamental, JsonFundamental>(
                _alpacaHttpClient, $"v1/assets/{symbol}/fundamental");
        }
    }
}