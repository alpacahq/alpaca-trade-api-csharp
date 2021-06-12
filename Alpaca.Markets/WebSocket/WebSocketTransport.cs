using System;
using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO.Pipelines;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    internal sealed class WebSocketsTransport : IDisposable
    {
        private sealed class DuplexPipe : IDuplexPipe
        {
            private DuplexPipe(PipeReader reader, PipeWriter writer)
            {
                Input = reader;
                Output = writer;
            }

            public PipeReader Input { get; }

            public PipeWriter Output { get; }

            public static DuplexPipePair CreateConnectionPair(PipeOptions inputOptions, PipeOptions outputOptions)
            {
                var input = new Pipe(inputOptions);
                var output = new Pipe(outputOptions);

                var transportToApplication = new DuplexPipe(output.Reader, input.Writer);
                var applicationToTransport = new DuplexPipe(input.Reader, output.Writer);

                return new DuplexPipePair(applicationToTransport, transportToApplication);
            }

            // This class exists to work around issues with value tuple on .NET Framework
            public readonly struct DuplexPipePair
            {
                public IDuplexPipe Transport { get; }
                public IDuplexPipe Application { get; }

                public DuplexPipePair(IDuplexPipe transport, IDuplexPipe application)
                {
                    Transport = transport;
                    Application = application;
                }
            }
        }
        
        private readonly ClientWebSocket _webSocket;

        private Task _running = Task.CompletedTask;

        private readonly Uri _resolvedUrl;
        
        private volatile bool _aborted;

        private readonly IDuplexPipe _application;

        private readonly IDuplexPipe _transport;

        public WebSocketsTransport(
            Uri url)
        {
#pragma warning disable PC001 // API not supported on all platforms
            _webSocket = new ClientWebSocket();
#pragma warning restore PC001 // API not supported on all platforms

            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            _resolvedUrl = url;

            var options = new PipeOptions(
                writerScheduler: PipeScheduler.ThreadPool,
                readerScheduler: PipeScheduler.ThreadPool,
                useSynchronizationContext: false);
            var pair = DuplexPipe.CreateConnectionPair(options, options);

            _transport = pair.Transport;
            _application = pair.Application;
        }
        
        public event Action? Opened;

        public event Action? Closed;

        public event Action<String>? MessageReceived;

        public event Action<Byte[]>? DataReceived;

        public event Action<Exception>? Error;

        public async Task StartAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                await _webSocket.ConnectAsync(_resolvedUrl, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch
            {
                _webSocket.Dispose();
                throw;
            }

            Opened?.Invoke();
            _running = processSocketAsync(_webSocket);
        }

        [SuppressMessage(
            "Design", "CA1031:Do not catch general exception types",
            Justification = "Expected behavior - we report exceptions via OnError event.")]
        public async Task StopAsync(
            CancellationToken cancellationToken = default)
        {
            await _transport!.Output.CompleteAsync().ConfigureAwait(false);
            await _transport!.Input.CompleteAsync().ConfigureAwait(false);

            // Cancel any pending reads from the application, this should start the entire shutdown process
            _application.Input.CancelPendingRead();

            try
            {
                var taskCompletionSource = new TaskCompletionSource<Boolean>();
                cancellationToken.Register(() => taskCompletionSource.TrySetCanceled());
                await Task.WhenAny(_running, taskCompletionSource.Task)
                    .ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                // exceptions have been handled in the Running task continuation by closing the channel with the exception
                Trace.TraceInformation(exception.Message);
                return;
            }
            finally
            {
                _webSocket.Dispose();
            }

            Closed?.Invoke();
        }

        public async ValueTask SendAsync(
            String message,
            CancellationToken cancellationToken) =>
            await _transport.Output
                .WriteAsync(Encoding.UTF8.GetBytes(message), cancellationToken)
                .ConfigureAwait(false);

        public void Dispose() => _webSocket.Dispose();

        private async Task processSocketAsync(
            WebSocket socket)
        {
            using (socket)
            {
                // Begin sending and receiving.
                var receiving = startReceiving(socket);
                var sending = startSending(socket);

                // Wait for send or receive to complete
                var trigger = await Task.WhenAny(receiving, sending)
                    .ConfigureAwait(false);

                if (trigger == receiving)
                {
                    // We're waiting for the application to finish and there are 2 things it could be doing
                    // 1. Waiting for application data
                    // 2. Waiting for a websocket send to complete

                    // Cancel the application so that ReadAsync yields
                    _application.Input.CancelPendingRead();

                    using var delayCts = new CancellationTokenSource();

                    var resultTask = await Task.WhenAny(sending, 
                            Task.Delay(Timeout.InfiniteTimeSpan, delayCts.Token))
                        .ConfigureAwait(false);

                    if (resultTask != sending)
                    {
                        _aborted = true;

                        // Abort the websocket if we're stuck in a pending send to the client
                        socket.Abort();
                    }
                    else
                    {
                        // Cancel the timeout
                        delayCts.Cancel();
                    }
                }
                else
                {
                    // We're waiting on the websocket to close and there are 2 things it could be doing
                    // 1. Waiting for websocket data
                    // 2. Waiting on a flush to complete (back pressure being applied)
                    _aborted = true;

                    // Abort the websocket if we're stuck in a pending receive from the client
                    socket.Abort();

                    // Cancel any pending flush so that we can quit
                    _application.Output.CancelPendingFlush();
                }
            }
        }

        [SuppressMessage("Design",
            "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        private async Task startReceiving(WebSocket socket)
        {
            try
            {
                while (true)
                {
#if NETSTANDARD2_1 || NETCOREAPP
                    // Do a 0 byte read so that idle connections don't allocate a buffer when waiting for a read
                    var result = await socket.ReceiveAsync(Memory<byte>.Empty, CancellationToken.None)
                        .ConfigureAwait(false);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        if (_webSocket.CloseStatus != WebSocketCloseStatus.NormalClosure)
                        {
                            throw new InvalidOperationException($"Websocket closed with error: {_webSocket.CloseStatus}.");
                        }

                        return;
                    }
#endif
                    var memory = _application.Output.GetMemory();
#if NETSTANDARD2_1 || NETCOREAPP
                    // Because we checked the CloseStatus from the 0 byte read above, we don't need to check again after reading
                    var receiveResult = await socket.ReceiveAsync(memory, CancellationToken.None)
                        .ConfigureAwait(false);
#elif NETSTANDARD2_0 || NET461
                    var _ = System.Runtime.InteropServices.MemoryMarshal.TryGetArray<byte>(memory, out var arraySegment);

                    // Exceptions are handled above where the send and receive tasks are being run.
                    var receiveResult = await socket.ReceiveAsync(arraySegment, CancellationToken.None).ConfigureAwait(false);
#else
#error TFMs need to be updated
#endif
                    // Need to check again for netstandard2.1 because a close can happen between a 0-byte read and the actual read
                    if (receiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        if (_webSocket.CloseStatus != WebSocketCloseStatus.NormalClosure)
                        {
                            throw new InvalidOperationException($"Websocket closed with error: {_webSocket.CloseStatus}.");
                        }

                        return;
                    }

                    _application.Output.Advance(receiveResult.Count);

                    var flushResult = await _application.Output.FlushAsync().ConfigureAwait(false);

                    // We canceled in the middle of applying back pressure
                    // or if the consumer is done
                    if (flushResult.IsCanceled || flushResult.IsCompleted)
                    {
                        break;
                    }

                    if (!receiveResult.EndOfMessage)
                    {
                        continue;
                    }

                    var readResult = await _transport.Input.ReadAsync().ConfigureAwait(false);

                    // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
                    switch (receiveResult.MessageType) //-V3002
                    {
                        case WebSocketMessageType.Binary:
                            DataReceived?.Invoke(readResult.Buffer.ToArray());
                            break;

                        case WebSocketMessageType.Text:
                            MessageReceived?.Invoke(Encoding.UTF8.GetString(readResult.Buffer.ToArray()));
                            break;
                    }

                    _transport.Input.AdvanceTo(readResult.Buffer.End);
                }
            }
            catch (OperationCanceledException exception)
            {
                Trace.TraceInformation(exception.Message);
            }
            catch (Exception exception)
            {
                if (!_aborted)
                {
                    await _application.Output.CompleteAsync(exception).ConfigureAwait(false);
                    Error?.Invoke(exception);
                }
            }
            finally
            {
                // We're done writing
                await _application.Output.CompleteAsync().ConfigureAwait(false);
            }
        }

        [SuppressMessage("Design",
            "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        private async Task startSending(WebSocket socket)
        {
            Exception? error = null;

            try
            {
                while (true)
                {
                    var result = await _application.Input.ReadAsync()
                        .ConfigureAwait(false);
                    var buffer = result.Buffer;

                    // Get a frame from the application
                    try
                    {
                        if (result.IsCanceled)
                        {
                            break;
                        }

                        if (!buffer.IsEmpty)
                        {
                            try
                            {
                                if (webSocketCanSend(socket))
                                {
                                    await socket.SendAsync(buffer, WebSocketMessageType.Binary)
                                        .ConfigureAwait(false);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            catch (Exception exception)
                            {
                                if (!_aborted)
                                {
                                    Trace.TraceInformation(exception.Message);
                                }
                                break;
                            }
                        }
                        else if (result.IsCompleted)
                        {
                            break;
                        }
                    }
                    finally
                    {
                        _application.Input.AdvanceTo(buffer.End);
                    }
                }
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex);
                error = ex;
            }
            finally
            {
                if (webSocketCanSend(socket))
                {
                    try
                    {
                        // We're done sending, send the close frame to the client if the websocket is still open
                        await socket.CloseOutputAsync(error != null
                            ? WebSocketCloseStatus.InternalServerError
                            : WebSocketCloseStatus.NormalClosure,
                            "", CancellationToken.None)
                            .ConfigureAwait(false);
                    }
                    catch (Exception exception)
                    {
                        Trace.TraceInformation(exception.Message);
                    }
                }

                await _application.Input.CompleteAsync().ConfigureAwait(false);
            }
        }

        private static bool webSocketCanSend(WebSocket ws) =>
            ws.State is not (
                WebSocketState.Aborted or
                WebSocketState.Closed or
                WebSocketState.CloseSent);
    }
}
