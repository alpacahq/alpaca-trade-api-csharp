using System.Buffers;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Channels;

namespace Alpaca.Markets.Tests;

internal sealed class MockWsClient<TConfiguration, TClient> : IDisposable
    where TConfiguration : StreamingClientConfiguration
    where TClient : class, IDisposable
{
    private readonly ConcurrentQueue<Guid> _requests = new();

    private readonly Mock<IWebSocket> _webSocket = new();

    private readonly Channel<String> _responses =
        Channel.CreateUnbounded<String>();

    public MockWsClient(
        TConfiguration configuration,
        Func<TConfiguration, TClient> factory)
    {
        configuration.WebSocketFactory = () => _webSocket.Object;
        Client = factory(configuration);

        // ReSharper disable once InvertIf
        if (String.Equals(configuration.ApiEndpoint.Scheme, Uri.UriSchemeWss, StringComparison.Ordinal))
        {
            _webSocket
                .Setup(socket => socket.ConnectAsync(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _webSocket
                .Setup(socket => socket.ReceiveAsync(It.IsAny<Memory<Byte>>()))
                .Returns<Memory<Byte>>(readResponseAsync);
        }
    }

    public TClient Client { get; }

    public void AddResponse(
        JToken response)
    {
        var request = Guid.NewGuid();
        _requests.Enqueue(request);

        _webSocket
            .When(() => _requests.TryPeek(out var item) && item.Equals(request) && _requests.TryDequeue(out _))
            .Setup(socket => socket.SendAsync(It.IsAny<ReadOnlySequence<Byte>>()))
            .Returns(() => AddMessageAsync(response));
    }

    public ValueTask AddMessageAsync(
        JToken message) =>
        AddMessageAsync(message.ToString());

    public ValueTask AddMessageAsync(
        String message) =>
        _responses.Writer.WriteAsync(message);

    public async ValueTask AddAuthenticationAsync(
        String? message = "authenticated")
    {
        if (message is null)
        {
            await AddMessageAsync(packResponse("success", new JObject()));
            AddResponse(new JObject());
        }
        else
        {
            await AddMessageAsync(getConnectionMessage("connected"));
            AddResponse(getConnectionMessage(message));
        }
    }

    public ValueTask AddErrorMessageAsync(
        HttpStatusCode errorCode) =>
        AddMessageAsync(packResponse("error", new JObject(
            new JProperty("code", (Int32)errorCode), new JProperty("msg", errorCode.ToString()))));

    public void AddSubscription(
        String channel,
        JToken symbols) =>
        AddResponse(getSubscriptionMessage(channel, symbols));

    public void AddException(
        Expression<Func<IWebSocket, Task>> expression,
        Exception exception) =>
        _webSocket.Setup(expression).ThrowsAsync(exception);

    public void Dispose()
    {
        _responses.Writer.Complete();
        _webSocket.VerifyAll();
        Client.Dispose();
    }

    private async ValueTask<ReceiveResult> readResponseAsync(
        Memory<Byte> memory)
    {
        var response = await _responses.Reader.ReadAsync();
        var encoded = Encoding.UTF8.GetBytes(response);
        encoded.CopyTo(memory);

        return new ReceiveResult(WebSocketMessageType.Binary, true, encoded.Length);
    }

    private static JArray getConnectionMessage(
        String message) =>
        packResponse("success", message);

    private static JArray getSubscriptionMessage(
        String channel,
        JToken symbols) =>
        packResponse("subscription", new JObject(
            new JProperty(channel, symbols)));

    private static JArray packResponse(
        String messageType,
        JToken message) =>
        packResponse(messageType, new JObject(
            new JProperty("msg", message)));

    private static JArray packResponse(
        String messageType,
        JObject message)
    {
        message.Add(MessageDataHelpers.StreamingMessageTypeTag, messageType);
        return new JArray(message);
    }
}
