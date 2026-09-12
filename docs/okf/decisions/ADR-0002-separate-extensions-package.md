---
type: Architecture Decision
title: ADR-0002 Ship the helper extensions as a separate, independently versioned package
description: Dependency-injection, resilience and pagination helpers live in Alpaca.Markets.Extensions rather than in the core SDK, and carry their own version number.
status: accepted
date: unknown
deciders: []
supersedes: []
affects: [alpaca_trade_api_csharp.sdk, alpaca_trade_api_csharp.extensions]
allium: []
evidence: ["Alpaca.Markets.Extensions/Alpaca.Markets.Extensions.csproj", "Alpaca.Markets/Alpaca.Markets.csproj", "Alpaca.Markets.Extensions/README.md", ".github/workflows/release.yml"]
tags: [alpacahq, adr, generated]
timestamp: 2026-09-12T00:00:00+00:00
generated_by: claude-opus-5 / layered-docs 2026-09
source_commit: 91bb999551c7be3a998ad66b145fdba8f9a6c6dc
source_branch: docs/layered-2026-09
generated_at: 2026-09-12T00:00:00+00:00
confidence: high
review_status: draft-needs-review
---

# ADR-0002 Ship the helper extensions as a separate, independently versioned package

## Context

Convenience helpers for the SDK — dependency-injection registration, typed `HttpClient` and Polly
wiring, async-enumerable pagination and streaming subscription helpers — pull in the
`Microsoft.Extensions.*` and `System.Linq.Async` dependency families. Folding them into the core
package would impose those dependencies on every consumer, including ones on the older target
frameworks.

## Decision

The helpers are built and published as a second package, `Alpaca.Markets.Extensions`, which takes a
`ProjectReference` on the core project and is described as containing helper extension methods for
the SDK (`Alpaca.Markets.Extensions/Alpaca.Markets.Extensions.csproj`,
`Alpaca.Markets.Extensions/README.md`). The two packages version independently: at this commit the
core is `8.0.0-beta6` and the extensions are `8.0.0-beta3`.

## Consequences

- The core package's dependency list stays narrow — Newtonsoft.Json, Polly and MessagePack — while
  `Microsoft.Extensions.Http`, `Microsoft.Extensions.Http.Polly` and `System.Linq.Async` are
  confined to the extensions package.
- Release automation carries two independent paths, keyed on tag prefix, with separate build
  artifacts and separate GitHub Releases (`.github/workflows/release.yml`); see ADR-0003.
- Release notes are maintained per package in each project file's `PackageReleaseNotes` element,
  and the two sets describe different changes at this commit.
- Each package has its own test project, and the extensions test project references both projects
  (`Alpaca.Markets.Extensions.Tests/Alpaca.Markets.Extensions.Tests.csproj`).
- Consumers who want the helpers must take a version-compatibility decision across two packages;
  no evidence in the repository describes how that compatibility is communicated. Confidence on
  that point: low.
