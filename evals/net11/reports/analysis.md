# Aggregated Analysis: .NET 11 Feature Adoption Evaluation

**Runs:** 1 | **Configurations:** 2 | **Scenarios:** 4 | **Dimensions:** 38
**Date:** 2026-04-17 00:04 UTC

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
| no-skills | Baseline (default Copilot) | — | — |
| dotnet-net11-skill | dotnet-net11 Skill | — | dotnet-net11:dotnet-net11 |

### How It Works

1. **Generate** — For each configuration, Copilot CLI (`copilot --yolo`) is given a scenario prompt and generates a complete project from scratch. One scenario is randomly selected per run.
2. **Verify** — Each generated project is built (`dotnet build`), run, format-checked, and scanned for vulnerabilities.
3. **Analyze** — An AI judge reviews the source code of all configurations side-by-side and scores each across 38 quality dimensions.

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
| MEDIUM | ×1 | 15 |
| LOW | ×0.5 | 3 |

**Maximum possible weighted score: 302.5** (all dimensions scoring 5).
Scores shown as **mean ± standard deviation** across runs.

---

## Executive Summary

Mean dimension scores across runs (1–5 scale, **higher is better**). ± values show standard deviation across runs.

| Dimension [Tier] | no-skills | dotnet-net11-skill |
|---|---|---|
| Zstandard Compression Usage [CRITICAL] | 5.0 | 5.0 |
| BFloat16 Type Usage [HIGH] | 3.0 | 5.0 |
| Rune-Based String Operations [HIGH] | 3.0 | 5.0 |
| HMAC Single-Step Verification [HIGH] | 4.0 | 5.0 |
| FrozenDictionary Collection Expressions [HIGH] | 2.0 | 2.0 |
| Collection Expression with() Arguments [HIGH] | 1.0 | 1.0 |
| Union Type Usage [CRITICAL] | 1.0 | 2.0 |
| MediaTypeMap Usage [MEDIUM] | 2.0 | 5.0 |
| DivisionRounding Modes [MEDIUM] | 2.0 | 5.0 |
| System.Text.Json New Features [CRITICAL] | 3.0 | 5.0 |
| RegexOptions.AnyNewLine [MEDIUM] | 2.0 | 5.0 |
| File System New APIs [HIGH] | 3.0 | 5.0 |
| Base64 Parity APIs [MEDIUM] | 5.0 | 5.0 |
| Generic Interlocked Operations [MEDIUM] | 5.0 | 5.0 |
| BitArray.PopCount [LOW] | 5.0 | 5.0 |
| Native OpenTelemetry Tracing [HIGH] | 1.0 | 1.0 |
| OpenAPI Version [MEDIUM] | 1.0 | 1.0 |
| Dynamic Output Cache Policy Provider [HIGH] | 1.0 | 1.0 |
| Zstandard Response Compression [HIGH] | 1.0 | 1.0 |
| Blazor EnvironmentBoundary Component [HIGH] | 1.0 | 1.0 |
| Blazor Label and DisplayName Components [HIGH] | 1.0 | 1.0 |
| QuickGrid OnRowClick [HIGH] | 1.0 | 1.0 |
| RelativeToCurrentUri Navigation [MEDIUM] | 1.0 | 1.0 |
| Blazor TempData Support [HIGH] | 1.0 | 1.0 |
| Blazor BasePath Component [MEDIUM] | 1.0 | 1.0 |
| EF Core GetEntriesForState [HIGH] | 1.0 | 1.0 |
| EF Core RemoveDbContext [HIGH] | 1.0 | 1.0 |
| EF Core ExcludeForeignKeyFromMigrations [MEDIUM] | 1.0 | 1.0 |
| EF Core JSON Query Functions [HIGH] | 1.0 | 1.0 |
| SignalR ConfigureConnection [MEDIUM] | 1.0 | 1.0 |
| Blazor Virtualize Variable-Height Items [MEDIUM] | 1.0 | 1.0 |
| Runtime Async Configuration [MEDIUM] | 1.0 | 1.0 |
| ProcessExitStatus Usage [MEDIUM] | 2.0 | 2.0 |
| OpenAPI Binary File Response [MEDIUM] | 1.0 | 1.0 |
| Brotli and Compression Options [LOW] | 3.0 | 3.0 |
| Vector Constants [LOW] | 1.0 | 1.0 |
| Overall .NET 11 API Adoption Rate [CRITICAL] | 2.0 | 3.0 |
| Token Efficiency [MEDIUM] | 5.0 | 2.0 |

