﻿using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    /// <summary>
    /// Configuration parameters object for <see cref="AlpacaDataClient"/> class.
    /// </summary>
    public sealed class AlpacaDataClientConfiguration
    {
        private static readonly HashSet<ApiVersion> _supportedDataApiVersions = new HashSet<ApiVersion> { ApiVersion.V1 };

        internal const ApiVersion DefaultApiVersion = ApiVersion.V1;

        /// <summary>
        /// Creates new instance of <see cref="AlpacaDataClientConfiguration"/> class.
        /// </summary>
        public AlpacaDataClientConfiguration()
        {
            ApiVersion = DefaultApiVersion;
            SecurityId = new SecretKey(String.Empty, String.Empty);
            ApiEndpoint = Environments.Live.AlpacaDataApi;
        }

        /// <summary>
        /// Security identifier for API authentication.
        /// </summary>
        public SecurityKey SecurityId { get; set; }

        /// <summary>
        /// Gets or sets Alpaca Data API base URL.
        /// </summary>
        public Uri ApiEndpoint { get; set; }

        /// <summary>
        /// Gets or sets Alpaca Trading API version.
        /// </summary>
        public ApiVersion ApiVersion { get; set; }

        internal void EnsureIsValid()
        {
            if (ApiEndpoint == null)
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(ApiEndpoint)}' property shouldn't be null.");
            }

            if (!_supportedDataApiVersions.Contains(ApiVersion))
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(ApiVersion)}' property is invalid.");
            }
        }
    }
}
