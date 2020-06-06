using System;

namespace Alpaca.Markets
{
    internal sealed class PolygonUriBuilder
    {
        private readonly UriBuilder _uriBuilder;

        public PolygonUriBuilder(
            UriBuilder uriBuilder,
            QueryBuilder queryBuilder)
        {
            _uriBuilder = uriBuilder;
            QueryBuilder = queryBuilder;
        }

        public static implicit operator UriBuilder(PolygonUriBuilder builder)
        {
            var uriBuilder = builder._uriBuilder;
            uriBuilder.Query = builder.QueryBuilder;
            return uriBuilder;
        }

        public QueryBuilder QueryBuilder { get; }

        public PolygonUriBuilder WithPath(
            String path)
        {
            _uriBuilder.Path = path;
            return this;
        }
    }
}
