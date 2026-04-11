# Aggregated Analysis: .NET Performance Analysis Skill Evaluation

**Runs:** 3 | **Configurations:** 2 | **Scenarios:** 1 | **Dimensions:** 9
**Date:** 2026-04-11 14:56 UTC

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

**Maximum possible weighted score: 100.0** (all dimensions scoring 5).
Scores shown as **mean ± standard deviation** across runs.

---

## Executive Summary

| Dimension [Tier] | no-skills | dotnet-perf-skills |
|---|---|---|
| Regex Anti-Pattern Detection [CRITICAL] | 4.3 ± 0.6 | 5.0 |
| String Allocation Detection [CRITICAL] | 4.3 ± 0.6 | 5.0 |
| Collection and LINQ Efficiency [CRITICAL] | 4.0 | 5.0 |
| Async and IO Pattern Detection [CRITICAL] | 4.0 | 5.0 |
| Reflection and Serialization Overhead [HIGH] | 3.7 ± 0.6 | 4.0 |
| Structural Optimization Detection [HIGH] | 4.0 | 5.0 |
| Severity Classification Accuracy [HIGH] | 2.7 ± 0.6 | 4.7 ± 0.6 |
| Fix Recommendation Quality [HIGH] | 3.3 ± 0.6 | 5.0 |
| Token Efficiency [MEDIUM] | 5.0 | 2.5 ± 0.7 |

---

## Final Rankings

| Rank | Configuration | Mean Score | % of Max (100) | Std Dev | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | dotnet-perf-skills | 99.0 | 99% | 2.6 | 96.0 | 101.0 |
| 🥈 | no-skills | 80.7 | 81% | 0.6 | 80.0 | 81.0 |

---

## Weighted Score per Run

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 81.0 | 96.0 |
| 2 | 80.0 | 100.0 |
| 3 | 81.0 | 101.0 |
| **Mean** | **80.7** | **99.0** |

---

## Token Usage Summary

Average token consumption per configuration (2 outlier run(s) excluded from averages).

| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time | Δ Input vs Baseline |
|---|---|---|---|---|---|---|
| no-skills | 428,182 | 9,042 | 340,046 | 10 | 3m 57s | — (baseline) |
| dotnet-perf-skills | 836,088 | 11,746 | 620,974 | 12 | 3m 56s | +95.3% |

---

## Token Usage Per Run

| Configuration | Run | Scenario | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time | Note |
|---|---|---|---|---|---|---|---|---|
| no-skills | 1 | analyze-perf-issues | 466,148 | 8,927 | 359,014 | 10 | 4m 7s |  |
| no-skills | 2 | analyze-perf-issues | 1,597,501 | 14,869 | 1,398,629 | 26 | 3m 47s | ⚠️ outlier |
| no-skills | 3 | analyze-perf-issues | 390,216 | 9,158 | 321,079 | 9 | 3m 48s |  |
| dotnet-perf-skills | 1 | analyze-perf-issues | — | — | — | — | — | ⚠️ no usage data |
| dotnet-perf-skills | 2 | analyze-perf-issues | 922,172 | 11,842 | 708,131 | 13 | 4m 5s |  |
| dotnet-perf-skills | 3 | analyze-perf-issues | 750,004 | 11,649 | 533,818 | 11 | 3m 48s |  |


### ⚠️ Token Usage Outliers

The following runs were detected as outliers using the Modified Z-score (MAD) method. They are excluded from averages and Token Efficiency scores.

| Configuration | Run | Total Tokens | Details |
|---|---|---|---|
| no-skills | 2 | 1,612,370 | 9 turns, 21 tool calls; ~2× more tool calls than typical |

#### Recommendations to Reduce Outliers

- **no-skills run 2**: Most-used tool: `view` (13 calls). The agent may have struggled with the task structure. Consider simplifying the scenario prompt or adding clearer instructions in the skill.

---

## Consistency Analysis

| Configuration | Score σ | Most Consistent Dim (σ) | Most Variable Dim (σ) |
|---|---|---|---|
| no-skills | 0.6 | Collection and LINQ Efficiency (0.0) | Regex Anti-Pattern Detection (0.6) |
| dotnet-perf-skills | 2.6 | Regex Anti-Pattern Detection (0.0) | Token Efficiency (0.7) |

---

## Per-Dimension Analysis

