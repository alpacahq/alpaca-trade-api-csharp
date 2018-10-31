using System;

namespace Alpaca.Markets
{
    internal sealed class FakeThrottler : IThrottler
    {
        private FakeThrottler() { }

        public static IThrottler Instance { get; } = new FakeThrottler();

        public Int32 MaxAttempts => 1;

        public void WaitToProceed() { }
    }
}