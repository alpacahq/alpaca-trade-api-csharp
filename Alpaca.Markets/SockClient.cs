using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca streaming API.
    /// </summary>
    [SuppressMessage(
        "Globalization","CA1303:Do not pass literals as localized parameters",
        Justification = "We do not plan to support localized exception messages in this SDK.")]
    // ReSharper disable once PartialTypeWithSinglePart
    public sealed partial class SockClient : SockClientBase
    {
        // Available Alpaca message types

        // ReSharper disable InconsistentNaming

        private const String TradeUpdates = "trade_updates";

        private const String AccountUpdates = "account_updates";

        private const String Authorization = "authorization";

        private const String Listening = "listening";

        // ReSharper restore InconsistentNaming

        private readonly IDictionary<String, Action<JToken>> _handlers;

        private readonly SockClientConfiguration _configuration;

        /// <summary>
        /// Creates new instance of <see cref="SockClient"/> object.
        /// </summary>
        /// <param name="keyId">Application key identifier.</param>
        /// <param name="secretKey">Application secret key.</param>
        /// <param name="alpacaRestApi">Alpaca REST API endpoint URL.</param>
        /// <param name="webSocketFactory">Factory class for web socket wrapper creation.</param>
        [Obsolete("This constructor is deprecated and will be removed in upcoming releases.", false)]
        public SockClient(
            String keyId,
            String secretKey,
            String alpacaRestApi = "https://api.alpaca.markets",
            IWebSocketFactory? webSocketFactory = null)
            : this(new SockClientConfiguration(keyId, secretKey, alpacaRestApi, webSocketFactory))
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="SockClient"/> object.
        /// </summary>
        /// <param name="keyId">Application key identifier.</param>
        /// <param name="secretKey">Application secret key.</param>
        /// <param name="alpacaRestApi">Alpaca REST API endpoint URL.</param>
        /// <param name="webSocketFactory">Factory class for web socket wrapper creation.</param>
        [Obsolete("This constructor is deprecated and will be removed in upcoming releases.", false)]
        public SockClient(
            String keyId,
            String secretKey,
            Uri alpacaRestApi,
            IWebSocketFactory? webSocketFactory)
            : this(new SockClientConfiguration(keyId, secretKey, alpacaRestApi, webSocketFactory))
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="SockClient"/> object.
        /// </summary>
        /// <param name="configuration"></param>
        public SockClient(
            SockClientConfiguration configuration)
            : base(
                getUriBuilder(ensureNotNull(configuration).AlpacaRestApi),
                configuration.WebSocketFactory)
        {
            configuration.EnsureIsValid();

            _configuration = configuration;

            _handlers = new Dictionary<String, Action<JToken>>(StringComparer.Ordinal)
            {
                { Listening, _ => { } },
                { Authorization, handleAuthorization },
                { AccountUpdates, handleAccountUpdate },
                { TradeUpdates, handleTradeUpdate }
            };
        }

        /// <summary>
        /// Occured when new account update received from stream.
        /// </summary>
        public event Action<IAccountUpdate>? OnAccountUpdate;

        /// <summary>
        /// Occured when new trade update received from stream.
        /// </summary>
        public event Action<ITradeUpdate>? OnTradeUpdate;

        /// <inheritdoc/>
        protected override void OnOpened()
        {
            SendAsJsonString(new JsonAuthRequest
            {
                Action = JsonAction.Authenticate,
                Data = new JsonAuthRequest.JsonData()
                {
                    KeyId = _configuration.KeyId,
                    SecretKey = _configuration.SecretKey
                }
            });

            base.OnOpened();
        }

        /// <inheritdoc/>
        [SuppressMessage(
            "Design", "CA1031:Do not catch general exception types",
            Justification = "Expected behavior - we report exceptions via OnError event.")]
        protected override void OnDataReceived(
            Byte[] binaryData)
        {
            try
            {
                var token = JObject.Parse(Encoding.UTF8.GetString(binaryData));

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

        private static UriBuilder getUriBuilder(
            Uri alpacaRestApi)
        {
            alpacaRestApi ??= new Uri("https://api.alpaca.markets");

            var uriBuilder = new UriBuilder(alpacaRestApi)
            {
                Scheme = alpacaRestApi.Scheme == "http" ? "ws" : "wss"
            };

            if (!uriBuilder.Path.EndsWith("/", StringComparison.Ordinal))
            {
                uriBuilder.Path += "/";
            }

            uriBuilder.Path += "stream";
            return uriBuilder;
        }

        private void handleAuthorization(
            JToken token)
        {
            var response = token.ToObject<JsonAuthResponse>();
            if (response is null)
            {
                HandleError(new InvalidOperationException("Invalid authentication response."));
                return;
            }

            OnConnected(response.Status);

            if (response.Status == AuthStatus.Authorized)
            {
                var listenRequest = new JsonListenRequest
                {
                    Action = JsonAction.Listen,
                    Data = new JsonListenRequest.JsonData()
                    {
                        Streams = new List<String>
                        {
                            TradeUpdates,
                            AccountUpdates
                        }
                    }
                };

                SendAsJsonString(listenRequest);
            }
        }

        private void handleTradeUpdate(
            JToken token) =>
            OnTradeUpdate.DeserializeAndInvoke<ITradeUpdate, JsonTradeUpdate>(token);

        private void handleAccountUpdate(
            JToken token) =>
            OnAccountUpdate.DeserializeAndInvoke<IAccountUpdate, JsonAccountUpdate>(token);

        private static SockClientConfiguration ensureNotNull(SockClientConfiguration configuration) => 
            configuration ?? throw new ArgumentNullException(nameof(configuration));
    }
}