---

## Final Rankings

Configurations ranked by mean weighted score — **higher is better**. Std Dev shows run-to-run variability (lower = more consistent).

| Rank | Configuration | Mean Score ↑ | % of Max (302.5) | Std Dev ↓ | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | dotnet-net11-skill | 152.5 | 50% | 0.0 | 152.5 | 152.5 |
| 🥈 | no-skills | 120.5 | 40% | 0.0 | 120.5 | 120.5 |

---

## Weighted Score per Run

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 120.5 | 152.5 |
| **Mean** | **120.5** | **152.5** |

---

## Verification Summary (All Runs)

| Configuration | Build Pass Rate | Run Pass Rate | Avg Warnings |
|---|---|---|---|
| no-skills | 1/1 (100%) | 1/1 (100%) | 0.0 |
| dotnet-net11-skill | 1/1 (100%) | 1/1 (100%) | 2.0 |

---

## Token Usage Summary

Average token consumption per configuration across all runs.

| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time | Δ Input vs Baseline |
|---|---|---|---|---|---|---|
| no-skills | 2,605,136 | 40,621 | 2,480,179 | 45 | 13m 36s | — (baseline) |
| dotnet-net11-skill | 5,424,794 | 37,478 | 5,205,885 | 62 | 17m 24s | +108.2% |

---

## Token Usage Per Run

| Configuration | Run | Scenario | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time | Note |
|---|---|---|---|---|---|---|---|---|
| no-skills | 1 | console-bcl | 2,605,136 | 40,621 | 2,480,179 | 45 | 13m 36s |  |
| dotnet-net11-skill | 1 | console-bcl | 5,424,794 | 37,478 | 5,205,885 | 62 | 17m 24s |  |


---

## Per-Dimension Analysis

### 1. Zstandard Compression Usage [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 5 | 5 |
| **Mean** | **5.0** | **5.0** |

#### Analysis

Both use native `System.IO.Compression.ZstandardStream` and `ZstandardCompressionOptions`.

```csharp
// dotnet-net11-skill: Demos/CompressionBenchmark.cs
var advancedOptions = new ZstandardCompressionOptions { Quality = 5, WindowLog = 22, AppendChecksum = true, EnableLongDistanceMatching = true };
using (var zstdStream = new ZstandardStream(advancedOutput, advancedOptions)) { zstdStream.Write(originalData); }
```

```csharp
// no-skills: Demos/CompressionBenchmark.cs
var zstdOptions = new ZstandardCompressionOptions { Quality = 6, AppendChecksum = true, EnableLongDistanceMatching = true, WindowLog = 22 };
data => Compress(data, stream => new ZstandardStream(stream, zstdOptions))
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 5` — both are fully native and correct.  
**Verdict:** Tie; both align with .NET 11 best practice.

### 2. BFloat16 Type Usage [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 3 | 5 |
| **Mean** | **3.0** | **5.0** |

#### Analysis

`dotnet-net11-skill` uses native arithmetic and `BinaryPrimitives` APIs directly; `no-skills` falls back to float-cast arithmetic and `MemoryMarshal`.

```csharp
// dotnet-net11-skill: Demos/BFloat16Demo.cs
BFloat16 sum = a + b;
BinaryPrimitives.WriteBFloat16LittleEndian(buffer, pi);
BFloat16 readBack = BinaryPrimitives.ReadBFloat16LittleEndian(buffer);
```

```csharp
// no-skills: Demos/MlNumericTypes.cs
var sum = (BFloat16)((float)a + (float)b);
MemoryMarshal.Write(leBytes, in a);
var restored = MemoryMarshal.Read<BFloat16>(leBytes);
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 3`.  
**Verdict:** `dotnet-net11-skill` is better; it uses the new primitive more idiomatically.

### 3. Rune-Based String Operations [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 3 | 5 |
| **Mean** | **3.0** | **5.0** |

#### Analysis

`dotnet-net11-skill` uses new `string`/`StringBuilder` Rune overloads heavily; `no-skills` uses manual rune loops.

```csharp
// dotnet-net11-skill: Demos/UnicodeTextProcessor.cs
Console.WriteLine($"Contains 🌍: {text.Contains(globe)}");
string replaced = text.Replace(globe, new Rune('X'));
Rune runeAt6 = sb.GetRuneAt(6);
```

