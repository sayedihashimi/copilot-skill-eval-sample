# Aggregated Analysis: .NET Performance Analysis Skill Evaluation

**Runs:** 5 | **Configurations:** 2 | **Scenarios:** 1 | **Dimensions:** 9
**Date:** 2026-04-10 21:00 UTC

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
| Regex Anti-Pattern Detection [CRITICAL] | 4.4 ± 0.5 | 5.0 |
| String Allocation Detection [CRITICAL] | 4.4 ± 0.5 | 5.0 |
| Collection and LINQ Efficiency [CRITICAL] | 4.4 ± 0.5 | 5.0 |
| Async and IO Pattern Detection [CRITICAL] | 4.6 ± 0.5 | 5.0 |
| Reflection and Serialization Overhead [HIGH] | 4.0 | 4.4 ± 0.5 |
| Structural Optimization Detection [HIGH] | 4.2 ± 0.8 | 5.0 |
| Severity Classification Accuracy [HIGH] | 3.8 ± 0.8 | 4.4 ± 0.5 |
| Fix Recommendation Quality [HIGH] | 4.2 ± 0.4 | 5.0 |
| Token Efficiency [MEDIUM] | 4.6 ± 0.5 | 2.0 |

---

## Final Rankings

| Rank | Configuration | Mean Score | % of Max (100) | Std Dev | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | dotnet-perf-skills | 99.6 | 100% | 0.9 | 98.0 | 100.0 |
| 🥈 | no-skills | 90.4 | 90% | 6.0 | 87.0 | 101.0 |

---

## Weighted Score per Run

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 89.0 | 100.0 |
| 2 | 101.0 | 98.0 |
| 3 | 87.0 | 100.0 |
| 4 | 88.0 | 100.0 |
| 5 | 87.0 | 100.0 |
| **Mean** | **90.4** | **99.6** |

---

## Token Usage Summary

Average token consumption per configuration across all runs.

| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time | Δ Input vs Baseline |
|---|---|---|---|---|---|---|
| no-skills | 388,779 | 8,841 | 319,918 | 9 | 3m 14s | — (baseline) |
| dotnet-perf-skills | 805,145 | 14,018 | 624,458 | 12 | 4m 41s | +107.1% |

---

## Token Usage Per Run

| Configuration | Run | Scenario | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time | Note |
|---|---|---|---|---|---|---|---|---|
| no-skills | 1 | analyze-perf-issues | 388,842 | 8,818 | 319,953 | 9 | 3m 16s |  |
| no-skills | 2 | analyze-perf-issues | 386,136 | 8,473 | 317,674 | 9 | 3m 2s |  |
| no-skills | 3 | analyze-perf-issues | 388,508 | 8,459 | 320,075 | 9 | 3m 4s |  |
| no-skills | 4 | analyze-perf-issues | 387,090 | 8,640 | 318,484 | 9 | 3m 16s |  |
| no-skills | 5 | analyze-perf-issues | 393,317 | 9,816 | 323,405 | 9 | 3m 31s |  |
| dotnet-perf-skills | 1 | analyze-perf-issues | 788,911 | 11,767 | 589,878 | 12 | 4m 5s |  |
| dotnet-perf-skills | 2 | analyze-perf-issues | 906,428 | 19,443 | 677,527 | 13 | 6m 0s |  |
| dotnet-perf-skills | 3 | analyze-perf-issues | 718,269 | 14,910 | 515,129 | 11 | 4m 38s |  |
| dotnet-perf-skills | 4 | analyze-perf-issues | 780,981 | 11,784 | 591,244 | 12 | 4m 3s |  |
| dotnet-perf-skills | 5 | analyze-perf-issues | 831,137 | 12,185 | 748,513 | 14 | 4m 37s |  |


---

## Consistency Analysis

| Configuration | Score σ | Most Consistent Dim (σ) | Most Variable Dim (σ) |
|---|---|---|---|
| no-skills | 6.0 | Reflection and Serialization Overhead (0.0) | Structural Optimization Detection (0.8) |
| dotnet-perf-skills | 0.9 | Regex Anti-Pattern Detection (0.0) | Reflection and Serialization Overhead (0.5) |

---

## Per-Dimension Analysis

### 1. Regex Anti-Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 5 | 5 |
| 3 | 4 | 5 |
| 4 | 5 | 5 |
| 5 | 4 | 5 |
| **Mean** | **4.4** | **5.0** |

#### Analysis

