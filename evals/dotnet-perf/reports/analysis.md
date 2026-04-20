# Aggregated Analysis: .NET Performance Analysis Skill Evaluation

**Runs:** 1 | **Configurations:** 1 | **Scenarios:** 1 | **Dimensions:** 12
**Date:** 2026-04-18 06:42 UTC

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
| dotnet-diag-auto-improve | dotnet/skills Performance Skills (improved) | — | dotnet-diag-auto-improve:dotnet-diag |

### How It Works

1. **Generate** — For each configuration, Copilot CLI (`copilot --yolo`) is given a scenario prompt and produces text output. One scenario is randomly selected per run.
2. **Analyze** — An AI judge reviews the text output of all configurations side-by-side and scores each across 12 quality dimensions.

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
| HIGH | ×2 | 7 |

**Maximum possible weighted score: 135.0** (all dimensions scoring 5).
Scores shown as **mean ± standard deviation** across runs.

---

## Executive Summary

Mean dimension scores across runs (1–5 scale, **higher is better**). ± values show standard deviation across runs.

| Dimension [Tier] | dotnet-diag-auto-improve |
|---|---|
| Regex Anti-Pattern Detection [CRITICAL] | 5.0 |
| String Allocation Detection [CRITICAL] | 5.0 |
| Collection and LINQ Efficiency [CRITICAL] | 5.0 |
| Async and IO Pattern Detection [CRITICAL] | 5.0 |
| Reflection and Serialization Overhead [HIGH] | 5.0 |
| Structural Optimization Detection [HIGH] | 5.0 |
| Aggregate and Replace Chain Detection [HIGH] | 5.0 |
| Span Usage Consistency [HIGH] | 5.0 |
| Inheritance Sealing Accuracy [HIGH] | 5.0 |
| Params Overload Optimization [MODERATE] | 5.0 |
| Severity Classification Accuracy [HIGH] | 4.0 |
| Fix Recommendation Quality [HIGH] | 5.0 |

---

## Final Rankings

Configurations ranked by mean weighted score — **higher is better**. Std Dev shows run-to-run variability (lower = more consistent).

| Rank | Configuration | Mean Score ↑ | % of Max (135) | Std Dev ↓ | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | dotnet-diag-auto-improve | 133.0 | 99% | 0.0 | 133.0 | 133.0 |

---

## Weighted Score per Run

| Run | dotnet-diag-auto-improve |
|---|---|
| 1 | 133.0 |
| **Mean** | **133.0** |

---

## Token Usage Summary

Average token consumption per configuration across all runs.

| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time |
|---|---|---|---|---|---|
| dotnet-diag-auto-improve | 880,139 | 14,626 | 501,118 | 13 | 15m 19s |

---

## Token Usage Per Run

| Configuration | Run | Scenario | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time | Note |
|---|---|---|---|---|---|---|---|---|
| dotnet-diag-auto-improve | 1 | analyze-perf-issues | 880,139 | 14,626 | 501,118 | 13 | 15m 19s |  |


---

## Per-Dimension Analysis

### 1. Regex Anti-Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | dotnet-diag-auto-improve |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

**dotnet-diag-auto-improve** (`output/dotnet-diag-auto-improve/run-1/analyze-perf-issues/performance-analysis.md`):
> | `RegexOptions.Compiled` | 48 (all in MarkdownStripper) |
> | `[GeneratedRegex]` | 0 |
> | `new Regex(` (uncached, per-call) | 8 (...) |
> **Fix:** Hoist to `private static readonly Regex` fields or use `[GeneratedRegex]` (preferred on .NET 8).

**Score:** dotnet-diag-auto-improve = **5/5** (detects per-call instantiation, compiled overuse, and gives .NET 8+ GeneratedRegex guidance).  
**Verdict:** **dotnet-diag-auto-improve** is comprehensive and precise.

### 2. String Allocation Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | dotnet-diag-auto-improve |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

**dotnet-diag-auto-improve**:
> #### 6. String Concatenation `+=` in Loops — O(n²) Allocation (7 sites)
> #### 9. `.ToLower()`/`.ToUpper()` Without Culture or `StringComparison` (17 instances)
> #### 12. Sequential `.Replace()` Chain in SlugGenerator (...)
> **Fix:** Use `StringBuilder` ... `StringComparison.OrdinalIgnoreCase` ... `ToLowerInvariant()`.

**Score:** dotnet-diag-auto-improve = **5/5** (covers all requested string categories with counts and fixes).  
**Verdict:** **dotnet-diag-auto-improve** clearly addresses high-impact string allocation issues.

### 3. Collection and LINQ Efficiency [CRITICAL × 3]

#### Scores Across Runs

