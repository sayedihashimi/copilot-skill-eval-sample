# Aggregated Analysis: .NET Performance Analysis Skill Evaluation

**Runs:** 3 | **Configurations:** 3 | **Scenarios:** 1 | **Dimensions:** 8
**Date:** 2026-04-16 02:58 UTC

---

## Overview

Evaluate how the dotnet/skills performance-related skills (analyzing-dotnet-performance, microbenchmarking, dotnet-trace-collect) improve Copilot's ability to detect performance anti-patterns in existing .NET code compared to baseline Copilot.

---

## What Was Tested

### Scenarios

Each run evaluates one of the following scenarios (randomly selected per run):

| Scenario | Description |
|---|---|
| analyze-perf-issues | Analyze a .NET class library with known performance anti-patterns across regex, string, collections, async, reflection, and structural categories |

### Configurations

Each configuration gives Copilot different custom skills or plugins. The **no-skills** baseline uses default Copilot with no custom instructions.

| Configuration | Description | Skills | Plugins |
|---|---|---|---|
| no-skills | Baseline (default Copilot) | — | — |
| dotnet-perf-skills | dotnet/skills Performance Skills | — | dotnet-skills:dotnet-diag |
| dotnet-perf-skills-improved | dotnet/skills Performance Skills (improved) | — | dotnet-skills-improved:dotnet-diag |

### How It Works

1. **Generate** — For each configuration, Copilot CLI (`copilot --yolo`) is given a scenario prompt and produces text output. One scenario is randomly selected per run.
2. **Analyze** — An AI judge reviews the text output of all configurations side-by-side and scores each across 8 quality dimensions.

Generation model: **claude-opus-4.6**
Analysis model: **gpt-5.3-codex**

---

## Scoring Methodology

Each dimension is scored on a **1–5 scale**:

| Score | Meaning |
|:---:|---|
| 5 | Excellent — comprehensive and well-structured |
| 4 | Good — thorough with minor gaps |
| 3 | Acceptable — covers the basics |
| 2 | Below average — significant gaps |
| 1 | Poor — missing or fundamentally wrong |

Dimensions are grouped into **tiers** that determine their weight in the final weighted score:

| Tier | Weight | Dimensions |
|---|:---:|:---:|
| CRITICAL | ×3 | 4 |
| HIGH | ×2 | 4 |

**Maximum possible weighted score: 100.0** (all dimensions scoring 5).
Scores shown as **mean ± standard deviation** across runs.

---

## Executive Summary

| Dimension [Tier] | no-skills | dotnet-perf-skills | dotnet-perf-skills-improved |
|---|---|---|---|
| Regex Anti-Pattern Detection [CRITICAL] | 4.0 | 4.3 ± 0.6 | 5.0 |
| String Allocation Detection [CRITICAL] | 4.7 ± 0.6 | 4.7 ± 0.6 | 4.7 ± 0.6 |
| Collection and LINQ Efficiency [CRITICAL] | 4.3 ± 0.6 | 4.3 ± 0.6 | 5.0 |
| Async and IO Pattern Detection [CRITICAL] | 4.7 ± 0.6 | 5.0 | 3.7 ± 1.5 |
| Reflection and Serialization Overhead [HIGH] | 4.0 ± 1.0 | 4.7 ± 0.6 | 4.3 ± 0.6 |
| Structural Optimization Detection [HIGH] | 3.3 ± 0.6 | 5.0 | 5.0 |
| Severity Classification Accuracy [HIGH] | 2.7 ± 0.6 | 3.7 ± 0.6 | 5.0 |
| Fix Recommendation Quality [HIGH] | 3.3 ± 0.6 | 4.0 | 5.0 |

---

## Final Rankings

| Rank | Configuration | Mean Score | % of Max (100) | Std Dev | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | dotnet-perf-skills-improved | 93.7 | 94% | 2.3 | 91.0 | 95.0 |
| 🥈 | dotnet-perf-skills | 89.7 | 90% | 2.3 | 87.0 | 91.0 |
| 🥉 | no-skills | 79.7 | 80% | 5.5 | 74.0 | 85.0 |

---

## Weighted Score per Run

| Run | no-skills | dotnet-perf-skills | dotnet-perf-skills-improved |
|---|---|---|---|
| 1 | 85.0 | 87.0 | 91.0 |
| 2 | 74.0 | 91.0 | 95.0 |
| 3 | 80.0 | 91.0 | 95.0 |
| **Mean** | **79.7** | **89.7** | **93.7** |

---

## Token Usage Summary

Average token consumption per configuration across all runs.

| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time |
|---|---|---|---|---|---|
| dotnet-perf-skills-improved | 752,362 | 12,153 | 514,295 | 11 | 4m 32s |

---

## Token Usage Per Run

