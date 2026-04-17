# Improvement Suggestions: dotnet-net11-skill

## Executive Summary
`dotnet-net11-skill` scored **158.0/297.5 (53.1%)**, with strong Web API feature depth but major losses from missing scenario breadth (Blazor/EF Core/console coverage) and two focus-dimension misses: **Union Type Usage (1/5)** and **FrozenDictionary Collection Expressions (2/5)**.  
`BFloat16` is good but incomplete (**4/5**) because guidance does not require bit-level endian helpers.

Top opportunities are:  
1. Add a plugin-level scenario-delivery skill to force complete, buildable outputs across scenario families.  
2. Tighten C# 15 skill instructions so `union` and direct `FrozenDictionary` collection expressions are mandatory and auditable.  
3. Expand BCL guidance to require full `BFloat16` API surface usage (not cast-only).

Most improvements are **skill-level instruction changes**, with one high-impact **plugin-level structural addition**.

## Current Performance Snapshot

Only one configuration exists in this run, so direct “below baseline/top config” comparisons are unavailable from this dataset. The trailing areas below are measured against the ideal score of 5.

| Dimension | Tier | Score | Gap to 5 |
|---|---|---:|---:|
| Zstandard Compression Usage | CRITICAL | 5.0 | 0.0 |
| BFloat16 Type Usage | HIGH | 4.0 | 1.0 |
| Rune-Based String Operations | HIGH | 5.0 | 0.0 |
| HMAC Single-Step Verification | HIGH | 5.0 | 0.0 |
| FrozenDictionary Collection Expressions | HIGH | 2.0 | 3.0 |
| Collection Expression with() Arguments | HIGH | 5.0 | 0.0 |
| Union Type Usage | CRITICAL | 1.0 | 4.0 |
| MediaTypeMap Usage | MEDIUM | 5.0 | 0.0 |
| DivisionRounding Modes | MEDIUM | 5.0 | 0.0 |
| System.Text.Json New Features | CRITICAL | 5.0 | 0.0 |
| RegexOptions.AnyNewLine | MEDIUM | 1.0 | 4.0 |
| File System New APIs | HIGH | 1.0 | 4.0 |
| Base64 Parity APIs | MEDIUM | 4.0 | 1.0 |
| Generic Interlocked Operations | MEDIUM | 1.0 | 4.0 |
| BitArray.PopCount | LOW | 1.0 | 4.0 |
| Native OpenTelemetry Tracing | HIGH | 4.0 | 1.0 |
| OpenAPI Version | MEDIUM | 3.0 | 2.0 |
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
- **Inventory**: 5 skills, 1 agent, hooks enabled, no MCP/LSP.
- **Manifest quality**: good metadata (`name`, `description`, `version`, `keywords`) and semantic versioning are present.
- **Structural gaps**:
  1. `net11-focus-dimensions` has no `reference/` and no `examples/` despite being the enforcement skill.
  2. No skill dedicated to **scenario completeness/buildability**, which matches observed failures: missing `blazor`, missing `console-bcl`, incomplete `efcore`.
  3. Current guard script checks some patterns but does not explicitly require stronger BFloat16 bit APIs.

## Improvement Suggestions

### Plugin-Level Improvements

