# Improvement Suggestions: dotnet-net11-skill

## Executive Summary
`dotnet-net11-skill` scored **119.0 / 297.5 (40%)**, with strong Web API coverage (JSON/OpenAPI/output cache/zstd) but major misses in cross-scenario coverage and build reliability (only **1/4** scenarios built). The highest-impact gaps are not missing intent, but **enforcement and activation**: the plugin contains broad guidance, yet evaluation output shows only webapi was implemented and build commands failed for console/blazor/efcore due project-path handling.

Top improvement opportunities are mostly **plugin-level guardrails** (activation, pre-stop validation, explicit build orchestration), then **skill-level precision fixes** (path-safe build commands, removal of conflicting/ambiguous API examples, and tighter required evidence).

> Note: This run includes only one configuration, so baseline-vs-top comparisons are not available in the provided analysis artifacts.

## Current Performance Snapshot
| Dimension | Tier | Score | Gap to 5 |
|---|---|---:|---:|
| Zstandard Compression Usage | CRITICAL | 4.0 | 1.0 |
| BFloat16 Type Usage | HIGH | 1.0 | 4.0 |
| Rune-Based String Operations | HIGH | 1.0 | 4.0 |
| HMAC Single-Step Verification | HIGH | 1.0 | 4.0 |
| FrozenDictionary Collection Expressions | HIGH | 1.0 | 4.0 |
| Collection Expression with() Arguments | HIGH | 1.0 | 4.0 |
| Union Type Usage | CRITICAL | 1.0 | 4.0 |
| MediaTypeMap Usage | MEDIUM | 5.0 | 0.0 |
| DivisionRounding Modes | MEDIUM | 1.0 | 4.0 |
| System.Text.Json New Features | CRITICAL | 5.0 | 0.0 |
| RegexOptions.AnyNewLine | MEDIUM | 1.0 | 4.0 |
| File System New APIs | HIGH | 1.0 | 4.0 |
| Base64 Parity APIs | MEDIUM | 2.0 | 3.0 |
| Generic Interlocked Operations | MEDIUM | 1.0 | 4.0 |
| BitArray.PopCount | LOW | 1.0 | 4.0 |
| Native OpenTelemetry Tracing | HIGH | 4.0 | 1.0 |
| OpenAPI Version | MEDIUM | 5.0 | 0.0 |
| Dynamic Output Cache Policy Provider | HIGH | 5.0 | 0.0 |
| Zstandard Response Compression | HIGH | 5.0 | 0.0 |
| Blazor EnvironmentBoundary Component | HIGH | 1.0 | 4.0 |
| Blazor Label and DisplayName Components | HIGH | 1.0 | 4.0 |
| QuickGrid OnRowClick | HIGH | 1.0 | 4.0 |
| RelativeToCurrentUri Navigation | MEDIUM | 1.0 | 4.0 |
| Blazor TempData Support | HIGH | 1.0 | 4.0 |
| Blazor BasePath Component | MEDIUM | 1.0 | 4.0 |
| EF Core GetEntriesForState | HIGH | 1.0 | 4.0 |
| EF Core RemoveDbContext | HIGH | 1.0 | 4.0 |
| EF Core ExcludeForeignKeyFromMigrations | MEDIUM | 1.0 | 4.0 |
| EF Core JSON Query Functions | HIGH | 1.0 | 4.0 |
| SignalR ConfigureConnection | MEDIUM | 1.0 | 4.0 |
| Blazor Virtualize Variable-Height Items | MEDIUM | 1.0 | 4.0 |
| Runtime Async Configuration | MEDIUM | 1.0 | 4.0 |
| ProcessExitStatus Usage | MEDIUM | 1.0 | 4.0 |
| OpenAPI Binary File Response | MEDIUM | 5.0 | 0.0 |
| Brotli and Compression Options | LOW | 2.0 | 3.0 |
| Vector Constants | LOW | 1.0 | 4.0 |
| Overall .NET 11 API Adoption Rate | CRITICAL | 2.0 | 3.0 |

