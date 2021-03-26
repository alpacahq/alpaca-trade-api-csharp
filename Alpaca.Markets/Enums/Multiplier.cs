using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Account multiplier value (enum values can be cast to related integer values).
    /// </summary>
    [Flags]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("Naming", "CA1720:Identifier contains type name",
        Justification = "We use these names because they explain situation better.")]
    public enum Multiplier
    {
        /// <summary>
        /// Invalid default value of enum.
        /// </summary>
        None = 0,

        /// <summary>
        /// Standard limited margin account with 1x buying power.
        /// </summary>
        Single = 1,

        /// <summary>
        /// Regular T margin account with 2x intraday and overnight buying power;
        /// this is the default for all non-PDT accounts with $2,000 or more equity.
        /// </summary>
        Double = 2,

        /// <summary>
        /// PDT account with 4x intraday buying power and 2x reg T overnight buying power.
        /// </summary>
        Quadruple = 4
    }
}
