# Aggregated Analysis: .NET Performance Analysis Skill Evaluation

**Runs:** 5 | **Configurations:** 2 | **Scenarios:** 1 | **Dimensions:** 14
**Date:** 2026-04-10 15:22 UTC

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
| Token Efficiency [MEDIUM] | 4.2 ± 1.8 | 4.6 ± 0.9 |
| Evidence & Quantification Rigor [MEDIUM] | 4.0 | 5.0 |
| Signal-to-Noise / Report Focus [MEDIUM] | 2.0 | 5.0 |
| Signal-to-Noise / Prioritization Focus [MEDIUM] | 3.0 | 5.0 |
| Evidence Quantification and Scan Rigor [MEDIUM] | 3.0 | 5.0 |

---

## Final Rankings

| Rank | Configuration | Mean Score | % of Max (100) | Std Dev | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | dotnet-perf-skills | 106.6 | 107% | 3.0 | 103.0 | 110.0 |
| 🥈 | no-skills | 92.6 | 93% | 6.4 | 85.0 | 100.0 |

---

## Weighted Score per Run

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 100.0 | 103.0 |
| 2 | 87.0 | 104.0 |
| 3 | 94.0 | 108.0 |
| 4 | 97.0 | 110.0 |
| 5 | 85.0 | 108.0 |
| **Mean** | **92.6** | **106.6** |

---

## Token Usage Summary

Average token consumption per configuration across all runs.

| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time | Δ Input vs Baseline |
|---|---|---|---|---|---|---|
| no-skills | 1,624,783 | 20,832 | 1,528,556 | 23 | 4m 5s | — (baseline) |
| dotnet-perf-skills | 1,167,529 | 14,953 | 931,312 | 23 | 4m 58s | -28.1% |

---

## Token Usage Per Run

| Configuration | Run | Scenario | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time |
|---|---|---|---|---|---|---|---|
| no-skills | 1 | analyze-perf-issues | 407,279 | 9,754 | 304,911 | 9 | 3m 55s |
| no-skills | 2 | analyze-perf-issues | 394,687 | 9,815 | 319,631 | 9 | 4m 8s |
| no-skills | 3 | analyze-perf-issues | 390,326 | 10,300 | 320,025 | 9 | 4m 8s |
| no-skills | 4 | analyze-perf-issues | 398,155 | 10,884 | 322,054 | 9 | 3m 50s |
| no-skills | 5 | analyze-perf-issues | 6,533,468 | 63,406 | 6,376,159 | 77 | 4m 25s |
| dotnet-perf-skills | 1 | analyze-perf-issues | 2,232,672 | 16,277 | 1,965,336 | 66 | 5m 14s |
| dotnet-perf-skills | 2 | analyze-perf-issues | 818,873 | 14,394 | 593,722 | 12 | 4m 42s |
| dotnet-perf-skills | 3 | analyze-perf-issues | 934,185 | 14,811 | 710,699 | 13 | 4m 59s |
| dotnet-perf-skills | 4 | analyze-perf-issues | 924,344 | 13,907 | 692,262 | 13 | 4m 42s |
| dotnet-perf-skills | 5 | analyze-perf-issues | 927,572 | 15,377 | 694,541 | 13 | 5m 14s |

---

## Consistency Analysis

| Configuration | Score σ | Most Consistent Dim (σ) | Most Variable Dim (σ) |
|---|---|---|---|
| no-skills | 6.4 | Fix Recommendation Quality (0.0) | Token Efficiency (1.8) |
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

**dotnet-perf-skills excerpt**
> **Top priorities:** ... per-call `new Regex()` in line-by-line parsing ... and 48 `RegexOptions.Compiled` instances that should use `[GeneratedRegex]` on .NET 8.  
> #### 3. `new Regex()` in per-line hot path (8 instances)  
> **Impact:** `TryParseLine` in LogAnalyzer is called per log line — potentially millions of times.

**no-skills excerpt**
> 2. **Regex per-line allocation** in `LogAnalyzer.TryParseLine` — creates 2-3 `Regex` objects per log line in a hot parse loop  
> 5. **40+ `RegexOptions.Compiled`** static fields in `MarkdownStripper` — excessive JIT startup cost  
> | 31 | 🟡 Moderate | 13-59 | **46 `RegexOptions.Compiled` static fields** ... should use `[GeneratedRegex]` |

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best because it is complete and more consistent (counts and severity framing are tighter, including explicit caveat for dynamic patterns).

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

**dotnet-perf-skills excerpt**
> #### 7. String `+=` concatenation in loops — O(n²) allocation (7 sites)  
> #### 8. `.ToLower()`/`.ToUpper()` without culture (19 instances)  
> **Fix:** Use `StringComparison.OrdinalIgnoreCase` comparisons or `ToLowerInvariant()`.

**no-skills excerpt**
> 3. **O(n²) string concatenation** — found in 8+ methods across all files (`+=` in loops)  
> | 39 | 🔴 Critical | 51-77 | **Character-by-character string `+=` in `ParseLine`** — O(n²) ...  
> | 27 | 🟡 Moderate | 33-36 | **Sequential `.Replace()` chain** — 9 string allocations per call |

**Score:** dotnet-perf-skills **5/5**; no-skills **5/5**.  
**Verdict:** **Tie**. Both catch loop concatenation, casing allocations/correctness, and replace-chain allocation patterns.

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

**dotnet-perf-skills excerpt**
> #### 6. `ContainsKey` + indexer double-lookup (10 instances)  
> #### 12. `List.Contains()` O(n) lookups in loops (2 sites)  
> ... `Skip(i).Take(5).ToList()` inside a loop creates a new list per iteration.

