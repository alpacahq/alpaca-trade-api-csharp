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

        public HashSet<Int32> RetryHttpStatuses { get; set; } = new HashSet<Int32>();

        public void AllStop(Int32 milliseconds) { }

        public void WaitToProceed() { }

        public Boolean CheckHttpResponse(HttpResponseMessage response) => true;
    }
}