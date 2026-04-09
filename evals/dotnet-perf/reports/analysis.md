# Aggregated Analysis: .NET Performance Analysis Skill Evaluation

**Runs:** 1 | **Configurations:** 2 | **Scenarios:** 1 | **Dimensions:** 10
**Date:** 2026-04-09 19:59 UTC

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
| Collection and LINQ Efficiency [CRITICAL] | 4.0 | 5.0 |
| Async and IO Pattern Detection [CRITICAL] | 4.0 | 5.0 |
| Reflection and Serialization Overhead [HIGH] | 4.0 | 4.0 |
| Structural Optimization Detection [HIGH] | 2.0 | 5.0 |
| Severity Classification Accuracy [HIGH] | 3.0 | 5.0 |
| Fix Recommendation Quality [HIGH] | 3.0 | 5.0 |
| Quantitative Evidence & Coverage [MEDIUM] | 3.0 | 5.0 |
| Token Efficiency [MEDIUM] | 5.0 | 1.0 |

---

## Final Rankings

| Rank | Configuration | Mean Score | % of Max (100) | Std Dev | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | dotnet-perf-skills | 104.0 | 104% | 0.0 | 104.0 | 104.0 |
| 🥈 | no-skills | 80.0 | 80% | 0.0 | 80.0 | 80.0 |

---

## Weighted Score per Run

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 80.0 | 104.0 |
| **Mean** | **80.0** | **104.0** |

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
| no-skills | 408,994 | 9,398 | 306,795 | 9 | 3m 38s | — (baseline) |
| dotnet-perf-skills | 1,191,627 | 22,558 | 946,515 | 16 | 5m 2s | +191.4% |

---

## Token Usage Per Run

| Configuration | Run | Scenario | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time |
|---|---|---|---|---|---|---|---|
| no-skills | 1 | analyze-perf-issues | 408,994 | 9,398 | 306,795 | 9 | 3m 38s |
| dotnet-perf-skills | 1 | analyze-perf-issues | 1,191,627 | 22,558 | 946,515 | 16 | 5m 2s |

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

`dotnet-perf-skills` directly identifies per-call `new Regex(...)`, high-instance `RegexOptions.Compiled` startup costs, and recommends `[GeneratedRegex]` as the preferred .NET 8 approach with explicit hotspot context.

```csharp
// dotnet-perf-skills (performance-analysis.md)
// Files: LogAnalyzer.cs:L50, L65, L75, L197 ...
[GeneratedRegex(@"\[(\d{4}-\d{2}-\d{2}\s+\d{2}:\d{2}:\d{2}\.\d{3})\]\s+\[(\w+)\]\s+\[(\w+)\]\s+(.+)")]
private static partial Regex StructuredLogRegex();
```

`no-skills` also catches the same major issues and recommends `[GeneratedRegex]`, but with less consistent scale accounting (e.g., 46 vs 48 compiled regex mentions across sections).

```csharp
// no-skills (performance-analysis.md)
// "Three new Regex(...) instantiations inside TryParseLine ... called for every line"
[GeneratedRegex(@"\[(\d{4}-\d{2}-\d{2}\s+\d{2}:\d{2}:\d{2}\.\d{3})\]\s+\[(\w+)\]\s+\[(\w+)\]\s+(.+)")]
private static partial Regex StructuredLogPattern();
```

**Score:** `dotnet-perf-skills: 5/5`, `no-skills: 4/5`  
**Verdict:** **dotnet-perf-skills** is stronger due to clearer hotspot prioritization and tighter startup-budget framing for `RegexOptions.Compiled`.

### 2. String Allocation Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

`dotnet-perf-skills` thoroughly flags `string +=` loop patterns, `.Replace()` chains, and `.ToLower()/.ToUpper()` allocation/culture issues, with direct O(n²) framing.

```csharp
// dotnet-perf-skills (performance-analysis.md)
var sb = new StringBuilder();
foreach (var row in rows) { sb.Append(...); }
return sb.ToString();
```

