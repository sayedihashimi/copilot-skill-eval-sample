# Improvement Suggestions: dotnet-net11-skill

## Executive Summary
`dotnet-net11-skill` scored **159.0/297.5 (53%)**, with strong Web API adoption (OpenAPI 3.2, dynamic output cache provider, Zstandard compression) but major coverage gaps in Blazor/EF/console-focused dimensions and only partial native tracing quality (**4/5**). The largest root cause is structural: guidance exists, but activation/coverage enforcement is not reliably translating into generated artifacts across all surfaces.

Top opportunities are: **(1)** add missing reference/examples for `net11-aspnet-observability` and `net11-blazor-ui`, **(2)** strengthen hook guardrails for the five priority dimensions, and **(3)** tighten SKILL instructions to force scenario-complete, compilable demos when a feature is otherwise absent.

**Comparative note:** this evaluation includes only one configuration, so baseline/top-config deltas are unavailable in this run. Recommendations are therefore tied to absolute score gaps and stated plugin purpose.

## Current Performance Snapshot

| Dimension | Score | Gap to 5 | Status |
|---|---:|---:|---|
| Zstandard Compression Usage | 5.0 | 0.0 | ✅ |
| BFloat16 Type Usage | 4.0 | 1.0 | ⚠️ |
| Rune-Based String Operations | 5.0 | 0.0 | ✅ |
| HMAC Single-Step Verification | 5.0 | 0.0 | ✅ |
| FrozenDictionary Collection Expressions | 3.0 | 2.0 | ⚠️ |
| Collection Expression with() Arguments | 5.0 | 0.0 | ✅ |
| Union Type Usage | 2.0 | 3.0 | ❌ |
| MediaTypeMap Usage | 5.0 | 0.0 | ✅ |
| DivisionRounding Modes | 2.0 | 3.0 | ❌ |
| System.Text.Json New Features | 5.0 | 0.0 | ✅ |
| RegexOptions.AnyNewLine | 1.0 | 4.0 | ❌ |
| File System New APIs | 1.0 | 4.0 | ❌ |
| Base64 Parity APIs | 1.0 | 4.0 | ❌ |
| Generic Interlocked Operations | 1.0 | 4.0 | ❌ |
| BitArray.PopCount | 1.0 | 4.0 | ❌ |
| Native OpenTelemetry Tracing | 4.0 | 1.0 | ⚠️ |
| OpenAPI Version | 5.0 | 0.0 | ✅ |
| Dynamic Output Cache Policy Provider | 5.0 | 0.0 | ✅ |
| Zstandard Response Compression | 5.0 | 0.0 | ✅ |
| Blazor EnvironmentBoundary Component | 1.0 | 4.0 | ❌ |
| Blazor Label and DisplayName Components | 1.0 | 4.0 | ❌ |
| QuickGrid OnRowClick | 1.0 | 4.0 | ❌ |
| RelativeToCurrentUri Navigation | 1.0 | 4.0 | ❌ |
| Blazor TempData Support | 1.0 | 4.0 | ❌ |
| Blazor BasePath Component | 1.0 | 4.0 | ❌ |
| EF Core GetEntriesForState | 1.0 | 4.0 | ❌ |
| EF Core RemoveDbContext | 1.0 | 4.0 | ❌ |
| EF Core ExcludeForeignKeyFromMigrations | 1.0 | 4.0 | ❌ |
| EF Core JSON Query Functions | 1.0 | 4.0 | ❌ |
| SignalR ConfigureConnection | 1.0 | 4.0 | ❌ |
| Blazor Virtualize Variable-Height Items | 1.0 | 4.0 | ❌ |
| Runtime Async Configuration | 1.0 | 4.0 | ❌ |
| ProcessExitStatus Usage | 1.0 | 4.0 | ❌ |
| OpenAPI Binary File Response | 5.0 | 0.0 | ✅ |
| Brotli and Compression Options | 2.0 | 3.0 | ❌ |
| Vector Constants | 1.0 | 4.0 | ❌ |
| Overall .NET 11 API Adoption Rate | 2.0 | 3.0 | ❌ |

## Plugin Structure Assessment

