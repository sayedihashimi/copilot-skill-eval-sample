# Comparative Analysis: dotnet-perf-skills, dotnet-perf-skills-improved, no-skills

This comparison evaluates **3 configuration directories** for the single run-1 scenario `analyze-perf-issues` (`output/{config}/run-1/analyze-perf-issues/`). Based on each directory’s `gen-notes.md`, the effective setups were: `dotnet-perf-skills` (baseline-style notes, no explicit skill callout), `dotnet-perf-skills-improved` (explicit plugin skill usage), and `no-skills` (explicit primary skill usage), then each was compared on the same 8 required dimensions.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | dotnet-perf-skills-improved | no-skills |
|---|---:|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 4 | 5 | 4 |
| String Allocation Detection [CRITICAL] | 4 | 5 | 5 |
| Collection and LINQ Efficiency [CRITICAL] | 4 | 5 | 5 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 2 | 5 |
| Reflection and Serialization Overhead [HIGH] | 5 | 5 | 4 |
| Structural Optimization Detection [HIGH] | 5 | 5 | 3 |
| Severity Classification Accuracy [HIGH] | 4 | 5 | 3 |
| Fix Recommendation Quality [HIGH] | 4 | 5 | 4 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-1/analyze-perf-issues/performance-analysis.md`)
> | 🔴 Critical | 9 | `new HttpClient()` per call (socket exhaustion), uncached `new Regex()` in per-line parsing, uncached `JsonSerializerOptions` |  
> | 🟡 Moderate | 22 | 48 `RegexOptions.Compiled` without `[GeneratedRegex]`...

**dotnet-perf-skills-improved** (`output/dotnet-perf-skills-improved/run-1/analyze-perf-issues/performance-analysis.md`)
> #### 3. Per-Call `new Regex()` in Hot Loop — LogAnalyzer (4 instances)  
> **Impact:** `TryParseLine` is called per log line... extreme hot-path regression (>10× slower than cached).  
> #### 5. 48 `RegexOptions.Compiled` Without `[GeneratedRegex]`...

**no-skills** (`output/no-skills/run-1/analyze-perf-issues/performance-analysis.md`)
> 2. Regex instantiation per log line in `LogAnalyzer.TryParseLine` — O(n) compilations on hot path  
> **12. 🟡 Moderate — 46 `RegexOptions.Compiled` instances (lines 13-59)**  
> **Fix:** ... use `[GeneratedRegex]`

**Score:** dotnet-perf-skills **4/5**, dotnet-perf-skills-improved **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills-improved** is best because it clearly ties per-line regex allocation to hot-path impact and explicitly frames 48 compiled regexes as startup/throughput debt with direct `[GeneratedRegex]` migration guidance.

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills**
> | 🟡 Moderate | 22 | ...25 `.ToLower()`/`.ToUpper()` without culture, `+=` string concat in loops |  
> | `.Replace(` | 65 |

**dotnet-perf-skills-improved**
> #### 4. String `+=` Concatenation in Loops — O(n²) Allocation (6 instances)  
> **Fix:** Replace with `StringBuilder`.  
> #### 6. `.ToLower()`/`.ToUpper()` Without Ordinal/Culture (18 instances)

**no-skills**
> **58. 🔴 Critical — Character-by-character string concatenation ...**  
> `current += line[i]` creates a new string for every character... O(n²) allocations.  
> **13. 🟡 Moderate — Long chain of `.Replace()` calls...**

**Score:** dotnet-perf-skills **4/5**, dotnet-perf-skills-improved **5/5**, no-skills **5/5**.  
**Verdict:** **Tie: dotnet-perf-skills-improved and no-skills**. Improved is better structured and ordinal-focused; no-skills gives very concrete char-by-char hot-path detail.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills**
> | `ContainsKey` | 18 |  
> #### 8. `List.Contains()` for Key Lookups — O(n²) in Diff (2 instances)  
> #### 17. `Skip(i).Take(5).ToList()` in Loop — O(n²) Sliding Window (1 instance)

**dotnet-perf-skills-improved**
> #### 7. `ContainsKey` + Indexer Double-Lookup (~10 instances)  
> #### 10. `List.Contains()` O(n) Where HashSet Would Be O(1) (3 instances)  
> #### 13. `Skip().Take().ToList()` in Loop — Sliding Window (1 instance)

**no-skills**
> **29. 🟡 Moderate — `ContainsKey` + indexer pattern...**  
> **9. 🟡 Moderate — `List.Contains()` in a loop...**  
> **30. 🟡 Moderate — `Skip(i).Take(5).ToList()` in loop...**

**Score:** dotnet-perf-skills **4/5**, dotnet-perf-skills-improved **5/5**, no-skills **5/5**.  
**Verdict:** **Tie: dotnet-perf-skills-improved and no-skills** for full coverage of requested patterns with concrete replacements (`TryGetValue`, `HashSet`, index-based windows).

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills**
> #### 1. `new HttpClient()` Per Call — Socket Exhaustion (3 instances)  
> #### 9. Sequential Awaits in Loop — No Parallelism (1 instance)  
> #### 14. Unbounded Parallelism in `SendBatchParallelAsync` (1 instance)  
> #### 15. Missing `CancellationToken` on Async Methods...

**dotnet-perf-skills-improved**
> #### 1. `new HttpClient()` Per Call — Socket Exhaustion (3 instances)  
> **Top priorities:** Fix `new HttpClient()`..., cache `JsonSerializerOptions`...

**no-skills**
> **15. 🔴 Critical — `new HttpClient()` per call...**  
> **16. 🔴 Critical — Unbounded parallelism in `SendBatchParallelAsync`...**  
> **17. 🟡 Moderate — Sequential awaits in `SendBatchAsync`...**  
> **18. 🟡 Moderate — Missing `CancellationToken` on all async methods**

**Score:** dotnet-perf-skills **5/5**, dotnet-perf-skills-improved **2/5**, no-skills **5/5**.  
**Verdict:** **Tie: dotnet-perf-skills and no-skills**. Both hit all required async/IO anti-patterns; improved output heavily under-covers this dimension beyond HttpClient.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills**
> #### 7. Uncached Reflection: `GetProperties()`/`SetValue()`/`GetValue()` Per Call (4 instances)  
> | 🔴 Critical | 9 | ...uncached `JsonSerializerOptions` |

**dotnet-perf-skills-improved**
> #### 2. Uncached `new JsonSerializerOptions` Per Call (5 instances)  
> #### 8. Uncached Reflection — `GetProperties()`/`SetValue()`/`GetValue()` (6 instances)

**no-skills**
> **47. 🔴 Critical — Uncached `GetProperties()` and `SetValue()` reflection...**  
> **19. 🟡 Moderate — `new JsonSerializerOptions` per serialization...**

**Score:** dotnet-perf-skills **5/5**, dotnet-perf-skills-improved **5/5**, no-skills **4/5**.  
**Verdict:** **Tie: dotnet-perf-skills and dotnet-perf-skills-improved**. Both classify and explain reflection + serializer-option caching impact strongly; no-skills detects both but under-prioritizes serializer options.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills**
> | Unsealed classes | 17 |  
> | Structs without `IEquatable<T>` | 2 of 2 |  
> | `static readonly FrozenDictionary<` | 0 |

**dotnet-perf-skills-improved**
> #### 11. Unsealed Classes (17 of 17 classes are unsealed)  
> #### 12. Structs Without `IEquatable<T>` (2 of 2 structs)  
> #### 15. `FrozenDictionary` Candidates (2 instances)

**no-skills**
> ### 5. Unsealed Classes (affects 3/10 files)  
> ### 6. Structs Without `IEquatable<T>` (affects 2/10 files)  
> **11. ℹ️ Info — `ReplacementMap` could be `FrozenDictionary`...**

**Score:** dotnet-perf-skills **5/5**, dotnet-perf-skills-improved **5/5**, no-skills **3/5**.  
**Verdict:** **Tie: dotnet-perf-skills and dotnet-perf-skills-improved**. They provide broader structural coverage and stronger scale framing; no-skills is narrower and less systematic on unsealed-class breadth.

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills**
> | 🔴 Critical | 9 | `new HttpClient()`..., uncached `new Regex()` in per-line parsing, uncached `JsonSerializerOptions` |  
> | 🟡 Moderate | 22 | 48 `RegexOptions.Compiled`..., `+=` string concat in loops |

**dotnet-perf-skills-improved**
> | 🔴 Critical | 5 | ...per-line hot loop..., `+=` string concat in loops (O(n²)), 48 `RegexOptions.Compiled`... |  
> **Impact:** `TryParseLine` is called per log line... (>10× slower...)

**no-skills**
> | 🔴 Critical | 8 |  
> **19. 🟡 Moderate — `new JsonSerializerOptions` per serialization...**  
> **12. 🟡 Moderate — 46 `RegexOptions.Compiled` instances...**

**Score:** dotnet-perf-skills **4/5**, dotnet-perf-skills-improved **5/5**, no-skills **3/5**.  
**Verdict:** **dotnet-perf-skills-improved** is best at separating hot-path criticals from moderate findings with clearer impact calibration.

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills**
> **Fix:** Inject `IHttpClientFactory` or use a `static readonly HttpClient`...  
> **Fix:** Add `CancellationToken cancellationToken = default`; pass to `Task.Delay` and `HttpClient` calls.  
> **Fix:** Use `TryGetValue`.

**dotnet-perf-skills-improved**
> **Fix:** Convert to `[GeneratedRegex]` static partial methods...  
> **Fix:** Use `StringComparison.OrdinalIgnoreCase`... `ToLowerInvariant()`...  
> **Prioritized Fix Recommendations** include API-specific steps (`IHttpClientFactory`, `FrozenDictionary`, `HashSet`).

**no-skills**
> **Fix:** Use `SemaphoreSlim` for throttling.  
> **Fix:** Cache property info with `ConcurrentDictionary<Type, PropertyInfo[]>`.  
> **Fix:** use `[GeneratedRegex]` / `StringBuilder` / `TryGetValue`.

**Score:** dotnet-perf-skills **4/5**, dotnet-perf-skills-improved **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills-improved** has the strongest actionability: specific APIs, concise migration patterns, and priority ordering with impact framing.

## Weighted Summary

Weights applied: **Critical ×3**, **High ×2**, **Medium ×1**, **Low ×0.5** (no Medium/Low dimensions in this rubric).

| Configuration | Critical subtotal | High subtotal | Total weighted score |
|---|---:|---:|---:|
| dotnet-perf-skills | (4+4+4+5)×3 = 51 | (5+5+4+4)×2 = 36 | **87** |
| dotnet-perf-skills-improved | (5+5+5+2)×3 = 51 | (5+5+5+5)×2 = 40 | **91** |
| no-skills | (4+5+5+5)×3 = 57 | (4+3+3+4)×2 = 28 | **85** |

## What All Versions Get Right

- All three identify `new HttpClient()` per call as a critical production risk.
- All three detect per-call regex usage and recommend modernization toward `[GeneratedRegex]` or cached regexes.
- All three flag string concatenation loops and recommend `StringBuilder`.
- All three catch core dictionary/list inefficiencies (`ContainsKey`+indexer, `List.Contains` in looped paths).

## Summary: Impact of Skills

Most impactful differences, ranked:

1. **Coverage balance across dimensions:** `dotnet-perf-skills-improved` is strongest overall, but it underperforms on async breadth; `dotnet-perf-skills` and `no-skills` are much better on full async/IO anti-pattern coverage.
2. **Severity calibration quality:** `dotnet-perf-skills-improved` best distinguishes hot-path criticals from moderate hygiene issues.
3. **Structural/systematic framing:** skill-driven outputs (`dotnet-perf-skills`, `dotnet-perf-skills-improved`) provide broader structural accounting (unsealed totals, IEquatable counts, FrozenDictionary candidates) than `no-skills`.

Overall by weighted score: **1) dotnet-perf-skills-improved (91), 2) dotnet-perf-skills (87), 3) no-skills (85)**. The improved skill configuration delivers the highest-quality prioritization and remediation guidance, while the strongest async-depth signal appears in dotnet-perf-skills and no-skills outputs.
