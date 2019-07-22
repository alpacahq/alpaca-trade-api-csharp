using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;

namespace Alpaca.Markets
{
    internal sealed class QueryBuilder
    {
        private readonly IDictionary<String, String> _queryParameters =
            new Dictionary<String, String>();

        public static implicit operator String(QueryBuilder builder) => builder.ToString();

        public QueryBuilder AddParameter(
            String name,
            String value)
        {
            _queryParameters.Add(name, value);
            return this;
        }

        public QueryBuilder AddParameter<TValue>(
            String name,
            TValue? value)
            where TValue : struct =>
            addParameter(name, value, EnumExtensions.ToEnumString);

        public QueryBuilder AddParameter(
            String name,
            DateTime? value,
            String format) =>
            addParameter(name, value,
                time =>
                {
                    if (time.Kind == DateTimeKind.Unspecified)
                    {
                        time = DateTime.SpecifyKind(time, DateTimeKind.Utc);
                    }

                    return time.ToString(format, CultureInfo.InvariantCulture);
                });

        public QueryBuilder AddParameter(
            String name,
            Int64? value) =>
            addParameter(name, value,
                time => time.ToString("D", CultureInfo.InvariantCulture));

        public override String ToString()
        {
            using (var content = new FormUrlEncodedContent(_queryParameters))
            {
                return content.ReadAsStringAsync().Result;
            }
        }

        private QueryBuilder addParameter<TValue>(
            String name,
            TValue? value,
            Func<TValue, String> converter)
            where TValue : struct =>
            value.HasValue ? AddParameter(name, converter(value.Value)) : this;
    }
}