#### P1. Add a dedicated scenario-delivery skill to stop partial outputs
- **Type**: New skill
- **Dimensions affected**: Overall .NET 11 API Adoption Rate, Blazor suite, EF Core suite, File System New APIs, RegexOptions.AnyNewLine
- **Problem**: Analysis reports “good depth in webapi, weak breadth across required scenarios”; build notes show EF scenario failed.
- **Suggested changes**:

  **New File** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-scenario-delivery\SKILL.md`):
  ```md
  ---
  name: net11-scenario-delivery
  description: Enforce complete multi-scenario net11.0 project delivery with buildable console, webapi, blazor, and efcore outputs. Use when evaluations require broad .NET 11 feature coverage and no partial scaffolds.
  ---

  # .NET 11 Scenario Delivery Gate

  1. Treat each requested scenario as a deliverable artifact, not optional scaffolding.
     - Required outputs: `console-bcl/`, `webapi/`, `blazor/`, `efcore/` when requested by prompt/evaluation.
     Success: all required scenario folders contain runnable source files, not only `obj/` caches.

  2. For each scenario, implement at least one scenario-native feature before adding cross-cutting demo helpers.
     - console-bcl: regex/file-system/base64/interlocked/popcount/vector/process APIs
     - webapi: JSON + OpenAPI + output cache + compression
     - blazor: EnvironmentBoundary/Label/DisplayName/QuickGrid/TempData/BasePath/navigation/Virtualize
     - efcore: GetEntriesForState/RemoveDbContext/ExcludeForeignKeyFromMigrations/JSON query functions
     Success: each scenario includes concrete API usage in production code.

  3. Run a completion checklist before finalizing:
     - verify every requested scenario has a compilable project file
     - verify every priority dimension maps to a concrete file and API symbol
     - if any item is missing, continue implementation instead of final answer
     Success: no dimension is left uncovered because a scenario was skipped.

  Example:
  Input: "Generate all four evaluation scenarios."
  Output includes all four project folders with feature-complete source, not partial placeholders.
  ```

- **Expected impact**: raises many 1/5 scenario-derived dimensions to 3–5 and can materially increase weighted total (largest single opportunity, CRITICAL tier impact).

#### P2. Improve manifest discoverability for evaluation/task triggers
- **Type**: Manifest fix
- **Dimensions affected**: Overall .NET 11 API Adoption Rate (indirect activation reliability)
- **Problem**: Verification data reports `loaded_skills` and `loaded_plugins` as empty during the run; adding stronger trigger wording improves selection probability in mixed prompts.
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
    "description": "Generates complete net11.0-first console, webapi, blazor, and efcore solutions using C# 15 union/collection-expression patterns and modern .NET 11 APIs with no legacy fallbacks.",
    "author": { "name": "net11-auto01" },
    "license": "MIT",
    "keywords": [
      "dotnet",
      "net11",
      "csharp15",
      "union",
      "frozendictionary",
      "bfloat16",
      "aspnetcore",
      "efcore",
      "blazor",
      "console-bcl",
      "system-text-json",
      "zstandard"
    ],
    "skills": ["./skills/"]
  }
  ```

- **Expected impact**: improves activation reliability and consistency; low implementation cost, medium upside.

### Skill-Level Improvements

