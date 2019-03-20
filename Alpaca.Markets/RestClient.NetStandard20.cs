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
                Convert.ToBoolean(configuration["staging"] ?? "false"),
                Convert.ToInt32(configuration["maxRetryAttempts"] ?? "5"),
                configuration.GetSection("retryHttpStatuses")
                    .GetChildren().ToList().ConvertAll<Int32>((ci) => Convert.ToInt32(ci.Value)),
                configuration["apiVersion"])
        {
        }
    }
}

#endif