**no-skills excerpt**
> | 28 | 🟡 Moderate | 75-81 | **`.ToList()` + `.Contains()` ... Should use `HashSet<string>`.**  
> | 35 | 🟡 Moderate | 85-89 | **`.ToList()` + `.Contains()` for key deduplication** ... O(n²)  
> | 61 | 🟡 Moderate | 157 | **`Skip(i).Take(5).ToList()`** in a loop — O(n²) sliding window ...

**Score:** dotnet-perf-skills **5/5**; no-skills **5/5**.  
**Verdict:** **Tie**. Both cover the requested high-impact collection/LINQ anti-patterns with concrete alternatives (`TryGetValue`, `HashSet`, avoid materialization).

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

**dotnet-perf-skills excerpt**
> #### 1. `new HttpClient()` per call — socket exhaustion (3 instances)  
> #### 13. Sequential awaits in loop — no parallelism (1 site)  
> #### 14. Unbounded parallelism — all tasks fired at once (1 site)

**no-skills excerpt**
> | 45 | 🔴 Critical | 163, 179, 191 | **`new HttpClient()` per call** ... causes socket exhaustion under load. |  
> | 46 | 🔴 Critical | 117 | **Sequential `await` in loop** in `SendBatchAsync` ... |  
> | 48 | 🟡 Moderate | 102 | **`Task.Delay` without `CancellationToken`** ... |

**Score:** dotnet-perf-skills **5/5**; no-skills **5/5**.  
**Verdict:** **Tie**. Both identify all required async/IO risks and actionable mitigation patterns.

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

**dotnet-perf-skills excerpt**
> #### 2. Uncached `new JsonSerializerOptions` per call (5 instances)  
> #### 16. Uncached reflection `GetProperties()`/`SetValue()`/`GetValue()` (6 instances)  
> **Fix:** Cache `PropertyInfo[]` per type in a `ConcurrentDictionary<...>`.

**no-skills excerpt**
> | 34 | 🟡 Moderate | 74, 117, 135, 142 | **`new JsonSerializerOptions` per call** ... |  
> | 7 | 🔴 Critical | 77 | **`GetProperties()` via reflection on every `MapTo<T>` call** ... |  
> | 8 | 🔴 Critical | 101 | **`prop.SetValue()` via reflection per property** ... |

**Score:** dotnet-perf-skills **4/5**; no-skills **4/5**.  
**Verdict:** **Tie**. Both are strong on reflection and serializer-options caching; neither goes deeper into partial parsing (`Utf8JsonReader`) opportunities.

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

**dotnet-perf-skills excerpt**
> #### 9. Unsealed classes — 18 of 18 classes (0 sealed)  
> #### 10. Structs without `IEquatable<T>` (2 instances)  
> #### 11. Static `Dictionary` — FrozenDictionary candidates (2 of 2, 0 optimized)

**no-skills excerpt**
> | 5 | ℹ️ Info | 24 | **Unsealed class `ValidationResult`** ... |  
> | 4 | ℹ️ Info | 11-20 | **Struct `ValidationError` without `IEquatable<T>`** ... |  
> | 13 | ℹ️ Info | 10 | **Static `Dictionary` could be `FrozenDictionary`** ... |

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is better due to systematic aggregation and stronger structural prioritization across the codebase.

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

**dotnet-perf-skills excerpt**
> | 🔴 Critical | 8 | ... `new HttpClient()` per call ... per-call `new Regex()` in hot loops |  
> | 🟡 Moderate | 10 | 18/18 classes unsealed, uncached reflection, unbounded parallelism ... |  
> | Rank | Finding | Severity | Effort | Impact |

**no-skills excerpt**
> **Total issues found: 68**  
> | 🔴 Critical | 8 |  
> | 🟡 Moderate | 30 |  
> | ℹ️ Info | 30 |

**Score:** dotnet-perf-skills **5/5**; no-skills **3/5**.  
**Verdict:** **dotnet-perf-skills** is best: hot-path issues are clearly elevated and prioritization is cleaner. `no-skills` dilutes signal with many low-impact findings and less consistent severity pressure (e.g., very large `RegexOptions.Compiled` cluster kept moderate).

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

**dotnet-perf-skills excerpt**
> **Fix:** Inject `IHttpClientFactory` or use a `static readonly HttpClient` with `PooledConnectionLifetime`.  
> **Fix:** Convert all to `[GeneratedRegex]` with `partial` class.  
> **Caveat:** ... dynamic pattern ... cannot use `[GeneratedRegex]`.

**no-skills excerpt**
> // Issue 45: Use IHttpClientFactory or a single static instance  
> // Issue 35: Use HashSet for key deduplication  
> | 8 | **Replace `.ToLower()`/`.ToUpper()` with `StringComparison.OrdinalIgnoreCase`** ... |

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** provides more consistently high-quality recommendations, especially where caveats and API choices matter.

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

### 10. Token Efficiency [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 3 |
| 2 | 5 | 5 |
| 3 | 5 | 5 |
| 4 | 5 | 5 |
| 5 | 1 | 5 |
| **Mean** | **4.2** | **4.6** |

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

#### Analysis

**dotnet-perf-skills excerpt**
> **Total issues found: 22 findings** across 10 files spanning all 8 scanned categories.  
> **Top priorities:** ...  
> ## Prioritized Fix Recommendations

**no-skills excerpt**
> **Total issues found: 68**  
> ## Findings by File  
> ... (70 numbered findings with many low-impact entries)

**Score:** dotnet-perf-skills **5/5**; no-skills **2/5**.  
**Verdict:** **dotnet-perf-skills** is substantially more consumable for engineering prioritization; `no-skills` is exhaustive but noisy.

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
