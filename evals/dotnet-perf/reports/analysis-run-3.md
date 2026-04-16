# Comparative Analysis: dotnet-perf-skills, dotnet-perf-skills-improved, no-skills

This run compares **3 configurations** across **1 shared scenario**: `analyze-perf-issues` at `output/{config}/run-3/analyze-perf-issues/performance-analysis.md`. Configuration identity was confirmed from each scenario’s `gen-notes.md`: `dotnet-perf-skills` (Performance Skills), `dotnet-perf-skills-improved` (Performance Skills improved), and `no-skills` (baseline/default Copilot).

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | dotnet-perf-skills-improved | no-skills |
|---|---:|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 5 | 4 |
| String Allocation Detection [CRITICAL] | 5 | 5 | 4 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 5 | 4 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 5 | 4 |
| Reflection and Serialization Overhead [HIGH] | 4 | 4 | 4 |
| Structural Optimization Detection [HIGH] | 5 | 5 | 3 |
| Severity Classification Accuracy [HIGH] | 5 | 4 | 3 |
| Fix Recommendation Quality [HIGH] | 5 | 4 | 3 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**Excerpt — dotnet-perf-skills** (`output/dotnet-perf-skills/run-3/analyze-perf-issues/performance-analysis.md`)
> #### 2. `new Regex()` Per Log Line in Hot Loop (4 instances in LogAnalyzer)  
> **Impact:** `TryParseLine` is called for every log line...  
> **Fix:** Hoist to `private static readonly Regex` fields, or use `[GeneratedRegex]` (preferred on .NET 8+).

**Excerpt — dotnet-perf-skills-improved** (`output/dotnet-perf-skills-improved/run-3/analyze-perf-issues/performance-analysis.md`)
> #### 9. Excessive `RegexOptions.Compiled` — 48 Instances (48 instances)  
> **Impact:** ...blows the compiled-regex budget...  
> **Fix:** ...replace all with `[GeneratedRegex]` source-generated attributes...

**Excerpt — no-skills** (`output/no-skills/run-3/analyze-perf-issues/performance-analysis.md`)
> 2. `LogAnalyzer.TryParseLine` — `new Regex()` per log line → O(n) regex compilations (🔴)  
> 4. `MarkdownStripper` — 40+ `RegexOptions.Compiled` instances → excessive JIT startup cost (🟡)  
> ...use `[GeneratedRegex]` for source-generated, zero-overhead regex.

**Score:** dotnet-perf-skills **5/5** · dotnet-perf-skills-improved **5/5** · no-skills **4/5**  
**Verdict:** **Tie between dotnet-perf-skills and dotnet-perf-skills-improved**; both explicitly cover hot-path `new Regex`, startup-bloat `RegexOptions.Compiled`, and `.NET 8+` `[GeneratedRegex]`.

## 2. String Allocation Detection [CRITICAL]

**Excerpt — dotnet-perf-skills**
> #### 7. String Concatenation (`+=`) in Loops — O(n²) Allocation (8+ locations)  
> #### 6. `.ToLower()`/`.ToUpper()` Without Culture or Ordinal (18 instances)  
> #### 8. Static `Regex.Replace()` Per Call — 12 Instances in SlugGenerator

**Excerpt — dotnet-perf-skills-improved**
> #### 7. String `+=` Concatenation in Loops — O(n²) Allocation (6 sites)  
> #### 8. `.ToLower()`/`.ToUpper()` Without Culture (15 instances)  
> **Fix:** Use `StringComparison.OrdinalIgnoreCase`... or `ToLowerInvariant()`

**Excerpt — no-skills**
> | 1 | 🔴 Critical | 51-79 | **Character-by-character string `+=` in `ParseLine`**... |  
> | 2 | 🔴 Critical | 88-108 | **Character-by-character string `+=` in `SplitLines`**... |  
> | 4 | 🟡 Moderate | 38, 123, 131, 145 | **`.ToLower()` without ordinal**... |

**Score:** dotnet-perf-skills **5/5** · dotnet-perf-skills-improved **5/5** · no-skills **4/5**  
**Verdict:** **Tie between both skills variants**; they are more systematic and explicitly connect loop allocations, casing allocations, and chained replacement allocations.

## 3. Collection and LINQ Efficiency [CRITICAL]

**Excerpt — dotnet-perf-skills**
> #### 14. `Skip(i).Take(5).ToList()` in Loop — O(n²) Sliding Window  
> #### 15. Keys.ToList() + Contains() for Key Union — O(n²)  
> #### 16. `ContainsKey` + Indexer Double-Lookup (12 instances)

