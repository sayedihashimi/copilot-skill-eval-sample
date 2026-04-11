# Aggregated Analysis: .NET Performance Analysis Skill Evaluation

**Runs:** 1 | **Configurations:** 2 | **Scenarios:** 1 | **Dimensions:** 9
**Date:** 2026-04-11 04:46 UTC

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

Generation model: **—**
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
| Regex Anti-Pattern Detection [CRITICAL] | 4.0 | 5.0 |
| String Allocation Detection [CRITICAL] | 4.0 | 5.0 |
| Collection and LINQ Efficiency [CRITICAL] | 4.0 | 5.0 |
| Async and IO Pattern Detection [CRITICAL] | 4.0 | 5.0 |
| Reflection and Serialization Overhead [HIGH] | 3.0 | 4.0 |
| Structural Optimization Detection [HIGH] | 4.0 | 5.0 |
| Severity Classification Accuracy [HIGH] | 3.0 | 5.0 |
| Fix Recommendation Quality [HIGH] | 4.0 | 5.0 |
| Token Efficiency [MEDIUM] | 5.0 | 2.0 |

---

## Final Rankings

| Rank | Configuration | Mean Score | % of Max (100) | Std Dev | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | dotnet-perf-skills | 100.0 | 100% | 0.0 | 100.0 | 100.0 |
| 🥈 | no-skills | 81.0 | 81% | 0.0 | 81.0 | 81.0 |

---

## Weighted Score per Run

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 81.0 | 100.0 |
| **Mean** | **81.0** | **100.0** |

---

## Token Usage Summary

Average token consumption per configuration across all runs.

| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time | Δ Input vs Baseline |
|---|---|---|---|---|---|---|
| no-skills | 412,742 | 9,735 | 304,919 | 9 | 4m 6s | — (baseline) |
| dotnet-perf-skills | 829,055 | 12,995 | 571,897 | 12 | 4m 23s | +100.9% |

---

## Token Usage Per Run

| Configuration | Run | Scenario | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time | Note |
|---|---|---|---|---|---|---|---|---|
| no-skills | 1 | analyze-perf-issues | 412,742 | 9,735 | 304,919 | 9 | 4m 6s |  |
| dotnet-perf-skills | 1 | analyze-perf-issues | 829,055 | 12,995 | 571,897 | 12 | 4m 23s |  |


---

## Per-Dimension Analysis

### 1. Regex Anti-Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

**dotnet-perf-skills excerpt**  
> **dotnet-perf-skills / performance-analysis.md**  
> "`new Regex(` (uncached per-call) | 8"  
> "`RegexOptions.Compiled` | 48"  
> "Fix: ... preferably `[GeneratedRegex]` ... .NET 8."  
> "48 `RegexOptions.Compiled` ... adding ~50-100ms to cold start."

**no-skills excerpt**  
> **no-skills / performance-analysis.md**  
> "`new Regex(...)` on every log line in `TryParseLine`"  
> "`MarkdownStripper` has 45+ compiled regexes ... should use `[GeneratedRegex]`"  
> "Three distinct sub-patterns: Per-call `new Regex()`, static `Regex.Replace()`, excessive `RegexOptions.Compiled`."

**Scores:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
dotnet-perf-skills is more complete and quantified (explicit 48 count + startup budget framing); baseline catches the same classes but with less rigor/precision.

**Verdict:** **dotnet-perf-skills** is best due to stronger quantification and clearer .NET 8 `[GeneratedRegex]` prioritization.

### 2. String Allocation Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

**dotnet-perf-skills excerpt**  
> **dotnet-perf-skills / performance-analysis.md**  
> "String `+=` concatenation in loops — O(n²) allocation (6 sites)"  
> "`.ToLower()` / `.ToUpper()` without culture ... 18 instances"  
> "`MarkdownStripper.StripMarkdown` — 47 chained `.Replace()` allocations per call"

**no-skills excerpt**  
> **no-skills / performance-analysis.md**  
> "Char-by-char `string +=` in `ParseLine` ... O(n²)."  
> "`.ToLower()` is culture-sensitive and allocates."  
> "`45+` chained `.Replace()` calls in `StripMarkdown` ... allocates a new string each pass."

**Scores:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
Both catch core issues; skill-guided output gives broader coverage and stronger cross-file synthesis.

**Verdict:** **dotnet-perf-skills**.

### 3. Collection and LINQ Efficiency [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

**dotnet-perf-skills excerpt**  
> **dotnet-perf-skills / performance-analysis.md**  
> "`ContainsKey` + indexer double-lookup (12 instances)"  
> "`ToList()` + `List.Contains()` ... O(n) per lookup"  
> "`Skip(i).Take(5).ToList()` in a loop — O(n²) LINQ"  
> "`List<T>` / `Dictionary<T>` without capacity hints"

**no-skills excerpt**  
> **no-skills / performance-analysis.md**  
> "`allKeys.ToList()` + `.Contains()` ... Should use `HashSet<string>`."  
> "`ContainsKey` + indexer ... Use `TryGetValue`."  
> "`.Distinct().ToList()` ... Could use a `HashSet<string>` from the start."  
> "`errorEntries.Skip(i).Take(5).ToList()` ... O(n²) total."

