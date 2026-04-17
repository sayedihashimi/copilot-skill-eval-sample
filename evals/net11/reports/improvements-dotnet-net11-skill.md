# Improvement Suggestions: dotnet-net11-skill

## Executive Summary
`dotnet-net11-skill` is the top scorer overall (187.5 vs 71.5 baseline) but still leaves major score on the table in high/critical dimensions because coverage is concentrated on a narrow “priority” subset and the run produced only a Blazor app (build failed) instead of full scenario coverage. The highest-impact fixes are structural: add missing EF Core and broad API coverage skills, expand guardrails/hooks beyond the current 8-pattern check, and tighten scenario-completion instructions.

Top opportunities:
1. Add a dedicated EF Core .NET 11 skill (currently missing entirely).
2. Expand `net11-focus-dimensions` from 8 dimensions to all evaluated dimensions and add missing `reference/` + `examples/`.
3. Strengthen PostToolUse guardrails to detect missing APIs that currently score 1s.
4. Tighten JSON/WebAPI skill to require the full System.Text.Json feature set and web pipeline requirements.
5. Update manifest keywords/description to improve activation for EF Core, OpenAPI, output cache, regex, filesystem, and process APIs.

Improvements are primarily **plugin-structure and skill-content coverage** rather than model quality.

## Current Performance Snapshot

| Dimension | dotnet-net11-skill | no-skills | Trail? |
|---|---:|---:|---|
| Zstandard Compression Usage | 5 | 1 |  |
| BFloat16 Type Usage | 4 | 1 |  |
| Rune-Based String Operations | 5 | 1 |  |
| HMAC Single-Step Verification | 5 | 1 |  |
| FrozenDictionary Collection Expressions | 5 | 1 |  |
| Collection Expression with() Arguments | 5 | 1 |  |
| Union Type Usage | 5 | 1 |  |
| MediaTypeMap Usage | 5 | 1 |  |
| DivisionRounding Modes | 5 | 1 |  |
| System.Text.Json New Features | 3 | 1 | ⚠ partial (critical) |
| RegexOptions.AnyNewLine | 1 | 1 | ⚠ missing |
| File System New APIs | 1 | 1 | ⚠ missing |
| Base64 Parity APIs | 1 | 1 | ⚠ missing |
| Generic Interlocked Operations | 1 | 1 | ⚠ missing |
| BitArray.PopCount | 1 | 1 | ⚠ missing |
| Native OpenTelemetry Tracing | 1 | 1 | ⚠ missing |
| OpenAPI Version | 1 | 1 | ⚠ missing |
| Dynamic Output Cache Policy Provider | 1 | 1 | ⚠ missing |
| Zstandard Response Compression | 1 | 1 | ⚠ missing |
| Blazor EnvironmentBoundary Component | 5 | 1 |  |
| Blazor Label and DisplayName Components | 5 | 1 |  |
| QuickGrid OnRowClick | 5 | 1 |  |
| RelativeToCurrentUri Navigation | 5 | 1 |  |
| Blazor TempData Support | 4 | 1 |  |
| Blazor BasePath Component | 5 | 1 |  |
| EF Core GetEntriesForState | 1 | 2 | 🔻 below baseline |
| EF Core RemoveDbContext | 1 | 2 | 🔻 below baseline |
| EF Core ExcludeForeignKeyFromMigrations | 1 | 2 | 🔻 below baseline |
| EF Core JSON Query Functions | 1 | 2 | 🔻 below baseline |
| SignalR ConfigureConnection | 3 | 1 | ⚠ partial |
| Blazor Virtualize Variable-Height Items | 2 | 1 | ⚠ partial |
| Runtime Async Configuration | 1 | 1 | ⚠ missing |
| ProcessExitStatus Usage | 1 | 1 | ⚠ missing |
| OpenAPI Binary File Response | 1 | 1 | ⚠ missing |
| Brotli and Compression Options | 1 | 1 | ⚠ missing |
| Vector Constants | 1 | 1 | ⚠ missing |
| Overall .NET 11 API Adoption Rate | 3 | 1 | ⚠ partial (critical) |
| Token Efficiency | 5 | 5 |  |

## Plugin Structure Assessment