### Plugin: `dotnet-net11`
- **Inventory:** 7 skills, 1 agent, hooks enabled, no MCP/LSP.
- **Manifest quality:** Strong (`name`, `version`, clear long description, rich keywords).
- **Key structural gaps:**
  1. `net11-aspnet-observability` has no `reference/` or `examples/`.
  2. `net11-blazor-ui` has no `reference/` or `examples/`.
  3. Hook guard script is global and binary (must-match patterns) but does not enforce the **specific anti-patterns** that caused priority-dimension deductions (e.g., external ASP.NET instrumentation package usage).
  4. Single reviewer agent exists; no dedicated scenario-completeness agent to force missing surface generation (Blazor/EF/console).

## Improvement Suggestions

### Plugin-Level Improvements

#### P1. Add missing reference and golden examples for priority-dimension skills
- **Type**: Documentation + supporting files
- **Dimensions affected**: Native OpenTelemetry Tracing, OpenAPI Version, Dynamic Output Cache Policy Provider, Zstandard Response Compression, Blazor EnvironmentBoundary Component
- **Problem**: Two core skills (`net11-aspnet-observability`, `net11-blazor-ui`) lack `reference/` and `examples/`, reducing activation quality and concrete output guidance.
- **Suggested changes**:

**New File** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-aspnet-observability\reference\aspnet-observability-net11.md`):
```md
# .NET 11 ASP.NET Observability + Pipeline Reference

Required startup shape:

```csharp
builder.Services.AddOpenApi(o => o.OpenApiVersion = OpenApiSpecVersion.OpenApi3_2);

builder.Services.AddOpenTelemetry()
    .WithTracing(t => t.AddSource("Microsoft.AspNetCore"));

builder.Services.AddOutputCache();
builder.Services.AddSingleton<IOutputCachePolicyProvider, CatalogOutputCachePolicyProvider>();

builder.Services.AddResponseCompression(o =>
{
    o.Providers.Add<ZstandardCompressionProvider>();
});
builder.Services.Configure<ZstandardCompressionProviderOptions>(o =>
{
    o.CompressionOptions = new ZstandardCompressionOptions { Quality = 3 };
});
```

Hard bans:
- `AddAspNetCoreInstrumentation()`
- OpenAPI versions lower than `OpenApi3_2`
- static-only output cache policy registration
- Brotli/Gzip-only compression setup for scenarios requiring zstd
```

**New File** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-aspnet-observability\examples\webapi-observability-golden.md`):
```md
```csharp
// Program.cs
builder.Services.AddOpenApi(o => o.OpenApiVersion = OpenApiSpecVersion.OpenApi3_2);
builder.Services.AddOpenTelemetry().WithTracing(t => t.AddSource("Microsoft.AspNetCore"));
builder.Services.AddOutputCache();
builder.Services.AddSingleton<IOutputCachePolicyProvider, CatalogOutputCachePolicyProvider>();
builder.Services.AddResponseCompression(o => o.Providers.Add<ZstandardCompressionProvider>());
builder.Services.Configure<ZstandardCompressionProviderOptions>(o =>
    o.CompressionOptions = new ZstandardCompressionOptions { Quality = 3 });
```
```

**New File** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-blazor-ui\reference\blazor-ui-net11.md`):
```md
# .NET 11 Blazor UI Reference

Priority requirement:
- Always include at least one `<EnvironmentBoundary Include/Exclude>` example in a compilable `.razor` file.

Core patterns:
- `<Label For>` and `<DisplayName For>`
- `QuickGrid` with `OnRowClick`
- `NavigationOptions.RelativeToCurrentUri`
- `[CascadingParameter] ITempData`
- `<BasePath />`
- `<Virtualize OverscanCount="15">` for variable-height lists
- `AddInteractiveServerRenderMode(o => o.ConfigureConnection(...))`
```

**New File** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-blazor-ui\examples\blazor-ui-golden.md`):
```md
```razor
<EnvironmentBoundary Include="Development">
    <Label For="() => Model.Title" />
    <InputText @bind-Value="Model.Title" />
</EnvironmentBoundary>

<QuickGrid Items="Rows" OnRowClick="OnRowClick">
    <PropertyColumn Property="x => x.Title" Title="@<DisplayName For='() => Model.Title' />" />
</QuickGrid>
```
```