## Plugin Structure Assessment
### Plugin: `dotnet-net11`
- **Inventory**: 7 skills, 3 agents, hooks configured, no MCP/LSP, README+CHANGELOG+LICENSE present.
- **Manifest quality**: good (`name`, `description`, `version`, `keywords` all present and useful).
- **Key structural gaps**:
  1. No single “entry” skill/command that deterministically orchestrates all sub-skills in evaluation prompts.
  2. `net11-eval-orchestrator` has no `reference/` or `examples/` despite being central to cross-scenario success.
  3. Hooks validate patterns but do not enforce **artifact-level completion** (evidence table + all-domain build pass) before finish.
  4. Build guard scripts rely on broad root scanning and do not force deterministic per-domain build commands with explicit project paths.

## Improvement Suggestions

### Plugin-Level Improvements

#### P1. Add a deterministic evaluation entry skill and wire it in README
- **Type**: New skill + Documentation
- **Dimensions affected**: Overall .NET 11 API Adoption Rate, all missing Blazor/EF/runtime/BCL dimensions, build pass rate
- **Problem**: Current plugin has many specialized skills, but no single activation path that forces orchestrated full-matrix execution.
- **Current behavior evidence**: Verification data reports `loaded_skills: []`, and only webapi built; console/blazor/efcore failed with MSB1003.
- **Suggested changes**:

  **New File** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-eval-entry\SKILL.md`):
  ```md
  ---
  name: net11-eval-entry
  description: Orchestrate full .NET 11 evaluation output across console-bcl, webapi, blazor, and efcore with explicit build gates and evidence mapping. Use when prompts mention evaluation, scorecards, feature adoption, or multi-scenario .NET 11 coverage.
  ---
  
  # .NET 11 Evaluation Entry Workflow
  
  1. Create or verify all required domain roots before coding:
     - `console-bcl/`
     - `webapi/`
     - `blazor/`
     - `efcore/`
     Success: each domain has a `.csproj`.
  
  2. Delegate implementation to focused skills:
     - BCL/runtime + C# 15: `net11-bcl-core`, `net11-csharp15`
     - Web API + JSON: `net11-json-webapi`
     - Cross-domain scaffolding + checklist closure: `net11-cross-scenario-coverage`, `net11-focus-dimensions`
     Success: every scored dimension has at least one concrete API symbol in code.
  
  3. Build using explicit project paths, never current-directory inference.
     - Run: `dotnet build <absolute-or-repo-relative-csproj-path>` for every generated project.
     Success: all projects pass; no MSB1003.
  
  4. Emit final evidence table in exact format:
     `Dimension | File | API Symbol | Build Status`
     Success: 37 rows, no empty cells, no "N/A".
  
  5. If any dimension is missing or any build fails, continue editing until all pass.
  
  Example:
  Input: "Create a .NET 11 feature-adoption evaluation output."
  Output: four domain projects + full evidence table + successful builds for every `.csproj`.
  ```

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\README.md`):
  ```md
  ## Included skills
  - `net11-features`
  - `net11-bcl-core`
  - `net11-json-webapi`
  - `net11-csharp15`
  - `net11-focus-dimensions`
  - `net11-cross-scenario-coverage`
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\README.md`):
  ```md
  ## Included skills
  - `net11-eval-entry` (primary entrypoint for evaluation/scorecard prompts)
  - `net11-features`
  - `net11-bcl-core`
  - `net11-json-webapi`
  - `net11-csharp15`
  - `net11-focus-dimensions`
  - `net11-cross-scenario-coverage`
  - `net11-eval-orchestrator`
  ```

- **Expected impact**: **+40 to +90 weighted points** (CRITICAL tier), mostly by preventing single-scenario partial outputs.

