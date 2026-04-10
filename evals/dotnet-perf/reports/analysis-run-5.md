# Comparative Analysis: no-skills, dotnet-perf-skills

This run compares **2 configurations** over **1 shared scenario**: `analyze-perf-issues` at `output/{config}/run-5/analyze-perf-issues/`. Configuration identity came from each scenario’s `gen-notes.md`: `no-skills` is baseline Copilot output, and `dotnet-perf-skills` explicitly used the `analyzing-dotnet-performance` plugin skill.

## Executive Summary

| Dimension [Tier] | no-skills | dotnet-perf-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 4 | 5 |
| String Allocation Detection [CRITICAL] | 4 | 5 |
| Collection and LINQ Efficiency [CRITICAL] | 4 | 5 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 5 |
| Reflection and Serialization Overhead [HIGH] | 4 | 5 |
| Structural Optimization Detection [HIGH] | 4 | 5 |
| Severity Classification Accuracy [HIGH] | 3 | 5 |
| Fix Recommendation Quality [HIGH] | 4 | 4 |
| Evidence Quantification and Scan Rigor [HIGH] | 3 | 5 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**no-skills excerpt** (`output/no-skills/run-5/analyze-perf-issues/performance-analysis.md`)
> **`new Regex` per log line** in `TryParseLine` — called for every line in the log.  
> ...with millions of lines this is catastrophic.  
> **40+ `RegexOptions.Compiled` static instances** ... should use `[GeneratedRegex]`.

**dotnet-perf-skills excerpt** (`output/dotnet-perf-skills/run-5/analyze-perf-issues/performance-analysis.md`)
> `new Regex(` per-call | **8** | LogAnalyzer: 4, TemplateEngine: 2, ValidationEngine: 1, CsvParser: 1  
> `RegexOptions.Compiled` | **48** | MarkdownStripper: 48  
> `[GeneratedRegex]` | **0** | None used — **0/48 ratio**

**Score:** no-skills **4/5**; dotnet-perf-skills **5/5**.  
**Verdict:** **dotnet-perf-skills** is best due to explicit counts, startup-budget framing, and direct .NET 8 `[GeneratedRegex]` guidance.

## 2. String Allocation Detection [CRITICAL]

**no-skills excerpt** (`output/no-skills/run-5/analyze-perf-issues/performance-analysis.md`)
> **Character-by-character `+=` on string** in `ParseLine` and `SplitLines`...  
> **String `+=` in loop** inside `ProcessLoops` — O(n²) allocation pattern.  
> `.ToLower()` uses the current thread culture, leading to the Turkish-I bug.

**dotnet-perf-skills excerpt** (`output/dotnet-perf-skills/run-5/analyze-perf-issues/performance-analysis.md`)
> `+=` string concatenation (in-loop sites) | **35** | Across 7 files  
> `.ToLower()/.ToUpper()` without culture | **16** | Across 6 files  
> Chained `.Replace()` allocations ... **47 sequential Regex.Replace calls**

**Score:** no-skills **4/5**; dotnet-perf-skills **5/5**.  
**Verdict:** **dotnet-perf-skills** leads on breadth and quantification while still covering the same core anti-patterns.

## 3. Collection and LINQ Efficiency [CRITICAL]

**no-skills excerpt** (`output/no-skills/run-5/analyze-perf-issues/performance-analysis.md`)
> `allKeys.ToList()` + `.Contains()` ... O(n) per lookup makes the key-union loop O(n²).  
> `ContainsKey` + indexer ... double lookup.  
> `Skip(i).Take(5).ToList()` in sliding window — allocates a new list on every iteration.

**dotnet-perf-skills excerpt** (`output/dotnet-perf-skills/run-5/analyze-perf-issues/performance-analysis.md`)
> `ContainsKey` sites (double-lookup candidates) | **13** | Across 6 files  
> `.ToList()` materializations | **18** | Across 6 files  
> `List.Contains()` O(n) in loop — O(n²) total ... SlugGenerator.cs:L75–L81

