using System;

namespace Alpaca.Markets.Tests
{
    internal static class ClientsFactory
    {
        private const String KEY_ID = "AKEW7ZBQUSNUHOJNQ5MS";

        private const String SECRET_KEY = "Yr2Tms89rQ6foRLNu4pz3w/yXOrxQGDmXctU1BCn";

        private const String API_URL = "https://staging-api.tradetalk.us";

        public static RestClient GetRestClient() =>
            new RestClient(KEY_ID, SECRET_KEY, new Uri(API_URL));

        public static SockClient GetSockClient() =>
            new SockClient(KEY_ID, SECRET_KEY, new Uri(API_URL));

        public static NatsClient GetNatsClient() =>
            new NatsClient(KEY_ID + "-staging");
    }
}
