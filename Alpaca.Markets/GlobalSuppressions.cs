﻿// This file is used by Code Analysis to maintain SuppressMessage attributes that are applied to this project.

using System;
using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Globalization",    "CA1303:Do not pass literals as localized parameters",
    Justification = "We do not plan to support localized exception messages in this SDK.")]
[assembly: SuppressMessage(
    "Design", "CA1003:Use generic event handler instances",
    Justification = "This event hanlding approach selected intentionally.")]

[assembly:CLSCompliant(false)]