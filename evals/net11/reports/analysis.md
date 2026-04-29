# Aggregated Analysis: .NET 11 Feature Adoption Evaluation

**Runs:** 1 | **Configurations:** 1 | **Scenarios:** 4 | **Dimensions:** 37
**Date:** 2026-04-21 17:17 UTC

---

## Overview

Evaluate how the dotnet-net11 skill improves Copilot's ability to generate code that correctly uses new .NET 11 APIs, C# language features, and framework capabilities instead of falling back to older patterns or third-party packages.

---

## What Was Tested

### Scenarios

Each run generates one of the following application scenarios (randomly selected per run):

| Scenario | Description |
|---|---|
| console-bcl | Create a console developer toolkit app that exercises new BCL features including Zstandard compression, BFloat16, Rune-based text processing, HMAC verification, frozen collections, MIME type mapping, hard links, division rounding, JSON serialization improvements, Base64 parity, regex enhancements, anonymous pipes, union types, and collection expression arguments.
 |
| webapi | Create a product catalog Web API that exercises new ASP.NET Core features including native OpenTelemetry tracing, OpenAPI 3.2, dynamic output cache policies, Zstandard response compression, and modern JSON serialization (PascalCase, per-member naming, type-level ignore, IReadOnlySet, generic GetTypeInfo).
 |
| blazor | Create a Blazor task management app that exercises new Blazor components and navigation features including EnvironmentBoundary, Label, DisplayName, QuickGrid OnRowClick, relative-to-current-URI navigation, hash fragment URIs, TempData, BasePath, variable-height virtualization, and SignalR connection configuration.
 |
| efcore | Create an EF Core order management demo that exercises new EF Core features including efficient state-based change tracking, clean DbContext replacement for testing, foreign key migration exclusion, and JSON column queries.
 |

### Configurations

Each configuration gives Copilot different custom skills or plugins. The **no-skills** baseline uses default Copilot with no custom instructions.

| Configuration | Description | Skills | Plugins |
|---|---|---|---|
| dotnet-net11-skill | dotnet-net11 Skill | — | dotnet-net11:dotnet-net11 |

### How It Works

1. **Generate** — For each configuration, Copilot CLI (`copilot --yolo`) is given a scenario prompt and generates a complete project from scratch. One scenario is randomly selected per run.
2. **Verify** — Each generated project is built (`dotnet build`), run, format-checked, and scanned for vulnerabilities.
3. **Analyze** — An AI judge reviews the source code of all configurations side-by-side and scores each across 37 quality dimensions.

Generation model: **claude-opus-4.6**
Analysis model: **gpt-5.3-codex**

---

## Scoring Methodology

Each dimension is scored on a **1–5 scale**:

| Score | Meaning |
|:---:|---|
| 5 | Excellent — follows all best practices |
| 4 | Good — minor gaps only |
| 3 | Acceptable — some issues present |
| 2 | Below average — significant gaps |
| 1 | Poor — missing or fundamentally wrong |

Dimensions are grouped into **tiers** that determine their weight in the final weighted score:

| Tier | Weight | Dimensions |
|---|:---:|:---:|
| CRITICAL | ×3 | 4 |
| HIGH | ×2 | 16 |
| MEDIUM | ×1 | 14 |
| LOW | ×0.5 | 3 |

**Maximum possible weighted score: 297.5** (all dimensions scoring 5).
Scores shown as **mean ± standard deviation** across runs.

---

## Executive Summary

Mean dimension scores across runs (1–5 scale, **higher is better**). ± values show standard deviation across runs.

