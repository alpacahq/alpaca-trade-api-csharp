---
type: Architecture Decision
title: ADR-0005 Model Live and Paper as environment objects and route data-feed tiers through them
description: Endpoint selection is expressed as two IEnvironment implementations, and the choice of environment also selects the IEX or SIP market-data stream.
status: accepted
date: unknown
deciders: []
supersedes: []
affects: [alpaca_trade_api_csharp.sdk]
allium: []
evidence: ["Alpaca.Markets/Environment/Environments.cs", "Alpaca.Markets/Environment/IEnvironment.cs", "Alpaca.Markets/Environment/LiveEnvironment.cs", "Alpaca.Markets/Environment/PaperEnvironment.cs", "README.md", "UsageExamples/README.md"]
tags: [alpacahq, adr, generated]
timestamp: 2026-09-12T00:00:00+00:00
generated_by: claude-opus-5 / layered-docs 2026-09
source_commit: 91bb999551c7be3a998ad66b145fdba8f9a6c6dc
source_branch: docs/layered-2026-09
generated_at: 2026-09-12T00:00:00+00:00
confidence: medium
review_status: draft-needs-review
---

# ADR-0005 Model Live and Paper as environment objects and route data-feed tiers through them

## Context

The platform exposes seven distinct endpoints — trading REST, data REST, account stream, market
data stream, crypto stream, news stream and options stream — and each has a live and a
paper-trading counterpart. Some of those counterparts differ and some are shared. Exposing raw
URLs to callers would make swapping between simulation and live trading an error-prone,
multi-place edit.

## Decision

Endpoint selection is a first-class abstraction. `IEnvironment` declares the seven endpoint
properties, and `LiveEnvironment` and `PaperEnvironment` supply the concrete values, exposed as the
singletons `Environments.Live` and `Environments.Paper`
(`Alpaca.Markets/Environment/Environments.cs`). Clients are created through factory methods on the
environment, so the whole switch is one expression, as shown in the README quick start.
`PaperEnvironment` inherits the endpoints that genuinely are shared by delegating to
`Environments.Live` for the data REST, crypto stream and news stream.

The same abstraction carries the market-data subscription tier. `README.md` states the rule
directly: use the paper environment's data streaming client for the free IEX stream and the live
environment's for the paid SIP stream, so that the tier follows from the environment rather than
from a separate feed parameter.

## Consequences

- Callers switch between simulation and live trading by changing one factory call, and
  `UsageExamples/README.md` relies on this when it distinguishes the paper-only, regular and power
  variants of the sample strategy.
- Overloading the environment with the data tier couples two concerns: a developer on a funded
  account who wants paper trading against SIP data cannot express that by environment choice alone.
  The repository does not record whether another route exists for that combination. Confidence on
  this consequence: low.
- The crypto, news and options streams are versioned separately in the endpoint paths, so adding a
  stream version is a change to both environment classes.
- `EnvironmentExtensions` concentrates the client factory methods, keeping the environment
  implementations to endpoint data only.
- The endpoints themselves belong to the Alpaca Markets platform, which this repository does not
  control; in the architecture model that dependency is the `sdk -[calls]-> ext_alpaca_api`
  relationship rather than something this decision governs
  (`Alpaca.Markets/Environment/LiveEnvironment.cs`, `Alpaca.Markets/Environment/PaperEnvironment.cs`).
