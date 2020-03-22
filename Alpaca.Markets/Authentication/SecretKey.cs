using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    /// <summary>
    /// Secret API key for Alpaca/Polygon APIs authentication.
    /// </summary>
    public sealed class SecretKey : SecurityKey
    {
        /// <summary>
        /// Creates new instance of <see cref="SecretKey"/> object.
        /// </summary>
        /// <param name="keyId">Secret API key identifier.</param>
        /// <param name="value">Secret API key value.</param>
        public SecretKey(
            String keyId,
            String value)
            : base(value) =>
            KeyId = keyId;

        private String KeyId { get; }

        internal override IEnumerable<KeyValuePair<String, String>> GetAuthenticationHeaders()
        {
            yield return new KeyValuePair<String, String>(
                "APCA-API-KEY-ID", KeyId);
            yield return new KeyValuePair<String, String>(
                "APCA-API-SECRET-KEY", Value);
        }

        internal override JsonAuthRequest.JsonData GetAuthenticationData() =>
            new JsonAuthRequest.JsonData
            {
                KeyId = KeyId,
                SecretKey = Value
            };
    }
}