#### P2. Add pre-stop guard hook to enforce evidence-table completeness and explicit domain build status
- **Type**: New hook + New script
- **Dimensions affected**: Overall .NET 11 API Adoption Rate, all dimensions currently scoring 1 due missing implementations
- **Problem**: Current hooks check pattern presence but do not verify that all dimensions are accounted for with build status evidence.
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\hooks\hooks.json`):
  ```json
      "Stop": [
        {
          "hooks": [
            {
              "type": "command",
              "command": "pwsh -NoProfile -File \"${CLAUDE_PLUGIN_ROOT}\\scripts\\net11-finalize-check.ps1\" -RepoRoot \"${PWD}\""
            }
          ]
        }
      ]
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\hooks\hooks.json`):
  ```json
      "Stop": [
        {
          "hooks": [
            {
              "type": "command",
              "command": "pwsh -NoProfile -File \"${CLAUDE_PLUGIN_ROOT}\\scripts\\net11-finalize-check.ps1\" -RepoRoot \"${PWD}\""
            },
            {
              "type": "command",
              "command": "pwsh -NoProfile -File \"${CLAUDE_PLUGIN_ROOT}\\scripts\\net11-evidence-guard.ps1\" -RepoRoot \"${PWD}\""
            }
          ]
        }
      ]
  ```

  **New File** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\scripts\net11-evidence-guard.ps1`):
  ```powershell
  param(
      [string]$RepoRoot = (Get-Location).Path
  )
  
  $ErrorActionPreference = "Stop"
  
  $evidenceFiles = @(
      "coverage-checklist.md",
      "feature-evidence.md",
      "reports/feature-evidence.md"
  ) | ForEach-Object { Join-Path $RepoRoot $_ } | Where-Object { Test-Path $_ }
  
  if (-not $evidenceFiles) {
      Write-Error "Missing evidence table file. Expected Dimension | File | API Symbol | Build Status."
      exit 1
  }
  
  $pattern = '^\s*[^|]+\|[^|]+\|[^|]+\|[^|]+\s*$'
  $rowCount = 0
  
  foreach ($file in $evidenceFiles) {
      $lines = Get-Content $file
      $rows = $lines | Where-Object { $_ -match $pattern -and $_ -notmatch '---' -and $_ -notmatch 'Dimension' }
      $rowCount += ($rows | Measure-Object).Count
  }
  
  if ($rowCount -lt 37) {
      Write-Error "Evidence table has only $rowCount rows; expected at least 37."
      exit 1
  }
  ```

- **Expected impact**: **+20 to +60 weighted points** by forcing closure on skipped dimensions before finalization.

#### P3. Add orchestrator reference/examples (currently missing) to improve deterministic execution
- **Type**: Supporting files for existing skill
- **Dimensions affected**: All dimensions with score 1 tied to missing domain implementations
- **Problem**: `net11-eval-orchestrator` is central but has no examples/reference, reducing instruction reliability.
- **Suggested changes**:

  **New File** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-eval-orchestrator\reference\orchestrator-build-order.md`):
  ```md
  # Build Order and Path-Safe Commands
  
  Use explicit project paths to avoid MSB1003:
  
  - `dotnet build console-bcl/console-bcl.csproj`
  - `dotnet build webapi/webapi.csproj`
  - `dotnet build blazor/blazor.csproj`
  - `dotnet build efcore/efcore.csproj`
  
  Never run `dotnet build` from a directory that may not contain a project file.
  ```

  **New File** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-eval-orchestrator\examples\full-run-golden.md`):
  ```md
  # Golden: Full Evaluation Completion
  
  Input: "Generate .NET 11 evaluation output."
  
  Required output:
  1. Four domain projects (`console-bcl`, `webapi`, `blazor`, `efcore`) each with a `.csproj`.
  2. Successful `dotnet build` for each explicit project path.
  3. Evidence table with 37 dimensions:
     `Dimension | File | API Symbol | Build Status`
  4. No "not applicable" entries.
  ```

- **Expected impact**: **+10 to +30 weighted points** via better activation consistency and fewer partial completions.

### Skill-Level Improvements

