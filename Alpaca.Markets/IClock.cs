using System;

namespace Alpaca.Markets
{
    public interface IClock
    {
        DateTime Timestamp { get; }

        Boolean IsOpen { get; }

        DateTime NextOpen { get; }

        DateTime NextClose { get;  }
    }
}