```csharp
// no-skills: Demos/UnicodeTextProcessor.cs
foreach (var rune in text.EnumerateRunes()) { ... }
var replaced = ReplaceRune(text, earth, star);
private static List<string> SplitByRune(string text, Rune separator) { ... }
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 3`.  
**Verdict:** `dotnet-net11-skill` best demonstrates .NET 11 Rune API surface.

### 4. HMAC Single-Step Verification [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

Both use `Verify`, but `dotnet-net11-skill` also uses `VerifyAsync` and `CryptographicOperations.VerifyHmac`; `no-skills` still includes two-step `FixedTimeEquals`.

```csharp
// dotnet-net11-skill: Demos/CryptoHashVerifier.cs
bool valid256 = HMACSHA256.Verify(key, data, hmac256);
bool streamValid = HMACSHA256.VerifyAsync(key, stream, streamHmac).GetAwaiter().GetResult();
bool agnosticValid = CryptographicOperations.VerifyHmac(HashAlgorithmName.SHA256, key, data, agnosticHmac);
```

```csharp
// no-skills: Demos/CryptoHashVerifier.cs
var verify256 = HMACSHA256.HashData(key, data);
Console.WriteLine($"Constant-time verify: {CryptographicOperations.FixedTimeEquals(hash256, verify256)}");
bool verified256 = HMACSHA256.Verify(key, data, hash256);
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 4`.  
**Verdict:** `dotnet-net11-skill` is cleaner and safer by preferring single-step APIs end-to-end.

### 5. FrozenDictionary Collection Expressions [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 2 | 2 |
| **Mean** | **2.0** | **2.0** |

#### Analysis

Neither uses C# 15 dictionary collection-expression initialization for `FrozenDictionary`.

```csharp
// dotnet-net11-skill: Demos/ImmutableLookupDemo.cs
FrozenDictionary<string, int> httpCodes = new Dictionary<string, int> { ... }.ToFrozenDictionary(StringComparer.Ordinal);
```

```csharp
// no-skills: Demos/ImmutableLookupTables.cs
var httpStatusCodes = new Dictionary<int, string> { ... }.ToFrozenDictionary();
```

**Score:** `dotnet-net11-skill: 2`, `no-skills: 2`.  
**Verdict:** Tie; both use older multi-step construction.

### 6. Collection Expression with() Arguments [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

Neither uses `with(...)` collection-expression constructor arguments.

```csharp
// dotnet-net11-skill: Demos/CollectionExpressionDemo.cs
HashSet<string> caseInsensitive = new(StringComparer.OrdinalIgnoreCase) { "Hello", "World", "HELLO" };
```

