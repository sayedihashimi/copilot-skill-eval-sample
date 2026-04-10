# Comparative Analysis: dotnet-perf-skills, no-skills

This run compares **2 configurations** over **1 scenario**: `analyze-perf-issues` (`output/{config}/run-3/analyze-perf-issues/performance-analysis.md`). Configuration identity comes from each run’s `gen-notes.md`: `dotnet-perf-skills` explicitly used the `analyzing-dotnet-performance` skill, while `no-skills` is the baseline inferred from directory naming and generic notes.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 4 |
| String Allocation Detection [CRITICAL] | 5 | 4 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 4 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 4 |
| Reflection and Serialization Overhead [HIGH] | 5 | 4 |
| Structural Optimization Detection [HIGH] | 5 | 3 |
| Severity Classification Accuracy [HIGH] | 3 | 4 |
| Fix Recommendation Quality [HIGH] | 5 | 4 |
| Evidence Quantification and Traceability [MEDIUM] | 5 | 4 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-3/analyze-perf-issues/performance-analysis.md`)
> #### 3. `new Regex()` per log line in hot parsing loop (4 instances in LogAnalyzer)  
> **Impact:** `TryParseLine` is called once per log line — potentially millions of times.  
> **Fix:** Hoist to `private static readonly Regex` fields or use `[GeneratedRegex]` on .NET 7+.

> #### 9. 47 `RegexOptions.Compiled` without `[GeneratedRegex]` (47 instances)  
> **Impact:** ... 47 instances far exceed the recommended budget.

**no-skills** (`output/no-skills/run-3/analyze-perf-issues/performance-analysis.md`)
> **Regex instantiated per line** in `LogAnalyzer.TryParseLine` — O(n) Regex compilations on hot path...  
> **40+ `RegexOptions.Compiled` static fields** in `MarkdownStripper` — excessive JIT compilation budget at startup

> On .NET 7+, `[GeneratedRegex]` source generators should replace both `RegexOptions.Compiled` and `new Regex()` in hot paths.

**Score:** dotnet-perf-skills **5/5** (more explicit counts, file-level precision, and startup/hot-path split); no-skills **4/5** (strong detection, slightly less rigorous quantification).  
**Verdict:** **dotnet-perf-skills** is best.

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills**
> #### 11. String `+=` concatenation in loops — O(n²) allocation (10+ methods)  
> ... `ParseLine, SplitLines, FormatCsv ... Summarize ... ProcessLoops ...`

> #### 20. Chained `.Replace()` calls — 47 intermediate string allocations (MarkdownStripper)

**no-skills**
> **Character-by-character string concatenation in `ParseLine()`**... `current += line[i]` ... O(n²).  
> **String concatenation for CSV output** in `FormatCsv()`.

> Calls to `.ToLower()` and `.ToUpper()` are used for case-insensitive comparison instead of `StringComparison.OrdinalIgnoreCase`...

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best for breadth and tighter aggregation.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills**
> #### 7. ContainsKey + indexer double-lookup pattern (15 instances)  
> **Fix:** Replace with `TryGetValue`.

> #### 18. `.ToList()` materializations ... (18 instances)  
> Notable: `Skip(i).Take(5).ToList()` inside a loop...

**no-skills**
> Dictionary access uses `ContainsKey()` followed by the indexer, performing two hash lookups where `TryGetValue()` would do one.

> **Use `HashSet<string>` instead of `List.Contains()`** in `JsonTransformer.Diff()` and `SlugGenerator.GenerateUniqueSlug()`

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best due to stronger counting and hotspot specificity.

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills**
> #### 1. `new HttpClient()` per call — socket exhaustion (3 instances)  
> **Fix:** Inject `IHttpClientFactory` or use a single `static readonly HttpClient` with `PooledConnectionLifetime`.

> #### 17. Missing `CancellationToken` in async methods  
> ... `Task.Delay` in retry loop cannot be cancelled.

**no-skills**
> **`new HttpClient()` per call** ... causes **socket exhaustion**...  
> **Sequential `await` in loop** in `SendBatchAsync()`...

> **Unbounded parallelism** in `SendBatchParallelAsync()`...  
> **`Task.Delay` without `CancellationToken`** in retry loop.

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best (more complete API-level remediation detail).

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills**
> #### 2. Uncached `new JsonSerializerOptions` per call (5 instances)  
> **Impact:** Up to 592× slower than cached options...

> #### 14. Uncached reflection — `GetProperties()`/`SetValue()`/`GetValue()` per call (5 instances)

**no-skills**
> **`new JsonSerializerOptions` on every call.** ... each allocate a new `JsonSerializerOptions { WriteIndented = true }`.

> **Uncached `GetProperties()` / `SetValue()` / `GetValue()` in `MapTo<T>()` and `MapFrom<T>()`**...

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best on quantitative impact framing.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills**
> #### 12. Unsealed leaf classes — 17 of 17 classes are unsealed  
> #### 13. Structs without `IEquatable<T>` (2 instances)

> #### 21. `static readonly Dictionary<>` — `FrozenDictionary` candidates (2 instances)

**no-skills**
> ### 5. Unsealed Leaf Classes (across 3/10 files)  
> **Files affected:** `DataPipeline.Record`, `ValidationEngine.ValidationResult`, `EntityMapper.MappingConfig`

> ### 6. Structs Without `IEquatable<T>` (across 2/10 files)

**Score:** dotnet-perf-skills **5/5**; no-skills **3/5**.  
**Verdict:** **dotnet-perf-skills** is best; baseline under-reports unsealed-class scope.

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills**
> | 🔴 Critical | 8 | ... `new HttpClient()` ... `new Regex()` ... |  
> #### 7. ContainsKey + indexer double-lookup pattern (15 instances)

**no-skills**
> | 29 | 🔴 Critical | ... `new HttpClient()` per call ... |  
> | 50 | ℹ️ Info | ... `ContainsKey` + indexer ... |

**Score:** dotnet-perf-skills **3/5** (good on major hazards, but over-elevates some medium-impact patterns); no-skills **4/5** (better hot-path vs moderate/info separation).  
**Verdict:** **no-skills** is best for prioritization calibration.

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills**
> **Fix:** Inject `IHttpClientFactory` or use ... `SocketsHttpHandler` with `PooledConnectionLifetime`.  
> **Fix:** Use `StringComparison.OrdinalIgnoreCase` ... `ToLowerInvariant()`.

> **Caveat:** Requires `partial class` declaration... for `[GeneratedRegex]`.

**no-skills**
> // Issue 29: Use IHttpClientFactory or a shared static instance  
> // Or better — inject IHttpClientFactory via constructor

> // Issue 67: FrozenDictionary on .NET 8+  
> ... `.ToFrozenDictionary();`

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best (more consistently precise and caveated).

## 9. Evidence Quantification and Traceability [MEDIUM]

**dotnet-perf-skills**
> ## Scan Execution Checklist  
> | `.ToLower()/.ToUpper()` without culture | 15 |  
> | `RegexOptions.Compiled` | 47 |  
> | `new HttpClient(` | 3 |

**no-skills**
> **Total issues found: 62**  
> | 🔴 Critical | 7 |  
> | 🟡 Moderate | 28 |  
> | ℹ️ Info | 27 |

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best due to richer per-pattern counts and inverse checks.

## Weighted Summary

Weights: Critical ×3, High ×2, Medium ×1, Low ×0.5.

| Configuration | Critical subtotal | High subtotal | Medium subtotal | Total weighted score |
|---|---:|---:|---:|---:|
| dotnet-perf-skills | 60 | 36 | 5 | **101** |
| no-skills | 48 | 30 | 4 | **82** |

## What All Versions Get Right

- Correctly flag `new HttpClient()` per call as production-risky.
- Correctly identify regex misuse in hot paths and recommend caching / `[GeneratedRegex]`.
- Detect O(n²) string concatenation (`+=`) patterns and recommend `StringBuilder`.
- Surface async throughput issues (sequential awaits, unbounded fan-out).
- Include actionable, API-specific fix guidance rather than purely generic advice.

## Summary: Impact of Skills

Most impactful differences, ranked: **(1)** broader structural detection (17/17 unsealed classes surfaced), **(2)** stronger quantified evidence (`Scan Execution Checklist` with hit counts), **(3)** more complete hotspot framing for regex/collection/string patterns.  
Overall: `dotnet-perf-skills` is the strongest output by weighted score (**101 vs 82**), while `no-skills` remains solid and is slightly better calibrated on severity in a few moderate patterns.