| Configuration | Run | Scenario | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time | Note |
|---|---|---|---|---|---|---|---|---|
| dotnet-perf-skills-improved | 1 | analyze-perf-issues | 750,358 | 12,283 | 478,972 | 11 | 4m 39s |  |
| dotnet-perf-skills-improved | 2 | analyze-perf-issues | 857,230 | 11,818 | 631,876 | 12 | 4m 20s |  |
| dotnet-perf-skills-improved | 3 | analyze-perf-issues | 649,499 | 12,359 | 432,038 | 10 | 4m 37s |  |


---

## Consistency Analysis

| Configuration | Score σ | Most Consistent Dim (σ) | Most Variable Dim (σ) |
|---|---|---|---|
| no-skills | 5.5 | Regex Anti-Pattern Detection (0.0) | Reflection and Serialization Overhead (1.0) |
| dotnet-perf-skills | 2.3 | Async and IO Pattern Detection (0.0) | Regex Anti-Pattern Detection (0.6) |
| dotnet-perf-skills-improved | 2.3 | Regex Anti-Pattern Detection (0.0) | Async and IO Pattern Detection (1.5) |

---

## Per-Dimension Analysis

### 1. Regex Anti-Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills | dotnet-perf-skills-improved |
|---|---|---|---|
| 1 | 4 | 4 | 5 |
| 2 | 4 | 5 | 5 |
| 3 | 4 | 4 | 5 |
| **Mean** | **4.0** | **4.3** | **5.0** |

#### Analysis

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

### 2. String Allocation Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills | dotnet-perf-skills-improved |
|---|---|---|---|
| 1 | 5 | 4 | 5 |
| 2 | 4 | 5 | 5 |
| 3 | 5 | 5 | 4 |
| **Mean** | **4.7** | **4.7** | **4.7** |

#### Analysis

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

### 3. Collection and LINQ Efficiency [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills | dotnet-perf-skills-improved |
|---|---|---|---|
| 1 | 5 | 4 | 5 |
| 2 | 4 | 4 | 5 |
| 3 | 4 | 5 | 5 |
| **Mean** | **4.3** | **4.3** | **5.0** |

#### Analysis

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

### 4. Async and IO Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills | dotnet-perf-skills-improved |
|---|---|---|---|
| 1 | 5 | 5 | 2 |
| 2 | 4 | 5 | 4 |
| 3 | 5 | 5 | 5 |
| **Mean** | **4.7** | **5.0** | **3.7** |

#### Analysis

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

### 5. Reflection and Serialization Overhead [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills | dotnet-perf-skills-improved |
|---|---|---|---|
| 1 | 4 | 5 | 5 |
| 2 | 3 | 4 | 4 |
| 3 | 5 | 5 | 4 |
| **Mean** | **4.0** | **4.7** | **4.3** |

#### Analysis

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

### 6. Structural Optimization Detection [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills | dotnet-perf-skills-improved |
|---|---|---|---|
| 1 | 3 | 5 | 5 |
| 2 | 4 | 5 | 5 |
| 3 | 3 | 5 | 5 |
| **Mean** | **3.3** | **5.0** | **5.0** |

#### Analysis

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

### 7. Severity Classification Accuracy [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills | dotnet-perf-skills-improved |
|---|---|---|---|
| 1 | 3 | 4 | 5 |
| 2 | 3 | 4 | 5 |
| 3 | 2 | 3 | 5 |
| **Mean** | **2.7** | **3.7** | **5.0** |

#### Analysis

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

### 8. Fix Recommendation Quality [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills | dotnet-perf-skills-improved |
|---|---|---|---|
| 1 | 4 | 4 | 5 |
| 2 | 3 | 4 | 5 |
| 3 | 3 | 4 | 5 |
| **Mean** | **3.3** | **4.0** | **5.0** |

#### Analysis

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

---

## Asset Usage Summary

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| dotnet-perf-skills-improved | 1 | 82cd5bf6…804e | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills-improved | 2 | 7639371d…f02c | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills-improved | 3 | d71e0550…5fdf | claude-opus-4.6 | — | — | ✅ |

---

## Copilot Recommendations (Best Run)

Prioritized recommendations extracted from the highest-scoring run's output for each configuration.

### no-skills (run 1, score 85)

## Prioritized Fix Recommendations

| Rank | Fix | Files | Severity | Effort |
|------|-----|-------|----------|--------|
| 1 | Replace `new HttpClient()` with `IHttpClientFactory` or shared instance | NotificationService | 🔴 Critical | Quick-fix |
| 2 | Promote per-line `new Regex()` to static fields / `[GeneratedRegex]` | LogAnalyzer | 🔴 Critical | Quick-fix |
| 3 | Replace char-by-char `+=` with `StringBuilder` | CsvParser | 🔴 Critical | Moderate |
| 4 | Pre-compile regex at rule-creation time | ValidationEngine | 🔴 Critical | Quick-fix |
| 5 | Cache `GetProperties()` / use compiled delegates for reflection | EntityMapper | 🔴 Critical | Moderate |
| 6 | Promote all per-call `Regex` to static / `[GeneratedRegex]` | TemplateEngine, SlugGenerator | 🔴 Critical | Quick-fix |
| 7 | Add `SemaphoreSlim` throttling to `SendBatchParallelAsync` | NotificationService | 🔴 Critical | Quick-fix |
| 8 | Replace string `+=` in loops with `StringBuilder` | All files | 🟡 Moderate | Moderate |
| 9 | Cache `JsonSerializerOptions` as static readonly | JsonTransformer, NotificationService | 🟡 Moderate | Quick-fix |
| 10 | Replace `List.Contains()` with `HashSet` lookups | SlugGenerator, JsonTransformer | 🟡 Moderate | Quick-fix |