### S1. Make orchestrator build gate path-safe and command-specific
- **Dimensions affected**: Overall .NET 11 API Adoption Rate; all dimensions currently absent because console/blazor/efcore did not build
- **Current score → Target score**: Overall Adoption **2.0 → 4.0+**
- **Problem**: Build report shows MSB1003 for 3 scenarios, indicating build invocation in wrong working directory.
- **Root cause**: `net11-eval-orchestrator` says “Build-gate every `.csproj`” but does not provide explicit path-safe command pattern.
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-eval-orchestrator\SKILL.md`):
  ```md
  4. Build-gate every `.csproj` and fix failures before final output.
     Success: all projects build.
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-eval-orchestrator\SKILL.md`):
  ```md
  4. Build-gate every generated project using explicit project-file paths (never directory-only `dotnet build`):
     - `dotnet build console-bcl/*.csproj`
     - `dotnet build webapi/*.csproj`
     - `dotnet build blazor/*.csproj`
     - `dotnet build efcore/*.csproj`
     If any command fails, continue editing until all four pass.
     Success: all domain projects build with no MSB1003 errors.
  ```

- **Expected impact**: High (CRITICAL tier indirect), because build failure prevented scoring opportunities across multiple dimensions.

### S2. Tighten cross-scenario scaffolding to require concrete project templates before feature coding
- **Dimensions affected**: Blazor suite, EF Core suite, runtime async/process exit, BCL features missing in non-webapi scenarios
- **Current score → Target score**: Most affected dimensions **1.0 → 3.0–5.0**
- **Problem**: Skill asks for missing domain scaffolding, but generated output still omitted viable implementations.
- **Root cause**: Missing concrete “project must compile first” gate with required files per domain.
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-cross-scenario-coverage\SKILL.md`):
  ```md
  2. Immediately scaffold all missing domains before implementing detailed business logic.
     - Do not continue if any required domain folder is absent.
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-cross-scenario-coverage\SKILL.md`):
  ```md
  2. Immediately scaffold all missing domains with compilable templates before implementing detailed business logic.
     - Do not continue if any required domain folder is absent.
     - Each domain must have a compilable `.csproj` and entrypoint (`Program.cs` for console/webapi/blazor, context/service files for efcore).
     - Run explicit domain build commands right after scaffolding; only then add feature APIs.
     Success: each domain compiles before feature details are added.
  ```

- **Expected impact**: High on HIGH/MEDIUM dimensions currently at 1.0 for Blazor/EF/runtime.

### S3. Fix JSON/WebAPI skill’s ambiguous API guidance that can cause non-rubric implementations
- **Dimensions affected**: System.Text.Json New Features, Zstandard Response Compression, Native OpenTelemetry Tracing
- **Current score → Target score**: Strong dimensions stay high (**4–5**), but reduce regression risk
- **Problem**: The skill includes template lines that conflict with its own requirements (generic `GetTypeInfo<T>()` requirement but non-generic example; zstd option property mismatch).
- **Root cause**: Internal inconsistency in `net11-json-webapi/SKILL.md` encourages mixed patterns.
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-json-webapi\SKILL.md`):
  ```md
  - Must call `options.GetTypeInfo<T>()` (generic only; do not use `GetTypeInfo(typeof(...))`)
  ...
      _ = o.SerializerOptions.TypeInfoResolver.GetTypeInfo<MyDto>();
  ...
  builder.Services.Configure<ZstandardCompressionProviderOptions>(o => o.Level = 3);
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-json-webapi\SKILL.md`):
  ```md
  - Must call the generic resolver API in code shape equivalent to `options.SerializerOptions.GetTypeInfo<MyDto>()`
  ...
      _ = o.SerializerOptions.GetTypeInfo<MyDto>();
  ...
  builder.Services.Configure<ZstandardCompressionProviderOptions>(
      o => o.CompressionOptions.Quality = 3);
  ```

- **Expected impact**: Moderate stability gain; preserves already-high CRITICAL/HIGH scores and avoids rubric drift.

### S4. Raise `net11-csharp15` from “feature mention” to “compile-verified artifact” requirements
- **Dimensions affected**: Union Type Usage, Collection Expression with() Arguments, FrozenDictionary Collection Expressions
- **Current score → Target score**: Union **1.0 → 3.0+**, with()/FrozenDictionary **1.0 → 4.0+**
- **Problem**: Despite direct mention of `union` and `with()`, generated code omitted both.
- **Root cause**: Skill lacks required file targets and “must compile” proof steps; currently relies on abstract statements.
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-csharp15\SKILL.md`):
  ```md
  2. Require one `union` declaration for a domain result and an exhaustive `switch` with no default.
     - Do not substitute abstract record/class hierarchies when this skill is active.
     - If the project cannot compile `union`, adjust project settings first, then re-generate the model.
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-csharp15\SKILL.md`):
  ```md
  2. Create a dedicated compile-verified artifact for union + exhaustive switch:
     - File target: `FeatureCoverage/UnionCoverage.cs` (or scenario-equivalent production file).
     - Must include one `union` declaration and one exhaustive `switch` with no `default`.
     - Build immediately after adding this file; if compile fails, keep iterating until it compiles.
     Success: compiled source contains `union` and exhaustive matching logic.
  ```