### 1. Regex Anti-Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 5 | 5 |
| 3 | 4 | 5 |
| **Mean** | **4.3** | **5.0** |

#### Analysis

**dotnet-perf-skills — `output/dotnet-perf-skills/run-2/analyze-perf-issues/performance-analysis.md`**
> **Three `new Regex(...)` per log line** — this is the single hottest path in the class.
>  
> **40+ `RegexOptions.Compiled` static fields** ... **Fix:** On .NET 8+, use `[GeneratedRegex]`

**no-skills — `output/no-skills/run-2/analyze-perf-issues/performance-analysis.md`**
> **Per-Call `new Regex()` in Hot Paths (8 instances)** ... In `LogAnalyzer.TryParseLine()`, this runs per log line
>  
> **48 `RegexOptions.Compiled` Without `[GeneratedRegex]`**

**Score:** dotnet-perf-skills **5/5**; no-skills **5/5**. Both clearly identify per-call regex construction, compiled-regex startup cost, and recommend `[GeneratedRegex]`.

**Verdict:** **Tie** — both outputs are excellent and specific on regex risks.

### 2. String Allocation Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 5 | 5 |
| 3 | 4 | 5 |
| **Mean** | **4.3** | **5.0** |

#### Analysis

**dotnet-perf-skills**
> **`string +=` character-by-character** in a loop ... O(N²) allocations.
>  
> **Chain of 40+ `.Replace()` calls**, each allocating a new string.
>  
> **`.ToLower()` without `StringComparison`** ... Turkish-I problem.

**no-skills**
> String Concatenation (`+=`) in Loops — O(n²) (7 sites)
>  
> `.ToLower()`/`.ToUpper()` Without Culture (16 instances)
>  
> Chained `Regex.Replace` Allocations in `SlugGenerator` (14 calls)

**Score:** dotnet-perf-skills **5/5**; no-skills **5/5**. Both cover loop concatenation, casing allocations/correctness, and replace-chain allocation pressure.

**Verdict:** **Tie** — both are comprehensive and actionable.

### 3. Collection and LINQ Efficiency [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 4 | 5 |
| 3 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

**dotnet-perf-skills**
> `allKeys.ToList()` + `.Contains(key)` ... O(n²) overall. **Fix:** Use `HashSet<string>`
>  
> `ContainsKey` + indexer double lookup.
>  
> `Skip(i).Take(5).ToList()` inside a loop — O(n²) allocation for the sliding window.

**no-skills**
> `ContainsKey` + Indexer Double-Lookup (12+ instances)
>  
> `.ToList()` + `.Contains()` — O(n) Lookups (2 instances)
>  
> `.Where().ToList()` Just for Counting (2 instances)

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**. Baseline is strong, but skill-backed output is more complete for hot-path LINQ/window patterns (explicit `Skip/Take/ToList` sliding-window issue).

**Verdict:** **dotnet-perf-skills wins** on depth in hot-path collection/LINQ cases.

### 4. Async and IO Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 4 | 5 |
| 3 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

**dotnet-perf-skills**
> **`new HttpClient()` per call** ... socket exhaustion risk.
>  
> **Unbounded parallelism** ... 10K notifications = 10K concurrent HTTP calls.
>  
> **Sequential `await` in a loop**
>  
> `Task.Delay` without `CancellationToken`

**no-skills**
> `new HttpClient()` Per Call — Socket Exhaustion (3 instances)
>  
> Sequential Awaits in Batch Loop (1 instance)
>  
> the parallel version ... has unbounded parallelism (separate concern).

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**. Both catch major IO issues, but dotnet-perf-skills is more direct and explicit on cancellation propagation.

**Verdict:** **dotnet-perf-skills wins** with more complete async hygiene coverage.

### 5. Reflection and Serialization Overhead [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 4 |
| 2 | 4 | 4 |
| 3 | 3 | 4 |
| **Mean** | **3.7** | **4.0** |

#### Analysis

**dotnet-perf-skills**
> `typeof(T).GetProperties()` on every call ... **Fix:** Cache `PropertyInfo[]`
>  
> `new JsonSerializerOptions` per call ... recreating them discards the cache

**no-skills**
> Uncached `new JsonSerializerOptions` Per Call ... **Up to 592× slower**
>  
> Uncached Reflection ... `GetProperties()`/`GetProperty()`/`SetValue()` in hot paths

