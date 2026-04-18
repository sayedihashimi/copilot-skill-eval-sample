# Aggregated Analysis: .NET 11 Feature Adoption Evaluation

**Runs:** 3 | **Configurations:** 1 | **Scenarios:** 4 | **Dimensions:** 37
**Date:** 2026-04-17 20:40 UTC

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
| Zstandard Compression Usage [CRITICAL] | 4.7 ± 0.6 |
| BFloat16 Type Usage [HIGH] | 4.0 |
| Rune-Based String Operations [HIGH] | 4.3 ± 0.6 |
| HMAC Single-Step Verification [HIGH] | 5.0 |
| FrozenDictionary Collection Expressions [HIGH] | 2.0 |
| Collection Expression with() Arguments [HIGH] | 1.3 ± 0.6 |
| Union Type Usage [CRITICAL] | 1.7 ± 0.6 |
| MediaTypeMap Usage [MEDIUM] | 5.0 |
| DivisionRounding Modes [MEDIUM] | 3.0 ± 1.7 |
| System.Text.Json New Features [CRITICAL] | 3.7 ± 1.5 |
| RegexOptions.AnyNewLine [MEDIUM] | 1.3 ± 0.6 |
| File System New APIs [HIGH] | 1.7 ± 1.2 |
| Base64 Parity APIs [MEDIUM] | 3.7 ± 2.3 |
| Generic Interlocked Operations [MEDIUM] | 2.3 ± 2.3 |
| BitArray.PopCount [LOW] | 2.3 ± 2.3 |
| Native OpenTelemetry Tracing [HIGH] | 1.3 ± 0.6 |
| OpenAPI Version [MEDIUM] | 1.3 ± 0.6 |
| Dynamic Output Cache Policy Provider [HIGH] | 2.3 ± 2.3 |
| Zstandard Response Compression [HIGH] | 2.3 ± 2.3 |
| Blazor EnvironmentBoundary Component [HIGH] | 1.0 |
| Blazor Label and DisplayName Components [HIGH] | 1.0 |
| QuickGrid OnRowClick [HIGH] | 1.0 |
| RelativeToCurrentUri Navigation [MEDIUM] | 1.0 |
| Blazor TempData Support [HIGH] | 1.0 |
| Blazor BasePath Component [MEDIUM] | 1.0 |
| EF Core GetEntriesForState [HIGH] | 1.3 ± 0.6 |
| EF Core RemoveDbContext [HIGH] | 1.3 ± 0.6 |
| EF Core ExcludeForeignKeyFromMigrations [MEDIUM] | 1.3 ± 0.6 |
| EF Core JSON Query Functions [HIGH] | 1.3 ± 0.6 |
| SignalR ConfigureConnection [MEDIUM] | 1.0 |
| Blazor Virtualize Variable-Height Items [MEDIUM] | 1.0 |
| Runtime Async Configuration [MEDIUM] | 1.0 |
| ProcessExitStatus Usage [MEDIUM] | 1.3 ± 0.6 |
| OpenAPI Binary File Response [MEDIUM] | 2.3 ± 2.3 |
| Brotli and Compression Options [LOW] | 1.3 ± 0.6 |
| Vector Constants [LOW] | 1.3 ± 0.6 |
| Overall .NET 11 API Adoption Rate [CRITICAL] | 2.0 |

---

## Final Rankings

Configurations ranked by mean weighted score — **higher is better**. Std Dev shows run-to-run variability (lower = more consistent).

| Rank | Configuration | Mean Score ↑ | % of Max (297.5) | Std Dev ↓ | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | dotnet-net11-skill | 129.8 | 44% | 15.9 | 111.5 | 140.5 |

---

## Weighted Score per Run

| Run | dotnet-net11-skill |
|---|---|
| 1 | 111.5 |
| 2 | 137.5 |
| 3 | 140.5 |
| **Mean** | **129.8** |

---

## Verification Summary (All Runs)

| Configuration | Build Pass Rate | Run Pass Rate | Avg Warnings |
|---|---|---|---|
| dotnet-net11-skill | 3/6 (50%) | 3/6 (50%) | 0.0 |

---

## Token Usage Summary

Average token consumption per configuration across all runs.

| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time |
|---|---|---|---|---|---|
| dotnet-net11-skill | 2,531,170 | 38,852 | 2,399,316 | 45 | 12m 31s |

