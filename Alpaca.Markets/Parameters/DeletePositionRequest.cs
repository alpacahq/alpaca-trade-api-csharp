using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaTradingClient.DeletePositionAsync(DeletePositionRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public sealed class DeletePositionRequest : Validation.IRequest
    {
        /// <summary>
        /// Creates new instance of <see cref="DeletePositionRequest"/> object.
        /// </summary>
        /// <param name="symbol">Symbol for liquidation.</param>
        public DeletePositionRequest(
            String symbol)
        {
            Symbol = symbol ?? throw new ArgumentException(
                "Symbol name cannot be null.", nameof(symbol));
        }

        /// <summary>
        /// Gets or sets the custom position liquidation size (if missed the position will be liquidated completely).
        /// </summary>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public PositionQuantity? PositionQuantity { get; [UsedImplicitly] set; }

        /// <summary>
        /// Gets the symbol for liquidation.
        /// </summary>
        public String Symbol { get; }

        internal async ValueTask<UriBuilder> GetUriBuilderAsync(
            HttpClient httpClient) =>
            new (httpClient.BaseAddress!)
            {
                Path = $"v2/positions/{Symbol}",
                Query = await new QueryBuilder()
                    .AddParameter("percentage", PositionQuantity?.AsPercentage())
                    .AddParameter("qty", PositionQuantity?.AsFractional())
                    .AsStringAsync().ConfigureAwait(false)
            };

        IEnumerable<RequestValidationException> Validation.IRequest.GetExceptions()
        {
            if (String.IsNullOrEmpty(Symbol))
            {
                yield return new RequestValidationException(
                    "Symbols shouldn't be empty.", nameof(Symbol));
            }
        }
    }
}
