using System;

namespace Alpaca.Markets
{
    internal static class UriExtensions
    {
        public static Uri AddApiVersionNumberSafe(
            this Uri baseUri,
            ApiVersion apiVersion)
        {
            var builder = new UriBuilder(baseUri);

            if (builder.Path.Equals("/", StringComparison.Ordinal))
            {
                builder.Path = $"{apiVersion.ToEnumString()}/";
            }
            if (!builder.Path.EndsWith("/", StringComparison.Ordinal))
            {
                builder.Path += "/";
            }

            return builder.Uri;
        }
    }
}
