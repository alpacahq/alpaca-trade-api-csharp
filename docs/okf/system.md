---
type: System
title: .NET SDK for Alpaca Markets API
description: C#/.NET client SDK, shipped as the Alpaca.Markets and Alpaca.Markets.Extensions NuGet packages, giving .NET applications typed REST and WebSocket access to the Alpaca trading, market data, crypto, options and news APIs.
resource: likec4://alpacahq/alpaca_trade_api_csharp
tags: [alpacahq, sdk, dotnet, generated]
status: active
owners: []
timestamp: 2026-09-12T00:00:00+00:00
generated_by: claude-opus-5 / layered-docs 2026-09
source_commit: 91bb999551c7be3a998ad66b145fdba8f9a6c6dc
source_branch: docs/layered-2026-09
generated_at: 2026-09-12T00:00:00+00:00
confidence: medium
review_status: draft-needs-review
links:
  repository: https://github.com/alpacahq/alpaca-trade-api-csharp
  architecture: ../architecture/alpaca_trade_api_csharp.c4
---

# .NET SDK for Alpaca Markets API

## Purpose

This repository is the official C#/.NET client SDK for the Alpaca Markets API. It is not a
deployed service: it builds two NuGet packages, `Alpaca.Markets` and `Alpaca.Markets.Extensions`,
that .NET application developers add to their own projects to place orders, query account state
and consume market data (`README.md`, `Alpaca.Markets/README.md`,
`Alpaca.Markets/Alpaca.Markets.csproj`).

The SDK hides the HTTP and WebSocket protocol behind typed clients and a pair of environment
objects — `Environments.Live` and `Environments.Paper` — so the same application code can be
pointed at live trading or at the paper-trading simulation by swapping one factory call
(`Alpaca.Markets/Environment/Environments.cs`, `README.md`). The repository is actively
maintained: the current package version is an 8.0.0 pre-release and dependency updates land
continuously (`Alpaca.Markets/Alpaca.Markets.csproj`, `.github/dependabot.yml`,
`.github/workflows/lockfiles.yml`).

## Capabilities

- Typed REST access to the Trading API — orders, positions, account, watchlists, clock and
  calendar — evidence: `Alpaca.Markets/AlpacaTradingClient.cs`,
  `Alpaca.Markets/AlpacaTradingClient.Orders.cs`, `Alpaca.Markets/AlpacaTradingClient.WatchList.cs`,
  `Alpaca.Markets/AlpacaTradingClient.General.cs` (account, positions, clock, calendar)
- Historical market data for equities, crypto and options — evidence:
  `Alpaca.Markets/AlpacaDataClient.cs`, `Alpaca.Markets/AlpacaCryptoDataClient.cs`,
  `Alpaca.Markets/AlpacaOptionsDataClient.cs`, `Alpaca.Markets/DataHistoricalClientBase.cs`
- Real-time WebSocket streaming for account updates, market data, crypto, options and news —
  evidence: `Alpaca.Markets/AlpacaStreamingClient.cs`,
  `Alpaca.Markets/AlpacaDataStreamingClient.cs`, `Alpaca.Markets/AlpacaCryptoStreamingClient.cs`,
  `Alpaca.Markets/AlpacaOptionsStreamingClient.cs`, `Alpaca.Markets/AlpacaNewsStreamingClient.cs`
- Live and paper environment selection, including the free IEX stream versus the paid SIP stream —
  evidence: `Alpaca.Markets/Environment/LiveEnvironment.cs`,
  `Alpaca.Markets/Environment/PaperEnvironment.cs`, `README.md`
- Multiple authentication styles (key/secret, basic, OAuth) — evidence:
  `Alpaca.Markets/Authentication/SecretKey.cs`, `Alpaca.Markets/Authentication/BasicKey.cs`,
  `Alpaca.Markets/Authentication/OAuthKey.cs`
- Client-side rate-limit handling and request throttling — evidence:
  `Alpaca.Markets/RateLimit/RateLimitHandler.cs`, `Alpaca.Markets/Throttling/ThrottleParameters.cs`
- Optional dependency-injection, resilience and pagination helpers — evidence:
  `Alpaca.Markets.Extensions/Alpaca.Markets.Extensions.csproj`,
  `Alpaca.Markets.Extensions/README.md`

