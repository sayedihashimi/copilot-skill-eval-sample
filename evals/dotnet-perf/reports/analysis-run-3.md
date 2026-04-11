# Comparative Analysis: dotnet-perf-skills, no-skills

This run compares **2 configurations** (`dotnet-perf-skills`, `no-skills`) across **1 shared scenario**: `analyze-perf-issues` at `output/{config}/run-3/analyze-perf-issues/performance-analysis.md`. Configuration identity was confirmed via each scenario's `gen-notes.md` (skills-enabled vs baseline).

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 4 |
| String Allocation Detection [CRITICAL] | 5 | 4 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 4 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 4 |
| Reflection and Serialization Overhead [HIGH] | 4 | 3 |
| Structural Optimization Detection [HIGH] | 5 | 4 |
| Severity Classification Accuracy [HIGH] | 5 | 3 |
| Fix Recommendation Quality [HIGH] | 5 | 4 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills excerpt** (`output/dotnet-perf-skills/run-3/analyze-perf-issues/performance-analysis.md`)
> `new Regex(` (per-call) | **8** | TemplateEngine (2), LogAnalyzer (4), ValidationEngine (1), CsvParser (1)  
> `RegexOptions.Compiled` | **48** | All in MarkdownStripper.cs (48 static fields)  
> `[GeneratedRegex]` | **0** | None used — 0/48 compiled regex use source generators

**no-skills excerpt** (`output/no-skills/run-3/analyze-perf-issues/performance-analysis.md`)
> **`new Regex(...)` per log line** in `TryParseLine` — if parsing 1M lines, this creates 1M regex objects.  
> **46 `RegexOptions.Compiled` instances** — each one JIT-compiles at first use, blowing the compiled-regex startup budget.  
> **Missing `[GeneratedRegex]`**: No file uses the .NET 7+ source generator

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best: it is more systematic (explicit counts/checklist and stronger source-generator framing).

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills excerpt** (`.../dotnet-perf-skills/.../performance-analysis.md`)
> String `+=` Concatenation in Loops — O(n²) Allocation (7 sites)  
> `.ToLower()`/`.ToUpper()` Without Culture or StringComparison (18 instances)  
> Chained `.Replace()` in SlugGenerator (9 allocations per call)

**no-skills excerpt** (`.../no-skills/.../performance-analysis.md`)
> Char-by-char `current += line[i]` in `ParseLine` — O(n²) allocations for every CSV line.  
> Same `+=` pattern in `SplitLines` — O(n²) for the entire file content.  
> `.ToLower()` without `StringComparison.OrdinalIgnoreCase` — allocates on every header key

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best for breadth (loop concatenation + casing + chained replace allocations together).

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills excerpt**
> `ContainsKey` (double-lookup candidates) | **10** | EntityMapper (5), LogAnalyzer (4), ValidationEngine (1)  
> `.ToList()` | **18** | Across 5 files — many unnecessary materializations  
> `.Skip(i).Take(5).ToList()` in Loop (1 instance)

**no-skills excerpt**
> `allKeys` built as `List<string>` with `.Contains()` — O(n) per lookup. Should be `HashSet<string>`.  
> `errorEntries.Skip(i).Take(5).ToList()` inside a loop — creates new list on every iteration.  
> `ContainsKey` + indexer double-lookup ... `TryGetValue` suffices.

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best: same core findings plus clearer quantified prioritization.

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills excerpt**
> `new HttpClient(` | **3** | All in NotificationService.cs  
> Sequential Awaits in Loop — No Parallelism (1 site)  
> Unbounded Parallelism in `SendBatchParallelAsync` (1 site)  
> Missing CancellationToken on Async Methods (all async methods)

**no-skills excerpt**
> **`new HttpClient()` per call** ... exhausts sockets  
> **Unbounded parallelism** ... creates 10k concurrent HTTP connections  
> Sequential `await` in `SendBatchAsync` loop — no parallelism at all.  
> `Task.Delay` in retry loop has no `CancellationToken`

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best due to tighter severity framing and complete propagation guidance.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills excerpt**
> Uncached `new JsonSerializerOptions` Per Call (4 instances)  
> Uncached Reflection in Hot Path — `GetProperties()`/`SetValue()`/`GetValue()` (4 sites)  
> Cache property accessors per type using `ConcurrentDictionary<Type, PropertyInfo[]>`

