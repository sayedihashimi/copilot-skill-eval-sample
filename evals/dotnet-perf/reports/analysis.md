# Aggregated Analysis: .NET Performance Analysis Skill Evaluation

**Runs:** 3 | **Configurations:** 2 | **Scenarios:** 1 | **Dimensions:** 9
**Date:** 2026-04-11 05:42 UTC

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
| Reflection and Serialization Overhead [HIGH] | 4.0 | 4.7 ± 0.6 |
| Structural Optimization Detection [HIGH] | 3.3 ± 0.6 | 5.0 |
| Severity Classification Accuracy [HIGH] | 3.7 ± 0.6 | 3.3 ± 0.6 |
| Fix Recommendation Quality [HIGH] | 4.3 ± 0.6 | 5.0 |
| Token Efficiency [MEDIUM] | 4.7 ± 0.6 | 2.0 |

---

## Final Rankings

| Rank | Configuration | Mean Score | % of Max (100) | Std Dev | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | dotnet-perf-skills | 97.3 | 97% | 1.2 | 96.0 | 98.0 |
| 🥈 | no-skills | 85.3 | 85% | 5.9 | 81.0 | 92.0 |

---

## Weighted Score per Run

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 92.0 | 98.0 |
| 2 | 81.0 | 98.0 |
| 3 | 83.0 | 96.0 |
| **Mean** | **85.3** | **97.3** |

---

## Token Usage Summary

Average token consumption per configuration (1 outlier run(s) excluded from averages).

| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time | Δ Input vs Baseline |
|---|---|---|---|---|---|---|
| no-skills | 390,663 | 8,751 | 311,181 | 9 | 3m 16s | — (baseline) |
| dotnet-perf-skills | 907,671 | 15,158 | 687,004 | 14 | 5m 35s | +132.3% |

---

## Token Usage Per Run

| Configuration | Run | Scenario | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time | Note |
|---|---|---|---|---|---|---|---|---|
| no-skills | 1 | analyze-perf-issues | 401,881 | 9,644 | 300,057 | 9 | 3m 34s |  |
| no-skills | 2 | analyze-perf-issues | 388,835 | 8,854 | 320,068 | 9 | 3m 16s |  |
| no-skills | 3 | analyze-perf-issues | 381,274 | 7,756 | 313,418 | 9 | 2m 59s |  |
| dotnet-perf-skills | 1 | analyze-perf-issues | 941,955 | 16,993 | 694,483 | 14 | 6m 0s |  |
| dotnet-perf-skills | 2 | analyze-perf-issues | — | — | — | — | — | ⚠️ no usage data |
| dotnet-perf-skills | 3 | analyze-perf-issues | 873,387 | 13,323 | 679,524 | 13 | 5m 11s |  |


---

## Consistency Analysis

| Configuration | Score σ | Most Consistent Dim (σ) | Most Variable Dim (σ) |
|---|---|---|---|
| no-skills | 5.9 | Collection and LINQ Efficiency (0.0) | Regex Anti-Pattern Detection (0.6) |
| dotnet-perf-skills | 1.2 | Regex Anti-Pattern Detection (0.0) | Reflection and Serialization Overhead (0.6) |

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

**dotnet-perf-skills** (`performance-analysis.md`)
> `new Regex(` (uncached per-call) | **8**  
> `RegexOptions.Compiled` | **48**  
> `[GeneratedRegex]` | **0** | None used — 0 of 48+ static patterns use source generator

**no-skills** (`performance-analysis.md`)
> **`new Regex(...)` per log line in `TryParseLine`** ... for a 1M-line log, it creates 1-3 million regex objects.  
> **40+ `RegexOptions.Compiled` static instances** ... startup budget blown.  
> **Recommendation:** On .NET 8+, use `[GeneratedRegex]` for all static patterns.

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best here due to explicit counts and stronger completeness checks (`[GeneratedRegex]` coverage = 0).

### 2. String Allocation Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 4 | 5 |
| 3 | 4 | 5 |
| **Mean** | **4.3** | **5.0** |

#### Analysis

**dotnet-perf-skills** (`performance-analysis.md`)
> String `+=` Concatenation in Loops — O(n²) Allocation (6 sites)  
> `.ToLower()`/`.ToUpper()` Without Culture or StringComparison (17 instances)  
> Chained `.Replace()` Calls in Loop (SlugGenerator) (9 iterations)

