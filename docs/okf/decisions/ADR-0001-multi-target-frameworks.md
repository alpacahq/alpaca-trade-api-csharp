---
type: Architecture Decision
title: ADR-0001 Multi-target the packages across .NET Standard, .NET Framework and modern .NET
description: Both NuGet packages build for netstandard2.0, netstandard2.1, net462, net8.0 and net10.0 rather than a single modern target.
status: accepted
date: unknown
deciders: []
supersedes: []
affects: [alpaca_trade_api_csharp.sdk, alpaca_trade_api_csharp.extensions, alpaca_trade_api_csharp.portable_helpers]
allium: []
evidence: ["Alpaca.Markets/Alpaca.Markets.csproj", "Alpaca.Markets.Extensions/Alpaca.Markets.Extensions.csproj", "README.md", "global.json", "Portable.Helpers/Portable.Helpers.projitems"]
tags: [alpacahq, adr, generated]
timestamp: 2026-09-12T00:00:00+00:00
generated_by: claude-opus-5 / layered-docs 2026-09
source_commit: 91bb999551c7be3a998ad66b145fdba8f9a6c6dc
source_branch: docs/layered-2026-09
generated_at: 2026-09-12T00:00:00+00:00
confidence: high
review_status: draft-needs-review
---

# ADR-0001 Multi-target the packages across .NET Standard, .NET Framework and modern .NET

## Context

The SDK is consumed by third-party .NET applications whose runtime is outside this repository's
control, ranging from legacy .NET Framework desktop and service code to current .NET releases. The
repository itself builds with a pinned .NET 10 SDK (`global.json` pins 10.0.400 with `rollForward`
disabled), so the build toolchain and the consumer runtime are deliberately decoupled.

## Decision

Both shipped projects declare the target framework set `netstandard2.0`, `netstandard2.1`,
`net462`, `net8.0` and `net10.0` (`Alpaca.Markets/Alpaca.Markets.csproj`,
`Alpaca.Markets.Extensions/Alpaca.Markets.Extensions.csproj`). `README.md` states the consequence
explicitly: consumers do not need the .NET 10 SDK unless they are building the repository itself.

## Consequences

- Framework-conditional dependency groups are required. The newer language and BCL features are
  back-filled only where the target lacks them: `Portable.System.DateTimeOnly`,
  `System.Threading.Channels` and `System.IO.Pipelines` are referenced under
  `IsTargetFrameworkCompatible` conditions, and `System.Net.Http.WinHttpHandler` plus
  `Microsoft.NETFramework.ReferenceAssemblies` only when the target starts with `net4`
  (`Alpaca.Markets/Alpaca.Markets.csproj`).
- A shared source project, `Portable.Helpers`, exists solely to supply `Index`, `Range`,
  `CallerArgumentExpression` and nullable helpers to the targets that lack them, and is imported
  into both packages (`Portable.Helpers/Portable.Helpers.projitems`).
- The extensions package must pin three different versions of the `Microsoft.Extensions.Http`
  family, selected by target framework
  (`Alpaca.Markets.Extensions/Alpaca.Markets.Extensions.csproj`).
- Trimming support is enabled only on targets compatible with net8.0
  (`EnableTrimAnalyzer`, `IsTrimmable`).
- CI installs both the 8.0.x and 10.0.x SDK bands to build the matrix
  (`.github/workflows/release.yml`).

## Alternatives considered

No alternatives are evidenced in the cited files.
