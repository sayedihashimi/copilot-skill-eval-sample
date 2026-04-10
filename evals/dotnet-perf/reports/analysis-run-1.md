# Comparative Analysis: dotnet-perf-skills, no-skills

This report compares **2 configurations** across **1 scenario** (`analyze-perf-issues`) using outputs at `output/{config}/run-1/analyze-perf-issues/performance-analysis.md`. Configuration identity was taken from `gen-notes.md`: `dotnet-perf-skills` used the `analyzing-dotnet-performance` skill, while `no-skills` is baseline Copilot analysis.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 5 |
| String Allocation Detection [CRITICAL] | 5 | 5 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 5 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 5 |
| Reflection and Serialization Overhead [HIGH] | 5 | 4 |
| Structural Optimization Detection [HIGH] | 5 | 3 |
| Severity Classification Accuracy [HIGH] | 3 | 4 |
| Fix Recommendation Quality [HIGH] | 5 | 4 |
| Coverage Breadth and Granularity [MEDIUM] | 4 | 5 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills** (`performance-analysis.md`) flags per-line regex allocation, compiled-regex startup budget, and migration path to source generators.

**dotnet-perf-skills excerpt:**
> **Impact:** Constructs and compiles regex on every invocation. In `LogAnalyzer.TryParseLine`, this runs per log line — potentially millions of calls...  
> **Impact:** Each `RegexOptions.Compiled` JIT-compiles the regex at startup... 48 compiled regexes in one class significantly increases startup time.  
> **Fix:** Convert all 48 to `[GeneratedRegex]` source-generated regexes.

**no-skills** (`performance-analysis.md`) also captures the same core regex problems with strong hot-path language.

**no-skills excerpt:**
> **`new Regex(...)` inside `TryParseLine()` — called per log line.** A 1M-line log file creates 1M+ regex objects.  
> **44 `static readonly Regex` with `RegexOptions.Compiled`**... should use `[GeneratedRegex]` source generators...  
> **Per-call `new Regex(...)`** is the most prevalent and impactful issue...

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**.  
**Verdict:** **Tie**; both are comprehensive and explicitly recommend `[GeneratedRegex]` with clear hot-path impact framing.

## 2. String Allocation Detection [CRITICAL]

Both outputs identify `+=` in loops, casing allocations/culture risk, and `.Replace()` chain amplification.

**dotnet-perf-skills excerpt:**
> **String Concatenation (`+=`) in Loops — O(n²) Allocation**...  
> **`.ToLower()`/`.ToUpper()` Without Culture or `StringComparison` (16 instances)**... Turkish-I problem...  
> `GenerateSlug`... 20+ intermediate string allocations.

**no-skills excerpt:**
> **Character-by-character `+=` string concatenation** in `ParseLine()` and `SplitLines()`... O(n²)...  
> **`.ToLower()` without `StringComparison.Ordinal`**... locale-sensitive (Turkish-I bug).  
> **Long chain of 44 `.Replace()` calls**, each allocating a new string.

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**.  
**Verdict:** **Tie**; both are excellent and concrete on allocation mechanics and correctness risk.

## 3. Collection and LINQ Efficiency [CRITICAL]

Both outputs cover O(n) lookup anti-patterns, materialization overhead, and sliding-window LINQ allocation issues.

**dotnet-perf-skills excerpt:**
> `ContainsKey` + indexer double-lookup | 15 | Across 6 files  
> **Unnecessary `.ToList()` Materializations (18 instances)**...  
> `List.Contains()` for Key Lookups... `JsonTransformer.Diff`... `SlugGenerator.GenerateUniqueSlug`... **Use `HashSet<string>`**

**no-skills excerpt:**
> **`allKeys` is a `List<string>` with `.Contains()` (O(n))**... Should be a `HashSet<string>`...  
> **`Skip(i).Take(5).ToList()` in a sliding window loop** — O(n²) allocations.  
> **`ContainsKey` + indexer double-lookup**... Replace with `TryGetValue`.

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**.  
**Verdict:** **Tie**; both clearly detect high-impact collection/LINQ inefficiencies and propose canonical replacements.

