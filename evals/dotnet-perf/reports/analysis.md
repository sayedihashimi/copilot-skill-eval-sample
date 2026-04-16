# Aggregated Analysis: .NET Performance Analysis Skill Evaluation

**Runs:** 3 | **Configurations:** 3 | **Scenarios:** 1 | **Dimensions:** 9
**Date:** 2026-04-16 04:03 UTC

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

| Dimension [Tier] | no-skills | dotnet-perf-skills | dotnet-perf-skills-improved |
|---|---|---|---|
| Regex Anti-Pattern Detection [CRITICAL] | 4.0 | 4.7 ± 0.6 | 5.0 |
| String Allocation Detection [CRITICAL] | 4.3 ± 0.6 | 4.7 ± 0.6 | 5.0 |
| Collection and LINQ Efficiency [CRITICAL] | 3.7 ± 0.6 | 4.3 ± 0.6 | 5.0 |
| Async and IO Pattern Detection [CRITICAL] | 4.0 | 4.3 ± 0.6 | 5.0 |
| Reflection and Serialization Overhead [HIGH] | 4.0 | 4.0 | 4.3 ± 0.6 |
| Structural Optimization Detection [HIGH] | 3.3 ± 0.6 | 4.3 ± 0.6 | 5.0 |
| Severity Classification Accuracy [HIGH] | 3.0 | 4.3 ± 1.2 | 4.0 ± 1.0 |
| Fix Recommendation Quality [HIGH] | 3.3 ± 0.6 | 4.7 ± 0.6 | 4.3 ± 0.6 |
| Token Efficiency [MEDIUM] | 5.0 | 2.0 | 2.0 |

---

## Final Rankings

| Rank | Configuration | Mean Score | % of Max (105) | Std Dev | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | dotnet-perf-skills-improved | 97.3 | 93% | 4.2 | 94.0 | 102.0 |
| 🥈 | dotnet-perf-skills | 90.7 | 86% | 10.1 | 80.0 | 100.0 |
| 🥉 | no-skills | 78.7 | 75% | 5.5 | 73.0 | 84.0 |

---

## Weighted Score per Run

| Run | no-skills | dotnet-perf-skills | dotnet-perf-skills-improved |
|---|---|---|---|
| 1 | 73.0 | 80.0 | 102.0 |
| 2 | 84.0 | 92.0 | 94.0 |
| 3 | 79.0 | 100.0 | 96.0 |
| **Mean** | **78.7** | **90.7** | **97.3** |

---

## Token Usage Summary

Average token consumption per configuration (1 outlier run(s) excluded from averages).

| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time | Δ Input vs Baseline |
|---|---|---|---|---|---|---|
| no-skills | 341,870 | 9,430 | 269,733 | 8 | 3m 40s | — (baseline) |
| dotnet-perf-skills | 734,642 | 12,416 | 505,246 | 11 | 4m 38s | +114.9% |
| dotnet-perf-skills-improved | 814,013 | 12,536 | 590,914 | 12 | 4m 38s | +138.1% |

---

## Token Usage Per Run

| Configuration | Run | Scenario | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time | Note |
|---|---|---|---|---|---|---|---|---|
| no-skills | 1 | analyze-perf-issues | 345,464 | 9,507 | 242,223 | 8 | 3m 51s | ⚠️ outlier |
| no-skills | 2 | analyze-perf-issues | 342,031 | 9,005 | 270,284 | 8 | 3m 32s |  |
| no-skills | 3 | analyze-perf-issues | 341,710 | 9,856 | 269,182 | 8 | 3m 48s |  |
| dotnet-perf-skills | 1 | analyze-perf-issues | 774,276 | 12,782 | 527,615 | 11 | 4m 54s |  |
| dotnet-perf-skills | 2 | analyze-perf-issues | 763,343 | 12,787 | 542,143 | 11 | 4m 37s |  |
| dotnet-perf-skills | 3 | analyze-perf-issues | 666,308 | 11,680 | 445,980 | 10 | 4m 21s |  |
| dotnet-perf-skills-improved | 1 | analyze-perf-issues | 851,107 | 12,533 | 626,385 | 12 | 4m 40s |  |
| dotnet-perf-skills-improved | 2 | analyze-perf-issues | 771,127 | 12,673 | 546,250 | 11 | 4m 38s |  |
| dotnet-perf-skills-improved | 3 | analyze-perf-issues | 819,806 | 12,403 | 600,106 | 12 | 4m 37s |  |


