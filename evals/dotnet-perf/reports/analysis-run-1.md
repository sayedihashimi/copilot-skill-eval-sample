# Comparative Analysis: dotnet-perf-skills, no-skills

This run compares **2 configurations** (`dotnet-perf-skills`, `no-skills`) for the same app scenario at `output/{config}/run-1/analyze-perf-issues/`. Both produced `performance-analysis.md` and `gen-notes.md`; `dotnet-perf-skills` explicitly reports the `analyzing-dotnet-performance` skill and a structured detection workflow, while `no-skills` represents baseline Copilot behavior.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 4 |
| String Allocation Detection [CRITICAL] | 5 | 4 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 4 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 4 |
| Reflection and Serialization Overhead [HIGH] | 4 | 4 |
| Structural Optimization Detection [HIGH] | 5 | 2 |
| Severity Classification Accuracy [HIGH] | 5 | 3 |
| Fix Recommendation Quality [HIGH] | 5 | 3 |
| Quantitative Evidence & Coverage [HIGH] | 5 | 3 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

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

## 2. String Allocation Detection [CRITICAL]

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

## 3. Collection and LINQ Efficiency [CRITICAL]

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

## 4. Async and IO Pattern Detection [CRITICAL]

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

## 5. Reflection and Serialization Overhead [HIGH]

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

## 6. Structural Optimization Detection [HIGH]

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

## 7. Severity Classification Accuracy [HIGH]

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

## 8. Fix Recommendation Quality [HIGH]

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

## 9. Quantitative Evidence & Coverage [HIGH]

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

## Weighted Summary

Scoring weights: **Critical ×3**, **High ×2**, **Medium ×1**, **Low ×0.5**.

| Configuration | Critical subtotal | High subtotal | Medium subtotal | Low subtotal | Total weighted score |
|---|---:|---:|---:|---:|---:|
| dotnet-perf-skills | (5+5+5+5)×3 = 60 | (4+5+5+5+5)×2 = 48 | 0 | 0 | **108** |
| no-skills | (4+4+4+4)×3 = 48 | (4+2+3+3+3)×2 = 30 | 0 | 0 | **78** |

## What All Versions Get Right

- Both identify `new HttpClient()` lifecycle misuse and its socket exhaustion risk.
- Both detect hot-path regex instantiation (`LogAnalyzer.TryParseLine`) and recommend caching/source generation.
- Both flag O(n²) string-concatenation loops and propose `StringBuilder`.
- Both catch collection inefficiencies (`List.Contains`, `ContainsKey+indexer`, avoidable `.ToList()` materialization).
- Both recognize async batch-shape issues (sequential awaits vs unbounded fan-out) and suggest bounded parallelism.

## Summary: Impact of Skills

Most impactful differences, in order: **(1)** stronger structural detection breadth, **(2)** better severity ranking consistency, **(3)** better quantitative scan rigor, and **(4)** cleaner fix specificity.  
Overall, `dotnet-perf-skills` delivers a materially higher-confidence analysis for perf triage (**108 vs 78 weighted**), while `no-skills` remains useful but less consistent and less comprehensive in high-impact prioritization.
