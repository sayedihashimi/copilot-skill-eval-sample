# Aggregated Analysis: .NET Performance Analysis Skill Evaluation

**Runs:** 3 | **Configurations:** 2 | **Scenarios:** 1 | **Dimensions:** 9
**Date:** 2026-04-16 01:18 UTC

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
2. **Analyze** — An AI judge reviews the text output of all configurations side-by-side and scores each across 9 quality dimensions.

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
| MEDIUM | ×1 | 1 |

**Maximum possible weighted score: 105.0** (all dimensions scoring 5).
Scores shown as **mean ± standard deviation** across runs.

---

## Executive Summary

| Dimension [Tier] | no-skills | dotnet-perf-skills |
|---|---|---|
| Regex Anti-Pattern Detection [CRITICAL] | 4.3 ± 0.6 | 5.0 |
| String Allocation Detection [CRITICAL] | 4.3 ± 0.6 | 5.0 |
| Collection and LINQ Efficiency [CRITICAL] | 4.7 ± 0.6 | 4.7 ± 0.6 |
| Async and IO Pattern Detection [CRITICAL] | 4.7 ± 0.6 | 5.0 |
| Reflection and Serialization Overhead [HIGH] | 4.7 ± 0.6 | 4.7 ± 0.6 |
| Structural Optimization Detection [HIGH] | 4.0 | 5.0 |
| Severity Classification Accuracy [HIGH] | 3.7 ± 0.6 | 4.3 ± 1.2 |
| Fix Recommendation Quality [HIGH] | 4.0 | 5.0 |
| Token Efficiency [MEDIUM] | 5.0 | 1.7 ± 0.6 |

---

## Final Rankings

| Rank | Configuration | Mean Score | % of Max (105) | Std Dev | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | dotnet-perf-skills | 98.7 | 94% | 4.9 | 93.0 | 102.0 |
| 🥈 | no-skills | 91.7 | 87% | 8.1 | 83.0 | 99.0 |

---

## Weighted Score per Run

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 99.0 | 102.0 |
| 2 | 83.0 | 101.0 |
| 3 | 93.0 | 93.0 |
| **Mean** | **91.7** | **98.7** |

---

## Token Usage Summary

Average token consumption per configuration across all runs.

| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time | Δ Input vs Baseline |
|---|---|---|---|---|---|---|
| no-skills | 341,409 | 9,173 | 260,970 | 8 | 3m 44s | — (baseline) |
| dotnet-perf-skills | 786,422 | 14,622 | 620,613 | 13 | 5m 40s | +130.3% |

---

## Token Usage Per Run

| Configuration | Run | Scenario | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time | Note |
|---|---|---|---|---|---|---|---|---|
| no-skills | 1 | analyze-perf-issues | 350,441 | 8,629 | 248,945 | 8 | 3m 35s |  |
| no-skills | 2 | analyze-perf-issues | 338,082 | 9,637 | 268,038 | 8 | 3m 49s |  |
| no-skills | 3 | analyze-perf-issues | 335,703 | 9,254 | 265,928 | 8 | 3m 49s |  |
| dotnet-perf-skills | 1 | analyze-perf-issues | 747,327 | 12,819 | 586,010 | 13 | 5m 12s |  |
| dotnet-perf-skills | 2 | analyze-perf-issues | 895,479 | 18,486 | 671,367 | 14 | 6m 20s |  |
| dotnet-perf-skills | 3 | analyze-perf-issues | 716,461 | 12,560 | 604,461 | 13 | 5m 28s |  |


---

## Consistency Analysis

| Configuration | Score σ | Most Consistent Dim (σ) | Most Variable Dim (σ) |
|---|---|---|---|
| no-skills | 8.1 | Structural Optimization Detection (0.0) | Regex Anti-Pattern Detection (0.6) |
| dotnet-perf-skills | 4.9 | Regex Anti-Pattern Detection (0.0) | Severity Classification Accuracy (1.2) |

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