**no-skills** (`performance-analysis.md`)
> **`current += line[i]` character-by-character string concatenation** ... O(n²) for long lines/files.  
> **Long chain of `.Replace()` calls in `StripMarkdown`** ... ~45 full-string allocations.  
> `.ToLower()` / `.ToUpper()` Without Culture ... locale-sensitive.

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is stronger on breadth and quantification; both identify the core allocation issues.

### 3. Collection and LINQ Efficiency [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 4 | 5 |
| 3 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

**dotnet-perf-skills** (`performance-analysis.md`)
> `ContainsKey` + Indexer Double-Lookup (~12 instances) ... **Fix:** Replace with `TryGetValue`.  
> `List.Contains()` for Lookups — O(n) per Check ... **Fix:** Use `HashSet<string>`.  
> `LogAnalyzer.DetectAnomalies` — `Skip(i).Take(5).ToList()` in Loop

**no-skills** (`performance-analysis.md`)
> **`allKeys` as `List<string>` with `.Contains()` ... O(n²) total. Fix: Use `HashSet<string>`**  
> **`ContainsKey` + indexer double-lookup** ... use `TryGetValue`.  
> **`Skip(i).Take(5).ToList()` in a loop** ... creates a new list on every iteration.

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** wins by covering more hot-path collection cases in one place (including broader materialization patterns).

### 4. Async and IO Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 4 | 5 |
| 3 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

**dotnet-perf-skills** (`performance-analysis.md`)
> `new HttpClient()` Per Call — Socket Exhaustion Risk (3 instances)  
> Sequential `await` in Loop — No Parallelism (1 instance)  
> Unbounded Parallelism in `SendBatchParallelAsync` ... Missing Cancellation Token ... `Task.Delay` can't be cancelled.

**no-skills** (`performance-analysis.md`)
> **`new HttpClient()` per call** ... causes **socket exhaustion**.  
> **Unbounded parallelism in `SendBatchParallelAsync`** ... spawns 10K concurrent HTTP requests.  
> **Sequential awaits in `SendBatchAsync`** ... `Task.Delay` without `CancellationToken`.

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is slightly better due to tighter prioritization and clearer impact framing, while both cover all required anti-patterns.

### 5. Reflection and Serialization Overhead [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 4 | 5 |
| 3 | 4 | 4 |
| **Mean** | **4.0** | **4.7** |

#### Analysis

**dotnet-perf-skills** (`performance-analysis.md`)
> Uncached `new JsonSerializerOptions` Per Call (4 instances) ... up to 592x slower than cached options  
> Uncached Reflection `GetProperties()`/`GetProperty()` in Hot Paths (3 instances)

**no-skills** (`performance-analysis.md`)
> **`new JsonSerializerOptions { WriteIndented = true }` on every call** ... use static readonly options.  
> **`typeof(TTarget).GetProperties()` on every call** ... cache `PropertyInfo[]` per type.

**Score:** dotnet-perf-skills **4/5**, no-skills **4/5**.  
**Verdict:** **Tie.** Both catch uncached reflection and serializer options well; neither goes deep on partial-deserialization alternatives (`Utf8JsonReader`) in this run.

### 6. Structural Optimization Detection [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 3 | 5 |
| 3 | 3 | 5 |
| **Mean** | **3.3** | **5.0** |

#### Analysis

**dotnet-perf-skills** (`performance-analysis.md`)
> `sealed class` | **0** ... Unsealed non-abstract non-static classes | **18**  
> `IEquatable` | **0** ... `public struct` (without IEquatable) | **2**  
> `static readonly Dictionary<` (FrozenDictionary candidate) | **2**

**no-skills** (`performance-analysis.md`)
> `ValidationError`, `DeliveryResult` structs lack `IEquatable<T>`.  
> `MappingConfig`, `ValidationResult`, `Record` classes are unsealed.  
> Static `Converters` dictionary could be `FrozenDictionary`.

**Score:** dotnet-perf-skills **5/5**, no-skills **3/5**.  
**Verdict:** **dotnet-perf-skills** is decisively better; it provides complete structural inventory and scale, while `no-skills` is more sample-based.

### 7. Severity Classification Accuracy [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 3 |
| 2 | 3 | 4 |
| 3 | 4 | 3 |
| **Mean** | **3.7** | **3.3** |

#### Analysis

