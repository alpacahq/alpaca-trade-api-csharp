#if NETSTANDARD2_0

using System;
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
                Convert.ToBoolean(configuration["staging"] ?? "false"))
        {
        }
    }
}

#endif