# Aggregated Analysis: .NET 11 Feature Adoption Evaluation

**Runs:** 1 | **Configurations:** 1 | **Scenarios:** 4 | **Dimensions:** 37
**Date:** 2026-04-17 05:05 UTC

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
| BFloat16 Type Usage [HIGH] | 1.0 |
| Rune-Based String Operations [HIGH] | 2.0 |
| HMAC Single-Step Verification [HIGH] | 1.0 |
| FrozenDictionary Collection Expressions [HIGH] | 2.0 |
| Collection Expression with() Arguments [HIGH] | 5.0 |
| Union Type Usage [CRITICAL] | 1.0 |
| MediaTypeMap Usage [MEDIUM] | 2.0 |
| DivisionRounding Modes [MEDIUM] | 1.0 |
| System.Text.Json New Features [CRITICAL] | 3.0 |
| RegexOptions.AnyNewLine [MEDIUM] | 1.0 |
| File System New APIs [HIGH] | 1.0 |
| Base64 Parity APIs [MEDIUM] | 2.0 |
| Generic Interlocked Operations [MEDIUM] | 1.0 |
| BitArray.PopCount [LOW] | 1.0 |
| Native OpenTelemetry Tracing [HIGH] | 2.0 |
| OpenAPI Version [MEDIUM] | 5.0 |
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
| 🥇 | dotnet-net11-skill | 120.5 | 41% | 0.0 | 120.5 | 120.5 |

---

## Weighted Score per Run

| Run | dotnet-net11-skill |
|---|---|
| 1 | 120.5 |
| **Mean** | **120.5** |

---

## Verification Summary (All Runs)

| Configuration | Build Pass Rate | Run Pass Rate | Avg Warnings |
|---|---|---|---|
| dotnet-net11-skill | 1/2 (50%) | 1/2 (50%) | 0.0 |

---

## Token Usage Summary

Average token consumption per configuration across all runs.

| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time |
|---|---|---|---|---|---|
| dotnet-net11-skill | 7,432,932 | 51,144 | 7,243,679 | 124 | 21m 15s |

---

## Token Usage Per Run

| Configuration | Run | Scenario | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time | Note |
|---|---|---|---|---|---|---|---|---|
| dotnet-net11-skill | 1 | webapi | 7,432,932 | 51,144 | 7,243,679 | 124 | 21m 15s |  |


---

## Per-Dimension Analysis

### 1. Zstandard Compression Usage [CRITICAL × 3]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

```csharp
// dotnet-net11-skill (webapi/Program.cs)
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<ZstandardCompressionProvider>();
});
```
**Score (dotnet-net11-skill): 5/5** — Uses built-in ASP.NET Core Zstandard provider, not third-party packages.  
**Verdict:** Strong modern usage.

### 2. BFloat16 Type Usage [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```md
<!-- dotnet-net11-skill (webapi/gen-notes.md) -->
| `BFloat16` | `System.Numerics.BFloat16` | Not included (demo only) |
```
**Score: 1/5** — No BFloat16 API usage in code.  
**Verdict:** Missing a priority numeric API.

### 3. Rune-Based String Operations [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 2 |
| **Mean** | **2.0** |

#### Analysis

```csharp
// dotnet-net11-skill (webapi/FeatureCoverage/Net11PriorityApiDemo.cs)
var text = "ProductHub \U0001F680 Catalog";
var rocketRune = new Rune(0x1F680);
var runeCount = text.EnumerateRunes().Count();
```
**Score: 2/5** — Rune appears, but not the new `string.*(Rune)` style APIs called out in rubric.  
**Verdict:** Partial Unicode modernization.

### 4. HMAC Single-Step Verification [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```md
<!-- dotnet-net11-skill (webapi/gen-notes.md) -->
| `HMACSHA256.Verify` | Single-step HMAC verify | Not included (demo only) |
```
**Score: 1/5** — Not implemented.  
**Verdict:** Security-related priority API missing.

### 5. FrozenDictionary Collection Expressions [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 2 |
| **Mean** | **2.0** |

#### Analysis

```csharp
// dotnet-net11-skill (webapi/Services/ProductService.cs)
private static readonly FrozenDictionary<string, decimal> DefaultPrices =
    new Dictionary<string, decimal> { ["Electronics"] = 99.99m }.ToFrozenDictionary();