**Excerpt — dotnet-perf-skills-improved**
> #### 11. `ContainsKey` + Indexer Double Lookup (8 instances)  
> #### 13. `ToList()` + `Contains()` Instead of HashSet — O(n²) Lookup (2 instances)  
> | `.ToList()` (potentially unnecessary) | **16** |

**Excerpt — no-skills**
> | 2 | 🟡 Moderate | 191 | **`.Distinct().ToList()` for tag deduplication**... |  
> | 4 | 🟡 Moderate | 157 | **`Skip(i).Take(5).ToList()` in a loop**... |  
> Pattern: `if (dict.ContainsKey(k)) dict[k]` performs two hash lookups.

**Score:** dotnet-perf-skills **5/5** · dotnet-perf-skills-improved **5/5** · no-skills **4/5**  
**Verdict:** **Tie between dotnet-perf-skills and dotnet-perf-skills-improved**; baseline is good, but skills outputs are more complete and more explicit on algorithmic complexity.

## 4. Async and IO Pattern Detection [CRITICAL]

**Excerpt — dotnet-perf-skills**
> #### 1. `new HttpClient()` Per Call — Socket Exhaustion Risk (3 instances)  
> #### 11. Sequential Awaits in Loop — No Parallelism (SendBatchAsync)  
> #### 13. Missing CancellationToken on Async Methods

**Excerpt — dotnet-perf-skills-improved**
> #### 1. `new HttpClient()` Per Call — Socket Exhaustion (3 instances)  
> #### 14. Sequential Awaits in Loop — No Parallelism (1 instance)  
> #### 15. Unbounded Parallelism in `SendBatchParallelAsync` (1 instance)

**Excerpt — no-skills**
> | 1 | 🔴 Critical | 163, 179, 191 | **`new HttpClient()` per call**... |  
> | 2 | 🔴 Critical | 116-119 | **Sequential `await` in loop**... |  
> | 3 | 🟡 Moderate | 130-133 | **Unbounded parallelism**... |

**Score:** dotnet-perf-skills **5/5** · dotnet-perf-skills-improved **5/5** · no-skills **4/5**  
**Verdict:** **Tie between both skills variants**; both cover all required async/IO anti-patterns with clearer production-risk framing.

## 5. Reflection and Serialization Overhead [HIGH]

**Excerpt — dotnet-perf-skills**
> #### 3. Uncached `new JsonSerializerOptions` Per Call (5 instances)  
> #### 9. Uncached Reflection: `GetProperties()` + `SetValue()`/`GetValue()` Per Call (2 instances)  
> #### 10. `TemplateEngine` Reflection in Hot Path

**Excerpt — dotnet-perf-skills-improved**
> #### 5. `new JsonSerializerOptions` Per Call — Up to 592x Slower (4 instances)  
> #### 10. Uncached Reflection — `GetProperties()`/`SetValue()`/`GetValue()` (5 call sites)  
> **Fix:** Cache `PropertyInfo[]` per type...

**Excerpt — no-skills**
> | 1 | 🟡 Moderate | 74, 117, 135, 142 | **`new JsonSerializerOptions` per call**... |  
> | 1 | 🟡 Moderate | 77 | **`GetProperties()` reflection per call (`MapTo<T>`)**... |  
> Reflection calls in hot paths without caching.

**Score:** dotnet-perf-skills **4/5** · dotnet-perf-skills-improved **4/5** · no-skills **4/5**  
**Verdict:** **Three-way tie**; all detect uncached reflection and serializer options well, but none strongly push partial-deserialization alternatives (`Utf8JsonReader`) in this run.

## 6. Structural Optimization Detection [HIGH]

**Excerpt — dotnet-perf-skills**
> | Structural | Unsealed classes | 18 of 18 (0 sealed) |  
> | Structural | Structs without `IEquatable<T>` | 2 of 2 (0 implement) |  
> #### 17. `static readonly Dictionary<>` — FrozenDictionary Candidates (2 instances)

**Excerpt — dotnet-perf-skills-improved**
> | `sealed class` | **0 of 18 classes** |  
> | `: IEquatable<T>` | **0 of 2 structs** |  
> #### 21. Static Read-Only Dictionaries — FrozenDictionary Candidates (2 instances)

