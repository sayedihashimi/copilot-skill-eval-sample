# Aggregated Analysis: .NET Performance Analysis Skill Evaluation

**Runs:** 5 | **Configurations:** 2 | **Scenarios:** 1 | **Dimensions:** 13
**Date:** 2026-04-28 16:41 UTC

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
2. **Analyze** — An AI judge reviews the text output of all configurations side-by-side and scores each across 13 quality dimensions.

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
| MEDIUM | ×1 | 1 |

**Maximum possible weighted score: 140.0** (all dimensions scoring 5).
Scores shown as **mean ± standard deviation** across runs.

---

## Executive Summary

Mean dimension scores across runs (1–5 scale, **higher is better**). ± values show standard deviation across runs.

| Dimension [Tier] | no-skills | dotnet-perf-skills |
|---|---|---|
| Regex Anti-Pattern Detection [CRITICAL] | 4.4 ± 0.5 | 5.0 |
| String Allocation Detection [CRITICAL] | 4.6 ± 0.5 | 5.0 |
| Collection and LINQ Efficiency [CRITICAL] | 4.4 ± 0.5 | 5.0 |
| Async and IO Pattern Detection [CRITICAL] | 4.6 ± 0.5 | 5.0 |
| Reflection and Serialization Overhead [HIGH] | 4.0 | 4.4 ± 0.5 |
| Structural Optimization Detection [HIGH] | 4.0 ± 0.7 | 4.8 ± 0.4 |
| Aggregate and Replace Chain Detection [HIGH] | 4.6 ± 0.5 | 5.0 |
| Span Usage Consistency [HIGH] | 4.6 ± 0.5 | 5.0 |
| Inheritance Sealing Accuracy [HIGH] | 4.8 ± 0.4 | 4.2 ± 1.3 |
| Params Overload Optimization [MODERATE] | 4.4 ± 0.5 | 4.2 ± 0.4 |
| Severity Classification Accuracy [HIGH] | 3.6 ± 0.5 | 4.4 ± 0.9 |
| Fix Recommendation Quality [HIGH] | 4.0 | 4.8 ± 0.4 |
| Token Efficiency [MEDIUM] | 5.0 | 1.6 ± 0.5 |

---

## Final Rankings

Configurations ranked by mean weighted score — **higher is better**. Std Dev shows run-to-run variability (lower = more consistent).

| Rank | Configuration | Mean Score ↑ | % of Max (140) | Std Dev ↓ | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | dotnet-perf-skills | 131.0 | 94% | 4.8 | 123.0 | 136.0 |
| 🥈 | no-skills | 122.6 | 88% | 9.3 | 111.0 | 134.0 |

---

## Weighted Score per Run

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 134.0 | 132.0 |
| 2 | 123.0 | 123.0 |
| 3 | 129.0 | 132.0 |
| 4 | 116.0 | 132.0 |
| 5 | 111.0 | 136.0 |
| **Mean** | **122.6** | **131.0** |

---

## Token Usage Summary

Average token consumption per configuration across all runs.

| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time | Δ Input vs Baseline |
|---|---|---|---|---|---|---|
| no-skills | 313,801 | 10,064 | 220,973 | 7 | 3m 43s | — (baseline) |
| dotnet-perf-skills | 762,640 | 13,820 | 522,437 | 10 | 4m 36s | +143.0% |

---

## Token Usage Per Run

| Configuration | Run | Scenario | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time | Note |
|---|---|---|---|---|---|---|---|---|
| no-skills | 1 | analyze-perf-issues | 344,879 | 10,348 | 229,923 | 7 | 4m 7s |  |
| no-skills | 2 | analyze-perf-issues | 267,659 | 10,829 | 179,676 | 6 | 3m 32s |  |
| no-skills | 3 | analyze-perf-issues | 333,066 | 10,167 | 245,443 | 7 | 3m 34s |  |
| no-skills | 4 | analyze-perf-issues | 294,137 | 9,380 | 207,336 | 7 | 3m 48s |  |
| no-skills | 5 | analyze-perf-issues | 329,266 | 9,594 | 242,489 | 7 | 3m 33s |  |
| dotnet-perf-skills | 1 | analyze-perf-issues | 769,860 | 13,285 | 557,207 | 11 | 4m 23s |  |
| dotnet-perf-skills | 2 | analyze-perf-issues | 839,116 | 14,394 | 585,015 | 11 | 4m 22s |  |
| dotnet-perf-skills | 3 | analyze-perf-issues | 676,382 | 14,811 | 431,994 | 9 | 4m 56s |  |
| dotnet-perf-skills | 4 | analyze-perf-issues | 676,680 | 12,847 | 431,994 | 9 | 4m 39s |  |
| dotnet-perf-skills | 5 | analyze-perf-issues | 851,161 | 13,765 | 605,973 | 12 | 4m 37s |  |


