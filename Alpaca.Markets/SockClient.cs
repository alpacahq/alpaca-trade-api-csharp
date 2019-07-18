﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocket4Net;
using System.Security.Authentication;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca streaming API.
    /// </summary>
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
        public SockClient(
            String keyId,
            String secretKey,
            String alpacaRestApi = null)
            : this(
                keyId,
                secretKey,
                new Uri(alpacaRestApi ?? "https://api.alpaca.markets"))
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="SockClient"/> object.
        /// </summary>
        /// <param name="keyId">Application key identifier.</param>
        /// <param name="secretKey">Application secret key.</param>
        /// <param name="alpacaRestApi">Alpaca REST API endpoint URL.</param>
        public SockClient(
            String keyId,
            String secretKey,
            Uri alpacaRestApi)
        {
            _keyId = keyId ?? throw new ArgumentException(nameof(keyId));
            _secretKey = secretKey ?? throw new ArgumentException(nameof(secretKey));

            alpacaRestApi = alpacaRestApi ?? new Uri("https://api.alpaca.markets");

            var uriBuilder = new UriBuilder(alpacaRestApi)
            {
                Scheme = alpacaRestApi.Scheme == "http" ? "ws" : "wss"
            };
            if (uriBuilder.Path.Substring(uriBuilder.Path.Length - 1, 1) != "/")
            {
                uriBuilder.Path += "/";
            }
            uriBuilder.Path += "stream";

            setupWebSocket(uriBuilder.Uri.ToString());
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
        /// Occured when any error happened in stream.
        /// </summary>
        public event Action<Exception> OnError;

        /// <summary>
        /// Occured when stream successfully connected.
        /// </summary>
        public event Action<AuthStatus> Connected;

        override private protected JsonAuthRequest getAuthRequest()
        {
            return new JsonAuthRequest
            {
                Action = JsonAction.Authenticate,
                Data = new JsonAuthRequest.JsonData()
                {
                    KeyId = _keyId,
                    SecretKey = _secretKey
                }
            };
        }

        private void handleDataReceived(
            Object sender,
            DataReceivedEventArgs e)
        {
            try
            {
                var message = Encoding.UTF8.GetString(e.Data);
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
                        OnError?.Invoke(new InvalidOperationException(
                            $"Unexpected message type '{stream}' received."));
                        break;
                }
            }
            catch (Exception exception)
            {
                OnError?.Invoke(exception);
            }
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

                sendAsJsonString(listenRequest);
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
