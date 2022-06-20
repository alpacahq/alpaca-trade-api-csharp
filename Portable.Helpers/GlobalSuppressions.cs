// This file is used by Code Analysis to maintain SuppressMessage attributes that are applied to this project.

global using JetBrains.Annotations;
global using System.Diagnostics.CodeAnalysis;
global using System.Runtime.CompilerServices;
global using System.Net.Sockets;

[assembly: CLSCompliant(true)]

[assembly: SuppressMessage("Globalization",
    "CA1303: Do not pass literals as localized parameters",
    Justification = "We do not plan to support localized exception messages in this SDK.")]
[assembly: SuppressMessage("Design",
    "CA1003: Use generic event handler instances",
    Justification = "This SDK uses Action<T> based events historically for performance reasons.")]
