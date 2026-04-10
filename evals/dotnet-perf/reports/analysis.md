# Aggregated Analysis: .NET Performance Analysis Skill Evaluation

**Runs:** 3 | **Configurations:** 2 | **Scenarios:** 1 | **Dimensions:** 11
**Date:** 2026-04-10 07:44 UTC

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

### How It Works

1. **Generate** — For each configuration, Copilot CLI (`copilot --yolo`) is given a scenario prompt and produces text output. One scenario is randomly selected per run.
2. **Analyze** — An AI judge reviews the text output of all configurations side-by-side and scores each across 11 quality dimensions.

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

| Dimension [Tier] | no-skills | dotnet-perf-skills |
|---|---|---|
| Regex Anti-Pattern Detection [CRITICAL] | 4.3 ± 0.6 | 5.0 |
| String Allocation Detection [CRITICAL] | 4.3 ± 0.6 | 5.0 |
| Collection and LINQ Efficiency [CRITICAL] | 4.3 ± 0.6 | 5.0 |
| Async and IO Pattern Detection [CRITICAL] | 4.3 ± 0.6 | 5.0 |
| Reflection and Serialization Overhead [HIGH] | 4.3 ± 0.6 | 5.0 |
| Structural Optimization Detection [HIGH] | 3.7 ± 1.2 | 5.0 |
| Severity Classification Accuracy [HIGH] | 3.7 ± 0.6 | 3.7 ± 0.6 |
| Fix Recommendation Quality [HIGH] | 4.0 | 4.7 ± 0.6 |
| Configuration Attribution Consistency [MEDIUM] | 1.0 | 5.0 |
| Quantification & Traceability [MEDIUM] | 3.0 | 5.0 |
| Evidence Quantification and Traceability [MEDIUM] | 4.0 | 5.0 |

---

## Final Rankings

| Rank | Configuration | Mean Score | % of Max (100) | Std Dev | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | dotnet-perf-skills | 101.7 | 102% | 1.2 | 101.0 | 103.0 |
| 🥈 | no-skills | 86.0 | 86% | 9.6 | 79.0 | 97.0 |

---

## Weighted Score per Run

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 97.0 | 101.0 |
| 2 | 79.0 | 103.0 |
| 3 | 82.0 | 101.0 |
| **Mean** | **86.0** | **101.7** |

---

## Token Usage Summary

Average token consumption per configuration across all runs.

| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time |
|---|---|---|---|---|---|
| dotnet-perf-skills | 939,739 | 15,059 | 693,302 | 13 | 5m 17s |

---

## Token Usage Per Run

| Configuration | Run | Scenario | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time |
|---|---|---|---|---|---|---|---|
| dotnet-perf-skills | 1 | analyze-perf-issues | 997,440 | 14,470 | 726,205 | 14 | 4m 59s |
| dotnet-perf-skills | 2 | analyze-perf-issues | 907,514 | 15,037 | 679,165 | 13 | 5m 17s |
| dotnet-perf-skills | 3 | analyze-perf-issues | 994,664 | 14,693 | 739,185 | 14 | 5m 15s |
| dotnet-perf-skills | 4 | analyze-perf-issues | 881,089 | 14,891 | 647,381 | 13 | 5m 18s |
| dotnet-perf-skills | 5 | analyze-perf-issues | 917,989 | 16,203 | 674,574 | 12 | 5m 34s |

---

## Consistency Analysis

| Configuration | Score σ | Most Consistent Dim (σ) | Most Variable Dim (σ) |
|---|---|---|---|
| no-skills | 9.6 | Fix Recommendation Quality (0.0) | Structural Optimization Detection (1.2) |
| dotnet-perf-skills | 1.2 | Regex Anti-Pattern Detection (0.0) | Severity Classification Accuracy (0.6) |

---

## Per-Dimension Analysis

### 1. Regex Anti-Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 4 | 5 |
| 3 | 4 | 5 |
| **Mean** | **4.3** | **5.0** |

#### Analysis

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

### 2. String Allocation Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 4 | 5 |
| 3 | 4 | 5 |
| **Mean** | **4.3** | **5.0** |