| Dimension [Tier] | dotnet-net11-skill |
|---|---|
| Zstandard Compression Usage [CRITICAL] | 5.0 |
| BFloat16 Type Usage [HIGH] | 4.0 |
| Rune-Based String Operations [HIGH] | 4.0 |
| HMAC Single-Step Verification [HIGH] | 5.0 |
| FrozenDictionary Collection Expressions [HIGH] | 2.0 |
| Collection Expression with() Arguments [HIGH] | 1.0 |
| Union Type Usage [CRITICAL] | 1.0 |
| MediaTypeMap Usage [MEDIUM] | 2.0 |
| DivisionRounding Modes [MEDIUM] | 2.0 |
| System.Text.Json New Features [CRITICAL] | 5.0 |
| RegexOptions.AnyNewLine [MEDIUM] | 1.0 |
| File System New APIs [HIGH] | 1.0 |
| Base64 Parity APIs [MEDIUM] | 5.0 |
| Generic Interlocked Operations [MEDIUM] | 1.0 |
| BitArray.PopCount [LOW] | 1.0 |
| Native OpenTelemetry Tracing [HIGH] | 4.0 |
| OpenAPI Version [MEDIUM] | 2.0 |
| Dynamic Output Cache Policy Provider [HIGH] | 5.0 |
| Zstandard Response Compression [HIGH] | 5.0 |
| Blazor EnvironmentBoundary Component [HIGH] | 1.0 |
| Blazor Label and DisplayName Components [HIGH] | 1.0 |
| QuickGrid OnRowClick [HIGH] | 1.0 |
| RelativeToCurrentUri Navigation [MEDIUM] | 1.0 |
| Blazor TempData Support [HIGH] | 1.0 |
| Blazor BasePath Component [MEDIUM] | 1.0 |
| EF Core GetEntriesForState [HIGH] | 1.0 |
| EF Core RemoveDbContext [HIGH] | 1.0 |
| EF Core ExcludeForeignKeyFromMigrations [MEDIUM] | 1.0 |
| EF Core JSON Query Functions [HIGH] | 1.0 |
| SignalR ConfigureConnection [MEDIUM] | 1.0 |
| Blazor Virtualize Variable-Height Items [MEDIUM] | 1.0 |
| Runtime Async Configuration [MEDIUM] | 1.0 |
| ProcessExitStatus Usage [MEDIUM] | 1.0 |
| OpenAPI Binary File Response [MEDIUM] | 5.0 |
| Brotli and Compression Options [LOW] | 1.0 |
| Vector Constants [LOW] | 1.0 |
| Overall .NET 11 API Adoption Rate [CRITICAL] | 2.0 |

---

## Final Rankings

Configurations ranked by mean weighted score — **higher is better**. Std Dev shows run-to-run variability (lower = more consistent).

| Rank | Configuration | Mean Score ↑ | % of Max (297.5) | Std Dev ↓ | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | dotnet-net11-skill | 141.5 | 48% | 0.0 | 141.5 | 141.5 |

---

## Weighted Score per Run

| Run | dotnet-net11-skill |
|---|---|
| 1 | 141.5 |
| **Mean** | **141.5** |

---

## Verification Summary (All Runs)

| Configuration | Build Pass Rate | Run Pass Rate | Avg Warnings |
|---|---|---|---|
| dotnet-net11-skill | 1/4 (25%) | 1/4 (25%) | 0.0 |

---

## Token Usage Summary

Average token consumption per configuration across all runs.

| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time |
|---|---|---|---|---|---|
| dotnet-net11-skill | 3,335,051 | 30,844 | 3,215,579 | 61 | 13m 10s |

---

## Token Usage Per Run

| Configuration | Run | Scenario | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time | Note |
|---|---|---|---|---|---|---|---|---|
| dotnet-net11-skill | 1 | webapi | 3,335,051 | 30,844 | 3,215,579 | 61 | 13m 10s |  |


---

## Per-Dimension Analysis

### 1. Zstandard Compression Usage [CRITICAL × 3]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

`dotnet-net11-skill` uses built-in Zstandard APIs correctly.

```csharp
// dotnet-net11-skill: webapi/ProductHub/FeatureCoverage/BclCoreProbe.cs
using (var zs = new ZstandardStream(output, new ZstandardCompressionOptions { Quality = 3 }, leaveOpen: true))
{
    zs.Write(input, 0, input.Length);
}
```

**Score (dotnet-net11-skill): 5/5** — Uses `ZstandardStream` and `ZstandardCompressionOptions`; no third-party Zstd package pattern.

**Verdict:** Strong and modern usage aligned with .NET 11 BCL guidance.

### 2. BFloat16 Type Usage [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 4 |
| **Mean** | **4.0** |

#### Analysis

Native `BFloat16` is used directly.

```csharp
// dotnet-net11-skill: webapi/ProductHub/FeatureCoverage/BclCoreProbe.cs
public static BFloat16 ConvertToBFloat16(float value) => (BFloat16)value;
```

**Score (dotnet-net11-skill): 4/5** — Correct native type usage, though coverage is minimal (no endian helpers/bit conversion APIs).

**Verdict:** Good adoption; could be expanded for broader numeric pipeline realism.

