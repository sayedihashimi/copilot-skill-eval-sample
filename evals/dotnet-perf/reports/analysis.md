# Aggregated Analysis: .NET Performance Analysis Skill Evaluation

**Runs:** 5 | **Configurations:** 2 | **Scenarios:** 1 | **Dimensions:** 14
**Date:** 2026-04-10 19:39 UTC

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
2. **Analyze** — An AI judge reviews the text output of all configurations side-by-side and scores each across 14 quality dimensions.

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
| Regex Anti-Pattern Detection [CRITICAL] | 4.4 ± 0.5 | 5.0 |
| String Allocation Detection [CRITICAL] | 4.6 ± 0.5 | 5.0 |
| Collection and LINQ Efficiency [CRITICAL] | 4.6 ± 0.5 | 5.0 |
| Async and IO Pattern Detection [CRITICAL] | 4.6 ± 0.5 | 5.0 |
| Reflection and Serialization Overhead [HIGH] | 4.2 ± 0.4 | 4.6 ± 0.5 |
| Structural Optimization Detection [HIGH] | 3.6 ± 0.5 | 5.0 |
| Severity Classification Accuracy [HIGH] | 3.4 ± 0.5 | 4.2 ± 1.1 |
| Fix Recommendation Quality [HIGH] | 4.0 | 4.8 ± 0.4 |
| Coverage Breadth and Granularity [MEDIUM] | 5.0 | 4.0 |
| Token Efficiency [MEDIUM] | 4.5 ± 0.6 | 2.0 |
| Evidence & Quantification Rigor [MEDIUM] | 4.0 | 5.0 |
| Signal-to-Noise / Report Focus [MEDIUM] | 2.0 | 5.0 |
| Signal-to-Noise / Prioritization Focus [MEDIUM] | 3.0 | 5.0 |
| Evidence Quantification and Scan Rigor [MEDIUM] | 3.0 | 5.0 |

---

## Final Rankings

| Rank | Configuration | Mean Score | % of Max (100) | Std Dev | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | dotnet-perf-skills | 103.6 | 104% | 3.0 | 100.0 | 107.0 |
| 🥈 | no-skills | 92.0 | 92% | 6.3 | 84.0 | 99.0 |

---

## Weighted Score per Run

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 99.0 | 100.0 |
| 2 | 87.0 | 101.0 |
| 3 | 94.0 | 105.0 |
| 4 | 96.0 | 107.0 |
| 5 | 84.0 | 105.0 |
| **Mean** | **92.0** | **103.6** |

---

## Token Usage Summary

Average token consumption per configuration (2 outlier run(s) excluded from averages).

| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time | Δ Input vs Baseline |
|---|---|---|---|---|---|---|
| no-skills | 397,612 | 10,188 | 316,655 | 9 | 4m 0s | — (baseline) |
| dotnet-perf-skills | 901,244 | 14,622 | 672,806 | 13 | 4m 54s | +126.7% |

---

## Token Usage Per Run

| Configuration | Run | Scenario | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time | Note |
|---|---|---|---|---|---|---|---|---|
| no-skills | 1 | analyze-perf-issues | 407,279 | 9,754 | 304,911 | 9 | 3m 55s |  |
| no-skills | 2 | analyze-perf-issues | 394,687 | 9,815 | 319,631 | 9 | 4m 8s |  |
| no-skills | 3 | analyze-perf-issues | 390,326 | 10,300 | 320,025 | 9 | 4m 8s |  |
| no-skills | 4 | analyze-perf-issues | 398,155 | 10,884 | 322,054 | 9 | 3m 50s |  |
| no-skills | 5 | analyze-perf-issues | 6,533,468 | 63,406 | 6,376,159 | 77 | 4m 25s | ⚠️ outlier |
| dotnet-perf-skills | 1 | analyze-perf-issues | 2,232,672 | 16,277 | 1,965,336 | 66 | 5m 14s | ⚠️ outlier |
| dotnet-perf-skills | 2 | analyze-perf-issues | 818,873 | 14,394 | 593,722 | 12 | 4m 42s |  |
| dotnet-perf-skills | 3 | analyze-perf-issues | 934,185 | 14,811 | 710,699 | 13 | 4m 59s |  |
| dotnet-perf-skills | 4 | analyze-perf-issues | 924,344 | 13,907 | 692,262 | 13 | 4m 42s |  |
| dotnet-perf-skills | 5 | analyze-perf-issues | 927,572 | 15,377 | 694,541 | 13 | 5m 14s |  |


