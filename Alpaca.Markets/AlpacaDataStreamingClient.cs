using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca data streaming API via websockets.
    /// </summary>
    public sealed class AlpacaDataStreamingClient :
        StreamingDataClientBase<AlpacaDataStreamingClientConfiguration>, 
        IAlpacaDataStreamingClient
    {
        private const String Listening = "listening";

        private const String Authorization = "authorization";

        private readonly IDictionary<String, Action<JToken>> _handlers;

        /// <summary>
        /// Creates new instance of <see cref="AlpacaDataStreamingClient"/> object.
        /// </summary>
        /// <param name="configuration">Configuration parameters object.</param>
        public AlpacaDataStreamingClient(
            AlpacaDataStreamingClientConfiguration configuration)
            : base(configuration.EnsureNotNull(nameof(configuration))) =>
            _handlers = new Dictionary<String, Action<JToken>>(StringComparer.Ordinal)
            {
                { Listening, handleListeningUpdates },
                { Authorization, handleAuthorization }
            };

        /// <inheritdoc />
        public IAlpacaDataSubscription<IStreamTrade> GetTradeSubscription(
            String symbol) => 
            GetSubscription<IStreamTrade, JsonStreamTradeAlpaca>(TradesChannel, symbol);

        /// <inheritdoc />
        public IAlpacaDataSubscription<IStreamQuote> GetQuoteSubscription(
            String symbol) =>
            GetSubscription<IStreamQuote, JsonStreamQuoteAlpaca>(QuotesChannel, symbol);

        /// <inheritdoc />
        public IAlpacaDataSubscription<IStreamAgg> GetMinuteAggSubscription() => 
            GetSubscription<IStreamAgg, JsonStreamAggAlpaca>(MinuteAggChannel);

        /// <inheritdoc />
        public IAlpacaDataSubscription<IStreamAgg> GetMinuteAggSubscription(
            String symbol) =>
            GetSubscription<IStreamAgg, JsonStreamAggAlpaca>(MinuteAggChannel, symbol);

        /// <inheritdoc/>
        protected override void OnOpened()
        {
            SendAsJsonString(new JsonAuthRequest
            {
                Action = JsonAction.Authenticate,
                Data = Configuration.SecurityId
                    .GetAuthenticationData()
            });

            base.OnOpened();
        }

        /// <inheritdoc />
        protected override void Subscribe(
            IEnumerable<String> streams) =>
            sendSubscriptionRequest(streams, JsonAction.Listen);

        /// <inheritdoc />
        protected override void Unsubscribe(
            IEnumerable<String> streams) =>
            sendSubscriptionRequest(streams, JsonAction.Unlisten);

        /// <inheritdoc/>
        [SuppressMessage(
            "Design", "CA1031:Do not catch general exception types",
            Justification = "Expected behavior - we report exceptions via OnError event.")]
        protected override void OnMessageReceived(
            String message)
        {
            try
            {
                var token = JObject.Parse(message);

                var payload = token["data"];
                var stream = token["stream"];

                if (payload is null ||
                    stream is null)
                {
                    HandleError(new InvalidOperationException());
                }
                else
                {
                    var streamAsString = stream.ToString();
                    if (HandleRealtimeDataUpdate(streamAsString, payload))
                    {
                        return;
                    }
                    HandleMessage(_handlers, streamAsString, payload);
                }
            }
            catch (Exception exception)
            {
                HandleError(exception);
            }
        }

        private void handleAuthorization(
            JToken token)
        {
            var connectionStatus = token.ToObject<JsonConnectionStatus>() ?? new JsonConnectionStatus();

            // ReSharper disable once ConstantConditionalAccessQualifier
            switch (connectionStatus.Status)
            {
                case ConnectionStatus.AlpacaDataStreamingAuthorized:
                    OnConnected(AuthStatus.Authorized);
                    break;

                case ConnectionStatus.AlpacaDataStreamingUnauthorized:
                    OnConnected(AuthStatus.Unauthorized);
                    break;

                default:
                    HandleError(new InvalidOperationException("Unknown connection status"));
                    break;
            }
        }
        
        [SuppressMessage(
            "Design", "CA1031:Do not catch general exception types",
            Justification = "Expected behavior - we report exceptions via OnError event.")]
        private void handleListeningUpdates(
            JToken token)
        {
            var listeningUpdate = token.ToObject<JsonListeningUpdate>() ?? new JsonListeningUpdate();

            if (!String.IsNullOrEmpty(listeningUpdate.Error))
            {
                HandleError(new InvalidOperationException(listeningUpdate.Error));
            }

            foreach (var stream in listeningUpdate.Streams)
            {
                try
                {
                    SubscriptionsOnUpdate(stream);
                }
                catch (Exception exception)
                {
                    HandleError(exception);
                }
            }
        }

        private void sendSubscriptionRequest(
            IEnumerable<String> streams,
            JsonAction action) =>
            SendAsJsonString(new JsonListenRequest
            {
                Action = action,
                Data = new JsonListenRequest.JsonData
                {
                    Streams = streams.ToList()
                }
            });
    }
}