- **Expected impact**: Better deterministic generation for priority surfaces; estimated **+8 to +18 weighted points**, primarily HIGH-tier dimensions.

#### P2. Make hook guardrails scenario-aware and priority-dimension specific
- **Type**: Hook improvement
- **Dimensions affected**: Native OpenTelemetry Tracing, OpenAPI Version, Dynamic Output Cache Policy Provider, Zstandard Response Compression, Blazor EnvironmentBoundary Component
- **Problem**: Current hook runs global generic checks; it does not explicitly block the observed tracing downgrade pattern nor verify exact focus-dimension patterns per scenario.
- **Suggested changes**:

**Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\hooks\hooks.json`):
```json
{
  "hooks": {
    "SessionStart": [
      {
        "hooks": [
          {
            "type": "command",
            "command": "pwsh -NoProfile -Command \"Write-Host '[dotnet-net11] Priority dimensions enforced: BFloat16, Rune, HMAC.Verify, FrozenDictionary expr, with(), union, DivisionRounding, JsonNamingPolicy, OpenAPI3.2, ASP.NET native tracing, output-cache provider, zstd HTTP compression, EnvironmentBoundary.'\""
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
            "command": "pwsh -NoProfile -Command \"Write-Host '[dotnet-net11] Enforcing focus dimensions: AddSource(Microsoft.AspNetCore), OpenAPI3.2, IOutputCachePolicyProvider, zstd HTTP compression, EnvironmentBoundary.'\""
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
            "command": "pwsh -NoProfile -File \"${CLAUDE_PLUGIN_ROOT}\\scripts\\net11-pattern-guard.ps1\" -Mode FocusDimensions"
          }
        ]
      }
    ]
  }
}
```

**Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\scripts\net11-pattern-guard.ps1`):
```powershell
$checks = @(
    @{ Name = "Missing native ASP.NET tracing source"; Must = "AddSource\(""Microsoft\.AspNetCore""\)"; Scope = "*.cs" },
    @{ Name = "Missing output cache provider"; Must = "IOutputCachePolicyProvider"; Scope = "*.cs" },
    @{ Name = "Missing zstd HTTP compression"; Must = "ZstandardCompressionProviderOptions|Zstandard"; Scope = "*.cs" },
    @{ Name = "Missing EnvironmentBoundary"; Must = "<EnvironmentBoundary"; Scope = "*.razor" }
)
```

**After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\scripts\net11-pattern-guard.ps1`):
```powershell
param(
    [string]$RepoRoot = (Get-Location).Path,
    [ValidateSet("Full","FocusDimensions")] [string]$Mode = "Full"
)

$focusChecks = @(
    @{ Name = "Missing native ASP.NET tracing source"; Must = 'AddSource\("Microsoft\.AspNetCore"\)'; Scope = "*.cs" },
    @{ Name = "Disallowed ASP.NET instrumentation package usage"; Ban = "AddAspNetCoreInstrumentation\("; Scope = "*.cs" },
    @{ Name = "Missing OpenAPI 3.2"; Must = "OpenApiSpecVersion\.OpenApi3_2"; Scope = "*.cs" },
    @{ Name = "Missing output cache provider"; Must = "IOutputCachePolicyProvider"; Scope = "*.cs" },
    @{ Name = "Missing zstd HTTP compression"; Must = "ZstandardCompressionProviderOptions|ZstandardCompressionProvider"; Scope = "*.cs" },
    @{ Name = "Missing EnvironmentBoundary"; Must = "<EnvironmentBoundary"; Scope = "*.razor" }
)

