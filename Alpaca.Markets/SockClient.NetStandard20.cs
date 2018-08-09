#if NETSTANDARD2_0

using Microsoft.Extensions.Configuration;

namespace Alpaca.Markets
{
    public sealed partial class SockClient
    {
        /// <summary>
        /// Creates new instance of <see cref="SockClient"/> object.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        public SockClient(
            IConfiguration configuration)
            : this(
                configuration["keyId"],
                configuration["secretKey"],
                configuration["alpacaRestApi"])
        {
        }
    }
}

#endif
