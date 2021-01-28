using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    internal sealed class FakeThrottler : IThrottler
    {
        private FakeThrottler() { }

        public static IThrottler Instance { get; } = new FakeThrottler();

        public Int32 MaxRetryAttempts { get; } = 1;

        public Task WaitToProceed(CancellationToken _) => Task.CompletedTask;

        public Boolean CheckHttpResponse(HttpResponseMessage response) => true;
    }
}
