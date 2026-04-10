# Comparative Analysis: no-skills, dotnet-perf-skills

This report compares **2 configurations** across **1 scenario** (`analyze-perf-issues`) using `output/{config}/run-5/analyze-perf-issues/`. Configuration identity comes from `gen-notes.md` plus directory naming: `no-skills` is baseline, and `dotnet-perf-skills` is the skill-enabled run (explicitly citing `analyzing-dotnet-performance`).

## Executive Summary

| Dimension [Tier] | no-skills | dotnet-perf-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 4 | 5 |
| String Allocation Detection [CRITICAL] | 4 | 5 |
| Collection and LINQ Efficiency [CRITICAL] | 4 | 5 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 5 |
| Reflection and Serialization Overhead [HIGH] | 4 | 5 |
| Structural Optimization Detection [HIGH] | 4 | 5 |
| Severity Classification Accuracy [HIGH] | 4 | 4 |
| Fix Recommendation Quality [HIGH] | 4 | 5 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**no-skills** (`output/no-skills/run-5/analyze-perf-issues/performance-analysis.md`)
> **`new Regex()` on every line** in `TryParseLine` ... called per log line ...  
> For a 1M-line log file, this is 3M regex compilations.  
> **>100x slower** than a cached static regex.

> **45 `RegexOptions.Compiled` static regexes** ...  
> these should use `[GeneratedRegex]` source generators for zero startup cost.

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-5/analyze-perf-issues/performance-analysis.md`)
> `RegexOptions.Compiled` \| 48  
> `[GeneratedRegex]` \| 0  
> `new Regex(` \| 8

> In `LogAnalyzer.TryParseLine` this runs per log line (potentially millions of times) ...  
> On .NET 8 these should all use `[GeneratedRegex]` source generator.

**Score:** no-skills **4/5**; dotnet-perf-skills **5/5**.  
**Verdict:** **dotnet-perf-skills** is best due to stronger quantification and clearer startup vs hot-path framing.

## 2. String Allocation Detection [CRITICAL]

**no-skills**
> Char-by-char `+=` string concatenation in `ParseLine` and `SplitLines` ...  
> For a 10K-character line this is ~10K allocations.

> String `+=` in loops is O(n²) ... Replace with `StringBuilder`.

> `.ToLower()` without specifying `CultureInfo.InvariantCulture` ... Turkish-I problem.

**dotnet-perf-skills**
> `string +=` concatenation in loops — O(n²) allocation (6 instances)

> `.ToLower()`/`.ToUpper()` without culture — 25 instances ...  
> Use `StringComparison.OrdinalIgnoreCase` or `ToLowerInvariant()`.

> Chained `.Replace()` calls — 9 allocations per slug.

**Score:** no-skills **4/5**; dotnet-perf-skills **5/5**.  
**Verdict:** **dotnet-perf-skills** is best; both detect core problems, but skills output is broader and more consistently quantified.

## 3. Collection and LINQ Efficiency [CRITICAL]

**no-skills**
> `allKeys` is a `List<string>` with `.Contains()` (O(n)) in `Diff` ... should be `HashSet<string>`.

> `Skip(i).Take(5).ToList()` in a loop — O(n²) total allocations for the sliding window.

> `ContainsKey` + indexer ... use `TryGetValue` to avoid double lookup.

**dotnet-perf-skills**
> `List.Contains()` O(n) lookup in loops ... Should use `HashSet<string>` for O(1) lookups.

> Unnecessary `.ToList()` materializations (20 instances).

> `ContainsKey` + indexer double-lookup (12 instances) ... Use `TryGetValue`.

**Score:** no-skills **4/5**; dotnet-perf-skills **5/5**.  
**Verdict:** **dotnet-perf-skills** is best for coverage density and sharper cross-file aggregation.

## 4. Async and IO Pattern Detection [CRITICAL]

**no-skills**
> **`new HttpClient()` per call** ... causes **socket exhaustion** under load ...  
> Must use `IHttpClientFactory` or a single static `HttpClient`.

> **Unbounded parallelism** in `SendBatchParallelAsync` ...  
> **Sequential awaits in a loop** in `SendBatchAsync`.

> `Task.Delay` without `CancellationToken` — retries cannot be cancelled.

**dotnet-perf-skills**
> `new HttpClient()` per call — socket exhaustion risk (3 instances)

> Unbounded parallelism in `SendBatchParallelAsync` ...  
> Sequential awaits in batch loop ...  
> Missing cancellation tokens in async methods (`Task.Delay` cannot be interrupted).

**Score:** no-skills **5/5**; dotnet-perf-skills **5/5**.  
**Verdict:** **Tie**. Both are comprehensive and actionable on the requested async/IO anti-pattern set.

## 5. Reflection and Serialization Overhead [HIGH]

**no-skills**
> `new JsonSerializerOptions` per call ... defeats the internal caching in `System.Text.Json`.

> `typeof(TTarget).GetProperties()` on every call ... should be cached per type.

> `prop.SetValue()` and `prop.GetValue()` per property ... use cached delegates.

**dotnet-perf-skills**
> Uncached `JsonSerializerOptions` per call (7 instances) ... up to 592× slower.

> Uncached reflection `GetProperties()`/`SetValue()`/`GetValue()` ...  
> Cache `PropertyInfo[]` per type in a `ConcurrentDictionary<Type, PropertyInfo[]>`.

**Score:** no-skills **4/5**; dotnet-perf-skills **5/5**.  
**Verdict:** **dotnet-perf-skills** is best due to stronger quantified impact and clearer cache patterns.

## 6. Structural Optimization Detection [HIGH]

**no-skills**
> `ValidationError` struct without `IEquatable<ValidationError>`.

> Static `Dictionary` never mutated — `FrozenDictionary` candidate.

> Unsealed nested classes `Record`, `PipelineResult`.

**dotnet-perf-skills**
> 17 unsealed classes, 0 sealed (17/17 unsealed).

> Structs without `IEquatable<T>` (2 instances).

> `FrozenDictionary` candidates — 2 static readonly dictionaries.

**Score:** no-skills **4/5**; dotnet-perf-skills **5/5**.  
**Verdict:** **dotnet-perf-skills** is best with systematic codebase-level saturation metrics.

## 7. Severity Classification Accuracy [HIGH]

**no-skills**
> 🔴 Critical: 6 issues — socket exhaustion from `new HttpClient`, O(n²) string concatenation in hot paths, regex instantiation per log line, unbounded parallelism.

> Rank 1: Replace `new HttpClient()` per call ... 🔴 Critical  
> Rank 2: Cache regex in `LogAnalyzer` ... 🔴 Critical

**dotnet-perf-skills**
> 🔴 Critical \| 10 \| ... `new HttpClient()` per call, per-call `new Regex()` in hot loops, uncached `JsonSerializerOptions`, `string +=` in loops, `ContainsKey` + indexer double-lookup.

> Top 3 priorities: (1) `new HttpClient()`, (2) per-call `new Regex()`, (3) `string +=` loops.

**Score:** no-skills **4/5**; dotnet-perf-skills **4/5**.  
**Verdict:** **Tie**. Both prioritize true hotspots well; both also slightly over-escalate some non-top-tier issues in places.

## 8. Fix Recommendation Quality [HIGH]

**no-skills**
> inject `IHttpClientFactory` or use a static `HttpClient` ...

> use `[GeneratedRegex]` (preferred on .NET 7+) ...

> use `StringBuilder`, `HashSet<string>`, and `TryGetValue`.

**dotnet-perf-skills**
> Inject `IHttpClientFactory` or use a single static readonly `HttpClient` with `PooledConnectionLifetime`.

> Convert to `[GeneratedRegex]` on .NET 8 ... with concrete partial-method pattern.

> Use `Parallel.ForEachAsync` with `MaxDegreeOfParallelism`; use `StringComparison.OrdinalIgnoreCase`; use `FrozenDictionary`.

**Score:** no-skills **4/5**; dotnet-perf-skills **5/5**.  
**Verdict:** **dotnet-perf-skills** is best for specificity, API precision, and production-ready implementation detail.

## Weighted Summary

Weights used: **Critical ×3**, **High ×2**, **Medium ×1**, **Low ×0.5**.

| Configuration | Critical Subtotal | High Subtotal | Weighted Total |
|---|---:|---:|---:|
| no-skills | (4+4+4+5) × 3 = 51 | (4+4+4+4) × 2 = 32 | **83** |
| dotnet-perf-skills | (5+5+5+5) × 3 = 60 | (5+5+4+5) × 2 = 38 | **98** |

## What All Versions Get Right

- Both correctly identify the top production-risk issues: `HttpClient` lifetime misuse, per-call regex in hot paths, and O(n²) string construction.
- Both provide concrete .NET-native remediation directions (`StringBuilder`, `TryGetValue`, `HashSet`, `[GeneratedRegex]`, `IHttpClientFactory`).
- Both cover not only hot-path bugs but also structural optimization opportunities (`FrozenDictionary`, sealing, `IEquatable<T>`).

## Summary: Impact of Skills

Most impactful differences are: **(1)** stronger quantification and scan-style completeness, **(2)** better cross-file aggregation of repeated anti-patterns, and **(3)** more API-specific fix guidance in the skill-enabled output. Based on weighted totals, **dotnet-perf-skills (98)** clearly outperforms **no-skills (83)**; baseline quality is solid, but the skills version is more comprehensive and triage-ready.