### ⚠️ Token Usage Outliers

The following runs were detected as outliers using the Modified Z-score (MAD) method. They are excluded from averages and Token Efficiency scores.

| Configuration | Run | Total Tokens | Details |
|---|---|---|---|
| no-skills | 5 | 6,596,874 | 11 turns, 23 tool calls; ~3× more tool calls than typical |
| dotnet-perf-skills | 1 | 2,248,949 | 12 turns, 59 tool calls; 1 skill invocations; ~5× more tool calls than typical |

---

## Consistency Analysis

| Configuration | Score σ | Most Consistent Dim (σ) | Most Variable Dim (σ) |
|---|---|---|---|
| no-skills | 6.3 | Fix Recommendation Quality (0.0) | Token Efficiency (0.6) |
| dotnet-perf-skills | 3.0 | Regex Anti-Pattern Detection (0.0) | Severity Classification Accuracy (1.1) |

---

## Per-Dimension Analysis

### 1. Regex Anti-Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 4 | 5 |
| 3 | 4 | 5 |
| 4 | 5 | 5 |
| 5 | 4 | 5 |
| **Mean** | **4.4** | **5.0** |

#### Analysis

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

### 2. String Allocation Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 4 | 5 |
| 3 | 5 | 5 |
| 4 | 5 | 5 |
| 5 | 4 | 5 |
| **Mean** | **4.6** | **5.0** |

#### Analysis

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

### 3. Collection and LINQ Efficiency [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 4 | 5 |
| 3 | 5 | 5 |
| 4 | 5 | 5 |
| 5 | 4 | 5 |
| **Mean** | **4.6** | **5.0** |

#### Analysis

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

### 4. Async and IO Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 4 | 5 |
| 3 | 5 | 5 |
| 4 | 4 | 5 |
| 5 | 5 | 5 |
| **Mean** | **4.6** | **5.0** |

#### Analysis

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

### 5. Reflection and Serialization Overhead [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 4 | 4 |
| 3 | 4 | 4 |
| 4 | 5 | 5 |
| 5 | 4 | 5 |
| **Mean** | **4.2** | **4.6** |

#### Analysis

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

### 6. Structural Optimization Detection [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 3 | 5 |
| 2 | 3 | 5 |
| 3 | 4 | 5 |
| 4 | 4 | 5 |
| 5 | 4 | 5 |
| **Mean** | **3.6** | **5.0** |

#### Analysis

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

### 7. Severity Classification Accuracy [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 3 |
| 2 | 4 | 3 |
| 3 | 3 | 5 |
| 4 | 3 | 5 |
| 5 | 3 | 5 |
| **Mean** | **3.4** | **4.2** |

#### Analysis

Both differentiate severity tiers, but baseline prioritization is slightly more impact-aligned for hot-path criticals vs broad moderate issues.

**dotnet-perf-skills excerpt:**
> | 🔴 Critical | 6 | `new HttpClient()`... uncached `new Regex()`... uncached `JsonSerializerOptions` |  
> | 5 | Replace 15 `ContainsKey` + indexer with `TryGetValue` | 🔴 Critical | ... |

**no-skills excerpt:**
> | 🔴 Critical | 7 | Socket exhaustion, O(n²) hot-path patterns, per-line regex instantiation |  
> | 1 | Cache regex as `static readonly` fields in `LogAnalyzer.TryParseLine` ... **Estimated 100x+ improvement** |

**Score:** dotnet-perf-skills **3/5**, no-skills **4/5**.  
**Verdict:** **no-skills wins narrowly**; `dotnet-perf-skills` appears to over-escalate some lookup optimizations as critical relative to true top-tier hot-path risks.