`no-skills` identifies the same core patterns, including char-by-char concatenation in CSV parsing and loop concatenation in templating/summarization.

```csharp
// no-skills (performance-analysis.md)
var current = new StringBuilder();
current.Append(line[i]);
fields.Add(current.ToString());
current.Clear();
```

**Score:** `dotnet-perf-skills: 5/5`, `no-skills: 4/5`  
**Verdict:** **dotnet-perf-skills** has better cross-cutting depth and ties casing fixes more consistently to `StringComparison.OrdinalIgnoreCase`.

### 3. Collection and LINQ Efficiency [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

`dotnet-perf-skills` gives broad and precise detection: `List.Contains` vs `HashSet`, `ContainsKey+indexer` vs `TryGetValue`, materialization removal, and sliding-window LINQ allocations.

```csharp
// dotnet-perf-skills (performance-analysis.md)
var allKeys = new HashSet<string>(flat1.Keys);
allKeys.UnionWith(flat2.Keys);
```

`no-skills` catches all major collection inefficiencies too, including `Skip(i).Take(5).ToList()` in-loop overhead and HashSet conversion opportunities.

```csharp
// no-skills (performance-analysis.md)
for (int i = 0; i <= errorEntries.Count - 5; i++)
{
    var timeSpan = errorEntries[i + 4].Timestamp - errorEntries[i].Timestamp;
}
```

**Score:** `dotnet-perf-skills: 5/5`, `no-skills: 4/5`  
**Verdict:** **dotnet-perf-skills** wins on consistency and prioritization quality across related collection patterns.

### 4. Async and IO Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 5 |
| **Mean** | **4.0** | **5.0** |

#### Analysis

`dotnet-perf-skills` correctly treats per-call `HttpClient` as critical, distinguishes sequential vs unbounded parallelism, and calls out missing cancellation propagation (`Task.Delay` included).

```csharp
// dotnet-perf-skills (performance-analysis.md)
private readonly HttpClient _client;
public NotificationService(HttpClient client, ...) { _client = client; ... }
```

`no-skills` catches the same anti-pattern cluster and provides bounded-parallelism recommendations, but has one weaker/contradictory serialization-related callout in this area.

```csharp
// no-skills (performance-analysis.md)
var options = new ParallelOptions { MaxDegreeOfParallelism = 10 };
await Parallel.ForEachAsync(requests, options, async (req, ct) => { ... });
```

**Score:** `dotnet-perf-skills: 5/5`, `no-skills: 4/5`  
**Verdict:** **dotnet-perf-skills** is more reliable in separating incident-level IO risks from secondary concerns.

### 5. Reflection and Serialization Overhead [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 4 | 4 |
| **Mean** | **4.0** | **4.0** |

#### Analysis

Both configurations detect uncached reflection and per-call `JsonSerializerOptions` effectively, with practical caching patterns.

```csharp
// dotnet-perf-skills (performance-analysis.md)
private static readonly ConcurrentDictionary<Type, PropertyInfo[]> s_propCache = new();
var properties = s_propCache.GetOrAdd(typeof(TTarget), t => t.GetProperties());
```

```csharp
// no-skills (performance-analysis.md)
private static readonly JsonSerializerOptions IndentedOptions = new() { WriteIndented = true };
```

**Score:** `dotnet-perf-skills: 4/5`, `no-skills: 4/5`  
**Verdict:** **Tie**. Both identify the key bottlenecks; neither strongly expands into partial-deserialization pathways (e.g., targeted `Utf8JsonReader`) beyond primary fixes.

### 6. Structural Optimization Detection [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 2 | 5 |
| **Mean** | **2.0** | **5.0** |

#### Analysis

`dotnet-perf-skills` is substantially more complete on structural perf opportunities: broad unsealed-class coverage, missing `IEquatable<T>` on structs, and FrozenDictionary candidates.

