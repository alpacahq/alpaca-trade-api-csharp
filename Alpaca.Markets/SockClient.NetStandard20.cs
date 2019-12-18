#if NETSTANDARD2_0 || NETSTANDARD2_1

using System;
using System.Diagnostics.Contracts;
using Microsoft.Extensions.Configuration;

namespace Alpaca.Markets
{
    public sealed partial class SockClient
    {
        /// <summary>
        /// Creates new instance of <see cref="SockClient"/> object.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        /// <param name="webSocketFactory">Factory class for web socket wrapper creation.</param>
        [Obsolete("This constructor is deprecated and will be removed in upcoming releases.", false)]
        public SockClient(
            IConfiguration configuration,
            IWebSocketFactory? webSocketFactory = null)
            : this(new SockClientConfiguration(configuration, webSocketFactory))
        {
            Contract.Requires(configuration != null);
        }
    }
}

#endif
