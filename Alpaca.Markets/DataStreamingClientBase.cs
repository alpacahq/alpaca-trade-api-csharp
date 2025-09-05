using System.Collections.Concurrent;
using Newtonsoft.Json.Linq;
using MessagePack;
using System.Buffers;

namespace Alpaca.Markets;

internal abstract class DataStreamingClientBase<TConfiguration> :
    StreamingClientBase<TConfiguration>
    where TConfiguration : StreamingClientConfiguration
{
    private interface ISubscription
    {
        void OnUpdate(
            Boolean isSubscribed);

        void OnReceived(
            JToken token);
    }

    private sealed class AlpacaExplicitDataSubscription<TApi, TJson>
        : IAlpacaDataSubscription<TApi>, ISubscription
        where TJson : class, TApi
    {
        private readonly String _stream;

        private Boolean _subscribed;

        internal AlpacaExplicitDataSubscription(
            String stream) =>
            _stream = stream;

        public IEnumerable<String> Streams
        {
            get { yield return _stream; }
        }

        public Boolean Subscribed
        {
            get => _subscribed;
            private set
            {
                if (_subscribed == value)
                {
                    return;
                }

                _subscribed = value;
                OnSubscribedChanged?.Invoke();
            }
        }

        public event Action<TApi>? Received;

        public event Action? OnSubscribedChanged;

        public void OnReceived(
            JToken token) =>
            Received?.Invoke(token.ToObject<TJson>() ?? throw new RestClientErrorException());

        public void OnUpdate(
            Boolean isSubscribed) => Subscribed = isSubscribed;
    }

    private sealed class AlpacaImplicitDataSubscription<TApi, TJson>
        : IAlpacaDataSubscription<TApi>, ISubscription
        where TJson : class, TApi
    {
        private readonly IAlpacaDataSubscription _parent;

        private readonly String _stream;

        internal AlpacaImplicitDataSubscription(
            String stream,
            IAlpacaDataSubscription parent)
        {
            _stream = stream;
            _parent = parent;
        }

        public IEnumerable<String> Streams
        {
            get { yield return _stream; }
        }

        public Boolean Subscribed => _parent.Subscribed;

        public event Action<TApi>? Received;

        public event Action? OnSubscribedChanged
        {
            add => _parent.OnSubscribedChanged += value;
            remove => _parent.OnSubscribedChanged -= value;
        }

        public void OnReceived(
            JToken token) =>
            Received?.Invoke(token.ToObject<TJson>() ?? throw new RestClientErrorException());

        public void OnUpdate(
            Boolean isSubscribed) =>
            Trace.WriteLine($"Update received for the channel '{_stream}' - not expected.");
    }

    private sealed class SubscriptionsDictionary
    {
        private readonly ConcurrentDictionary<String, ISubscription> _subscriptions = new(StringComparer.Ordinal);

        public IAlpacaDataSubscription<TApi> GetOrAdd<TApi, TJson>(
            String stream)
            where TJson : class, TApi =>
            (IAlpacaDataSubscription<TApi>)_subscriptions.GetOrAdd(
                stream, key => new AlpacaExplicitDataSubscription<TApi, TJson>(key));

        public IAlpacaDataSubscription<TApi> GetOrAdd<TApi, TJson>(
            String stream,
            IAlpacaDataSubscription parent)
            where TJson : class, TApi
        {
            return (IAlpacaDataSubscription<TApi>)_subscriptions.GetOrAdd(
                stream, key => new AlpacaImplicitDataSubscription<TApi, TJson>(key, parent));
        }

        public void OnUpdate(
            HashSet<String> streams)
        {
            foreach (var (stream, subscription) in _subscriptions)
            {
                subscription.OnUpdate(streams.Contains(stream));
            }
        }

        public void OnReceived(
            String stream,
            JToken token)
        {
            if (_subscriptions.TryGetValue(stream, out var subscription))
            {
                subscription.OnReceived(token);
            }
        }
    }
    
    private static class MessagePackToJTokenDeserializer
    {
        public static JToken Deserialize(byte[] msgPackBytes, Action<string> onWarning)
        {
            var reader = new MessagePackReader(new ReadOnlySequence<byte>(msgPackBytes));
            return toJToken(ref reader, onWarning);
        }

        // Only the data types used by Alpaca real-time data are covered here
        private static JToken toJToken(ref MessagePackReader reader, Action<string> onWarning)
        {
            var type = reader.NextMessagePackType;

            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (type)
            {
                case MessagePackType.Map:
                    var size = reader.ReadMapHeader();
                    var obj = new JObject();
                    for (var i = 0; i < size; ++i)
                    {
                        var key = reader.ReadString();
                        if (key is not null)
                        {
                            obj[key] = toJToken(ref reader, onWarning);
                        }
                        else
                        {
                            reader.Skip();
                        }
                    }
                    return obj;

                case MessagePackType.Array:
                    var length = reader.ReadArrayHeader();
                    var arr = new JArray();
                    for (var i = 0; i < length; ++i)
                    {
                        arr.Add(toJToken(ref reader, onWarning));
                    }
                    return arr;

                case MessagePackType.String:
                    return new JValue(reader.ReadString());

                case MessagePackType.Integer:
                    return new JValue(reader.ReadInt64());

                case MessagePackType.Float:
                    return reader.NextCode == MessagePackCode.Float32
                        ? new JValue(reader.ReadSingle())
                        : new JValue(reader.ReadDouble());

                case MessagePackType.Boolean:
                    return new JValue(reader.ReadBoolean());

                case MessagePackType.Extension:
                    var extHeader = reader.ReadExtensionFormatHeader();
                    if (extHeader.TypeCode == ReservedMessagePackExtensionTypeCode.DateTime)
                    {
                        return new JValue(reader.ReadDateTime(extHeader));
                    }
                    onWarning.Invoke($"Ignored MessagePack data type Extension:{extHeader.TypeCode}");
                    break;

                default:
                    onWarning.Invoke($"Ignored MessagePack data type {Enum.GetName(typeof(MessagePackType), type)}");
                    break;
            }

            reader.Skip();
            return JValue.CreateNull();
        }
    }
    
    // Available Alpaca data streaming message types

    private const String ErrorInfo = "error";

    private const String Subscription = "subscription";

    private const String ConnectionSuccess = "success";

    protected const String NewsChannel = "n";

    private const String TradesChannel = "t";

    protected const String QuotesChannel = "q";

    private const String DailyBarsChannel = "d";

    protected const String StatusesChannel = "s";

    private const String MinuteBarsChannel = "b";

    private const String UpdatedBarsChannel = "u";

    protected const String OrderBooksChannel = "o";

    protected const String LimitUpDownChannel = "l";

    protected const String CorrectionsChannel = "c";

    protected const String CancellationsChannel = "x";

    protected const String WildcardSymbolString = "*";

    private const Int32 SubscriptionChunkSize = 100;

    // ReSharper disable once StaticMemberInGenericType
    private static readonly Char[] _channelSeparator = [ '.' ];

    // ReSharper disable once StaticMemberInGenericType
    private static readonly SortedSet<String> _implicitChannels = new(StringComparer.Ordinal)
    {
        CancellationsChannel,
        CorrectionsChannel
    };

    private readonly IDictionary<String, Action<JToken>> _handlers;

    private readonly SubscriptionsDictionary _subscriptions = new();

    protected DataStreamingClientBase(
        TConfiguration configuration)
        : base(configuration.EnsureNotNull()) =>
        _handlers = new Dictionary<String, Action<JToken>>(StringComparer.Ordinal)
        {
                { CancellationsChannel, handleRealtimeDataUpdate },
                { CorrectionsChannel, handleRealtimeDataUpdate },
                { LimitUpDownChannel, handleRealtimeDataUpdate },
                { UpdatedBarsChannel, handleRealtimeDataUpdate },
                { MinuteBarsChannel, handleRealtimeDataUpdate },
                { OrderBooksChannel, handleRealtimeDataUpdate },
                { DailyBarsChannel, handleRealtimeDataUpdate },
                { ConnectionSuccess, handleConnectionSuccess },
                { StatusesChannel, handleRealtimeDataUpdate },
                { Subscription, handleSubscriptionUpdates },
                { TradesChannel, handleRealtimeDataUpdate },
                { QuotesChannel, handleRealtimeDataUpdate },
                { NewsChannel, handleRealtimeNewsUpdate },
                { ErrorInfo, handleErrorMessages }
        };

    public IAlpacaDataSubscription<ITrade> GetTradeSubscription() =>
        GetSubscription<ITrade, JsonRealTimeTrade>(TradesChannel, WildcardSymbolString);

    public IAlpacaDataSubscription<ITrade> GetTradeSubscription(
        String symbol) =>
        GetSubscription<ITrade, JsonRealTimeTrade>(TradesChannel, symbol.EnsureNotNull());

    public IAlpacaDataSubscription<IBar> GetMinuteBarSubscription() =>
        GetSubscription<IBar, JsonRealTimeBar>(MinuteBarsChannel, WildcardSymbolString);

    public IAlpacaDataSubscription<IBar> GetMinuteBarSubscription(
        String symbol) =>
        GetSubscription<IBar, JsonRealTimeBar>(MinuteBarsChannel, symbol.EnsureNotNull());

    public IAlpacaDataSubscription<IBar> GetDailyBarSubscription(
        String symbol) =>
        GetSubscription<IBar, JsonRealTimeBar>(DailyBarsChannel, symbol.EnsureNotNull());

    public IAlpacaDataSubscription<IBar> GetUpdatedBarSubscription(
        String symbol) =>
        GetSubscription<IBar, JsonRealTimeBar>(UpdatedBarsChannel, symbol.EnsureNotNull());

    public ValueTask SubscribeAsync(
        IAlpacaDataSubscription subscription) =>
        SubscribeAsync(subscription, CancellationToken.None);

    public ValueTask SubscribeAsync(
        IAlpacaDataSubscription subscription,
        CancellationToken cancellationToken) =>
        subscribeAsync(subscription.EnsureNotNull().Streams, cancellationToken);

    public ValueTask SubscribeAsync(
        IEnumerable<IAlpacaDataSubscription> subscriptions) =>
        SubscribeAsync(subscriptions, CancellationToken.None);

    public ValueTask SubscribeAsync(
        IEnumerable<IAlpacaDataSubscription> subscriptions,
        CancellationToken cancellationToken) =>
        subscribeAsync(subscriptions.SelectMany(subscription => subscription.Streams), cancellationToken);

    public ValueTask UnsubscribeAsync(
        IAlpacaDataSubscription subscription) =>
        UnsubscribeAsync(subscription, CancellationToken.None);

    public ValueTask UnsubscribeAsync(
        IAlpacaDataSubscription subscription,
        CancellationToken cancellationToken) =>
        unsubscribeAsync(subscription.EnsureNotNull().Streams, cancellationToken);

    public ValueTask UnsubscribeAsync(
        IEnumerable<IAlpacaDataSubscription> subscriptions) =>
        UnsubscribeAsync(subscriptions, CancellationToken.None);

    public ValueTask UnsubscribeAsync(
        IEnumerable<IAlpacaDataSubscription> subscriptions,
        CancellationToken cancellationToken) =>
        unsubscribeAsync(subscriptions.SelectMany(subscription => subscription.Streams), cancellationToken);

    protected IAlpacaDataSubscription<TApi> GetSubscription<TApi, TJson>(
        String channelName,
        String symbol)
        where TJson : class, TApi =>
        _subscriptions.GetOrAdd<TApi, TJson>(getStreamName(channelName, symbol));

    protected IAlpacaDataSubscription<TApi> GetSubscription<TApi, TJson>(
        String channelName,
        String symbol,
        IAlpacaDataSubscription parent)
        where TJson : class, TApi =>
        _subscriptions.GetOrAdd<TApi, TJson>(getStreamName(channelName, symbol), parent);

#pragma warning disable IDE0079
    [SuppressMessage(
#pragma warning restore IDE0079
        "Design", "CA1031:Do not catch general exception types",
        Justification = "Expected behavior - we report exceptions via OnError event.")]
    protected sealed override void OnMessageReceived(
        String message)
    {
        try
        {
            foreach (var token in JArray.Parse(message))
            {
                var messageType = token["T"];
                if (messageType is null)
                {
                    HandleWarning("Incoming message missing message type.");
                }
                else
                {
                    HandleMessage(_handlers, messageType.ToString(), token);
                }
            }
        }
        catch (Exception exception)
        {
            HandleError(exception);
        }
    }

#pragma warning disable IDE0079
    [SuppressMessage(
#pragma warning restore IDE0079
        "Design", "CA1031:Do not catch general exception types",
        Justification = "Expected behavior - we report exceptions via OnError event.")]
    protected sealed override void OnMessageReceived(
        byte[] binaryData)
    {
        try
        {
            foreach (var token in MessagePackToJTokenDeserializer.Deserialize(binaryData, HandleWarning))
            {
                var messageType = token["T"];
                if (messageType is null)
                {
                    HandleWarning("Incoming message missing message type.");
                }
                else
                {
                    var conditions = token["c"];

                    if (conditions is not null)
                    {
                        if (JTokenType.String == conditions.Type)
                        {
                            var conditionsString = conditions.Value<string>();

                            if (conditionsString is not null)
                            {
                                token["c"] = new JArray(conditionsString
                                    .Select(c => c.ToString()).ToArray<Object>());
                            }
                        }
                    }

                    HandleMessage(_handlers, messageType.ToString(), token);
                }
            }
        }
        catch (Exception exception)
        {
            HandleError(exception);
        }
    }

#pragma warning disable IDE0079
    [SuppressMessage(
#pragma warning restore IDE0079
        "Design", "CA1031:Do not catch general exception types",
        Justification = "Expected behavior - we report exceptions via OnError event.")]
    private async void handleConnectionSuccess(
        JToken token)
    {
        try
        {
            var connectionSuccess = token.ToObject<JsonConnectionSuccess>() ?? new JsonConnectionSuccess();

            // ReSharper disable once ConstantConditionalAccessQualifier
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (connectionSuccess.Status)
            {
                case ConnectionStatus.Connected:
                    await SendAsync(Configuration.SecurityId.GetAuthentication())
                        .ConfigureAwait(false);
                    break;

                case ConnectionStatus.Authenticated:
                    OnConnected(AuthStatus.Authorized);
                    break;

                default:
                    HandleWarning($"Unknown connection status `{connectionSuccess.Status}` found.");
                    break;
            }
        }
        catch (Exception exception)
        {
            HandleError(exception);
        }
    }

#pragma warning disable IDE0079
    [SuppressMessage(
#pragma warning restore IDE0079
        "Design", "CA1031:Do not catch general exception types",
        Justification = "Expected behavior - we report exceptions via OnError event.")]
    private void handleSubscriptionUpdates(
        JToken token)
    {
        try
        {
            var subscriptionUpdate = token.ToObject<JsonSubscriptionUpdate>() ?? new JsonSubscriptionUpdate();

            var streams = new HashSet<String>(
                getStreams(subscriptionUpdate.Trades.EmptyIfNull(), TradesChannel)
                    .Concat(getStreams(subscriptionUpdate.News.EmptyIfNull(), NewsChannel))
                    .Concat(getStreams(subscriptionUpdate.Quotes.EmptyIfNull(), QuotesChannel))
                    .Concat(getStreams(subscriptionUpdate.Statuses.EmptyIfNull(), StatusesChannel))
                    .Concat(getStreams(subscriptionUpdate.Lulds.EmptyIfNull(), LimitUpDownChannel))
                    .Concat(getStreams(subscriptionUpdate.DailyBars.EmptyIfNull(), DailyBarsChannel))
                    .Concat(getStreams(subscriptionUpdate.OrderBooks.EmptyIfNull(), OrderBooksChannel))
                    .Concat(getStreams(subscriptionUpdate.MinuteBars.EmptyIfNull(), MinuteBarsChannel))
                    .Concat(getStreams(subscriptionUpdate.UpdatedBars.EmptyIfNull(), UpdatedBarsChannel)),
                StringComparer.Ordinal);

            _subscriptions.OnUpdate(streams);
        }
        catch (Exception exception)
        {
            HandleError(exception);
        }
    }

#pragma warning disable IDE0079
    [SuppressMessage(
#pragma warning restore IDE0079
        "Design", "CA1031:Do not catch general exception types",
        Justification = "Expected behavior - we report exceptions via OnError event.")]
    private void handleRealtimeDataUpdate(
        JToken token)
    {
        try
        {
            var channel = token["T"]?.ToString() ?? String.Empty;
            var symbol = token["S"]?.ToString() ?? String.Empty;

            _subscriptions.OnReceived(getStreamName(channel, symbol), token);
            _subscriptions.OnReceived(getStreamName(channel, WildcardSymbolString), token);
        }
        catch (Exception exception)
        {
            HandleError(exception);
        }
    }

#pragma warning disable IDE0079
    [SuppressMessage(
#pragma warning restore IDE0079
        "Design", "CA1031:Do not catch general exception types",
        Justification = "Expected behavior - we report exceptions via OnError event.")]
    private void handleRealtimeNewsUpdate(
        JToken token)
    {
        try
        {
            var channel = token["T"]?.ToString() ?? String.Empty;
            var symbols = token["symbols"]?.Values<String>() ?? [];

            foreach (var symbol in symbols.Where(value => !String.IsNullOrEmpty(value)))
            {
                _subscriptions.OnReceived(getStreamName(channel, symbol!), token);
            }

            _subscriptions.OnReceived(getStreamName(channel, WildcardSymbolString), token);
        }
        catch (Exception exception)
        {
            HandleError(exception);
        }
    }

#pragma warning disable IDE0079
    [SuppressMessage(
#pragma warning restore IDE0079
        "Design", "CA1031:Do not catch general exception types",
        Justification = "Expected behavior - we report exceptions via OnError event.")]
    private async void handleErrorMessages(
        JToken token)
    {
        const Int32 connectionLimitExceeded = 406;
        const Int32 authenticationTimeout = 404;
        const Int32 alreadyAuthenticated = 403;
        const Int32 authenticationFailed = 402;
        const Int32 notAuthenticated = 401;

        try
        {
            var error = token.ToObject<JsonStreamError>() ?? new JsonStreamError();
            switch (error.Code)
            {
                case notAuthenticated:
                    await SendAsync(Configuration.SecurityId.GetAuthentication())
                        .ConfigureAwait(false);
                    break;

                case connectionLimitExceeded:
                    OnConnected(AuthStatus.TooManyConnections);
                    break;

                case authenticationTimeout:
                case authenticationFailed:
                    OnConnected(AuthStatus.Unauthorized);
                    break;

                case alreadyAuthenticated:
                    OnConnected(AuthStatus.Authorized);
                    break;

                default:
                    HandleError(new RestClientErrorException(error));
                    break;
            }
        }
        catch (Exception exception)
        {
            HandleError(exception);
        }
    }

    private ValueTask subscribeAsync(
        IEnumerable<String> streams,
        CancellationToken cancellationToken) =>
        sendSubscriptionRequestsAsync(JsonAction.Subscribe, streams, cancellationToken);

    private ValueTask unsubscribeAsync(
        IEnumerable<String> streams,
        CancellationToken cancellationToken) =>
        sendSubscriptionRequestsAsync(JsonAction.Unsubscribe, streams, cancellationToken);

    private async ValueTask sendSubscriptionRequestsAsync(
        JsonAction action,
        IEnumerable<String> streams,
        CancellationToken cancellationToken)
    {
        foreach (var chunk in streams.Chunk(SubscriptionChunkSize))
        {
            await sendSubscriptionRequestAsync(
                action, getLookup(chunk), cancellationToken)
                .ConfigureAwait(false);
        }
    }

    private ValueTask sendSubscriptionRequestAsync(
        JsonAction action,
        ILookup<String, String> streamsByChannels,
        CancellationToken cancellationToken) =>
        streamsByChannels.Count != 0
            ? SendAsync(new JsonSubscriptionUpdate
            {
                Action = action,
                News = getSymbols(streamsByChannels, NewsChannel),
                Trades = getSymbols(streamsByChannels, TradesChannel),
                Quotes = getSymbols(streamsByChannels, QuotesChannel),
                Statuses = getSymbols(streamsByChannels, StatusesChannel),
                Lulds = getSymbols(streamsByChannels, LimitUpDownChannel),
                DailyBars = getSymbols(streamsByChannels, DailyBarsChannel),
                MinuteBars = getSymbols(streamsByChannels, MinuteBarsChannel),
                OrderBooks = getSymbols(streamsByChannels, OrderBooksChannel),
                UpdatedBars = getSymbols(streamsByChannels, UpdatedBarsChannel)
            }, cancellationToken)
            : new ValueTask();

    private static ILookup<String, String> getLookup(
        IEnumerable<String> streams) =>
        streams
            .Select(stream => stream.Split(
                _channelSeparator, 2, StringSplitOptions.RemoveEmptyEntries))
            .Where(pair => pair.Length == 2)
            .Where(pair => !_implicitChannels.Contains(pair[0]))
            .ToLookup(
                pair => pair[0],
                pair => pair[1],
                StringComparer.Ordinal);

    private static IEnumerable<String> getStreams(
        IEnumerable<String> symbols,
        String channelName) =>
        symbols.Select(symbol => getStreamName(channelName, symbol));

    private static String getStreamName(
        String channelName,
        String symbol) =>
        $"{channelName}.{symbol}";

    private static List<String>? getSymbols(
        ILookup<String, String> streamsByChannels,
        String stream) =>
        streamsByChannels[stream]
            .Where(symbol => !String.IsNullOrEmpty(symbol))
            .ToList().NullIfEmpty();
}