```csharp
// no-skills: Demos/CollectionInitialization.cs
Dictionary<string, int> scores = new() { ["Alice"] = 95, ["Bob"] = 87, ["Charlie"] = 92 };
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; both miss the new C# 15 `with()` syntax.

### 7. Union Type Usage [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 2 |
| **Mean** | **1.0** | **2.0** |

#### Analysis

Neither uses `union`; both use record hierarchies. `dotnet-net11-skill` keeps an exhaustive switch without default; `no-skills` adds fallback throw arms.

```csharp
// dotnet-net11-skill: Demos/UnionTypesDemo.cs
public abstract record Shape { public sealed record Circle(double Radius) : Shape; ... }
static double ComputeArea(Shape shape) => shape switch { Shape.Circle c => ..., Shape.Rectangle r => ..., Shape.Triangle t => ... };
```

```csharp
// no-skills: Demos/DiscriminatedUnions.cs
public abstract record Shape { ... }
Shape.Triangle t => 0.5 * t.Base * t.Height,
_ => throw new ArgumentOutOfRangeException(nameof(shape))
```

**Score:** `dotnet-net11-skill: 2`, `no-skills: 1`.  
**Verdict:** `dotnet-net11-skill` is slightly better, but both miss the requested C# 15 union model.

### 8. MediaTypeMap Usage [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 2 | 5 |
| **Mean** | **2.0** | **5.0** |

#### Analysis

`dotnet-net11-skill` correctly uses `MediaTypeMap`; `no-skills` uses manual dictionaries plus `MediaTypeNames`.

```csharp
// dotnet-net11-skill: Demos/MimeTypeResolver.cs
string? mediaType = MediaTypeMap.GetMediaType(ext);
string? ext = MediaTypeMap.GetExtension(mime);
```

```csharp
// no-skills: Demos/MimeTypeResolver.cs
private static readonly Dictionary<string, string> ExtensionToMime = new(StringComparer.OrdinalIgnoreCase) { [".json"] = MediaTypeNames.Application.Json, ... };
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 2`.  
**Verdict:** `dotnet-net11-skill` is decisively better.

### 9. DivisionRounding Modes [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 2 | 5 |
| **Mean** | **2.0** | **5.0** |

#### Analysis

`dotnet-net11-skill` uses `DivisionRounding`; `no-skills` uses manual formulas.

```csharp
// dotnet-net11-skill: Demos/IntegerMathToolkit.cs
foreach (DivisionRounding mode in Enum.GetValues<DivisionRounding>()) { ... }
static T Divide<T>(T left, T right, DivisionRounding mode) where T : IBinaryInteger<T> => T.Divide(left, right, mode);
```

```csharp
// no-skills: Demos/IntegerMathToolkit.cs
var ceilResult = (int)Math.Ceiling((double)dividend / divisor);
var euclideanRemainder = ((dividend % divisor) + divisor) % divisor;
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 2`.  
**Verdict:** `dotnet-net11-skill` follows the new safe API.

### 10. System.Text.Json New Features [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 3 | 5 |
| **Mean** | **3.0** | **5.0** |

#### Analysis

`dotnet-net11-skill` adopts almost the full new feature set; `no-skills` only partial.

```csharp
// dotnet-net11-skill: Demos/JsonSerializationShowcase.cs
[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
[JsonNamingPolicy(JsonKnownNamingPolicy.CamelCase)]
IReadOnlySet<string> tags = new HashSet<string> { "dotnet", "csharp", "net11" };
var typeInfo = metaOptions.GetTypeInfo<EventData>();
```

```csharp
// no-skills: Demos/JsonSerializationShowcase.cs
PropertyNamingPolicy = JsonNamingPolicy.PascalCase,
DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
var typeInfo = options.GetTypeInfo(typeof(UserProfile));
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 3`.  
**Verdict:** `dotnet-net11-skill` is clearly superior and more modern.

### 11. RegexOptions.AnyNewLine [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 2 | 5 |
| **Mean** | **2.0** | **5.0** |

#### Analysis

`dotnet-net11-skill` uses `RegexOptions.AnyNewLine`; `no-skills` manually splits newline classes.

```csharp
// dotnet-net11-skill: Demos/UniversalNewlineRegex.cs
Regex.Matches(text, @"^line\d$", RegexOptions.Multiline | RegexOptions.AnyNewLine);
```

```csharp
// no-skills: Demos/UniversalNewlineRegex.cs
var universalNewlinePattern = new Regex(@"\r\n|\r|\n|\u0085|\u2028|\u2029");
var lines = universalNewlinePattern.Split(text);
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 2`.  
**Verdict:** `dotnet-net11-skill` uses the intended .NET 11 API directly.

### 12. File System New APIs [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 3 | 5 |
| **Mean** | **3.0** | **5.0** |

#### Analysis

`dotnet-net11-skill` uses all requested APIs; `no-skills` only partially.

```csharp
// dotnet-net11-skill: Demos/FileSystemUtilities.cs
FileSystemInfo link = File.CreateHardLink(linkPath, originalPath);
using SafeFileHandle nullHandle = File.OpenNullHandle();
SafeFileHandle.CreateAnonymousPipe(out SafeFileHandle readEnd, out SafeFileHandle writeEnd, asyncRead: true, asyncWrite: false);
```

```csharp
// no-skills: Demos/FileSystemUtilities.cs
File.CreateHardLink(hardLinkPath, originalPath);
using var nullHandle = File.OpenHandle(OperatingSystem.IsWindows() ? "NUL" : "/dev/null", FileMode.Open, FileAccess.Write);
using var server = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.None);
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 3`.  
**Verdict:** `dotnet-net11-skill` better replaces legacy patterns with new BCL APIs.

### 13. Base64 Parity APIs [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 5 | 5 |
| **Mean** | **5.0** | **5.0** |

#### Analysis

Both implement modern Base64 APIs correctly.

```csharp
// dotnet-net11-skill: Demos/Base64Utilities.cs
string encoded = Base64.EncodeToString(data);
byte[] decoded = Base64.DecodeFromChars(encoded);
char[] encodedChars = Base64.EncodeToChars(data);
```

```csharp
// no-skills: Demos/Base64Utilities.cs
var base64String = Base64.EncodeToString(originalBytes);
var decoded = Base64.DecodeFromChars(base64String);
var charsWritten = Base64.EncodeToChars(originalBytes, charBuffer);
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 5`.  
**Verdict:** Tie; both are excellent.

### 14. Generic Interlocked Operations [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 5 | 5 |
| **Mean** | **5.0** | **5.0** |

#### Analysis

Both use generic enum overloads directly.

```csharp
// dotnet-net11-skill: Demos/ConcurrentFlagDemo.cs
FilePermissions oldValue = Interlocked.Or(ref permissions, FilePermissions.Write);
oldValue = Interlocked.And(ref permissions, ~FilePermissions.Write);
```

```csharp
// no-skills: Demos/ConcurrentFlagOperations.cs
Interlocked.Or(ref permissions, Permissions.Write);
Interlocked.And(ref permissions, ~Permissions.Read);
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 5`.  
**Verdict:** Tie; both follow best practice.

### 15. BitArray.PopCount [LOW × 0]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 5 | 5 |
| **Mean** | **5.0** | **5.0** |

#### Analysis

Both correctly use `PopCount()`.

```csharp
// dotnet-net11-skill: Demos/BitCountingDemo.cs
int setBitCount = bits.PopCount();
Console.WriteLine($"  PopCount: {largeBits.PopCount()}");
```

```csharp
// no-skills: Demos/BitCounting.cs
var popCount = bits.PopCount();
Console.WriteLine($"  Large ... PopCount: {large.PopCount()}");
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 5`.  
**Verdict:** Tie; both adopt the new API.

### 16. Native OpenTelemetry Tracing [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

No `webapi` app exists in either run; there is no `AddOpenTelemetry()` usage to evaluate.

```text
# dotnet-net11-skill: output/dotnet-net11-skill/run-1
console-bcl
copilot-chat.md
events.jsonl
```

```text
# no-skills: output/no-skills/run-1
console-bcl
copilot-chat.md
events.jsonl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

