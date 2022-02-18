namespace System;

internal static class NullableHelper
{
    [UsedImplicitly]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T EnsureNotNull<T>(
        this T value,
        [CallerArgumentExpression("value")]String name = "")
        where T : class => value ?? throw new ArgumentNullException(name);

    [UsedImplicitly]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void EnsurePropertyNotNull<T>(
        this T value,
        [CallerArgumentExpression("value")] String name = "")
        where T : class
    {
        if (value is null)
        {
            throw new InvalidOperationException($"The value of '{name}' property shouldn't be null.");
        }
    }
}