#### Analysis

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

### 3. Collection and LINQ Efficiency [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 4 | 5 |
| 3 | 4 | 5 |
| **Mean** | **4.3** | **5.0** |

#### Analysis

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

### 4. Async and IO Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 4 | 5 |
| 3 | 4 | 5 |
| **Mean** | **4.3** | **5.0** |

#### Analysis

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

### 5. Reflection and Serialization Overhead [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 4 | 5 |
| 3 | 4 | 5 |
| **Mean** | **4.3** | **5.0** |

#### Analysis

**dotnet-perf-skills**
> #### 2. Uncached `new JsonSerializerOptions` per call (5 instances)  
> **Impact:** Up to 592× slower than cached options...

> #### 14. Uncached reflection — `GetProperties()`/`SetValue()`/`GetValue()` per call (5 instances)

**no-skills**
> **`new JsonSerializerOptions` on every call.** ... each allocate a new `JsonSerializerOptions { WriteIndented = true }`.

> **Uncached `GetProperties()` / `SetValue()` / `GetValue()` in `MapTo<T>()` and `MapFrom<T>()`**...

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best on quantitative impact framing.

### 6. Structural Optimization Detection [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 3 | 5 |
| 3 | 3 | 5 |
| **Mean** | **3.7** | **5.0** |

#### Analysis

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

### 7. Severity Classification Accuracy [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 4 |
| 2 | 3 | 4 |
| 3 | 4 | 3 |
| **Mean** | **3.7** | **3.7** |

#### Analysis

**dotnet-perf-skills**
> | 🔴 Critical | 8 | ... `new HttpClient()` ... `new Regex()` ... |  
> #### 7. ContainsKey + indexer double-lookup pattern (15 instances)

**no-skills**
> | 29 | 🔴 Critical | ... `new HttpClient()` per call ... |  
> | 50 | ℹ️ Info | ... `ContainsKey` + indexer ... |

**Score:** dotnet-perf-skills **3/5** (good on major hazards, but over-elevates some medium-impact patterns); no-skills **4/5** (better hot-path vs moderate/info separation).  
**Verdict:** **no-skills** is best for prioritization calibration.

### 8. Fix Recommendation Quality [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 4 |
| 2 | 4 | 5 |
| 3 | 4 | 5 |
| **Mean** | **4.0** | **4.7** |

#### Analysis

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

### 9. Configuration Attribution Consistency [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 1 | 5 |
| 2 | — | — |
| 3 | — | — |
| **Mean** | **1.0** | **5.0** |

### 10. Quantification & Traceability [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | — | — |
| 2 | 3 | 5 |
| 3 | — | — |
| **Mean** | **3.0** | **5.0** |

### 11. Evidence Quantification and Traceability [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | — | — |
| 2 | — | — |
| 3 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

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

---

## Asset Usage Summary

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| dotnet-perf-skills | 1 | 3a3259b0…ae66 | claude-opus-4.6 | analyzing-dotnet-performance | dotnet-diag | ✅ |
| dotnet-perf-skills | 2 | 07e988e3…ef0e | claude-opus-4.6 | analyzing-dotnet-performance | dotnet-diag | ✅ |
| dotnet-perf-skills | 3 | e6866bda…29e7 | claude-opus-4.6 | analyzing-dotnet-performance | dotnet-diag | ✅ |
| dotnet-perf-skills | 4 | 141b77e4…8f59 | claude-opus-4.6 | analyzing-dotnet-performance | dotnet-diag | ✅ |
| dotnet-perf-skills | 5 | d7947c08…ffc4 | claude-opus-4.6 | analyzing-dotnet-performance | dotnet-diag | ✅ |

---

## Raw Data References

- Per-run analysis: `reports/analysis-run-1.md`
- Per-run analysis: `reports/analysis-run-2.md`
- Per-run analysis: `reports/analysis-run-3.md`
- Verification data: `reports/verification-data.json`
- Score data: `reports/scores-data.json`
- Build notes: `reports/build-notes.md`
- Generation usage: `reports/generation-usage.json`
