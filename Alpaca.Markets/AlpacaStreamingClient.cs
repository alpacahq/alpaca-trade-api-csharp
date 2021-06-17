using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca streaming API.
    /// </summary>
    internal sealed class AlpacaStreamingClient :
        StreamingClientBase<AlpacaStreamingClientConfiguration>,
        IAlpacaStreamingClient
    {
        // Available Alpaca message types

        private const String TradeUpdates = "trade_updates";

        private const String Authorization = "authorization";

        private const String Listening = "listening";

        private readonly IDictionary<String, Action<JToken>> _handlers;

        /// <summary>
        /// Creates new instance of <see cref="AlpacaStreamingClient"/> object.
        /// </summary>
        /// <param name="configuration">Configuration parameters object.</param>
        public AlpacaStreamingClient(
            AlpacaStreamingClientConfiguration configuration)
            : base(configuration.EnsureNotNull(nameof(configuration)))
        {
            _handlers = new Dictionary<String, Action<JToken>>(StringComparer.Ordinal)
            {
                { Listening, _ => { } },
                { Authorization, handleAuthorization },
                { TradeUpdates, handleTradeUpdate }
            };
        }

        /// <inheritdoc />
        public event Action<ITradeUpdate>? OnTradeUpdate;

        /// <inheritdoc/>
        protected override async void OnOpened()
        {
            await SendAsJsonStringAsync(new JsonAuthRequest
            {
                Action = JsonAction.Authenticate,
                Data = Configuration.SecurityId
                    .GetAuthenticationData()
            }).ConfigureAwait(false);

            base.OnOpened();
        }

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
                var messageType = token["stream"];

                if (payload is null ||
                    messageType is null)
                {
                    HandleError(new InvalidOperationException());
                }
                else
                {
                    HandleMessage(_handlers, messageType.ToString(), payload);
                }
            }
            catch (Exception exception)
            {
                HandleError(exception);
            }
        }

        private async void handleAuthorization(
            JToken token)
        {
            var response = token.ToObject<JsonAuthResponse>();
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (response is null)
            {
                HandleError(new InvalidOperationException("Invalid authentication response."));
                return;
            }

            OnConnected(response.Status);

            if (response.Status != AuthStatus.Authorized)
            {
                return;
            }

            var listenRequest = new JsonListenRequest
            {
                Action = JsonAction.Listen,
                Data = new JsonListenRequest.JsonData
                {
                    Streams = new List<String> { TradeUpdates }
                }
            };

            await SendAsJsonStringAsync(listenRequest).ConfigureAwait(false);
        }

        private void handleTradeUpdate(
            JToken token) =>
            OnTradeUpdate.DeserializeAndInvoke<ITradeUpdate, JsonTradeUpdate>(token);
    }
}
