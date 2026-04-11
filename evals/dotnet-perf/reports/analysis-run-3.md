# Comparative Analysis: dotnet-perf-skills, no-skills

This run compares **2 configurations** (`dotnet-perf-skills`, `no-skills`) on **1 scenario**: `analyze-perf-issues` from `output/{config}/run-3/analyze-perf-issues/`. Configuration identity was confirmed from each run's `gen-notes.md`, with both outputs targeting the same .NET 8 performance-analysis task.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 4 |
| String Allocation Detection [CRITICAL] | 5 | 4 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 4 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 4 |
| Reflection and Serialization Overhead [HIGH] | 4 | 4 |
| Structural Optimization Detection [HIGH] | 5 | 3 |
| Severity Classification Accuracy [HIGH] | 3 | 4 |
| Fix Recommendation Quality [HIGH] | 5 | 4 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills** (`performance-analysis.md`)
> `new Regex(` (uncached per-call) | **8**  
> `RegexOptions.Compiled` | **48**  
> `[GeneratedRegex]` | **0** | None used — 0 of 48+ static patterns use source generator

**no-skills** (`performance-analysis.md`)
> **`new Regex(...)` per log line in `TryParseLine`** ... for a 1M-line log, it creates 1-3 million regex objects.  
> **40+ `RegexOptions.Compiled` static instances** ... startup budget blown.  
> **Recommendation:** On .NET 8+, use `[GeneratedRegex]` for all static patterns.

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best here due to explicit counts and stronger completeness checks (`[GeneratedRegex]` coverage = 0).

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills** (`performance-analysis.md`)
> String `+=` Concatenation in Loops — O(n²) Allocation (6 sites)  
> `.ToLower()`/`.ToUpper()` Without Culture or StringComparison (17 instances)  
> Chained `.Replace()` Calls in Loop (SlugGenerator) (9 iterations)

**no-skills** (`performance-analysis.md`)
> **`current += line[i]` character-by-character string concatenation** ... O(n²) for long lines/files.  
> **Long chain of `.Replace()` calls in `StripMarkdown`** ... ~45 full-string allocations.  
> `.ToLower()` / `.ToUpper()` Without Culture ... locale-sensitive.

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is stronger on breadth and quantification; both identify the core allocation issues.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills** (`performance-analysis.md`)
> `ContainsKey` + Indexer Double-Lookup (~12 instances) ... **Fix:** Replace with `TryGetValue`.  
> `List.Contains()` for Lookups — O(n) per Check ... **Fix:** Use `HashSet<string>`.  
> `LogAnalyzer.DetectAnomalies` — `Skip(i).Take(5).ToList()` in Loop

**no-skills** (`performance-analysis.md`)
> **`allKeys` as `List<string>` with `.Contains()` ... O(n²) total. Fix: Use `HashSet<string>`**  
> **`ContainsKey` + indexer double-lookup** ... use `TryGetValue`.  
> **`Skip(i).Take(5).ToList()` in a loop** ... creates a new list on every iteration.

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** wins by covering more hot-path collection cases in one place (including broader materialization patterns).

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills** (`performance-analysis.md`)
> `new HttpClient()` Per Call — Socket Exhaustion Risk (3 instances)  
> Sequential `await` in Loop — No Parallelism (1 instance)  
> Unbounded Parallelism in `SendBatchParallelAsync` ... Missing Cancellation Token ... `Task.Delay` can't be cancelled.

**no-skills** (`performance-analysis.md`)
> **`new HttpClient()` per call** ... causes **socket exhaustion**.  
> **Unbounded parallelism in `SendBatchParallelAsync`** ... spawns 10K concurrent HTTP requests.  
> **Sequential awaits in `SendBatchAsync`** ... `Task.Delay` without `CancellationToken`.

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is slightly better due to tighter prioritization and clearer impact framing, while both cover all required anti-patterns.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills** (`performance-analysis.md`)
> Uncached `new JsonSerializerOptions` Per Call (4 instances) ... up to 592x slower than cached options  
> Uncached Reflection `GetProperties()`/`GetProperty()` in Hot Paths (3 instances)

**no-skills** (`performance-analysis.md`)
> **`new JsonSerializerOptions { WriteIndented = true }` on every call** ... use static readonly options.  
> **`typeof(TTarget).GetProperties()` on every call** ... cache `PropertyInfo[]` per type.

