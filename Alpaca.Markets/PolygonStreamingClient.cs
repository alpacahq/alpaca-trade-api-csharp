using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Polygon streaming API via websockets.
    /// </summary>
    public sealed class PolygonStreamingClient :
        StreamingDataClientBase<PolygonStreamingClientConfiguration>,
        IPolygonStreamingClient
    {
        private const String StatusMessage = "status";

        private readonly IDictionary<String, Action<JToken>> _handlers;

        /// <summary>
        /// Creates new instance of <see cref="PolygonStreamingClient"/> object.
        /// </summary>
        /// <param name="configuration">Configuration parameters object.</param>
        public PolygonStreamingClient(
            PolygonStreamingClientConfiguration configuration)
            : base(configuration.EnsureNotNull(nameof(configuration))) =>
            _handlers = new Dictionary<String, Action<JToken>>(StringComparer.Ordinal)
            {
                { StatusMessage, handleAuthorization }
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

        /// <inheritdoc />
        public IAlpacaDataSubscription<IStreamAgg> GetSecondAggSubscription() => 
            GetSubscription<IStreamAgg, JsonStreamAggAlpaca>(SecondAggChannel);

        /// <inheritdoc />
        public IAlpacaDataSubscription<IStreamAgg> GetSecondAggSubscription(
            String symbol) =>
            GetSubscription<IStreamAgg, JsonStreamAggAlpaca>(SecondAggChannel, symbol);

        /// <inheritdoc />
        protected override void Subscribe(
            IEnumerable<String> streams) =>
            SendAsJsonString(new JsonListenRequest
            {
                Action = JsonAction.PolygonSubscribe,
                Params = getParams(streams)
            });

        /// <inheritdoc />
        protected override void Unsubscribe(
            IEnumerable<String> streams) =>
            SendAsJsonString(new JsonUnsubscribeRequest
            {
                Action = JsonAction.PolygonUnsubscribe,
                Params = getParams(streams)
            });

        /// <inheritdoc/>
        [SuppressMessage(
            "Design", "CA1031:Do not catch general exception types",
            Justification = "Expected behavior - we report exceptions via OnError event.")]
        protected override void OnMessageReceived(
            String message)
        {
            try
            {
                foreach (var token in JArray.Parse(message))
                {
                    var messageType = token["ev"];
                    if (messageType is null)
                    {
                        HandleError(new InvalidOperationException());
                    }
                    else
                    {
                        var stream = messageType.ToString();
                        if (HandleRealtimeDataUpdate(stream, token))
                        {
                            return;
                        }
                        HandleMessage(_handlers, stream, token);
                    }
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
            var connectionStatus = token.ToObject<JsonConnectionStatus>();

            // ReSharper disable once ConstantConditionalAccessQualifier
            switch (connectionStatus?.Status)
            {
                case ConnectionStatus.Connected:
                    SendAsJsonString(new JsonAuthRequest
                    {
                        Action = JsonAction.PolygonAuthenticate,
                        Params = Configuration.KeyId
                    });
                    break;

                case ConnectionStatus.AuthenticationSuccess:
                    OnConnected(AuthStatus.Authorized);
                    break;

                case ConnectionStatus.AuthenticationFailed:
                case ConnectionStatus.AuthenticationRequired:
                    HandleError(new InvalidOperationException(connectionStatus.Message));
                    break;

                case ConnectionStatus.Failed:
                case ConnectionStatus.Success:
                    break;

                default:
                    HandleError(new InvalidOperationException("Unknown connection status"));
                    break;
            }
        }

        private static String getParams(
            IEnumerable<String> streams) =>
            String.Join(",", streams);
    }
}