**Score:** dotnet-perf-skills **4/5**; no-skills **4/5**. Both identify the core reflection/options problems and propose caching. Neither output deeply develops partial parsing (`Utf8JsonReader`) guidance.

**Verdict:** **Tie** — comparable quality on this dimension.

### 6. Structural Optimization Detection [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 4 | 5 |
| 3 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

**dotnet-perf-skills**
> `MappingConfig` ... **Unsealed class**
>  
> `ValidationError` ... **Struct without `IEquatable<ValidationError>`**
>  
> Static `Dictionary` ... candidate for `FrozenDictionary` on .NET 8+

**no-skills**
> 17 Unsealed Classes (17 of 17, 0 sealed)
>  
> Structs Without `IEquatable<T>` (2 structs, 0 implementing)
>  
> `FrozenDictionary` Candidates (2 instances)

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**. Baseline detects all categories but is more blanket/less selective (e.g., “seal all 17 classes”).

**Verdict:** **dotnet-perf-skills wins** for targeted, lower-risk structural recommendations.

### 7. Severity Classification Accuracy [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 3 | 4 |
| 2 | 2 | 5 |
| 3 | 3 | 5 |
| **Mean** | **2.7** | **4.7** |

#### Analysis

**dotnet-perf-skills**
> **Top priorities:** `new HttpClient` per call ... regex in hot paths ... string `+=` in loops ... unbounded parallelism
>  
> 40+ `RegexOptions.Compiled` ... startup cost ... 🟡 Moderate

**no-skills**
> 🔴 **Critical**: ... `ContainsKey` + indexer double-lookups, and `.ToList()` + `.Contains()` O(n) lookups
>  
> **Fix `new HttpClient()` ... cache `JsonSerializerOptions` ... hoist `new Regex()`**

**Score:** dotnet-perf-skills **5/5**; no-skills **2/5**. dotnet-perf-skills mostly separates production-critical hot-path issues from moderate optimization items. no-skills over-classifies several medium-impact collection patterns as critical, reducing prioritization signal.

**Verdict:** **dotnet-perf-skills clearly wins** on practical triage quality.

### 8. Fix Recommendation Quality [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 3 | 5 |
| 2 | 3 | 5 |
| 3 | 4 | 5 |
| **Mean** | **3.3** | **5.0** |

#### Analysis

**dotnet-perf-skills**
> **Fix:** Inject `IHttpClientFactory` or use a single `static HttpClient`.
>  
> **Fix:** Use `StringComparison.OrdinalIgnoreCase` ... or `ToLowerInvariant()`.
>  
> **Fix:** use `[GeneratedRegex]` partial methods

**no-skills**
> **Fix:** Hoist to `private static readonly Regex` ... or preferably `[GeneratedRegex]`
>  
> **Fix:** Add `sealed` keyword to all leaf classes. None ... are subclassed within the project.
>  
> **Fix:** ... on .NET 9+ use `params ReadOnlySpan<T>`

**Score:** dotnet-perf-skills **5/5**; no-skills **3/5**. Baseline includes many concrete APIs, but several recommendations are over-broad or high-confidence without sufficient safety qualifiers.

**Verdict:** **dotnet-perf-skills wins** for consistently precise, context-safe guidance.

### 9. Token Efficiency [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | — |
| 2 | — | 2 |
| 3 | 5 | 3 |
| **Mean** | **5.0** | **2.5** |

---

## Asset Usage Summary

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| no-skills | 1 | 0c1b6ff4…3add | claude-opus-4.6 | — | — | ✅ |
| no-skills | 2 | 6a0dd039…61cf | claude-opus-4.6 | — | — | ✅ |
| no-skills | 3 | 9302f1a3…e7d5 | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills | 1 | 8ac9a6e8…72d8 | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills | 2 | 817bf3ae…35b7 | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills | 3 | 906c444e…6822 | claude-opus-4.6 | — | — | ✅ |

---

## Copilot Recommendations (Best Run)

Prioritized recommendations extracted from the highest-scoring run's output for each configuration.

### no-skills (run 1, score 81)

## Prioritized Fix Recommendations

