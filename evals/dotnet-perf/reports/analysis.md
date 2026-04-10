# Aggregated Analysis: .NET Performance Analysis Skill Evaluation

**Runs:** 3 | **Configurations:** 2 | **Scenarios:** 1 | **Dimensions:** 11
**Date:** 2026-04-09 21:03 UTC

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
3. **Analyze** — An AI judge reviews the source code of all configurations side-by-side and scores each across 11 quality dimensions.

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
| Regex Anti-Pattern Detection [CRITICAL] | 4.7 ± 0.6 | 5.0 |
| String Allocation Detection [CRITICAL] | 4.7 ± 0.6 | 4.3 ± 0.6 |
| Collection and LINQ Efficiency [CRITICAL] | 4.3 ± 0.6 | 4.7 ± 0.6 |
| Async and IO Pattern Detection [CRITICAL] | 5.0 | 4.3 ± 1.2 |
| Reflection and Serialization Overhead [HIGH] | 4.0 | 5.0 |
| Structural Optimization Detection [HIGH] | 4.0 ± 1.0 | 4.7 ± 0.6 |
| Severity Classification Accuracy [HIGH] | 4.3 ± 0.6 | 3.3 ± 0.6 |
| Fix Recommendation Quality [HIGH] | 4.0 | 5.0 |
| Internal Consistency and Traceability [MEDIUM] | 2.0 | 5.0 |
| Token Efficiency [MEDIUM] | 4.7 ± 0.6 | 1.3 ± 0.6 |
| Evidence Quantification & Auditability [MEDIUM] | 3.0 | 5.0 |

---

## Final Rankings

| Rank | Configuration | Mean Score | % of Max (100) | Std Dev | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | dotnet-perf-skills | 97.3 | 97% | 1.5 | 96.0 | 99.0 |
| 🥈 | no-skills | 96.0 | 96% | 7.0 | 88.0 | 101.0 |

---

## Weighted Score per Run

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 88.0 | 96.0 |
| 2 | 101.0 | 97.0 |
| 3 | 99.0 | 99.0 |
| **Mean** | **96.0** | **97.3** |

---

## Verification Summary (All Runs)

| Configuration | Build Pass Rate | Run Pass Rate | Avg Warnings |
|---|---|---|---|
| no-skills | 0/3 (0%) | 0/3 (0%) | 0.0 |
| dotnet-perf-skills | 0/3 (0%) | 0/3 (0%) | 0.0 |

---

## Token Usage Summary

Average token consumption per configuration across all runs.

| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time | Δ Input vs Baseline |
|---|---|---|---|---|---|---|
| no-skills | 398,932 | 10,888 | 325,994 | 9 | 3m 44s | — (baseline) |
| dotnet-perf-skills | 1,166,096 | 20,786 | 885,098 | 16 | 5m 1s | +192.3% |

---

## Token Usage Per Run

| Configuration | Run | Scenario | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time |
|---|---|---|---|---|---|---|---|
| no-skills | 2 | analyze-perf-issues | 404,728 | 12,338 | 330,315 | 9 | 4m 8s |
| no-skills | 3 | analyze-perf-issues | 393,137 | 9,438 | 321,672 | 9 | 3m 20s |
| dotnet-perf-skills | 1 | analyze-perf-issues | 1,306,909 | 22,982 | 1,078,211 | 17 | 5m 3s |
| dotnet-perf-skills | 2 | analyze-perf-issues | 965,786 | 14,808 | 715,883 | 14 | 4m 59s |
| dotnet-perf-skills | 3 | analyze-perf-issues | 1,225,594 | 24,567 | 861,201 | 16 | 5m 1s |

---

## Consistency Analysis

| Configuration | Score σ | Most Consistent Dim (σ) | Most Variable Dim (σ) |
|---|---|---|---|
| no-skills | 7.0 | Async and IO Pattern Detection (0.0) | Structural Optimization Detection (1.0) |
| dotnet-perf-skills | 1.5 | Regex Anti-Pattern Detection (0.0) | Async and IO Pattern Detection (1.2) |

---

## Per-Dimension Analysis

### 1. Regex Anti-Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 5 | 5 |
| 3 | 5 | 5 |
| **Mean** | **4.7** | **5.0** |

#### Analysis

Both configurations clearly catch per-call regex allocation, heavy `RegexOptions.Compiled`, and recommend `[GeneratedRegex]`.

