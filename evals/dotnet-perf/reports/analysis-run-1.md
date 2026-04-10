# Comparative Analysis: dotnet-perf-skills, no-skills

This run compares **2 configurations** (`dotnet-perf-skills`, `no-skills`) for the same app at `run-1/analyze-perf-issues`. Configuration identity was confirmed from `gen-notes.md` in each directory: `dotnet-perf-skills` explicitly documents the `analyzing-dotnet-performance` skill, while `no-skills` is baseline analysis output.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 4 |
| String Allocation Detection [CRITICAL] | 5 | 4 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 4 |
| Async and IO Pattern Detection [CRITICAL] | 3 | 5 |
| Reflection and Serialization Overhead [HIGH] | 5 | 4 |
| Structural Optimization Detection [HIGH] | 5 | 3 |
| Severity Classification Accuracy [HIGH] | 3 | 4 |
| Fix Recommendation Quality [HIGH] | 5 | 4 |
| Internal Consistency and Traceability [HIGH] | 5 | 2 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

`dotnet-perf-skills` is highly systematic: it quantifies `new Regex(`, `RegexOptions.Compiled`, static `Regex.Replace`, and explicitly recommends `[GeneratedRegex]` for .NET 8.

```csharp
// dotnet-perf-skills: output/.../performance-analysis.md
| `new Regex(` (per-call) | **8** | LogAnalyzer (4), TemplateEngine (2), ValidationEngine (1), CsvParser (1) |
| `RegexOptions.Compiled` | **48** | All in MarkdownStripper.cs |
| `[GeneratedRegex]` | **0** | None used anywhere |
```

`no-skills` also catches key regex issues (per-call instantiation and 40+ compiled regexes), but with less global quantification.

```csharp
// no-skills: output/.../performance-analysis.md
**32. Critical - `new Regex(...)` per log line in `TryParseLine`**
**46. Critical - 40+ `RegexOptions.Compiled` static regex instances**
**Fix:** On .NET 8+, switch to `[GeneratedRegex]` source generators
```

**Score:** `dotnet-perf-skills` **5/5**, `no-skills` **4/5**  
**Verdict:** **dotnet-perf-skills** is best due to complete detection coverage plus explicit hit-count accounting.

## 2. String Allocation Detection [CRITICAL]

`dotnet-perf-skills` identifies `+=` loop allocations, `.ToLower()/.ToUpper()` hotspots, and replacement-chain allocations with broader cross-file totals.

```csharp
// dotnet-perf-skills
| `.ToLower()/.ToUpper()` | **17** | Across 6 files |
| `+= string` in loops | **10** | Across 6 files |
#### 5. `+= string` Concatenation in Loops - O(n^2) Allocation (10 instances)
```

`no-skills` catches the same patterns, but mostly per-file and with less consolidated prioritization.

```csharp
// no-skills
**1. Moderate - String concatenation (`+=`) character-by-character in `ParseLine`**
**37. Moderate - String concatenation in `Summarize`**
**63. Info - `.ToLower()` without ordinal culture**
```

**Score:** `dotnet-perf-skills` **5/5**, `no-skills` **4/5**  
**Verdict:** **dotnet-perf-skills** provides stronger whole-codebase string allocation analysis.

## 3. Collection and LINQ Efficiency [CRITICAL]

`dotnet-perf-skills` provides exact counts for `ContainsKey`+indexer, `.ToList()` materialization, and HashSet/FrozenDictionary opportunities.

```csharp
// dotnet-perf-skills
| `.ContainsKey(` + indexer (double-lookup) | **13** | Across 6 files |
| `.ToList()` (unnecessary materialization) | **19** | Across 5 files |
#### 8. `List<string>.Contains()` in Loop - O(n^2) Lookup
```

`no-skills` identifies the same categories (`TryGetValue`, `HashSet`, `Skip(i).Take(5).ToList()`), but with weaker aggregate framing.

```csharp
// no-skills
**34. Moderate - `Skip(i).Take(5).ToList()` sliding window**
**50. Moderate - `existingSlugs.ToList()` + `.Contains()` loop**
**Fix:** Use `HashSet<string>`
```

**Score:** `dotnet-perf-skills` **5/5**, `no-skills` **4/5**  
**Verdict:** **dotnet-perf-skills** is stronger on breadth and measured impact.

## 4. Async and IO Pattern Detection [CRITICAL]

`dotnet-perf-skills` strongly flags `new HttpClient()` per call, but does **not** detect other required async patterns (sequential awaits, unbounded parallelism, cancellation propagation).

```csharp
// dotnet-perf-skills
#### 1. `new HttpClient()` Per Call - Socket Exhaustion (3 instances)
**Fix:** Inject `IHttpClientFactory` or use a single `static readonly HttpClient`
```

`no-skills` covers all requested async/IO anti-patterns in `NotificationService`.

```csharp
// no-skills
**40. Moderate - Sequential awaits in `SendBatchAsync` loop**
**41. Moderate - Unbounded parallelism in `SendBatchParallelAsync`**
**42. Moderate - `Task.Delay` without `CancellationToken` in retry loop**
```

**Score:** `dotnet-perf-skills` **3/5**, `no-skills` **5/5**  
**Verdict:** **no-skills** is best in async/IO coverage; it captures operational concurrency and cancellation issues that the skill run missed.

## 5. Reflection and Serialization Overhead [HIGH]

Both configurations detect uncached reflection and per-call `JsonSerializerOptions`; `dotnet-perf-skills` is more explicit on scale and impact.

```csharp
// dotnet-perf-skills
#### 4. Uncached `JsonSerializerOptions` - Up to 592x Slower (4 instances)
#### 13. Uncached Reflection - `GetProperties()`/`GetProperty()`/`SetValue()`/`GetValue()`
```

```csharp
// no-skills
**8. Critical - `new JsonSerializerOptions` on every call**
**13. Critical - Uncached `GetProperties()` + `SetValue()` reflection in `MapTo<T>`**
```

**Score:** `dotnet-perf-skills` **5/5**, `no-skills` **4/5**  
**Verdict:** **dotnet-perf-skills** edges out via stronger quantification and clearer performance context.

## 6. Structural Optimization Detection [HIGH]

`dotnet-perf-skills` performs complete structural checks (all classes/structs plus FrozenDictionary candidates).

```csharp
// dotnet-perf-skills
| Unsealed classes (non-abstract, non-static) | **18** | 0 of 18 classes are sealed |
| `public struct` without `IEquatable<T>` | **2** | 0 of 2 structs implement IEquatable |
| `static readonly Dictionary<` (FrozenDictionary candidate) | **2** |
```

`no-skills` identifies structural issues, but only partially and less exhaustively.

```csharp
// no-skills
**18. Info - `MappingConfig` class is unsealed**
**21. Info - `ValidationError` struct without `IEquatable<ValidationError>`**
**51. Info - `ReplacementMap` is a `FrozenDictionary` candidate**
```

**Score:** `dotnet-perf-skills` **5/5**, `no-skills` **3/5**  
**Verdict:** **dotnet-perf-skills** is best for full structural optimization coverage.

## 7. Severity Classification Accuracy [HIGH]

`dotnet-perf-skills` correctly elevates major risks, but over-classifies some medium-impact patterns (e.g., `ContainsKey`+indexer as Critical), reducing prioritization precision.

```csharp
// dotnet-perf-skills
#### 6. `ContainsKey` + Indexer Double-Lookup (13 instances)
... placed under Critical ...
```

`no-skills` generally keeps lower-impact issues in Moderate/Info and keeps production-risk items critical.

```csharp
// no-skills
Critical: `new HttpClient`, per-call regex hot paths, `JsonSerializerOptions`
Info: `ContainsKey` + indexer quick-fix across codebase
```

**Score:** `dotnet-perf-skills` **3/5**, `no-skills` **4/5**  
**Verdict:** **no-skills** is slightly better at impact-tier separation; `dotnet-perf-skills` has stronger discovery but noisier severity calibration.

## 8. Fix Recommendation Quality [HIGH]

Both provide actionable fixes; `dotnet-perf-skills` is more API-specific and modern (.NET 8-focused).

```csharp
// dotnet-perf-skills
[GeneratedRegex(...)]
StringComparison.OrdinalIgnoreCase
System.Collections.Frozen / ToFrozenDictionary()
IHttpClientFactory
```

```csharp
// no-skills
IHttpClientFactory or shared HttpClient
SemaphoreSlim throttling for batch sends
ConcurrentDictionary<Type, PropertyInfo[]> cache
```

**Score:** `dotnet-perf-skills` **5/5**, `no-skills` **4/5**  
**Verdict:** **dotnet-perf-skills** gives the most concrete, modern, and copy-pasteable fix patterns.

## 9. Internal Consistency and Traceability [HIGH]

`dotnet-perf-skills` is internally coherent (counts, checklist, and severity totals align tightly). `no-skills` has mismatches (e.g., summary says 48 issues / 9 files, but findings enumerate beyond that and mention 10 files in places).

```csharp
// no-skills (inconsistent metadata)
This analysis covers **9 source files** ...
A total of **48 performance issues** ...
... findings numbered up to **57**
```

**Score:** `dotnet-perf-skills` **5/5**, `no-skills` **2/5**  
**Verdict:** **dotnet-perf-skills** is materially better for auditability and confidence in the report.

## Weighted Summary

Weights used:
- Critical = score x 3
- High = score x 2
- Medium = score x 1
- Low = score x 0.5

| Dimension | Tier | dotnet-perf-skills | no-skills |
|---|---|---:|---:|
| Regex Anti-Pattern Detection | Critical | 15 | 12 |
| String Allocation Detection | Critical | 15 | 12 |
| Collection and LINQ Efficiency | Critical | 15 | 12 |
| Async and IO Pattern Detection | Critical | 9 | 15 |
| Reflection and Serialization Overhead | High | 10 | 8 |
| Structural Optimization Detection | High | 10 | 6 |
| Severity Classification Accuracy | High | 6 | 8 |
| Fix Recommendation Quality | High | 10 | 8 |
| Internal Consistency and Traceability | High | 10 | 4 |
| **Total Weighted Score** |  | **100** | **85** |

## What All Versions Get Right

- Both clearly identify **`new HttpClient()` per call** as a severe production risk.
- Both detect **per-call regex instantiation** in hot paths and recommend caching or generated regex.
- Both call out **loop-time string concatenation** and recommend `StringBuilder`.
- Both surface **reflection and serializer-option caching** concerns in `EntityMapper`/`JsonTransformer`.
- Both provide concrete .NET-specific remediations (not just generic "optimize this" advice).

## Summary: Impact of Skills

Most impactful differences, ranked:
1. **Coverage depth and quantification:** `dotnet-perf-skills` gives comprehensive hit-count driven detection across regex/string/collection/structural domains.
2. **Async/IO breadth:** `no-skills` is stronger for concurrency/cancellation anti-patterns beyond `HttpClient` lifetime.
3. **Consistency and auditability:** `dotnet-perf-skills` is substantially more internally consistent and easier to trust for prioritization.

Overall assessment:
- **dotnet-perf-skills**: Best overall for broad, measurable, and high-confidence performance analysis (**weighted winner: 100**).
- **no-skills**: Solid baseline with particularly good async/IO diagnostics, but weaker consistency and less systematic cross-cutting quantification (**weighted: 85**).
