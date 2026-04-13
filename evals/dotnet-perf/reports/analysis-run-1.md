# Comparative Analysis: dotnet-perf-skills, no-skills

This report compares **2 configurations** (`dotnet-perf-skills`, `no-skills`) for the **run-1** output of the **analyze-perf-issues** scenario at `output/{config}/run-1/analyze-perf-issues/performance-analysis.md`. Configuration identity was confirmed from `gen-notes.md`: `dotnet-perf-skills` explicitly used the `analyzing-dotnet-performance` plugin skill, while `no-skills` is baseline output.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 5 |
| String Allocation Detection [CRITICAL] | 5 | 5 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 4 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 5 |
| Reflection and Serialization Overhead [HIGH] | 4 | 4 |
| Structural Optimization Detection [HIGH] | 5 | 4 |
| Severity Classification Accuracy [HIGH] | 5 | 4 |
| Fix Recommendation Quality [HIGH] | 5 | 4 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**Coverage excerpts**

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-1/analyze-perf-issues/performance-analysis.md`)
> #### 2. `new Regex()` Per Log Line in Hot Path (4 instances)  
> **Impact:** `TryParseLine` is called per log line — potentially millions of times.  
> **Fix:** Hoist to `static readonly` fields (or `[GeneratedRegex]` on .NET 8+).

> #### 10. 48 `RegexOptions.Compiled` Without `[GeneratedRegex]` (48 instances)  
> **Impact:** Each compiled regex JIT-compiles at first use, consuming startup budget.  
> **Fix:** Convert all 48 static `Regex` fields to `[GeneratedRegex]` partial methods.

**no-skills** (`output/no-skills/run-1/analyze-perf-issues/performance-analysis.md`)
> **12. 🟡 Moderate — 46 `RegexOptions.Compiled` instances**  
> Each `RegexOptions.Compiled` regex JIT-compiles at startup...  
> **Fix:** On .NET 7+, replace with `[GeneratedRegex]` source generators...

> **26. 🔴 Critical — `new Regex()` per log line in `TryParseLine`**  
> ...called once per log line. For a 1M-line log file, this creates 3 million `Regex` objects.

**Scores**

| Configuration | Score | Justification |
|---|---:|---|
| dotnet-perf-skills | 5 | Catches both per-call regex hot paths and compiled-overuse at scale (48), with explicit `[GeneratedRegex]` migration guidance. |
| no-skills | 5 | Also catches the same two critical regex patterns with strong impact framing and concrete fixes. |

**Verdict:** Tie. Both outputs are excellent on regex performance risks.

## 2. String Allocation Detection [CRITICAL]

**Coverage excerpts**

**dotnet-perf-skills**
> #### 12. String `+=` Concatenation in Loops — O(n²) Allocation (9 instances)  
> **Impact:** Each `+=` allocates a new string copying all previous content.  
> **Fix:** Use `StringBuilder`.

> #### 11. `.ToLower()`/`.ToUpper()` Without StringComparison (25 instances)  
> ...culture-sensitive comparison (Turkish-I bug risk).  
> **Fix:** Replace with `StringComparison.OrdinalIgnoreCase`.

**no-skills**
> **58. 🔴 Critical — Character-by-character string concatenation**  
> `current += line[i]` creates a new string for every character... O(n²) allocations.

> ### 3. `.ToLower()` / `.ToUpper()` Without Culture (affects 6/10 files)  
> ...Replace with `StringComparison.OrdinalIgnoreCase` or `ToLowerInvariant()`.

**Scores**

| Configuration | Score | Justification |
|---|---:|---|
| dotnet-perf-skills | 5 | Strongly covers loop concat, casing allocations, and impact metrics with broad counts. |
| no-skills | 5 | Identifies all required string issues, including `+=` loops, replace chains, and culture/casing pitfalls. |

**Verdict:** Tie. Both are comprehensive and actionable.

## 3. Collection and LINQ Efficiency [CRITICAL]

**Coverage excerpts**

**dotnet-perf-skills**
> #### 8. `List.Contains()` for Key Lookups — O(n²) in Diff  
> **Fix:** Use `HashSet<string>` instead of `List<string>`.

> #### 13. `ContainsKey` + Indexer Double-Lookup (12 instances)  
> **Fix:** Use `TryGetValue`.

> #### 17. `Skip(i).Take(5).ToList()` in Loop — O(n²) Sliding Window  
> **Fix:** Use index-based access on the list directly.

**no-skills**
> **9. 🟡 Moderate — `List.Contains()` in a loop**  
> ...`existing.Contains(baseSlug)` is O(n) per call...  
> **Fix:** Use `HashSet<string>`.

> **30. 🟡 Moderate — `Skip(i).Take(5).ToList()` in loop**  
> ...creates a new enumerator and list on every iteration...

**Scores**

| Configuration | Score | Justification |
|---|---:|---|
| dotnet-perf-skills | 5 | Excellent breadth: O(n) lookups, materialization, sliding windows, double-lookups, and count-backed prioritization. |
| no-skills | 4 | Strong coverage of key anti-patterns, but less consistently quantified and slightly less systematic than skills output. |

**Verdict:** **dotnet-perf-skills** is best due to tighter, count-driven collection/LINQ prioritization.

## 4. Async and IO Pattern Detection [CRITICAL]

**Coverage excerpts**

**dotnet-perf-skills**
> #### 1. `new HttpClient()` Per Call — Socket Exhaustion  
> **Fix:** Inject `IHttpClientFactory` or use a `static readonly HttpClient`...

> #### 14. Unbounded Parallelism in `SendBatchParallelAsync`  
> ...Use `Parallel.ForEachAsync` with `MaxDegreeOfParallelism`...

> #### 15. Missing `CancellationToken` on Async Methods  
> ...pass to `Task.Delay` and `HttpClient` calls.

**no-skills**
> **15. 🔴 Critical — `new HttpClient()` per call**  
> ...causes **socket exhaustion** under load.

> **16. 🔴 Critical — Unbounded parallelism in `SendBatchParallelAsync`**  
> ...10,000 concurrent HTTP calls.

> **18. 🟡 Moderate — Missing `CancellationToken` on all async methods**

**Scores**

| Configuration | Score | Justification |
|---|---:|---|
| dotnet-perf-skills | 5 | Fully covers all required async/IO anti-patterns with correct APIs and risk framing. |
| no-skills | 5 | Also fully covers required async/IO issues and provides concrete mitigation patterns. |

**Verdict:** Tie. Both are production-relevant and complete.

## 5. Reflection and Serialization Overhead [HIGH]

**Coverage excerpts**

**dotnet-perf-skills**
> #### 7. Uncached Reflection: `GetProperties()`/`SetValue()`/`GetValue()` Per Call  
> **Impact:** Reflection is ~100x slower than direct property access.

> #### 4. Uncached `new JsonSerializerOptions` Per Call (6 instances)  
> **Impact:** Up to 592x slower than cached options...

**no-skills**
> **47. 🔴 Critical — Uncached `GetProperties()` and `SetValue()` reflection**  
> ...extremely slow in batch operations.

> **53. 🟡 Moderate — `new JsonSerializerOptions` per call**  
> ...expensive to construct (builds internal caches).

**Scores**

| Configuration | Score | Justification |
|---|---:|---|
| dotnet-perf-skills | 4 | Strong on reflection and options caching; less direct discussion of partial parsing alternatives. |
| no-skills | 4 | Same core detections and fixes; similarly light on parsing-strategy alternatives (`Utf8JsonReader`). |

**Verdict:** Tie. Both are good, with minor depth gaps beyond core detections.

## 6. Structural Optimization Detection [HIGH]

**Coverage excerpts**

**dotnet-perf-skills**
> #### 26. `static readonly Dictionary<>` — FrozenDictionary Candidates (2 instances)  
> ...`FrozenDictionary` provides ~50% faster lookups...

> #### 27. Structs Without `IEquatable<T>` (2 of 2 structs)

> #### 28. Unsealed Leaf Classes (14 of 17 classes)

**no-skills**
> **11. ℹ️ Info — `ReplacementMap` could be `FrozenDictionary`**

> **20. ℹ️ Info — Struct `DeliveryResult` without `IEquatable<T>`**

> **38. ℹ️ Info — Unsealed class `Record`**  
> ...`ValidationResult`, `MappingConfig` also identified elsewhere.

**Scores**

| Configuration | Score | Justification |
|---|---:|---|
| dotnet-perf-skills | 5 | Comprehensive structural pass with quantified coverage across classes/structs/dictionaries. |
| no-skills | 4 | Correctly identifies requested targets, but with narrower and less quantified structural breadth. |

**Verdict:** **dotnet-perf-skills** is stronger due to systematic structural inventory and prioritization.

## 7. Severity Classification Accuracy [HIGH]

**Coverage excerpts**

**dotnet-perf-skills**
> | 🔴 Critical | 9 | `new HttpClient()` per call..., uncached `new Regex()` in per-line parsing... |

> #### 2. `new Regex()` Per Log Line in Hot Path  
> **Impact:** ...potentially millions of times.

**no-skills**
> **Top priorities:**  
> 1. `new HttpClient` per call...  
> 2. Regex instantiation per log line...  
> 3. Character-by-character string concatenation...

> | 🔴 Critical | 8 |

**Scores**

| Configuration | Score | Justification |
|---|---:|---|
| dotnet-perf-skills | 5 | Better hot-path weighting and consistent scale-based severity calibration in the report body. |
| no-skills | 4 | Generally accurate, but some severity boundaries are less consistently calibrated across findings. |

**Verdict:** **dotnet-perf-skills** provides more consistent severity ranking signal.

## 8. Fix Recommendation Quality [HIGH]

**Coverage excerpts**

**dotnet-perf-skills**
> **Fix:** Inject `IHttpClientFactory` or use a `static readonly HttpClient` with `PooledConnectionLifetime`.

> **Fix:** Replace `.ToLower()` comparisons with `StringComparison.OrdinalIgnoreCase`... use `StringComparer.OrdinalIgnoreCase`.

> **Fix:** Convert all 48 static `Regex` fields to `[GeneratedRegex]` partial methods.

**no-skills**
> **Fix:** On .NET 7+, replace with `[GeneratedRegex]` source generators...

> **Fix:** Add `CancellationToken cancellationToken = default` parameter and pass it to `Task.Delay` and HTTP calls.

> **Fix:** Use `HashSet<string>`... `TryGetValue`... `StringBuilder`.

**Scores**

| Configuration | Score | Justification |
|---|---:|---|
| dotnet-perf-skills | 5 | Highly specific API-level fixes with concrete patterns and low-risk remediation pathing. |
| no-skills | 4 | Actionable and mostly correct, but less consistently precise in API-level migration planning. |

**Verdict:** **dotnet-perf-skills** has the highest fix-actionability density.

## Weighted Summary

Weights applied:
- Critical dimensions: score × 3
- High dimensions: score × 2
- Medium dimensions: score × 1
- Low dimensions: score × 0.5

| Dimension | Tier | Weight | dotnet-perf-skills | no-skills |
|---|---|---:|---:|---:|
| Regex Anti-Pattern Detection | CRITICAL | 3 | 15 | 15 |
| String Allocation Detection | CRITICAL | 3 | 15 | 15 |
| Collection and LINQ Efficiency | CRITICAL | 3 | 15 | 12 |
| Async and IO Pattern Detection | CRITICAL | 3 | 15 | 15 |
| Reflection and Serialization Overhead | HIGH | 2 | 8 | 8 |
| Structural Optimization Detection | HIGH | 2 | 10 | 8 |
| Severity Classification Accuracy | HIGH | 2 | 10 | 8 |
| Fix Recommendation Quality | HIGH | 2 | 10 | 8 |
| **Total Weighted Score** |  |  | **98** | **89** |

## What All Versions Get Right

- Both clearly identify the highest-risk production issue: `new HttpClient()` per call with socket exhaustion implications.
- Both detect regex hot-path misuse (`new Regex` per call/per line) and compiled-regex startup overhead.
- Both consistently call out string allocation pitfalls (`+=` loops, casing allocations) and recommend `StringBuilder`/ordinal comparisons.
- Both provide practical remediation APIs developers can apply quickly (`GeneratedRegex`, `IHttpClientFactory`, `TryGetValue`, `HashSet`).

## Summary: Impact of Skills

**Most impactful differences (ranked):**
1. **Systematic quantification and scan discipline** in `dotnet-perf-skills` (hit counts, broader structural inventory) improves prioritization confidence.
2. **Collection/LINQ and structural coverage depth** is higher with skills (more complete breadth and explicit instance counts).
3. **Severity calibration consistency** is stronger in skills output, especially for hot-path vs. setup-time distinctions.

Overall, both configurations perform well on critical .NET performance anti-patterns, but **dotnet-perf-skills** is the stronger report for engineering execution because it is more systematic, more consistently prioritized, and more operationally actionable by weighted score (**98 vs 89**).
