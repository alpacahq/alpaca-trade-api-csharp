using System.Net.Http.Headers;

namespace Alpaca.Markets.Tests;

public sealed class HttpClientExtensionsTest
{
    // Real value observed on Mono/Xamarin runtimes. Mono.Runtime.GetDisplayName()'s format
    // is explicitly documented as unstable and commonly includes characters (parentheses,
    // colons, slashes) that are invalid inside an HTTP product token.
    private const String MonoFrameworkDescription =
        "Mono 6.12.0.122 (2020-02/8e1b8f4 Fri Feb 14 12:20:22 EST 2020)";

    [Fact]
    public void ConfigureSetsUserAgentHeaderInExpectedFormat()
    {
        using var httpClient = new HttpClient();
        var securityKey = new SecretKey(
            Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

        httpClient.Configure(securityKey, new Uri("https://paper-api.alpaca.markets"));

        var userAgent = httpClient.DefaultRequestHeaders.UserAgent.ToList();

        Assert.Equal(2, userAgent.Count);
        assertValidProductToken(userAgent[0], "APCA-DOTNET");
        assertValidProductToken(userAgent[1]);

        return;

        static void assertValidProductToken(
            ProductInfoHeaderValue productInfo,
            String? expectedName = null)
        {
            var product = productInfo.Product;

            Assert.NotNull(product);
            Assert.False(String.IsNullOrEmpty(product!.Version));
            Assert.DoesNotContain(' ', product.Name);
            Assert.DoesNotContain(' ', product.Version!);
            Assert.Matches(@"^\d+(\.\d+){1,3}$", product.Version!);

            if (expectedName is not null)
            {
                Assert.Equal(expectedName, product.Name);
            }
        }
    }

    [Theory]
    [InlineData(".NET 8.0.7", ".NET", "8.0.7")]
    [InlineData(".NET Core 3.1.32", ".NETCore", "3.1.32")]
    [InlineData(".NET Framework 4.8.9037.0", ".NETFramework", "4.8.9037.0")]
    [InlineData("Mono 6.12.0.122", "Mono", "6.12.0.122")]
    public void ParseRuntimeInfoParsesKnownFrameworkDescriptions(
        String frameworkDescription,
        String expectedName,
        String expectedVersion)
    {
        var (name, version) = HttpClientExtensions.ParseRuntimeInfo(frameworkDescription);

        Assert.Equal(expectedName, name);
        Assert.Equal(expectedVersion, version);
    }

    [Theory]
    [InlineData(MonoFrameworkDescription)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("SomeRuntimeWithNoVersionAtAll")]
    [InlineData("Weird/Runtime:Name (with) [separators] 1.2.3")]
    public void ParseRuntimeInfoNeverProducesAnInvalidProductToken(
        String frameworkDescription)
    {
        var (name, version) = HttpClientExtensions.ParseRuntimeInfo(frameworkDescription);

        Assert.False(String.IsNullOrEmpty(name));
        Assert.False(String.IsNullOrEmpty(version));

        // The real regression: constructing the header value must never throw, no matter
        // how malformed the underlying runtime description is (e.g. Mono's, which is
        // explicitly documented as an unstable format).
        var exception = Record.Exception(() => new ProductInfoHeaderValue(name, version));
        Assert.Null(exception);
    }

    [Fact]
    public void ParseRuntimeInfoExtractsRealMonoVersionFromNoisyDescription()
    {
        var (name, version) = HttpClientExtensions.ParseRuntimeInfo(MonoFrameworkDescription);

        Assert.Equal("Mono", name);
        Assert.Equal("6.12.0.122", version);
    }
}
