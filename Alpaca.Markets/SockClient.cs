using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocket4Net;

namespace Alpaca.Markets
{
    public sealed class SockClient : IDisposable
    {
        private readonly WebSocket _webSocket;

        private readonly String _keyId;

        private readonly String _secretKey;

        public SockClient(
            String keyId,
            String secretKey,
            Uri restApi)
        {
            _keyId = keyId;
            _secretKey = secretKey;

            var uriBuilder = new UriBuilder(restApi)
            {
                Scheme = restApi.Scheme == "http" ? "ws" : "wss"
            };
            uriBuilder.Path += "/stream";

            _webSocket = new WebSocket(uriBuilder.Uri.ToString());

            _webSocket.Opened += handleOpened;
            _webSocket.Closed += handleClosed;

            _webSocket.DataReceived += handleDataReceived;
            _webSocket.Error += (sender, args) => OnError?.Invoke(args.Exception);
        }

        public event Action<IAccountUpdate> OnAccountUpdate;

        public event Action<ITradeUpdate> OnTradeUpdate;

        public event Action<AuthStatus> Connected;

        public event Action<Exception> OnError;

        public Task ConnectAsync()
        {
            return _webSocket.OpenAsync();
        }

        public Task DisconnectAsync()
        {
            return _webSocket.CloseAsync();
        }

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

            sendAsJsonString(authenticateRequest);
        }

        private void handleClosed(
            Object sender,
            EventArgs e)
        {
        }

        private void handleDataReceived(
            Object sender,
            DataReceivedEventArgs e)
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
            JsonTradeUpdate update)
        {
            OnTradeUpdate?.Invoke(update);
        }

        private void handleAccountUpdates(
            JsonAccountUpdate update)
        {
            OnAccountUpdate?.Invoke(update);
        }

        private void sendAsJsonString(
            Object value)
        {
            using (var textWriter = new StringWriter())
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(textWriter, value);
                _webSocket.Send(textWriter.ToString());
            }
        }
    }
}