**Score:** no-skills **4/5**; dotnet-perf-skills **5/5**.  
**Verdict:** **dotnet-perf-skills** wins by explicitly tying each collection issue to count and impact.

## 4. Async and IO Pattern Detection [CRITICAL]

**no-skills excerpt** (`output/no-skills/run-5/analyze-perf-issues/performance-analysis.md`)
> **`new HttpClient` per call** ... Leads to **socket exhaustion** in production  
> **Sequential `await` in loop** in `SendBatchAsync`  
> **Unbounded parallelism** in `SendBatchParallelAsync` ... 10,000 concurrent HTTP connections

**dotnet-perf-skills excerpt** (`output/dotnet-perf-skills/run-5/analyze-perf-issues/performance-analysis.md`)
> `new HttpClient(` | **3** | NotificationService: 3  
> Sequential Awaits in Loop ... `SendBatchAsync`  
> Unbounded Parallelism ... Missing Cancellation Tokens ... `Task.Delay` has no `CancellationToken`

**Score:** no-skills **5/5**; dotnet-perf-skills **5/5**.  
**Verdict:** **Tie**. Both outputs clearly identify the major async/IO production risks and provide actionable remediation patterns.

## 5. Reflection and Serialization Overhead [HIGH]

**no-skills excerpt** (`output/no-skills/run-5/analyze-perf-issues/performance-analysis.md`)
> `typeof(TTarget).GetProperties()` on every call ... should be cached per type.  
> `new JsonSerializerOptions` on every call ... instantiation is expensive.

**dotnet-perf-skills excerpt** (`output/dotnet-perf-skills/run-5/analyze-perf-issues/performance-analysis.md`)
> Uncached `new JsonSerializerOptions` Per Call (**4 instances**)  
> **Impact:** Up to **592x slower** than cached options  
> Uncached Reflection — `GetProperties()`/`GetValue()`/`SetValue()` (**5 instances**)

**Score:** no-skills **4/5**; dotnet-perf-skills **5/5**.  
**Verdict:** **dotnet-perf-skills** is stronger because it quantifies impact and distinguishes repeated hot-path reflection operations.

## 6. Structural Optimization Detection [HIGH]

**no-skills excerpt** (`output/no-skills/run-5/analyze-perf-issues/performance-analysis.md`)
> `ValidationError` struct without `IEquatable<ValidationError>`  
> Static `Dictionary` could be `FrozenDictionary` on .NET 8+  
> Unsealed `Record` class ... JIT cannot devirtualize

**dotnet-perf-skills excerpt** (`output/dotnet-perf-skills/run-5/analyze-perf-issues/performance-analysis.md`)
> `sealed class` | **0** | 0/17 classes sealed  
> `: IEquatable` on structs | **0** | 0/2 structs implement it  
> Static `readonly Dictionary` — FrozenDictionary Candidates (**2 instances**)

**Score:** no-skills **4/5**; dotnet-perf-skills **5/5**.  
**Verdict:** **dotnet-perf-skills** is better due to full inverse-coverage ratios (0/x) and codebase-wide structural accounting.

## 7. Severity Classification Accuracy [HIGH]

**no-skills excerpt** (`output/no-skills/run-5/analyze-perf-issues/gen-notes.md`)
> Classified **55+ findings** by severity (**8 Critical, 24 Moderate, 23+ Info**)

**dotnet-perf-skills excerpt** (`output/dotnet-perf-skills/run-5/analyze-perf-issues/gen-notes.md`)
> Severity classification framework:  
> 🔴 Critical: >10x regression or production incident risk  
> 🟡 Moderate: 2–10x regression, measurable at scale  
> ℹ️ Info: Best practice, minor savings