**dotnet-perf-skills excerpt** (`output/dotnet-perf-skills/run-4/analyze-perf-issues/performance-analysis.md`):
> **Regex instantiation in hot loops** — `LogAnalyzer.TryParseLine` creates 2–3 `Regex` objects per log line (🔴 O(n) compilations)  
> **40+ `RegexOptions.Compiled`** — `MarkdownStripper` bloats JIT startup budget (🟡)  
> On .NET 7+ use `[GeneratedRegex]` source generators

**no-skills excerpt** (`output/no-skills/run-4/analyze-perf-issues/performance-analysis.md`):
> `new Regex()` allocated per log line (4 instances in hot path)  
> `RegexOptions.Compiled` | 48 (all in MarkdownStripper.cs)  
> `[GeneratedRegex]` | 0

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**. Both identify per-call regex allocation, excessive compiled regex use, and recommend `[GeneratedRegex]` on .NET 8.

**Verdict:** **Tie**; both outputs are excellent on regex coverage.

### 2. String Allocation Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 5 | 5 |
| 3 | 4 | 5 |
| 4 | 4 | 5 |
| 5 | 4 | 5 |
| **Mean** | **4.4** | **5.0** |

#### Analysis

**dotnet-perf-skills excerpt:**
> **O(n²) string concatenation** — `TemplateEngine`, `CsvParser`, `LogAnalyzer`, `DataPipeline` all use `+=` in loops  
> `.ToLower()` without culture — Turkish-I bug  
> Long chain of `.Replace()` calls — each allocates a new string

**no-skills excerpt:**
> `+=` string concatenation in loops — O(n²) allocation (6 instances)  
> Char-by-char `+=` string building in CsvParser (2 instances)  
> `.ToLower()` / `.ToUpper()` without culture (17 instances)

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**. Baseline is strong, but skill output is broader on replace-chain allocation detail and cross-file allocation patterns.

**Verdict:** **dotnet-perf-skills** is better due to deeper allocation-pattern coverage.

### 3. Collection and LINQ Efficiency [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 5 | 5 |
| 3 | 4 | 5 |
| 4 | 4 | 5 |
| 5 | 4 | 5 |
| **Mean** | **4.4** | **5.0** |

#### Analysis

**dotnet-perf-skills excerpt:**
> `existingSlugs.ToList()` then `.Contains()` in a `while` loop — O(n) per lookup. Use a `HashSet<string>`.  
> `ContainsKey` + indexer pattern — use `TryGetValue`  
> `.ToList()` + `Skip(i).Take(5).ToList()` inside a loop — O(n²) allocations

**no-skills excerpt:**
> `ContainsKey` + indexer double-lookup (8 instances)  
> `List.Contains()` O(n) lookups instead of HashSet (2 instances)  
> Missing collection capacity hints (5+ instances)

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**. Baseline hits core issues but misses some concrete hot-path LINQ materialization details present in skill output.

**Verdict:** **dotnet-perf-skills** wins on specificity and breadth.

### 4. Async and IO Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 5 | 5 |
| 3 | 4 | 5 |
| 4 | 4 | 5 |
| 5 | 5 | 5 |
| **Mean** | **4.6** | **5.0** |

#### Analysis

**dotnet-perf-skills excerpt:**
> **`new HttpClient()` per call** ... leads to socket exhaustion under load  
> Sequential `await` in `SendBatchAsync` loop — no parallelism  
> `SendBatchParallelAsync` — **unbounded parallelism**  
> `Task.Delay(_retryDelay)` ... without `CancellationToken`

**no-skills excerpt:**
> `new HttpClient()` per call — socket exhaustion risk  
> Sequential awaits in loop — no parallelism  
> Unbounded `Task.WhenAll` parallelism (1 instance)

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**. Both catch the major async/IO failures, but skill output adds explicit cancellation propagation gaps.

**Verdict:** **dotnet-perf-skills** is more complete on async robustness.

### 5. Reflection and Serialization Overhead [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 4 |
| 2 | 4 | 4 |
| 3 | 4 | 5 |
| 4 | 4 | 4 |
| 5 | 4 | 5 |
| **Mean** | **4.0** | **4.4** |

#### Analysis

**dotnet-perf-skills excerpt:**
> **`typeof(TTarget).GetProperties()` + `prop.SetValue(target, value)` per call** ... Cache `PropertyInfo[]`  
> **`new JsonSerializerOptions { WriteIndented = true }` per call** in `Merge` ... should be a `static readonly` field

**no-skills excerpt:**
> Uncached `new JsonSerializerOptions` per call (4 instances)  
> Uncached reflection `GetProperties()` / `GetProperty()` / `SetValue()` (3 call sites)

**Score:** dotnet-perf-skills **4/5**, no-skills **4/5**. Both cover key reflection/serializer issues well; neither strongly develops partial-deserialization alternatives.

**Verdict:** **Tie**; both are good and actionable.