### S1. Enforce true `union` and direct `FrozenDictionary` expression syntax in C# 15 skill
- **Dimensions affected**: Union Type Usage, FrozenDictionary Collection Expressions, Collection Expression with() Arguments
- **Current score → Target score**: Union **1.0 → 5.0**, FrozenDictionary **2.0 → 5.0**
- **Problem**: Generated code used an abstract record hierarchy and `Dictionary + ToFrozenDictionary()`.
- **Root cause**: current skill allows fallback (“if unavailable add demo”) and does not include explicit banned replacement examples.
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-csharp15\SKILL.md`):
  ```md
  # C# 15 in .NET 11

  1. Require at least one production use of collection expression `with(...)`.
     - Example target: `var values = [with(capacity: 32), ..sourceValues];`
     Success: at least one non-test file contains `with(` in a collection expression.

  2. Require one `union` declaration for a domain result and an exhaustive `switch` with no default.
     - If union syntax is unavailable in project configuration, add a dedicated `UnionDemo` file and annotate required compiler setting.
     Success: output contains `union` keyword and exhaustive matching logic.

  3. Require one `FrozenDictionary<,>` created directly via collection expression.
     Success: no `Dictionary` + `ToFrozenDictionary()` conversion pattern appears.
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-csharp15\SKILL.md`):
  ```md
  # C# 15 in .NET 11

  1. Require at least one production use of collection expression `with(...)`.
     - Example target: `var values = [with(capacity: 32), ..sourceValues];`
     Success: at least one non-test file contains `with(` in a collection expression.

  2. Require one real `union` declaration for a domain result and an exhaustive `switch` with no default.
     - Do not substitute abstract-record or interface hierarchies when `union` is requested.
     - If compiler configuration blocks `union`, update project language settings and continue; do not downgrade the model shape.
     Success: output contains `union` keyword in production code plus exhaustive matching logic.

  3. Require one `FrozenDictionary<,>` created directly via collection expression and forbid conversion flow.
     - Required form: `FrozenDictionary<string, Category> map = ["electronics": new Category("Electronics")];`
     - Forbidden form: `new Dictionary<...> { ... }.ToFrozenDictionary()`
     Success: no `ToFrozenDictionary()` appears for primary static maps.
  ```

- **Expected impact**: high; this directly addresses two lowest focus-dimension scores (CRITICAL + HIGH tier).

### S2. Strengthen BFloat16 guidance from cast-only to full API surface
- **Dimensions affected**: BFloat16 Type Usage
- **Current score → Target score**: **4.0 → 5.0**
- **Problem**: output shows cast round-trip only (`BFloat16 bf = (BFloat16)value`), missing endian/bit APIs.
- **Root cause**: skill requires only conversion helper and does not demand `BinaryPrimitives`/`BitConverter` BFloat16 helpers.
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-bcl-core\SKILL.md`):
  ```md
  2. Implement a minimal BCL feature probe module (`FeatureCoverage/BclCoreProbe.cs`) with all required APIs:
     - `System.Numerics.BFloat16` conversion method
     - Rune-aware string method using `string.Contains(Rune)` and `string.Replace(Rune, Rune)`
     - `HMACSHA256.Verify` (or `CryptographicOperations.VerifyHmac`) in a verification method
     - `FrozenDictionary<K,V> map = ["k": v]` static lookup
     - `MediaTypeMap.GetMediaType/GetExtension`
     - `int.Divide(..., DivisionRounding.ToPositiveInfinity)` pagination helper
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-bcl-core\SKILL.md`):
  ```md
  2. Implement a minimal BCL feature probe module (`FeatureCoverage/BclCoreProbe.cs`) with all required APIs:
     - `System.Numerics.BFloat16` conversion plus one bit-level helper:
       - `BinaryPrimitives.WriteBFloat16LittleEndian(...)` or
       - `BitConverter.BFloat16ToInt16Bits(...)`
     - Rune-aware string method using `string.Contains(Rune)` and `string.Replace(Rune, Rune)`
     - `HMACSHA256.Verify` (or `CryptographicOperations.VerifyHmac`) in a verification method
     - `FrozenDictionary<K,V> map = ["k": v]` static lookup
     - `MediaTypeMap.GetMediaType/GetExtension`
     - `int.Divide(..., DivisionRounding.ToPositiveInfinity)` pagination helper
  ```

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-bcl-core\reference\bcl-core-patterns.md`):
  ```md
  Required outcomes:
  - BFloat16 conversion helper
  - Rune-based text operations helper
  - HMAC single-step verification helper
  - FrozenDictionary expression lookup table
  - DivisionRounding pagination helper
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-bcl-core\reference\bcl-core-patterns.md`):
  ```md
  Required outcomes:
  - BFloat16 conversion helper plus endian/bit helper (`BinaryPrimitives.WriteBFloat16LittleEndian` or `BitConverter.BFloat16ToInt16Bits`)
  - Rune-based text operations helper
  - HMAC single-step verification helper
  - FrozenDictionary expression lookup table
  - MediaTypeMap forward/reverse lookup helper
  - DivisionRounding pagination helper
  ```

- **Expected impact**: likely +1 in HIGH tier dimension with low risk.

### S3. Add missing reference/examples to the enforcement skill (`net11-focus-dimensions`)
- **Dimensions affected**: FrozenDictionary Collection Expressions, Union Type Usage, MediaTypeMap Usage, DivisionRounding Modes, System.Text.Json New Features, Overall Adoption
- **Current score → Target score**: mixed; primarily consistency gains and reduced regression risk
- **Problem**: the enforcement skill has no `reference/` or `examples/`, reducing activation quality and concrete guidance under best-practice rubric.
- **Root cause**: SKILL exists but lacks reusable golden artifacts.
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-focus-dimensions\SKILL.md`):
  ```md
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
  3. Use only target APIs:
     - `System.Numerics.BFloat16` (include one endian/bit helper from reference)
     - Rune overloads (`string.Contains/IndexOf/Replace/Split(Rune)`)
     - `HMACSHA256.Verify` / `CryptographicOperations.VerifyHmac`
     - `FrozenDictionary<K,V> map = ["k": v]` (no `ToFrozenDictionary()` fallback)
     - Collection expression `with(...)`
     - `union` with exhaustive `switch` (no abstract-record substitute)
     - `MediaTypeMap.GetMediaType/GetExtension`
     - `int.Divide/DivRem(..., DivisionRounding.*)`
     - JSON features: `JsonNamingPolicy.PascalCase`, `[JsonNamingPolicy]`, type-level `[JsonIgnore]`, `IReadOnlySet<T>`, `GetTypeInfo<T>()`
     Success: generated code contains these concrete API names where expected.

  5. Use `reference/focus-dimension-matrix.md` and `examples/focus-dimensions-golden.md` as mandatory templates for coverage files.
     Success: all priority dimensions map to at least one copied-and-adapted snippet from the example set.
  ```

  **New File** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-focus-dimensions\reference\focus-dimension-matrix.md`):
  ```md
  # Focus Dimension Matrix

  | Dimension | Required API | Forbidden fallback |
  |---|---|---|
  | BFloat16 | `BFloat16` + `BinaryPrimitives.WriteBFloat16LittleEndian` or `BitConverter.BFloat16ToInt16Bits` | manual bit masking only |
  | Rune strings | `string.Contains/Replace(Rune)` | surrogate-pair string assembly |
  | HMAC verify | `HMACSHA256.Verify` / `VerifyHmac` | `HashData` + `FixedTimeEquals` |
  | FrozenDictionary | `FrozenDictionary<K,V> map = ["k": v]` | `Dictionary` + `ToFrozenDictionary()` |
  | Collection with() | `[with(capacity: n), ..values]` | constructor + AddRange |
  | Union | `union` + exhaustive switch | abstract class/record hierarchy |
  | MediaTypeMap | `MediaTypeMap.GetMediaType/GetExtension` | manual dictionary |
  | DivisionRounding | `Divide/DivRem(..., DivisionRounding.*)` | custom rounding math |
  | JSON | `JsonNamingPolicy.PascalCase`, `[JsonNamingPolicy]`, type `[JsonIgnore]`, `IReadOnlySet<T>`, `GetTypeInfo<T>()` | custom PascalCase policy |
  ```

  **New File** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-focus-dimensions\examples\focus-dimensions-golden.md`):
  ````md
  ```csharp
  // BFloat16 + bit helper
  BFloat16 bf = (BFloat16)1.25f;
  Span<byte> bytes = stackalloc byte[2];
  BinaryPrimitives.WriteBFloat16LittleEndian(bytes, bf);

  // Rune
  var hasEmoji = text.Contains(new Rune(0x1F680));

  // HMAC
  var ok = HMACSHA256.Verify(key, data, mac);

  // FrozenDictionary expression
  FrozenDictionary<string,int> codes = ["ok": 200, "notfound": 404];

  // Collection with()
  var items = [with(capacity: source.Count), ..source];

  // Union
  union ApiResult { Success(string Value); Error(string Message); }

  // MediaTypeMap
  var media = MediaTypeMap.GetMediaType(".json");

  // DivisionRounding
  var pages = int.Divide(total, size, DivisionRounding.ToPositiveInfinity);

  // System.Text.Json
  options.PropertyNamingPolicy = JsonNamingPolicy.PascalCase;
  ```
  ````

