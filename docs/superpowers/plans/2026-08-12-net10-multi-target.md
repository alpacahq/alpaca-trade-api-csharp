# Add net10.0 Multi-Targeting Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add first-class `net10.0` library assets while keeping `net8.0` and down-level TFMs.

**Architecture:** Extend `TargetFrameworks` with `net10.0`, align shipping System/Extensions packages to 10.0.x, broaden modern-TFM MSBuild conditions, move tests/examples to net10.0, refresh locks + PackageValidation, update CI runtimes and docs.

**Tech Stack:** .NET SDK 10.0.302, multi-targeted class libraries, xUnit tests, GitHub Actions `setup-dotnet`.

**Spec:** `docs/superpowers/specs/2026-08-12-net10-multi-target-design.md`

## Global Constraints

- Keep TFMs: `netstandard2.0;netstandard2.1;net462;net8.0;net10.0` (do not drop net8)
- Shipping deps → 10.0.x line (prefer 10.0.11 to match tests)
- Additive change only (no major version bump for dropping TFMs)
- Treat warnings as errors; restore locked mode for library packages

---

### Task 1: Library TFMs, conditions, PackageReferences

**Files:**
- `Alpaca.Markets/Alpaca.Markets.csproj`
- `Alpaca.Markets.Extensions/Alpaca.Markets.Extensions.csproj`

- [x] Add `net10.0` to `TargetFrameworks`
- [x] Broaden trim + DateTimeOnly conditions to net8 **and** net10
- [x] Bump System.*/Microsoft.Extensions.* to 10.0.11 (WinHttpHandler 10.0.11; Portable.System.DateTimeOnly latest compatible)
- [x] Regenerate lock files with unlocked restore then re-enable locked mode

### Task 2: Tests and UsageExamples → net10.0

**Files:**
- `Alpaca.Markets.Tests/Alpaca.Markets.Tests.csproj`
- `Alpaca.Markets.Extensions.Tests/Alpaca.Markets.Extensions.Tests.csproj`
- `UsageExamples/UsageExamples.csproj`

- [x] Tests: `TargetFramework` → `net10.0`
- [x] Examples: `net4.8;net10.0`; bump System.* to 10.0.11

### Task 3: CI + docs

**Files:**
- `.github/workflows/release.yml` (+ other workflows that build/test if needed)
- `README.md`, `CLAUDE.md`
- Spec status → approved/implemented

- [x] Install 8.0.x and 10.0.x runtimes alongside global.json SDK
- [x] Document .NET 10 in supported targets

### Task 4: Validate

- [x] `dotnet build` (0 errors/warnings)
- [x] `dotnet test` passes
- [x] PackageValidation / generate suppressions if needed
- [x] Confirm nupkg contains `lib/net10.0/` and still `lib/net8.0/`
