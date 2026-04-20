# Improvement Suggestions: dotnet-net11-skill

## Executive Summary
`dotnet-net11-skill` scored **72.5/297.5 (24%)** and only showed strong behavior in one EF Core dimension (`GetEntriesForState = 4`). Most dimensions scored `1` because the generated run only covered `efcore` and missed BCL/WebAPI/Blazor features expected by the plugin’s stated purpose (`reports/analysis.md`, lines 952-963).

The highest-impact opportunities are mostly **plugin-structure changes** (add missing scenario-focused skills and stronger guardrails), followed by **targeted SKILL.md improvements** (net11-focus-dimensions, net11-json-webapi, net11-features).

Top 5 opportunities:
1. Add a dedicated **Blazor net11 skill** (currently no Blazor-focused skill exists).
2. Add a dedicated **EF Core net11 advanced skill** for the dimensions currently stuck at 2/3.
3. Expand the **hook guard + reviewer agent** from an 8-dimension check to full rubric coverage.
4. Fill missing `reference/` + `examples/` for `net11-focus-dimensions`.
5. Tighten `net11-json-webapi` and `net11-features` instructions so they force demonstrable APIs, not narrative claims.

## Current Performance Snapshot
Only one configuration/run exists in this dataset, so baseline/top-config comparisons are unavailable. The table below uses the observed run and marks scores `<4` as trailing.

| Dimension | Score | Status |
|---|---:|---|
| Zstandard Compression Usage | 1 | ❌ trailing |
| BFloat16 Type Usage | 1 | ❌ trailing |
| Rune-Based String Operations | 1 | ❌ trailing |
| HMAC Single-Step Verification | 1 | ❌ trailing |
| FrozenDictionary Collection Expressions | 1 | ❌ trailing |
| Collection Expression with() Arguments | 1 | ❌ trailing |
| Union Type Usage | 1 | ❌ trailing |
| MediaTypeMap Usage | 1 | ❌ trailing |
| DivisionRounding Modes | 1 | ❌ trailing |
| System.Text.Json New Features | 1 | ❌ trailing |
| RegexOptions.AnyNewLine | 1 | ❌ trailing |
| File System New APIs | 1 | ❌ trailing |
| Base64 Parity APIs | 1 | ❌ trailing |
| Generic Interlocked Operations | 1 | ❌ trailing |
| BitArray.PopCount | 1 | ❌ trailing |
| Native OpenTelemetry Tracing | 1 | ❌ trailing |
| OpenAPI Version | 1 | ❌ trailing |
| Dynamic Output Cache Policy Provider | 1 | ❌ trailing |
| Zstandard Response Compression | 1 | ❌ trailing |
| Blazor EnvironmentBoundary Component | 1 | ❌ trailing |
| Blazor Label and DisplayName Components | 1 | ❌ trailing |
| QuickGrid OnRowClick | 1 | ❌ trailing |
| RelativeToCurrentUri Navigation | 1 | ❌ trailing |
| Blazor TempData Support | 1 | ❌ trailing |
| Blazor BasePath Component | 1 | ❌ trailing |
| EF Core GetEntriesForState | 4 | ✅ strongest |
| EF Core RemoveDbContext | 3 | ⚠️ partial |
| EF Core ExcludeForeignKeyFromMigrations | 2 | ❌ trailing |
| EF Core JSON Query Functions | 2 | ❌ trailing |
| SignalR ConfigureConnection | 1 | ❌ trailing |
| Blazor Virtualize Variable-Height Items | 1 | ❌ trailing |
| Runtime Async Configuration | 1 | ❌ trailing |
| ProcessExitStatus Usage | 1 | ❌ trailing |
| OpenAPI Binary File Response | 1 | ❌ trailing |
| Brotli and Compression Options | 1 | ❌ trailing |
| Vector Constants | 1 | ❌ trailing |
| Overall .NET 11 API Adoption Rate | 1 | ❌ trailing |

## Plugin Structure Assessment
### Plugin: `dotnet-net11`
- **Inventory**: 5 skills, 1 agent, hooks configured, no MCP/LSP.
- **Manifest quality**: `plugin.json` is valid and has strong base metadata (`name`, `version`, `description`, `keywords`), but keywords do not explicitly advertise Blazor/OpenAPI/output-cache dimensions that are currently underperforming.
- **Key structural gaps**:
  1. No dedicated Blazor skill (yet 8 Blazor/SignalR dimensions are scored and all at 1).
  2. No dedicated EF Core advanced skill (current EF scores show partial modernization, especially FK exclusion and JSON functions).
  3. Guardrails check only a subset of dimensions (`scripts/net11-pattern-guard.ps1`), so many rubric failures are not prevented.
  4. `net11-focus-dimensions` lacks `reference/` and `examples/`, reducing activation clarity and instruction quality under best-practice rubric.

