
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaDataClient.ListNewsArticlesAsync(NewsArticlesRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    public sealed class NewsArticlesRequest : Validation.IRequest
    {
        private readonly HashSet<String> _symbols = new (StringComparer.Ordinal);

        /// <summary>
        /// Creates new instance of <see cref="NewsArticlesRequest"/> object.
        /// </summary>
        public NewsArticlesRequest()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="NewsArticlesRequest"/> object.
        /// </summary>
        /// <param name="symbols">Asset names for data retrieval.</param>
        public NewsArticlesRequest(
            IEnumerable<String> symbols) =>
            _symbols.UnionWith(symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets assets names list for data retrieval.
        /// </summary>
        public IReadOnlyCollection<String> Symbols => _symbols;

        /// <summary>
        /// Gets or sets inclusive date interval for filtering items in response.
        /// </summary>
        [UsedImplicitly]
        public IInclusiveTimeInterval? TimeInterval { get; set; }

        /// <summary>
        /// Gets or sets articles sorting (by <see cref="INewsArticle.UpdatedAtUtc"/> property) direction.
        /// </summary>
        [UsedImplicitly]
        public SortDirection? SortDirection { get; set; }

        /// <summary>
        /// Gets or sets flag for sending <see cref="INewsArticle.Content"/> property value for each news article.
        /// </summary>
        [UsedImplicitly]
        public Boolean? SendFullContentForItems { get; set; }

        /// <summary>
        /// Gets or sets flag for excluding news articles that do not contain <see cref="INewsArticle.Content"/>
        /// property value (just <see cref="INewsArticle.Headline"/> and <see cref="INewsArticle.Summary"/> values).
        /// </summary>
        [UsedImplicitly]
        public Boolean? ExcludeItemsWithoutContent { get; set; }

        /// <summary>
        /// Gets the pagination parameters for the request (page size and token).
        /// </summary>
        [UsedImplicitly]
        public Pagination Pagination { get; } = new ();

        internal async ValueTask<UriBuilder> GetUriBuilderAsync(
            HttpClient httpClient) =>
            new UriBuilder(httpClient.BaseAddress!)
            {
                Query = await Pagination.QueryBuilder
                        .AddParameter("symbols", Symbols)
                        .AddParameter("start", TimeInterval?.From, "O")
                        .AddParameter("end", TimeInterval?.Into, "O")
                        .AddParameter("sort", SortDirection)
                        .AddParameter("include_content", SendFullContentForItems)
                        .AddParameter("exclude_contentless", ExcludeItemsWithoutContent)
                    .AsStringAsync().ConfigureAwait(false)
            }.AppendPath("../../v1beta1/news");

        IEnumerable<RequestValidationException> Validation.IRequest.GetExceptions()
        {
            if (Symbols.Any(String.IsNullOrEmpty))
            {
                yield return new RequestValidationException(
                    "Symbol shouldn't be empty.", nameof(Symbols));
            }

            if (Pagination is not Validation.IRequest validation)
            {
                yield break;
            }

            foreach (var exception in validation.GetExceptions())
            {
                yield return exception;
            }
        }
    }
}