**Score:** dotnet-perf-skills **4/5**, no-skills **4/5**.  
**Verdict:** **Tie.** Both catch uncached reflection and serializer options well; neither goes deep on partial-deserialization alternatives (`Utf8JsonReader`) in this run.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills** (`performance-analysis.md`)
> `sealed class` | **0** ... Unsealed non-abstract non-static classes | **18**  
> `IEquatable` | **0** ... `public struct` (without IEquatable) | **2**  
> `static readonly Dictionary<` (FrozenDictionary candidate) | **2**

**no-skills** (`performance-analysis.md`)
> `ValidationError`, `DeliveryResult` structs lack `IEquatable<T>`.  
> `MappingConfig`, `ValidationResult`, `Record` classes are unsealed.  
> Static `Converters` dictionary could be `FrozenDictionary`.

**Score:** dotnet-perf-skills **5/5**, no-skills **3/5**.  
**Verdict:** **dotnet-perf-skills** is decisively better; it provides complete structural inventory and scale, while `no-skills` is more sample-based.

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills** (`performance-analysis.md`)
> 🔴 Critical | 8 | ... `new HttpClient()` ... uncached `new Regex()` ... `new JsonSerializerOptions` ... uncached reflection  
> #### 6. `ContainsKey` + Indexer Double-Lookup (~12 instances) ... **🔴 Critical**  
> #### 7. `List.Contains()` for Lookups ... **🔴 Critical**

**no-skills** (`performance-analysis.md`)
> **Top priorities:** (1) `new HttpClient` per call ... (2) Regex instantiation per log line ... (3) 40+ `RegexOptions.Compiled` ... (4) string `+=` in tight loops  
> `ContainsKey` + indexer double-lookup ... **ℹ️ Info** / **🟡 Moderate** by context

**Score:** dotnet-perf-skills **3/5**, no-skills **4/5**.  
**Verdict:** **no-skills** is better calibrated in this dimension; `dotnet-perf-skills` over-escalates some moderate collection issues to critical, which weakens prioritization fidelity.

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills** (`performance-analysis.md`)
> **Fix:** Inject `HttpClient` ... or use `IHttpClientFactory`.  
> **Fix:** Hoist to `static readonly` ... use `[GeneratedRegex]`.  
> **Fix:** Replace with `StringBuilder` / `TryGetValue` / `HashSet<string>` / `Parallel.ForEachAsync`.

**no-skills** (`performance-analysis.md`)
> **Fix:** Use `StringBuilder` or `Span<char>`.  
> **Fix:** Use `SemaphoreSlim` or `Parallel.ForEachAsync` with `MaxDegreeOfParallelism`.  
> **Fix:** `sealed` classes, `IEquatable<T>`, cache `JsonSerializerOptions`.

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** provides more consistently specific API-level remediations and stronger implementation patterns.

## Weighted Summary

Weights: **Critical ×3**, **High ×2**, **Medium ×1**, **Low ×0.5**.

| Configuration | Critical Raw (4 dims) | Critical Weighted | High Raw (4 dims) | High Weighted | Total Weighted |
|---|---:|---:|---:|---:|---:|
| dotnet-perf-skills | 20 | 60 | 17 | 34 | **94** |
| no-skills | 16 | 48 | 15 | 30 | **78** |

## What All Versions Get Right

- Both identify the major production-risk issues: `new HttpClient()` per call and regex-per-line in log parsing.
- Both call out excessive `RegexOptions.Compiled` usage in `MarkdownStripper` and recommend `[GeneratedRegex]`.
- Both detect high-allocation string construction patterns (`+=` in loops, replace chains, casing allocations).
- Both provide actionable modernization guidance (`StringBuilder`, `TryGetValue`, `HashSet`, cached serializer options, cancellation-aware async).

## Summary: Impact of Skills

Most impactful differences (ranked): 1) stronger quantified coverage (counts/inventory) in regex, string, and structural categories, 2) more complete hot-path collection/LINQ detection, 3) more implementation-ready fix guidance.  
Overall, **dotnet-perf-skills** delivers the stronger report by weighted score (**94 vs 78**), mainly through better completeness and actionability; **no-skills** is still solid and in one area (severity calibration) is more conservative and often better prioritized.
