# Comparative Analysis: dotnet-perf-skills, no-skills

This run compares **2 configurations** from `output/{config}/run-3/analyze-perf-issues/`: `dotnet-perf-skills` and `no-skills`. Both analyzed the same `analyze-perf-issues` scenario (Perf01 class library). `dotnet-perf-skills` explicitly reports use of the `analyzing-dotnet-performance` skill in `gen-notes.md`, while `no-skills` is the baseline configuration inferred from directory naming and its simpler `gen-notes.md`.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 5 |
| String Allocation Detection [CRITICAL] | 4 | 5 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 4 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 5 |
| Reflection and Serialization Overhead [HIGH] | 5 | 4 |
| Structural Optimization Detection [HIGH] | 5 | 4 |
| Severity Classification Accuracy [HIGH] | 3 | 5 |
| Fix Recommendation Quality [HIGH] | 5 | 4 |
| Evidence Quantification & Auditability [MEDIUM] | 5 | 3 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

Both configurations correctly identify per-call regex allocation, heavy `RegexOptions.Compiled`, and migration to `[GeneratedRegex]`.

```csharp
// dotnet-perf-skills (LogAnalyzer.cs / MarkdownStripper.cs)
var structured = new Regex(@"\[(\d{4}-\d{2}...)\]...");
private static readonly Regex HeaderPattern =
    new(@"^#{1,6}\s+", RegexOptions.Compiled | RegexOptions.Multiline);
[GeneratedRegex(@"^#{1,6}\s+", RegexOptions.Multiline)]
private static partial Regex HeaderPattern();
```

```csharp
// no-skills (LogAnalyzer.cs / TemplateEngine.cs / MarkdownStripper.cs)
var structured = new Regex(@"...");
private static readonly Regex ConditionalPattern = new(@"\{\{#if\s+(\w+)\}\}...");
[GeneratedRegex(@"^#{1,6}\s+", RegexOptions.Multiline)]
private static partial Regex HeaderPattern();
```

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**.  
**Verdict:** Tie; both catch the exact high-impact regex anti-patterns and recommend modern .NET 8+ source generation.

## 2. String Allocation Detection [CRITICAL]

Both detect `+=` loops, casing allocations, and replace-chain churn; baseline is more exhaustive per file/method.

```csharp
// dotnet-perf-skills (CsvParser.cs / LogAnalyzer.cs)
var sb = "";
foreach (var row in rows)
    sb += string.Join(...) + "\n";
// suggested: StringBuilder.Append(...)
```

```csharp
// no-skills (CsvParser.cs / SlugGenerator.cs)
var current = "";
for (int i = 0; i < line.Length; i++)
    current += line[i]; // O(n^2)
slug = slug.Replace("...", "...").Replace("...", "..."); // chained allocations
```

**Score:** dotnet-perf-skills **4/5**, no-skills **5/5**.  
**Verdict:** `no-skills` wins on breadth and hotspot specificity (char-by-char parser path, replace-chain compounding, loop-level casing allocations).

## 3. Collection and LINQ Efficiency [CRITICAL]

`dotnet-perf-skills` gives stronger quantified coverage for `ContainsKey`+indexer, `.ToList()` materialization, and `List.Contains` in loops.

```csharp
// dotnet-perf-skills (JsonTransformer.cs / LogAnalyzer.cs)
if (!groups.TryGetValue(key, out var list))
{
    list = new List<...>();
    groups[key] = list;
}
var timeSpan = errorEntries[i + 4].Timestamp - errorEntries[i].Timestamp;
```

```csharp
// no-skills (SlugGenerator.cs / JsonTransformer.cs)
var existing = existingSlugs.ToList();
while (existing.Contains($"{baseSlug}-{counter}")) counter++;
var allKeys = flat1.Keys.ToList();
if (!allKeys.Contains(key)) allKeys.Add(key);
```

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** `dotnet-perf-skills` is better due to clearer quantification and stronger hot-path framing (15 double-lookups, 18 materializations, sliding-window allocations).

## 4. Async and IO Pattern Detection [CRITICAL]

Both are strong and accurate: per-call `HttpClient`, sequential-await loops, unbounded fan-out, and cancellation propagation gaps.

```csharp
// dotnet-perf-skills (NotificationService.cs)
using var client = new HttpClient();
await Parallel.ForEachAsync(requests,
    new ParallelOptions { MaxDegreeOfParallelism = 10 },
    async (request, ct) => { /* ... */ });
```

```csharp
// no-skills (NotificationService.cs)
using var client = new HttpClient(); // per call in SendEmailAsync/SendSmsAsync/SendPushAsync
foreach (var request in requests)
    await SendAsync(request); // sequential latency stacking
```

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**.  
**Verdict:** Tie; both prioritize production-risk async/IO issues correctly and give actionable fixes.

## 5. Reflection and Serialization Overhead [HIGH]

Both detect uncached reflection and serializer option churn; skill-based output is stronger on serialization severity and quantitative impact.

```csharp
// dotnet-perf-skills (JsonTransformer.cs)
return JsonSerializer.Serialize(merged, new JsonSerializerOptions { WriteIndented = true });
private static readonly JsonSerializerOptions s_indentedOptions = new() { WriteIndented = true };
```

