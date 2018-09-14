using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Alpaca.Markets.Tests
{
    internal static class ClientsFactory
    {
        private static readonly IConfigurationRoot _configuration;

        static ClientsFactory()
        {
            var data = new Dictionary<String, String>
            {
                { "staging", "true" },
                { "keyId", "AKEW7ZBQUSNUHOJNQ5MS" },
                { "secretKey", "Yr2Tms89rQ6foRLNu4pz3w/yXOrxQGDmXctU1BCn" },
                { "alpacaRestApi",  "https://staging-api.tradetalk.us" },
            };

            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(data);

            _configuration = builder.Build();
        }

        public static RestClient GetRestClient() =>
            new RestClient(_configuration);

        public static SockClient GetSockClient() =>
            new SockClient(_configuration);

        public static NatsClient GetNatsClient() =>
            new NatsClient(_configuration);
    }
}