### 3. Rune-Based String Operations [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 4 |
| **Mean** | **4.0** |

#### Analysis

Rune overloads are used instead of surrogate handling.

```csharp
// dotnet-net11-skill: webapi/ProductHub/FeatureCoverage/BclCoreProbe.cs
public static bool ContainsRune(string text, int codePoint) => text.Contains(new Rune(codePoint));
public static int IndexOfRune(string text, int codePoint) => text.IndexOf(new Rune(codePoint));
```

**Score (dotnet-net11-skill): 4/5** — Correct API family, but narrow coverage.

**Verdict:** Good Unicode-safe approach and better than manual UTF-16 logic.

### 4. HMAC Single-Step Verification [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

Single-step verify API is implemented.

```csharp
// dotnet-net11-skill: webapi/ProductHub/FeatureCoverage/BclCoreProbe.cs
public static bool VerifyHmac(byte[] key, byte[] data, byte[] mac) =>
    HMACSHA256.Verify(key, data, mac);
```

**Score (dotnet-net11-skill): 5/5** — Uses modern safer single-step verification.

**Verdict:** Best-practice cryptographic validation pattern.

### 5. FrozenDictionary Collection Expressions [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 2 |
| **Mean** | **2.0** |

#### Analysis

Uses `ToFrozenDictionary()` instead of direct collection-expression construction.

```csharp
// dotnet-net11-skill: webapi/ProductHub/FeatureCoverage/BclCoreProbe.cs
private static readonly FrozenDictionary<string, int> StatusCodes =
    new Dictionary<string, int> { ["ok"] = 200, ["notfound"] = 404, ["error"] = 500 }.ToFrozenDictionary();
```

**Score (dotnet-net11-skill): 2/5** — Present but via older multi-step pattern.

**Verdict:** Functional but not the targeted .NET 11/C# 15 idiom.

### 6. Collection Expression with() Arguments [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No `with()` collection expression argument usage found.

```text
// dotnet-net11-skill: console-bcl/DevToolkit/DevToolkit.csproj.lscache
[sourceFiles]
obj/Debug/net11.0/
 DevToolkit.GlobalUsings.g.cs
```

**Score (dotnet-net11-skill): 1/5** — Missing in generated source.

**Verdict:** This capability is not demonstrated.

### 7. Union Type Usage [CRITICAL × 3]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No `union`-keyword implementation is present.

```text
// dotnet-net11-skill: console-bcl/console-bcl.csproj.lscache
[sourceFiles]
obj/Debug/net11.0/
 console-bcl.GlobalUsings.g.cs
```

**Score (dotnet-net11-skill): 1/5** — Missing.

**Verdict:** Critical gap for C# 15 union coverage.

### 8. MediaTypeMap Usage [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 2 |
| **Mean** | **2.0** |

#### Analysis

MIME mapping is implemented with `FileExtensionContentTypeProvider`, not `MediaTypeMap`.

```csharp
// dotnet-net11-skill: webapi/ProductHub/Endpoints/ExportEndpoints.cs
private static readonly FileExtensionContentTypeProvider _mimeProvider = new();
_mimeProvider.TryGetContentType(ext, out var mediaType);
```

**Score (dotnet-net11-skill): 2/5** — Present behavior, but not the requested new API.

**Verdict:** Works functionally; misses the intended .NET 11 mapping API.

### 9. DivisionRounding Modes [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 2 |
| **Mean** | **2.0** |

#### Analysis

Manual ceiling division is used.

```csharp
// dotnet-net11-skill: webapi/ProductHub/FeatureCoverage/BclCoreProbe.cs
public static int CalculatePageCount(int totalItems, int pageSize) =>
    (totalItems + pageSize - 1) / pageSize;
```

**Score (dotnet-net11-skill): 2/5** — Correct result for one mode, but no `DivisionRounding` API.

**Verdict:** Acceptable fallback; not modern API adoption.

### 10. System.Text.Json New Features [CRITICAL × 3]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

Multiple .NET 11 JSON features are used together.

```csharp
// dotnet-net11-skill: webapi/ProductHub/Program.cs + Models/Product.cs
options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.PascalCase;
_ = options.SerializerOptions.GetTypeInfo<Product>();
[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
[JsonNamingPolicy(JsonKnownNamingPolicy.CamelCase)]
public IReadOnlySet<string> Tags { get; set; } = new HashSet<string>();
```