- **Expected impact**: High on CRITICAL/HIGH language-adoption dimensions currently at floor scores.

### S5. Add explicit anti-legacy replacements to `net11-features` with a required “before → after” migration block
- **Dimensions affected**: Base64 Parity APIs, HMAC Single-Step Verification, FrozenDictionary Collection Expressions, DivisionRounding Modes
- **Current score → Target score**: Base64 **2.0 → 4.0+**, others **1.0 → 4.0+**
- **Problem**: Analysis shows legacy APIs remained (`Convert.ToBase64String`) and multiple required replacements were not realized.
- **Root cause**: Skill bans legacy patterns but does not force concrete replacement snippets per pattern.
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-features\SKILL.md`):
  ```md
  3. Avoid legacy fallback patterns unless explicitly requested.
     - Do not use: `ZstdSharp`, manual surrogate handling, `HashData + FixedTimeEquals`,
       `Dictionary + ToFrozenDictionary()`, manual MIME dictionaries, manual division rounding helpers,
       custom JSON naming policy classes for PascalCase.
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-features\SKILL.md`):
  ```md
  3. Replace legacy fallback patterns with concrete .NET 11 APIs (show one migration per item when encountered):
     - `Convert.ToBase64String` -> `Base64.EncodeToString`
     - `HashData + FixedTimeEquals` -> `HMACSHA256.Verify` or `VerifyHmac`
     - `Dictionary + ToFrozenDictionary()` -> direct `FrozenDictionary` collection expression
     - manual divide-round helpers -> `int.Divide/DivRem(..., DivisionRounding.*)`
     Success: response includes at least one explicit `before -> after` replacement line for each encountered legacy pattern.
  ```

- **Expected impact**: Medium-to-high across several HIGH/MEDIUM dimensions.

## Summary of Recommended Changes
### New files
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-eval-entry\SKILL.md`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\scripts\net11-evidence-guard.ps1`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-eval-orchestrator\reference\orchestrator-build-order.md`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-eval-orchestrator\examples\full-run-golden.md`

### Modified files
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\README.md`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\hooks\hooks.json`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-eval-orchestrator\SKILL.md`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-cross-scenario-coverage\SKILL.md`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-json-webapi\SKILL.md`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-csharp15\SKILL.md`
- `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-features\SKILL.md`

## Risks and Trade-offs
- Stronger build/evidence gates can increase completion time and token usage.
- Adding mandatory cross-domain artifacts may overfit evaluation scenarios and produce extra files in normal tasks.
- A strict “no finalize without 37-dimension evidence” policy can block legitimate narrow-scope requests unless gated by trigger keywords (recommended in new entry skill description).