**dotnet-perf-skills — `output/dotnet-perf-skills/run-3/analyze-perf-issues/performance-analysis.md`**
> #### 2. `new Regex()` per log line in hot path (8 instances)  
> **Impact:** `LogAnalyzer.TryParseLine` creates 2–3 `new Regex` objects per line. For a 1M-line log, that's 2–3M regex compilations.  
> #### 8. 48 `RegexOptions.Compiled` without `[GeneratedRegex]` (48 instances)

**no-skills — `output/no-skills/run-3/analyze-perf-issues/performance-analysis.md`**
> 1. 🟡 **47 `RegexOptions.Compiled` static instances** (lines 13–59)  
> - Each `RegexOptions.Compiled` regex JIT-compiles at first use, consuming significant startup time and memory.  
> 1. 🔴 **`new Regex(...)` per log line** (lines 50, 65, 75)

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
dotnet-perf-skills is more complete and quantified (hot-path + startup-budget framing + explicit `[GeneratedRegex]` migration).

**Verdict:** **dotnet-perf-skills** is best for regex coverage depth and prioritization context.

### 2. String Allocation Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 4 | 5 |
| 3 | 4 | 5 |
| **Mean** | **4.3** | **5.0** |

#### Analysis

**dotnet-perf-skills — `.../performance-analysis.md`**
> #### 4. String `+=` concatenation in loops — O(n²) (11+ sites)  
> #### 9. `.ToLower()`/`.ToUpper()` without culture (25 instances)  
> #### 19. `.Replace()` chains in loops (SlugGenerator)

**no-skills — `.../performance-analysis.md`**
> ### 2. String Concatenation in Loops (6 files)  
> `TemplateEngine`, `LogAnalyzer`, `DataPipeline`, `NotificationService`, `CsvParser`, `ValidationEngine` all use `+=` string concatenation in loops.  
> ### 3. `.ToLower()` / `.ToUpper()` without Culture (7 files)

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
Both catch the required issues; dotnet-perf-skills is more concrete on counts and specific allocation chains (`SlugGenerator`, `MarkdownStripper`).

**Verdict:** **dotnet-perf-skills** is best on string-allocation specificity.

### 3. Collection and LINQ Efficiency [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 4 | 5 |
| 3 | 5 | 4 |
| **Mean** | **4.7** | **4.7** |

#### Analysis

**dotnet-perf-skills — `.../performance-analysis.md`**
> #### 16. Unnecessary `.ToList()` materializations (20 instances)  
> #### 17. `List.Contains` / `allKeys.Contains` — O(n) lookup (3 sites)  
> #### 5. `ContainsKey` + indexer double-lookup (18 instances)

**no-skills — `.../performance-analysis.md`**
> 5. 🟡 **`Skip(i).Take(5).ToList()` in sliding window** (line 157)  
> 5. ℹ️ **`.Distinct().ToList()` for tag dedup** (line 191)  
> 2. 🟡 **`Keys.ToList()` + `.Contains()` for key union in `Diff`** (lines 85–89)

**Score:** dotnet-perf-skills **4/5**; no-skills **5/5**.  
no-skills explicitly calls out the rubric’s sliding-window `Skip().Take().ToList()` issue and multiple concrete LINQ materialization anti-patterns.

**Verdict:** **no-skills** is best for collection/LINQ hot-path granularity.

### 4. Async and IO Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 4 | 5 |
| 3 | 5 | 5 |
| **Mean** | **4.7** | **5.0** |

#### Analysis

**dotnet-perf-skills — `.../performance-analysis.md`**
> #### 1. `new HttpClient()` per call — socket exhaustion (3 instances)  
> #### 7. Sequential awaits in loop — no parallelism (1 instance)  
> #### 13. Unbounded parallelism (1 instance)  
> #### 14. Missing `CancellationToken` on async methods

