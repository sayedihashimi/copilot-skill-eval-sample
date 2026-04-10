# Comparative Analysis: dotnet-perf-skills, no-skills

This run compares **2 configurations** (`dotnet-perf-skills`, `no-skills`) across **1 scenario**: `analyze-perf-issues` at `output/{config}/run-1/analyze-perf-issues/`. Configuration identity was confirmed from each scenario’s `gen-notes.md`: `dotnet-perf-skills` explicitly used `analyzing-dotnet-performance`, while `no-skills` is baseline analysis without a skill framework.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 4 |
| String Allocation Detection [CRITICAL] | 5 | 5 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 5 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 5 |
| Reflection and Serialization Overhead [HIGH] | 4 | 4 |
| Structural Optimization Detection [HIGH] | 5 | 3 |
| Severity Classification Accuracy [HIGH] | 5 | 3 |
| Fix Recommendation Quality [HIGH] | 5 | 4 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-1/analyze-perf-issues/performance-analysis.md`)
> #### 2. `new Regex()` per log line in hot path (4 instances)  
> **Impact:** `TryParseLine` is called for every line in a log file (potentially millions).  
> **Fix:** Hoist to `static readonly` fields or use `[GeneratedRegex]` (preferred on .NET 8)

> #### 8. 48× `RegexOptions.Compiled` without `[GeneratedRegex]` (48 instances)  
> **Impact:** 48 compiled regexes bloat startup with JIT compilation...

**no-skills** (`output/no-skills/run-1/analyze-perf-issues/performance-analysis.md`)
> 🔴 **`new Regex(...)` on every log line in `TryParseLine`** (lines 50, 65, 75)  
> worst regex anti-pattern... three `new Regex()` calls per line  
> **Fix:** `static readonly` fields or `[GeneratedRegex]`

> 🟡 **45+ `RegexOptions.Compiled` static instances** (lines 13–59)  
> convert to `[GeneratedRegex]` source-generated partial methods

**Score:** **dotnet-perf-skills 5/5**, **no-skills 4/5**. `dotnet-perf-skills` is more explicit on startup-budget framing and scan quantification.

**Verdict:** **dotnet-perf-skills**.

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills** (`performance-analysis.md`)
> #### 5. String concatenation (`+=`) in loops — O(n²) (4 sites)  
> **Files:** TemplateEngine... LogAnalyzer... DataPipeline...  
> **Fix:** Replace with `StringBuilder`

> #### 9. `.ToLower()`/`.ToUpper()` without culture (18 instances)  
> ...use `StringComparison.OrdinalIgnoreCase` ... `ToLowerInvariant()`

**no-skills** (`performance-analysis.md`)
> 🟡 **String concatenation char-by-char in `ParseLine`** ...  
> `current += line[i]` allocates a new string on every character  
> **Fix:** Use `StringBuilder`

> `.ToLower()` without ordinal...  
> `kvp.Key.Contains(path, StringComparison.OrdinalIgnoreCase)`

**Score:** **dotnet-perf-skills 5/5**, **no-skills 5/5**. Both clearly catch loop-concat and casing-allocation issues with concrete fixes.

**Verdict:** **Tie**.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills** (`performance-analysis.md`)
> #### 6. `List.Contains()` in loop — O(n) per check (2 sites)  
> **Fix:** Use `HashSet<string>` for O(1) lookups

> #### 16. `Skip(i).Take(5).ToList()` inside loop — O(n²) LINQ  
> ...N list allocations

**no-skills** (`performance-analysis.md`)
> `List.Contains()` for key lookup in `Diff` ... total O(n²)  
> **Fix:** Use a `HashSet<string>`

> `Skip(i).Take(5).ToList()` in a loop... O(n²) behavior  
> **Fix:** Use array indexing with a sliding window

**Score:** **dotnet-perf-skills 5/5**, **no-skills 5/5**. Both cover hot-path collection misuse and LINQ materialization waste.

**Verdict:** **Tie**.

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills** (`performance-analysis.md`)
> #### 1. `new HttpClient()` per call — socket exhaustion  
> #### 12. Sequential awaits in loop — no parallelism  
> #### 13. Unbounded parallelism...  
> #### 14. Missing cancellation tokens...

**no-skills** (`performance-analysis.md`)
> 🔴 `new HttpClient()` per call... **Critical production issue**  
> 🔴 Sequential awaits in `SendBatchAsync` loop  
> 🟡 Unbounded parallelism...  
> 🟡 Missing `CancellationToken` ... `Task.Delay` without cancellation token

**Score:** **dotnet-perf-skills 5/5**, **no-skills 5/5**. Both identify all required async/IO anti-patterns and mitigation patterns.

**Verdict:** **Tie**.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills** (`performance-analysis.md`)
> #### 4. Uncached `new JsonSerializerOptions` per call (5 instances)  
> ...re-builds the internal type metadata cache

> #### 11. Uncached reflection — `GetProperties()`/`SetValue()`/`GetValue()`  
> ...10-100x slower than compiled delegates

**no-skills** (`performance-analysis.md`)
> `new JsonSerializerOptions` on every call... expensive to construct  
> **Fix:** `static readonly` shared instance

> Uncached `GetProperties()` reflection...  
> Reflection `SetValue`/`GetValue` per property...

**Score:** **dotnet-perf-skills 4/5**, **no-skills 4/5**. Both capture core reflection/serializer overhead; neither strongly addresses partial-parse alternatives for over-deserialization.

**Verdict:** **Tie**.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills** (`performance-analysis.md`)
> #### 17. Unsealed classes (16 of 16 non-struct types)  
> #### 18. Structs without `IEquatable<T>` (2 structs...)  
> #### 20. Static `Dictionary<>` candidates for `FrozenDictionary` (2 instances)

**no-skills** (`performance-analysis.md`)
> Unsealed nested class `MappingConfig`...  
> Unsealed class `ValidationResult`...  
> `Record` and `PipelineResult`... `FrozenDictionary` candidate...

**Score:** **dotnet-perf-skills 5/5**, **no-skills 3/5**. `dotnet-perf-skills` is systematic and quantified; `no-skills` is narrower and more partial.

**Verdict:** **dotnet-perf-skills**.

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills** (`performance-analysis.md`)
> 🔴 Critical | 7 | ...`new HttpClient()`... uncached `new Regex()` in hot loops...  
> 🟡 Moderate | 9 | 48 `RegexOptions.Compiled` ... uncached reflection...

**dotnet-perf-skills** (`gen-notes.md`)
> Severity classification framework... 🔴 Critical / 🟡 Moderate / ℹ️ Info  
> ...escalation rules based on instance counts

**no-skills** (`performance-analysis.md`)
> 🔴 Critical | 6 | Socket exhaustion, O(n²) hot paths, regex per-line...  
> 🟡 Moderate | 18 | ...missing capacity hints, reflection per-call

**Score:** **dotnet-perf-skills 5/5**, **no-skills 3/5**. Skill-based output has clearer hot-path vs startup separation and more disciplined ranking.

**Verdict:** **dotnet-perf-skills**.

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills** (`performance-analysis.md` + `gen-notes.md`)
> **Fix:** `IHttpClientFactory` or static `HttpClient` with `PooledConnectionLifetime`  
> **Fix:** `[GeneratedRegex]` on .NET 8  
> ...warned against suggesting `[GeneratedRegex]` for dynamic patterns and against `Span<T>` in async methods

**no-skills** (`performance-analysis.md`)
> **Fix:** `IHttpClientFactory` / static instance  
> **Fix:** `StringComparison.OrdinalIgnoreCase`  
> **Fix:** `FrozenDictionary` and `Parallel.ForEachAsync`

**Score:** **dotnet-perf-skills 5/5**, **no-skills 4/5**. Both are actionable; skill-based output is more guardrail-aware and consistently API-specific.

**Verdict:** **dotnet-perf-skills**.

## Weighted Summary

Weights used: **Critical ×3**, **High ×2**, **Medium ×1**, **Low ×0.5**.

| Dimension | Tier | Weight | dotnet-perf-skills | no-skills |
|---|---|---:|---:|---:|
| Regex Anti-Pattern Detection | Critical | 3 | 15 | 12 |
| String Allocation Detection | Critical | 3 | 15 | 15 |
| Collection and LINQ Efficiency | Critical | 3 | 15 | 15 |
| Async and IO Pattern Detection | Critical | 3 | 15 | 15 |
| Reflection and Serialization Overhead | High | 2 | 8 | 8 |
| Structural Optimization Detection | High | 2 | 10 | 6 |
| Severity Classification Accuracy | High | 2 | 10 | 6 |
| Fix Recommendation Quality | High | 2 | 10 | 8 |
| **Total Weighted Score** |  |  | **98** | **85** |

## What All Versions Get Right

- Both identify the top production risks: per-call `HttpClient`, per-call regex in hot paths, and O(n²) string concatenation.
- Both flag dictionary/collection inefficiencies (`ContainsKey`+indexer, `List.Contains` in loops) and provide low-friction replacements.
- Both recommend modern .NET APIs (`[GeneratedRegex]`, `StringBuilder`, `StringComparison.OrdinalIgnoreCase`, `Parallel.ForEachAsync`).

## Summary: Impact of Skills

Most impactful differences are: **(1)** stronger structural coverage, **(2)** better severity prioritization discipline, **(3)** tighter fix guidance with explicit .NET guardrails. Overall, both outputs are useful, but **dotnet-perf-skills** is clearly stronger for implementation prioritization, reflected in weighted scoring (**98 vs 85**).