```csharp
// no-skills (EntityMapper.cs / TemplateEngine.cs)
var properties = typeof(TTarget).GetProperties();
property.SetValue(target, value);
var p = obj.GetType().GetProperty(part);
```

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** `dotnet-perf-skills` wins by pairing reflection + serialization with stronger performance impact framing and cache guidance.

## 6. Structural Optimization Detection [HIGH]

Both identify missing `IEquatable<T>`, unsealed classes, and `FrozenDictionary` opportunities; skill run is more systematic.

```csharp
// dotnet-perf-skills (project-wide structural scan)
// sealed class count: 0 of 17
public struct DeliveryResult : IEquatable<DeliveryResult> { ... }
private static readonly FrozenDictionary<string, string> ReplacementMap =
    new Dictionary<string, string> { ... }.ToFrozenDictionary();
```

```csharp
// no-skills (targeted structural findings)
// Unsealed leaf classes: DataPipeline.Record, ValidationResult, MappingConfig
public struct ValidationError // without IEquatable<T>
private static readonly Dictionary<string, string> ReplacementMap = new() { ... };
```

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** `dotnet-perf-skills` is more complete (full-class inventory plus explicit inverse checks) while baseline is good but less exhaustive.

## 7. Severity Classification Accuracy [HIGH]

Baseline severity ranking better matches hot-path-vs-moderate prioritization; skill run occasionally over-escalates moderate patterns.

```csharp
// dotnet-perf-skills classification examples
// Critical: new HttpClient per call ✅
// Critical: new Regex per-line hot path ✅
// Critical: ContainsKey + indexer at 15 sites (arguably high-moderate)
```

```csharp
// no-skills classification examples
// Critical: HttpClient per call, per-line regex, char-by-char +=, uncached reflection ✅
// Moderate/Info: ContainsKey + indexer, capacity hints, minor boxing ✅
```

**Score:** dotnet-perf-skills **3/5**, no-skills **5/5**.  
**Verdict:** `no-skills` is more calibrated to practical triage, keeping truly critical issues above broad but lower-impact cleanup items.

## 8. Fix Recommendation Quality [HIGH]

Both provide concrete APIs and patterns; skill run is slightly more implementation-ready and caveat-aware.

```csharp
// dotnet-perf-skills
[GeneratedRegex(@"[àáâãäå]")]
private static partial Regex DiacriticARegex();
private static readonly HttpClient s_httpClient = new(new SocketsHttpHandler
{
    PooledConnectionLifetime = TimeSpan.FromMinutes(5)
});
```

```csharp
// no-skills
var existing = new HashSet<string>(existingSlugs);
while (existing.Contains($"{baseSlug}-{counter}")) counter++;
private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _propertyCache = new();
```

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** `dotnet-perf-skills` gives slightly higher-actionability fixes with stronger modern API targeting across categories.

## 9. Evidence Quantification & Auditability [MEDIUM]

Skill output includes a reproducible scan matrix with hit counts; baseline is detailed but less auditable as a checklist artifact.

```csharp
// dotnet-perf-skills scan evidence
// new Regex(: 8
// RegexOptions.Compiled: 48
// [GeneratedRegex]: 0
// ContainsKey + indexer: 15
// ToLower/ToUpper: 19
```

```csharp
// no-skills evidence style
// Per-file tables with many findings and line ranges,
// but no explicit scan recipe matrix with hit counts.
```

**Score:** dotnet-perf-skills **5/5**, no-skills **3/5**.  
**Verdict:** `dotnet-perf-skills` is clearly stronger for repeatability, reviewability, and coverage verification.

## Weighted Summary

Weights applied: **Critical ×3**, **High ×2**, **Medium ×1**, **Low ×0.5**

| Configuration | Critical Subtotal | High Subtotal | Medium Subtotal | Weighted Total |
|---|---:|---:|---:|---:|
| dotnet-perf-skills | 57 | 36 | 5 | **98** |
| no-skills | 57 | 34 | 3 | **94** |

## What All Versions Get Right

- Both flag the highest-risk runtime issue: `new HttpClient()` per call in `NotificationService`.
- Both catch hot-path regex anti-patterns in `LogAnalyzer.TryParseLine` and recommend `[GeneratedRegex]` migration.
- Both identify O(n²) string growth from loop concatenation and recommend `StringBuilder`.
- Both detect collection anti-patterns (`ContainsKey` + indexer, `List.Contains`, unnecessary `.ToList()`).
- Both include reflection caching and serializer options caching as important optimizations.

## Summary: Impact of Skills

1. **Most impactful improvement with skills:** measurable auditability (scan checklist + hit counts) and more systematic project-wide structural/regex coverage.
2. **Most impactful baseline advantage:** more conservative severity calibration that better separates critical production risks from moderate cleanup tasks.
3. **Overall assessment:** `dotnet-perf-skills` delivers the stronger report for execution planning and reproducibility (**98 vs 94** weighted), while `no-skills` remains highly useful and especially strong in practical severity prioritization.