**no-skills — `.../performance-analysis.md`**
> 1. 🔴 **`new HttpClient()` per call — socket exhaustion** (lines 163, 179, 191)  
> 2. 🔴 **Sequential awaits in batch loop** (lines 117–122)  
> 3. 🔴 **Unbounded parallelism in `SendBatchParallelAsync`** (lines 130–133)  
> 5. 🟡 **`Task.Delay` without `CancellationToken`** (line 102)

**Score:** dotnet-perf-skills **5/5**; no-skills **5/5**.  
Both reports cover all required async/IO anti-patterns with concrete fixes (`IHttpClientFactory`, throttling, cancellation propagation).

**Verdict:** **Tie** — both are production-relevant and actionable.

### 5. Reflection and Serialization Overhead [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 4 | 5 |
| 3 | 5 | 4 |
| **Mean** | **4.7** | **4.7** |

#### Analysis

**dotnet-perf-skills — `.../performance-analysis.md`**
> #### 3. Uncached `new JsonSerializerOptions` per call (4 instances)  
> #### 15. Uncached reflection — `GetProperties()`/`SetValue()`/`GetValue()` per call (4 sites)  
> **Fix:** Cache `PropertyInfo[]` per type in a `ConcurrentDictionary<Type, PropertyInfo[]>`.

**no-skills — `.../performance-analysis.md`**
> 1. 🔴 **Uncached `GetProperties()` reflection per call** (lines 77, 114)  
> 2. 🟡 **Uncached `SetValue` / `GetValue` reflection** (lines 101, 119)  
> 6. ℹ️ **Full deserialization for `PrettyPrint`** (line 140)

**Score:** dotnet-perf-skills **4/5**; no-skills **5/5**.  
Both detect core reflection/serializer issues; no-skills adds explicit note on unnecessary full deserialization path optimization.

**Verdict:** **no-skills** is best on serialization-path completeness.

### 6. Structural Optimization Detection [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 4 | 5 |
| 3 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

**dotnet-perf-skills — `.../performance-analysis.md`**
> #### 10. Unsealed classes — 17 of 17 (0% sealed)  
> #### 11. Structs without `IEquatable<T>` (2 of 2 structs)  
> #### 12. `static readonly Dictionary<>` — FrozenDictionary candidates (2 instances)

**no-skills — `.../performance-analysis.md`**
> ### 5. Missing `IEquatable<T>` on Structs (2 files)  
> ### 6. Unsealed Classes (3 files)  
> 7. ℹ️ **Static `Dictionary` could be `FrozenDictionary`** (line 11)

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
dotnet-perf-skills provides stronger breadth and quantification, especially on sealing coverage across the whole codebase.

**Verdict:** **dotnet-perf-skills** is best for structural optimization detection.

### 7. Severity Classification Accuracy [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 3 | 5 |
| 3 | 4 | 3 |
| **Mean** | **3.7** | **4.3** |

#### Analysis

**dotnet-perf-skills — `.../performance-analysis.md`**
> | 🔴 Critical | 7 | `new HttpClient` per call ..., uncached `new Regex` in hot loops ... |  
> #### 5. `ContainsKey` + indexer double-lookup (18 instances)  
> **Impact:** ~2× slower per lookup ...

**no-skills — `.../performance-analysis.md`**
> | 🔴 Critical | 8 |  
> | 🟡 Moderate | 22 |  
> 4. 🟡 **`ContainsKey` + indexer instead of `TryGetValue`** ...  
> 1. 🔴 **`new HttpClient()` per call — socket exhaustion** ...

**Score:** dotnet-perf-skills **3/5**; no-skills **4/5**.  
Both prioritize true criticals, but dotnet-perf-skills appears to over-escalate `ContainsKey`+indexer as critical where moderate is more consistent with impact.

**Verdict:** **no-skills** is best on severity calibration.

### 8. Fix Recommendation Quality [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 4 | 5 |
| 3 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

