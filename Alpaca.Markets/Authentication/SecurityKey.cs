namespace Alpaca.Markets;

/// <summary>
/// Base class for 'security key' abstraction.
/// </summary>
public abstract class SecurityKey
{
    /// <summary>
    /// Creates new instance of <see cref="SecurityKey"/> object.
    /// </summary>
    /// <param name="value">Security key value.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="value"/> argument is <c>null</c>.
    /// </exception>
    protected SecurityKey(String value) => Value = value.EnsureNotNull();

    internal String Value { get; }

    internal abstract IEnumerable<KeyValuePair<String, String>> GetAuthenticationHeaders();

    internal abstract JsonAuthRequest.JsonData GetAuthenticationData();

    internal abstract JsonAuthentication GetAuthentication();
}
