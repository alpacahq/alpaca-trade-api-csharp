using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    /// <summary>
    /// OAuth key for Alpaca/Polygon APIs authentication.
    /// </summary>
    public sealed class OAuthKey : SecurityKey
    {
        /// <summary>
        /// Creates new instance of <see cref="OAuthKey"/> object.
        /// </summary>
        /// <param name="value">OAuth key.</param>
        public OAuthKey(
            String value)
            : base(value)
        {
        }

        internal override IEnumerable<KeyValuePair<String, String>> GetAuthenticationHeaders()
        {
            yield return new KeyValuePair<String, String>(
                "Authorization", "Bearer " + Value);
        }

        internal override JsonAuthRequest.JsonData GetAuthenticationData() =>
            new JsonAuthRequest.JsonData
            {
                OAuthToken = Value
            };
    }
}