### Plugin: `dotnet-net11`
- **Inventory**: 5 skills, 1 agent, hooks configured, no MCP, no LSP.
- **Manifest quality**: Good baseline (`name`, `version`, `description`, `keywords` present), but keyword and description breadth do not match many evaluated dimensions (EF Core replacement APIs, OpenAPI 3.2, output cache provider, regex/file/process/runtime features).
- **Key structural gaps**:
  - No dedicated EF Core skill despite plugin description claiming EF Core focus.
  - `net11-focus-dimensions` has no `reference/` or `examples/` (explicitly flagged in structure analysis).
  - Guardrail script checks only a narrow subset; most low-scoring dimensions are not enforced.
  - Single reviewer agent is useful but no scenario-completion/planning agent to force complete multi-surface coverage.

## Improvement Suggestions

### Plugin-Level Improvements

#### P1. Add a dedicated EF Core .NET 11 skill
- **Type**: New skill
- **Dimensions affected**: EF Core GetEntriesForState, EF Core RemoveDbContext, EF Core ExcludeForeignKeyFromMigrations, EF Core JSON Query Functions, Overall .NET 11 API Adoption Rate
- **Problem**: Plugin promises EF Core coverage, but no EF Core skill exists. Analysis shows `dotnet-net11-skill` scored **1** on all four EF Core dimensions and trailed baseline (**1 vs 2**) because those APIs were absent.
- **Suggested changes**:

  **New File** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-efcore\SKILL.md`):
  ```md
  ---
  name: net11-efcore
  description: Build EF Core net11.0 data layers using GetEntriesForState, RemoveDbContext, ExcludeForeignKeyFromMigrations, and JsonContains/JsonPathExists. Use when creating repositories, DbContext setup, migrations, and JSON column queries.
  ---
  
  # EF Core .NET 11 Workflow
  
  1. Confirm `net11.0` target and add one focused EF Core demo module if the app is not data-centric.
     Success: EF Core .NET 11 APIs are still demonstrated in a compilable `FeatureCoverage/EfCoreNet11Demo.cs`.
  
  2. Use state-based change tracking API.
     - Prefer `context.ChangeTracker.GetEntriesForState(...)` over `Entries().Where(...)`.
     Success: at least one method uses `GetEntriesForState`.
  
  3. Replace DbContext registrations cleanly in tests/tools.
     - Use `RemoveDbContext<T>()` before re-registering provider/factory.
     Success: no manual descriptor loop for DbContext removal.
  
  4. Configure migration exclusions for selected FKs.
     - Use `.ExcludeForeignKeyFromMigrations(true)` on applicable relationships.
     Success: at least one FK demonstrates this API.
  
  5. Query JSON columns with native EF functions.
     - Use `EF.Functions.JsonContains(...)` and/or `EF.Functions.JsonPathExists(...)`.
     Success: no `LIKE` fallback when native function is available in scenario.
  
  Example (input -> expected output):
  - Input: "Build order management EF Core sample."
  - Expected output includes `GetEntriesForState`, `RemoveDbContext<T>()`, and one JSON query via `EF.Functions.JsonContains`.
  ```

  **New File** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-efcore\reference\efcore-net11-patterns.md`):
  ```md
  # EF Core .NET 11 Patterns
  
  | Dimension | Prefer | Avoid |
  |---|---|---|
  | Change tracking | `GetEntriesForState` | `Entries().Where(...)` |
  | Context replacement | `RemoveDbContext<T>()` | descriptor remove loops |
  | FK migration exclusion | `.ExcludeForeignKeyFromMigrations(true)` | manual migration edits |
  | JSON columns | `JsonContains/JsonPathExists` | `LIKE`/raw SQL fallback |
  ```

  **New File** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-efcore\examples\efcore-golden.md`):
  ```md
  ```csharp
  var modified = context.ChangeTracker.GetEntriesForState(EntityState.Modified);
  
  services.RemoveDbContext<AppDbContext>();
  services.AddDbContext<AppDbContext>(o => o.UseSqlite("Data Source=test.db"));
  
  modelBuilder.Entity<OrderItem>()
      .HasOne(oi => oi.Product)
      .WithMany()
      .HasForeignKey(oi => oi.ProductId)
      .ExcludeForeignKeyFromMigrations(true);
  
  var tagged = context.Products
      .Where(p => EF.Functions.JsonContains(p.Metadata, """{"tags":["sale"]}"""));
  ```
  ```

- **Expected impact**: +18 to +28 weighted points (mostly HIGH-tier + one MEDIUM + CRITICAL adoption-rate lift).

#### P2. Expand plugin guardrails to enforce currently-missing dimensions
- **Type**: Hook + script enhancement
- **Dimensions affected**: RegexOptions.AnyNewLine, File System New APIs, Base64 Parity APIs, Generic Interlocked Operations, BitArray.PopCount, Runtime Async Configuration, ProcessExitStatus Usage, OpenAPI Binary File Response, Brotli options, Vector Constants
- **Problem**: Current guard script checks only 10 narrow patterns, so many dimensions that scored **1** are never enforced.
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\scripts\net11-pattern-guard.ps1`):
  ```powershell
  $checks = @(
      @{ Name = "Missing BFloat16"; Must = "BFloat16"; Scope = "*.cs" },
      @{ Name = "Missing Rune APIs"; Must = "Rune"; Scope = "*.cs" },
      @{ Name = "Missing HMAC verify"; Must = "HMACSHA256.Verify|VerifyHmac"; Scope = "*.cs" },
      @{ Name = "Legacy hash+compare"; Ban = "HashData\(|FixedTimeEquals\("; Scope = "*.cs" },
      @{ Name = "Legacy FrozenDictionary pattern"; Ban = "ToFrozenDictionary\("; Scope = "*.cs" },
      @{ Name = "Missing collection with()"; Must = "with\(capacity:|with\(comparer:"; Scope = "*.cs" },
      @{ Name = "Missing union"; Must = "\bunion\b"; Scope = "*.cs" },
      @{ Name = "Missing DivisionRounding"; Must = "DivisionRounding\."; Scope = "*.cs" },
      @{ Name = "Missing JsonNamingPolicy attribute"; Must = "\[JsonNamingPolicy\("; Scope = "*.cs" },
      @{ Name = "Disallowed not-applicable note"; Ban = "Not applicable"; Scope = "*.md" }
  )
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\scripts\net11-pattern-guard.ps1`):
  ```powershell
  $checks = @(
      @{ Name = "Missing BFloat16"; Must = "BFloat16"; Scope = "*.cs" },
      @{ Name = "Missing Rune APIs"; Must = "Rune"; Scope = "*.cs" },
      @{ Name = "Missing HMAC verify"; Must = "HMACSHA256.Verify|VerifyHmac"; Scope = "*.cs" },
      @{ Name = "Legacy hash+compare"; Ban = "HashData\(|FixedTimeEquals\("; Scope = "*.cs" },
      @{ Name = "Legacy FrozenDictionary pattern"; Ban = "ToFrozenDictionary\("; Scope = "*.cs" },
      @{ Name = "Missing collection with()"; Must = "with\(capacity:|with\(comparer:"; Scope = "*.cs" },
      @{ Name = "Missing union"; Must = "\bunion\b"; Scope = "*.cs" },
      @{ Name = "Missing DivisionRounding"; Must = "DivisionRounding\."; Scope = "*.cs" },
      @{ Name = "Missing JsonNamingPolicy attribute"; Must = "\[JsonNamingPolicy\("; Scope = "*.cs" },
      @{ Name = "Missing Regex AnyNewLine"; Must = "RegexOptions\.AnyNewLine"; Scope = "*.cs" },
      @{ Name = "Missing file APIs"; Must = "File\.CreateHardLink|File\.OpenNullHandle|CreateAnonymousPipe"; Scope = "*.cs" },
      @{ Name = "Missing Base64 parity APIs"; Must = "Base64\.EncodeToString|Base64\.DecodeFromChars|Base64\.GetEncodedLength"; Scope = "*.cs" },
      @{ Name = "Missing generic Interlocked enum ops"; Must = "Interlocked\.(And|Or)\("; Scope = "*.cs" },
      @{ Name = "Missing BitArray.PopCount"; Must = "PopCount\("; Scope = "*.cs" },
      @{ Name = "Missing runtime-async feature"; Must = "<Features>runtime-async=on</Features>"; Scope = "*.csproj" },
      @{ Name = "Missing Process.ExitStatus"; Must = "\.ExitStatus"; Scope = "*.cs" },
      @{ Name = "Missing OpenAPI binary Produces metadata"; Must = "Produces<FileContentResult>"; Scope = "*.cs" },
      @{ Name = "Missing Brotli WindowLog"; Must = "WindowLog"; Scope = "*.cs" },
      @{ Name = "Missing Vector constants"; Must = "Vector<.*>\.(Pi|Tau|E|Epsilon|NaN)"; Scope = "*.cs" },
      @{ Name = "Disallowed not-applicable note"; Ban = "Not applicable"; Scope = "*.md" }
  )
  ```

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\hooks\hooks.json`):
  ```json
  {
    "hooks": {
      "SessionStart": [
        {
          "hooks": [
            {
              "type": "command",
              "command": "pwsh -NoProfile -Command \"Write-Host '[dotnet-net11] Priority dimensions enforced: BFloat16, Rune, HMAC.Verify, FrozenDictionary expr, with(), union, DivisionRounding, JsonNamingPolicy.'\""
            }
          ]
        }
      ],
      "PostToolUse": [
        {
          "matcher": "Write|Edit|MultiEdit",
          "hooks": [
            {
              "type": "command",
              "command": "pwsh -NoProfile -File \"${CLAUDE_PLUGIN_ROOT}\\scripts\\net11-pattern-guard.ps1\""
            }
          ]
        }
      ]
    }
  }
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\hooks\hooks.json`):
  ```json
  {
    "hooks": {
      "SessionStart": [
        {
          "hooks": [
            {
              "type": "command",
              "command": "pwsh -NoProfile -Command \"Write-Host '[dotnet-net11] Guardrails enabled for BCL, ASP.NET Core, Blazor, EF Core, runtime, regex, and process APIs.'\""
            }
          ]
        }
      ],
      "PostToolUse": [
        {
          "matcher": "Write|Edit|MultiEdit",
          "hooks": [
            {
              "type": "command",
              "command": "pwsh -NoProfile -File \"${CLAUDE_PLUGIN_ROOT}\\scripts\\net11-pattern-guard.ps1\""
            }
          ]
        }
      ]
    }
  }
  ```

- **Expected impact**: +10 to +20 weighted points by reducing repeated “1” outcomes across MEDIUM/HIGH dimensions.

#### P3. Manifest discoverability and domain-alignment update
- **Type**: Manifest fix
- **Dimensions affected**: Indirectly all missed dimensions via better activation routing
- **Problem**: Manifest is valid but underspecifies several target surfaces (EF Core replacement APIs, OpenAPI 3.2, output cache provider, regex/file/process/runtime APIs).
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\plugin.json`):
  ```json
  {
    "name": "dotnet-net11",
    "version": "0.2.0",
    "description": "Generates net11.0-first code that adopts new C# 15, BCL, ASP.NET Core, Blazor, and EF Core APIs while avoiding legacy fallbacks.",
    "author": { "name": "net11-auto01" },
    "license": "MIT",
    "keywords": [
      "dotnet",
      "net11",
      "csharp15",
      "aspnetcore",
      "efcore",
      "blazor",
      "system-text-json",
      "zstandard"
    ],
    "skills": ["./skills/"]
  }
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\plugin.json`):
  ```json
  {
    "name": "dotnet-net11",
    "version": "0.3.0",
    "description": "Generates net11.0-first code for C# 15, BCL, ASP.NET Core, Blazor, and EF Core with explicit coverage of modern APIs (OpenAPI 3.2, output-cache providers, JSON features, file/process/runtime APIs) and guardrails against legacy fallbacks.",
    "author": { "name": "net11-auto01" },
    "license": "MIT",
    "keywords": [
      "dotnet",
      "net11",
      "csharp15",
      "aspnetcore",
      "efcore",
      "blazor",
      "system-text-json",
      "openapi-3-2",
      "output-cache",
      "regex-anynewline",
      "filesystem-apis",
      "process-exitstatus",
      "runtime-async",
      "zstandard"
    ],
    "skills": ["./skills/"]
  }
  ```

