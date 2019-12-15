#if NETSTANDARD2_0 || NETSTANDARD2_1

using System;
using System.Linq;
using System.Globalization;
using System.Diagnostics.Contracts;
using Microsoft.Extensions.Configuration;

namespace Alpaca.Markets
{
    public sealed partial class RestClient
    {
        /// <summary>
        /// Creates new instance of <see cref="RestClient"/> object.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        public RestClient(
            IConfiguration configuration)
            : this(
                configuration?["keyId"]
                    ?? throw new ArgumentException("Provide 'keyId' configuration parameter.", nameof(configuration)),
                configuration["secretKey"]
                    ?? throw new ArgumentException("Provide 'secretKey' configuration parameter.", nameof(configuration)),
                configuration["alpacaRestApi"],
                configuration["polygonRestApi"],
                configuration["alpacaDataApi"],
                toInt32OrNull(configuration["apiVersion"]),
                toInt32OrNull(configuration["dataApiVersion"]),
                toBooleanOrNull(configuration["staging"]),
                new ThrottleParameters(null, null,
                    toInt32OrNull(configuration["maxRetryAttempts"]),
                    configuration?.GetSection("retryHttpStatuses")
                    .GetChildren().Select(item => Convert.ToInt32(item.Value, CultureInfo.InvariantCulture))))
        {
            Contract.Requires(configuration != null);
        }

        private static Int32? toInt32OrNull(
            String value) => 
            value != null ? Convert.ToInt32(value, CultureInfo.InvariantCulture) : (Int32?)null;

        private static Boolean? toBooleanOrNull(
            String value) =>
            value != null ? Convert.ToBoolean(value, CultureInfo.InvariantCulture) : (Boolean?)null;
    }
}

#endif