**dotnet-perf-skills** (`performance-analysis.md`)
> 🔴 Critical | 8 | ... `new HttpClient()` ... uncached `new Regex()` ... `new JsonSerializerOptions` ... uncached reflection  
> #### 6. `ContainsKey` + Indexer Double-Lookup (~12 instances) ... **🔴 Critical**  
> #### 7. `List.Contains()` for Lookups ... **🔴 Critical**

**no-skills** (`performance-analysis.md`)
> **Top priorities:** (1) `new HttpClient` per call ... (2) Regex instantiation per log line ... (3) 40+ `RegexOptions.Compiled` ... (4) string `+=` in tight loops  
> `ContainsKey` + indexer double-lookup ... **ℹ️ Info** / **🟡 Moderate** by context

**Score:** dotnet-perf-skills **3/5**, no-skills **4/5**.  
**Verdict:** **no-skills** is better calibrated in this dimension; `dotnet-perf-skills` over-escalates some moderate collection issues to critical, which weakens prioritization fidelity.

### 8. Fix Recommendation Quality [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 4 | 5 |
| 3 | 4 | 5 |
| **Mean** | **4.3** | **5.0** |

#### Analysis

**dotnet-perf-skills** (`performance-analysis.md`)
> **Fix:** Inject `HttpClient` ... or use `IHttpClientFactory`.  
> **Fix:** Hoist to `static readonly` ... use `[GeneratedRegex]`.  
> **Fix:** Replace with `StringBuilder` / `TryGetValue` / `HashSet<string>` / `Parallel.ForEachAsync`.

**no-skills** (`performance-analysis.md`)
> **Fix:** Use `StringBuilder` or `Span<char>`.  
> **Fix:** Use `SemaphoreSlim` or `Parallel.ForEachAsync` with `MaxDegreeOfParallelism`.  
> **Fix:** `sealed` classes, `IEquatable<T>`, cache `JsonSerializerOptions`.

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** provides more consistently specific API-level remediations and stronger implementation patterns.

### 9. Token Efficiency [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 2 |
| 2 | 5 | — |
| 3 | 5 | 2 |
| **Mean** | **4.7** | **2.0** |

---

## Asset Usage Summary

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| no-skills | 1 | f09771d4…0f9d | claude-opus-4.6 | — | — | ✅ |
| no-skills | 2 | 148356d7…3a43 | claude-opus-4.6 | — | — | ✅ |
| no-skills | 3 | a91fcbea…021f | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills | 1 | 701e3e26…9b7a | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills | 2 | cef98178…f362 | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills | 3 | 452b8866…44f7 | claude-opus-4.6 | — | — | ✅ |

---

## Copilot Recommendations (Best Run)

Final recommendations from Copilot's highest-scoring run for each configuration.

### no-skills (run 1, score 92)

Done. Analyzed all 10 C# files and wrote `performance-analysis.md` with 62 findings:

- **7 Critical** — `new HttpClient()` per call (socket exhaustion), `new Regex()` per log line, O(n²) string concatenation in CSV parser and template engine
- **28 Moderate** — per-call regex allocations, uncached reflection, missing culture in `.ToLower()`, unbounded parallelism, sequential awaits
- **27 Info** — missing capacity hints, unsealed classes, structs without `IEquatable<T>`, `FrozenDictionary` candidates

Top fix: inject `IHttpClientFactory` in `NotificationService` to prevent socket exhaustion. Both output files are in `output/no-skills/run-1/analyze-perf-issues/`.

### dotnet-perf-skills (run 1, score 98)

Done. I analyzed all 10 `.cs` files in `perf01/` using the **`analyzing-dotnet-performance`** skill and produced:

- **`performance-analysis.md`** — Full report with 40 findings (8 🔴 Critical, 22 🟡 Moderate, 10 ℹ️ Info) across regex, strings, collections, async, reflection, serialization, and structural categories
- **`gen-notes.md`** — Documents skill usage and how it influenced the analysis

**Top critical findings:** `new HttpClient()` per call (socket exhaustion), uncached `new Regex()` in log parsing hot loops, uncached `JsonSerializerOptions` (up to 592× slower), `ContainsKey`+indexer double-lookups (12 sites), and string `+=` in loops (O(n²) across 8 methods).

---

## Raw Data References

- Per-run analysis: `reports/analysis-run-1.md`
- Per-run analysis: `reports/analysis-run-2.md`
- Per-run analysis: `reports/analysis-run-3.md`
- Verification data: `reports/verification-data.json`
- Score data: `reports/scores-data.json`
- Build notes: `reports/build-notes.md`
- Generation usage: `reports/generation-usage.json`