---

## Consistency Analysis

Score σ (standard deviation) measures how much a configuration's weighted score varies across runs — **lower is better**, meaning the configuration produces more reliable, repeatable results.

| Configuration | Score σ (lower = more consistent) | Most Consistent Dim (σ) | Most Variable Dim (σ) |
|---|---|---|---|
| no-skills | 9.3 | Reflection and Serialization Overhead (0.0) | Structural Optimization Detection (0.7) |
| dotnet-perf-skills | 4.8 | Regex Anti-Pattern Detection (0.0) | Inheritance Sealing Accuracy (1.3) |

---

## Per-Dimension Analysis

### 1. Regex Anti-Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 4 | 5 |
| 3 | 5 | 5 |
| 4 | 4 | 5 |
| 5 | 4 | 5 |
| **Mean** | **4.4** | **5.0** |

#### Analysis

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-4/analyze-perf-issues/performance-analysis.md`)
> | `new Regex(` (per-call construction) | **8** |  
> | `RegexOptions.Compiled` (startup budget) | **48** (all in MarkdownStripper.cs) |  
> | `[GeneratedRegex]` (source-gen) | **0** |

> **Impact:** LogAnalyzer.TryParseLine creates 2–3 Regex objects *per log line*. Parsing 100k lines = 200k–300k Regex compilations.  
> **Fix:** Hoist to `private static readonly` fields or use `[GeneratedRegex]`.

**no-skills** (`output/no-skills/run-4/analyze-perf-issues/performance-analysis.md`)
> 🔴 **Critical — `new Regex()` per log line in `TryParseLine` (lines 50, 65, 75)**  
> Creates up to 3 regex objects per line. For a 1M-line log file, this is 1-3 million regex compilations.  
> **Fix:** Hoist to `private static readonly Regex` fields or use `[GeneratedRegex]`.

> 🔴 **Critical — 40+ `RegexOptions.Compiled` regex instances (lines 13-59)**  
> **Fix:** On .NET 7+, use `[GeneratedRegex]` source generator for zero-startup-cost regexes.

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best due to stronger quantification and broader regex strategy coverage (counts + scan checklist + targeted caveats).

### 2. String Allocation Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 5 | 5 |
| 3 | 5 | 5 |
| 4 | 4 | 5 |
| 5 | 4 | 5 |
| **Mean** | **4.6** | **5.0** |

#### Analysis

**dotnet-perf-skills**
> #### 7. Char-by-char `+=` string building in CsvParser (3 loops)  
> **Impact:** `current += line[i]` allocates a new string per character.  
> **Fix:** Use `StringBuilder` for all three methods.

> #### 11. `.ToLower()` / `.ToUpper()` without culture (17 instances)  
> **Fix:** Replace with `ToLowerInvariant()` ... or use `StringComparison.OrdinalIgnoreCase` for comparisons.

**no-skills**
> 🔴 **Critical — Char-by-char string concatenation in `ParseLine` and `SplitLines`**  
> `current += line[i]` inside a loop is O(n²).  
> **Fix:** Use `StringBuilder` or `Span<char>` / `ValueStringBuilder`.

> 🟡 **Moderate — Sequential `.Replace()` chain over `ReplacementMap`**  
> Each `.Replace()` scans the entire string and allocates a new string — 9 intermediate allocations.

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best; it is more complete on loop concatenation breadth, replace-chain patterns, and casing guidance.

### 3. Collection and LINQ Efficiency [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 4 | 5 |
| 3 | 5 | 5 |
| 4 | 4 | 5 |
| 5 | 4 | 5 |
| **Mean** | **4.4** | **5.0** |

#### Analysis

**dotnet-perf-skills**
> #### 8. `.ToList()` + `.Contains()` for key set — O(n²) lookup in Diff  
> **Fix:** Use `HashSet<string>` for `allKeys`.

> #### 12. ContainsKey + indexer double-lookup (13 actionable sites)  
> **Fix:** Replace with `TryGetValue` pattern.

**no-skills**
> 🟡 **Moderate — `Skip(i).Take(5).ToList()` in sliding-window loop**  
> Allocates a new list on every iteration; O(n²) total.

> 🟡 **Moderate — `.ToList()` + `.Contains()` for key union in `Diff`  
> **Fix:** Use `HashSet<string>` or iterate `flat1.Keys.Union(flat2.Keys)`.

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** leads with better scope and explicit actionable counts for dictionary and LINQ overhead.

### 4. Async and IO Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 5 | 5 |
| 3 | 5 | 5 |
| 4 | 4 | 5 |
| 5 | 4 | 5 |
| **Mean** | **4.6** | **5.0** |

#### Analysis

**dotnet-perf-skills**
> #### 1. `new HttpClient()` per call — socket exhaustion risk (3 instances)  
> **Fix:** Inject `IHttpClientFactory` or use a single `static readonly HttpClient`...

> #### 21. Sequential awaits in loop — SendBatchAsync  
> #### 22. Unbounded parallelism — SendBatchParallelAsync  
> #### 23. Missing `CancellationToken` in async methods

**no-skills**
> 🔴 **Critical — `new HttpClient()` per call (lines 163, 179, 192)**  
> **Fix:** Inject `IHttpClientFactory` or use a single static/shared `HttpClient` instance.

> 🟡 **Moderate — Sequential awaits in `SendBatchAsync` loop**  
> 🟡 **Moderate — `Task.Delay` without `CancellationToken` in retry loop**

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best with clearer risk framing and complete async-pattern triad coverage.

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

**dotnet-perf-skills**
> #### 4. Uncached `new JsonSerializerOptions` per call (5 instances)  
> **Impact:** Up to 592× slower than cached options (.NET 6 benchmark).

> #### 15. Uncached reflection — `GetProperties()` / `GetProperty()` per call  
> **Fix:** Cache `PropertyInfo[]` per type in a `ConcurrentDictionary<Type, PropertyInfo[]>`.

**no-skills**
> 🟡 **Moderate — `new JsonSerializerOptions` on every call (lines 74, 117, 135, 142)**  
> **Fix:** Use a `private static readonly JsonSerializerOptions` shared instance.

> 🟡 **Moderate — Uncached `GetProperties()` reflection per call in `MapTo<T>` and `MapFrom<T>`

**Score:** dotnet-perf-skills **4/5**; no-skills **4/5**.  
**Verdict:** **Tie**. Both identify the key hotspots; neither strongly develops partial-deserialization alternatives (e.g., `Utf8JsonReader`) in this output.

### 6. Structural Optimization Detection [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 4 |
| 2 | 4 | 5 |
| 3 | 4 | 5 |
| 4 | 4 | 5 |
| 5 | 3 | 5 |
| **Mean** | **4.0** | **4.8** |

#### Analysis

**dotnet-perf-skills**
> #### 13. 3 structs without `IEquatable<T>` (3 of 3 = 0%)  
> #### 14. 23 unsealed leaf classes (23 of 26 non-abstract classes)  
> #### 25. 3 static `Dictionary<>` that are FrozenDictionary candidates

**no-skills**
> ### 6. Structs Without `IEquatable<T>`  
> ### 7. Unsealed Leaf Classes  
> ### 10. Static Dictionaries as `FrozenDictionary` Candidates

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best due to stronger quantified evidence and class-level precision.

### 7. Aggregate and Replace Chain Detection [HIGH × 2]

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

**dotnet-perf-skills**
> #### 16. `.Aggregate()` with `.Replace()` — 16 intermediate string allocations  
> #### 17. `char.ToString()` in loop (1 instance)

**no-skills**
> 🟡 **Moderate — `.Aggregate()` with `.Replace()` creating 16 intermediate strings**  
> 🟡 **Moderate — `char.ToString()` allocation per iteration (line 64)

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best by linking the pattern more explicitly to severity and remediation framing.

### 8. Span Usage Consistency [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 5 | 5 |
| 3 | 5 | 5 |
| 4 | 4 | 5 |
| 5 | 4 | 5 |
| **Mean** | **4.6** | **5.0** |

#### Analysis

**dotnet-perf-skills**
> #### 19. `value[..n].TrimEnd()` — double allocation (1 instance)  
> #### 20. Inconsistent `AsSpan` usage across truncators  
> #### 18. `List<char>` for static symbol sets — heap allocation

**no-skills**
> 🟡 **Moderate — `value[..n].TrimEnd()` double allocation (line 32)**  
> 🟡 **Moderate — Inconsistent `AsSpan` usage across truncators**  
> ℹ️ **Info — `List<char>` for symbol sets in `TruncationSymbols`**

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best with tighter consistency analysis and better explicit optimization alternatives.

### 9. Inheritance Sealing Accuracy [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 5 | 2 |
| 3 | 5 | 5 |
| 4 | 5 | 4 |
| 5 | 4 | 5 |
| **Mean** | **4.8** | **4.2** |

#### Analysis

**dotnet-perf-skills**
> **Impact:** ... 3 base classes identified ... — those must remain unsealed.  
> **Files (leaf classes that should be sealed):** MetricFormatter, EnglishOrdinalizer, GermanOrdinalizer, SpanishOrdinalizer...

**no-skills**
> ### 7. Unsealed Leaf Classes  
> ...  
> **Recommendation:** Seal leaf classes ... Do NOT seal base classes (`DefaultOrdinalizer`, `Ordinalizer`).

**Score:** dotnet-perf-skills **4/5**; no-skills **5/5**.  
**Verdict:** **no-skills** is best here because it explicitly and unambiguously guards against sealing `DefaultOrdinalizer`/`Ordinalizer`.

### 10. Params Overload Optimization [MODERATE × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 4 |
| 2 | 4 | 4 |
| 3 | 4 | 4 |
| 4 | 5 | 4 |
| 5 | 4 | 5 |
| **Mean** | **4.4** | **4.2** |

#### Analysis

**dotnet-perf-skills**
> #### 28. `params` methods without single-argument fast-path overloads (4 instances)  
> **Fix:** Add 1-argument overload or switch to `params ReadOnlySpan<T>` on .NET 9+.

**no-skills**
> 🟡 **Moderate — `params ITruncator[]` without single-argument fast path (line 107)**  
> **Fix:** Add overload: `Apply(string value, int maxLength, ITruncator truncator)`.

**Score:** dotnet-perf-skills **4/5**; no-skills **5/5**.  
**Verdict:** **no-skills** is best for this dimension due to directly matching the concrete `Apply(..., params ITruncator[])` fast-path requirement.

### 11. Severity Classification Accuracy [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 3 | 4 |
| 3 | 4 | 3 |
| 4 | 4 | 5 |
| 5 | 3 | 5 |
| **Mean** | **3.6** | **4.4** |

#### Analysis

**dotnet-perf-skills**
> | 🔴 Critical | 8 | `new HttpClient()` per call ... uncached `new Regex()` in per-line hot path, uncached `JsonSerializerOptions` |  
> | 🟡 Moderate | 28 | `+=` string concatenation in loops ... 48 `RegexOptions.Compiled` ... ContainsKey+indexer ... |

**no-skills**
> | 🔴 Critical | 6 |  
> | 🟡 Moderate | 22 |  
> | ℹ️ Info | 19 |

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best; its prioritization better separates hot-path critical risks from lower-impact hygiene items.

### 12. Fix Recommendation Quality [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 4 | 4 |
| 3 | 4 | 5 |
| 4 | 4 | 5 |
| 5 | 4 | 5 |
| **Mean** | **4.0** | **4.8** |

#### Analysis

**dotnet-perf-skills**
> **Fix:** Inject `IHttpClientFactory` or use a single `static readonly HttpClient` ...  
> **Fix:** Hoist to `[GeneratedRegex]` static partial methods.  
> **Fix:** Replace with `TryGetValue` pattern.

**no-skills**
> **Fix:** Inject `IHttpClientFactory` or use a single static/shared `HttpClient` instance.  
> **Fix:** On .NET 7+, use `[GeneratedRegex]` source generator...  
> **Fix:** Use `StringComparison.OrdinalIgnoreCase` or `.ToLowerInvariant()`.

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best with more consistently specific, implementation-ready fixes and stronger API-level guidance.

### 13. Token Efficiency [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 2 |
| 2 | 5 | 1 |
| 3 | 5 | 2 |
| 4 | 5 | 2 |
| 5 | 5 | 1 |
| **Mean** | **5.0** | **1.6** |

---

## Asset Usage Summary

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| no-skills | 1 | 48fd107a…294f | claude-opus-4.6 | — | — | ✅ |
| no-skills | 2 | b305400e…403f | claude-opus-4.6 | — | — | ✅ |
| no-skills | 3 | ab682227…714c | claude-opus-4.6 | — | — | ✅ |
| no-skills | 4 | 65b6fb3f…2d63 | claude-opus-4.6 | — | — | ✅ |
| no-skills | 5 | a058cfbd…8db7 | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills | 1 | 7f9d8da1…1374 | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills | 2 | e691ccb9…dfd0 | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills | 3 | 9f79a09b…49b0 | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills | 4 | 114fb90e…78d0 | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills | 5 | 7b3d3800…181c | claude-opus-4.6 | — | — | ✅ |

---

## Copilot Recommendations (Best Run)

Prioritized recommendations extracted from the highest-scoring run's output for each configuration.

### no-skills (run 1, score 134)

## Prioritized Fix Recommendations

| Rank | Finding | Files | Severity | Effort | Impact |
|------|---------|-------|----------|--------|--------|
| 1 | Replace `new HttpClient()` with `IHttpClientFactory` or static instance | NotificationService | 🔴 Critical | Quick-fix | Prevents socket exhaustion in production |
| 2 | Hoist `new Regex(...)` to static fields (or `[GeneratedRegex]`) | LogAnalyzer, TemplateEngine, SlugGenerator, ValidationEngine, CsvParser | 🔴 Critical | Moderate | Eliminates regex recompilation on every call; massive savings in LogAnalyzer (per-line) |
| 3 | Replace char-by-char `+=` with `StringBuilder` in CSV parsing | CsvParser | 🔴 Critical | Moderate | Eliminates O(n²) string building per field/line |
| 4 | Cache `GetProperties()` and compiled accessors in `EntityMapper` | EntityMapper | 🔴 Critical | Moderate | Eliminates repeated reflection in batch operations |
| 5 | Add `SemaphoreSlim` throttling to `SendBatchParallelAsync` | NotificationService | 🔴 Critical | Quick-fix | Prevents unbounded parallelism overwhelming downstream |
| 6 | Convert 46 `RegexOptions.Compiled` to `[GeneratedRegex]` | MarkdownStripper | 🟡 Moderate | Moderate | Reduces startup time and memory; compile-time codegen |
| 7 | Replace `+=` string concatenation in loops with `StringBuilder` | TemplateEngine, LogAnalyzer, DataPipeline, NotificationService, ValidationEngine | 🟡 Moderate | Quick-fix | Reduces O(n²) to O(n) in report/output builders |
| 8 | Replace `ContainsKey`+indexer with `TryGetValue` everywhere | 6 files | 🟡 Moderate | Quick-fix | Eliminates double hash lookups; simple search-replace |
| 9 | Use `HashSet<T>` for lookup collections | SlugGenerator, JsonTransformer, TextTruncation | 🟡 Moderate | Quick-fix | O(1) vs O(n) lookups |
| 10 | Implement `IEquatable<T>` on all value-type structs | UnitFormatter, NotificationService, ValidationEngine | 🟡 Moderate | Moderate | Eliminates boxing and reflection-based equality |

### dotnet-perf-skills (run 5, score 136)

## Prioritized Fix Recommendations

| Rank | Finding | Effort | Impact |
|------|---------|--------|--------|
| 1 | Replace `new HttpClient()` with `IHttpClientFactory` or static instance | Quick-fix | Prevents production socket exhaustion |
| 2 | Cache `JsonSerializerOptions` as static field | Quick-fix | Up to 592× faster serialization |
| 3 | Hoist `new Regex()` to static fields or `[GeneratedRegex]` | Quick-fix | 10-100× faster in LogAnalyzer per-line loops |
| 4 | Replace all `+=` string loops with `StringBuilder` (9 sites) | Moderate | Eliminates O(n²) allocation in all string-building loops |
| 5 | Convert 48 `RegexOptions.Compiled` to `[GeneratedRegex]` | Moderate | Eliminates 100-500ms cold-start penalty |
| 6 | Replace `ContainsKey` + indexer with `TryGetValue` (10 sites) | Quick-fix | ~2× faster per dictionary lookup |
| 7 | Fix `.ToLower()`/`.ToUpper()` to use ordinal (15 sites) | Quick-fix | Correctness fix + 2-3× faster |
| 8 | Cache reflection `GetProperties()` per type | Moderate | 10-100× faster per mapping call |
| 9 | Seal all leaf classes | Quick-fix | Enables JIT devirtualization |
| 10 | Implement `IEquatable<T>` on 3 structs | Moderate | Eliminates boxing on equality checks |

> ⚠️ **Disclaimer:** These results are generated by an AI assistant and are non-deterministic. Findings may include false positives, miss real issues, or suggest changes that are incorrect for your specific context. Always verify recommendations with benchmarks and human review before applying changes to production code.

---

## Raw Data References

- Per-run analysis: `reports/analysis-run-1.md`
- Per-run analysis: `reports/analysis-run-2.md`
- Per-run analysis: `reports/analysis-run-3.md`
- Per-run analysis: `reports/analysis-run-4.md`
- Per-run analysis: `reports/analysis-run-5.md`
- Score data: `reports/scores-data.json`
- Generation usage: `reports/generation-usage.json`
