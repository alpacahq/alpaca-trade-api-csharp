using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    internal sealed class FakeThrottler : IThrottler
    {
#if NET45
        private static readonly Lazy<Task> _completedTask = new Lazy<Task>(() => Task.Run(() => {}));
#endif

        private FakeThrottler() { }

        public static IThrottler Instance { get; } = new FakeThrottler();

        public Int32 MaxRetryAttempts { get; set; } = 1;

        public HashSet<Int32> RetryHttpStatuses { get; set; } = new HashSet<Int32>();

#if NET45
        public Task WaitToProceed() => _completedTask.Value;
#else
        public Task WaitToProceed() => Task.CompletedTask;
#endif

        public Boolean CheckHttpResponse(HttpResponseMessage response) => true;
    }
}