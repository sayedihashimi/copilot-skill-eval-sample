# Comparative Analysis: dotnet-perf-skills, no-skills

This run compares **2 configurations** in `output/*/run-3/analyze-perf-issues/`: `dotnet-perf-skills` (skill-guided analysis; see `gen-notes.md`) and `no-skills` (baseline/default Copilot, inferred from directory and notes). The evaluated scenario is **analyze-perf-issues** for the .NET performance anti-pattern corpus.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 4 |
| String Allocation Detection [CRITICAL] | 5 | 4 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 4 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 5 |
| Reflection and Serialization Overhead [HIGH] | 4 | 4 |
| Structural Optimization Detection [HIGH] | 5 | 3 |
| Severity Classification Accuracy [HIGH] | 4 | 3 |
| Fix Recommendation Quality [HIGH] | 5 | 4 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-3/analyze-perf-issues/performance-analysis.md`):
> #### 2. `new Regex()` per log line in hot path (8 instances)  
> **Impact:** `LogAnalyzer.TryParseLine` creates 2–3 `new Regex` objects per line.  
> **Fix:** Hoist to `static readonly` fields, or use `[GeneratedRegex]` on .NET 7+.

> #### 8. 48 `RegexOptions.Compiled` without `[GeneratedRegex]` (48 instances)  
> **Impact:** ... 48 compiled regexes is well above the recommended budget (~10–15).  
> **Fix:** Convert all 48 static regex fields to `[GeneratedRegex]` partial methods.

**no-skills** (`output/no-skills/run-3/analyze-perf-issues/performance-analysis.md`):
> 1. 🔴 **`new Regex(...)` per log line** (lines 50, 65, 75)  
> `TryParseLine` is called for every line in the log file.  
> **Fix:** Extract to `private static readonly Regex` fields

> 1. 🟡 **47 `RegexOptions.Compiled` static instances** (lines 13–59)  
> Having 47 compiled regexes exceeds the practical budget.  
> **Fix:** ... replace with `[GeneratedRegex]` source generators

**Score:** dotnet-perf-skills **5/5** (covers per-call instantiation, compiled-budget problem, and modern fix path with strong scale framing); no-skills **4/5** (detects both core issues and recommends `[GeneratedRegex]`, but slightly less precise depth and prioritization framing).

**Verdict:** **dotnet-perf-skills** is best due to stronger quantified impact and clearer startup-budget framing.

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills**:
> #### 4. String `+=` concatenation in loops — O(n²) (11+ sites)  
> In `CsvParser.ParseLine`/`SplitLines` this runs per-character...  
> **Fix:** Replace with `StringBuilder`

> #### 9. `.ToLower()`/`.ToUpper()` without culture (25 instances)  
> **Impact:** ... Turkish-I problem can cause bugs.  
> **Fix:** Use `StringComparison.OrdinalIgnoreCase` ... or `ToLowerInvariant()`

**no-skills**:
> 1. 🔴 **Character-by-character string concatenation** ...  
> `current += line[i]` ... For a CSV with 1M characters, this is O(n²).  
> **Fix:** Use `StringBuilder`

> ### 3. `.ToLower()` / `.ToUpper()` without Culture (7 files)  
> ... Causes the Turkish-I problem and unnecessary allocations.  
> **Recommendation:** Use `ToLowerInvariant()` / `ToUpperInvariant()` or `StringComparison.OrdinalIgnoreCase`.

**Score:** dotnet-perf-skills **5/5** (explicitly catches loop-concat hotspots, chained replacements, and culture-safe alternatives); no-skills **4/5** (good coverage of main patterns, slightly less systematic quantification).

**Verdict:** **dotnet-perf-skills** leads by breadth and tighter pattern accounting.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills**:
> #### 5. `ContainsKey` + indexer double-lookup (18 instances)  
> **Fix:** Replace with `TryGetValue` pattern.

> #### 17. `List.Contains` / `allKeys.Contains` — O(n) lookup (3 sites)  
> ... `allKeys` is a `List<string>` checked N times = O(n²).  
> **Fix:** Use `HashSet<string>`

**no-skills**:
> 5. 🟡 **`Skip(i).Take(5).ToList()` in sliding window** (line 157)  
> Allocates a new list on each iteration...  
> **Fix:** Use index-based access

> 5. ℹ️ **`.Distinct().ToList()` for tag dedup** (line 191)  
> Allocates two collections. Could use `HashSet<string>` for tags from the start.

**Score:** dotnet-perf-skills **5/5** (hits all requested hotspots: contains/lookups, materializations, capacity hints, and sliding-window allocation); no-skills **4/5** (strong and actionable, but less complete on some cross-file coverage density).

**Verdict:** **dotnet-perf-skills** provides the more comprehensive collection/LINQ optimization map.

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills**:
> #### 1. `new HttpClient()` per call — socket exhaustion (3 instances)  
> **Fix:** Inject `IHttpClientFactory` or use a `static readonly HttpClient`

> #### 7. Sequential awaits in loop — no parallelism (1 instance)  
> **Fix:** Use `Task.WhenAll` with throttling via `SemaphoreSlim`

> #### 13. Unbounded parallelism (1 instance)  
> #### 14. Missing `CancellationToken` on async methods

**no-skills**:
> 1. 🔴 **`new HttpClient()` per call — socket exhaustion**  
> 2. 🔴 **Sequential awaits in batch loop**  
> 3. 🔴 **Unbounded parallelism in `SendBatchParallelAsync`**

> 5. 🟡 **`Task.Delay` without `CancellationToken`**  
> **Fix:** Accept and pass `CancellationToken` through the async chain.

**Score:** dotnet-perf-skills **5/5**; no-skills **5/5**. Both detect all required async/IO anti-patterns with correct, deployable fixes.

**Verdict:** **Tie**. Both outputs are excellent on async/IO risk detection.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills**:
> #### 3. Uncached `new JsonSerializerOptions` per call (4 instances)  
> **Impact:** Up to 592× slower than caching...  
> **Fix:** Use a `private static readonly JsonSerializerOptions` field.

> #### 15. Uncached reflection — `GetProperties()`/`SetValue()`/`GetValue()` per call (4 sites)  
> **Fix:** Cache `PropertyInfo[]` per type in a `ConcurrentDictionary<Type, PropertyInfo[]>`.

**no-skills**:
> 1. 🔴 **Uncached `GetProperties()` reflection per call** ...  
> **Fix:** Cache in a `ConcurrentDictionary<Type, PropertyInfo[]>`

> 1. 🟡 **`new JsonSerializerOptions` per call** ...  
> **Fix:** Extract to a static field

**Score:** dotnet-perf-skills **4/5**; no-skills **4/5**. Both identify core reflection and serializer-option caching issues clearly; neither strongly develops partial-deserialization alternatives as a primary recommendation.

**Verdict:** **Tie** on core coverage quality.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills**:
> #### 10. Unsealed classes — 17 of 17 (0% sealed)  
> **Fix:** Add `sealed` keyword to all leaf classes.

> #### 11. Structs without `IEquatable<T>` (2 of 2 structs)  
> #### 12. `static readonly Dictionary<>` — FrozenDictionary candidates (2 instances)

**no-skills**:
> ### 5. Missing `IEquatable<T>` on Structs (2 files)  
> ... `DeliveryResult` and `ValidationError` are structs without `IEquatable<T>`

> ### 6. Unsealed Classes (3 files)  
> `Record`, `MappingConfig`, `ValidationResult` are leaf classes that could be sealed

**Score:** dotnet-perf-skills **5/5** (full requested surface with strong ratios and .NET 8+ `FrozenDictionary` targeting); no-skills **3/5** (finds key items but under-reports unsealed-class breadth and is less systematic).

**Verdict:** **dotnet-perf-skills** is clearly better for structural/JIT-aware optimization coverage.

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills**:
> | 🔴 Critical | 7 | `new HttpClient` per call ..., uncached `new Regex` in hot loops, uncached `JsonSerializerOptions` |  
> | 🟡 Moderate | 12 | 48 `RegexOptions.Compiled` ..., 25× `.ToLower()`/`.ToUpper()`..., 17 unsealed classes |

**no-skills**:
> | 🔴 Critical | 8 |  
> | 🟡 Moderate | 22 |  
> | ℹ️ Info | 18 |

> **Top priorities:** Fix `new HttpClient` per-call patterns ..., cache Regex instances in hot paths ..., replace string concatenation loops ...

**Score:** dotnet-perf-skills **4/5** (generally strong hot-path vs moderate-tier separation, though a few borderline classifications remain); no-skills **3/5** (good priority list, but tiering is broader/less discriminating and some issues are less tightly calibrated to impact).

**Verdict:** **dotnet-perf-skills** has more reliable prioritization signal.

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills**:
> **Fix:** Inject `IHttpClientFactory` or use a `static readonly HttpClient` with `PooledConnectionLifetime`.  
> **Fix:** ... use `[GeneratedRegex]` on .NET 7+.  
> **Fix:** Convert to `FrozenDictionary` using `.ToFrozenDictionary()`.

**no-skills**:
> **Fix:** Use `Task.WhenAll` with throttling ... `SemaphoreSlim`  
> **Fix:** Use `string.Equals(..., StringComparison.OrdinalIgnoreCase)`  
> **Fix:** Cache in a `ConcurrentDictionary<Type, PropertyInfo[]>`

**Score:** dotnet-perf-skills **5/5** (specific API-level fixes, concrete patterns, and consistently actionable guidance); no-skills **4/5** (mostly strong and specific, but a bit more uneven in precision across categories).

**Verdict:** **dotnet-perf-skills** provides the highest-quality remediation guidance.

## Weighted Summary

Weights used: **Critical ×3**, **High ×2**, **Medium ×1**, **Low ×0.5**.

| Configuration | Critical Weighted (4 dims) | High Weighted (4 dims) | Medium Weighted | Low Weighted | Total Weighted Score |
|---|---:|---:|---:|---:|---:|
| dotnet-perf-skills | 60 | 36 | 0 | 0 | **96** |
| no-skills | 51 | 28 | 0 | 0 | **79** |

## What All Versions Get Right

- Both identify the highest-risk production issues: per-call `HttpClient`, per-call regex in hot parsing, and loop-based string concatenation.
- Both provide concrete .NET-native fix directions (`StringBuilder`, `TryGetValue`, `IHttpClientFactory`, regex caching/generation).
- Both include prioritized recommendations rather than only raw findings.
- Both call out async concurrency control concerns (sequential await vs unbounded fan-out).

## Summary: Impact of Skills

The most impactful differences are: **(1)** stronger structural optimization coverage (17/17 unsealed, 2/2 `IEquatable` gaps, `FrozenDictionary` candidates), **(2)** more quantified regex/startup-budget analysis, and **(3)** tighter severity stratification tied to hot-path impact in `dotnet-perf-skills`.  
Overall assessment by weighted score: **dotnet-perf-skills (96)** is the strongest output and materially outperforms **no-skills (79)** in completeness and prioritization quality, while both remain strong on async/IO fundamentals.