---

## Token Usage Per Run

| Configuration | Run | Scenario | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time | Note |
|---|---|---|---|---|---|---|---|---|
| dotnet-net11-skill | 1 | efcore | 1,756,752 | 31,342 | 1,639,598 | 34 | 10m 51s |  |
| dotnet-net11-skill | 2 | console-bcl | 2,368,679 | 54,336 | 2,175,961 | 41 | 13m 37s |  |
| dotnet-net11-skill | 3 | webapi | 3,468,079 | 30,878 | 3,382,389 | 60 | 13m 5s |  |


---

## Consistency Analysis

Score σ (standard deviation) measures how much a configuration's weighted score varies across runs — **lower is better**, meaning the configuration produces more reliable, repeatable results.

| Configuration | Score σ (lower = more consistent) | Most Consistent Dim (σ) | Most Variable Dim (σ) |
|---|---|---|---|
| dotnet-net11-skill | 15.9 | BFloat16 Type Usage (0.0) | Base64 Parity APIs (2.3) |

---

## Per-Dimension Analysis

### 1. Zstandard Compression Usage [CRITICAL × 3]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 4 |
| 2 | 5 |
| 3 | 5 |
| **Mean** | **4.7** |

#### Analysis

**Implementation**
```csharp
// dotnet-net11-skill - Demos/CompressionBenchmark.cs
BenchmarkAlgorithm("Zstandard (default)", originalBytes,
    (output) => new ZstandardStream(output, new ZstandardCompressionOptions()),
    (input) => new ZstandardStream(input, CompressionMode.Decompress));
```
**Score**: `dotnet-net11-skill: 5/5` — native `ZstandardStream` + `ZstandardCompressionOptions`, no third-party package.  
**Verdict**: Best-practice .NET 11 usage.

### 2. BFloat16 Type Usage [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 4 |
| 2 | 4 |
| 3 | 4 |
| **Mean** | **4.0** |

#### Analysis

**Implementation**
```csharp
// dotnet-net11-skill - Demos/BFloat16Demo.cs
BFloat16 a = (BFloat16)3.14f;
BFloat16 b = (BFloat16)2.0f;
BFloat16 sum = a + b;
```
**Score**: `dotnet-net11-skill: 4/5` — native `BFloat16` is used well, but no `WriteBFloat16LittleEndian`/`BFloat16ToInt16Bits` APIs.  
**Verdict**: Strong adoption, not maximal.

### 3. Rune-Based String Operations [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 4 |
| 2 | 5 |
| 3 | 4 |
| **Mean** | **4.3** |

#### Analysis

**Implementation**
```csharp
// dotnet-net11-skill - Demos/UnicodeTextProcessor.cs
Console.WriteLine(text.Contains(rocket));
int idx = text.IndexOf(rocket);
var replaced = text.Replace(rocket, star);
foreach (var part in splitText.Split(rocket)) { ... }
```
**Score**: `dotnet-net11-skill: 5/5` — direct Rune APIs across search/replace/split/enumeration.  
**Verdict**: Excellent Unicode-safe approach.

### 4. HMAC Single-Step Verification [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 5 |
| 2 | 5 |
| 3 | 5 |
| **Mean** | **5.0** |

#### Analysis

**Implementation**
```csharp
// dotnet-net11-skill - Demos/CryptoHashVerifier.cs
bool valid256 = HMACSHA256.Verify(key, data, mac256);
bool agnosticValid = CryptographicOperations.VerifyHmac(
    HashAlgorithmName.SHA256, key, data, mac256);
```
**Score**: `dotnet-net11-skill: 5/5` — uses single-step verify APIs correctly.  
**Verdict**: Secure modern pattern.

### 5. FrozenDictionary Collection Expressions [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 2 |
| 2 | 2 |
| 3 | 2 |
| **Mean** | **2.0** |

#### Analysis

**Implementation**
```csharp
// dotnet-net11-skill - Demos/ImmutableLookupTables.cs
FrozenDictionary<string, int> httpStatus = new Dictionary<string, int>
{
    ["OK"] = 200, ["Created"] = 201
}.ToFrozenDictionary();
```
**Score**: `dotnet-net11-skill: 2/5` — works, but uses old `Dictionary + ToFrozenDictionary()` pattern.  
**Verdict**: Functional but not latest collection-expression style.