**Score (dotnet-net11-skill): 5/5** — Strong feature coverage and correct patterns.

**Verdict:** Best area in this run; modern JSON APIs are applied correctly.

### 11. RegexOptions.AnyNewLine [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No regex newline-mode modernization appears.

```text
// dotnet-net11-skill: webapi/ProductHub/FeatureCoverage/BclCoreProbe.cs
// (No RegexOptions.AnyNewLine usage in source)
```

**Score (dotnet-net11-skill): 1/5** — Missing.

**Verdict:** No evidence of new regex newline handling.

### 12. File System New APIs [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No hard-link/null-handle/anonymous-pipe API usage is present.

```text
// dotnet-net11-skill: console-bcl/DevToolkit/DevToolkit.csproj.lscache
[sourceFiles]
obj/Debug/net11.0/
 DevToolkit.AssemblyInfo.cs
```

**Score (dotnet-net11-skill): 1/5** — Missing scenario implementation.

**Verdict:** High-priority BCL filesystem area is unimplemented.

### 13. Base64 Parity APIs [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

New Base64 convenience APIs are used correctly.

```csharp
// dotnet-net11-skill: webapi/ProductHub/Endpoints/ExportEndpoints.cs
var base64 = Base64.EncodeToString(Encoding.UTF8.GetBytes(sb.ToString()));
```

```csharp
// dotnet-net11-skill: webapi/ProductHub/FeatureCoverage/BclCoreProbe.cs
Encoding.UTF8.GetString(Base64.DecodeFromChars(base64));
```

**Score (dotnet-net11-skill): 5/5** — Uses parity APIs rather than `Convert.ToBase64String`.

**Verdict:** Excellent adoption of the new Base64 surface.

### 14. Generic Interlocked Operations [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No generic enum `Interlocked.And/Or` usage; only integer increment.

```csharp
// dotnet-net11-skill: webapi/ProductHub/Services/ProductService.cs
Id = Interlocked.Increment(ref _nextId),
```

**Score (dotnet-net11-skill): 1/5** — Target API not demonstrated.

**Verdict:** Concurrency usage exists, but not the new generic enum operations.

### 15. BitArray.PopCount [LOW × 0]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No `BitArray.PopCount()` usage found.

```text
// dotnet-net11-skill: webapi/ProductHub/FeatureCoverage/BclCoreProbe.cs
// (No BitArray.PopCount usage in source)
```

**Score (dotnet-net11-skill): 1/5** — Missing.

**Verdict:** Low-priority API not covered.

### 16. Native OpenTelemetry Tracing [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 4 |
| **Mean** | **4.0** |

#### Analysis

Tracing uses ASP.NET Core source directly.

```csharp
// dotnet-net11-skill: webapi/ProductHub/Program.cs
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing
        .AddSource("Microsoft.AspNetCore")
        .AddConsoleExporter());
```

**Score (dotnet-net11-skill): 4/5** — Native source is correct; still relies on external OTel package wiring.

**Verdict:** Good direction; close to ideal native setup.

### 17. OpenAPI Version [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 2 |
| **Mean** | **2.0** |

#### Analysis

Spec version is explicitly set to 3.1, not 3.2.

```csharp
// dotnet-net11-skill: webapi/ProductHub/Program.cs
builder.Services.AddOpenApi(options =>
    options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_1);
```

**Score (dotnet-net11-skill): 2/5** — Explicit but below requested 3.2.

**Verdict:** Reasonable compatibility fallback, but misses target version.

### 18. Dynamic Output Cache Policy Provider [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

A DI-resolved provider implementation is present.

```csharp
// dotnet-net11-skill: webapi/ProductHub/Services/ApiOutputCachePolicyProvider.cs
public sealed class ApiOutputCachePolicyProvider : IOutputCachePolicyProvider
{
    public ValueTask<IOutputCachePolicy?> GetPolicyAsync(string policyName) => ...
}
```

```csharp
// dotnet-net11-skill: webapi/ProductHub/Program.cs
builder.Services.AddSingleton<IOutputCachePolicyProvider, ApiOutputCachePolicyProvider>();
```

**Score (dotnet-net11-skill): 5/5** — Correct dynamic provider pattern.

**Verdict:** Strong implementation aligned with runtime policy resolution best practice.

### 19. Zstandard Response Compression [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