### ⚠️ Token Usage Outliers

The following runs were detected as outliers using the Modified Z-score (MAD) method. They are excluded from averages and Token Efficiency scores.

| Configuration | Run | Total Tokens | Details |
|---|---|---|---|
| no-skills | 1 | 354,971 | 9 turns, 20 tool calls; ~2× more tool calls than typical |

#### Recommendations to Reduce Outliers

- **no-skills run 1**: Most-used tool: `view` (12 calls). The agent may have struggled with the task structure. Consider simplifying the scenario prompt or adding clearer instructions in the skill.

---

## Consistency Analysis

| Configuration | Score σ | Most Consistent Dim (σ) | Most Variable Dim (σ) |
|---|---|---|---|
| no-skills | 5.5 | Regex Anti-Pattern Detection (0.0) | String Allocation Detection (0.6) |
| dotnet-perf-skills | 10.1 | Reflection and Serialization Overhead (0.0) | Severity Classification Accuracy (1.2) |
| dotnet-perf-skills-improved | 4.2 | Regex Anti-Pattern Detection (0.0) | Severity Classification Accuracy (1.0) |

---

## Per-Dimension Analysis

### 1. Regex Anti-Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills | dotnet-perf-skills-improved |
|---|---|---|---|
| 1 | 4 | 4 | 5 |
| 2 | 4 | 5 | 5 |
| 3 | 4 | 5 | 5 |
| **Mean** | **4.0** | **4.7** | **5.0** |

#### Analysis

`dotnet-perf-skills` covers per-call regex, compiled-regex startup budget, and `[GeneratedRegex]` migration very explicitly.

**Excerpt — dotnet-perf-skills (`output/dotnet-perf-skills/run-2/analyze-perf-issues/performance-analysis.md`)**
> #### 2. Uncached `new Regex()` in per-line hot path (8 instances)  
> **Impact:** `LogAnalyzer.TryParseLine` creates 2-3 new `Regex` objects per log line...  
> **Fix:** Hoist to `static readonly` fields, or use `[GeneratedRegex]` on .NET 8+.

**Excerpt — dotnet-perf-skills-improved (`output/dotnet-perf-skills-improved/run-2/analyze-perf-issues/performance-analysis.md`)**
> #### 13. 48 `RegexOptions.Compiled` without `[GeneratedRegex]` (MarkdownStripper)  
> **Impact:** 48 compiled regex instances in one class...  
> **Fix:** Convert to `[GeneratedRegex]` partial methods.

**Excerpt — no-skills (`output/no-skills/run-2/analyze-perf-issues/performance-analysis.md`)**
> ### 1. Regex Anti-Patterns (affects 5/10 files)  
> - **Per-call `new Regex`**: `TemplateEngine`, `SlugGenerator`, `CsvParser`, `ValidationEngine`, `LogAnalyzer`  
> - **Excessive `RegexOptions.Compiled`**: `MarkdownStripper` (45 compiled regexes)  
> - **Missing `[GeneratedRegex]`**: All files

**Score:** dotnet-perf-skills **5/5** · dotnet-perf-skills-improved **5/5** · no-skills **4/5**  
**Verdict:** **Tie between dotnet-perf-skills and dotnet-perf-skills-improved**. Both are comprehensive and hot-path aware; baseline is strong but less precise/consistent in counts and rigor.