**Score:** no-skills **3/5**; dotnet-perf-skills **5/5**.  
**Verdict:** **dotnet-perf-skills** is best; severity labels are explicitly rule-driven and consistently tied to impact and scale, while no-skills is broader but less calibrated.

## 8. Fix Recommendation Quality [HIGH]

**no-skills excerpt** (`output/no-skills/run-5/analyze-perf-issues/performance-analysis.md`)
> inject `IHttpClientFactory` or use a single static client  
> use `SemaphoreSlim` throttling ... `Task.WhenAll`  
> convert to `[GeneratedRegex]`

**dotnet-perf-skills excerpt** (`output/dotnet-perf-skills/run-5/analyze-perf-issues/performance-analysis.md`)
> use `[GeneratedRegex]` partial methods on .NET 7+  
> use `Parallel.ForEachAsync` with `MaxDegreeOfParallelism`  
> use `ConcurrentDictionary<Type, PropertyInfo[]>` caching

**Score:** no-skills **4/5**; dotnet-perf-skills **4/5**.  
**Verdict:** **Tie**. Both are specific and practical with concrete API-level fixes and code patterns.

## 9. Evidence Quantification and Scan Rigor [HIGH]

**no-skills excerpt** (`output/no-skills/run-5/analyze-perf-issues/performance-analysis.md`)
> Found **55+ performance anti-patterns** ...  
> (detailed file sections, but mostly narrative totals)

**dotnet-perf-skills excerpt** (`output/dotnet-perf-skills/run-5/analyze-perf-issues/performance-analysis.md`)
> ## Scan Execution Checklist  
> `.ToLower()/.ToUpper()` without culture | **16**  
> `RegexOptions.Compiled` | **48**  
> `new List/Dictionary<` per-call allocations | **33**

**Score:** no-skills **3/5**; dotnet-perf-skills **5/5**.  
**Verdict:** **dotnet-perf-skills** is substantially stronger on reproducibility and auditability via systematic recipe counts.

## Weighted Summary

Weights used: **Critical ×3**, **High ×2**, **Medium ×1**, **Low ×0.5**.

| Dimension | Tier | Weight | no-skills | dotnet-perf-skills |
|---|---|---:|---:|---:|
| Regex Anti-Pattern Detection | Critical | 3 | 12 | 15 |
| String Allocation Detection | Critical | 3 | 12 | 15 |
| Collection and LINQ Efficiency | Critical | 3 | 12 | 15 |
| Async and IO Pattern Detection | Critical | 3 | 15 | 15 |
| Reflection and Serialization Overhead | High | 2 | 8 | 10 |
| Structural Optimization Detection | High | 2 | 8 | 10 |
| Severity Classification Accuracy | High | 2 | 6 | 10 |
| Fix Recommendation Quality | High | 2 | 8 | 8 |
| Evidence Quantification and Scan Rigor | High | 2 | 6 | 10 |
| **Total Weighted Score** |  |  | **87** | **108** |

## What All Versions Get Right

- Both identify the highest-impact incidents: per-call `HttpClient`, per-call regex in hot paths, and loop-based string concatenation.
- Both call out culture-sensitive casing (`ToLower`/`ToUpper`) and recommend ordinal/invariant alternatives.
- Both detect reflection and serialization overhead in `EntityMapper`/`JsonTransformer`.
- Both provide concrete API-level remediation patterns (e.g., `StringBuilder`, `TryGetValue`, `[GeneratedRegex]`, throttled async).

## Summary: Impact of Skills

Most impactful differences, ranked:
1. **Quantified scan rigor** (explicit recipe hit counts and 0/x inverse checks)  
2. **Severity calibration quality** (rule-based prioritization tied to impact/scale)  
3. **Coverage depth consistency** across regex, structural, and collection anti-patterns

Overall, `dotnet-perf-skills` is the stronger configuration for this scenario by weighted score (**108 vs 87**), mainly because it is more systematic and defensible while preserving the same practical fix actionability.