Response compression and request decompression include Zstandard.

```csharp
// dotnet-net11-skill: webapi/ProductHub/Program.cs
builder.Services.AddResponseCompression();
builder.Services.Configure<ZstandardCompressionProviderOptions>(
    o => o.CompressionOptions.Quality = 3);
builder.Services.AddRequestDecompression();
```

**Score (dotnet-net11-skill): 5/5** — Correct and comprehensive Zstd server support.

**Verdict:** Best-practice compression setup for .NET 11 APIs.

### 20. Blazor EnvironmentBoundary Component [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No Blazor source was generated for the feature.

```text
// dotnet-net11-skill: blazor/TaskFlow/TaskFlow.csproj.lscache
[sourceFiles]
obj/Debug/net11.0/
 TaskFlow.GlobalUsings.g.cs
```

**Score (dotnet-net11-skill): 1/5** — Missing.

**Verdict:** Cannot evaluate component usage because app source is absent.

### 21. Blazor Label and DisplayName Components [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No generated Blazor components or pages are available.

```text
// dotnet-net11-skill: blazor/TaskFlow/TaskFlow.csproj.lscache
[sourceFiles]
obj/Debug/net11.0/
 TaskFlow.AssemblyInfo.cs
```

**Score (dotnet-net11-skill): 1/5** — Missing.

**Verdict:** Form/table metadata components are not demonstrated.

### 22. QuickGrid OnRowClick [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No QuickGrid code appears in available sources.

```text
// dotnet-net11-skill: blazor/TaskFlow/TaskFlow.csproj.lscache
[sourceFiles]
obj/Debug/net11.0/
 .NETCoreApp,Version=v11.0.AssemblyAttributes.cs
```

**Score (dotnet-net11-skill): 1/5** — Missing.

**Verdict:** High-value Blazor data-grid modernization not implemented.

### 23. RelativeToCurrentUri Navigation [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No Blazor navigation implementation is present.

```text
// dotnet-net11-skill: blazor/TaskFlow/TaskFlow.csproj.lscache
// No .razor source entries; only generated obj files are listed.
```

**Score (dotnet-net11-skill): 1/5** — Missing.

**Verdict:** No evidence of relative-to-current URI APIs.

### 24. Blazor TempData Support [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No `ITempData`/flash-message pattern in generated Blazor app.

```text
// dotnet-net11-skill: blazor/TaskFlow/TaskFlow.csproj.lscache
// No Blazor component source was generated for run-1.
```

**Score (dotnet-net11-skill): 1/5** — Missing.

**Verdict:** Cross-page flash messaging is not covered.

### 25. Blazor BasePath Component [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No evidence of `<BasePath />` usage due missing app source.

```text
// dotnet-net11-skill: blazor/TaskFlow/TaskFlow.csproj.lscache
// Source listing includes only generated assembly files.
```

**Score (dotnet-net11-skill): 1/5** — Missing.

**Verdict:** Hosting-base-path modernization is not demonstrated.

### 26. EF Core GetEntriesForState [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No EF Core source files are present for run-1.

```text
// dotnet-net11-skill: efcore/samples/net11/efcore-showcase/efcore-showcase.csproj.lscache
[sourceFiles]
obj/Debug/net11.0/
 efcore-showcase.GlobalUsings.g.cs
```

**Score (dotnet-net11-skill): 1/5** — Missing.

**Verdict:** Cannot validate state-based change-tracking APIs.

### 27. EF Core RemoveDbContext [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No registration code exists to assess `RemoveDbContext<T>()`.

```text
// dotnet-net11-skill: efcore/samples/net11/efcore-showcase/efcore-showcase.csproj.lscache
// No user-authored EF source files are listed.
```

**Score (dotnet-net11-skill): 1/5** — Missing.

**Verdict:** Test-focused context replacement API not demonstrated.

### 28. EF Core ExcludeForeignKeyFromMigrations [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No model configuration code is available.

```text
// dotnet-net11-skill: efcore/samples/net11/efcore-showcase/efcore-showcase.csproj.lscache
// Only generated obj source entries are present.
```

**Score (dotnet-net11-skill): 1/5** — Missing.

**Verdict:** FK migration exclusion support is not shown.

### 29. EF Core JSON Query Functions [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No LINQ/query source demonstrates `EF.Functions.JsonContains/JsonPathExists`.

