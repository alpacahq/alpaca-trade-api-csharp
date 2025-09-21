# GitHub Issues Analysis

Analysis of open GitHub issues categorized by implementation complexity, considering SDK-side feasibility.

## Easy 🟢

**Issues that can be completed with minimal effort and low risk:**

- **#387 - Review and improve XML comments** (`help wanted`)
  - Simple documentation fixes, typos, and clarity improvements
  - No code changes, just documentation updates

- **#388 - Add more code examples into XML documentation** (`help wanted`)
  - Adding `<example>` tags to existing XML comments
  - Straightforward documentation enhancement

- **#770 - Add order imbalance stock messages**
  - New message type implementation with Go SDK reference
  - Well-defined scope, similar to existing message handlers

- **#23 - Prepare C#/.NET based step-by-step tutorial** (`help wanted`)
  - Documentation/example creation based on existing Python tutorial
  - No SDK code changes required

- **#332 - Replace SynchronizationQueue with channel**
  - Replace custom queue with `System.Threading.Channel`
  - Well-defined refactoring task

## Medium 🟡

**Issues requiring moderate effort with manageable complexity:**

- **#772 - Add crypto perpetual futures support**
  - New feature with Go SDK reference implementation
  - Adding new endpoints and data models

- **#721 - Add support for options real-time streaming**
  - New streaming functionality with Python SDK reference
  - WebSocket streaming implementation

- **#468 - Provide documentation for all current SDKs**
  - Multi-version documentation generation system
  - Build/CI pipeline modifications

- **#355 - Consider C# records usage**
  - Large refactoring from interfaces to records
  - Requires careful migration planning

- **#482 - Prepare SDK for trimming feature**
  - .NET 7+ trimming compatibility
  - Complex reflection and serialization issues

## Hard 🔴

**Issues requiring significant effort and architectural changes:**

- **#145 - Switch to new JSON handling library**
  - Major dependency change from Newtonsoft.Json to System.Text.Json
  - Breaking change requiring extensive testing

- **#406 - Reduce code duplication in REST and streaming clients**
  - Major architectural refactoring
  - Requires redesigning client abstractions and shared code

## Impossible 🚫

**Issues that cannot be fixed from SDK side due to backend limitations:**

- **#724 - [BUG]: Problem subscribing many assets at once**
  - Backend limitation preventing subscription to >1400 symbols
  - Requires server-side changes to handle larger subscription batches

- **#617 - ClientOrderId propagation**
  - Backend issue where custom ClientOrderId is not propagated to bracket order legs
  - Server-side order processing problem that SDK cannot resolve

---

*Last updated: 2025-09-21*
*Total issues analyzed: 14*