## Improvement Suggestions

### Plugin-Level Improvements

#### P1. Add a dedicated Blazor net11 skill
- **Type**: New skill
- **Dimensions affected**: Blazor EnvironmentBoundary, Label/DisplayName, QuickGrid OnRowClick, RelativeToCurrentUri Navigation, TempData, BasePath, Virtualize variable height, SignalR ConfigureConnection, Overall adoption.
- **Problem**: Plugin has no Blazor-focused skill despite many Blazor dimensions. Current run shows all Blazor dimensions at `1` with “no Blazor app generated” evidence (`reports/analysis.md`, lines 587-594, 607-614, 827-834).
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\README.md`):
  ```md
  ## Included skills
  - `net11-features`
  - `net11-bcl-core`
  - `net11-json-webapi`
  - `net11-csharp15`
  - `net11-focus-dimensions`
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\README.md`):
  ```md
  ## Included skills
  - `net11-features`
  - `net11-bcl-core`
  - `net11-json-webapi`
  - `net11-blazor-ui`
  - `net11-csharp15`
  - `net11-focus-dimensions`
  ```

  **New File** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-blazor-ui\SKILL.md`):
  ```md
  ---
  name: net11-blazor-ui
  description: Build Blazor net11.0 apps using EnvironmentBoundary, Label/DisplayName, QuickGrid OnRowClick, TempData, BasePath, variable-height Virtualize, and SignalR ConfigureConnection. Use when creating or updating Blazor pages/components.
  ---
  
  # .NET 11 Blazor UI Workflow
  
  1. Add at least one component/page that uses `<EnvironmentBoundary>` and `<BasePath />`.
     Success: both tags appear in `.razor` files.
  
  2. Use `<Label For>` and `<DisplayName For>` in form/table UI instead of hardcoded labels.
     Success: generated markup includes both components with strongly typed `For`.
  
  3. Use `QuickGrid` with `OnRowClick` and no button-column workaround.
     Success: `OnRowClick` is bound directly on `QuickGrid`.
  
  4. Use relative navigation APIs (`NavigationOptions.RelativeToCurrentUri`) and `GetUriWithHash`.
     Success: navigation code avoids manual URI string concatenation.
  
  5. Configure TempData and SignalR in interactive server mode.
     Success: includes `[CascadingParameter] ITempData` and `AddInteractiveServerRenderMode(... ConfigureConnection ...)`.
  
  6. Use `<Virtualize>` for variable-height lists with `OverscanCount="15"`.
     Success: virtualized component compiles and includes overscan setting.
  
  Example:
  Input: "Build a task board in Blazor."
  Output includes `QuickGrid OnRowClick`, `EnvironmentBoundary`, and TempData flash messaging.
  ```

- **Expected impact**: High (HIGH+MEDIUM tiers), roughly **+30 to +45 weighted points** when Blazor scenarios are sampled.

#### P2. Add a dedicated EF Core advanced net11 skill
- **Type**: New skill
- **Dimensions affected**: EF Core RemoveDbContext, EF Core ExcludeForeignKeyFromMigrations, EF Core JSON Query Functions, Overall adoption.
- **Problem**: EF Core dimensions are partially modernized (`3`, `2`, `2`), with explicit old-pattern evidence (`HasAnnotation("Relational:ForeignKeyIsExcludedFromMigrations", true)` and `EF.Functions.Like(...)`) in `reports/analysis.md` lines 758-767 and 783-790.
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\README.md`):
  ```md
  ## Included skills
  - `net11-features`
  - `net11-bcl-core`
  - `net11-json-webapi`
  - `net11-csharp15`
  - `net11-focus-dimensions`
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\README.md`):
  ```md
  ## Included skills
  - `net11-features`
  - `net11-bcl-core`
  - `net11-json-webapi`
  - `net11-blazor-ui`
  - `net11-efcore-advanced`
  - `net11-csharp15`
  - `net11-focus-dimensions`
  ```

  **New File** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-efcore-advanced\SKILL.md`):
  ```md
  ---
  name: net11-efcore-advanced
  description: Generate EF Core net11.0 demos using GetEntriesForState, RemoveDbContext, ExcludeForeignKeyFromMigrations, and EF JSON query functions. Use when building EF Core data/model workflows.
  ---
  
  # .NET 11 EF Core Advanced Workflow
  
  1. Use state-based change tracking via `GetEntriesForState(...)`.
     Success: no fallback to `Entries().Where(...)` for the same use case.
  
  2. Use framework removal API for context replacement.
     Success: code uses `RemoveDbContext<TContext>()` and re-registers cleanly.
  
  3. Use dedicated FK migration exclusion API.
     Success: code uses `.ExcludeForeignKeyFromMigrations(true)`; no annotation-string fallback.
  
  4. Use built-in JSON query functions.
     Success: includes `EF.Functions.JsonContains(...)` or `EF.Functions.JsonPathExists(...)` in executable query code.
  
  5. If provider limitations exist, keep primary demo on provider supporting these APIs and add a concise note.
     Success: generated project still demonstrates real API invocations, not text-only mentions.
  ```