## 4. Async and IO Pattern Detection [CRITICAL]

Both outputs correctly identify `HttpClient` lifetime, sequential-await latency, unbounded parallelism, and cancellation gaps.

**dotnet-perf-skills excerpt:**
> **`new HttpClient()` Per Call — Socket Exhaustion Risk**...  
> **Unbounded Parallelism**... can saturate thread pool, exhaust connections...  
> **Missing `CancellationToken` on Async Operations**... `Task.Delay`... cannot be cancelled.

**no-skills excerpt:**
> **`new HttpClient()` per call**... causes **socket exhaustion** under load...  
> **Unbounded parallelism** in `SendBatchParallelAsync()`...  
> **Sequential `await` in a loop**... Use `Task.WhenAll` with a semaphore...

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**.  
**Verdict:** **Tie**; both outputs are production-oriented and technically accurate on async/IO failure modes.

## 5. Reflection and Serialization Overhead [HIGH]

Both outputs detect reflection and `JsonSerializerOptions` reuse, but skill-enabled output is stronger on quantified impact and consolidated framing.

**dotnet-perf-skills excerpt:**
> **Uncached `new JsonSerializerOptions` Per Call (5 instances)**... **Up to 592x slower**...  
> **Uncached Reflection in Mapping Hot Path**... `SetValue()`/`GetValue()` are ~100x slower...  
> **Fix:** Cache `PropertyInfo[]` per type in a `ConcurrentDictionary<Type, PropertyInfo[]>`.

**no-skills excerpt:**
> **`new JsonSerializerOptions { WriteIndented = true }` on every call**... metadata cache rebuilding.  
> **`typeof(T).GetProperties()` and `prop.SetValue()` on every call**... should cache `PropertyInfo[]` per type.  
> **`GetType().GetProperty()` in `ResolveValue`**... cache property lookups.

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills wins** due to sharper quantitative impact language and clearer cross-cutting prioritization.

## 6. Structural Optimization Detection [HIGH]

Both outputs discuss `sealed`, `IEquatable<T>`, and `FrozenDictionary`, but coverage depth differs significantly.

**dotnet-perf-skills excerpt:**
> **Unsealed Classes (18 of 18 classes — 0% sealed)**...  
> **Structs Without `IEquatable<T>` (2 instances)**...  
> **Static `readonly Dictionary` → `FrozenDictionary` Candidates (2 instances, 0 FrozenDictionary)**

**no-skills excerpt:**
> **Unsealed Leaf Classes (3/10 files)**... `MappingConfig`, `ValidationResult`, `Record`, `PipelineResult`...  
> `ValidationError` and `DeliveryResult` structs don't implement `IEquatable<T>`...  
> Static `Dictionary`... candidate for `FrozenDictionary`.

**Score:** dotnet-perf-skills **5/5**, no-skills **3/5**.  
**Verdict:** **dotnet-perf-skills wins clearly** with broader structural census and stronger codebase-level optimization framing.

## 7. Severity Classification Accuracy [HIGH]

Both differentiate severity tiers, but baseline prioritization is slightly more impact-aligned for hot-path criticals vs broad moderate issues.

**dotnet-perf-skills excerpt:**
> | 🔴 Critical | 6 | `new HttpClient()`... uncached `new Regex()`... uncached `JsonSerializerOptions` |  
> | 5 | Replace 15 `ContainsKey` + indexer with `TryGetValue` | 🔴 Critical | ... |

**no-skills excerpt:**
> | 🔴 Critical | 7 | Socket exhaustion, O(n²) hot-path patterns, per-line regex instantiation |  
> | 1 | Cache regex as `static readonly` fields in `LogAnalyzer.TryParseLine` ... **Estimated 100x+ improvement** |

