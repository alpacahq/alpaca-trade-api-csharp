using System;
using Xunit;

namespace Alpaca.Markets.Tests
{
    public sealed class RestClientTest
    {
        [Fact]
        public void GetAccountsWorks()
        {
            var restClient = new RestClient(
                "AKEW7ZBQUSNUHOJNQ5MS",
                "Yr2Tms89rQ6foRLNu4pz3w/yXOrxQGDmXctU1BCn",
                new Uri("https://staging-api.tradetalk.us"));

            var account = restClient.GetAccount();

            Assert.NotNull(account);
            Assert.Equal("USD", account.Currency);
        }
    }
}