- **Expected impact**: Medium-high, about **+10 to +18 weighted points** on EF-heavy runs.

#### P3. Expand guardrails (hook script + reviewer agent) to full rubric coverage
- **Type**: New hook behavior + Agent enhancement
- **Dimensions affected**: Most trailing dimensions, especially CRITICAL/HIGH.
- **Problem**: Existing guard script only checks a narrow subset and misses many scored dimensions (OpenAPI 3.2, output cache provider, Blazor APIs, EF JSON funcs, etc.). Existing reviewer agent scope is also limited to priority subset.
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
      # Core/BCL/C#15
      @{ Name = "Missing BFloat16"; Must = "BFloat16"; Scope = "*.cs" },
      @{ Name = "Missing Rune APIs"; Must = "Rune|Contains\(new Rune|IndexOf\(new Rune|Replace\(new Rune"; Scope = "*.cs" },
      @{ Name = "Missing HMAC verify"; Must = "HMACSHA256.Verify|VerifyHmac"; Scope = "*.cs" },
      @{ Name = "Legacy hash+compare"; Ban = "HashData\(|FixedTimeEquals\("; Scope = "*.cs" },
      @{ Name = "Missing FrozenDictionary expression"; Must = "FrozenDictionary<.*>\s+\w+\s*=\s*\["; Scope = "*.cs" },
      @{ Name = "Legacy FrozenDictionary pattern"; Ban = "ToFrozenDictionary\("; Scope = "*.cs" },
      @{ Name = "Missing collection with()"; Must = "with\(capacity:|with\(comparer:"; Scope = "*.cs" },
      @{ Name = "Missing union"; Must = "\bunion\b"; Scope = "*.cs" },
      @{ Name = "Missing DivisionRounding"; Must = "DivisionRounding\."; Scope = "*.cs" },
      @{ Name = "Missing MediaTypeMap"; Must = "MediaTypeMap\.GetMediaType|MediaTypeMap\.GetExtension"; Scope = "*.cs" },
      @{ Name = "Missing Base64 parity APIs"; Must = "Base64\.EncodeToString|Base64\.DecodeFromChars|Base64\.GetEncodedLength"; Scope = "*.cs" },
      @{ Name = "Missing RegexOptions.AnyNewLine"; Must = "RegexOptions\.AnyNewLine"; Scope = "*.cs" },
      @{ Name = "Missing generic Interlocked enum ops"; Must = "Interlocked\.(Or|And)\("; Scope = "*.cs" },
      # Web API / ASP.NET Core
      @{ Name = "Missing OpenAPI 3.2"; Must = "OpenApiSpecVersion\.OpenApi3_2"; Scope = "*.cs" },
      @{ Name = "Missing output cache provider"; Must = "IOutputCachePolicyProvider"; Scope = "*.cs" },
      @{ Name = "Missing native ASP.NET tracing source"; Must = "AddSource\(\""Microsoft\.AspNetCore\""\)"; Scope = "*.cs" },
      @{ Name = "Missing zstd response compression"; Must = "AddResponseCompression|ZstandardCompressionProviderOptions"; Scope = "*.cs" },
      # EF Core
      @{ Name = "Missing ExcludeForeignKeyFromMigrations"; Must = "ExcludeForeignKeyFromMigrations\(true\)"; Scope = "*.cs" },
      @{ Name = "Missing EF JSON query function"; Must = "EF\.Functions\.(JsonContains|JsonPathExists)\("; Scope = "*.cs" },
      @{ Name = "Disallowed not-applicable note"; Ban = "Not applicable"; Scope = "*.md" }
  )
  ```

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\agents\net11-adoption-reviewer.agent.md`):
  ```md
  You are a strict .NET 11 adoption reviewer.
  
  1. Check for required usage of priority APIs: BFloat16, Rune APIs, HMAC verify, FrozenDictionary expressions, with(), union, DivisionRounding, JsonNamingPolicy attribute.
  2. Flag fallback patterns: HashData+FixedTimeEquals, ToFrozenDictionary conversion flow, manual MIME maps, custom PascalCase policy classes.
  3. Produce concise remediation instructions with exact files and replacement snippets.
  4. End with a pass/fail checklist by dimension.
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\agents\net11-adoption-reviewer.agent.md`):
  ```md
  You are a strict .NET 11 adoption reviewer.
  
  1. Evaluate all scored dimensions for the current scenario family (console-bcl, webapi, blazor, efcore), and require at least one concrete API call per required dimension.
  2. Flag fallback patterns and score-limiting substitutions (annotation-string EF flags, LIKE-for-JSON when JSON funcs are available, button-column QuickGrid row click, manual URI concatenation, custom policy classes for PascalCase, etc.).
  3. Produce file-level remediation with before/after snippets that compile.
  4. End with a pass/fail checklist covering every dimension in scope and a weighted-risk summary (CRITICAL first).
  ```