### dotnet-perf-skills (run 2, score 91)

## Prioritized Fix Recommendations

| Rank | Finding | Severity | Effort | Impact |
|------|---------|----------|--------|--------|
| 1 | Reuse HttpClient (socket exhaustion) | 🔴 | Quick-fix | Prevents production incidents |
| 2 | Cache Regex in LogAnalyzer hot loop | 🔴 | Quick-fix | >10x parsing speedup |
| 3 | Cache JsonSerializerOptions | 🔴 | Quick-fix | Up to 592x per-call improvement |
| 4 | Convert 48 Compiled regex → `[GeneratedRegex]` | 🔴 | Moderate | Near-zero startup, better throughput |
| 5 | Replace `+=` string loops with StringBuilder | 🔴 | Moderate | Eliminates O(n²) allocations in 7 files |
| 6 | Cache reflection (GetProperties/SetValue) | 🔴 | Moderate | 10-100x faster entity mapping |
| 7 | Cache Regex in ValidationEngine | 🔴 | Quick-fix | Eliminates per-validation regex construction |
| 8 | Replace `.ToLower()` with StringComparison | 🟡 | Moderate | 2-3x faster comparisons, correct i18n |
| 9 | Replace ContainsKey+indexer with TryGetValue | 🟡 | Quick-fix | ~2x faster per lookup |
| 10 | Seal all 17 classes | 🟡 | Quick-fix | Enables JIT devirtualization |

| Severity | Count | Top Issue |
|----------|-------|-----------|
| 🔴 Critical | 7 | `new HttpClient()` per call (socket exhaustion) |
| 🟡 Moderate | 21 | `.ToLower()` without culture (25 instances) |
| ℹ️ Info | 10 | `params` array allocation, minor boxing |

> ⚠️ **Disclaimer:** These results are generated by an AI assistant and are non-deterministic. Findings may include false positives, miss real issues, or suggest changes that are incorrect for your specific context. Always verify recommendations with benchmarks and human review before applying changes to production code.

### dotnet-perf-skills-improved (run 2, score 95)

## Prioritized Fix Recommendations

| # | Fix | Impact | Effort | Files |
|---|-----|--------|--------|-------|
| 1 | Replace `new HttpClient()` with shared/injected instance | 🔴 Socket exhaustion prevention | Quick-fix | 1 file |
| 2 | Hoist `new Regex()` to static fields / `[GeneratedRegex]` | 🔴 >10x throughput in LogAnalyzer | Moderate | 4 files |
| 3 | Cache `JsonSerializerOptions` as static readonly | 🔴 Up to 592x faster serialization | Quick-fix | 1 file |
| 4 | Replace `+=` string loops with `StringBuilder` | 🔴 O(n²) → O(n) allocation | Moderate | 7 files |
| 5 | Convert 48 `Compiled` regexes to `[GeneratedRegex]` | 🔴 Near-zero startup cost | Moderate | 1 file |
| 6 | Replace `.ToLower()`/`.ToUpper()` with ordinal/invariant | 🟡 Eliminates 17 allocations + Turkish-I bug | Quick-fix | 6 files |
| 7 | Cache reflection `GetProperties()` per type | 🟡 ~100x faster mapping | Moderate | 2 files |
| 8 | Replace `ContainsKey`+indexer with `TryGetValue` | 🟡 ~2x per lookup | Quick-fix | 6 files |
| 9 | Use `HashSet` instead of `List.Contains` | 🟡 O(n) → O(1) lookups | Quick-fix | 2 files |
| 10 | Implement `IEquatable<T>` on structs | 🟡 Eliminates reflection equality | Quick-fix | 2 files |

| Severity | Count | Top Issue |
|----------|-------|-----------|
| 🔴 Critical | 5 | `new HttpClient()` per call — socket exhaustion |
| 🟡 Moderate | 8 | `.ToLower()` without culture (17 instances across 6 files) |
| ℹ️ Info | 5 | 17/17 classes unsealed |

> ⚠️ **Disclaimer:** These results are generated by an AI assistant and are non-deterministic. Findings may include false positives, miss real issues, or suggest changes that are incorrect for your specific context. Always verify recommendations with benchmarks and human review before applying changes to production code.

---

## Raw Data References

- Per-run analysis: `reports/analysis-run-1.md`
- Per-run analysis: `reports/analysis-run-2.md`
- Per-run analysis: `reports/analysis-run-3.md`
- Score data: `reports/scores-data.json`
- Generation usage: `reports/generation-usage.json`
