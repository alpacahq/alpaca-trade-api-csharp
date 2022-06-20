namespace Alpaca.Markets;

/// <summary>
/// OAuth key for Alpaca API authentication.
/// </summary>
[UsedImplicitly]
public sealed class OAuthKey : SecurityKey
{
    /// <summary>
    /// Creates new instance of <see cref="OAuthKey"/> object.
    /// </summary>
    /// <param name="value">OAuth key.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="value"/> argument is <c>null</c>.
    /// </exception>
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
        new()
        {
            OAuthToken = Value
        };

    [ExcludeFromCodeCoverage]
    internal override JsonAuthentication GetAuthentication() =>
        throw new InvalidOperationException();
}