```csharp
// dotnet-perf-skills (output/dotnet-perf-skills/run-2/analyze-perf-issues/performance-analysis.md)
// "new Regex() Inside Per-Line Hot Path (5 instances in LogAnalyzer)"
[GeneratedRegex(@"\[(\d{4}-\d{2}-\d{2}\s+\d{2}:\d{2}:\d{2}\.\d{3})\]\s+\[(\w+)\]\s+\[(\w+)\]\s+(.+)")]
private static partial Regex StructuredLogRegex();
```

```csharp
// no-skills (output/no-skills/run-2/analyze-perf-issues/performance-analysis.md)
// "45+ static readonly Regex fields with RegexOptions.Compiled ... should use [GeneratedRegex]"
[GeneratedRegex(@"^#{1,6}\s+", RegexOptions.Multiline)]
private static partial Regex HeaderPattern();
```

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**.  
**Verdict:** Tie. Both identify the exact high-impact regex issues and map to modern .NET 8 source-generated regex guidance.

### 2. String Allocation Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 5 | 4 |
| 3 | 5 | 4 |
| **Mean** | **4.7** | **4.3** |

#### Analysis

Both detect loop concatenation, replace-chains, and case-normalization allocation/culture issues; `no-skills` is more exhaustive at file/method granularity.

```csharp
// dotnet-perf-skills
// "String += Concatenation in Loops — O(n²) (12+ sites)"
var sb = new StringBuilder();
foreach (var row in rows)
    sb.Append(string.Join(...)).Append('\n');
```

```csharp
// no-skills
// "Char-by-char current += line[i] in ParseLine — O(n²)"
var current = "";
for (int i = 0; i < line.Length; i++) { current += line[i]; }
```

**Score:** dotnet-perf-skills **4/5**, no-skills **5/5**.  
**Verdict:** `no-skills` is stronger here due to deeper call-site coverage (CsvParser, TemplateEngine, LogAnalyzer, MarkdownStripper, SlugGenerator) with explicit hotspot framing.

### 3. Collection and LINQ Efficiency [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 5 | 4 |
| 3 | 4 | 5 |
| **Mean** | **4.3** | **4.7** |

#### Analysis

Both catch HashSet vs List membership, `ContainsKey`+indexer, avoidable `.ToList()`, and `Skip().Take().ToList()` windows.

```csharp
// dotnet-perf-skills
var allKeys = new HashSet<string>(flat1.Keys);
allKeys.UnionWith(flat2.Keys);
```

```csharp
// no-skills
var existing = new HashSet<string>(existingSlugs);
while (existing.Contains($"{baseSlug}-{counter}")) { counter++; }
```

**Score:** dotnet-perf-skills **4/5**, no-skills **5/5**.  
**Verdict:** `no-skills` gives better per-file pinpointing and clearer prioritization of O(n×m) hotspots.

### 4. Async and IO Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 3 |
| 2 | 5 | 5 |
| 3 | 5 | 5 |
| **Mean** | **5.0** | **4.3** |

#### Analysis

Both runs strongly identify per-call `HttpClient`, sequential awaits, unbounded parallelism, and cancellation-token gaps.

```csharp
// dotnet-perf-skills
using var client = new HttpClient();
// -> replace with IHttpClientFactory or shared static client
```

```csharp
// no-skills
var semaphore = new SemaphoreSlim(10); // bounded concurrency recommendation
await Parallel.ForEachAsync(requests, new ParallelOptions { MaxDegreeOfParallelism = 10 }, ...);
```

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**.  
**Verdict:** Tie. Both correctly emphasize production-risk async/IO anti-patterns and propose standard .NET fixes.

### 5. Reflection and Serialization Overhead [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 4 | 5 |
| 3 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

Both detect uncached reflection and serializer options churn; `dotnet-perf-skills` adds stronger quantification and direct caching patterns.

```csharp
// dotnet-perf-skills
private static readonly JsonSerializerOptions s_indentedOptions = new() { WriteIndented = true };
return JsonSerializer.Serialize(obj, s_indentedOptions);
```

```csharp
// no-skills
private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _propertyCache = new();
var properties = _propertyCache.GetOrAdd(typeof(TTarget), t => t.GetProperties());
```

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** `dotnet-perf-skills` is slightly better due to tighter serialization-caching emphasis and stronger impact framing.

### 6. Structural Optimization Detection [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 3 | 5 |
| 2 | 5 | 4 |
| 3 | 4 | 5 |
| **Mean** | **4.0** | **4.7** |

#### Analysis

