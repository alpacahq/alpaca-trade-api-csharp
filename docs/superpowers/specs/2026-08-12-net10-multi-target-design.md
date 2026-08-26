# Design: Add .NET 10 multi-targeting (Path A)

**Date:** 2026-08-12  
**Status:** Superseded by `2026-08-13-net10-only-design.md`  
**Branch:** `feature/net10-multi-target`

## Goal

Ship first-class `net10.0` assets in `Alpaca.Markets` and `Alpaca.Markets.Extensions` while keeping existing TFMs (including `net8.0`) for backward compatibility. Dropping `net8.0` is explicitly deferred to a later PR.

## Non-goals

- Removing `net8.0`, `netstandard2.0`, `netstandard2.1`, or `net462`
- Requiring consumers to install .NET 10 to use the package on net8 / netstandard / net462
- Changing public API surface beyond what PackageValidation/PublicAPI already allow

## Current state

| Layer | Today |
|-------|--------|
| Build SDK | `global.json` → `10.0.302` |
| Library TFMs | `netstandard2.0;netstandard2.1;net462;net8.0` |
| Library deps | `System.*` / `Microsoft.Extensions.*` **9.0.8** |
| Tests | `net8.0` (some test-only packages already 10.0.x) |
| CI | Installs SDK from `global.json` only |

## Target state

| Layer | After this work |
|-------|-----------------|
| Library TFMs | `netstandard2.0;netstandard2.1;net462;net8.0;net10.0` |
| Nupkg | Adds `lib/net10.0/*.dll` (+ XML docs) |
| Library deps | Shipping `System.*` / `Microsoft.Extensions.*` → **10.0.x** (aligned with test packages, e.g. 10.0.11) |
| Tests / UsageExamples | Primary modern TFM → `net10.0` (examples keep `net4.8;net10.0`) |
| CI | Explicitly install runtimes needed to build/test multi-TFM (at least 8.0 + 10.0) |
| Docs | README/CLAUDE: supported targets include .NET 10; build still requires SDK 10 |

## Design decisions

### 1. Multi-target, do not replace

Adding `net10.0` is an additive NuGet change. Net8 consumers continue to resolve `lib/net8.0/`. Net10 consumers get `lib/net10.0/`.

### 2. Broaden “modern .NET” MSBuild conditions

Replace narrow `StartsWith('net8')` checks used for trim and for *excluding* `Portable.System.DateTimeOnly` so they apply to `net8.0` **and** `net10.0` (e.g. `net8.0` / `net10.0` via `StartsWith('net8') Or StartsWith('net10')`, or equivalent). Down-level TFMs keep portable shims.

### 3. Dependency line → 10.0.x for libraries

Bump:

- `System.Threading.Channels`
- `System.IO.Pipelines`
- `System.Net.Http.WinHttpHandler` (net4 only)
- `Microsoft.Extensions.Http` / `Microsoft.Extensions.Http.Polly` (Extensions)
- `Portable.System.DateTimeOnly` if a 10.x line exists; otherwise keep current major that still supports down-level TFMs

These packages multi-target; net8 consumers can still restore them. Lock files must be regenerated (`RestoreLockedMode`).

### 4. Tests and examples on net10

- `Alpaca.Markets.Tests` / `Alpaca.Markets.Extensions.Tests` → `net10.0`
- `UsageExamples` → `net4.8;net10.0`
- Keeps CI exercising the new asset; net8 library asset remains covered by PackageValidation / multi-TFM build

### 5. PackageValidation + PublicAPI

- Build with PackageValidation enabled; regenerate `CompatibilitySuppressions.xml` via the project’s documented generate-suppression property if new cross-TFM `CP*` diffs appear for `lib/net10.0`
- Confirm PublicAPI shipped/unshipped still pass per TFM

### 6. Versioning

Additive TFM support → **minor** (or stay on current beta line). No major bump. Dropping net8 later is the breaking/major candidate.

### 7. CI

Update `release.yml` (and other workflows that build/test) so `setup-dotnet` installs:

- SDK from `global.json` (10.0.302)
- Explicit `8.0.x` and `10.0.x` runtimes (or equivalent multi-version install)

Ensures tests and multi-target builds do not silently rely on roll-forward alone.

## Implementation outline

1. Update both library `.csproj` TFMs + conditions + PackageReferences
2. Update test/example projects to `net10.0`
3. Regenerate `packages.lock.json` for both libraries
4. Build Release; fix PackageValidation suppressions / PublicAPI as needed
5. Update CI workflow runtime installs
6. Update README + CLAUDE.md target lists
7. `dotnet build` + `dotnet test` on SDK 10; verify nupkg contains `lib/net10.0/`

## Risks and mitigations

| Risk | Mitigation |
|------|------------|
| PackageValidation noise for new TFM | Generate suppressions only for intentional diffs; review file |
| Dependabot already on 10.x for tests, 9.x for libs | Align libraries to 10.x in this PR intentionally |
| Local machines missing net8 runtime | CI installs both; document SDK 10 for contributors |
| Condition typos excluding DateTimeOnly on net10 | Explicit net8 \|\| net10 conditions; verify net10 build uses BCL `DateTimeOnly` |

## Success criteria

- [ ] Both packages include `lib/net10.0/`
- [ ] Both packages still include `lib/net8.0/` (and existing down-level TFMs)
- [ ] Solution builds with 0 warnings/errors under SDK 10.0.302
- [ ] Unit tests pass on `net10.0`
- [ ] Docs list .NET 10 as a supported target; build instructions remain SDK 10
- [ ] CI installs runtimes required for multi-TFM verification