**dotnet-perf-skills — `.../performance-analysis.md`**
> **Fix:** Inject `IHttpClientFactory` or use a `static readonly HttpClient` with `PooledConnectionLifetime`.  
> **Fix:** Hoist to `static readonly` fields, or use `[GeneratedRegex]` on .NET 7+.  
> **Fix:** Use `StringComparison.OrdinalIgnoreCase` for comparisons, or `ToLowerInvariant()`.

**no-skills — `.../performance-analysis.md`**
> **Fix:** Use `Task.WhenAll` with throttling ... `SemaphoreSlim`.  
> **Fix:** Use `TryGetValue` or `CollectionsMarshal.GetValueRefOrAddDefault`.  
> **Recommendation:** Adopt a project-wide policy of `[GeneratedRegex]` ... or `static readonly Regex` fields.

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
Both are actionable; dotnet-perf-skills is more consistently specific to safe, mainstream .NET APIs and keeps suggestions tightly prioritized.

**Verdict:** **dotnet-perf-skills** is best for practical remediation guidance.

### 9. Token Efficiency [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 2 |
| 2 | 5 | 1 |
| 3 | 5 | 2 |
| **Mean** | **5.0** | **1.7** |

---

## Asset Usage Summary

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| no-skills | 1 | 88bad97c…2c17 | claude-opus-4.6 | — | — | ✅ |
| no-skills | 2 | 6792eece…dfd1 | claude-opus-4.6 | — | — | ✅ |
| no-skills | 3 | eeffd00e…8c7c | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills | 1 | 58d615ab…ed01 | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills | 2 | 847d61a6…b949 | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills | 3 | bb15194f…5759 | claude-opus-4.6 | — | — | ✅ |

---

## Copilot Recommendations (Best Run)

Prioritized recommendations extracted from the highest-scoring run's output for each configuration.

### no-skills (run 1, score 99)

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

### dotnet-perf-skills (run 1, score 102)

## Prioritized Fix Recommendations

| # | Fix | Severity | Effort | Impact |
|---|-----|----------|--------|--------|
| 1 | Replace `new HttpClient()` with shared/injected instance | 🔴 | Quick-fix | Prevents socket exhaustion in production |
| 2 | Cache Regex instances in `LogAnalyzer.TryParseLine` | 🔴 | Quick-fix | >100x faster log parsing |
| 3 | Cache `JsonSerializerOptions` as static field | 🔴 | Quick-fix | Up to 592x faster serialization |
| 4 | Cache `Regex` in `ValidationEngine.AddPattern` | 🔴 | Quick-fix | Eliminates regex compilation per validation |
| 5 | Cache reflection `PropertyInfo[]` in `EntityMapper` | 🔴 | Moderate | ~100x faster property mapping |
| 6 | Use `HashSet` instead of `List.Contains` in `JsonTransformer.Diff` | 🔴 | Quick-fix | O(n) → O(1) per key lookup |
| 7 | Convert 48 `RegexOptions.Compiled` to `[GeneratedRegex]` | 🟡 | Moderate | Eliminates JIT startup cost, better throughput |
| 8 | Replace `+=` string concat with `StringBuilder` (9 methods) | 🟡 | Moderate | Eliminates O(n²) allocation in all output methods |
| 9 | Replace `.ToLower()` with `StringComparer.OrdinalIgnoreCase` | 🟡 | Moderate | 2-3x faster comparisons, fixes Turkish-I bug |
| 10 | Replace `ContainsKey`+indexer with `TryGetValue` (12 sites) | 🟡 | Quick-fix | ~2x faster dictionary lookups |

> ⚠️ **Disclaimer:** These results are generated by an AI assistant and are non-deterministic. Findings may include false positives, miss real issues, or suggest changes that are incorrect for your specific context. Always verify recommendations with benchmarks and human review before applying changes to production code.

---

## Raw Data References

- Per-run analysis: `reports/analysis-run-1.md`
- Per-run analysis: `reports/analysis-run-2.md`
- Per-run analysis: `reports/analysis-run-3.md`
- Score data: `reports/scores-data.json`
- Generation usage: `reports/generation-usage.json`
