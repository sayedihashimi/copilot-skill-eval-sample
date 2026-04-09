# Aggregated Analysis: .NET Performance Analysis Skill Evaluation

**Runs:** 1 | **Configurations:** 2 | **Scenarios:** 1 | **Dimensions:** 10
**Date:** 2026-04-09 19:00 UTC

---

## Overview

Evaluate how the dotnet/skills performance-related skills (analyzing-dotnet-performance, microbenchmarking, dotnet-trace-collect) improve Copilot's ability to detect performance anti-patterns in existing .NET code compared to baseline Copilot.

---

## What Was Tested

### Scenarios

Each run generates one of the following application scenarios (randomly selected per run):

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

1. **Generate** — For each configuration, Copilot CLI (`copilot --yolo`) is given a scenario prompt and generates a complete project from scratch. One scenario is randomly selected per run.
2. **Verify** — Each generated project is built (`dotnet build`), run, format-checked, and scanned for NuGet vulnerabilities.
3. **Analyze** — An AI judge reviews the source code of all configurations side-by-side and scores each across 10 quality dimensions.

Generation model: **claude-opus-4.6**
Analysis model: **gpt-5.3-codex**

---

## Scoring Methodology

Each dimension is scored on a **1–5 scale**:

| Score | Meaning |
|:---:|---|
| 5 | Excellent — follows all best practices |
| 4 | Good — minor gaps only |
| 3 | Acceptable — some issues present |
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
| Collection and LINQ Efficiency [CRITICAL] | 4.0 | 4.0 |
| Async and IO Pattern Detection [CRITICAL] | 5.0 | 5.0 |
| Reflection and Serialization Overhead [HIGH] | 4.0 | 5.0 |
| Structural Optimization Detection [HIGH] | 3.0 | 5.0 |
| Severity Classification Accuracy [HIGH] | 4.0 | 3.0 |
| Fix Recommendation Quality [HIGH] | 4.0 | 5.0 |
| Evidence Coverage & Traceability [MEDIUM] | 4.0 | 5.0 |
| Token Efficiency [MEDIUM] | 5.0 | — |

---

## Final Rankings

| Rank | Configuration | Mean Score | % of Max (100) | Std Dev | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | dotnet-perf-skills | 98.0 | 98% | 0.0 | 98.0 | 98.0 |
| 🥈 | no-skills | 90.0 | 90% | 0.0 | 90.0 | 90.0 |

---

## Weighted Score per Run

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 90.0 | 98.0 |
| **Mean** | **90.0** | **98.0** |

---

## Verification Summary (All Runs)

| Configuration | Build Pass Rate | Run Pass Rate | Avg Warnings |
|---|---|---|---|
| no-skills | 0/1 (0%) | 0/1 (0%) | 0.0 |
| dotnet-perf-skills | 0/1 (0%) | 0/1 (0%) | 0.0 |

---

## Token Usage Summary

Average token consumption per configuration across all runs.

| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time | Δ Input vs Baseline |
|---|---|---|---|---|---|---|
| no-skills | 415,394 | 10,127 | 306,482 | 9 | 3m 23s | — (baseline) |

---

## Token Usage Per Run

| Configuration | Run | Scenario | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time |
|---|---|---|---|---|---|---|---|
| no-skills | 1 | analyze-perf-issues | 415,394 | 10,127 | 306,482 | 9 | 3m 23s |

---

## Consistency Analysis

| Configuration | Score σ | Most Consistent Dim (σ) | Most Variable Dim (σ) |
|---|---|---|---|

---

## Per-Dimension Analysis

### 1. Regex Anti-Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

`dotnet-perf-skills` explicitly ties hot-path `new Regex(...)`, large `RegexOptions.Compiled` inventory, and `.NET 8` `[GeneratedRegex]` migration together with counts and targeted fixes. `no-skills` also detects these patterns, but with less consistent prioritization.

