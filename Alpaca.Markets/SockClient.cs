using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca streaming API.
    /// </summary>
    // ReSharper disable once PartialTypeWithSinglePart
    public sealed partial class SockClient : SockClientBase
    {
        private readonly String _keyId;

        private readonly String _secretKey;

        /// <summary>
        /// Creates new instance of <see cref="SockClient"/> object.
        /// </summary>
        /// <param name="keyId">Application key identifier.</param>
        /// <param name="secretKey">Application secret key.</param>
        /// <param name="alpacaRestApi">Alpaca REST API endpoint URL.</param>
        /// <param name="webSocketFactory">Factory class for web socket wrapper creation.</param>
        public SockClient(
            String keyId,
            String secretKey,
            String alpacaRestApi = null,
            IWebSocketFactory webSocketFactory = null)
            : this(
                keyId,
                secretKey,
                new Uri(alpacaRestApi ?? "https://api.alpaca.markets"),
                webSocketFactory ?? new WebSocket4NetFactory())
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="SockClient"/> object.
        /// </summary>
        /// <param name="keyId">Application key identifier.</param>
        /// <param name="secretKey">Application secret key.</param>
        /// <param name="alpacaRestApi">Alpaca REST API endpoint URL.</param>
        /// <param name="webSocketFactory">Factory class for web socket wrapper creation.</param>
        public SockClient(
            String keyId,
            String secretKey,
            Uri alpacaRestApi,
            IWebSocketFactory webSocketFactory)
            : base(getUriBuilder(alpacaRestApi), webSocketFactory)
        {
            _keyId = keyId ?? throw new ArgumentException(nameof(keyId));
            _secretKey = secretKey ?? throw new ArgumentException(nameof(secretKey));

            alpacaRestApi = alpacaRestApi ?? new Uri("https://api.alpaca.markets");

            var uriBuilder = new UriBuilder(alpacaRestApi)
            {
                Scheme = alpacaRestApi.Scheme == "http" ? "ws" : "wss"
            };
            uriBuilder.Path += "/stream";

            _webSocket = new WebSocket(uriBuilder.Uri.ToString(),
                sslProtocols: SslProtocols.Tls11 | SslProtocols.Tls12);

            _webSocket.Opened += handleOpened;
            _webSocket.Closed += handleClosed;

            _webSocket.DataReceived += handleDataReceived;
            _webSocket.Error += (sender, args) => OnError?.Invoke(args.Exception);
        }

        /// <summary>
        /// Occured when new account update received from stream.
        /// </summary>
        public event Action<IAccountUpdate> OnAccountUpdate;

        /// <summary>
        /// Occured when new trade update received from stream.
        /// </summary>
        public event Action<ITradeUpdate> OnTradeUpdate;

        /// <summary>
        /// Occured when stream successfully connected.
        /// </summary>
        public event Action<AuthStatus> Connected;

        /// <summary>
        /// Occured when any error happened in stream.
        /// </summary>
        public event Action<Exception> OnError;

        /// <summary>
        /// Opens connection to Alpaca streaming API.
        /// </summary>
        /// <returns>Waitable task object for handling action completion in asyncronious mode.</returns>
        public Task ConnectAsync()
        {
#if NET45
            return Task.Run(() => _webSocket.Open());
#else
            return _webSocket.OpenAsync();
#endif
        }

        /// <summary>
        /// Closes connection to Alpaca streaming API.
        /// </summary>
        /// <returns>Waitable task object for handling action completion in asyncronious mode.</returns>
        public Task DisconnectAsync()
        {
#if NET45
            return Task.Run(() => _webSocket.Close());
#else
            return _webSocket.CloseAsync();
#endif
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _webSocket?.Dispose();
        }

        private void handleOpened(
            Object sender,
            EventArgs e)
        {
            var authenticateRequest = new JsonAuthRequest
            {
                Action = JsonAction.Authenticate,
                Data = new JsonAuthRequest.JsonData()
                {
                    KeyId = _keyId,
                    SecretKey = _secretKey
                }
            };

        /// <inheritdoc/>
        protected override void OnDataReceived(
            Byte[] binaryData)
        {
            try
            {
                var message = Encoding.UTF8.GetString(binaryData);
                var root = JObject.Parse(message);

                var data = root["data"];
                var stream = root["stream"].ToString();

                switch (stream)
                {
                    case "authorization":
                        handleAuthorization(
                            data.ToObject<JsonAuthResponse>());
                        break;

                    case "listening":
                        Connected?.Invoke(AuthStatus.Authorized);
                        break;

                    case "trade_updates":
                        handleTradeUpdates(
                            data.ToObject<JsonTradeUpdate>());
                        break;

                    case "account_updates":
                        handleAccountUpdates(
                            data.ToObject<JsonAccountUpdate>());
                        break;

                    default:
                        HandleError(new InvalidOperationException(
                            $"Unexpected message type '{stream}' received."));
                        break;
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
            alpacaRestApi = alpacaRestApi ?? new Uri("https://api.alpaca.markets");

            var uriBuilder = new UriBuilder(alpacaRestApi)
            {
                Scheme = alpacaRestApi.Scheme == "http" ? "ws" : "wss"
            };

            if (!uriBuilder.Path.EndsWith("/"))
            {
                uriBuilder.Path += "/";
            }

            uriBuilder.Path += "stream";
            return uriBuilder;
        }

        private void handleAuthorization(
            JsonAuthResponse response)
        {
            if (response.Status == AuthStatus.Authorized)
            {
                var listenRequest = new JsonListenRequest
                {
                    Action = JsonAction.Listen,
                    Data = new JsonListenRequest.JsonData()
                    {
                        Streams = new List<String>
                        {
                            "trade_updates",
                            "account_updates"
                        }
                    }
                };

                SendAsJsonString(listenRequest);
            }
            else
            {
                Connected?.Invoke(response.Status);
            }
        }

        private void handleTradeUpdates(
            ITradeUpdate update)
        {
            OnTradeUpdate?.Invoke(update);
        }

        private void handleAccountUpdates(
            IAccountUpdate update)
        {
            OnAccountUpdate?.Invoke(update);
        }
    }
}
