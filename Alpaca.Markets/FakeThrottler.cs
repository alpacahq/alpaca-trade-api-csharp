using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Alpaca.Markets
{
    internal sealed class FakeThrottler : IThrottler
    {
        private FakeThrottler() { }

        public static IThrottler Instance { get; } = new FakeThrottler();

        public Int32 MaxRetryAttempts { get; set; } = 1;

        public IEnumerable<int> RetryHttpStatuses { get; set; } = new Int32[] { };

        public void AllStop(int milliseconds) { }

        public void WaitToProceed() { }

        public bool CheckHttpResponse(HttpResponseMessage response) => true;
    }
}