using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAlpacaDataSubscription
    {
        /// <summary>
        /// 
        /// </summary>
        String Stream { get; }

        /// <summary>
        /// 
        /// </summary>
        Boolean Subscribed { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TApi"></typeparam>
    public interface IAlpacaDataSubscription<out TApi> : IAlpacaDataSubscription
    {
        /// <summary>
        /// 
        /// </summary>
        event Action<TApi> Received;
    }
}
