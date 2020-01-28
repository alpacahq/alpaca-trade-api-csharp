using System;
using System.Net.Http;

namespace Alpaca.Markets
{
    /// <summary>
    /// Base class for 'security key' abstraction.
    /// </summary>
    public abstract class SecurityKey
    {
        /// <summary>
        /// Creates new instance of <see cref="SecurityKey"/> object.
        /// </summary>
        /// <param name="value">Security key value.</param>
        protected SecurityKey(String value) => 
            Value = value ?? throw new ArgumentNullException(nameof(value));

        internal String Value { get; }

        internal abstract void AddAuthenticationHeader(
            HttpClient httpClient,
            String keyId);

        internal static SecurityKey Create(
            String secretKey,
            String oauthKey) => 
            String.IsNullOrEmpty(secretKey)
                ? (SecurityKey) new SecretKey(secretKey)
                : new OAuthKey(oauthKey);
    }
}
