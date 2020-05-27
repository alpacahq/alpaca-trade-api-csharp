using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca REST API and Polygon REST API endpoints.
    /// </summary>
    [Obsolete("This class is deprecated and will be removed in the upcoming releases. Use the AlpacaDataClient, AlpacaTradingClient and PolygonDataClient classes instead.", true)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public sealed class RestClient : IDisposable
    {
        private readonly AlpacaTradingClient _alpacaTradingClient;

        private readonly PolygonDataClient _polygonDataClient;

        private readonly AlpacaDataClient _alpacaDataClient;

        /// <summary>
        /// Creates new instance of <see cref="RestClient"/> object.
        /// </summary>
        /// <param name="keyId">Application key identifier.</param>
        /// <param name="secretKey">Application secret key.</param>
        /// <param name="alpacaRestApi">Alpaca REST API endpoint URL.</param>
        /// <param name="polygonRestApi">Polygon REST API endpoint URL.</param>
        /// <param name="alpacaDataApi">Alpaca REST data API endpoint URL.</param>
        /// <param name="apiVersion">Version of Alpaca API to call.  Valid values are "1" or "2".</param>
        /// <param name="dataApiVersion">Version of Alpaca data API to call.  The only valid value is currently "1".</param>
        /// <param name="isStagingEnvironment">If <c>true</c> use staging.</param>
        /// <param name="throttleParameters">Parameters for requests throttling.</param>
        /// <param name="oauthKey">Key for alternative authentication via oauth. keyId and secretKey will be ignored if provided.</param>
        public RestClient(
            String keyId,
            String secretKey,
            String? alpacaRestApi = null,
            String? polygonRestApi = null,
            String? alpacaDataApi = null,
            Int32? apiVersion = null,
            Int32? dataApiVersion = null,
            Boolean? isStagingEnvironment = null,
            ThrottleParameters? throttleParameters = null,
            String? oauthKey = null)
            : this(createConfiguration(
                keyId,
                secretKey,
                alpacaRestApi.GetUrlSafe(Environments.Live.AlpacaTradingApi),
                polygonRestApi.GetUrlSafe(Environments.Live.PolygonDataApi),
                alpacaDataApi.GetUrlSafe(Environments.Live.AlpacaDataApi),
                apiVersion ?? (Int32)AlpacaTradingClientConfiguration.DefaultApiVersion,
                dataApiVersion ?? (Int32)AlpacaDataClientConfiguration.DefaultApiVersion,
                throttleParameters ?? ThrottleParameters.Default,
                isStagingEnvironment ?? false,
                oauthKey ?? String.Empty))
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="RestClient"/> object.
        /// </summary>
        /// <param name="keyId">Application key identifier.</param>
        /// <param name="secretKey">Application secret key.</param>
        /// <param name="alpacaRestApi">Alpaca REST API endpoint URL.</param>
        /// <param name="polygonRestApi">Polygon REST API endpoint URL.</param>
        /// <param name="alpacaDataApi">Alpaca REST data API endpoint URL.</param>
        /// <param name="apiVersion">Version of Alpaca API to call.  Valid values are "1" or "2".</param>
        /// <param name="dataApiVersion">Version of Alpaca data API to call.  The only valid value is currently "1".</param>
        /// <param name="isStagingEnvironment">If <c>true</c> use staging.</param>
        /// <param name="throttleParameters">Parameters for requests throttling.</param>
        /// <param name="oauthKey">Key for alternative authentication via oauth. keyId and secretKey will be ignored if provided.</param>
        public RestClient(
            String keyId,
            String secretKey,
            Uri alpacaRestApi,
            Uri polygonRestApi,
            Uri alpacaDataApi,
            Int32 apiVersion,
            Int32 dataApiVersion,
            ThrottleParameters throttleParameters,
            Boolean isStagingEnvironment,
            String oauthKey)
            : this(createConfiguration(
                keyId, secretKey, alpacaRestApi, polygonRestApi, alpacaDataApi, apiVersion, dataApiVersion, throttleParameters, isStagingEnvironment, oauthKey))
        {
        }

        private static RestClientConfiguration createConfiguration(
            String keyId,
            String secretKey,
            Uri alpacaRestApi,
            Uri polygonRestApi,
            Uri alpacaDataApi,
            Int32 apiVersion,
            Int32 dataApiVersion,
            ThrottleParameters throttleParameters,
            Boolean isStagingEnvironment,
            String oauthKey)
        {
            if (!(String.IsNullOrEmpty(secretKey) ^
                String.IsNullOrEmpty(oauthKey)))
            {
                throw new ArgumentException("Application secret key or OAuth key (but not both) should not be null or empty.");
            }
            return new RestClientConfiguration
            {
                KeyId = keyId ?? throw new ArgumentException("Application key id should not be null.", nameof(keyId)),
                SecurityId = String.IsNullOrEmpty(secretKey) ? (SecurityKey) new SecretKey(keyId, secretKey) : new OAuthKey(oauthKey),
                TradingApiUrl = alpacaRestApi ?? Environments.Live.AlpacaTradingApi,
                PolygonApiUrl = polygonRestApi ?? Environments.Live.PolygonDataApi,
                DataApiUrl = alpacaDataApi ?? Environments.Live.AlpacaDataApi,
                TradingApiVersion = (ApiVersion)apiVersion,
                DataApiVersion = (ApiVersion)dataApiVersion,
                ThrottleParameters = throttleParameters ?? ThrottleParameters.Default,
                IsStagingEnvironment = isStagingEnvironment
            };
        }

        /// <summary>
        /// Creates new instance of <see cref="RestClient"/> object.
        /// </summary>
        /// <param name="configuration">Configuration parameters object.</param>
        private RestClient(
            RestClientConfiguration configuration)
        {
            configuration.EnsureNotNull(nameof(configuration));

            _alpacaTradingClient = new AlpacaTradingClient(configuration.AlpacaTradingClientConfiguration);
            _polygonDataClient = new PolygonDataClient(configuration.PolygonDataClientConfiguration);
            _alpacaDataClient = new AlpacaDataClient(configuration.AlpacaDataClientConfiguration);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _alpacaTradingClient.Dispose();
            _alpacaDataClient.Dispose();
            _polygonDataClient.Dispose();
        }

        /// <summary>
        /// Gets account information from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only account information.</returns>
        public Task<IAccount> GetAccountAsync(
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.GetAccountAsync(cancellationToken);

        /// <summary>
        /// Gets account configuration settings from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Mutable version of account configuration object.</returns>
        public Task<IAccountConfiguration> GetAccountConfigurationAsync(
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.GetAccountConfigurationAsync(cancellationToken);

        /// <summary>
        /// Updates account configuration settings using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="accountConfiguration">New account configuration object for updating.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Mutable version of updated account configuration object.</returns>
        public Task<IAccountConfiguration> PatchAccountConfigurationAsync(
            IAccountConfiguration accountConfiguration,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.PatchAccountConfigurationAsync(
                accountConfiguration, cancellationToken);

        /// <summary>
        /// Gets list of account activities from Alpaca REST API endpoint by specific activity.
        /// </summary>
        /// <param name="activityType">The activity type you want to view entries for.</param>
        /// <param name="date">The date for which you want to see activities.</param>
        /// <param name="until">The response will contain only activities submitted before this date. (Cannot be used with date.)</param>
        /// <param name="after">The response will contain only activities submitted after this date. (Cannot be used with date.)</param>
        /// <param name="direction">The response will be sorted by time in this direction. (Default behavior is descending.)</param>
        /// <param name="pageSize">The maximum number of entries to return in the response.</param>
        /// <param name="pageToken">The ID of the end of your current page of results.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of asset information objects.</returns>
        public Task<IReadOnlyList<IAccountActivity>> ListAccountActivitiesAsync(
            AccountActivityType activityType,
            DateTime? date = null,
            DateTime? until = null,
            DateTime? after = null,
            SortDirection? direction = null,
            Int64? pageSize = null,
            String? pageToken = null,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.ListAccountActivitiesAsync(
                activityType, date, until, after, direction, pageSize, pageToken, cancellationToken);

        /// <summary>
        /// Gets list of account activities from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="activityTypes">The list of activity types you want to view entries for.</param>
        /// <param name="date">The date for which you want to see activities.</param>
        /// <param name="until">The response will contain only activities submitted before this date. (Cannot be used with date.)</param>
        /// <param name="after">The response will contain only activities submitted after this date. (Cannot be used with date.)</param>
        /// <param name="direction">The response will be sorted by time in this direction. (Default behavior is descending.)</param>
        /// <param name="pageSize">The maximum number of entries to return in the response.</param>
        /// <param name="pageToken">The ID of the end of your current page of results.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of account activity record objects.</returns>
        public Task<IReadOnlyList<IAccountActivity>> ListAccountActivitiesAsync(
            IEnumerable<AccountActivityType>? activityTypes = null,
            DateTime? date = null,
            DateTime? until = null,
            DateTime? after = null,
            SortDirection? direction = null,
            Int64? pageSize = null,
            String? pageToken = null,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.ListAccountActivitiesAsync(
                activityTypes, date, until, after, direction, pageSize, pageToken, cancellationToken);

        /// <summary>
        /// Gets portfolio equity history from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="startDate">Start date for desired history.</param>
        /// <param name="endDate">End date for desired history. Default value is today. </param>
        /// <param name="period">Period value for desired history. Default value is 1 month.</param>
        /// <param name="timeFrame">
        /// Time frame value for desired history.
        /// Default value is 1 minute for a period shorter than 7 days, 15 minutes for a period less than 30 days, or 1 day for a longer period.
        /// </param>
        /// <param name="extendedHours">If true, include extended hours in the result. This is effective only for time frame less than 1 day.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of account activity record objects.</returns>
        public Task<IPortfolioHistory> GetPortfolioHistoryAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            TimeFrame? timeFrame = null,
            HistoryPeriod? period = null,
            Boolean? extendedHours = null,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.GetPortfolioHistoryAsync(
                startDate, endDate, timeFrame, period, extendedHours, cancellationToken);

        /// <summary>
        /// Gets list of available assets from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="assetStatus">Asset status for filtering.</param>
        /// <param name="assetClass">Asset class for filtering.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of asset information objects.</returns>
        public Task<IReadOnlyList<IAsset>> ListAssetsAsync(
            AssetStatus? assetStatus = null,
            AssetClass? assetClass = null,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.ListAssetsAsync(
                assetStatus, assetClass, cancellationToken);

        /// <summary>
        /// Get single asset information by asset name from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for searching.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only asset information.</returns>
        public Task<IAsset> GetAssetAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.GetAssetAsync(symbol, cancellationToken);

        /// <summary>
        /// Gets list of available orders from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="orderStatusFilter">Order status for filtering.</param>
        /// <param name="orderListSorting">The chronological order of response based on the submission time.</param>
        /// <param name="untilDateTimeExclusive">Returns only orders until specified timestamp (exclusive).</param>
        /// <param name="afterDateTimeExclusive">Returns only orders after specified timestamp (exclusive).</param>
        /// <param name="limitOrderNumber">Maximal number of orders in response.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of order information objects.</returns>
        public Task<IReadOnlyList<IOrder>> ListOrdersAsync(
            OrderStatusFilter? orderStatusFilter = null,
            SortDirection? orderListSorting = null,
            DateTime? untilDateTimeExclusive = null,
            DateTime? afterDateTimeExclusive = null,
            Int64? limitOrderNumber = null,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.ListOrdersAsync(
                orderStatusFilter, orderListSorting, untilDateTimeExclusive,
                afterDateTimeExclusive, limitOrderNumber, cancellationToken);

        /// <summary>
        /// Creates new order for execution using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Order asset name.</param>
        /// <param name="quantity">Order quantity.</param>
        /// <param name="side">Order side (buy or sell).</param>
        /// <param name="type">Order type.</param>
        /// <param name="duration">Order duration.</param>
        /// <param name="limitPrice">Order limit price.</param>
        /// <param name="stopPrice">Order stop price.</param>
        /// <param name="clientOrderId">Client order ID.</param>
        /// <param name="extendedHours">Whether or not this order should be allowed to execute during extended hours trading.</param>
        /// <param name="orderClass">Order class for advanced order types.</param>
        /// <param name="takeProfitLimitPrice">Profit taking limit price for for advanced order types.</param>
        /// <param name="stopLossStopPrice">Stop loss stop price for for advanced order types.</param>
        /// <param name="stopLossLimitPrice">Stop loss limit price for for advanced order types.</param>
        /// <param name="nested">Whether or not child orders should be listed as 'legs' of parent orders. (Advanced order types only.)</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only order information object for newly created order.</returns>
        public Task<IOrder> PostOrderAsync(
            String symbol,
            Int64 quantity,
            OrderSide side,
            OrderType type,
            TimeInForce duration,
            Decimal? limitPrice = null,
            Decimal? stopPrice = null,
            String? clientOrderId = null,
            Boolean? extendedHours = null,
            OrderClass? orderClass = null,
            Decimal? takeProfitLimitPrice = null,
            Decimal? stopLossStopPrice = null,
            Decimal? stopLossLimitPrice = null,
            Boolean? nested = false,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.PostOrderAsync(
                symbol, quantity, side, type, duration, limitPrice, stopPrice,
                clientOrderId, extendedHours, orderClass, takeProfitLimitPrice,
                stopLossStopPrice, stopLossLimitPrice,nested, cancellationToken);

        /// <summary>
        /// Updates existing order using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="orderId">Server side order identifier.</param>
        /// <param name="quantity">Updated order quantity or <c>null</c> if quantity is not changed.</param>
        /// <param name="duration">Updated order duration or <c>null</c> if duration is not changed.</param>
        /// <param name="limitPrice">Updated order limit price or <c>null</c> if limit price is not changed.</param>
        /// <param name="stopPrice">Updated order stop price or <c>null</c> if stop price is not changed.</param>
        /// <param name="clientOrderId">Updated client order ID or <c>null</c> if client order ID is not changed.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only order information object for updated order.</returns>
        public Task<IOrder> PatchOrderAsync(
            Guid orderId,
            Int64? quantity = null,
            TimeInForce? duration = null,
            Decimal? limitPrice = null,
            Decimal? stopPrice = null,
            String? clientOrderId = null,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.PatchOrderAsync(
                orderId, quantity, duration, limitPrice, stopPrice, clientOrderId, cancellationToken);

        /// <summary>
        /// Get single order information by client order ID from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="clientOrderId">Client order ID for searching.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only order information object.</returns>
        public Task<IOrder> GetOrderAsync(
            String clientOrderId,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.GetOrderAsync(clientOrderId, cancellationToken);

        /// <summary>
        /// Get single order information by server order ID from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="orderId">Server order ID for searching.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only order information object.</returns>
        public Task<IOrder> GetOrderAsync(
            Guid orderId,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.GetOrderAsync(orderId, cancellationToken);

        /// <summary>
        /// Deletes/cancel order on server by server order ID using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="orderId">Server order ID for cancelling.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns><c>True</c> if order cancellation was accepted.</returns>
        public Task<Boolean> DeleteOrderAsync(
            Guid orderId,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.DeleteOrderAsync(orderId, cancellationToken);

        /// <summary>
        /// Deletes/cancel all open orders using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>List of order cancellation status objects.</returns>
        public Task<IReadOnlyList<IOrderActionStatus>> DeleteAllOrdersAsync(
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.DeleteAllOrdersAsync(cancellationToken);

        /// <summary>
        /// Gets list of available positions from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of position information objects.</returns>
        public Task<IReadOnlyList<IPosition>> ListPositionsAsync(
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.ListPositionsAsync(cancellationToken);

        /// <summary>
        /// Gets position information by asset name from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Position asset name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only position information object.</returns>
        public Task<IPosition> GetPositionAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.GetPositionAsync(symbol, cancellationToken);

        /// <summary>
        /// Liquidates all open positions at market price using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>List of position cancellation status objects.</returns>
        public Task<IReadOnlyList<IPositionActionStatus>> DeleteAllPositionsAsync(
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.DeleteAllPositionsAsync(cancellationToken);

        /// <summary>
        /// Liquidate an open position at market price using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Symbol for liquidation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns><c>True</c> if position liquidation was accepted.</returns>
        public Task<Boolean> DeletePositionAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.DeletePositionAsync(symbol, cancellationToken);

        /// <summary>
        /// Get current time information from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only clock information object.</returns>
        public Task<IClock> GetClockAsync(
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.GetClockAsync(cancellationToken);

        /// <summary>
        /// Gets list of trading days from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="startDateInclusive">Start time for filtering (inclusive).</param>
        /// <param name="endDateInclusive">End time for filtering (inclusive).</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of trading date information object.</returns>
        public Task<IReadOnlyList<ICalendar>> ListCalendarAsync(
            DateTime? startDateInclusive = null,
            DateTime? endDateInclusive = null,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.ListCalendarAsync(
                startDateInclusive, endDateInclusive, cancellationToken);

        /// <summary>
        /// Gets lookup table of historical daily bars lists for all assets from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbols">>Asset names for data retrieval.</param>
        /// <param name="timeFrame">Type of time bars for retrieval.</param>
        /// <param name="areTimesInclusive">
        /// If <c>true</c> - both <paramref name="timeFrom"/> and <paramref name="timeInto"/> parameters are treated as inclusive.
        /// </param>
        /// <param name="timeFrom">Start time for filtering.</param>
        /// <param name="timeInto">End time for filtering.</param>
        /// <param name="limit">Maximal number of daily bars in data response.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of daily bars for specified asset.</returns>
        public Task<IReadOnlyDictionary<String, IReadOnlyList<IAgg>>> GetBarSetAsync(
            IEnumerable<String> symbols,
            TimeFrame timeFrame,
            Int32? limit = 100,
            Boolean areTimesInclusive = true,
            DateTime? timeFrom = null,
            DateTime? timeInto = null,
            CancellationToken cancellationToken = default) =>
            _alpacaDataClient.GetBarSetAsync(symbols, timeFrame, limit, areTimesInclusive, timeFrom, timeInto, cancellationToken);

        /// <summary>
        /// Gets list of available exchanges from Polygon REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of exchange information objects.</returns>
        public Task<IReadOnlyList<IExchange>> ListExchangesAsync(
            CancellationToken cancellationToken = default) =>
            _polygonDataClient.ListExchangesAsync(cancellationToken);

        /// <summary>
        /// Gets mapping dictionary for symbol types from Polygon REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>
        /// Read-only dictionary with keys equal to symbol type abbreviation and values
        /// equal to full symbol type names descriptions for each supported symbol type.
        /// </returns>
        public Task<IReadOnlyDictionary<String, String>> GetSymbolTypeMapAsync(
            CancellationToken cancellationToken = default) =>
            _polygonDataClient.GetSymbolTypeMapAsync(cancellationToken);

        /// <summary>
        /// Gets list of historical trades for a single asset from Polygon's REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="date">Single date for data retrieval.</param>
        /// <param name="timestamp">Paging - Using the timestamp of the last result will give you the next page of results.</param>
        /// <param name="timestampLimit">Maximum timestamp allowed in the results.</param>
        /// <param name="limit">Limits the size of the response.</param>
        /// <param name="reverse">Reverses the order of the results.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of historical trade information.</returns>
        public Task<IHistoricalItems<IHistoricalTrade>> ListHistoricalTradesAsync(
            String symbol,
            DateTime date,
            Int64? timestamp = null,
            Int64? timestampLimit = null,
            Int32? limit = null,
            Boolean? reverse = null,
            CancellationToken cancellationToken = default) =>
            _polygonDataClient.ListHistoricalTradesAsync(symbol, date, timestamp, timestampLimit, limit, reverse, cancellationToken);

        /// <summary>
        /// Gets list of historical trades for a single asset from Polygon's REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="date">Single date for data retrieval.</param>
        /// <param name="timestamp">Paging - Using the timestamp of the last result will give you the next page of results.</param>
        /// <param name="timestampLimit">Maximum timestamp allowed in the results.</param>
        /// <param name="limit">Limits the size of the response.</param>
        /// <param name="reverse">Reverses the order of the results.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of historical trade information.</returns>
        public Task<IHistoricalItems<IHistoricalQuote>> ListHistoricalQuotesAsync(
            String symbol,
            DateTime date,
            Int64? timestamp = null,
            Int64? timestampLimit = null,
            Int32? limit = null,
            Boolean? reverse = null,
            CancellationToken cancellationToken = default) =>
            _polygonDataClient.ListHistoricalQuotesAsync(symbol, date, timestamp, timestampLimit, limit, reverse, cancellationToken);

        /// <summary>
        /// Gets list of historical minute bars for single asset from Polygon's v2 REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="multiplier">Number of bars to combine in each result.</param>
        /// <param name="dateFromInclusive">Start time for filtering (inclusive).</param>
        /// <param name="dateToInclusive">End time for filtering (inclusive).</param>
        /// <param name="unadjusted">Set to true if the results should not be adjusted for splits.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of minute bars for specified asset.</returns>
        public Task<IHistoricalItems<IAgg>> ListMinuteAggregatesAsync(
            String symbol,
            Int32 multiplier,
            DateTime dateFromInclusive,
            DateTime dateToInclusive,
            Boolean unadjusted = false,
            CancellationToken cancellationToken = default) =>
            _polygonDataClient.ListMinuteAggregatesAsync(symbol, multiplier, dateFromInclusive, dateToInclusive, unadjusted, cancellationToken);

        
        /// <summary>
        /// Gets list of historical hour bars for single asset from Polygon's v2 REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="multiplier">Number of bars to combine in each result.</param>
        /// <param name="dateFromInclusive">Start time for filtering (inclusive).</param>
        /// <param name="dateToInclusive">End time for filtering (inclusive).</param>
        /// <param name="unadjusted">Set to true if the results should not be adjusted for splits.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of minute bars for specified asset.</returns>
        public Task<IHistoricalItems<IAgg>> ListHourAggregatesAsync(
            String symbol,
            Int32 multiplier,
            DateTime dateFromInclusive,
            DateTime dateToInclusive,
            Boolean unadjusted = false,
            CancellationToken cancellationToken = default) =>
            _polygonDataClient.ListHourAggregatesAsync(symbol, multiplier, dateFromInclusive, dateToInclusive, unadjusted, cancellationToken);

        /// <summary>
        /// Gets list of historical minute bars for single asset from Polygon's v2 REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="multiplier">Number of bars to combine in each result.</param>
        /// <param name="dateFromInclusive">Start time for filtering (inclusive).</param>
        /// <param name="dateToInclusive">End time for filtering (inclusive).</param>
        /// <param name="unadjusted">Set to true if the results should not be adjusted for splits.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of day bars for specified asset.</returns>
        public Task<IHistoricalItems<IAgg>> ListDayAggregatesAsync(
            String symbol,
            Int32 multiplier,
            DateTime dateFromInclusive,
            DateTime dateToInclusive,
            Boolean unadjusted = false,
            CancellationToken cancellationToken = default) =>
            _polygonDataClient.ListDayAggregatesAsync(symbol, multiplier, dateFromInclusive, dateToInclusive, unadjusted, cancellationToken);

        /// <summary>
        /// Gets last trade for singe asset from Polygon REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only last trade information.</returns>
        public Task<ILastTrade> GetLastTradeAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _polygonDataClient.GetLastTradeAsync(symbol, cancellationToken);

        /// <summary>
        /// Gets current quote for singe asset from Polygon REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only current quote information.</returns>
        public Task<ILastQuote> GetLastQuoteAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _polygonDataClient.GetLastQuoteAsync(symbol, cancellationToken);

        /// <summary>
        /// Gets mapping dictionary for specific tick type from Polygon REST API endpoint.
        /// </summary>
        /// <param name="tickType">Tick type for conditions map.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>
        /// Read-only dictionary with keys equal to condition integer values and values
        /// equal to full tick condition descriptions for each supported tick type.
        /// </returns>
        public Task<IReadOnlyDictionary<Int64, String>> GetConditionMapAsync(
            TickType tickType = TickType.Trades,
            CancellationToken cancellationToken = default) =>
            _polygonDataClient.GetConditionMapAsync(tickType, cancellationToken);

        /// <summary>
        /// Gets list of watch list objects from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of watch list objects.</returns>
        public Task<IReadOnlyList<IWatchList>> ListWatchListsAsync(
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.ListWatchListsAsync(cancellationToken);

        /// <summary>
        /// Add new watch list object into Alpaca REST API endpoint.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="assets">List of asset names for new watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Newly created watch list object.</returns>
        public Task<IWatchList> CreateWatchListAsync(
            String name,
            IEnumerable<String>? assets = null,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.CreateWatchListAsync(name, assets, cancellationToken);

        /// <summary>
        /// Get watch list object from Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="watchListId">Unique watch list identifier.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Watch list object with proper <paramref name="watchListId"/> value.</returns>
        public Task<IWatchList> GetWatchListByIdAsync(
            Guid watchListId,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.GetWatchListByIdAsync(watchListId, cancellationToken);

        /// <summary>
        /// Get watch list object from Alpaca REST API endpoint by watch list user-defined name.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Watch list object with proper <paramref name="name"/> value.</returns>
        public Task<IWatchList> GetWatchListByNameAsync(
            String name,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.GetWatchListByNameAsync(name, cancellationToken);

        /// <summary>
        /// Updates watch list object from Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="watchListId">Unique watch list identifier.</param>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="assets">List of asset names for new watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="watchListId"/> value.</returns>
        public Task<IWatchList> UpdateWatchListByIdAsync(
            Guid watchListId,
            String name,
            IEnumerable<String> assets,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.UpdateWatchListByIdAsync(watchListId, name, assets, cancellationToken);

        /// <summary>
        /// Adds asset into watch list using Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="watchListId">Unique watch list identifier.</param>
        /// <param name="asset">Asset name for adding into watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="watchListId"/> value.</returns>
        public Task<IWatchList> AddAssetIntoWatchListByIdAsync(
            Guid watchListId,
            String asset,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.AddAssetIntoWatchListByIdAsync(watchListId, asset, cancellationToken);

        /// <summary>
        /// Adds asset into watch list using Alpaca REST API endpoint by watch list name.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="asset">Asset name for adding into watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="name"/> value.</returns>
        public Task<IWatchList> AddAssetIntoWatchListByNameAsync(
            String name,
            String asset,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.AddAssetIntoWatchListByNameAsync(name, asset, cancellationToken);

        /// <summary>
        /// Deletes asset from watch list using Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="watchListId">Unique watch list identifier.</param>
        /// <param name="asset">Asset name for adding into watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="watchListId"/> value.</returns>
        public Task<IWatchList> DeleteAssetFromWatchListByIdAsync(
            Guid watchListId,
            String asset,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.DeleteAssetFromWatchListByIdAsync(watchListId, asset, cancellationToken);

        /// <summary>
        /// Deletes asset from watch list using Alpaca REST API endpoint by watch list name.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="asset">Asset name for adding into watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="name"/> value.</returns>
        public Task<IWatchList> DeleteAssetFromWatchListByNameAsync(
            String name,
            String asset,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.DeleteAssetFromWatchListByNameAsync(name, asset, cancellationToken);

        /// <summary>
        /// Deletes watch list from Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="watchListId">Unique watch list identifier.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Returns <c>true</c> if operation completed successfully.</returns>
        public Task<Boolean> DeleteWatchListByIdAsync(
            Guid watchListId,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.DeleteWatchListByIdAsync(watchListId, cancellationToken);

        /// <summary>
        /// Deletes watch list from Alpaca REST API endpoint by watch list name.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Returns <c>true</c> if operation completed successfully.</returns>
        public Task<Boolean> DeleteWatchListByNameAsync(
            String name,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.DeleteWatchListByNameAsync(name, cancellationToken);
    }
}
