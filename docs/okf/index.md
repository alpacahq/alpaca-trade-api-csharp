# .NET SDK for Alpaca Markets API — knowledge bundle (OKF)

Generated draft (layered-docs 2026-09) — review required. Concept id = path without `.md`.

- [system](system.md) — System: the C#/.NET client SDK for the Alpaca Markets API, shipped as the Alpaca.Markets and Alpaca.Markets.Extensions NuGet packages.
- [decisions/ADR-0001-multi-target-frameworks](decisions/ADR-0001-multi-target-frameworks.md) — Architecture Decision: both packages target .NET Standard 2.0/2.1, .NET Framework 4.6.2, .NET 8.0 and .NET 10.0.
- [decisions/ADR-0002-separate-extensions-package](decisions/ADR-0002-separate-extensions-package.md) — Architecture Decision: the helper extensions ship as a second, independently versioned package.
- [decisions/ADR-0003-tag-driven-nuget-release](decisions/ADR-0003-tag-driven-nuget-release.md) — Architecture Decision: NuGet publication is gated on repository identity and an sdk/ext tag prefix.
- [decisions/ADR-0004-public-api-and-lockfile-guards](decisions/ADR-0004-public-api-and-lockfile-guards.md) — Architecture Decision: package validation, public API analyzers and locked restores guard the shipped surface.
- [decisions/ADR-0005-live-and-paper-environments](decisions/ADR-0005-live-and-paper-environments.md) — Architecture Decision: Live and Paper are environment objects that also select the market-data feed tier.
