# Aggregated Analysis: .NET Performance Analysis Skill Evaluation

**Runs:** 3 | **Configurations:** 2 | **Scenarios:** 1 | **Dimensions:** 9
**Date:** 2026-04-13 22:43 UTC

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
| Collection and LINQ Efficiency [CRITICAL] | 4.3 ± 0.6 | 5.0 |
| Async and IO Pattern Detection [CRITICAL] | 4.7 ± 0.6 | 5.0 |
| Reflection and Serialization Overhead [HIGH] | 4.0 | 4.3 ± 0.6 |
| Structural Optimization Detection [HIGH] | 3.7 ± 0.6 | 5.0 |
| Severity Classification Accuracy [HIGH] | 3.7 ± 0.6 | 4.3 ± 0.6 |
| Fix Recommendation Quality [HIGH] | 4.0 | 5.0 |
| Token Efficiency [MEDIUM] | 5.0 | 1.7 ± 0.6 |

---

## Final Rankings

| Rank | Configuration | Mean Score | % of Max (105) | Std Dev | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | dotnet-perf-skills | 99.0 | 94% | 1.0 | 98.0 | 100.0 |
| 🥈 | no-skills | 88.7 | 84% | 5.0 | 84.0 | 94.0 |

---

## Weighted Score per Run

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 94.0 | 100.0 |
| 2 | 88.0 | 99.0 |
| 3 | 84.0 | 98.0 |
| **Mean** | **88.7** | **99.0** |

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
| no-skills | 5.0 | Reflection and Serialization Overhead (0.0) | Regex Anti-Pattern Detection (0.6) |
| dotnet-perf-skills | 1.0 | Regex Anti-Pattern Detection (0.0) | Reflection and Serialization Overhead (0.6) |

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

**dotnet-perf-skills excerpt** (`output/dotnet-perf-skills/run-2/analyze-perf-issues/performance-analysis.md`)
> 2. **Regex instantiation in hot loops** — `new Regex()` called per log line in LogAnalyzer (4 instances)  
> 3. **48 `RegexOptions.Compiled` regexes** in MarkdownStripper with 0 `[GeneratedRegex]` usage project-wide  
> **Fix:** Hoist to `static readonly` fields, or better, use `[GeneratedRegex]`

**no-skills excerpt** (`output/no-skills/run-2/analyze-perf-issues/performance-analysis.md`)
> 2. `LogAnalyzer.TryParseLine` — `new Regex()` per log line → O(n) regex compilations on large files  
> 5. `MarkdownStripper` — 45+ `RegexOptions.Compiled` instances → excessive JIT startup cost  
> **Fix for #1 (on .NET 7+):** ... `[GeneratedRegex(...)]`

**Score:** `dotnet-perf-skills: 5/5`, `no-skills: 4/5` (both catch the key issues; skills output is more explicit on project-wide `[GeneratedRegex]` absence and quantified regex counts).

**Verdict:** **dotnet-perf-skills** is best on regex depth and prioritization.

### 2. String Allocation Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 4 | 5 |
| 3 | 4 | 5 |
| **Mean** | **4.3** | **5.0** |

#### Analysis

**dotnet-perf-skills excerpt**
> #### 6. O(n²) String Concatenation via `+=` in Loops (7 files, ~15 sites)  
> ... `CsvParser.ParseLine`, `CsvParser.SplitLines`, ... `TemplateEngine.ProcessLoops`, `TemplateEngine.RenderBatch`  
> #### 8. `.ToLower()`/`.ToUpper()` Without Culture or StringComparison (25 instances)

**no-skills excerpt**
> | 1 | 🔴 Critical | 51–79 | Char-by-char string `+=` (O(n²)) | `ParseLine` builds field values with `current += line[i]` |  
> | 2 | 🔴 Critical | 88–108 | Char-by-char string `+=` (O(n²)) | `SplitLines` has the same O(n²) pattern |  
> | 6 |  |  | `.ToLower()`/`.ToUpper()` calls without `StringComparison` |

**Score:** `dotnet-perf-skills: 5/5`, `no-skills: 4/5` (both strong; skills output is more systematic and counted across files).

**Verdict:** **dotnet-perf-skills** is stronger due to clearer scale framing and consolidated cross-file signal.

### 3. Collection and LINQ Efficiency [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 5 | 5 |
| 3 | 4 | 5 |
| **Mean** | **4.3** | **5.0** |

#### Analysis

**dotnet-perf-skills excerpt**
> #### 10. `ContainsKey` + Indexer Double-Lookup (12 instances)  
> #### 19. `.ToList()` for Counting / O(n) `.Contains()` on Lists (5 instances)  
> #### 22. `Skip(i).Take(5).ToList()` in Sliding Window Loop (1 instance)

**no-skills excerpt**
> | 6 | 🔴 Critical | 75–85 | `List.Contains` in while loop (O(n²)) | ... Use a `HashSet<string>`. |  
> | 5 | 🟡 Moderate | 85–89 | `List.Contains` for key lookups (O(n²)) | `JsonTransformer.Diff` ... |  
> | 7 | 🟡 Moderate | 152–157 | `.ToList()` + `Skip(i).Take(5).ToList()` per iteration |

**Score:** `dotnet-perf-skills: 5/5`, `no-skills: 5/5` (both cover all requested hot-path collection/LINQ anti-patterns with concrete fixes).

**Verdict:** **Tie** — both are comprehensive and actionable.

### 4. Async and IO Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| 2 | 4 | 5 |
| 3 | 5 | 5 |
| **Mean** | **4.7** | **5.0** |

