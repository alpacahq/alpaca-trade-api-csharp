#if !NET6_0_OR_GREATER

namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Parameter)]
internal sealed class CallerArgumentExpressionAttribute : Attribute
{
    public CallerArgumentExpressionAttribute(
        String parameterName) =>
        ParameterName = parameterName;

    [UsedImplicitly]
    public String ParameterName { get; }
}

#endif