## Interfaces

**Inbound:** none at runtime. The public surface is the .NET API of the two packages, tracked
explicitly in `Alpaca.Markets/PublicAPI.Shipped.txt` and `Alpaca.Markets/PublicAPI.Unshipped.txt`
and consumed through NuGet (`README.md`).

**Outbound:** HTTPS to the Alpaca trading and market-data REST endpoints and WSS to the account,
market-data, crypto, options and news streams, resolved per environment in
`Alpaca.Markets/Environment/LiveEnvironment.cs` and
`Alpaca.Markets/Environment/PaperEnvironment.cs`. Release automation pushes packages to NuGet.org
and publishes documentation to GitHub Pages (`.github/workflows/release.yml`,
`.github/workflows/documentation.yml`).

## Dependencies

- Alpaca Markets platform APIs — the system the SDK is a client for — evidence
  `Alpaca.Markets/Environment/LiveEnvironment.cs`
- Newtonsoft.Json — JSON serialisation of requests and responses — evidence
  `Alpaca.Markets/Alpaca.Markets.csproj`
- Polly — retry and resilience policies — evidence `Alpaca.Markets/Alpaca.Markets.csproj`
- MessagePack — binary decoding of the market-data streams — evidence
  `Alpaca.Markets/Alpaca.Markets.csproj`
- Microsoft.Extensions.Http and Microsoft.Extensions.Http.Polly — DI and typed HttpClient wiring in
  the extensions package — evidence `Alpaca.Markets.Extensions/Alpaca.Markets.Extensions.csproj`
- Portable.System.DateTimeOnly, System.Threading.Channels, System.IO.Pipelines and
  System.Net.Http.WinHttpHandler — back-fills used only on the older target frameworks — evidence
  `Alpaca.Markets/Alpaca.Markets.csproj`
- NuGet.org — publication target and restore source — evidence `.github/workflows/release.yml`,
  `Alpaca.Markets/packages.lock.json`
- GitHub Actions, Releases and Pages — evidence `.github/workflows/release.yml`,
  `.github/workflows/documentation.yml`
- Codacy — code grade and coverage reporting — evidence `.github/workflows/release.yml`

## Data & storage

The SDK owns no persistent store. It holds only in-flight request and response models
(`Alpaca.Markets/Messages/`, `Alpaca.Markets/Parameters/`, `Alpaca.Markets/Orders/`) and the
caller's credentials in memory (`Alpaca.Markets/Authentication/SecurityKey.cs`). Credentials are
supplied by the consuming application; the repository contains no keys. Dependency versions are
pinned in per-project `packages.lock.json` files with `RestoreLockedMode` enabled
(`Alpaca.Markets/Alpaca.Markets.csproj`).

## Operations

There is no deployment: no Dockerfile, compose file, Kubernetes manifest, Terraform or serverless
configuration exists anywhere in the tree (verified against the full `git ls-files` listing at
`91bb999`). Build and release are entirely GitHub Actions:

- `.github/workflows/release.yml` — builds, runs the test suites with coverage on every push and
  pull request, and on tags beginning with `sdk` or `ext` pushes the matching package to NuGet.org
  and drafts a GitHub Release.
- `.github/workflows/documentation.yml` — manually dispatched; builds the DocFX site from
  `Documentation/docfx.json` and deploys it to GitHub Pages.
- `.github/workflows/codeql-analysis.yml` — CodeQL scan on push, on pull requests to `develop` and
  weekly.
- `.github/dependabot.yml` — raises the dependency update pull requests that drive the continuous
  updates: `nuget` daily, `dotnet-sdk` weekly and `github-actions` weekly, each assigned to a single
  named GitHub handle.
- `.github/workflows/lockfiles.yml` — refreshes NuGet lock files on Dependabot pull requests
  (guarded by `github.actor == 'dependabot[bot]'`).
- `global.json` pins the .NET SDK to 10.0.400 with `rollForward` disabled; local docs preview is
  `Documentation/serve.sh` or `Documentation/serve.cmd` (`README.md`).

## Behaviour (Allium)

