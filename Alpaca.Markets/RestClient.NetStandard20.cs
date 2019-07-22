#if NETSTANDARD2_0

using System;
using System.Linq;
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
                configuration["keyId"],
                configuration["secretKey"],
                configuration["alpacaRestApi"],
                configuration["polygonRestApi"],
                configuration["alpacaDataApi"],
                toInt32OrNull(configuration["apiVersion"]),
                toInt32OrNull(configuration["dataApiVersion"]),
                toBooleanOrNull(configuration["staging"]),
                new ThrottleParameters(null, null,
                    toInt32OrNull(configuration["maxRetryAttempts"]),
                    configuration.GetSection("retryHttpStatuses")
                    .GetChildren().Select(item => Convert.ToInt32(item.Value))))
        {
        }

        private static Int32? toInt32OrNull(
            String value) => 
            value != null ? Convert.ToInt32(value) : (Int32?)null;

        private static Boolean? toBooleanOrNull(
            String value) =>
            value != null ? Convert.ToBoolean(value) : (Boolean?)null;
    }
}

#endif