- **Expected impact**: High for consistency and prevention; likely **+20 to +35 weighted points** by reducing missed dimensions before final output.

### Skill-Level Improvements

### S1. Upgrade `net11-focus-dimensions` into a complete skill package
- **Dimensions affected**: Cross-cutting (especially CRITICAL/HIGH dimensions with score `1`).
- **Current score → Target score**: Many `1` → `3-4`.
- **Problem**: The skill exists but has no `reference/` or `examples/` directories, and instructions are broad.
- **Root cause**: Missing supporting artifacts weakens activation and leaves too much interpretation for model output.
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-focus-dimensions\SKILL.md`):
  ```md
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
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-focus-dimensions\SKILL.md`):
  ```md
  # .NET 11 Priority Dimension Enforcement
  
  1. Build a dimension matrix first (`Dimension`, `Required API`, `File`).
     Success: matrix includes every priority dimension before code generation starts.
  
  2. For each missing dimension, add one focused helper in `FeatureCoverage/` and wire it into runtime entry points.
     Success: every helper is actually executed by endpoint/command/test; no “dead demo” files.
  
  3. Use only required APIs from `reference/priority-dimensions-matrix.md` and forbid fallback patterns listed there.
     Success: each matrix row points to an exact symbol used in code.
  
  4. Emit a final checklist grouped by CRITICAL/HIGH/MEDIUM/LOW with file paths.
     Success: checklist has zero missing rows and no “not applicable” for priority dimensions.
  
  Example:
  Input: "Create EF Core order demo."
  Output: EF demo + `FeatureCoverage` helpers for missing BCL/Web/Blazor priority dimensions.
  ```

  **New File** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-focus-dimensions\reference\priority-dimensions-matrix.md`):
  ```md
  # Priority Dimensions Matrix
  
  | Dimension | Required API | Forbidden fallback |
  |---|---|---|
  | BFloat16 | `System.Numerics.BFloat16` | manual bit packing |
  | Rune operations | `string.Contains/IndexOf/Replace/Split(Rune)` | surrogate-pair manual logic |
  | HMAC verify | `HMACSHA256.Verify` / `VerifyHmac` | `HashData` + `FixedTimeEquals` |
  | FrozenDictionary | `FrozenDictionary<K,V> map = ["k": v]` | `Dictionary` + `ToFrozenDictionary()` |
  | Collection with() | `[with(capacity: n), ..values]` | constructor + add loop |
  | Union | `union` + exhaustive `switch` | abstract-class hierarchy for simple DU |
  | DivisionRounding | `int.Divide/DivRem(..., DivisionRounding.*)` | manual remainder rounding |
  | JSON | `JsonNamingPolicy.PascalCase`, `[JsonNamingPolicy]`, `GetTypeInfo<T>()` | custom PascalCase policy class |
  ```

  **New File** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-focus-dimensions\examples\focus-dimensions-complete.md`):
  ```md
  ~~~csharp
  // FeatureCoverage/Net11PriorityApiDemo.cs
  FrozenDictionary<string,int> map = ["ok": 200];
  var pages = int.Divide(totalItems, pageSize, DivisionRounding.ToPositiveInfinity);
  var hasRocket = text.Contains(new Rune(0x1F680));
  var verified = HMACSHA256.Verify(key, data, mac);
  ~~~
  ```

- **Expected impact**: Medium-high; improved instruction quality and support files should raise coverage reliability and reduce `1` scores.

### S2. Tighten `net11-json-webapi` to force executable API usage
- **Dimensions affected**: System.Text.Json New Features, Native OpenTelemetry Tracing, OpenAPI Version, Dynamic Output Cache Policy Provider, Zstandard Response Compression, OpenAPI Binary File Response.
- **Current score → Target score**: `1` → `4`.
- **Problem**: Current run had no Web API generation and no OpenAPI/tracing/cache evidence (`reports/analysis.md`, lines 527-535, 547-554, 567-574, 892-899).
- **Root cause**: SKILL instructions are clear but still allow non-executable mention-style coverage and don’t prescribe a minimal startup template for guaranteed inclusion.
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-json-webapi\SKILL.md`):
  ```md
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
  2. Add a minimal executable startup slice (Program.cs) containing all required APIs:
     - `OpenApiSpecVersion.OpenApi3_2`
     - `builder.Services.AddOpenTelemetry().WithTracing(t => t.AddSource("Microsoft.AspNetCore"))`
     - `builder.Services.AddSingleton<IOutputCachePolicyProvider, AppOutputCachePolicyProvider>()`
     - `builder.Services.AddResponseCompression();` + `Configure<ZstandardCompressionProviderOptions>(...)`
     Success: all symbols appear in runnable startup code (not comments/text output).
  
  3. Add one binary endpoint with explicit OpenAPI metadata:
     - `.MapGet("/download", ...).Produces<FileContentResult>(contentType: "application/octet-stream")`
     Success: OpenAPI output includes binary response metadata for that route.
  
  4. If the primary scenario is not Web API, generate `FeatureCoverage/Net11WebApiProbe.cs` with the same API calls and wire it via tests or startup.
     Success: dimensions are still demonstrated in executable code.
  ```