```csharp
// dotnet-perf-skills (performance-analysis.md)
| `RegexOptions.Compiled` | 48 | All in MarkdownStripper.cs |
| `[GeneratedRegex]` | 0 | ❌ None used — 0 of 70 regex patterns use source gen |
#### 3. `new Regex()` in per-line hot path (4 instances in LogAnalyzer)
```

```csharp
// no-skills (performance-analysis.md)
#### 🔴 CRITICAL — `new Regex()` per log line (lines 50, 65, 75)
#### 🟡 MODERATE — 47 `RegexOptions.Compiled` static instances (lines 13–59)
**Fix:** Replace all with `[GeneratedRegex]` partial methods
```

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best due to stronger budget framing (`48` compiled, `0/70` generated) and cleaner prioritization.

### 2. String Allocation Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

Both identify `string +=` loops, casing allocation issues, and replace-chain allocation pressure. The skills run is more cohesive across files and ties these to cross-cutting policy.

```csharp
// dotnet-perf-skills (performance-analysis.md)
#### 9. String `+=` concatenation in loops — O(n²) allocation
**Fix:** Replace with `StringBuilder`
### 2. String Building via `+=` Instead of StringBuilder
```

```csharp
// no-skills (performance-analysis.md)
#### 🔴 CRITICAL — String concatenation in loop (line 53–63)
#### 🟡 MODERATE — `.ToLower()` without culture (line 30)
**Fix:** Use `StringComparison.OrdinalIgnoreCase`
```

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** wins for breadth and stronger project-wide synthesis.

### 3. Collection and LINQ Efficiency [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 4 |
| **Mean** | **4.0** | **4.0** |

#### Analysis

Both detect major collection inefficiencies including `List.Contains` vs `HashSet`, `ContainsKey`+indexer, and LINQ materialization/windowing overhead.

```csharp
// dotnet-perf-skills (performance-analysis.md)
#### 7. ContainsKey + indexer double-lookup (12 instances)
**Fix:** Replace with `TryGetValue`
#### 16. O(n) `List.Contains` instead of `HashSet`
```

```csharp
// no-skills (performance-analysis.md)
#### 🟡 MODERATE — `Skip(i).Take(5).ToList()` in a loop (line 157)
#### ℹ️ INFO — `ContainsKey` + indexer pattern
**Fix:** Use `TryGetValue` or `CollectionsMarshal.GetValueRefOrAddDefault`
```

**Score:** dotnet-perf-skills **4/5**, no-skills **4/5**.  
**Verdict:** **Tie**. `no-skills` surfaces sliding-window LINQ cost clearly; `dotnet-perf-skills` is stronger on broad count-based detection.

### 4. Async and IO Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 5 |
| **Mean** | **5.0** | **5.0** |

#### Analysis

Both runs strongly capture all required async/IO anti-patterns (per-call `HttpClient`, sequential waits, unbounded parallelism, cancellation gaps).

```csharp
// dotnet-perf-skills (performance-analysis.md)
#### 1. `new HttpClient()` per call — Socket Exhaustion
#### 11. Sequential awaits in loop
#### 12. Unbounded parallelism
#### 13. `Task.Delay` without CancellationToken
```

```csharp
// no-skills (performance-analysis.md)
#### 🔴 CRITICAL — `new HttpClient()` per call — socket exhaustion
#### 🔴 CRITICAL — unbounded parallelism in `SendBatchParallelAsync`
#### 🟡 MODERATE — Missing `CancellationToken` throughout
```

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**.  
**Verdict:** **Tie**. Both are production-relevant and actionable.

### 5. Reflection and Serialization Overhead [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

Both detect reflection hotspots and per-call serializer options; skills output is more explicit about impact and cache behavior.

```csharp
// dotnet-perf-skills (performance-analysis.md)
#### 2. Uncached `new JsonSerializerOptions` per call (5 instances)
#### 10. Uncached reflection `GetProperties()`/`SetValue()`/`GetValue()`
```

```csharp
// no-skills (performance-analysis.md)
#### 🟡 MODERATE — `new JsonSerializerOptions` per call
#### 🔴 CRITICAL — uncached reflection in `EntityMapper`
private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertyCache = new();
```

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is better calibrated for serializer overhead and cross-file consistency.