### 2. String Allocation Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills | dotnet-perf-skills-improved |
|---|---|---|---|
| 1 | 4 | 4 | 5 |
| 2 | 5 | 5 | 5 |
| 3 | 4 | 5 | 5 |
| **Mean** | **4.3** | **4.7** | **5.0** |

#### Analysis

All three detect loop concatenation and casing-allocation issues; skills variants add stronger impact framing and broader pattern grouping.

**Excerpt — dotnet-perf-skills**
> #### 4. O(n²) string concatenation in loops (5 sites)  
> **Impact:** `+=` on strings in loops creates a new string on every iteration...  
> **Fix:** Replace with `StringBuilder`.

**Excerpt — dotnet-perf-skills-improved**
> #### 14. `.ToLower()`/`.ToUpper()` without culture or ordinal (17 instances)  
> **Impact:** Culture-sensitive by default (Turkish-I problem), each call allocates a new string.  
> **Fix:** Use `StringComparison.OrdinalIgnoreCase`...

**Excerpt — no-skills**
> | 1 | 🔴 Critical | 51-79 | **Character-by-character string concatenation** in `ParseLine`... | Use `StringBuilder` or `ReadOnlySpan<char>` slicing. |  
> | 3 | 🟡 Moderate | 38 | **`.ToLower()` without ordinal on header keys**... | Lowercase headers once and reuse... |

**Score:** dotnet-perf-skills **5/5** · dotnet-perf-skills-improved **5/5** · no-skills **5/5**  
**Verdict:** **Three-way tie**. All outputs clearly capture the highest-impact string-allocation problems and provide concrete remediation.

### 3. Collection and LINQ Efficiency [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills | dotnet-perf-skills-improved |
|---|---|---|---|
| 1 | 3 | 4 | 5 |
| 2 | 4 | 4 | 5 |
| 3 | 4 | 5 | 5 |
| **Mean** | **3.7** | **4.3** | **5.0** |

#### Analysis

Improved skills output is the most exhaustive on O(n) lookup and materialization patterns; baseline and standard skills are good but less complete.

**Excerpt — dotnet-perf-skills**
> #### 8. `ContainsKey` + indexer double-lookup pattern (10 actionable sites)  
> **Fix:** Replace with `TryGetValue`.  
> #### 11. `Skip(i).Take(5).ToList()` sliding window in loop (1 instance)

**Excerpt — dotnet-perf-skills-improved**
> #### 9. `List.Contains()` used as lookup — O(n) per check (2 sites)  
> **Fix:** Use `HashSet<string>` for O(1) lookups.  
> #### 24. Three separate iterations over same list (LogAnalyzer.Analyze)

**Excerpt — no-skills**
> | 6 | 🔴 Critical | 75-85 | **`.ToList()` + `.Contains()` (O(n)) in a loop**... | Use a `HashSet<string>`... |  
> ### 4. `ContainsKey` + Indexer Double Lookup (affects 4/10 files)  
> Found in `EntityMapper`, `ValidationEngine`, `LogAnalyzer`, `DataPipeline`.

**Score:** dotnet-perf-skills **4/5** · dotnet-perf-skills-improved **5/5** · no-skills **4/5**  
**Verdict:** **dotnet-perf-skills-improved wins** due to broader LINQ/materialization coverage and clearer complexity framing.

### 4. Async and IO Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills | dotnet-perf-skills-improved |
|---|---|---|---|
| 1 | 4 | 4 | 5 |
| 2 | 4 | 4 | 5 |
| 3 | 4 | 5 | 5 |
| **Mean** | **4.0** | **4.3** | **5.0** |

#### Analysis

All outputs catch `HttpClient` misuse and batch-send async issues; improved skills has the clearest end-to-end coverage including cancellation.

**Excerpt — dotnet-perf-skills**
> #### 1. `new HttpClient()` per call — socket exhaustion risk (3 instances)  
> #### 12. Sequential awaits in loop ... + unbounded parallelism ...  
> **Fix:** Use `Parallel.ForEachAsync` ... Add `CancellationToken` parameters.

