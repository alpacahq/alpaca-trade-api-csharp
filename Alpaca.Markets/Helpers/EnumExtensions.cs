using System.Reflection;

namespace Alpaca.Markets;

internal static class EnumExtensions
{
    private static class NamesHelper<
#if NET6_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)]
#endif
        T> where T : struct, Enum
    {
        public static readonly IReadOnlyDictionary<String, T> ValuesByNames =
#if NET8_0_OR_GREATER
            Enum.GetValues<T>()
#else
            Enum.GetValues(typeof(T)).OfType<T>()
#endif
                .ToDictionary(getJsonName, value => value);

        private static string getJsonName(
            T value) =>
            typeof(T).GetField(
#if NET8_0_OR_GREATER
                    Enum.GetName(value)
#else
                    Enum.GetName(typeof(T), value)
#endif
                    ?? value.ToString())?
                .GetCustomAttribute<EnumMemberAttribute>()?.Value ?? value.ToString();
    }

    private static readonly Char[] _doubleQuotes = [ '"' ];

    public static String ToEnumString<T>(
        this T enumValue)
        where T : struct, Enum =>
        JsonConvert.SerializeObject(enumValue).Trim(_doubleQuotes);

    public static T FromEnumString<
#if NET6_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)]
#endif
        T>(
        this T fallbackEnumValue,
        JsonReader reader)
        where T : struct, Enum =>
        reader.TokenType == JsonToken.String
            ? NamesHelper<T>.ValuesByNames.GetValueOrDefault(
                reader.Value as String ?? String.Empty, fallbackEnumValue)
            : fallbackEnumValue;
}
