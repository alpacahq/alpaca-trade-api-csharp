using System.Globalization;

namespace Alpaca.Markets;

internal sealed class QueryBuilder
{
    private const String ListSeparator = ",";

    private readonly IDictionary<String, String> _queryParameters =
        new Dictionary<String, String>();

    public QueryBuilder AddParameter(
        String name,
        String? value)
    {
        if (value is not null)
        {
            _queryParameters.Add(name, value);
        }
        return this;
    }

    public QueryBuilder AddParameter(
        String name,
        Boolean? value) =>
        addParameter(name, value,
            notNull => notNull.ToString(CultureInfo.InvariantCulture));

    public QueryBuilder AddParameter<TValue>(
        String name,
        TValue? value)
        where TValue : struct, Enum =>
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
        DateOnly? value) =>
        addParameter(name, value,
            date => date.ToString("O", CultureInfo.InvariantCulture));

    public QueryBuilder AddParameter(
        String name,
        IReadOnlyCollection<String> values) =>
        values.Count != 0
            ? AddParameter(name, String.Join(ListSeparator, values))
            : this;

    public QueryBuilder AddParameter<TValue>(
        String name,
        IReadOnlyCollection<TValue> values)
        where TValue : struct, Enum =>
        values.Count != 0
            ? addParameter(name, values, EnumExtensions.ToEnumString)
            : this;

    public QueryBuilder AddParameter(
        String name,
        Int64? value) =>
        addParameter(name, value,
            notNull => notNull.ToString("D", CultureInfo.InvariantCulture));

    public QueryBuilder AddParameter(
        String name,
        Decimal? value) =>
        addParameter(name, value,
            notNull => notNull.ToString("F9", CultureInfo.InvariantCulture));

    public async ValueTask<String> AsStringAsync()
    {
#pragma warning disable 8620
        using var content = new FormUrlEncodedContent(_queryParameters);
#pragma warning restore 8620
        return await content.ReadAsStringAsync().ConfigureAwait(false);
    }

    private QueryBuilder addParameter<TValue>(
        String name,
        TValue? value,
        Func<TValue, String> converter)
        where TValue : struct =>
        value.HasValue ? AddParameter(name, converter(value.Value)) : this;

    private QueryBuilder addParameter<TValue>(
        String name,
        IEnumerable<TValue>? values,
        Func<TValue, String> converter)
        where TValue : struct =>
        values is not null
            ? AddParameter(name, String.Join(ListSeparator, values.Select(converter)))
            : this;
}