### 17. OpenAPI Version [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

No Web API project means no `AddOpenApi(...OpenApi3_2)` usage.

```text
# dotnet-net11-skill run-1
console-bcl
copilot-chat.md
events.jsonl
```

```text
# no-skills run-1
console-bcl
copilot-chat.md
events.jsonl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

### 18. Dynamic Output Cache Policy Provider [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

No Web API project; no `IOutputCachePolicyProvider` implementation.

```text
# dotnet-net11-skill run-1: no webapi directory
console-bcl
```

```text
# no-skills run-1: no webapi directory
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

### 19. Zstandard Response Compression [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

No ASP.NET Core pipeline; no `AddResponseCompression()`/`ZstandardCompressionProviderOptions`.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

### 20. Blazor EnvironmentBoundary Component [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

No Blazor project; no `<EnvironmentBoundary ...>` usage.

```text
# dotnet-net11-skill run-1: no blazor directory
console-bcl
```

```text
# no-skills run-1: no blazor directory
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

### 21. Blazor Label and DisplayName Components [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

No Blazor forms/tables; no `<Label>`/`<DisplayName>`.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

### 22. QuickGrid OnRowClick [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

No Blazor QuickGrid; no `OnRowClick`.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

### 23. RelativeToCurrentUri Navigation [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

No Blazor navigation code.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

### 24. Blazor TempData Support [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

No Blazor SSR code; no `ITempData`.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

### 25. Blazor BasePath Component [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

No Blazor layout/head rendering code; no `<BasePath />`.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

### 26. EF Core GetEntriesForState [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

No EF Core app exists; no `ChangeTracker.GetEntriesForState(...)`.

```text
# dotnet-net11-skill run-1: no efcore directory
console-bcl
```

```text
# no-skills run-1: no efcore directory
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

### 27. EF Core RemoveDbContext [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

No EF registration/testing setup; no `RemoveDbContext<T>()`.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

### 28. EF Core ExcludeForeignKeyFromMigrations [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

No model configuration with `ExcludeForeignKeyFromMigrations(true)`.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

### 29. EF Core JSON Query Functions [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

No EF query layer; no `EF.Functions.JsonContains`/`JsonPathExists`.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

### 30. SignalR ConfigureConnection [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

No interactive server components; no `ConfigureConnection`.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

### 31. Blazor Virtualize Variable-Height Items [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

No `<Virtualize>` usage to evaluate.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

### 32. Runtime Async Configuration [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

Neither project has `<Features>runtime-async=on</Features>`.

```xml
<!-- dotnet-net11-skill: DevToolkit.csproj -->
<TargetFramework>net11.0</TargetFramework>
<LangVersion>preview</LangVersion>
```