#### Analysis

**dotnet-perf-skills excerpt**
> #### 1. `new HttpClient()` Per Call — Socket Exhaustion Risk (3 instances)  
> #### 16. Sequential Awaits in Loop (1 instance)  
> #### 17. Unbounded Parallelism (1 instance)  
> #### 18. Missing `CancellationToken` on Async Methods

**no-skills excerpt**
> | 1 | 🔴 Critical | 163, 179, 191 | `new HttpClient()` per call | ... socket exhaustion |  
> | 2 | 🟡 Moderate | 116–118 | Sequential `await` in loop |  
> | 3 | 🔴 Critical | 130–133 | Unbounded parallelism |  
> | 4 | 🟡 Moderate | 102 | Missing `CancellationToken` |

**Score:** `dotnet-perf-skills: 5/5`, `no-skills: 4/5` (baseline identifies all key patterns, but skills output provides richer impact and fix framing for cancellation/retry behavior).

**Verdict:** **dotnet-perf-skills** is better on async/IO operational risk framing.

### 5. Reflection and Serialization Overhead [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 4 |
| 2 | 4 | 5 |
| 3 | 4 | 4 |
| **Mean** | **4.0** | **4.3** |

#### Analysis

**dotnet-perf-skills excerpt**
> #### 4. Uncached `JsonSerializerOptions` Per Call (4 instances)  
> #### 7. Uncached Reflection — `GetProperties()` / `SetValue()` / `GetValue()` Per Call (4 instances)  
> **Fix:** Cache `PropertyInfo[]` per type ... `ConcurrentDictionary<Type, PropertyInfo[]>`

**no-skills excerpt**
> | 1 | 🟡 Moderate | 74 | `new JsonSerializerOptions` per call | ... Cache as `static readonly`. |  
> | 1 | 🔴 Critical | 77 | Uncached `GetProperties()` reflection | ... Cache per type using `ConcurrentDictionary<Type, PropertyInfo[]>`. |

**Score:** `dotnet-perf-skills: 5/5`, `no-skills: 4/5` (both identify the core problems; skills output is broader and more quantified).

**Verdict:** **dotnet-perf-skills** is stronger on serialization + reflection breadth.

### 6. Structural Optimization Detection [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 4 | 5 |
| 3 | 3 | 5 |
| **Mean** | **3.7** | **5.0** |

#### Analysis

**dotnet-perf-skills excerpt**
> #### 13. FrozenDictionary Candidates — `static readonly Dictionary<>` Never Mutated (2 instances)  
> #### 14. Structs Without `IEquatable<T>` (2 instances)  
> #### 15. Unsealed Classes — 0 of 17 Sealed (17 instances)

**no-skills excerpt**
> ### 5. Unsealed Classes  
> **DataPipeline.Record**, **ValidationEngine.ValidationResult**, **EntityMapper.MappingConfig** ...  
> ### 6. Structs Without `IEquatable<T>`  
> ... **DeliveryResult** and **ValidationError**

**Score:** `dotnet-perf-skills: 5/5`, `no-skills: 4/5` (baseline catches structural themes but is less exhaustive on FrozenDictionary and global class-count framing).

**Verdict:** **dotnet-perf-skills** wins on structural completeness.

### 7. Severity Classification Accuracy [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 4 | 4 |
| 3 | 3 | 4 |
| **Mean** | **3.7** | **4.3** |

#### Analysis

**dotnet-perf-skills excerpt**
> | Rank | Finding | Severity | Effort | Impact |  
> | 1 | Reuse HttpClient (socket exhaustion) | 🔴 | Quick-fix | Prevents production incidents |  
> | 2 | Cache Regex in LogAnalyzer hot loop | 🔴 | Quick-fix | >10x parsing speedup |

**no-skills excerpt**
> | Rank | File | Issue | Severity | Effort | Impact |  
> | 1 | NotificationService.cs | `new HttpClient()` per call — socket exhaustion | 🔴 Critical | Moderate | Prevents production outages |  
> | 2 | LogAnalyzer.cs | `new Regex()` per log line in `TryParseLine` | 🔴 Critical | Quick-fix | 100x+ speedup |

**Score:** `dotnet-perf-skills: 4/5`, `no-skills: 4/5` (both prioritize true hot-path/incident issues correctly above moderate cleanup items).

**Verdict:** **Tie** — both rank critical production risks first with reasonable impact ordering.

### 8. Fix Recommendation Quality [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 4 | 5 |
| 3 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

**dotnet-perf-skills excerpt**
> **Fix:** Inject `IHttpClientFactory` or use a single `static readonly HttpClient` ...  
> **Fix:** ... use `[GeneratedRegex]` ...  
> **Fix:** Replace with `TryGetValue` ... `FrozenDictionary` ... `StringComparison.OrdinalIgnoreCase`

**no-skills excerpt**
> The fix is consistent: promote to `static readonly Regex` fields, or use `[GeneratedRegex]` on .NET 7+.  
> Always replace with `StringBuilder`.  
> Replace with `TryGetValue` ... Cache as `static readonly`.

**Score:** `dotnet-perf-skills: 5/5`, `no-skills: 4/5` (both actionable; skills output is more API-specific and consistently ties fixes to .NET 8 capabilities).

**Verdict:** **dotnet-perf-skills** provides higher-quality, more implementation-ready recommendations.

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

### no-skills (run 1, score 94)

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

### dotnet-perf-skills (run 1, score 100)

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