$checks = if ($Mode -eq "FocusDimensions") { $focusChecks } else { $focusChecks + $fullChecks }
```

- **Expected impact**: closes the remaining tracing quality gap and reduces false-positive “covered but wrong pattern” output; estimated **+2 to +6 weighted points**.

#### P3. Add a scenario-completeness reviewer agent
- **Type**: New agent
- **Dimensions affected**: Blazor EnvironmentBoundary Component, Overall .NET 11 API Adoption Rate (plus all missing Blazor/EF dimensions)
- **Problem**: Current reviewer is adoption-focused but not strict enough about missing scenario surfaces; run output lacked Blazor/EF implementations and scored many dimensions at 1.
- **Suggested changes**:

**New File** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\agents\net11-scenario-completeness.agent.md`):
```md
---
name: net11-scenario-completeness
description: Ensures generated net11 output includes all required scenario surfaces or compilable FeatureCoverage fallbacks for missing dimensions.
model: claude-sonnet-4.5
maxTurns: 8
tools: [Read, Grep, Glob]
---

You enforce scenario completeness before finalizing.

1. Detect expected surfaces: console-bcl, webapi, blazor, efcore.
2. For any missing surface, require a compilable `FeatureCoverage/` substitute that demonstrates the missing APIs.
3. For focus dimensions, require exact symbols:
   - `AddSource("Microsoft.AspNetCore")`
   - `OpenApiSpecVersion.OpenApi3_2`
   - `IOutputCachePolicyProvider`
   - `ZstandardCompressionProviderOptions` or `ZstandardCompressionProvider`
   - `<EnvironmentBoundary ...>`
4. Reject “not applicable” without concrete replacement implementation.
5. Output pass/fail checklist mapped by dimension -> file -> symbol.
```

- **Expected impact**: biggest lever on low-scoring dimensions by forcing complete coverage behavior; estimated **+20 to +45 weighted points**.

### Skill-Level Improvements

### S1. Tighten native tracing + startup focus patterns in `net11-json-webapi`
- **Dimensions affected**: Native OpenTelemetry Tracing, OpenAPI Version, Dynamic Output Cache Policy Provider, Zstandard Response Compression
- **Current score → Target score**: 4.0 → 5.0 (tracing), maintain 5.0 on the other three
- **Problem**: Generated output still looked “package-heavy” for tracing quality despite correct `AddSource`.
- **Root cause**: The skill requires startup patterns but does not explicitly ban common fallback instrumentation calls at code level.
- **Suggested changes**:

**Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-json-webapi\SKILL.md`):
```md
2. Program.cs must include these exact startup patterns:
   - `builder.Services.AddOpenApi(... OpenApiSpecVersion.OpenApi3_2 ...)`
   - `builder.Services.AddOpenTelemetry().WithTracing(t => t.AddSource("Microsoft.AspNetCore"))`
   - `builder.Services.AddSingleton<IOutputCachePolicyProvider, <YourProvider>>()`
   Success: all three patterns appear verbatim in startup code.
```

**After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-json-webapi\SKILL.md`):
```md
2. Program.cs must include these exact startup patterns and bans:
   - `builder.Services.AddOpenApi(... OpenApiSpecVersion.OpenApi3_2 ...)`
   - `builder.Services.AddOpenTelemetry().WithTracing(t => t.AddSource("Microsoft.AspNetCore"))`
   - `builder.Services.AddSingleton<IOutputCachePolicyProvider, <YourProvider>>()`
   - ban `AddAspNetCoreInstrumentation(...)` for this workflow
   Success: all required patterns appear and banned pattern does not appear.
```

- **Expected impact**: removes residual tracing-quality deduction and reduces regressions in startup wiring.

### S2. Make EnvironmentBoundary coverage mandatory and compilable in `net11-blazor-ui`
- **Dimensions affected**: Blazor EnvironmentBoundary Component (priority), plus Label/DisplayName, QuickGrid OnRowClick, TempData, BasePath, SignalR ConfigureConnection, Virtualize
- **Current score → Target score**: 1.0 → 4.0–5.0
- **Problem**: Blazor dimensions scored 1 because no Blazor surface appeared in generated artifacts.
- **Root cause**: Skill guidance is descriptive but does not require a concrete fallback artifact when the main scenario is not a Blazor app.
- **Suggested changes**:

**Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-blazor-ui\SKILL.md`):
```md
1. Use declarative environment gating with `<EnvironmentBoundary>`.
   Success: no manual `IHostEnvironment` branching for environment-specific rendering.
```

**After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-blazor-ui\SKILL.md`):
```md
1. Use declarative environment gating with `<EnvironmentBoundary>`.
   - If the primary app is not Blazor, create `FeatureCoverage/BlazorEnvironmentBoundaryDemo.razor` in a minimal compilable Blazor/Razor component project.
   - Reference that component from at least one page/test to prevent dead-code omission.
   Success: generated output always contains at least one compiled `.razor` file with `<EnvironmentBoundary Include/Exclude>`.
```