```csharp
// dotnet-perf-skills (performance-analysis.md)
public struct DeliveryResult : IEquatable<DeliveryResult>
{
    public bool Equals(DeliveryResult other) => Recipient == other.Recipient && Success == other.Success && SentAt == other.SentAt;
}
```

`no-skills` mentions structural items but under-covers unsealed classes (only a subset called out), reducing practical remediation coverage.

```csharp
// no-skills (performance-analysis.md)
// "Unsealed leaf classes Record, PipelineResult" (limited subset)
```

**Score:** `dotnet-perf-skills: 5/5`, `no-skills: 2/5`  
**Verdict:** **dotnet-perf-skills** is clearly better due to systematic, codebase-level structural detection.

### 7. Severity Classification Accuracy [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 3 | 5 |
| **Mean** | **3.0** | **5.0** |

#### Analysis

`dotnet-perf-skills` is more aligned with hot-path impact: per-line regex, `HttpClient` lifecycle, and loop string building are ranked above lower-impact hygiene items.

```csharp
// dotnet-perf-skills (performance-analysis.md)
// "Top 3 priorities: HttpClient reuse, hot-path regex, JsonSerializerOptions caching"
```

`no-skills` still prioritizes major issues but shows inconsistency in severity placement and occasional contradictory notes.

```csharp
// no-skills (performance-analysis.md)
// "new JsonSerializerOptions per call" note in NotificationService section despite default-options comment
```

**Score:** `dotnet-perf-skills: 5/5`, `no-skills: 3/5`  
**Verdict:** **dotnet-perf-skills** gives the more trustworthy priority order for engineering triage.

### 8. Fix Recommendation Quality [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 3 | 5 |
| **Mean** | **3.0** | **5.0** |

#### Analysis

`dotnet-perf-skills` recommendations are specific, API-accurate, and usually include usable replacement patterns and caveats.

```csharp
// dotnet-perf-skills (performance-analysis.md)
if (dict.TryGetValue(key, out var value))
    Use(value);
```

`no-skills` includes many actionable recommendations, but quality is less consistent and occasionally mixes weaker or ambiguous guidance.

```csharp
// no-skills (performance-analysis.md)
// Includes good guidance (IHttpClientFactory, HashSet, StringBuilder) but with occasional contradictory annotations
```

**Score:** `dotnet-perf-skills: 5/5`, `no-skills: 3/5`  
**Verdict:** **dotnet-perf-skills** is best due to higher precision and fewer misleading edges.

### 9. Quantitative Evidence & Coverage [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 3 | 5 |
| **Mean** | **3.0** | **5.0** |

#### Analysis

`dotnet-perf-skills` explicitly reports scan-hit counts, absence checks, and cross-cutting totals, which improves repeatability and confidence.

```text
# dotnet-perf-skills (performance-analysis.md)
RegexOptions.Compiled: 48
[GeneratedRegex]: 0
sealed class: 0
IEquatable: 0
```

`no-skills` provides broad findings but with less rigorous global counting and occasional count drift between sections.

```text
# no-skills (performance-analysis.md)
Cross-cutting detection is present, but aggregate count consistency is weaker
```

**Score:** `dotnet-perf-skills: 5/5`, `no-skills: 3/5`  
**Verdict:** **dotnet-perf-skills** better supports objective triage via measurable evidence.

### 10. Token Efficiency [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-perf-skills |
|---|---|---|
| 1 | 5 | 1 |
| **Mean** | **5.0** | **1.0** |

---

## Asset Usage Summary

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| no-skills | 1 | af06eebf…b044 | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills | 1 | — | — | — | — | ✅ |

---

## Raw Data References

- Per-run analysis: `reports/analysis-run-1.md`
- Verification data: `reports/verification-data.json`
- Score data: `reports/scores-data.json`
- Build notes: `reports/build-notes.md`
- Generation usage: `reports/generation-usage.json`