**Scores:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
Baseline coverage is good; skill-guided output is tighter in severity and breadth.

**Verdict:** **dotnet-perf-skills**.

### 4. Async and IO Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

**dotnet-perf-skills excerpt**  
> **dotnet-perf-skills / performance-analysis.md**  
> "`new HttpClient()` per call — socket exhaustion risk (3 instances)"  
> "Sequential `await` in batch loop"  
> "Unbounded parallelism in `SendBatchParallelAsync`"  
> "Missing `CancellationToken` on async methods (all async methods)"

**no-skills excerpt**  
> **no-skills / performance-analysis.md**  
> "`new HttpClient()` per call ... production incident risk."  
> "Sequential `await` in `SendBatchAsync` loop"  
> "Unbounded parallelism ... 10K concurrent HTTP calls"  
> "`Task.Delay` without `CancellationToken` in retry loop."

**Scores:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
Both are strong; skill-guided output better integrates cancellation and prioritization context.

**Verdict:** **dotnet-perf-skills**.

### 5. Reflection and Serialization Overhead [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 3 | 4 |
| **Mean** | **3.0** | **4.0** |

#### Analysis

**dotnet-perf-skills excerpt**  
> **dotnet-perf-skills / performance-analysis.md**  
> "Uncached `new JsonSerializerOptions` per call ... Up to 592x slower than cached options."  
> "Uncached reflection ... `GetProperties()`/`GetProperty()`/`SetValue()`/`GetValue()`."

**no-skills excerpt**  
> **no-skills / performance-analysis.md**  
> "`new JsonSerializerOptions` on every call ... expensive."  
> "`GetProperties()` + `SetValue()` on every call ... Reflection is ~100x slower."

**Scores:** dotnet-perf-skills **4/5**, no-skills **3/5**.  
Both identify key anti-patterns; neither substantially develops partial parsing (`Utf8JsonReader`) alternatives. Skill-guided output is better quantified.

**Verdict:** **dotnet-perf-skills**.

### 6. Structural Optimization Detection [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

**dotnet-perf-skills excerpt**  
> **dotnet-perf-skills / performance-analysis.md**  
> "Unsealed leaf classes — 0 of 18 classes are sealed"  
> "Structs without `IEquatable<T>` (2 of 2 structs)"  
> "`static readonly Dictionary<>` — FrozenDictionary candidates (2 instances)"

**no-skills excerpt**  
> **no-skills / performance-analysis.md**  
> "Missing `IEquatable<T>` on Structs (2 files)"  
> "Unsealed Leaf Classes (3 files)"  
> "Static `Dictionary` ... candidate for `FrozenDictionary`."

**Scores:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
Baseline detects all structural themes but with less complete class-level accounting than skill-guided output.

**Verdict:** **dotnet-perf-skills**.

### 7. Severity Classification Accuracy [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 3 | 5 |
| **Mean** | **3.0** | **5.0** |

#### Analysis

**dotnet-perf-skills excerpt**  
> **dotnet-perf-skills / performance-analysis.md**  
> "🔴 Critical ... `new HttpClient()` per call, uncached `new Regex()` in hot loops, `new JsonSerializerOptions` per call"  
> "Top Priorities: ... per-line/per-call methods ... convert 48 `Compiled` regexes ..."

**no-skills excerpt**  
> **no-skills / performance-analysis.md**  
> "| 🔴 Critical | 6 | Socket exhaustion, O(n²) hot-path allocations, regex per-line instantiation |"  
> "| 🟡 Moderate | 28 | ... missing caching, sequential awaits |"  
> "`new JsonSerializerOptions` on every call" (marked moderate)

**Scores:** dotnet-perf-skills **5/5**, no-skills **3/5**.  
dotnet-perf-skills separates critical hot-path issues more consistently; baseline is useful but flattens severity in places (notably serializer options and broader startup-budget framing).

**Verdict:** **dotnet-perf-skills**.

### 8. Fix Recommendation Quality [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

**dotnet-perf-skills excerpt**  
> **dotnet-perf-skills / performance-analysis.md**  
> "Inject `IHttpClientFactory` or use ... `SocketsHttpHandler { PooledConnectionLifetime ... }`"  
> "Use `StringComparison.OrdinalIgnoreCase` ..."  
> "Convert to `[GeneratedRegex]` ... Make the class `partial`."  
> "Use `Parallel.ForEachAsync` with `MaxDegreeOfParallelism`."

**no-skills excerpt**  
> **no-skills / performance-analysis.md**  
> "After — inject ... `IHttpClientFactory`"  
> "Use `SemaphoreSlim` or `Parallel.ForEachAsync`"  
> "Cache `GetProperties()` result per type"  
> "Use `HashSet<string>`."

**Scores:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
Both are actionable; skill-guided recommendations are more consistently specific and API-precise across all categories.

**Verdict:** **dotnet-perf-skills**.

### 9. Token Efficiency [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 2 |
| **Mean** | **5.0** | **2.0** |

---

## Raw Data References

- Per-run analysis: `reports/analysis-run-1.md`
- Verification data: `reports/verification-data.json`
- Score data: `reports/scores-data.json`
- Build notes: `reports/build-notes.md`
- Generation usage: `reports/generation-usage.json`
