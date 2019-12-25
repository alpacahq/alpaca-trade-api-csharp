using System;

namespace Alpaca.Markets
{
    internal sealed class StagingEnvironment : IEnvironment
    {
        public Uri AlpacaTradingApi { get; } = new Uri("https://staging-api.tradetalk.us");

        public Uri AlpacaDataApi => Environments.Live.AlpacaDataApi;

        public Uri PolygonDataApi => Environments.Live.PolygonDataApi;

        public Uri AlpacaStreamingApi { get; } = new Uri("wws://staging-api.tradetalk.us/stream");

        public Uri PolygonStreamingApi => Environments.Live.PolygonStreamingApi;
    }
}
