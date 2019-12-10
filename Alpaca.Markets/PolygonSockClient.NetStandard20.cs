#if NETSTANDARD2_0 || NETSTANDARD2_1

using System;
using System.Globalization;
using System.Diagnostics.Contracts;
using Microsoft.Extensions.Configuration;

namespace Alpaca.Markets
{
    public sealed partial class PolygonSockClient
    {
        /// <summary>
        /// Creates new instance of <see cref="PolygonSockClient"/> object.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        /// <param name="webSocketFactory">Factory class for web socket wrapper creation.</param>
        public PolygonSockClient(
            IConfiguration configuration,
            IWebSocketFactory? webSocketFactory = null)
            : this(
                configuration?["keyId"]
                    ?? throw new ArgumentException("Provide 'keyId' configuration parameter.", nameof(configuration)),
                configuration?["polygonWebsocketApi"] ?? "wss://alpaca.socket.polygon.io/stocks",
                Convert.ToBoolean(configuration?["staging"] ?? "false", CultureInfo.InvariantCulture),
                webSocketFactory)
        {
            Contract.Requires(configuration != null);
        }
    }
}

#endif
