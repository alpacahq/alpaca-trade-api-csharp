using System;
using System.Collections.Generic;

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

        internal abstract IEnumerable<KeyValuePair<String, String>> GetAuthenticationHeaders();

        internal abstract JsonAuthRequest.JsonData GetAuthenticationData();
    }
}