- **Expected impact**: High for Web API scenarios; likely **+25 to +35 weighted points** in those runs.

### S3. Expand `net11-features` from “priority list” to scenario matrix execution
- **Dimensions affected**: Overall .NET 11 API Adoption Rate plus many dimension-specific scores.
- **Current score → Target score**: Overall adoption `1` → `3-4`.
- **Problem**: Current guidance focuses mostly on priority APIs but does not map required APIs by scenario family (console/webapi/blazor/efcore), which contributed to broad missing coverage when only `efcore` was generated.
- **Root cause**: Missing scenario-to-dimension execution contract in the skill body.
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-features\SKILL.md`):
  ```md
  1. Confirm applicability and enforce priority coverage.
     - Apply this guidance only when target is `net11.0`.
     - For the priority dimensions, never output "not applicable" without adding a minimal demo helper.
     - If the main scenario does not naturally use a priority API, create `FeatureCoverage/Net11PriorityApiDemo.cs` and wire it into startup/tests.
     Success: every priority dimension is represented by at least one concrete API use in code.
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-features\SKILL.md`):
  ```md
  1. Confirm applicability and build a scenario matrix.
     - Apply this guidance only when target is `net11.0`.
     - Build a matrix of `Scenario family -> required dimensions -> file path`.
     - Scenario families: `console-bcl`, `webapi`, `blazor`, `efcore`.
     Success: every required dimension in the active family has a planned file target before coding begins.
  
  2. Enforce runnable coverage, not mention-only coverage.
     - Never output "not applicable" for required dimensions.
     - If a dimension is outside the main architecture, place it in `FeatureCoverage/*Probe.cs` and execute via startup/tests.
     Success: each dimension has an executable symbol reference and invocation path.
  
  3. Emit a final matrix-backed checklist.
     Success: checklist entries map 1:1 to generated files and API symbols.
  ```

- **Expected impact**: High cross-scenario reliability; expected **+20+ weighted points** over multiple random scenario runs.

## Summary of Recommended Changes
### Modified files
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\README.md`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\scripts\net11-pattern-guard.ps1`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\agents\net11-adoption-reviewer.agent.md`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-focus-dimensions\SKILL.md`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-json-webapi\SKILL.md`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-features\SKILL.md`

### New files
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-blazor-ui\SKILL.md`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-efcore-advanced\SKILL.md`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-focus-dimensions\reference\priority-dimensions-matrix.md`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-focus-dimensions\examples\focus-dimensions-complete.md`

## Risks and Trade-offs
- Stronger “must-have dimension” guardrails can increase false positives for narrowly scoped tasks; mitigate by making checks scenario-aware.
- Adding multiple focused skills improves activation precision but can increase selection complexity if descriptions overlap; keep descriptions trigger-specific.
- Forcing cross-scenario probe modules can improve scores while risking architectural noise; keep probe code isolated under `FeatureCoverage/` and clearly documented.