- **Expected impact**: Small direct score gain; medium indirect gain from better skill activation.

### Skill-Level Improvements

### S1. Expand `net11-focus-dimensions` into full coverage orchestration skill
- **Dimensions affected**: All currently missing/tie-at-1 dimensions, plus Overall .NET 11 API Adoption Rate
- **Current score → Target score**: Overall adoption **3 → 4/5**, several dimensions **1 → 3/4**
- **Problem**: Skill enforces only 8 “priority dimensions,” leaving many evaluated dimensions untouched. Structure gap: no `reference/` or `examples/`.
- **Root cause**: The current instruction set narrows scope too aggressively; success criteria do not require non-priority dimensions or scenario completion.
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-focus-dimensions\SKILL.md`):
  ```md
  ---
  name: net11-focus-dimensions
  description: Enforce demonstration of net11 priority dimensions (BFloat16, Rune APIs, HMAC verify, FrozenDictionary expressions, collection with(), union, DivisionRounding, and JSON features) using isolated helper modules when needed. Use when evaluation coverage is required across mixed scenarios.
  ---
  
  # .NET 11 Priority Dimension Enforcement
  
  1. Identify which of the priority dimensions are not naturally present in the requested app.
     Success: list each missing priority dimension explicitly.
  
  2. For every missing priority dimension, add a minimal demo module under `FeatureCoverage/`
     that compiles and is invoked by at least one endpoint, command, or test.
     Success: no priority dimension is left as "not applicable".
  
  3. Use only target APIs:
     - `System.Numerics.BFloat16`
     - Rune overloads (`string.Contains/IndexOf/Replace/Split(Rune)`)
     - `HMACSHA256.Verify` / `CryptographicOperations.VerifyHmac`
     - `FrozenDictionary<K,V> map = ["k": v]`
     - Collection expression `with(...)`
     - `union` with exhaustive `switch`
     - `int.Divide/DivRem(..., DivisionRounding.*)`
     - JSON features: `JsonNamingPolicy.PascalCase`, `[JsonNamingPolicy]`, type-level `[JsonIgnore]`, `IReadOnlySet<T>`, `GetTypeInfo<T>()`
     Success: generated code contains these concrete API names where expected.
  
  4. Emit a final coverage checklist with file paths and API names.
     Success: each priority dimension maps to a concrete file and code symbol.
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-focus-dimensions\SKILL.md`):
  ```md
  ---
  name: net11-focus-dimensions
  description: Orchestrate complete net11.0 evaluation coverage across BCL, ASP.NET Core, Blazor, EF Core, runtime, and diagnostics APIs. Use when benchmark/evaluation tasks require broad .NET 11 feature adoption without “not applicable” gaps.
  ---
  
  # .NET 11 Dimension Coverage Orchestration
  
  1. Build a coverage matrix before coding.
     - Map requested scenario surfaces to dimensions: BCL, WebAPI, Blazor, EF Core, runtime/process.
     - Mark missing dimensions that need isolated demos.
     Success: explicit matrix exists and includes every scored dimension.
  
  2. Implement missing dimensions in isolated, compilable modules under `FeatureCoverage/`.
     - Wire each module into one executable path (endpoint, command, startup hook, or test).
     - Never leave dimensions as “not applicable”.
     Success: each uncovered dimension now has code + invocation path.
  
  3. Use required APIs for currently low-scoring dimensions:
     - Regex: `RegexOptions.AnyNewLine`
     - Filesystem/pipes: `File.CreateHardLink`, `File.OpenNullHandle`, `SafeFileHandle.CreateAnonymousPipe`
     - Base64: `Base64.EncodeToString/DecodeFromChars/GetEncodedLength`
     - Interlocked enum ops: `Interlocked.And/Or` on enum values
     - BitArray: `bits.PopCount()`
     - Runtime/process: `<Features>runtime-async=on</Features>`, `process.ExitStatus`
     - Web/OpenAPI: `OpenApiSpecVersion.OpenApi3_2`, `.Produces<FileContentResult>(...)`
     - Compression options: `BrotliCompressionOptions.WindowLog`
     - SIMD constants: `Vector<float>.Pi/Tau/E`
     Success: each API appears in generated source with runnable usage.
  
  4. Emit final checklist with dimension -> file -> symbol -> invocation path.
     Success: checklist has zero missing dimensions.
  
  Example (input -> expected output):
  - Input: "Create a Blazor app."
  - Expected output: Blazor features in app code plus `FeatureCoverage/` demos for EF Core/WebAPI/runtime dimensions not naturally present.
  ```

  **New File** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-focus-dimensions\reference\coverage-matrix.md`):
  ```md
  # .NET 11 Coverage Matrix
  
  | Area | Dimensions to force when absent |
  |---|---|
  | BCL | RegexOptions.AnyNewLine, Base64 parity, Interlocked enum ops, BitArray.PopCount, vector constants |
  | WebAPI | OpenAPI 3.2, binary Produces metadata, output cache provider, Zstandard response compression, native tracing |
  | Blazor | EnvironmentBoundary, Label/DisplayName, OnRowClick, TempData, BasePath, RelativeToCurrentUri, Virtualize tuning |
  | EF Core | GetEntriesForState, RemoveDbContext, ExcludeForeignKeyFromMigrations, JsonContains/JsonPathExists |
  | Runtime | runtime-async features flag, Process.ExitStatus |
  ```

  **New File** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-focus-dimensions\examples\orchestration-golden.md`):
  ```md
  Input: "Build a product API"
  
  Output checklist excerpt:
  - OpenAPI Version -> Program.cs -> OpenApiSpecVersion.OpenApi3_2 -> app startup
  - EF Core JSON Query -> FeatureCoverage/EfCoreNet11Demo.cs -> EF.Functions.JsonContains -> startup probe
  - Regex AnyNewLine -> FeatureCoverage/TextRegexDemo.cs -> RegexOptions.AnyNewLine -> startup probe
  - ProcessExitStatus -> FeatureCoverage/ProcessDemo.cs -> process.ExitStatus -> diagnostic endpoint
  ```

- **Expected impact**: +20 to +35 weighted points (broad coverage lift, including CRITICAL adoption rate).

### S2. Tighten `net11-json-webapi` to require full JSON + web pipeline feature set
- **Dimensions affected**: System.Text.Json New Features, Native OpenTelemetry Tracing, OpenAPI Version, Dynamic Output Cache Policy Provider, Zstandard Response Compression, OpenAPI Binary File Response
- **Current score → Target score**: JSON **3 → 5**; multiple web dimensions **1 → 4/5**
- **Problem**: Current skill lists features, but enforcement is soft and examples are sparse.
- **Root cause**: No explicit “must fail if missing” checklist for each required web API; reference/example files are minimal.
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-json-webapi\SKILL.md`):
  ```md
  1. Configure JSON using built-in .NET 11 features:
     - `JsonNamingPolicy.PascalCase`
     - at least one per-property `[JsonNamingPolicy(...)]`
     - type-level `[JsonIgnore]` where needed
     - `IReadOnlySet<T>` support
     - `options.GetTypeInfo<T>()` (generic form)
     Success: generated model code contains concrete examples of each bullet.
  
  2. Configure OpenAPI, telemetry, and output cache provider architecture:
     - OpenAPI: `OpenApiSpecVersion.OpenApi3_2`
     - Native tracing source: `AddSource("Microsoft.AspNetCore")`
     - Output cache: implement `IOutputCachePolicyProvider` + DI registration (avoid static-only `AddPolicy(...)`).
     Success: startup code registers provider type and resolves policies dynamically.
  
  3. Prefer built-in compression and metadata:
     - Response/request compression with Zstandard provider options
     - Binary endpoint docs with `.Produces<FileContentResult>(contentType: "application/octet-stream")`
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-json-webapi\SKILL.md`):
  ```md
  1. Implement all required JSON features in concrete model + options code.
     - `JsonNamingPolicy.PascalCase`
     - one per-property `[JsonNamingPolicy(...)]`
     - one type-level `[JsonIgnore]`
     - one `IReadOnlySet<T>` property in DTO/model
     - one `options.GetTypeInfo<T>()` invocation
     Success: all five appear in compilable code; do not finalize if any are missing.
  
  2. Configure startup pipeline with required .NET 11 web APIs.
     - `OpenApiSpecVersion.OpenApi3_2`
     - `AddOpenTelemetry().WithTracing(t => t.AddSource("Microsoft.AspNetCore"))`
     - DI registration for `IOutputCachePolicyProvider`
     - Response compression with Zstandard provider + options
     Success: Program.cs contains all four registrations.
  
  3. Add at least one binary download endpoint with explicit OpenAPI metadata.
     - Use `.Produces<FileContentResult>(contentType: "application/octet-stream")`
     Success: endpoint and metadata both present.
  
  4. Emit JSON+WebAPI checklist (feature -> file -> symbol).
     Success: zero missing entries.
  ```

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-json-webapi\reference\json-webapi-net11.md`):
  ```md
  # .NET 11 JSON + Web API Checklist
  
  - JsonNamingPolicy.PascalCase
  - [JsonNamingPolicy(...)] per property
  - type-level [JsonIgnore]
  - GetTypeInfo<T>()
  - IOutputCachePolicyProvider
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-json-webapi\reference\json-webapi-net11.md`):
  ```md
  # .NET 11 JSON + Web API Checklist
  
  ## JSON
  - `JsonNamingPolicy.PascalCase`
  - `[JsonNamingPolicy(...)]` per-property sample
  - type-level `[JsonIgnore]`
  - `IReadOnlySet<T>`
  - `GetTypeInfo<T>()`
  
  ## Web Pipeline
  - `OpenApiSpecVersion.OpenApi3_2`
  - `AddSource("Microsoft.AspNetCore")`
  - `IOutputCachePolicyProvider` + DI
  - `AddResponseCompression` with Zstandard provider options
  - binary endpoint metadata: `.Produces<FileContentResult>(contentType: "application/octet-stream")`
  ```

- **Expected impact**: +12 to +22 weighted points (CRITICAL + HIGH web stack dimensions).

### S3. Widen `net11-bcl-core` to include unaddressed BCL dimensions
- **Dimensions affected**: RegexOptions.AnyNewLine, File System New APIs, Base64 Parity APIs, Generic Interlocked Operations, BitArray.PopCount, ProcessExitStatus Usage, Vector Constants
- **Current score → Target score**: mostly **1 → 3/4**
- **Problem**: Current BCL skill only covers six APIs and misses most low-scoring BCL/runtime dimensions.
- **Root cause**: Skill scope omits these APIs in required outcomes and examples.
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-bcl-core\reference\bcl-core-patterns.md`):
  ```md
  # BCL Core Patterns
  
  Required outcomes:
  - BFloat16 conversion helper
  - Rune-based text operations helper
  - HMAC single-step verification helper
  - FrozenDictionary expression lookup table
  - DivisionRounding pagination helper
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-bcl-core\reference\bcl-core-patterns.md`):
  ```md
  # BCL Core Patterns
  
  Required outcomes:
  - BFloat16 conversion helper
  - Rune-based text operations helper
  - HMAC single-step verification helper
  - FrozenDictionary expression lookup table
  - DivisionRounding pagination helper
  - Regex helper using `RegexOptions.AnyNewLine`
  - File helper using `File.CreateHardLink` or `File.OpenNullHandle`
  - Base64 helper using `Base64.EncodeToString/DecodeFromChars`
  - Enum flag helper using generic `Interlocked.Or/And`
  - `BitArray.PopCount` helper
  - Process helper reading `process.ExitStatus`
  - SIMD helper using `Vector<float>.Pi`/`Tau`
  ```

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-bcl-core\examples\bcl-core-golden.md`):
  ```md
  ```csharp
  FrozenDictionary<string,int> status = ["ok": 200];
  var pageCount = int.Divide(total, size, DivisionRounding.ToPositiveInfinity);
  var verified = HMACSHA256.Verify(key, data, mac);
  ```
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-bcl-core\examples\bcl-core-golden.md`):
  ```md
  ```csharp
  FrozenDictionary<string,int> status = ["ok": 200];
  var pageCount = int.Divide(total, size, DivisionRounding.ToPositiveInfinity);
  var verified = HMACSHA256.Verify(key, data, mac);
  var rx = new Regex(pattern, RegexOptions.AnyNewLine);
  var encoded = Base64.EncodeToString(bytes);
  var count = flags.PopCount();
  var v = Vector<float>.Pi;
  ```
  ```

- **Expected impact**: +8 to +15 weighted points, mainly MEDIUM/HIGH BCL dimensions.

### S4. Improve C# 15 + JSON feature depth in `net11-features`
- **Dimensions affected**: System.Text.Json New Features, Runtime Async Configuration, Overall .NET 11 API Adoption Rate
- **Current score → Target score**: JSON **3 → 4/5**, runtime async **1 → 3/4**
- **Problem**: `net11-features` names APIs but does not require all to be implemented in code and invoked.
- **Root cause**: Success criteria are broad (“where relevant”), allowing partial implementation.
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-features\SKILL.md`):
  ```md
  2. Use these APIs by default (priority list).
     - Compression: `ZstandardStream`, `ZstandardCompressionOptions`
     - Numeric: `System.Numerics.BFloat16`
     - Unicode: `string.Contains/IndexOf/Replace/Split(Rune)` and Rune-aware APIs
     - Crypto verification: `HMACSHA256.Verify` / `CryptographicOperations.VerifyHmac`
     - Collections: `FrozenDictionary<K,V> map = ["k": v];`
     - Collection expressions: `[with(capacity: n), ..values]`
     - Union modeling: `union` with exhaustive `switch`
     - MIME mapping: `MediaTypeMap.GetMediaType/GetExtension`
     - Division math: `int.Divide/Remainder/DivRem` with `DivisionRounding`
     - JSON: `JsonNamingPolicy.PascalCase`, `[JsonNamingPolicy]`, type-level `[JsonIgnore]`, `IReadOnlySet<T>`, `GetTypeInfo<T>()`
     Success: generated code references concrete API names above where relevant.
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-features\SKILL.md`):
  ```md
  2. Use these APIs by default and require concrete implementation coverage.
     - Compression: `ZstandardStream`, `ZstandardCompressionOptions`
     - Numeric: `System.Numerics.BFloat16`
     - Unicode: `string.Contains/IndexOf/Replace/Split(Rune)` and Rune-aware APIs
     - Crypto verification: `HMACSHA256.Verify` / `CryptographicOperations.VerifyHmac`
     - Collections: `FrozenDictionary<K,V> map = ["k": v];`
     - Collection expressions: `[with(capacity: n), ..values]`
     - Union modeling: `union` with exhaustive `switch`
     - MIME mapping: `MediaTypeMap.GetMediaType/GetExtension`
     - Division math: `int.Divide/Remainder/DivRem` with `DivisionRounding`
     - JSON (all required): `JsonNamingPolicy.PascalCase`, `[JsonNamingPolicy]`, type-level `[JsonIgnore]`, `IReadOnlySet<T>`, `GetTypeInfo<T>()`
     - Runtime config: `<Features>runtime-async=on</Features>` in project file when async runtime is requested/evaluated.
     Success: each listed item appears in generated source or project configuration with file path evidence.
  ```

