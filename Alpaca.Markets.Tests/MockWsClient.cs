using System.Buffers;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Channels;

namespace Alpaca.Markets.Tests;

public sealed class MockWsClient<TConfiguration, TClient> : IDisposable
    where TConfiguration : StreamingClientConfiguration
    where TClient : class, IDisposable
{
    private readonly ConcurrentQueue<Guid> _requests = new ();

    private readonly Mock<IWebSocket> _webSocket = new ();

    private readonly Channel<String> _responses =
        Channel.CreateUnbounded<String>();

    public MockWsClient(
        TConfiguration configuration,
        Func<TConfiguration, TClient> factory)
    {
        configuration.WebSocketFactory = () => _webSocket.Object;
        Client = factory(configuration);

        _webSocket
            .Setup(_ => _.ConnectAsync(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _webSocket
            .Setup(_ => _.ReceiveAsync(It.IsAny<Memory<Byte>>()))
            .Returns<Memory<Byte>>(readResponseAsync);
    }

    public TClient Client { get; }

    public void AddResponse(
        JObject response)
    {
        var request = Guid.NewGuid();
        _requests.Enqueue(request);

        _webSocket
            .When(() => _requests.TryPeek(out var item) && item.Equals(request) && _requests.TryDequeue(out _))
            .Setup(_ => _.SendAsync(It.IsAny<ReadOnlySequence<Byte>>()))
            .Returns(() => AddMessageAsync(response));
    }

    public ValueTask AddMessageAsync(
        JObject message) =>
        _responses.Writer.WriteAsync(message.ToString());

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
}