**Excerpt — no-skills**
> ### 6. Unsealed Leaf Classes (3 files)  
> ### 5. Missing `IEquatable<T>` on Structs (2 files)  
> ...`FrozenDictionary` candidate...

**Score:** dotnet-perf-skills **5/5** · dotnet-perf-skills-improved **5/5** · no-skills **3/5**  
**Verdict:** **Tie between both skills variants**; baseline catches the categories but is narrower and less systematic on unsealed-class scope.

## 7. Severity Classification Accuracy [HIGH]

**Excerpt — dotnet-perf-skills** (`gen-notes.md` + `performance-analysis.md`)
> ...`new HttpClient()` per call was classified as 🔴 Critical... while `ContainsKey + indexer` was classified as ℹ️ Info...  
> ...18 instances of `.ToLower()`/`.ToUpper()` ... escalated ... to a 🟡 Moderate systematic issue.

**Excerpt — dotnet-perf-skills-improved**
> | 🔴 Critical | 7 | `new HttpClient()` per call... |  
> | 🟡 Moderate | 10 | `+= string` in loops... excessive `RegexOptions.Compiled` |  
> | ℹ️ Info | 5 | Missing capacity hints... |

**Excerpt — no-skills**
> 🔴 **Critical**: 8 issues — socket exhaustion, O(n²) string concatenation...  
> 🟡 **Moderate**: 24 issues — per-call regex instantiation...  
> ℹ️ **Info**: 20 issues...

**Score:** dotnet-perf-skills **5/5** · dotnet-perf-skills-improved **4/5** · no-skills **3/5**  
**Verdict:** **dotnet-perf-skills wins** for the clearest hot-path-aware calibration and explicit severity rationale.

## 8. Fix Recommendation Quality [HIGH]

**Excerpt — dotnet-perf-skills**
> **Fix:** Inject `IHttpClientFactory` or use a single `static readonly HttpClient` with `SocketsHttpHandler...`  
> **Fix:** ...`[GeneratedRegex]`...  
> **Fix:** Add `CancellationToken` parameter... pass to `Task.Delay` and `HttpClient` calls.

**Excerpt — dotnet-perf-skills-improved**
> **Fix:** Use bounded parallelism with `Parallel.ForEachAsync` or `SemaphoreSlim`.  
> **Fix:** Cache `PropertyInfo[]` per type using `ConcurrentDictionary<Type, PropertyInfo[]>`...  
> **Fix:** Use `StringComparison.OrdinalIgnoreCase`...

**Excerpt — no-skills**
> **Fix recommendations:** ...`IHttpClientFactory`... `StringBuilder`... `HashSet<string>`...  
> ...`ConcurrentDictionary<Type, PropertyInfo[]>`...  
> ...`[GeneratedRegex]`...

**Score:** dotnet-perf-skills **5/5** · dotnet-perf-skills-improved **4/5** · no-skills **3/5**  
**Verdict:** **dotnet-perf-skills is best**; recommendations are most consistently specific, API-accurate, and tied to measured impact.

## Weighted Summary

Weights used: **Critical ×3**, **High ×2**, **Medium ×1**, **Low ×0.5**.

| Configuration | Critical Raw (max 20) | Critical Weighted | High Raw (max 20) | High Weighted | Total Weighted |
|---|---:|---:|---:|---:|---:|
| dotnet-perf-skills | 20 | 60 | 19 | 38 | **98** |
| dotnet-perf-skills-improved | 20 | 60 | 17 | 34 | **94** |
| no-skills | 16 | 48 | 13 | 26 | **74** |

## What All Versions Get Right

- All three clearly identify the highest-risk `HttpClient` per-call anti-pattern.
- All three detect hot-path regex misuse in `LogAnalyzer` and call out `[GeneratedRegex]`.
- All three capture O(n²) string growth patterns (`+=` loops) and recommend `StringBuilder`.
- All three flag core reflection and JSON options caching opportunities in mapping/transform paths.

## Summary: Impact of Skills

The biggest skill-driven gains are: **(1)** stronger severity calibration, **(2)** more systematic structural coverage (ratios like `0/18 sealed`, `0/2 IEquatable`), and **(3)** higher-quality, API-specific fixes with fewer weak recommendations. Overall ranking by weighted score is **dotnet-perf-skills (98) > dotnet-perf-skills-improved (94) > no-skills (74)**. The improved variant remains strong and close on critical detection breadth, but the standard skills run is more consistent in prioritization quality and fix precision in this scenario.
