﻿#if NETSTANDARD2_0

using System;
using System.Globalization;
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
            IWebSocketFactory webSocketFactory = null)
            : this(
                configuration?["keyId"],
                configuration?["polygonWebsocketApi"],
                Convert.ToBoolean(configuration?["staging"] ?? "false", CultureInfo.InvariantCulture),
                webSocketFactory)
        {
        }
    }
}

#endif