### 6. Structural Optimization Detection [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 3 | 5 |
| 2 | 5 | 5 |
| 3 | 4 | 5 |
| 4 | 5 | 5 |
| 5 | 4 | 5 |
| **Mean** | **4.2** | **5.0** |

#### Analysis

**dotnet-perf-skills excerpt:**
> `Record` class is unsealed — JIT cannot devirtualize  
> `DeliveryResult` struct does not implement `IEquatable<DeliveryResult>`  
> Static `Dictionary` that never mutates — candidate for `FrozenDictionary` on .NET 8+

**no-skills excerpt:**
> Unsealed classes — 17 of 17 classes are unsealed  
> Structs without `IEquatable<T>` (2 of 2)  
> `static readonly Dictionary<>` — FrozenDictionary candidates (2 instances)

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**. Both provide complete structural findings aligned to the rubric.

**Verdict:** **Tie**; both are comprehensive.

### 7. Severity Classification Accuracy [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 3 | 5 |
| 2 | 4 | 4 |
| 3 | 5 | 4 |
| 4 | 3 | 5 |
| 5 | 4 | 4 |
| **Mean** | **3.8** | **4.4** |

#### Analysis

**dotnet-perf-skills excerpt:**
> Top priorities: `new HttpClient()` per call ...  
> `LogAnalyzer.TryParseLine` creates 2–3 `Regex` objects per log line ...  
> 40+ `RegexOptions.Compiled` ... startup budget (🟡)

**no-skills excerpt:**
> #### 7. `ContainsKey` + indexer double-lookup (8 instances)  
> **Impact:** ... ~2x slower per access. **Critical** in LogAnalyzer...

**Score:** dotnet-perf-skills **5/5**, no-skills **3/5**. Skill output consistently prioritizes production/hot-path failures above moderate inefficiencies; baseline over-elevates some medium-impact lookup patterns to critical.

**Verdict:** **dotnet-perf-skills** clearly leads on prioritization fidelity.

### 8. Fix Recommendation Quality [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 5 | 5 |
| 3 | 4 | 5 |
| 4 | 4 | 5 |
| 5 | 4 | 5 |
| **Mean** | **4.2** | **5.0** |

#### Analysis

**dotnet-perf-skills excerpt:**
> use `IHttpClientFactory` or a shared `static HttpClient`  
> Add `SemaphoreSlim` throttling to `SendBatchParallelAsync`  
> Convert 40+ `RegexOptions.Compiled` to `[GeneratedRegex]`

**no-skills excerpt:**
> Inject a shared `HttpClient` ... or use `IHttpClientFactory`  
> Convert all to `[GeneratedRegex]` partial methods  
> Replace with `TryGetValue`

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**. Both are actionable and API-specific; skill output is slightly stronger in consistency and fit to issue severity.

**Verdict:** **dotnet-perf-skills** provides the highest-quality fix guidance overall.

### 9. Token Efficiency [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 2 |
| 2 | 5 | 2 |
| 3 | 5 | 2 |
| 4 | 5 | 2 |
| 5 | 4 | 2 |
| **Mean** | **4.6** | **2.0** |

---

## Asset Usage Summary

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| no-skills | 1 | f39acac9…fc1c | claude-opus-4.6 | — | — | ✅ |
| no-skills | 2 | cf284294…d4d5 | claude-opus-4.6 | — | — | ✅ |
| no-skills | 3 | f96f996f…22d2 | claude-opus-4.6 | — | — | ✅ |
| no-skills | 4 | 334e5cff…18aa | claude-opus-4.6 | analyzing-dotnet-performance | dotnet-diag | ⚠️ Mismatch |
| no-skills | 5 | 334e5cff…18aa | claude-opus-4.6 | analyzing-dotnet-performance | dotnet-diag | ⚠️ Mismatch |
| dotnet-perf-skills | 1 | 48939371…6976 | claude-opus-4.6 | analyzing-dotnet-performance | dotnet-diag | ✅ |
| dotnet-perf-skills | 2 | bd7d0d92…fd7d | claude-opus-4.6 | analyzing-dotnet-performance | dotnet-diag | ✅ |
| dotnet-perf-skills | 3 | 21d36a5b…d892 | claude-opus-4.6 | analyzing-dotnet-performance | dotnet-diag | ✅ |
| dotnet-perf-skills | 4 | 556101cd…5816 | claude-opus-4.6 | analyzing-dotnet-performance | dotnet-diag | ✅ |
| dotnet-perf-skills | 5 | eb5baeb3…e06e | None | — | — | ✅ |

### ⚠️ Asset Notes

Some runs had missing expected skills or plugins. Review the session events.jsonl files for details.

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