| Run | dotnet-diag-auto-improve |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

**dotnet-diag-auto-improve**:
> #### 7. `ContainsKey` + Indexer Double-Lookup (10+ sites)
> #### 16. `.ToList()` + `.Contains()` for Key Lookups — O(n²) (2 instances)
> #### 17. `Skip(i).Take(5).ToList()` in Loop — Sliding Window Allocation (1 instance)
> #### 18. `Distinct().ToList()` Allocation (1 instance)

**Score:** dotnet-diag-auto-improve = **5/5** (captures the full LINQ/collection hot-path set, including explicit Distinct/Skip+Take counts).  
**Verdict:** **dotnet-diag-auto-improve** provides excellent collection/LINQ coverage.

### 4. Async and IO Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | dotnet-diag-auto-improve |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

**dotnet-diag-auto-improve**:
> #### 1. `new HttpClient()` Per Call — Socket Exhaustion (3 instances)
> #### 19. Sequential `await` in Loop — No Parallelism
> #### 20. Unbounded Parallelism in `SendBatchParallelAsync`
> #### 21. Missing Cancellation Tokens in Async Methods

**Score:** dotnet-diag-auto-improve = **5/5** (identifies all required async/IO anti-patterns and includes mitigation patterns).  
**Verdict:** **dotnet-diag-auto-improve** is strong and production-relevant in async/IO analysis.

### 5. Reflection and Serialization Overhead [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-diag-auto-improve |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

**dotnet-diag-auto-improve**:
> #### 2. Uncached `new JsonSerializerOptions` Per Call (5 instances)
> #### 8. Uncached Reflection — `GetProperties()`/`GetProperty()`/`SetValue()` Per Call
> - Full `Deserialize*` hot-path hits: **4**
> - `Utf8JsonReader`/`JsonDocument` usage sites: **0** — partial parsing ... is a valid optimization

**Score:** dotnet-diag-auto-improve = **5/5** (matches all reflection/serialization targets, including partial-parse guidance).  
**Verdict:** **dotnet-diag-auto-improve** is complete and actionable here.

### 6. Structural Optimization Detection [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-diag-auto-improve |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

**dotnet-diag-auto-improve**:
> #### 23. Unsealed Leaf Classes — 0 of 26 Non-Abstract Classes Are Sealed
> #### 24. Structs Without `IEquatable<T>` — 0 of 3 Structs Implement It
> #### 27. `static readonly Dictionary<>` — FrozenDictionary Candidates (3 instances)

**Score:** dotnet-diag-auto-improve = **5/5** (covers sealing, struct equality, and FrozenDictionary opportunities).  
**Verdict:** **dotnet-diag-auto-improve** provides strong structural optimization detection.

### 7. Aggregate and Replace Chain Detection [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-diag-auto-improve |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

**dotnet-diag-auto-improve**:
> #### 11. `.Aggregate()` with `.Replace()` — 16 Intermediate String Allocations
> #### 22. `char.ToString()` Allocation in Loop
> **Files:** UnitFormatter.cs:L60-L66 ... UnitFormatter.cs:L64

**Score:** dotnet-diag-auto-improve = **5/5** (explicitly catches both aggregate-replace chaining and per-iteration `char.ToString()` allocations).  
**Verdict:** **dotnet-diag-auto-improve** handles this subtle pattern very well.

### 8. Span Usage Consistency [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-diag-auto-improve |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

**dotnet-diag-auto-improve**:
> #### 14. `value[..n].TrimEnd()` Double Allocation
> #### 15. Cross-File Inconsistency: `Substring` vs `AsSpan` in Truncators
> #### 13. `List<char>` Where `ReadOnlySpan<char>` or String Would Suffice

**Score:** dotnet-diag-auto-improve = **5/5** (detects double-allocation, inconsistent Span adoption, and static char-set storage inefficiency).  
**Verdict:** **dotnet-diag-auto-improve** is comprehensive on Span-related consistency issues.

### 9. Inheritance Sealing Accuracy [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-diag-auto-improve |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

**dotnet-diag-auto-improve**:
> **Fix:** Add `sealed` keyword to all leaf classes. Leave `Ordinalizer` and `DefaultOrdinalizer` unsealed (they are base classes).
> - Did **not** suggest sealing `Ordinalizer` or `DefaultOrdinalizer` (they are base classes with subclasses)

**Score:** dotnet-diag-auto-improve = **5/5** (correctly identifies leaf classes while avoiding base-class false positives).  
**Verdict:** **dotnet-diag-auto-improve** shows high precision in inheritance-aware sealing advice.

### 10. Params Overload Optimization [MODERATE × 1]

#### Scores Across Runs

| Run | dotnet-diag-auto-improve |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

