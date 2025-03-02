using System.Text;
using System.Net;

namespace Alpaca.Markets;

/// <summary>
/// Basie key for Alpaca Broker API authentication.
/// </summary>
[UsedImplicitly]
public sealed class BasicKey : SecurityKey
{
    /// <summary>
    /// Creates new instance of <see cref="BasicKey"/> object.
    /// </summary>
    /// <param name="credential">User credentials (name and password).</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="credential"/> argument is <c>null</c>.
    /// </exception>
    public BasicKey(
        NetworkCredential credential)
        : base(Convert.ToBase64String(Encoding.ASCII.GetBytes(
            $"{credential.EnsureNotNull().UserName}:{credential.Password}")))
    {
    }

    internal override IEnumerable<KeyValuePair<String, String>> GetAuthenticationHeaders()
    {
        yield return new KeyValuePair<String, String>(
            "Authorization", "Basic " + Value);
    }

    internal override JsonAuthRequest.JsonData GetAuthenticationData() =>
        throw new InvalidOperationException();

    [ExcludeFromCodeCoverage]
    internal override JsonAuthentication GetAuthentication() =>
        throw new InvalidOperationException();
}