```text
// dotnet-net11-skill: efcore/samples/net11/efcore-showcase/efcore-showcase.csproj.lscache
// No app query code is present in run-1.
```

**Score (dotnet-net11-skill): 1/5** — Missing.

**Verdict:** High-priority EF JSON query APIs are unimplemented.

### 30. SignalR ConfigureConnection [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No Blazor server render-mode connection configuration appears.

```text
// dotnet-net11-skill: blazor/TaskFlow/TaskFlow.csproj.lscache
// No Program.cs/.razor source provided for run-1.
```

**Score (dotnet-net11-skill): 1/5** — Missing.

**Verdict:** SignalR connection-option modernization is absent.

### 31. Blazor Virtualize Variable-Height Items [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No `<Virtualize>` usage is visible because Blazor source is missing.

```text
// dotnet-net11-skill: blazor/TaskFlow/TaskFlow.csproj.lscache
// Missing component/page source for virtualization review.
```

**Score (dotnet-net11-skill): 1/5** — Missing.

**Verdict:** Variable-height virtualization behavior cannot be evaluated.

### 32. Runtime Async Configuration [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No project file in run-1 demonstrates `<Features>runtime-async=on</Features>`.

```text
// dotnet-net11-skill: console-bcl/console-bcl.csproj.lscache
// No user project XML source is present; only lscache metadata.
```

**Score (dotnet-net11-skill): 1/5** — Missing.

**Verdict:** Runtime async feature-toggle usage is not shown.

### 33. ProcessExitStatus Usage [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No process-management code using `process.ExitStatus` is present.

```text
// dotnet-net11-skill: console-bcl/DevToolkit/DevToolkit.csproj.lscache
// No application source files beyond generated obj entries.
```

**Score (dotnet-net11-skill): 1/5** — Missing.

**Verdict:** Rich process-exit semantics are not demonstrated.

### 34. OpenAPI Binary File Response [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

Binary response metadata is declared correctly.

```csharp
// dotnet-net11-skill: webapi/ProductHub/Endpoints/ExportEndpoints.cs
.Produces<FileContentResult>(contentType: "application/octet-stream");
```

**Score (dotnet-net11-skill): 5/5** — Proper OpenAPI binary response description.

**Verdict:** Excellent API documentation hygiene for download endpoints.

### 35. Brotli and Compression Options [LOW × 0]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

Brotli option modernization (e.g., `WindowLog`) is not present.

```csharp
// dotnet-net11-skill: webapi/ProductHub/Program.cs
builder.Services.Configure<ZstandardCompressionProviderOptions>(
    o => o.CompressionOptions.Quality = 3);
```

**Score (dotnet-net11-skill): 1/5** — Compression config exists, but not the targeted Brotli properties.

**Verdict:** Low-priority tuning surface not covered.

### 36. Vector Constants [LOW × 0]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No `Vector<T>.Pi/E/Tau/...` usage appears.

```text
// dotnet-net11-skill: webapi/ProductHub/FeatureCoverage/BclCoreProbe.cs
// No SIMD vector constant APIs used in source.
```

**Score (dotnet-net11-skill): 1/5** — Missing.

**Verdict:** SIMD constant enhancements are not demonstrated.

### 37. Overall .NET 11 API Adoption Rate [CRITICAL × 3]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 2 |
| **Mean** | **2.0** |

#### Analysis

The Web API app shows good targeted adoption, but 3 expected app scenarios are effectively absent in run-1 source.

```text
// dotnet-net11-skill: run-1 directory state
console-bcl/DevToolkit/DevToolkit.csproj.lscache
blazor/TaskFlow/TaskFlow.csproj.lscache
efcore/samples/net11/efcore-showcase/efcore-showcase.csproj.lscache
```

**Score (dotnet-net11-skill): 2/5** — Partial adoption concentrated in `webapi`; broad scenario coverage is missing.

**Verdict:** Good depth in one app, insufficient breadth across the full benchmark.

---

## Asset Usage Summary

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| dotnet-net11-skill | 1 | b953c0e1…1c7f | claude-opus-4.6 | — | — | ✅ |

---

## Raw Data References

- Per-run analysis: `reports/analysis-run-1.md`
- Verification data: `reports/verification-data.json`
- Score data: `reports/scores-data.json`
- Build notes: `reports/build-notes.md`
- Generation usage: `reports/generation-usage.json`