**Excerpt — dotnet-perf-skills-improved**
> #### 18. Sequential awaits in batch loop (1 instance)  
> #### 19. Unbounded parallelism in `SendBatchParallelAsync` (1 instance)  
> #### 20. Missing cancellation tokens in async methods

**Excerpt — no-skills**
> | 1 | 🔴 Critical | 163, 179, 191 | **`new HttpClient` per call**... | Use `IHttpClientFactory`... |  
> | 2 | 🟡 Moderate | 116-123 | **Sequential `await` in loop**... | Use `Task.WhenAll` with throttling... |  
> | 3 | 🟡 Moderate | 130-133 | **Unbounded parallelism**... |

**Score:** dotnet-perf-skills **4/5** · dotnet-perf-skills-improved **5/5** · no-skills **4/5**  
**Verdict:** **dotnet-perf-skills-improved is best** for explicit, complete async/IO anti-pattern coverage.

### 5. Reflection and Serialization Overhead [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills | dotnet-perf-skills-improved |
|---|---|---|---|
| 1 | 4 | 4 | 5 |
| 2 | 4 | 4 | 4 |
| 3 | 4 | 4 | 4 |
| **Mean** | **4.0** | **4.0** | **4.3** |

#### Analysis

All three detect uncached reflection and per-call serializer options strongly; none deeply push partial parsing (`Utf8JsonReader`) opportunities.

**Excerpt — dotnet-perf-skills**
> #### 3. Uncached `new JsonSerializerOptions` per call (4 instances)  
> #### 7. Uncached reflection `GetProperties()`...`SetValue()`...`GetValue()` (6 instances)

**Excerpt — dotnet-perf-skills-improved**
> #### 2. Uncached `new JsonSerializerOptions` per call (5 instances)  
> #### 11. Uncached reflection `GetProperties()`/`GetProperty()` in hot paths (3 instances)  
> #### 12. Uncached reflection `SetValue()`/`GetValue()` in mapping loops (3 instances)

**Excerpt — no-skills**
> | 1 | 🔴 Critical | 74 | **`new JsonSerializerOptions` per call in `Merge`**... | Cache as a `private static readonly JsonSerializerOptions`. |  
> | 1 | 🔴 Critical | 77, 101-102 | **Uncached `GetProperties()` + `SetValue()` via reflection**... |

**Score:** dotnet-perf-skills **4/5** · dotnet-perf-skills-improved **4/5** · no-skills **4/5**  
**Verdict:** **Tie**. Coverage is strong across all three, with a common gap on recommending selective parsing strategies.

### 6. Structural Optimization Detection [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills | dotnet-perf-skills-improved |
|---|---|---|---|
| 1 | 4 | 4 | 5 |
| 2 | 3 | 4 | 5 |
| 3 | 3 | 5 | 5 |
| **Mean** | **3.3** | **4.3** | **5.0** |

#### Analysis

Improved skills output is strongest on sealed-class census + `IEquatable<T>` + `FrozenDictionary` specifics.

**Excerpt — dotnet-perf-skills**
> #### 14. Unsealed leaf classes (17 of 17 classes unsealed, 0 sealed)  
> #### 9. Structs without `IEquatable<T>` (2 of 2 structs)  
> #### 10. `static readonly Dictionary<>` → `FrozenDictionary` candidates (2 instances)

**Excerpt — dotnet-perf-skills-improved**
> #### 16. Unsealed classes — 18 of 18 classes are unsealed (0 sealed)  
> #### 17. Structs without `IEquatable<T>` (2 of 2 structs)  
> #### 15. `static readonly Dictionary<>` — FrozenDictionary candidates (2 instances)

**Excerpt — no-skills**
> ### 6. Unsealed Leaf Classes (affects 3/10 files)  
> `ValidationResult`, `MappingConfig`, `Record`...  
> ### 7. Structs Without `IEquatable<T>` (affects 2/10 files)