```
**Score: 2/5** — Uses `FrozenDictionary`, but old `Dictionary + ToFrozenDictionary()` path.  
**Verdict:** Functional but not .NET 11-first idiom.

### 6. Collection Expression with() Arguments [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

```csharp
// dotnet-net11-skill (webapi/FeatureCoverage/Net11PriorityApiDemo.cs)
List<int> values = [with(capacity: 16), ..sourceValues];
```
**Score: 5/5** — Correct C# 15 collection expression with `with(capacity:)`.  
**Verdict:** Excellent adoption.

### 7. Union Type Usage [CRITICAL × 3]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```csharp
// dotnet-net11-skill (webapi/Models/ApiResult.cs)
public abstract record ApiResult<T>
{
    public sealed record Success(T Value) : ApiResult<T>;
}
```
**Score: 1/5** — Uses class hierarchy fallback; no `union` keyword/types.  
**Verdict:** Misses core language feature target.

### 8. MediaTypeMap Usage [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 2 |
| **Mean** | **2.0** |

#### Analysis

```csharp
// dotnet-net11-skill (webapi/Endpoints/MimeEndpoints.cs)
private static readonly FileExtensionContentTypeProvider ContentTypeProvider = new();
```
**Score: 2/5** — Built-in ASP.NET provider, but not `MediaTypeMap.GetMediaType/GetExtension`.  
**Verdict:** Acceptable fallback, not target API.

### 9. DivisionRounding Modes [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```md
<!-- dotnet-net11-skill (webapi/gen-notes.md) -->
| `DivisionRounding` | `int.Divide(..., DivisionRounding.*)` | Not included (demo only) |
```
**Score: 1/5** — Not implemented.  
**Verdict:** Missing.

### 10. System.Text.Json New Features [CRITICAL × 3]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 3 |
| **Mean** | **3.0** |

#### Analysis

```csharp
// dotnet-net11-skill (webapi/Models/Product.cs)
[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
public IReadOnlySet<string> Tags { get; set; } = new HashSet<string>();
[JsonPropertyName("metadata")] public Dictionary<string, object>? Metadata { get; set; }
```
```csharp
// dotnet-net11-skill (webapi/Endpoints/ProductEndpoints.cs)
var typeInfo = JsonSerializerOptions.Default.GetTypeInfo<Product>();
```
**Score: 3/5** — Good coverage of `IReadOnlySet`, type-level ignore, and generic `GetTypeInfo<T>()`; does not use `JsonNamingPolicy.PascalCase`/`[JsonNamingPolicy]`.  
**Verdict:** Mixed; solid partial adoption.

### 11. RegexOptions.AnyNewLine [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```text
// dotnet-net11-skill (run-1/webapi)
No RegexOptions.AnyNewLine usage found in source files.
```
**Score: 1/5** — Missing.  
**Verdict:** No Unicode newline handling via new regex option.

### 12. File System New APIs [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```text
// dotnet-net11-skill (run-1/webapi)
No File.CreateHardLink / File.OpenNullHandle / CreateAnonymousPipe usage found.
```
**Score: 1/5** — Missing.  
**Verdict:** No new FS API adoption.

### 13. Base64 Parity APIs [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 2 |
| **Mean** | **2.0** |

#### Analysis

```csharp
// dotnet-net11-skill (webapi/Endpoints/ExportEndpoints.cs)
var base64 = Convert.ToBase64String(csvBytes);
```
**Score: 2/5** — Base64 functionality exists, but legacy `Convert` API is used.  
**Verdict:** Works, but not new parity APIs.

### 14. Generic Interlocked Operations [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```csharp
// dotnet-net11-skill (webapi/Services/ProductService.cs)
Id = Interlocked.Increment(ref _nextId),
```
**Score: 1/5** — Uses classic integer interlocked only; no generic enum `Interlocked.And/Or`.  
**Verdict:** Missing target pattern.

### 15. BitArray.PopCount [LOW × 0]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```text
// dotnet-net11-skill (run-1/webapi)
No BitArray.PopCount() usage found.
```
**Score: 1/5** — Missing.  
**Verdict:** Low-priority API not adopted.

### 16. Native OpenTelemetry Tracing [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 2 |
| **Mean** | **2.0** |

#### Analysis

```csharp
// dotnet-net11-skill (webapi/Program.cs)
builder.Logging.AddConsole();
var activitySource = new System.Diagnostics.ActivitySource("ProductHub");
builder.Services.AddSingleton(activitySource);
```
**Score: 2/5** — Tracing intent exists, but does not show `AddSource("Microsoft.AspNetCore")` path from rubric.  
**Verdict:** Partial observability; misses expected native integration style.

### 17. OpenAPI Version [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

```csharp
// dotnet-net11-skill (webapi/Program.cs)
options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_2;
```
**Score: 5/5** — Exact OpenAPI 3.2 target used.  
**Verdict:** Excellent.

### 18. Dynamic Output Cache Policy Provider [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

```csharp
// dotnet-net11-skill (webapi/Program.cs)
builder.Services.AddOutputCache();
builder.Services.AddSingleton<IOutputCachePolicyProvider, CatalogOutputCachePolicyProvider>();
```
```csharp
// dotnet-net11-skill (webapi/Services/CatalogOutputCachePolicyProvider.cs)
public ValueTask<IOutputCachePolicy?> GetPolicyAsync(string policyName) { ... }
```
**Score: 5/5** — Correct provider interface and DI-based dynamic resolution.  
**Verdict:** Best-practice architecture.

### 19. Zstandard Response Compression [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

```csharp
// dotnet-net11-skill (webapi/Program.cs)
options.Providers.Add<ZstandardCompressionProvider>();
builder.Services.AddRequestDecompression();
app.UseRequestDecompression();
app.UseResponseCompression();
```
**Score: 5/5** — Response and request compression are wired with Zstandard provider.  
**Verdict:** Strong modern ASP.NET compression setup.

### 20. Blazor EnvironmentBoundary Component [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```text
// dotnet-net11-skill (run-1 contents)
apicheck/
copilot-chat.md
efcore/
events.jsonl
webapi/
```
**Score: 1/5** — No `blazor/` app present; feature absent.  
**Verdict:** Not implemented.

### 21. Blazor Label and DisplayName Components [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```text
// dotnet-net11-skill (run-1 contents)
No Blazor component source files present.
```
**Score: 1/5** — Missing with absent Blazor scenario.  
**Verdict:** Not implemented.

### 22. QuickGrid OnRowClick [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```text
// dotnet-net11-skill (run-1 contents)
No .razor files detected under run-1.
```
**Score: 1/5** — No QuickGrid usage exists.  
**Verdict:** Not implemented.

### 23. RelativeToCurrentUri Navigation [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```text
// dotnet-net11-skill (run-1 contents)
No Blazor navigation code found.
```
**Score: 1/5** — Missing due absent Blazor app.  
**Verdict:** Not implemented.

### 24. Blazor TempData Support [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```text
// dotnet-net11-skill (run-1 contents)
No Blazor SSR TempData usage found.
```
**Score: 1/5** — Missing.  
**Verdict:** Not implemented.

### 25. Blazor BasePath Component [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```text
// dotnet-net11-skill (run-1 contents)
No Blazor app files; <BasePath /> not present.
```
**Score: 1/5** — Missing.  
**Verdict:** Not implemented.

### 26. EF Core GetEntriesForState [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```ini
; dotnet-net11-skill (efcore/OrderVault/OrderVault.csproj.lscache)
[sourceFiles]
obj/Debug/net11.0/
 .NETCoreApp,Version=v11.0.AssemblyAttributes.cs
 OrderVault.AssemblyInfo.cs
 OrderVault.GlobalUsings.g.cs
```
**Score: 1/5** — EF Core source implementation is absent; target API not demonstrated.  
**Verdict:** Not implemented.

### 27. EF Core RemoveDbContext [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```ini
; dotnet-net11-skill (efcore/OrderVault/OrderVault.csproj.lscache)
[sourceFiles]
obj/Debug/net11.0/
 OrderVault.AssemblyInfo.cs
```
**Score: 1/5** — No `RemoveDbContext<T>()` pattern in available code.  
**Verdict:** Not implemented.

### 28. EF Core ExcludeForeignKeyFromMigrations [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```text
// dotnet-net11-skill (efcore app folder)
No migration/model source files present to show FK exclusion API.
```
**Score: 1/5** — Missing.  
**Verdict:** Not implemented.

### 29. EF Core JSON Query Functions [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```text
// dotnet-net11-skill (efcore app folder)
No EF.Functions.JsonContains / JsonPathExists usage found.
```
**Score: 1/5** — Missing.  
**Verdict:** Not implemented.

### 30. SignalR ConfigureConnection [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```text
// dotnet-net11-skill (run-1 contents)
No Blazor interactive server configuration found.
```
**Score: 1/5** — Missing.  
**Verdict:** Not implemented.

### 31. Blazor Virtualize Variable-Height Items [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```text
// dotnet-net11-skill (run-1 contents)
No <Virtualize> components present.
```
**Score: 1/5** — Missing.  
**Verdict:** Not implemented.

### 32. Runtime Async Configuration [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```xml
<!-- dotnet-net11-skill (webapi/ProductHub.csproj) -->
<PropertyGroup>
  <TargetFramework>net11.0</TargetFramework>
  <LangVersion>preview</LangVersion>
</PropertyGroup>
```
**Score: 1/5** — No `<Features>runtime-async=on</Features>` configuration.  
**Verdict:** Not implemented.

### 33. ProcessExitStatus Usage [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```text
// dotnet-net11-skill (run-1/webapi)
No process.ExitStatus access patterns found.
```
**Score: 1/5** — Missing structured process status usage.  
**Verdict:** Not implemented.

### 34. OpenAPI Binary File Response [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

```csharp
// dotnet-net11-skill (webapi/Endpoints/ExportEndpoints.cs)
.Produces<FileContentResult>(200, contentType: "application/octet-stream");
```
**Score: 5/5** — Correct binary download metadata for OpenAPI.  
**Verdict:** Excellent API docs practice.

### 35. Brotli and Compression Options [LOW × 0]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```csharp
// dotnet-net11-skill (webapi/Program.cs)
options.Providers.Add<BrotliCompressionProvider>();
```
**Score: 1/5** — Brotli provider enabled, but no new options like `WindowLog`.  
**Verdict:** Legacy/basic usage only.

### 36. Vector Constants [LOW × 0]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

```text
// dotnet-net11-skill (run-1/webapi)
No Vector<float>.Pi/E/Tau/etc usage found.
```
**Score: 1/5** — Missing.  
**Verdict:** Low-priority SIMD constants not adopted.

### 37. Overall .NET 11 API Adoption Rate [CRITICAL × 3]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 2 |
| **Mean** | **2.0** |

#### Analysis

```md
<!-- dotnet-net11-skill -->
Strong: OpenAPI 3.2, dynamic output cache provider, Zstd response/request compression, collection with().
Weak/missing: union keyword, BFloat16, HMAC verify, DivisionRounding, MediaTypeMap, EF/Blazor scenario features.
```
**Score: 2/5** — Some high-value APIs are correct in `webapi`, but large portions of requested .NET 11 surface are absent or fallback-based, and two scenario apps are missing.  
**Verdict:** Partial adoption with major coverage gaps.

---

## Asset Usage Summary

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| dotnet-net11-skill | 1 | a1c49ea3…62a3 | claude-opus-4.6 | — | — | ✅ |

---

## Raw Data References

- Per-run analysis: `reports/analysis-run-1.md`
- Verification data: `reports/verification-data.json`
- Score data: `reports/scores-data.json`
- Build notes: `reports/build-notes.md`
- Generation usage: `reports/generation-usage.json`
