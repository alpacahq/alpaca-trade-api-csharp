using Xunit;

namespace Alpaca.Markets.Tests
{
    public sealed class RestClientPolygonTest
    {
        private readonly RestClient _restClient = RestClientFactory.GetRestClient();

        [Fact(Skip = "Timeout")]
        public async void GetExchangesWorks()
        {
            var exchanges = await _restClient.GetExchangesAsync();

            Assert.NotNull(exchanges);
            Assert.NotEmpty(exchanges);
        }
    }
}