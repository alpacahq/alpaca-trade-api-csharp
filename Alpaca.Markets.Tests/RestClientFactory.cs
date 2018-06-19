using System;

namespace Alpaca.Markets.Tests
{
    internal static class RestClientFactory
    {
        public static RestClient GetRestClient() =>
            new RestClient(
                "AKEW7ZBQUSNUHOJNQ5MS",
                "Yr2Tms89rQ6foRLNu4pz3w/yXOrxQGDmXctU1BCn",
                new Uri("https://staging-api.tradetalk.us"));
    }
}
