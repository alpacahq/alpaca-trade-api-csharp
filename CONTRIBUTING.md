# Contributing

Thanks for helping improve the Alpaca C#/.NET SDK. By participating, you agree to follow our [Code of Conduct](./CODE_OF_CONDUCT.md).

## Choose the right channel

- **Library / SDK issues:** Bugs, feature requests, or questions specific to this SDK → [GitHub Issues](https://github.com/alpacahq/alpaca-trade-api-csharp/issues/new/choose).
- **Security vulnerabilities:** Do not report them publicly. Follow the private reporting process in our [Security Policy](./SECURITY.md).
- **Alpaca API bugs:** Bugs in the Alpaca API itself, its OpenAPI specs, or its documentation → [alpacahq/alpaca-api](https://github.com/alpacahq/alpaca-api/issues/new/choose).
- **Community:** API feature requests, general questions, and dashboard bugs → [Alpaca Community Forum](https://forum.alpaca.markets/) or [Slack](https://alpaca.markets/slack).
- **Account help:** Questions about your account, funding, or orders → [Support request](https://alpaca.markets/support/request).
- **Outages:** Service status and incident updates → [Status page](https://status.alpaca.markets/).

Please do not include credentials, account details, request or trace IDs, or other sensitive information in an issue, discussion, pull request, test, or example.

## Contribution policy

### Contributions we welcome

Focused contributions are welcome when they improve this SDK, including:

- bug fixes and compatibility improvements;
- support for documented Alpaca API capabilities;
- focused tests and reliability improvements;
- documentation and examples; and
- maintainability improvements that preserve public behavior.

### Discuss substantial changes first

Open an issue before investing in a substantial SDK design, new dependency, broad refactor, or third-party integration. Early discussion helps confirm that the proposal fits the SDK's scope and avoids duplicated or unusable work.

Bugs in the Alpaca API itself, its OpenAPI specs, or its documentation must be fixed upstream, not patched in this SDK. Report them in [alpacahq/alpaca-api](https://github.com/alpacahq/alpaca-api/issues/new/choose). API feature requests, general questions, and dashboard bugs belong on the [Alpaca Community Forum](https://forum.alpaca.markets/) or [Slack](https://alpaca.markets/slack). Contributions that add support for already documented API capabilities are welcome after the SDK design is agreed with maintainers.

### Out of scope

This repository does not accept:

- trading strategies, algorithm design, or investment advice;
- Broker API features;
- bugs in the Alpaca API, its OpenAPI specs, or docs.alpaca.markets;
- API feature requests, dashboard bugs, or outage reports;
- questions that are not about this SDK, including questions about your account, funding, or orders;
- credentials, private account data, request or trace IDs, or other secrets;
- purely cosmetic changes without a material documentation, stability, functionality, or testability improvement;
- referral links or promotional content; or
- unapproved integrations with third-party products or services.

Contributions that add or promote third-party products or services, including provider-specific clients, API calls, hosted endpoints, referral links, or promotional content, require prior written approval from Alpaca before implementation. Open an issue describing the technical scope; repository maintainers will coordinate the necessary internal review and confirm whether the proposal may proceed. Contributors do not need to identify or contact internal approvers.

Using an ordinary open-source dependency or improving generic interoperability does not by itself make a contribution a provider-specific integration. Maintainers will explain when a proposal falls outside this policy. A pull request that proceeds without the required approval may be closed as out of scope, so open an issue first if you are unsure whether a proposal qualifies.

## License and contribution terms

The project is licensed under the [Apache License 2.0](./LICENSE). Subject to its full terms, the license permits use, reproduction, modification, and distribution of the software. It does not grant permission to use Alpaca trade names, trademarks, service marks, or product names except as described in the license.

Unless you explicitly state otherwise, a contribution intentionally submitted for inclusion in this project is provided under Apache-2.0 without additional terms, as described in Section 5 of the license. This project does not currently require a separate Contributor License Agreement (CLA) or Developer Certificate of Origin (DCO) sign-off.

## Contribution workflow

1. Search existing issues and pull requests.
2. Open an issue first for work described under [Discuss substantial changes first](#discuss-substantial-changes-first).
3. Fork the repository and create a focused branch.
4. Make the change in hand-written source files.
5. Add focused tests for new or changed behavior and update relevant documentation.
6. Update public API and dependency lock artifacts when applicable.
7. Run the relevant checks listed below.
8. Open a focused pull request, explain why the change is needed, and respond to review feedback.

Use clear, descriptive commit messages and pull request titles. Conventional Commits are not required, and contributors do not need to squash commits before opening a pull request. Maintainers use GitHub's squash-merge workflow for the final merge.

## Prerequisites

- The .NET SDK version pinned in [`global.json`](./global.json). The repository disables SDK roll-forward, so the exact version is required.
- The .NET 8 runtime for the `net8.0` test targets.

The shipping libraries target .NET Standard 2.0, .NET Standard 2.1, .NET Framework 4.6.2, .NET 8.0, and .NET 10.0. Tests target .NET 8.0 and .NET 10.0. Usage examples target .NET Framework 4.8 and .NET 10.0.

## Setup and checks

Restore the pinned local tools and locked dependencies, then build and test the solution:

```bash
dotnet tool restore
dotnet restore --locked-mode
dotnet build --no-restore
dotnet test --no-build --no-restore
```

Run a Release build when a change can affect packaging, trimming, or package compatibility:

```bash
dotnet build --configuration Release --no-restore
```

CI collects coverage on Linux with the pinned dotCover tool:

```bash
dotnet dotcover test --no-restore --dcXML=Configuration.xml
```

Local dotCover support depends on the operating system and pinned tool version. A successful `dotnet test` run is the required local test check when dotCover profiling is unavailable.

All warnings are treated as errors at warning level 5. Follow [`.editorconfig`](./.editorconfig) and the checked-in JetBrains settings, keep nullable analysis clean where enabled, and match the surrounding style. Private methods use camelCase in this repository. There is no numeric coverage threshold, but changed behavior must have focused tests.

## Testing and financial safety

- Repository tests must never place live orders or depend on a live trading account.
- Tests must be deterministic and run without Alpaca credentials. Use mocks, fakes, MockHttp, or synthetic fixtures for API behavior.
- Runnable trading examples must use paper trading by default and require an explicit user choice before targeting live trading.
- Real credentials, account identifiers, account data, and production payloads must never appear in tests, fixtures, examples, or committed logs. Remove request and trace IDs from public or committed material; share them only through private Alpaca Support channels when needed.

## Public API and dependency artifacts

The SDK clients, request and response models, streaming behavior, and public API are hand-written C#. This repository does not have a generated client tree.

The following checked-in files are review-critical API compatibility artifacts:

- `Alpaca.Markets/PublicAPI.Shipped.txt`
- `Alpaca.Markets/PublicAPI.Unshipped.txt`
- `Alpaca.Markets/CompatibilitySuppressions.xml`
- `Alpaca.Markets.Extensions/PublicAPI.Shipped.txt`
- `Alpaca.Markets.Extensions/PublicAPI.Unshipped.txt`
- `Alpaca.Markets.Extensions/CompatibilitySuppressions.xml`

Update the appropriate `PublicAPI.Unshipped.txt` file for intentional public API changes. Do not change public API files or compatibility suppressions for governance-only or internal changes.

Package validation compares both packages with version 7.2.0. When an intentional API addition requires a compatibility suppression, generate the suppression file with:

```bash
dotnet build --property ApiCompatGenerateSuppressionFile=true
```

Review every generated suppression and commit only entries that correspond to the intended API addition. Do not suppress accidental breaking changes.

The shipping projects use locked NuGet restore. When an intentional dependency change requires lock-file updates, regenerate and review `packages.lock.json`, then verify that `dotnet restore --locked-mode` succeeds. Preserve strong-name signing and do not replace or expose signing material.

## Documentation

Public documentation is generated from XML comments and published with DocFX. Keep XML comments, package READMEs, the root [README](./README.md), the [UsageExamples](./UsageExamples), and the [Wiki](https://github.com/alpacahq/alpaca-trade-api-csharp/wiki) consistent with behavior changes.

Preview the documentation locally on macOS or Linux:

```bash
./Documentation/serve.sh
```

On Windows:

```cmd
Documentation\serve.cmd
```

## Pull request checklist

Before requesting review, confirm that:

- the change is focused, explained, and linked to prior discussion when needed;
- public API files, compatibility suppressions, and lock files are updated only when applicable and have been reviewed;
- new or changed behavior has focused tests;
- tests are deterministic and credential-free and cannot place live orders;
- trading examples default to paper trading and fixtures use synthetic data;
- relevant XML comments, documentation, and examples are updated;
- applicable restore, build, test, coverage, package-validation, and documentation checks pass;
- no credentials, account data, request or trace IDs, or other secrets are included; and
- any provider-specific integration or promotional contribution has written Alpaca approval confirmed by repository maintainers.