- **Expected impact**: directly addresses the largest focus-dimension miss; strong HIGH-tier score recovery.

### S3. Strengthen explicit “no omission” requirement in `net11-focus-dimensions`
- **Dimensions affected**: Blazor EnvironmentBoundary Component, Overall .NET 11 API Adoption Rate, and all scenario-missing dimensions
- **Current score → Target score**: 2.0 → 3.5–4.5 (overall adoption)
- **Problem**: Many dimensions stayed at 1 because missing surfaces were not replaced by concrete demos.
- **Root cause**: The skill says to add `FeatureCoverage/`, but does not require each demo to be **compiled and referenced** by build graph with explicit project-path expectations.
- **Suggested changes**:

**Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-focus-dimensions\SKILL.md`):
```md
2. For every missing priority dimension, add a minimal demo module under `FeatureCoverage/`
   that compiles and is invoked by at least one endpoint, command, or test.
   Success: no priority dimension is left as "not applicable".
```

**After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-focus-dimensions\SKILL.md`):
```md
2. For every missing priority dimension, add a minimal demo module under `FeatureCoverage/` that is part of the solution build graph.
   - Required: each module must be referenced by an endpoint, startup registration, command handler, or test assertion.
   - Required output section: `Dimension -> File -> Symbol -> Invocation point`.
   Success: no priority dimension is left as "not applicable", and every demo is build-reachable.
```

- **Expected impact**: prevents “present in prose, absent in artifact” failures and raises overall adoption consistency.

### S4. Add explicit anti-regression checks in `net11-aspnet-observability`
- **Dimensions affected**: Native OpenTelemetry Tracing, OpenAPI Version, Dynamic Output Cache Policy Provider, Zstandard Response Compression
- **Current score → Target score**: 4.0/5.0/5.0/5.0 → sustained 5.0s
- **Problem**: Focus dimensions are instructed, but failure modes (wrong tracing instrumentation style) are not codified as banned output patterns.
- **Root cause**: “Do not add external instrumentation packages” is present, but no concrete anti-pattern list or final checklist row for each focus dimension.
- **Suggested changes**:

**Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-aspnet-observability\SKILL.md`):
```md
1. Configure native tracing only.
   - Register OpenTelemetry tracing with `AddSource("Microsoft.AspNetCore")`.
   - Do not add external ASP.NET instrumentation packages.
   Success: startup code contains `AddSource("Microsoft.AspNetCore")` and no external instrumentation package usage.
```

**After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-aspnet-observability\SKILL.md`):
```md
1. Configure native tracing only.
   - Register tracing with `AddSource("Microsoft.AspNetCore")`.
   - Ban: `AddAspNetCoreInstrumentation(...)` and package references that only provide legacy ASP.NET instrumentation.
   - Emit final checklist row: `Native tracing -> Program.cs -> AddSource("Microsoft.AspNetCore")`.
   Success: required symbol exists and banned tracing calls do not exist.
```

- **Expected impact**: closes remaining tracing gap and protects already-strong focus dimensions from regression.

## Summary of Recommended Changes

### New files
1. `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-aspnet-observability\reference\aspnet-observability-net11.md`
2. `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-aspnet-observability\examples\webapi-observability-golden.md`
3. `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-blazor-ui\reference\blazor-ui-net11.md`
4. `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-blazor-ui\examples\blazor-ui-golden.md`
5. `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\agents\net11-scenario-completeness.agent.md`

### Modified files
1. `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\hooks\hooks.json`
2. `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\scripts\net11-pattern-guard.ps1`
3. `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-json-webapi\SKILL.md`
4. `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-blazor-ui\SKILL.md`
5. `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-focus-dimensions\SKILL.md`
6. `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-aspnet-observability\SKILL.md`

## Risks and Trade-offs
- Enforcing all priority dimensions in every run can increase token usage and code volume; mitigate with isolated, minimal `FeatureCoverage/` modules.
- Stronger hook checks can create false positives if scenario context is ignored; use the proposed scenario-aware mode.
- Adding mandatory fallback demos may slightly reduce architectural purity for narrow tasks, but it materially improves measurable adoption coverage in this evaluation framework.