### 6. Collection Expression with() Arguments [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| 2 | 2 |
| 3 | 1 |
| **Mean** | **1.3** |

#### Analysis

**Implementation**
```csharp
// dotnet-net11-skill - Demos/CollectionWithDemo.cs
List<int> preAllocated = new(capacity: 64);
preAllocated.AddRange(first);
preAllocated.AddRange(second);
```
**Score**: `dotnet-net11-skill: 2/5` — no C# 15 `with(capacity: ...)` in collection expressions.  
**Verdict**: Uses older constructor/add workflow.

### 7. Union Type Usage [CRITICAL × 3]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 2 |
| 2 | 2 |
| 3 | 1 |
| **Mean** | **1.7** |

#### Analysis

**Implementation**
```csharp
// dotnet-net11-skill - Demos/UnionTypesDemo.cs
public abstract record Shape
{
    public sealed record Circle(double Radius) : Shape;
}
```
**Score**: `dotnet-net11-skill: 2/5` — class hierarchy fallback, no `union` keyword.  
**Verdict**: Adequate fallback, misses target language feature.

### 8. MediaTypeMap Usage [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 5 |
| 2 | 5 |
| 3 | 5 |
| **Mean** | **5.0** |

#### Analysis

**Implementation**
```csharp
// dotnet-net11-skill - Demos/MimeTypeResolver.cs
var mediaType = MediaTypeMap.GetMediaType(ext);
var extension = MediaTypeMap.GetExtension(mime);
```
**Score**: `dotnet-net11-skill: 5/5` — direct built-in MIME map APIs.  
**Verdict**: Exactly aligned with rubric.

### 9. DivisionRounding Modes [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 2 |
| 2 | 5 |
| 3 | 2 |
| **Mean** | **3.0** |

#### Analysis

**Implementation**
```csharp
// dotnet-net11-skill - Demos/IntegerMathToolkit.cs
=> T.Divide(left, right, mode);
=> T.Remainder(left, right, mode);
=> T.DivRem(left, right, mode);
```
**Score**: `dotnet-net11-skill: 5/5` — modern `DivisionRounding` via `IBinaryInteger<T>`.  
**Verdict**: Correct and robust.

### 10. System.Text.Json New Features [CRITICAL × 3]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 2 |
| 2 | 4 |
| 3 | 5 |
| **Mean** | **3.7** |

#### Analysis

**Implementation**
```csharp
// dotnet-net11-skill - Demos/JsonSerializationShowcase.cs
PropertyNamingPolicy = JsonNamingPolicy.PascalCase;
[JsonNamingPolicy(JsonKnownNamingPolicy.CamelCase)]
[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
public IReadOnlySet<string> Tags { get; set; }
```
**Score**: `dotnet-net11-skill: 4/5` — strong usage, but metadata access still uses `resolver.GetTypeInfo(typeof(...))` cast instead of generic API.  
**Verdict**: Good modern JSON adoption with one legacy edge.

### 11. RegexOptions.AnyNewLine [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| 2 | 2 |
| 3 | 1 |
| **Mean** | **1.3** |

#### Analysis

**Implementation**
```csharp
// dotnet-net11-skill - Demos/UniversalNewlineRegex.cs
[GeneratedRegex(@"^.+$", RegexOptions.Multiline)]
[GeneratedRegex(@"\r\n|[\n\r\u0085\u2028\u2029]")]
```
**Score**: `dotnet-net11-skill: 2/5` — manual newline handling; does not use `RegexOptions.AnyNewLine`.  
**Verdict**: Works but misses the new API.

### 12. File System New APIs [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| 2 | 3 |
| 3 | 1 |
| **Mean** | **1.7** |

#### Analysis

**Implementation**
```csharp
// dotnet-net11-skill - Demos/FileSystemUtilities.cs
File.CreateHardLink(hardLinkPath, originalPath);
SafeFileHandle.CreateAnonymousPipe(out readHandle, out writeHandle,
    asyncRead: false, asyncWrite: true);
```
**Score**: `dotnet-net11-skill: 3/5` — adopts hard-link and anonymous-pipe APIs, but uses `"NUL"/"/dev/null"` path instead of `File.OpenNullHandle`.  
**Verdict**: Partial modernization.