| Rank | Fix | Files | Severity | Effort | Impact |
|------|-----|-------|----------|--------|--------|
| 1 | **Replace `new HttpClient()` with injected/singleton instance** | NotificationService | 🔴 Critical | Quick-fix | Prevents socket exhaustion under load |
| 2 | **Hoist `new Regex(...)` in `TryParseLine` to static fields / `[GeneratedRegex]`** | LogAnalyzer | 🔴 Critical | Quick-fix | Eliminates regex compilation per log line (potentially millions of times) |
| 3 | **Replace char-by-char `+=` with `StringBuilder` in `ParseLine` / `SplitLines`** | CsvParser | 🔴 Critical | Quick-fix | Eliminates O(n²) allocations on every CSV parse |
| 4 | **Convert 40+ `RegexOptions.Compiled` to `[GeneratedRegex]`** | MarkdownStripper | 🟡 Moderate | Moderate | Eliminates JIT compilation overhead at startup; AOT-friendly |
| 5 | **Cache `new JsonSerializerOptions` as static readonly** | JsonTransformer | 🟡 Moderate | Quick-fix | Preserves internal serialization metadata cache |
| 6 | **Cache `GetProperties()` per type in `MapTo<T>` / `MapFrom<T>`** | EntityMapper | 🔴 Critical | Moderate | Eliminates reflection per call in potentially hot mapping paths |
| 7 | **Replace `List.Contains` with `HashSet` in `Diff` and `GenerateUniqueSlug`** | JsonTransformer, SlugGenerator | 🟡 Moderate | Quick-fix | O(1) vs O(n) lookups |
| 8 | **Pre-compile regex at registration in `AddPattern`** | ValidationEngine | 🟡 Moderate | Quick-fix | Regex compiled once, not per validation call |
| 9 | **Add `SemaphoreSlim` throttle to `SendBatchParallelAsync`** | NotificationService | 🟡 Moderate | Quick-fix | Prevents resource exhaustion from unbounded parallelism |
| 10 | **Replace `string +=` with `StringBuilder` across all report-formatting methods** | LogAnalyzer, DataPipeline, NotificationService, TemplateEngine, ValidationEngine | 🟡 Moderate | Moderate | Reduces allocations in all output-building code |

### dotnet-perf-skills (run 3, score 101)

## Prioritized Fix Recommendations

| # | Fix | Impact | Effort | Files |
|---|-----|--------|--------|-------|
| 1 | Replace `new HttpClient()` with shared/injected instance | 🔴 Socket exhaustion prevention | Quick-fix | NotificationService.cs |
| 2 | Hoist `new Regex()` to `[GeneratedRegex]` static fields | 🔴 10-100x per-call improvement | Moderate | LogAnalyzer, TemplateEngine, ValidationEngine, CsvParser |
| 3 | Cache `JsonSerializerOptions` as `static readonly` | 🔴 Up to 592x improvement | Quick-fix | JsonTransformer.cs |
| 4 | Replace `+=` string concatenation with `StringBuilder` | 🔴 O(n²) → O(n) allocation | Moderate | 7 files |
| 5 | Convert 48 `Compiled` regex to `[GeneratedRegex]` | 🟡 Faster startup, better throughput | Moderate | MarkdownStripper.cs |
| 6 | Replace `ContainsKey` + indexer with `TryGetValue` | 🟡 ~2x per-lookup | Quick-fix | 4 files |
| 7 | Fix `.ToLower()`/`.ToUpper()` — use ordinal comparison | 🟡 2-3x faster + correctness | Quick-fix | 6 files |
| 8 | Cache reflection (`GetProperties`/`SetValue`) per type | 🔴 100-1000x faster | Moderate | EntityMapper.cs, TemplateEngine.cs |
| 9 | Seal all 18 leaf classes | 🟡 JIT devirtualization | Quick-fix | All files |
| 10 | Use `HashSet` instead of `List.Contains()` | 🟡 O(n) → O(1) lookup | Quick-fix | SlugGenerator.cs, JsonTransformer.cs |

> ⚠️ **Disclaimer:** These results are generated by an AI assistant and are non-deterministic. Findings may include false positives, miss real issues, or suggest changes that are incorrect for your specific context. Always verify recommendations with benchmarks and human review before applying changes to production code.

---

## Raw Data References

- Per-run analysis: `reports/analysis-run-1.md`
- Per-run analysis: `reports/analysis-run-2.md`
- Per-run analysis: `reports/analysis-run-3.md`
- Verification data: `reports/verification-data.json`
- Score data: `reports/scores-data.json`
- Build notes: `reports/build-notes.md`
- Generation usage: `reports/generation-usage.json`