**Score:** dotnet-perf-skills **4/5** · dotnet-perf-skills-improved **5/5** · no-skills **3/5**  
**Verdict:** **dotnet-perf-skills-improved wins** with the most systematic structural analysis.

### 7. Severity Classification Accuracy [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills | dotnet-perf-skills-improved |
|---|---|---|---|
| 1 | 3 | 3 | 5 |
| 2 | 3 | 5 | 3 |
| 3 | 3 | 5 | 4 |
| **Mean** | **3.0** | **4.3** | **4.0** |

#### Analysis

Standard skills output has the cleanest impact-tier calibration. Improved and baseline over-escalate some moderate patterns.

**Excerpt — dotnet-perf-skills**
> | 🔴 Critical | 5 | `new HttpClient()` per call..., uncached `new Regex()` in per-line parser... |  
> | 🟡 Moderate | 8 | `.ToLower()`/`.ToUpper()`..., `ContainsKey`+indexer..., structs without `IEquatable<T>`... |

**Excerpt — dotnet-perf-skills-improved**
> | 🔴 Critical | 12 | Per-call `new Regex()`..., `new HttpClient()`..., uncached `JsonSerializerOptions`... |  
> #### 10. ContainsKey + indexer double-lookup (10 instances)  
> **Impact:** ~2× slower per dictionary access...

**Excerpt — no-skills**
> | 🔴 Critical | 7 |  
> | 🟡 Moderate | 22 |  
> | ℹ️ Info | 23 |  
> | 4 | ℹ️ Info | 126-133 | **`ContainsKey` + indexer** for tag counting. |

**Score:** dotnet-perf-skills **5/5** · dotnet-perf-skills-improved **3/5** · no-skills **3/5**  
**Verdict:** **dotnet-perf-skills is best** because it consistently keeps hot-path/regression-risk issues above lower-impact micro-optimizations.

### 8. Fix Recommendation Quality [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills | dotnet-perf-skills-improved |
|---|---|---|---|
| 1 | 3 | 4 | 5 |
| 2 | 4 | 5 | 4 |
| 3 | 3 | 5 | 4 |
| **Mean** | **3.3** | **4.7** | **4.3** |

#### Analysis

All three are actionable, but standard skills output has the best balance of API-specific fixes plus guardrails against bad advice.

**Excerpt — dotnet-perf-skills**
> **Fix:** Inject `IHttpClientFactory` or use a single `static readonly HttpClient`...  
> **Fix:** ...use `[GeneratedRegex]` on .NET 8+...  
> **Fix:** Use `StringComparison.OrdinalIgnoreCase`... `ToLowerInvariant()`...

**Excerpt — dotnet-perf-skills-improved**
> **Fix:** Use `Parallel.ForEachAsync` with `MaxDegreeOfParallelism`, or `SemaphoreSlim`...  
> **Fix:** Add `CancellationToken` parameters and pass to `Task.Delay`...

**Excerpt — no-skills**
> | Rank | 1 | Replace `new HttpClient()` with `IHttpClientFactory` or static instance |  
> | Rank | 7 | Migrate 45 `RegexOptions.Compiled` to `[GeneratedRegex]` |

**Score:** dotnet-perf-skills **5/5** · dotnet-perf-skills-improved **4/5** · no-skills **4/5**  
**Verdict:** **dotnet-perf-skills wins** on precision and correctness of fix guidance.

### 9. Token Efficiency [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills | dotnet-perf-skills-improved |
|---|---|---|---|
| 1 | — | 2 | 2 |
| 2 | 5 | 2 | 2 |
| 3 | 5 | 2 | 2 |
| **Mean** | **5.0** | **2.0** | **2.0** |

---