### 13. Base64 Parity APIs [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| 2 | 5 |
| 3 | 5 |
| **Mean** | **3.7** |

#### Analysis

**Implementation**
```csharp
// dotnet-net11-skill - Demos/Base64Utilities.cs
var base64String = Base64.EncodeToString(originalBytes);
var decoded = Base64.DecodeFromChars(base64String);
var encodedLength = Base64.GetEncodedLength(originalBytes.Length);
```
**Score**: `dotnet-net11-skill: 5/5` — fully uses parity helpers.  
**Verdict**: Excellent adoption.

### 14. Generic Interlocked Operations [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| 2 | 5 |
| 3 | 1 |
| **Mean** | **2.3** |

#### Analysis

**Implementation**
```csharp
// dotnet-net11-skill - Demos/ConcurrentFlagOps.cs
Interlocked.Or(ref permissions, FilePermissions.Write);
Interlocked.And(ref permissions, ~FilePermissions.Write);
```
**Score**: `dotnet-net11-skill: 5/5` — direct enum usage, no int-casting workaround.  
**Verdict**: Correct and clean.

### 15. BitArray.PopCount [LOW × 0]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| 2 | 5 |
| 3 | 1 |
| **Mean** | **2.3** |

#### Analysis

**Implementation**
```csharp
// dotnet-net11-skill - Demos/BitCountingDemo.cs
int count = bits.PopCount();
int largeCount = largeBits.PopCount();
```
**Score**: `dotnet-net11-skill: 5/5` — uses native popcount API directly.  
**Verdict**: Best-practice implementation.

### 16. Native OpenTelemetry Tracing [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| 2 | 1 |
| 3 | 2 |
| **Mean** | **1.3** |

#### Analysis

**Implementation**
```text
# dotnet-net11-skill - output/dotnet-net11-skill/run-2
console-bcl
copilot-chat.md
events.jsonl
```
**Score**: `dotnet-net11-skill: 1/5` — `webapi` app (where tracing setup belongs) is missing in run-2.  
**Verdict**: Not implemented.

### 17. OpenAPI Version [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| 2 | 1 |
| 3 | 2 |
| **Mean** | **1.3** |

#### Analysis

**Implementation**
```text
# dotnet-net11-skill - output/dotnet-net11-skill/run-2
console-bcl
copilot-chat.md
events.jsonl
```
**Score**: `dotnet-net11-skill: 1/5` — no `webapi` source to configure OpenAPI 3.2.  
**Verdict**: Missing.

### 18. Dynamic Output Cache Policy Provider [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| 2 | 1 |
| 3 | 5 |
| **Mean** | **2.3** |

#### Analysis

**Implementation**
```text
# dotnet-net11-skill - output/dotnet-net11-skill/run-2
console-bcl
copilot-chat.md
events.jsonl
```
**Score**: `dotnet-net11-skill: 1/5` — no `webapi` implementation present.  
**Verdict**: Missing.

### 19. Zstandard Response Compression [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| 2 | 1 |
| 3 | 5 |
| **Mean** | **2.3** |

#### Analysis

**Implementation**
```text
# dotnet-net11-skill - output/dotnet-net11-skill/run-2
console-bcl
copilot-chat.md
events.jsonl
```
**Score**: `dotnet-net11-skill: 1/5` — web API compression config absent because app is absent.  
**Verdict**: Missing.

### 20. Blazor EnvironmentBoundary Component [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| 2 | 1 |
| 3 | 1 |
| **Mean** | **1.0** |

#### Analysis

**Implementation**
```text
# dotnet-net11-skill - output/dotnet-net11-skill/run-2
console-bcl
copilot-chat.md
events.jsonl
```
**Score**: `dotnet-net11-skill: 1/5` — `blazor` app missing.  
**Verdict**: Missing.

### 21. Blazor Label and DisplayName Components [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| 2 | 1 |
| 3 | 1 |
| **Mean** | **1.0** |

#### Analysis

**Implementation**
```text
# dotnet-net11-skill - output/dotnet-net11-skill/run-2
console-bcl
copilot-chat.md
events.jsonl
```
**Score**: `dotnet-net11-skill: 1/5` — no Blazor source exists in run-2.  
**Verdict**: Missing.

### 22. QuickGrid OnRowClick [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| 2 | 1 |
| 3 | 1 |
| **Mean** | **1.0** |

