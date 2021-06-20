using System;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates implementations of the <see cref="ITimeInterval"/> interface and helper methods for it.
    /// </summary>
    public static class TimeInterval
    {
        private readonly struct Inclusive : IInclusiveTimeInterval, IEquatable<Inclusive>
        {
            internal Inclusive(
                DateTime? from,
                DateTime? into)
            {
                if (from > into)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(from), "Time interval should be valid.");
                }

                From = from;
                Into = into;
            }

            public DateTime? From { get; }

            public DateTime? Into { get; }

            public Boolean Equals(Inclusive other) =>
                Nullable.Equals(From, other.From) && 
                Nullable.Equals(Into, other.Into);

            public override Boolean Equals(Object? obj) => 
                obj is Inclusive other && Equals(other);

            public override Int32 GetHashCode()
            {
                unchecked
                {
                    return (From.GetHashCode() * 397) ^ Into.GetHashCode();
                }
            }
        }

        private readonly struct Exclusive : IExclusiveTimeInterval, IEquatable<Exclusive>
        {
            internal Exclusive(
                DateTime? from,
                DateTime? into)
            {
                if (from > into)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(from), "Time interval should be valid.");
                }

                From = from;
                Into = into;
            }

            public DateTime? From { get; }

            public DateTime? Into { get; }

            public Boolean Equals(Exclusive other) =>
                Nullable.Equals(From, other.From) && 
                Nullable.Equals(Into, other.Into);

            public override Boolean Equals(Object? obj) => 
                obj is Exclusive other && Equals(other);

            public override Int32 GetHashCode()
            {
                unchecked
                {
                    return (From.GetHashCode() * 397) ^ Into.GetHashCode();
                }
            }
        }

        /// <summary>
        /// Gets boolean flag signals that time interval is empty (both start and end date equal to <c>null</c>).
        /// </summary>
        /// <param name="interval">Target time interval for checking.</param>
        /// <returns>
        /// Returns <c>true</c> if both <see cref="ITimeInterval.From"/> and <see cref="ITimeInterval.Into"/> equal to <c>null</c>.
        /// </returns>
        public static Boolean IsEmpty(
            this ITimeInterval interval) =>
            interval.EnsureNotNull(nameof(interval)).Into is null && interval.From is null;

        /// <summary>
        /// Gets boolean flag signals that time interval is open (both start or end date equal to <c>null</c>).
        /// </summary>
        /// <param name="interval">Target time interval for checking.</param>
        /// <returns>
        /// Returns <c>true</c> if both <see cref="ITimeInterval.From"/> or <see cref="ITimeInterval.Into"/> equal to <c>null</c>.
        /// </returns>
        public static Boolean IsOpen(
            this ITimeInterval interval) =>
            interval.EnsureNotNull(nameof(interval)).Into is null ^ interval.From is null;

        /// <summary>
        /// Set inclusive time interval for <paramref name="request"/> object.
        /// </summary>
        /// <param name="request">Target request for setting filtering interval.</param>
        /// <param name="from">Starting date/time point for filtering.</param>
        /// <param name="into">Ending date/time point for filtering.</param>
        /// <returns>Fluent interface - returns <paramref name="request"/> object.</returns>
        public static TRequest SetInclusiveTimeInterval<TRequest>(
            this TRequest request,
            DateTime from,
            DateTime into)
            where TRequest : IRequestWithTimeInterval<IInclusiveTimeInterval> =>
            request.SetTimeInterval(new Inclusive(from, into));

        /// <summary>
        /// Set inclusive time interval for <paramref name="request"/> object.
        /// </summary>
        /// <param name="request">Target request for setting filtering interval.</param>
        /// <param name="interval">Time interval (date/time pair) for filtering.</param>
        /// <returns>Fluent interface - returns <paramref name="request"/> object.</returns>
        public static TRequest SetTimeInterval<TRequest>(
            this TRequest request,
            IInclusiveTimeInterval interval)
            where TRequest : IRequestWithTimeInterval<IInclusiveTimeInterval>
        {
            request.SetInterval(interval);
            return request;
        }

        /// <summary>
        /// Gets inclusive open time interval starting from the <paramref name="from"/> date/time point.
        /// </summary>
        /// <param name="from">Starting date/time point for filtering.</param>
        /// <returns>Inclusive open time interval.</returns>
        public static IInclusiveTimeInterval GetInclusiveIntervalFromThat(
            this DateTime from) =>
            new Inclusive(from, null);

        /// <summary>
        /// Gets inclusive open time interval ending at the <paramref name="into"/> date/time point.
        /// </summary>
        /// <param name="into">Ending date/time point for filtering.</param>
        /// <returns>Inclusive open time interval.</returns>
        [UsedImplicitly]
        public static IInclusiveTimeInterval GetInclusiveIntervalTillThat(
            this DateTime into) =>
            new Inclusive(null, into);

        /// <summary>
        /// Set exclusive time interval for <paramref name="request"/> object.
        /// </summary>
        /// <param name="request">Target request for setting filtering interval.</param>
        /// <param name="from">Starting date/time point for filtering.</param>
        /// <param name="into">Ending date/time point for filtering.</param>
        /// <returns>Fluent interface - returns <paramref name="request"/> object.</returns>
        [UsedImplicitly]
        public static TRequest SetExclusiveTimeInterval<TRequest>(
            this TRequest request,
            DateTime from,
            DateTime into)
            where TRequest : IRequestWithTimeInterval<IExclusiveTimeInterval> =>
            request.SetTimeInterval(new Exclusive(from, into));

        /// <summary>
        /// Set exclusive time interval for <paramref name="request"/> object.
        /// </summary>
        /// <param name="request">Target request for setting filtering interval.</param>
        /// <param name="interval">Time interval (date/time pair) for filtering.</param>
        /// <returns>Fluent interface - returns <paramref name="request"/> object.</returns>
        public static TRequest SetTimeInterval<TRequest>(
            this TRequest request,
            IExclusiveTimeInterval interval)
            where TRequest : IRequestWithTimeInterval<IExclusiveTimeInterval>
        {
            request.SetInterval(interval);
            return request;
        }

        /// <summary>
        /// Gets exclusive open time interval starting from the <paramref name="from"/> date/time point.
        /// </summary>
        /// <param name="from">Starting date/time point for filtering.</param>
        /// <returns>Exclusive open time interval.</returns>
        [UsedImplicitly]
        public static IExclusiveTimeInterval GetExclusiveIntervalFromThat(
            this DateTime from) =>
            new Exclusive(from, null);

        /// <summary>
        /// Gets exclusive open time interval ending at the <paramref name="into"/> date/time point.
        /// </summary>
        /// <param name="into">Ending date/time point for filtering.</param>
        /// <returns>Exclusive open time interval.</returns>
        [UsedImplicitly]
        public static IExclusiveTimeInterval GetExclusiveIntervalTillThat(
            this DateTime into) =>
            new Exclusive(null, into);

        internal static IInclusiveTimeInterval GetInclusive(
            DateTime? from,
            DateTime? into) =>
            new Inclusive(from, into);

        internal static IExclusiveTimeInterval GetExclusive(
            DateTime? from,
            DateTime? into) =>
            new Exclusive(from, into);

        internal static IInclusiveTimeInterval InclusiveEmpty { get; } = new Inclusive();

        internal static IExclusiveTimeInterval ExclusiveEmpty { get; } = new Exclusive();
    }
}
