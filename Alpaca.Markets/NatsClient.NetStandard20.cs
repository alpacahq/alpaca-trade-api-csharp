#if NETSTANDARD2_0

using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Alpaca.Markets
{
    public sealed partial class NatsClient
    {
        /// <summary>
        /// Creates new instance of <see cref="NatsClient"/> object.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        public NatsClient(
            IConfiguration configuration)
            : this(
                configuration["keyId"],
                Convert.ToBoolean(configuration["staging"] ?? "false"),
                configuration.GetSection("natsServers")
                    .GetChildren().Select(_ => _.Value))
        {
        }
    }
}

#endif