#### Analysis

**Implementation**
```text
# dotnet-net11-skill - output/dotnet-net11-skill/run-2
console-bcl
copilot-chat.md
events.jsonl
```
**Score**: `dotnet-net11-skill: 1/5` — no QuickGrid app code in run-2.  
**Verdict**: Missing.

### 23. RelativeToCurrentUri Navigation [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| 2 | 1 |
| 3 | 1 |
| **Mean** | **1.0** |

#### Analysis

**Implementation**
```text
# dotnet-net11-skill - output/dotnet-net11-skill/run-2
console-bcl
copilot-chat.md
events.jsonl
```
**Score**: `dotnet-net11-skill: 1/5` — Blazor navigation features cannot be observed.  
**Verdict**: Missing.

### 24. Blazor TempData Support [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| 2 | 1 |
| 3 | 1 |
| **Mean** | **1.0** |

#### Analysis

**Implementation**
```text
# dotnet-net11-skill - output/dotnet-net11-skill/run-2
console-bcl
copilot-chat.md
events.jsonl
```
**Score**: `dotnet-net11-skill: 1/5` — no Blazor SSR app present.  
**Verdict**: Missing.

### 25. Blazor BasePath Component [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| 2 | 1 |
| 3 | 1 |
| **Mean** | **1.0** |

#### Analysis

**Implementation**
```text
# dotnet-net11-skill - output/dotnet-net11-skill/run-2
console-bcl
copilot-chat.md
events.jsonl
```
**Score**: `dotnet-net11-skill: 1/5` — missing `blazor` scenario implementation.  
**Verdict**: Missing.

### 26. EF Core GetEntriesForState [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 2 |
| 2 | 1 |
| 3 | 1 |
| **Mean** | **1.3** |

#### Analysis

**Implementation**
```text
# dotnet-net11-skill - output/dotnet-net11-skill/run-2
console-bcl
copilot-chat.md
events.jsonl
```
**Score**: `dotnet-net11-skill: 1/5` — `efcore` app missing.  
**Verdict**: Missing.

### 27. EF Core RemoveDbContext [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 2 |
| 2 | 1 |
| 3 | 1 |
| **Mean** | **1.3** |

#### Analysis

**Implementation**
```text
# dotnet-net11-skill - output/dotnet-net11-skill/run-2
console-bcl
copilot-chat.md
events.jsonl
```
**Score**: `dotnet-net11-skill: 1/5` — no EF Core setup available in this run.  
**Verdict**: Missing.

### 28. EF Core ExcludeForeignKeyFromMigrations [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 2 |
| 2 | 1 |
| 3 | 1 |
| **Mean** | **1.3** |

#### Analysis

**Implementation**
```text
# dotnet-net11-skill - output/dotnet-net11-skill/run-2
console-bcl
copilot-chat.md
events.jsonl
```
**Score**: `dotnet-net11-skill: 1/5` — migration API usage cannot be observed.  
**Verdict**: Missing.

### 29. EF Core JSON Query Functions [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 2 |
| 2 | 1 |
| 3 | 1 |
| **Mean** | **1.3** |

#### Analysis

**Implementation**
```text
# dotnet-net11-skill - output/dotnet-net11-skill/run-2
console-bcl
copilot-chat.md
events.jsonl
```
**Score**: `dotnet-net11-skill: 1/5` — no EF Core JSON querying code present.  
**Verdict**: Missing.

### 30. SignalR ConfigureConnection [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| 2 | 1 |
| 3 | 1 |
| **Mean** | **1.0** |

#### Analysis

**Implementation**
```text
# dotnet-net11-skill - output/dotnet-net11-skill/run-2
console-bcl
copilot-chat.md
events.jsonl
```
**Score**: `dotnet-net11-skill: 1/5` — no Blazor interactive-server code available.  
**Verdict**: Missing.

### 31. Blazor Virtualize Variable-Height Items [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| 2 | 1 |
| 3 | 1 |
| **Mean** | **1.0** |

#### Analysis

**Implementation**
```text
# dotnet-net11-skill - output/dotnet-net11-skill/run-2
console-bcl
copilot-chat.md
events.jsonl
```
**Score**: `dotnet-net11-skill: 1/5` — virtualization scenario not generated.  
**Verdict**: Missing.