```xml
<!-- no-skills: bcl-showcase.csproj -->
<TargetFramework>net11.0</TargetFramework>
<LangVersion>preview</LangVersion>
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; feature not configured.

### 33. ProcessExitStatus Usage [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 2 | 2 |
| **Mean** | **2.0** | **2.0** |

#### Analysis

Both use legacy `ExitCode`/`HasExited` instead of `ProcessExitStatus`.

```csharp
// dotnet-net11-skill: Demos/ProcessExitDemo.cs
successProcess.WaitForExit();
Console.WriteLine($"  Exit code: {successProcess.ExitCode}");
```

```csharp
// no-skills: Demos/ProcessExitInfo.cs
await process.WaitForExitAsync();
Console.WriteLine($"  Exit code: {process.ExitCode}");
```

**Score:** `dotnet-net11-skill: 2`, `no-skills: 2`.  
**Verdict:** Tie; both still use old process-exit model.

### 34. OpenAPI Binary File Response [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

No Web API endpoints are present, so no `.Produces<FileContentResult>(...)` coverage.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

### 35. Brotli and Compression Options [LOW × 0]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 3 | 3 |
| **Mean** | **3.0** | **3.0** |

#### Analysis

Both configure Zstandard options well, but neither uses `BrotliCompressionOptions.WindowLog`.

```csharp
// dotnet-net11-skill: Demos/CompressionBenchmark.cs
var advancedOptions = new ZstandardCompressionOptions { Quality = 5, WindowLog = 22, AppendChecksum = true, EnableLongDistanceMatching = true };
```

```csharp
// no-skills: Demos/CompressionBenchmark.cs
var zstdOptions = new ZstandardCompressionOptions { Quality = 6, AppendChecksum = true, EnableLongDistanceMatching = true, WindowLog = 22 };
```

**Score:** `dotnet-net11-skill: 3`, `no-skills: 3`.  
**Verdict:** Tie; partial adoption (Zstandard yes, Brotli options no).

### 36. Vector Constants [LOW × 0]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

Neither uses `Vector<T>.Pi`, `Vector<T>.E`, etc.

```csharp
// dotnet-net11-skill: Demos/BFloat16Demo.cs
Console.WriteLine($"  Pi:  {(BFloat16)MathF.PI}");
Console.WriteLine($"  Tau: {(BFloat16)(MathF.PI * 2)}");
```

```csharp
// no-skills: Demos/MlNumericTypes.cs
Console.WriteLine($"    Pi ≈ {float.Pi}");
Console.WriteLine($"    Tau ≈ {float.Tau}");
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; both miss the SIMD vector constant APIs.

### 37. Overall .NET 11 API Adoption Rate [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 2 | 3 |
| **Mean** | **2.0** | **3.0** |

#### Analysis

`dotnet-net11-skill` adopts more .NET 11 APIs deeply in console scope (Rune overloads, AnyNewLine, MediaTypeMap, DivisionRounding, full file APIs). `no-skills` adopts many, but with more legacy fallbacks (manual MIME map, manual division/newline handling, older pipe/null-handle patterns).

```csharp
// dotnet-net11-skill examples
MediaTypeMap.GetMediaType(ext);
RegexOptions.AnyNewLine;
T.Divide(left, right, mode);
```

```csharp
// no-skills examples
private static readonly Dictionary<string, string> ExtensionToMime = ...
var euclideanRemainder = ((dividend % divisor) + divisor) % divisor;
var universalNewlinePattern = new Regex(@"\r\n|\r|\n|\u0085|\u2028|\u2029");
```

**Score:** `dotnet-net11-skill: 3`, `no-skills: 2`.  
**Verdict:** `dotnet-net11-skill` has better fidelity where implemented, but both are capped by missing `webapi`, `blazor`, and `efcore` scenarios.

### 38. Token Efficiency [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 5 | 2 |
| **Mean** | **5.0** | **2.0** |

---

## Asset Usage Summary

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| no-skills | 1 | f7c66703…5724 | claude-opus-4.6 | — | — | ✅ |
| dotnet-net11-skill | 1 | 5dcd99cd…377e | claude-opus-4.6 | — | — | ✅ |

---

## Raw Data References

- Per-run analysis: `reports/analysis-run-1.md`
- Verification data: `reports/verification-data.json`
- Score data: `reports/scores-data.json`
- Build notes: `reports/build-notes.md`
- Generation usage: `reports/generation-usage.json`