Both identify unsealed classes, missing `IEquatable<T>`, and `FrozenDictionary` opportunities; `no-skills` maps more explicitly to requested named targets.

```csharp
// dotnet-perf-skills
private static readonly FrozenDictionary<string, string> ReplacementMap =
    new Dictionary<string, string> { ... }.ToFrozenDictionary();
```

```csharp
// no-skills
public sealed class Record { ... }          // DataPipeline.Record
public struct DeliveryResult : IEquatable<DeliveryResult> { ... }
```

**Score:** dotnet-perf-skills **4/5**, no-skills **5/5**.  
**Verdict:** `no-skills` better matches the scenario-specific structural checklist (explicit leaf classes/structs).

### 7. Severity Classification Accuracy [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 3 |
| 2 | 4 | 4 |
| 3 | 5 | 3 |
| **Mean** | **4.3** | **3.3** |

#### Analysis

Both rank hot-path risks highly (per-line regex, per-call HttpClient, O(n²) string growth), but both also contain some aggressive elevation of moderate issues.

```csharp
// dotnet-perf-skills severity framing
// "new HttpClient() Per Call — Socket Exhaustion (Critical)"
// "new Regex() Inside Per-Line Hot Path (Critical)"
```

```csharp
// no-skills severity framing
// "new Regex(...) inside TryParseLine — called per log line (Critical)"
// "ContainsKey + indexer — double lookup (Moderate)"
```

**Score:** dotnet-perf-skills **4/5**, no-skills **4/5**.  
**Verdict:** Tie. Both prioritize major production risks correctly; both occasionally over-rate lower-impact patterns.

### 8. Fix Recommendation Quality [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| 2 | 4 | 5 |
| 3 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

Both provide concrete API-level guidance; `dotnet-perf-skills` is more consistently prescriptive with caveats and implementation details.

```csharp
// dotnet-perf-skills
[GeneratedRegex(@"^#{1,6}\s+", RegexOptions.Multiline)]
private static partial Regex HeaderPattern();
// caveat: class must be partial; call HeaderPattern()
```

```csharp
// no-skills
// Before: O(n) Contains in loop
var existing = existingSlugs.ToList();
while (existing.Contains(baseSlug + "-" + counter.ToString())) { ... }
// After: HashSet + interpolation
```

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** `dotnet-perf-skills` is best on actionability quality: precise APIs, migration notes, and safer implementation patterns.

### 9. Internal Consistency and Traceability [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 2 | 5 |
| 2 | — | — |
| 3 | — | — |
| **Mean** | **2.0** | **5.0** |

### 10. Token Efficiency [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 1 |
| 2 | 4 | 2 |
| 3 | 5 | 1 |
| **Mean** | **4.7** | **1.3** |

### 11. Evidence Quantification & Auditability [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | — | — |
| 2 | 3 | 5 |
| 3 | 3 | 5 |
| **Mean** | **3.0** | **5.0** |

#### Analysis

`dotnet-perf-skills` includes a scan checklist with category hit counts; `no-skills` is detailed but less explicitly auditable by recipe.

```csharp
// dotnet-perf-skills scan evidence
// `new Regex(` hits: 8
// `RegexOptions.Compiled` hits: 48
// `new HttpClient(` hits: 3
// `.ToLower()/.ToUpper()` hits: 22
```

```csharp
// no-skills evidence style
// per-file issue tables with line-level findings, but no global recipe count matrix
```

**Score:** dotnet-perf-skills **5/5**, no-skills **3/5**.  
**Verdict:** `dotnet-perf-skills` provides better reproducibility and easier verification of coverage completeness.

---

## Asset Usage Summary

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| no-skills | 2 | 1c610bb9…9d2b | claude-opus-4.6 | — | — | ✅ |
| no-skills | 3 | a6c39e40…7b35 | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills | 1 | — | — | — | — | ✅ |
| dotnet-perf-skills | 2 | 1e857169…6a28 | claude-opus-4.6 | analyzing-dotnet-performance | dotnet-diag | ✅ |
| dotnet-perf-skills | 3 | — | — | — | — | ✅ |

---

## Raw Data References

- Per-run analysis: `reports/analysis-run-1.md`
- Per-run analysis: `reports/analysis-run-2.md`
- Per-run analysis: `reports/analysis-run-3.md`
- Verification data: `reports/verification-data.json`
- Score data: `reports/scores-data.json`
- Build notes: `reports/build-notes.md`
- Generation usage: `reports/generation-usage.json`