### 32. Runtime Async Configuration [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| 2 | 1 |
| 3 | 1 |
| **Mean** | **1.0** |

#### Analysis

**Implementation**
```xml
<!-- dotnet-net11-skill - DevToolkit.csproj -->
<TargetFramework>net11.0</TargetFramework>
<LangVersion>preview</LangVersion>
```
**Score**: `dotnet-net11-skill: 1/5` — no `<Features>runtime-async=on</Features>` configuration present.  
**Verdict**: Missing target configuration.

### 33. ProcessExitStatus Usage [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| 2 | 2 |
| 3 | 1 |
| **Mean** | **1.3** |

#### Analysis

**Implementation**
```csharp
// dotnet-net11-skill - Demos/ProcessExitInfo.cs
Console.WriteLine($"  ExitCode: {process.ExitCode}");
Console.WriteLine($"  HasExited: {process.HasExited}");
```
**Score**: `dotnet-net11-skill: 2/5` — uses classic `ExitCode`/`HasExited`, not rich `process.ExitStatus`.  
**Verdict**: Functional but outdated API surface.

### 34. OpenAPI Binary File Response [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| 2 | 1 |
| 3 | 5 |
| **Mean** | **2.3** |

#### Analysis

**Implementation**
```text
# dotnet-net11-skill - output/dotnet-net11-skill/run-2
console-bcl
copilot-chat.md
events.jsonl
```
**Score**: `dotnet-net11-skill: 1/5` — no Web API endpoints or OpenAPI metadata in run-2.  
**Verdict**: Missing.

### 35. Brotli and Compression Options [LOW × 0]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| 2 | 2 |
| 3 | 1 |
| **Mean** | **1.3** |

#### Analysis

**Implementation**
```csharp
// dotnet-net11-skill - Demos/CompressionBenchmark.cs
BenchmarkAlgorithm("Brotli", originalBytes,
    (output) => new BrotliStream(output, CompressionLevel.Optimal),
    (input) => new BrotliStream(input, CompressionMode.Decompress));
```
**Score**: `dotnet-net11-skill: 2/5` — Brotli is used, but not new option properties like `BrotliCompressionOptions.WindowLog`.  
**Verdict**: Basic compression usage only.

### 36. Vector Constants [LOW × 0]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| 2 | 2 |
| 3 | 1 |
| **Mean** | **1.3** |

#### Analysis

**Implementation**
```csharp
// dotnet-net11-skill - Demos/BFloat16Demo.cs
Console.WriteLine($"    AllBitsSet: {Vector<float>.AllBitsSet}");
Console.WriteLine($"    One: {Vector<float>.One}");
Console.WriteLine($"    Zero: {Vector<float>.Zero}");
```
**Score**: `dotnet-net11-skill: 2/5` — uses standard constants, not newer math constants (`Pi`, `E`, `Tau`, etc.).  
**Verdict**: Partial SIMD API adoption.

### 37. Overall .NET 11 API Adoption Rate [CRITICAL × 3]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 2 |
| 2 | 2 |
| 3 | 2 |
| **Mean** | **2.0** |

#### Analysis

**Implementation**
```text
# dotnet-net11-skill - run-2 structure
Implemented app: console-bcl
Missing apps: webapi, blazor, efcore
```
**Score**: `dotnet-net11-skill: 2/5` — console app demonstrates many .NET 11 APIs well, but three required scenario apps are absent, heavily reducing overall adoption coverage.  
**Verdict**: Good depth in one scenario, low breadth across required scenarios.

---

## Asset Usage Summary

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| dotnet-net11-skill | 1 | f050d290…e4cf | claude-opus-4.6 | — | — | ✅ |
| dotnet-net11-skill | 2 | c1add1c3…2cfc | claude-opus-4.6 | — | — | ✅ |
| dotnet-net11-skill | 3 | 66727453…6720 | claude-opus-4.6 | — | — | ✅ |

---

## Raw Data References

- Per-run analysis: `reports/analysis-run-1.md`
- Per-run analysis: `reports/analysis-run-2.md`
- Per-run analysis: `reports/analysis-run-3.md`
- Verification data: `reports/verification-data.json`
- Score data: `reports/scores-data.json`
- Build notes: `reports/build-notes.md`
- Generation usage: `reports/generation-usage.json`