**Score:** dotnet-perf-skills **3/5**, no-skills **4/5**.  
**Verdict:** **no-skills wins narrowly**; `dotnet-perf-skills` appears to over-escalate some lookup optimizations as critical relative to true top-tier hot-path risks.

## 8. Fix Recommendation Quality [HIGH]

Both provide actionable remediations; skill-enabled output is slightly stronger in API specificity and direct migration patterns.

**dotnet-perf-skills excerpt:**
> **Fix:** Inject `IHttpClientFactory` or use a `static readonly HttpClient` with `PooledConnectionLifetime`.  
> **Fix:** Convert all 48 to `[GeneratedRegex]` source-generated regexes. The class must become `partial`.  
> **Fix:** Chain `.ToFrozenDictionary()`... add `using System.Collections.Frozen;`.

**no-skills excerpt:**
> Use `IHttpClientFactory` or a shared static `HttpClient`.  
> Migrate `MarkdownStripper` from `RegexOptions.Compiled` to `[GeneratedRegex]` (or remove `Compiled`).  
> Add `SemaphoreSlim` throttling... `CancellationToken` support.

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills wins** on fix precision and deeper .NET API targeting.

## 9. Coverage Breadth and Granularity [MEDIUM]

The baseline report is broader and more granular (48 findings, file-by-file), while skill-enabled output is tighter and more curated.

**dotnet-perf-skills excerpt:**
> **Total issues found: 25 findings** across 10 files...  
> Top 3 priorities... 10-item prioritized recommendations.

**no-skills excerpt:**
> ...reveals **48 performance issues** across 10 source files spanning 7 categories.  
> **Findings by File** ... full per-file tables and fixes.

**Score:** dotnet-perf-skills **4/5**, no-skills **5/5**.  
**Verdict:** **no-skills wins** on raw breadth and per-file granularity.

## Weighted Summary

Weights: Critical ×3, High ×2, Medium ×1, Low ×0.5.

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 15 | 15 |
| String Allocation Detection [CRITICAL] | 15 | 15 |
| Collection and LINQ Efficiency [CRITICAL] | 15 | 15 |
| Async and IO Pattern Detection [CRITICAL] | 15 | 15 |
| Reflection and Serialization Overhead [HIGH] | 10 | 8 |
| Structural Optimization Detection [HIGH] | 10 | 6 |
| Severity Classification Accuracy [HIGH] | 6 | 8 |
| Fix Recommendation Quality [HIGH] | 10 | 8 |
| Coverage Breadth and Granularity [MEDIUM] | 4 | 5 |
| **Total Weighted Score** | **100** | **95** |

## What All Versions Get Right

- Both clearly identify the biggest production risks: per-line regex creation in `LogAnalyzer` and `new HttpClient()` per call in `NotificationService`.
- Both call out O(n²) string-building patterns and propose `StringBuilder`.
- Both detect collection hot-path inefficiencies (`List.Contains`, `ContainsKey`+indexer, unnecessary `.ToList()` materialization).
- Both recommend `[GeneratedRegex]` for .NET 8+ and mention startup/perf implications of heavy `RegexOptions.Compiled`.
- Both include actionable fix patterns with concrete .NET APIs, not just generic advice.

## Summary: Impact of Skills

Most impactful differences (ranked):
1. **Structural optimization depth:** `dotnet-perf-skills` gives a full codebase structural census (18/18 unsealed, explicit Frozen/IEquatable framing), while baseline is narrower.
2. **Fix precision:** `dotnet-perf-skills` is more explicit on exact APIs/migration patterns (`ToFrozenDictionary`, `PooledConnectionLifetime`, class `partial` for `[GeneratedRegex]`).
3. **Coverage breadth:** `no-skills` surfaces more total findings and more per-file granularity.
4. **Severity calibration:** `no-skills` is slightly more conservative/impact-aligned in critical ranking.

Overall, **dotnet-perf-skills ranks first (100 vs 95 weighted)**: it is more optimization-focused and technically prescriptive, while **no-skills** remains very strong and more exhaustive in raw enumeration.
