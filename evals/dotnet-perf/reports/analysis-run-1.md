# Comparative Analysis: dotnet-perf-skills, no-skills

Two configuration outputs were compared under `output/{config}/run-1/analyze-perf-issues/` for the same scenario (`analyze-perf-issues`). `dotnet-perf-skills` is explicitly identified in `gen-notes.md` via `analyzing-dotnet-performance` skill usage, while `no-skills` is inferred from directory naming (its `gen-notes.md` is a short run summary). Both produced `performance-analysis.md`, but the skills run is more calibrated and structured for this benchmark’s priority dimensions.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 4 |
| String Allocation Detection [CRITICAL] | 5 | 4 |
| Collection and LINQ Efficiency [CRITICAL] | 4 | 4 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 5 |
| Reflection and Serialization Overhead [HIGH] | 5 | 4 |
| Structural Optimization Detection [HIGH] | 5 | 3 |
| Severity Classification Accuracy [HIGH] | 3 | 4 |
| Fix Recommendation Quality [HIGH] | 5 | 4 |
| Evidence Coverage & Traceability [MEDIUM] | 5 | 4 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

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

## 2. String Allocation Detection [CRITICAL]

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

## 3. Collection and LINQ Efficiency [CRITICAL]

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

## 4. Async and IO Pattern Detection [CRITICAL]

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

## 5. Reflection and Serialization Overhead [HIGH]

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

## 6. Structural Optimization Detection [HIGH]

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

## 7. Severity Classification Accuracy [HIGH]

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

## 8. Fix Recommendation Quality [HIGH]

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

## 9. Evidence Coverage & Traceability [MEDIUM]

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

## Weighted Summary

Weights used: Critical ×3, High ×2, Medium ×1.

| Configuration | Critical subtotal | High subtotal | Medium subtotal | Total weighted |
|---|---:|---:|---:|---:|
| dotnet-perf-skills | (5+5+4+5)×3 = 57 | (5+5+3+5)×2 = 36 | 5×1 = 5 | **98** |
| no-skills | (4+4+4+5)×3 = 51 | (4+3+4+4)×2 = 30 | 4×1 = 4 | **85** |

## What All Versions Get Right

- Both identify the biggest production risks: per-call `HttpClient`, hot-path regex construction, and loop-based string concatenation.
- Both recommend modern .NET APIs (`[GeneratedRegex]`, `StringBuilder`, `HashSet`, `IHttpClientFactory`, `TryGetValue`).
- Both include concrete file/line-level references and actionable fixes instead of generic “optimize this” guidance.

## Summary: Impact of Skills

Most impactful differences: **(1)** stronger regex/systemic structural detection, **(2)** better cross-cutting count-based evidence, and **(3)** more standardized, implementation-ready fix guidance in `dotnet-perf-skills`. Overall ranking by weighted score: **dotnet-perf-skills (98) > no-skills (85)**. `no-skills` remains strong and is sometimes better at severity restraint, but the skills configuration delivers the more complete and benchmark-aligned analysis package.