- **Expected impact**: medium-high consistency improvement and better rubric alignment (supporting files + clarity).

### S4. Expand top-level feature skill with explicit focus-dimension pass/fail gate
- **Dimensions affected**: all focus dimensions (especially Union/FrozenDictionary/BFloat16)
- **Current score → Target score**: keeps 5/5 dimensions stable; lifts weaker focus dimensions to 5
- **Problem**: current checklist requires listing dimensions but does not define hard failure criteria for specific fallback patterns.
- **Root cause**: “checklist” is descriptive, not enforceable.
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-features\SKILL.md`):
  ```md
  4. Include a strict verification checklist in the response.
     - List each priority dimension with file path + API symbol.
     - If any priority dimension is missing, continue implementation instead of finalizing.
     Success: checklist is complete and contains zero missing priority dimensions.
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-features\SKILL.md`):
  ```md
  4. Include a strict verification checklist in the response.
     - List each priority dimension with file path + API symbol.
     - Add a PASS/FAIL verdict per dimension using explicit bans:
       - FAIL if `ToFrozenDictionary(` appears for primary static maps
       - FAIL if `union` is absent where a discriminated result is modeled
       - FAIL if BFloat16 usage is cast-only with no endian/bit helper
     - If any priority dimension is FAIL, continue implementation instead of finalizing.
     Success: checklist is complete and all priority dimensions are PASS.
  ```

- **Expected impact**: medium; directly reduces recurrent false positives where API names appear but target patterns are still missed.

## Summary of Recommended Changes

### New files
1. `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-scenario-delivery\SKILL.md`
2. `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-focus-dimensions\reference\focus-dimension-matrix.md`
3. `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-focus-dimensions\examples\focus-dimensions-golden.md`

### Modified files
1. `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\plugin.json`
2. `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-csharp15\SKILL.md`
3. `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-bcl-core\SKILL.md`
4. `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-bcl-core\reference\bcl-core-patterns.md`
5. `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-focus-dimensions\SKILL.md`
6. `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-features\SKILL.md`

## Risks and Trade-offs
- Stricter hard gates can increase completion time and token usage because the model must satisfy more explicit checks before finishing.
- Requiring `union` unconditionally may fail in environments where compiler support is not enabled; this is mitigated by explicitly instructing project config updates instead of fallback modeling.
- Adding scenario-delivery requirements may produce larger generated outputs; however, this directly targets the largest observed score loss source (scenario incompleteness).
