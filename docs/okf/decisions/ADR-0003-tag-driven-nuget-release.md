---
type: Architecture Decision
title: ADR-0003 Drive NuGet publication from tag prefixes in GitHub Actions
description: Packages are pushed to NuGet.org only from tags beginning with sdk or ext, on the canonical repository, using a short-lived NuGet login token.
status: accepted
date: unknown
deciders: []
supersedes: []
affects: [alpaca_trade_api_csharp.ci, alpaca_trade_api_csharp.sdk, alpaca_trade_api_csharp.extensions, alpaca_trade_api_csharp.tests]
allium: []
evidence: [".github/workflows/release.yml", "Alpaca.Markets/Alpaca.Markets.csproj", "Configuration.xml"]
tags: [alpacahq, adr, generated]
timestamp: 2026-09-12T00:00:00+00:00
generated_by: claude-opus-5 / layered-docs 2026-09
source_commit: 91bb999551c7be3a998ad66b145fdba8f9a6c6dc
source_branch: docs/layered-2026-09
generated_at: 2026-09-12T00:00:00+00:00
confidence: high
review_status: draft-needs-review
---

# ADR-0003 Drive NuGet publication from tag prefixes in GitHub Actions

## Context

Two packages are released from one repository and one build (ADR-0002), and forks must never be
able to publish. The build itself runs on every push and pull request, so the release trigger has
to be narrower than the build trigger.

## Decision

`.github/workflows/release.yml` builds and tests on every push and pull request, then gates both
artifact upload and publication on two conditions together: the repository must be
`alpacahq/alpaca-trade-api-csharp`, and the ref must start with `refs/tags/sdk` for the core
package or `refs/tags/ext` for the extensions package. Publication uses the `NuGet/login` action to
exchange a configured user for a short-lived API key, pushes with `dotnet nuget push` to
`https://api.nuget.org/v3/index.json`, and then creates a draft GitHub Release.

## Consequences

- Two near-identical publish jobs exist, `publish-sdk` and `publish-ext`, each depending on the
  shared `build` job and each requesting `id-token: write` for the NuGet login exchange.
- Packages are produced during the ordinary release build rather than in a dedicated packaging
  step, because both project files set `GeneratePackageOnBuild`; the publish jobs only download the
  uploaded artifacts.
- Symbol packages ship alongside the binaries: both projects set `IncludeSymbols` with the `snupkg`
  format and reference `Microsoft.SourceLink.GitHub`, and the artifact paths collect both `.nupkg`
  and `.snupkg` files.
- Releases are created as drafts, so the final publish step remains a human action.
- Coverage is collected in the same build job via `dotnet dotcover test` against `Configuration.xml`
  and uploaded to Codacy only when the project token is present, so forks build and test but do not
  report.
