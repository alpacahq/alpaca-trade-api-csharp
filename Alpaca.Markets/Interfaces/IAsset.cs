using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates asset information from Alpaca REST API.
    /// </summary>
    public interface IAsset
    {
        /// <summary>
        /// Gets unique asset identifier.
        /// </summary>
        Guid AssetId { get; }

        /// <summary>
        /// Gets asset class.
        /// </summary>
        [SuppressMessage(
            "Naming", "CA1716:Identifiers should not match keywords",
            Justification = "Already used by clients and creates conflict only in VB.NET")]
        AssetClass Class { get; }

        /// <summary>
        /// Gets asset source exchange.
        /// </summary>
        Exchange Exchange { get; }

        /// <summary>
        /// Gets asset name.
        /// </summary>
        String Symbol { get; }

        /// <summary>
        /// Get asset status in API.
        /// </summary>
        AssetStatus Status { get; }

        /// <summary>
        /// Returns <c>true</c> if asset is tradable.
        /// </summary>
        Boolean IsTradable { get; }

        /// <summary>
        /// Asset is marginable or not
        /// </summary>
        Boolean Marginable { get; }

        /// <summary>
        /// Asset is shortable or not
        /// </summary>
        Boolean Shortable { get; }

        /// <summary>
        /// Asset is easy-to-borrow or not
        /// </summary>
        Boolean EasyToBorrow { get; }
    }
}
