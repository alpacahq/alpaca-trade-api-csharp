using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    [SuppressMessage("Naming", "CA1720:Identifier contains type name",
        Justification = "We use these names because they explain situation better.")]
    public enum Multiplier
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,

        /// <summary>
        /// 
        /// </summary>
        Single = 1,

        /// <summary>
        /// 
        /// </summary>
        Double = 2,

        /// <summary>
        /// 
        /// </summary>
        Quadruple = 4
    }
}
