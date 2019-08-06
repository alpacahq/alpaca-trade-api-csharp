using System;
#if NETSTANDARD2_0

using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

#endif

namespace Alpaca.Markets.Tests
{
    internal static class ClientsFactory
    {
        private const String KEY_ID = "AKEW7ZBQUSNUHOJNQ5MS";

        private const String SECRET_KEY = "Yr2Tms89rQ6foRLNu4pz3w/yXOrxQGDmXctU1BCn";

        private const String ALPACA_REST_API = "https://staging-api.tradetalk.us";

#if NETSTANDARD2_0

        private static readonly IConfigurationRoot _configuration;

        static ClientsFactory()
        {
            var data = new Dictionary<String, String>
            {
                { "staging", "true" },
                { "keyId", KEY_ID },
                { "secretKey",  SECRET_KEY },
                { "alpacaRestApi", ALPACA_REST_API },
            };

            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(data);

            _configuration = builder.Build();
        }

        public static RestClient GetRestClient() =>
            new RestClient(_configuration);

        public static SockClient GetSockClient() =>
            new SockClient(_configuration);

        public static PolygonSockClient GetPolygonSockClient() =>
            new PolygonSockClient(_configuration);

#else

        public static RestClient GetRestClient() =>
            new RestClient(KEY_ID, SECRET_KEY, ALPACA_REST_API, isStagingEnvironment: true);

        public static SockClient GetSockClient() =>
            new SockClient(KEY_ID, SECRET_KEY, ALPACA_REST_API);

        public static PolygonSockClient GetPolygonSockClient() =>
            new PolygonSockClient(KEY_ID, isStagingEnvironment: true);

#endif
    }
}