**no-skills excerpt**
> `new JsonSerializerOptions { WriteIndented = true }` on every call.  
> `typeof(TTarget).GetProperties()` and `prop.SetValue()` called on every invocation of `MapTo<T>`.  
> Same uncached `GetProperties()` and `GetValue()` in `MapFrom<T>`.

**Score:** dotnet-perf-skills **4/5**; no-skills **3/5**.  
**Verdict:** **dotnet-perf-skills** is best; both catch the major issues, but skills output is more explicit about hot-path impact and remediation structure.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills excerpt**
> `public class` (unsealed, non-abstract) | **18** | 0 sealed out of 18 classes  
> `public struct` without `IEquatable<T>` | **2** | DeliveryResult, ValidationError  
> `static readonly Dictionary<` | **2** ... FrozenDictionary candidates

**no-skills excerpt**
> `ValidationError` struct does not implement `IEquatable<ValidationError>`  
> `MappingConfig` class is not sealed.  
> `ReplacementMap` is static and never mutated — `FrozenDictionary` candidate on .NET 8+.

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best because it demonstrates full coverage with baseline/absence counts.

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills excerpt**
> 🔴 Critical | 12 | `new HttpClient()` per call ... per-call `new Regex()` in hot loops ... string `+=` in loops  
> 🟡 Moderate | 32 | ... `RegexOptions.Compiled` budget (48 instances) ...  
> Top 3 priorities: (1) HttpClient, (2) per-call Regex, (3) JsonSerializerOptions

**no-skills excerpt**
> 🔴 Critical | 6 | ...  
> 🟡 Moderate | 19 | ...  
> `new JsonSerializerOptions` ... | 🟡 Moderate  
> `Task.Delay` in retry loop has no `CancellationToken` | ℹ️ Info

**Score:** dotnet-perf-skills **5/5**; no-skills **3/5**.  
**Verdict:** **dotnet-perf-skills** is best; it separates hot-path criticals from startup/moderate issues more consistently.

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills excerpt**
> Fix: Inject `IHttpClientFactory` or use a single `static readonly HttpClient` ...  
> Fix: Hoist to `static readonly` fields, or use `[GeneratedRegex]` on .NET 7+  
> Fix: Use `StringComparison.OrdinalIgnoreCase` ... `ToLowerInvariant()` when needed

**no-skills excerpt**
> Fix for #43 — inject `IHttpClientFactory` or use static client  
> Fix for #40 — use `[GeneratedRegex]` (requires `partial class`)  
> Fix for #39 — use `HashSet<string>`

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best: highly actionable API-specific fixes with better prioritization alignment.

## Weighted Summary

Weights used: **Critical ×3**, **High ×2** (no medium/low dimensions in this rubric).

| Configuration | Critical subtotal (4 dims) | High subtotal (4 dims) | Total weighted score |
|---|---:|---:|---:|
| dotnet-perf-skills | (5+5+5+5)×3 = 60 | (4+5+5+5)×2 = 38 | **98** |
| no-skills | (4+4+4+4)×3 = 48 | (3+4+3+4)×2 = 28 | **76** |

## What All Versions Get Right

- Correctly identify the highest-risk async issue: `new HttpClient()` per call and socket-exhaustion risk.
- Detect regex misuse in hot paths, including per-call `new Regex(...)` in log parsing.
- Flag O(n²) string construction patterns (`string +=` in loops) and recommend `StringBuilder`.
- Call out dictionary/list lookup inefficiencies (`ContainsKey`+indexer, `List.Contains` vs `HashSet`).
- Provide concrete .NET-specific fixes (`IHttpClientFactory`, `[GeneratedRegex]`, `TryGetValue`, cached `JsonSerializerOptions`).

## Summary: Impact of Skills

**Ranking of impact:** 1) Severity/prioritization quality, 2) Regex/string/collections completeness, 3) Structural and quantified coverage.  
Overall, `dotnet-perf-skills` is the stronger output (**98 vs 76**) because it is more systematic, better tiered by runtime impact, and more complete on cross-cutting anti-pattern detection. `no-skills` is still solid and actionable, but comparatively less consistent in severity calibration and breadth quantification.