**dotnet-diag-auto-improve**:
> #### 26. `params` Without Single-Argument Fast-Path Overloads (3 methods)
> **Impact:** Always allocates a `params` array even for the common 1-argument case.
> **Fix:** Add 1-argument and 2-argument overloads ...

**Score:** dotnet-diag-auto-improve = **5/5** (finds exactly the expected params allocation pattern and recommends the standard fix).  
**Verdict:** **dotnet-diag-auto-improve** fully satisfies this dimension.

### 11. Severity Classification Accuracy [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-diag-auto-improve |
|---|---|
| 1 | 4 |
| **Mean** | **4.0** |

#### Analysis

**dotnet-diag-auto-improve**:
> | 🔴 Critical | 8 | `new HttpClient()` per call ... per-line `new Regex()` ... uncached `JsonSerializerOptions` |
> | 🟡 Moderate | 14 | `+=` ... `RegexOptions.Compiled` ... `ContainsKey` + indexer ... |
> | ℹ️ Info | 8 | Missing `sealed` ... `List` without capacity hints ... `params` ... |

**Score:** dotnet-diag-auto-improve = **4/5** (mostly well-prioritized with strong hot-path emphasis; minor over-severity risk where `ContainsKey+indexer` is promoted to critical in some contexts).  
**Verdict:** **dotnet-diag-auto-improve** is strong overall, with small prioritization calibration room.

### 12. Fix Recommendation Quality [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-diag-auto-improve |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

**dotnet-diag-auto-improve**:
> **Fix:** Inject `IHttpClientFactory` or use a single `static readonly HttpClient`...
> **Fix:** ... use `[GeneratedRegex]` ...
> **Fix:** ... `StringComparison.OrdinalIgnoreCase` ...
> **Fix:** ... `.ToFrozenDictionary()` ... `using System.Collections.Frozen;`

**Score:** dotnet-diag-auto-improve = **5/5** (specific APIs, concrete patterns, and no unsafe/incorrect recommendations).  
**Verdict:** **dotnet-diag-auto-improve** delivers highly actionable, technically correct fixes.

---

## Asset Usage Summary

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| dotnet-diag-auto-improve | 1 | 55a1d57d…31ca | claude-opus-4.6 | — | — | ✅ |

---

## Copilot Recommendations (Best Run)

Prioritized recommendations extracted from the highest-scoring run's output for each configuration.

### dotnet-diag-auto-improve (run 1, score 133)

## Prioritized Fix Recommendations

| # | Fix | Impact | Effort | Files |
|---|-----|--------|--------|-------|
| 1 | Replace `new HttpClient()` with `IHttpClientFactory` or static instance | 🔴 Socket exhaustion prevention | Moderate | NotificationService.cs |
| 2 | Cache `JsonSerializerOptions` as static readonly | 🔴 Up to 592x speedup | Quick-fix | JsonTransformer.cs, NotificationService.cs |
| 3 | Hoist `new Regex()` to static fields or `[GeneratedRegex]` | 🔴 >10x in LogAnalyzer hot path | Moderate | LogAnalyzer.cs, TemplateEngine.cs, ValidationEngine.cs, CsvParser.cs |
| 4 | Convert 48 `RegexOptions.Compiled` to `[GeneratedRegex]` | 🟡 Faster startup + throughput | Significant | MarkdownStripper.cs |
| 5 | Replace `+=` string concat in loops with `StringBuilder` | 🔴 O(n²)→O(n) | Moderate | 5 files, 7 sites |
| 6 | Replace `ContainsKey`+indexer with `TryGetValue` | 🔴 ~2x faster per lookup | Quick-fix | 7 files, 14 sites |
| 7 | Replace `.ToLower()`/`.ToUpper()` with ordinal comparisons | 🟡 2-3x faster + correctness | Moderate | 6 files, 17 sites |
| 8 | Cache reflection `GetProperties()` per type | 🔴 Orders of magnitude for batch ops | Moderate | EntityMapper.cs |
| 9 | Use `HashSet` instead of `List.Contains()` for lookups | 🟡 O(n)→O(1) | Quick-fix | SlugGenerator.cs, JsonTransformer.cs |
| 10 | Add `sealed` to ~20 leaf classes | ℹ️ JIT devirtualization | Quick-fix | All files |

> ⚠️ **Disclaimer:** These results are generated by an AI assistant and are non-deterministic. Findings may include false positives, miss real issues, or suggest changes that are incorrect for your specific context. Always verify recommendations with benchmarks and human review before applying changes to production code.

---

## Raw Data References

- Per-run analysis: `reports/analysis-run-1.md`
- Score data: `reports/scores-data.json`
- Generation usage: `reports/generation-usage.json`