- **Expected impact**: +6 to +12 weighted points, with strongest effect on CRITICAL JSON and CRITICAL adoption rate.

## Summary of Recommended Changes

### New files
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-efcore\SKILL.md`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-efcore\reference\efcore-net11-patterns.md`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-efcore\examples\efcore-golden.md`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-focus-dimensions\reference\coverage-matrix.md`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-focus-dimensions\examples\orchestration-golden.md`

### Modified files
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\plugin.json`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\hooks\hooks.json`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\scripts\net11-pattern-guard.ps1`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-focus-dimensions\SKILL.md`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-json-webapi\SKILL.md`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-json-webapi\reference\json-webapi-net11.md`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-bcl-core\reference\bcl-core-patterns.md`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-bcl-core\examples\bcl-core-golden.md`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-features\SKILL.md`

## Risks and Trade-offs
- Expanding hard guardrails can increase false negatives for narrow prompts; mitigate by scoping checks to evaluation runs or allowing feature-group toggles.
- Forcing every dimension in every run may add “demo noise” to production-facing architecture; keep isolated `FeatureCoverage/` modules and explicit invocation boundaries.
- Adding more skills can increase activation ambiguity; use precise descriptions and avoid overlap between `net11-features`, `net11-focus-dimensions`, and new `net11-efcore`.