## Asset Usage Summary

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| no-skills | 1 | 3fc19cbe…24c6 | claude-opus-4.6 | — | — | ✅ |
| no-skills | 2 | 6cf34a77…35be | claude-opus-4.6 | — | — | ✅ |
| no-skills | 3 | 2a170aa8…4ace | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills | 1 | a136cc9e…208b | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills | 2 | 973a218f…43c6 | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills | 3 | 15a7bb3e…33f4 | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills-improved | 1 | de018865…8ec0 | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills-improved | 2 | b081e1af…e884 | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills-improved | 3 | 54364348…8e95 | claude-opus-4.6 | — | — | ✅ |

---

## Copilot Recommendations (Best Run)

Prioritized recommendations extracted from the highest-scoring run's output for each configuration.

### no-skills (run 2, score 84)

## Prioritized Fix Recommendations

| Rank | Fix | Files | Severity | Effort | Impact |
|------|-----|-------|----------|--------|--------|
| 1 | Replace `new HttpClient()` with `IHttpClientFactory` or static instance | NotificationService | 🔴 Critical | Quick-fix | Prevents socket exhaustion in production |
| 2 | Hoist `new Regex(...)` to `static readonly` fields | LogAnalyzer, TemplateEngine, CsvParser, ValidationEngine | 🔴 Critical | Quick-fix | Eliminates thousands of regex compilations per call |
| 3 | Replace `string +=` in loops with `StringBuilder` | TemplateEngine, CsvParser, LogAnalyzer, NotificationService, DataPipeline, ValidationEngine | 🔴 Critical | Quick-fix | Eliminates O(n²) allocation patterns |
| 4 | Cache `GetProperties()`/reflection results per type | EntityMapper, TemplateEngine | 🔴 Critical | Moderate | 10-100x speedup for batch mapping operations |
| 5 | Cache `JsonSerializerOptions` as static field | JsonTransformer | 🔴 Critical | Quick-fix | Eliminates expensive options object creation per call |
| 6 | Use `HashSet<string>` instead of `List.Contains` | SlugGenerator, JsonTransformer | 🔴 Critical | Quick-fix | O(1) vs O(n) lookups — critical for large datasets |
| 7 | Migrate 45 `RegexOptions.Compiled` to `[GeneratedRegex]` | MarkdownStripper | 🟡 Moderate | Moderate | Eliminates JIT startup cost, improves throughput |
| 8 | Add `SemaphoreSlim` throttling to parallel batch send | NotificationService | 🟡 Moderate | Quick-fix | Prevents downstream service overwhelm |
| 9 | Use `CancellationToken` in async methods | NotificationService | 🟡 Moderate | Moderate | Enables graceful cancellation of retries and batch ops |
| 10 | Replace `.ToLower()` with `OrdinalIgnoreCase` comparisons | SlugGenerator, CsvParser, JsonTransformer, EntityMapper, LogAnalyzer | 🟡 Moderate | Quick-fix | Avoids unnecessary allocations and Turkish-I bugs |

### dotnet-perf-skills (run 3, score 100)

## Prioritized Fix Recommendations

| # | Fix | Severity | Effort | Impact |
|---|-----|----------|--------|--------|
| 1 | Replace `new HttpClient()` with shared/injected client | 🔴 | Quick-fix | Prevents socket exhaustion in production |
| 2 | Hoist `new Regex()` in `LogAnalyzer.TryParseLine` to static fields/`[GeneratedRegex]` | 🔴 | Quick-fix | >100x faster log parsing |
| 3 | Cache `JsonSerializerOptions` as `static readonly` in `JsonTransformer` | 🔴 | Quick-fix | Up to 592x faster serialization |
| 4 | Replace `+=` string concatenation in loops with `StringBuilder` (all 6 sites) | 🔴 | Moderate | Eliminates O(n²) allocations in all text-building paths |
| 5 | Migrate `MarkdownStripper`'s 48 compiled regexes to `[GeneratedRegex]` | 🟡 | Moderate | Eliminates ~50-100ms startup cost |
| 6 | Replace `SlugGenerator`'s 14 `Regex.Replace` calls with `[GeneratedRegex]` | 🔴 | Moderate | Eliminates regex cache thrashing |
| 7 | Cache reflection in `EntityMapper` with `ConcurrentDictionary<Type, PropertyInfo[]>` | 🟡 | Moderate | ~100x faster property mapping |
| 8 | Replace all `ContainsKey` + indexer with `TryGetValue` (8 sites) | 🟡 | Quick-fix | ~2x faster dictionary access |
| 9 | Add `sealed` to all 18 leaf classes | 🟡 | Quick-fix | Enables JIT devirtualization |
| 10 | Replace `.ToLower()`/`.ToUpper()` with invariant/ordinal alternatives | 🟡 | Quick-fix | 2-3x faster comparisons, fixes Turkish-I bug |

