# Design: .NET 10–only targeting

**Date:** 2026-08-13  
**Status:** Approved  
**Branch:** `feature/net10-multi-target` (reworked)

## Goal

Rework the current net10 multi-target work into a single-TFM .NET 10 SDK. Drop `net8.0`, `netstandard2.0`, `netstandard2.1`, and `net462`. Remove framework `#if` dual paths and down-level shims. Keep package versioning on the **8.0.0-beta** line.

Supersedes the “keep net8” parts of `docs/superpowers/specs/2026-08-12-net10-multi-target-design.md`.

## Non-goals

- Preserving NuGet assets for net8 / netstandard / net462
- A major version bump for this support drop
- Broker API work
- Public API shape changes unrelated to TFM cleanup

## Target state

| Layer | After |
|-------|--------|
| Library TFMs | `net10.0` only (`Alpaca.Markets`, `Alpaca.Markets.Extensions`) |
| Tests | `net10.0` |
| UsageExamples | `net10.0` only (drop `net4.8`) |
| Package versions | Stay on `8.0.0-beta*` |
| Shipping deps | `System.*` / `Microsoft.Extensions.*` **10.0.x** on the single TFM |
| CI | SDK from `global.json` + .NET 10 runtime only (no 8.0.x install) |
| Docs | Supported target: .NET 10 |

## Design decisions

### 1. Single TFM

Both libraries use `<TargetFramework>net10.0</TargetFramework>` (not multi-target). NuGet ships only `lib/net10.0/`. Consumers must use .NET 10 (or a compatible higher TFM when one exists).

### 2. Delete framework conditionals

In library and Extensions code, remove `#if NETFRAMEWORK` / `NETSTANDARD*` / `NET6_0_OR_GREATER` / `NET8_0_OR_GREATER` / `NET10_0_OR_GREATER` dual implementations. Keep the modern (.NET 10) branch only.

Examples (non-exhaustive):

- `CancelAsync()` instead of `Cancel()`
- Generic `Enum.GetValues<T>()` / `Enum.GetName(value)`
- Net-modern HTTP / WebSocket / trim attribute paths without `#else`

### 3. Portable.Helpers

Remove polyfills that exist only for down-level TFMs (`Index`, `Range`, KeyValuePair extensions, `CallerArgumentExpressionAttribute` polyfill). Keep only helpers that remain useful on net10 without `#if` (e.g. `EnsureNotNull` / `ValidatedNotNullAttribute`), or move those into the main projects and delete the shared project if nothing shared remains.

### 4. Package reference simplification

Drop down-level-only packages and TFM-conditioned ItemGroups:

- `Portable.System.DateTimeOnly`
- `System.Net.Http.WinHttpHandler` (net4)
- `Microsoft.NETFramework.ReferenceAssemblies`
- `IsExternalInit` if no longer required on net10
- Extensions “net8-only” and “not net8” package splits — one set of 10.0.x references

Regenerate `packages.lock.json` under locked restore.

### 5. Versioning & compatibility tooling

- Keep `Version` / `AssemblyVersion` / `FileVersion` on the 8.0.0-beta line
- Release notes: first-class support is .NET 10 only; older TFMs removed
- PackageValidation / `CompatibilitySuppressions.xml` / PublicAPI: update for single `net10.0` asset (generate suppressions only for intentional diffs)

### 6. Tests, examples, CI, docs

- Test projects: `net10.0` only
- UsageExamples: `net10.0` only; remove net4-specific PackageReferences and conditions
- `release.yml`: remove explicit `8.0.x` runtime install
- README + CLAUDE.md: list .NET 10 as the supported target; build still requires SDK 10 from `global.json`

## Consumer impact

Apps targeting net8 / netstandard / net462 cannot consume the new package assets without retargeting to `net10.0`. This is intentional.

## Success criteria

- [ ] Both packages ship only `lib/net10.0/`
- [ ] No remaining `#if` framework dual paths in library/Extensions production code (except any truly unavoidable analyzer-only cases reviewed case-by-case)
- [ ] Solution builds with 0 warnings/errors under SDK 10.0.302
- [ ] `dotnet test` passes on net10.0
- [ ] Docs and CI match net10-only support
- [ ] Package versions remain on 8.0.0-beta line