none — no Allium specification exists in this repository at `91bb999` (no `.allium` file appears in
`git ls-files`), and no accepted spec was supplied for this generation.

## Decisions

- [ADR-0001 Multi-target the packages across .NET Standard, .NET Framework and modern .NET](decisions/ADR-0001-multi-target-frameworks.md)
- [ADR-0002 Ship the helper extensions as a separate, independently versioned package](decisions/ADR-0002-separate-extensions-package.md)
- [ADR-0003 Drive NuGet publication from tag prefixes in GitHub Actions](decisions/ADR-0003-tag-driven-nuget-release.md)
- [ADR-0004 Guard the public API surface with package validation, API analyzers and locked restores](decisions/ADR-0004-public-api-and-lockfile-guards.md)
- [ADR-0005 Model Live and Paper as environment objects and route data-feed tiers through them](decisions/ADR-0005-live-and-paper-environments.md)

## Evidence

- `README.md`, `Alpaca.Markets/README.md`, `Alpaca.Markets.Extensions/README.md`,
  `UsageExamples/README.md`
- `Alpaca.Markets.sln`, `global.json`, `Configuration.xml`
- `Alpaca.Markets/Alpaca.Markets.csproj`,
  `Alpaca.Markets.Extensions/Alpaca.Markets.Extensions.csproj`,
  `UsageExamples/UsageExamples.csproj`, `Alpaca.Markets.Tests/Alpaca.Markets.Tests.csproj`,
  `Alpaca.Markets.Extensions.Tests/Alpaca.Markets.Extensions.Tests.csproj`
- `Alpaca.Markets/Environment/Environments.cs`, `Alpaca.Markets/Environment/LiveEnvironment.cs`,
  `Alpaca.Markets/Environment/PaperEnvironment.cs`
- `Alpaca.Markets/PublicAPI.Shipped.txt`, `Alpaca.Markets/CompatibilitySuppressions.xml`
- `Portable.Helpers/Portable.Helpers.projitems`, `Documentation/docfx.json`
- `Alpaca.Markets/AlpacaTradingClient.cs`, `Alpaca.Markets/AlpacaTradingClient.General.cs`,
  `Alpaca.Markets/AlpacaTradingClient.Orders.cs`, `Alpaca.Markets/AlpacaTradingClient.WatchList.cs`
- `.github/workflows/release.yml`, `.github/workflows/documentation.yml`,
  `.github/workflows/codeql-analysis.yml`, `.github/workflows/lockfiles.yml`,
  `.github/dependabot.yml`
- `SECURITY.md`, `CONTRIBUTING.md`, `CRYPTO_PERPETUALS.md`, `CONTRIBUTORS.md`

## Open questions

> Unverified: ownership — there is no CODEOWNERS file and no maintaining *team* is named anywhere
> in the repository, so `owners` is left empty. Individual de-facto roles are evidenced:
> `CONTRIBUTORS.md` carries all-contributors "Maintenance 🚧" and "Reviewed Pull Requests 👀" badges
> for two GitHub handles, and `.github/dependabot.yml` assigns every dependency-update stream to one
> named handle. The package metadata names Alpaca Securities LLC as the authoring company
> (`Alpaca.Markets/Alpaca.Markets.csproj`), which is a copyright statement rather than a maintaining
> team. Whether any of these people is the accountable owner is not stated. Confidence: low.

> Unverified: no record of the alternatives weighed for the target-framework set is citable from
> this repository, so ADR-0001 rests on the manifest and README evidence alone. Confidence: low.

> Unverified: the `ext_*` external ids used by the architecture model (`ext_alpaca_api`,
> `ext_nuget`, `ext_codacy`) are evidenced from this repository but have not yet been reconciled
> with the org-level external dictionary, and `ext_github` is used here for Actions CI, Releases,
> Pages and SourceLink. Reconciliation is an org-level action outside this repository.
> Confidence: low.

> Unverified: the support policy in `SECURITY.md` lists 8.x and 7.x as supported, but no branch or
> release policy for maintaining 7.x alongside 8.x is evidenced in the repository. Confidence: low.

> Unverified: whether the crypto perpetual futures enum described in `CRYPTO_PERPETUALS.md` is
> intended to remain in the shipped public API is not evidenced. Confidence: low.