> ⚠️ **Disclaimer:** These results are generated by an AI assistant and are non-deterministic. Findings may include false positives, miss real issues, or suggest changes that are incorrect for your specific context. Always verify recommendations with benchmarks and human review before applying changes to production code.

### dotnet-perf-skills-improved (run 1, score 102)

## Prioritized Fix Recommendations

| # | Fix | Impact | Effort | Files |
|---|-----|--------|--------|-------|
| 1 | Replace `new HttpClient()` with static/injected client | 🔴 Prevents socket exhaustion | Quick-fix | NotificationService.cs |
| 2 | Cache regex instances in `LogAnalyzer.TryParseLine` as `[GeneratedRegex]` | 🔴 >10x speedup for log parsing | Quick-fix | LogAnalyzer.cs |
| 3 | Cache `JsonSerializerOptions` as `static readonly` | 🔴 Up to 592x faster serialization | Quick-fix | JsonTransformer.cs |
| 4 | Convert 48 `RegexOptions.Compiled` to `[GeneratedRegex]` in MarkdownStripper | 🟡 Near-zero startup, AOT-ready | Moderate | MarkdownStripper.cs |
| 5 | Replace string `+=` loops with `StringBuilder` (6 sites) | 🔴 Eliminates O(n²) allocation | Quick-fix | TemplateEngine, LogAnalyzer, DataPipeline, NotificationService |
| 6 | Cache reflection `GetProperties()` per type | 🔴 ~100x faster property access | Quick-fix | EntityMapper.cs |
| 7 | Replace `.ToLower()`/`.ToUpper()` with ordinal comparisons (18 sites) | 🟡 2-3x faster, fixes Turkish-I | Quick-fix | All 6 affected files |
| 8 | Pre-compile regex in `ValidationEngine.AddPattern` | 🔴 Eliminates per-validation regex construction | Quick-fix | ValidationEngine.cs |
| 9 | Use `HashSet<string>` in `SlugGenerator.GenerateUniqueSlug` | 🟡 O(1) vs O(n) lookups | Quick-fix | SlugGenerator.cs |
| 10 | Add `CancellationToken` to all async methods | 🟡 Enables graceful cancellation | Moderate | NotificationService.cs |

### Positive Findings

- ✅ No `async void` methods — all async methods return `Task` or `Task<T>`
- ✅ No sync-over-async (`.Result`, `.Wait()`) patterns detected
- ✅ `TryGetValue` is used correctly in some places (TemplateEngine.cs:L34, L49)
- ✅ `HashCode.Combine` used for struct `GetHashCode` (NotificationService.cs:L48)
- ✅ Null-checking and input validation present in most public methods

> ⚠️ **Disclaimer:** These results are generated by an AI assistant and are non-deterministic. Findings may include false positives, miss real issues, or suggest changes that are incorrect for your specific context. Always verify recommendations with benchmarks and human review before applying changes to production code.

---

## Raw Data References

- Per-run analysis: `reports/analysis-run-1.md`
- Per-run analysis: `reports/analysis-run-2.md`
- Per-run analysis: `reports/analysis-run-3.md`
- Score data: `reports/scores-data.json`
- Generation usage: `reports/generation-usage.json`
