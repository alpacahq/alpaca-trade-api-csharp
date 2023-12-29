#if !NET6_0_OR_GREATER

namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Parameter)]
internal sealed class CallerArgumentExpressionAttribute(
    String parameterName) : Attribute
{
    [UsedImplicitly]
    public String ParameterName { get; } = parameterName;
}

#endif
