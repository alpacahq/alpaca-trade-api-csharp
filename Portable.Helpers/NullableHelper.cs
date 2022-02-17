namespace System;

internal static class NullableHelper
{
    [UsedImplicitly]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T EnsureNotNull<T>(
        this T value,
        [CallerArgumentExpression("value")]String name = "")
        where T : class => value ?? throw new ArgumentNullException(name);
}