### 6. Structural Optimization Detection [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 3 | 5 |
| **Mean** | **3.0** | **5.0** |

#### Analysis

`dotnet-perf-skills` catches structural issues as a systemic pattern (`0/17 sealed`, `0/2 IEquatable`, FrozenDictionary candidates). `no-skills` catches them, but with narrower coverage/severity.

```csharp
// dotnet-perf-skills (performance-analysis.md)
| `sealed class` | 0 | ❌ 0 of 17 classes sealed |
| `public struct` without `IEquatable<T>` | 2 |
#### 17. FrozenDictionary candidates — 2 static readonly dictionaries
```

```csharp
// no-skills (performance-analysis.md)
#### ℹ️ INFO — Unsealed `Record` class
#### ℹ️ INFO — `Converters` dictionary could be `FrozenDictionary`
#### 🟡 MODERATE — Struct without `IEquatable<T>` — `DeliveryResult`
```

**Score:** dotnet-perf-skills **5/5**, no-skills **3/5**.  
**Verdict:** **dotnet-perf-skills** clearly wins by identifying structural debt globally, not as isolated local notes.

### 7. Severity Classification Accuracy [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 3 |
| **Mean** | **4.0** | **3.0** |

#### Analysis

`no-skills` better separates high-impact runtime issues from lower-impact cleanup items. `dotnet-perf-skills` occasionally over-escalates (e.g., `ContainsKey`+indexer marked critical).

```csharp
// dotnet-perf-skills (performance-analysis.md)
#### 7. ContainsKey + indexer double-lookup (12 instances)
### 🔴 Critical
```

```csharp
// no-skills (performance-analysis.md)
#### 🔴 CRITICAL — `new HttpClient()` per call
#### 🔴 CRITICAL — `new Regex()` per log line
#### ℹ️ INFO — `ContainsKey` + indexer pattern
```

**Score:** dotnet-perf-skills **3/5**, no-skills **4/5**.  
**Verdict:** **no-skills** is better here because hot-path and incident-prone issues are prioritized more cleanly.

### 8. Fix Recommendation Quality [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

Both provide API-specific remediations and examples; skills output is tighter and avoids overcomplication.

```csharp
// dotnet-perf-skills (performance-analysis.md)
[GeneratedRegex(...)]
private static partial Regex StructuredLogRegex();
private static readonly HttpClient s_http = new(new SocketsHttpHandler { ... });
if (dict.TryGetValue(key, out var value)) Use(value);
```

```csharp
// no-skills (performance-analysis.md)
await Parallel.ForEachAsync(requests, options, async (req, ct) => { ... });
var existing = new HashSet<string>(existingSlugs);
ref var count = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out _);
```

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** has more consistently pragmatic fixes for typical engineering teams.

### 9. Evidence Coverage & Traceability [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

Both cite file/line evidence; the skills run adds stronger aggregate signal counting and inverse checks.

```csharp
// dotnet-perf-skills (gen-notes.md + performance-analysis.md)
`RegexOptions.Compiled` count: 48
`GeneratedRegex` count: 0
Sealed classes: 0 sealed / 17 total
```

```csharp
// no-skills (performance-analysis.md)
#### Findings by File
### Cross-Cutting Themes
| 🔴 Critical | 7 | 🟡 Moderate | 18 | ℹ️ Info | 16 |
```

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** has better benchmark-style traceability and coverage accounting.

### 10. Token Efficiency [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | — |
| **Mean** | **5.0** | — |

---

## Asset Usage Summary

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| no-skills | 1 | 5e4524a0…363b | claude-opus-4.6 | — | — | ✅ |

---

## Raw Data References

- Per-run analysis: `reports/analysis-run-1.md`
- Verification data: `reports/verification-data.json`
- Score data: `reports/scores-data.json`
- Build notes: `reports/build-notes.md`
- Generation usage: `reports/generation-usage.json`