### 8. Fix Recommendation Quality [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 4 | 5 |
| 3 | 4 | 5 |
| 4 | 4 | 5 |
| 5 | 4 | 4 |
| **Mean** | **4.0** | **4.8** |

#### Analysis

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

### 9. Coverage Breadth and Granularity [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 4 |
| 2 | — | — |
| 3 | — | — |
| 4 | — | — |
| 5 | — | — |
| **Mean** | **5.0** | **4.0** |

#### Analysis

The baseline report is broader and more granular (48 findings, file-by-file), while skill-enabled output is tighter and more curated.

**dotnet-perf-skills excerpt:**
> **Total issues found: 25 findings** across 10 files...  
> Top 3 priorities... 10-item prioritized recommendations.

**no-skills excerpt:**
> ...reveals **48 performance issues** across 10 source files spanning 7 categories.  
> **Findings by File** ... full per-file tables and fixes.

**Score:** dotnet-perf-skills **4/5**, no-skills **5/5**.  
**Verdict:** **no-skills wins** on raw breadth and per-file granularity.

### 10. Token Efficiency [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | — |
| 2 | 5 | 2 |
| 3 | 5 | 2 |
| 4 | 4 | 2 |
| 5 | — | 2 |
| **Mean** | **4.5** | **2.0** |

### 11. Evidence & Quantification Rigor [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | — | — |
| 2 | 4 | 5 |
| 3 | — | — |
| 4 | — | — |
| 5 | — | — |
| **Mean** | **4.0** | **5.0** |

### 12. Signal-to-Noise / Report Focus [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | — | — |
| 2 | — | — |
| 3 | 2 | 5 |
| 4 | — | — |
| 5 | — | — |
| **Mean** | **2.0** | **5.0** |

### 13. Signal-to-Noise / Prioritization Focus [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | — | — |
| 2 | — | — |
| 3 | — | — |
| 4 | 3 | 5 |
| 5 | — | — |
| **Mean** | **3.0** | **5.0** |

### 14. Evidence Quantification and Scan Rigor [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | — | — |
| 2 | — | — |
| 3 | — | — |
| 4 | — | — |
| 5 | 3 | 5 |
| **Mean** | **3.0** | **5.0** |

---

## Asset Usage Summary

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| no-skills | 1 | 80f7ffa2…164c | claude-opus-4.6 | — | — | ✅ |
| no-skills | 2 | 5773ed00…88bf | claude-opus-4.6 | — | — | ✅ |
| no-skills | 3 | 168afc70…d899 | claude-opus-4.6 | — | — | ✅ |
| no-skills | 4 | 580d7962…3259 | claude-opus-4.6 | — | — | ✅ |
| no-skills | 5 | cb9ce225…b9b8 | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills | 1 | f0fc77a1…6a50 | claude-opus-4.6 | analyzing-dotnet-performance | dotnet-diag | ✅ |
| dotnet-perf-skills | 2 | cf35d49f…7d29 | claude-opus-4.6 | analyzing-dotnet-performance | dotnet-diag | ✅ |
| dotnet-perf-skills | 3 | 885f9fb8…12e2 | claude-opus-4.6 | analyzing-dotnet-performance | dotnet-diag | ✅ |
| dotnet-perf-skills | 4 | 1f82eea2…eb3f | claude-opus-4.6 | analyzing-dotnet-performance | dotnet-diag | ✅ |
| dotnet-perf-skills | 5 | 13176b77…1664 | claude-opus-4.6 | analyzing-dotnet-performance | dotnet-diag | ✅ |

---

## Raw Data References

- Per-run analysis: `reports/analysis-run-1.md`
- Per-run analysis: `reports/analysis-run-2.md`
- Per-run analysis: `reports/analysis-run-3.md`
- Per-run analysis: `reports/analysis-run-4.md`
- Per-run analysis: `reports/analysis-run-5.md`
- Verification data: `reports/verification-data.json`
- Score data: `reports/scores-data.json`
- Build notes: `reports/build-notes.md`
- Generation usage: `reports/generation-usage.json`
