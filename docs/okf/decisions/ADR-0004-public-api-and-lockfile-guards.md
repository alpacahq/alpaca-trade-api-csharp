---
type: Architecture Decision
title: ADR-0004 Guard the public API surface with package validation, API analyzers and locked restores
description: Breaking changes and dependency drift are caught at build time through package validation against a baseline, tracked public API files, warnings-as-errors and locked NuGet restore.
status: accepted
date: unknown
deciders: []
supersedes: []
affects: [alpaca_trade_api_csharp.sdk, alpaca_trade_api_csharp.extensions, alpaca_trade_api_csharp.ci]
allium: []
evidence: ["Alpaca.Markets/Alpaca.Markets.csproj", "Alpaca.Markets.Extensions/Alpaca.Markets.Extensions.csproj", "Alpaca.Markets/PublicAPI.Shipped.txt", "Alpaca.Markets/CompatibilitySuppressions.xml", ".github/dependabot.yml", ".github/workflows/lockfiles.yml", ".github/workflows/codeql-analysis.yml"]
tags: [alpacahq, adr, generated]
timestamp: 2026-09-12T00:00:00+00:00
generated_by: claude-opus-5 / layered-docs 2026-09
source_commit: 91bb999551c7be3a998ad66b145fdba8f9a6c6dc
source_branch: docs/layered-2026-09
generated_at: 2026-09-12T00:00:00+00:00
confidence: high
review_status: draft-needs-review
---

# ADR-0004 Guard the public API surface with package validation, API analyzers and locked restores

## Context

The deliverable is a public library whose compiled surface is a contract with third-party
applications across five target frameworks (ADR-0001). An accidental signature change or a silently
floating transitive dependency reaches consumers as a runtime break, and the repository has no way
to observe those consumers.

## Decision

Correctness of the public surface is enforced by the build rather than by review. Both project
files enable `EnablePackageValidation` with `PackageValidationBaselineVersion` set to 7.2.0,
`EnableStrictModeForCompatibleFrameworksInPackage` and `EnableStrictModeForCompatibleTfms`,
reference `Microsoft.CodeAnalysis.PublicApiAnalyzers` against the checked-in
`PublicAPI.Shipped.txt` and `PublicAPI.Unshipped.txt` files, and set `TreatWarningsAsErrors` with
`AnalysisMode` `AllEnabledByDefault`, `EnableNETAnalyzers`, `WarningLevel` 5 and `Nullable` enabled.
Dependencies are restored in locked mode (`RestorePackagesWithLockFile`, `RestoreLockedMode`), and
`.github/workflows/lockfiles.yml` regenerates and commits the lock files on Dependabot pull
requests so the lock stays the source of truth.

## Consequences

- Deliberate breaking changes must be recorded rather than merged silently: the core project's
  release notes carry an explicit BREAKING entry for a removed account-configuration property, and
  `Alpaca.Markets/CompatibilitySuppressions.xml` exists to record accepted validation exceptions.
- Assemblies are strong-name signed from `Alpaca.Markets.snk` with `SignAssembly`, and the test
  project is granted access through an `InternalsVisibleTo` entry carrying the public key.
- Dependabot updates cannot land without a matching lock-file refresh, which is why the lock-file
  workflow is restricted to the Dependabot actor (`github.actor == 'dependabot[bot]'`) and commits
  back to the pull request (`.github/workflows/lockfiles.yml`). The updates themselves are raised by
  `.github/dependabot.yml`, which tracks the `nuget` (daily), `dotnet-sdk` (weekly) and
  `github-actions` (weekly) ecosystems.
- Static analysis is layered on top in CI: CodeQL runs on push, on pull requests to `develop` and
  weekly (`.github/workflows/codeql-analysis.yml`), and Codacy grades the repository
  (`README.md` badges).
