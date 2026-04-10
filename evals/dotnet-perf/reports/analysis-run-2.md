# Comparative Analysis: dotnet-perf-skills, no-skills

This run compares **2 configurations** (`dotnet-perf-skills`, `no-skills`) on the same scenario app at `output/{config}/run-2/analyze-perf-issues/`. Configuration metadata comes from each run’s `gen-notes.md`: `dotnet-perf-skills` used the **analyzing-dotnet-performance** skill workflow, while `no-skills` is the baseline run without that skill orchestration.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 5 |
| String Allocation Detection [CRITICAL] | 4 | 5 |
| Collection and LINQ Efficiency [CRITICAL] | 4 | 5 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 5 |
| Reflection and Serialization Overhead [HIGH] | 5 | 4 |
| Structural Optimization Detection [HIGH] | 4 | 5 |
| Severity Classification Accuracy [HIGH] | 4 | 4 |
| Fix Recommendation Quality [HIGH] | 5 | 4 |
| Evidence Quantification & Auditability [MEDIUM] | 5 | 3 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

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

## 2. String Allocation Detection [CRITICAL]

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

## 3. Collection and LINQ Efficiency [CRITICAL]

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

## 4. Async and IO Pattern Detection [CRITICAL]

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

## 5. Reflection and Serialization Overhead [HIGH]

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

## 6. Structural Optimization Detection [HIGH]

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

## 7. Severity Classification Accuracy [HIGH]

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

## 8. Fix Recommendation Quality [HIGH]

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

## 9. Evidence Quantification & Auditability [MEDIUM]

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

## Weighted Summary

Weights: **Critical ×3**, **High ×2**, **Medium ×1**, **Low ×0.5**

| Configuration | Weighted Total |
|---|---:|
| dotnet-perf-skills | **95** |
| no-skills | **97** |

`no-skills` narrowly leads on weighted score due to broader hotspot-specific coverage in string/collections/structural dimensions, while `dotnet-perf-skills` leads on evidence rigor and fix precision.

## What All Versions Get Right

- Both identify the most dangerous production issue: `new HttpClient()` per call.
- Both recognize per-line/per-call regex allocation and recommend `[GeneratedRegex]` on .NET 8+.
- Both call out O(n²) string growth patterns and `StringBuilder` replacements.
- Both detect collection lookup anti-patterns (`List.Contains`, `ContainsKey` + indexer).
- Both include reflection caching and serializer-options caching as material optimization opportunities.

## Summary: Impact of Skills

1. **Most impactful difference:** `dotnet-perf-skills` improves auditability and deterministic coverage (scan checklist + hit counts).  
2. **Most impactful baseline advantage:** `no-skills` provides denser scenario-specific hotspot mapping and slightly stronger structural targeting.  
3. **Overall assessment:** Both outputs are high quality and production-useful; `no-skills` edges total weighted score (**97 vs 95**), while `dotnet-perf-skills` is the more systematic and repeatable analysis workflow.
