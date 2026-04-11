# Comparative Analysis: dotnet-perf-skills, no-skills

This run compares **2 configurations** on **1 shared scenario**: `analyze-perf-issues` (`output/{config}/run-1/analyze-perf-issues/`). Configuration identity was taken from `gen-notes.md` where present and directory naming otherwise: `dotnet-perf-skills` (performance skill-guided analysis) and `no-skills` (baseline).

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

**dotnet-perf-skills excerpt**  
> **dotnet-perf-skills / performance-analysis.md**  
> "`new Regex(` (uncached per-call) | 8"  
> "`RegexOptions.Compiled` | 48"  
> "Fix: ... preferably `[GeneratedRegex]` ... .NET 8."  
> "48 `RegexOptions.Compiled` ... adding ~50-100ms to cold start."

**no-skills excerpt**  
> **no-skills / performance-analysis.md**  
> "`new Regex(...)` on every log line in `TryParseLine`"  
> "`MarkdownStripper` has 45+ compiled regexes ... should use `[GeneratedRegex]`"  
> "Three distinct sub-patterns: Per-call `new Regex()`, static `Regex.Replace()`, excessive `RegexOptions.Compiled`."

**Scores:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
dotnet-perf-skills is more complete and quantified (explicit 48 count + startup budget framing); baseline catches the same classes but with less rigor/precision.

**Verdict:** **dotnet-perf-skills** is best due to stronger quantification and clearer .NET 8 `[GeneratedRegex]` prioritization.

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills excerpt**  
> **dotnet-perf-skills / performance-analysis.md**  
> "String `+=` concatenation in loops — O(n²) allocation (6 sites)"  
> "`.ToLower()` / `.ToUpper()` without culture ... 18 instances"  
> "`MarkdownStripper.StripMarkdown` — 47 chained `.Replace()` allocations per call"

**no-skills excerpt**  
> **no-skills / performance-analysis.md**  
> "Char-by-char `string +=` in `ParseLine` ... O(n²)."  
> "`.ToLower()` is culture-sensitive and allocates."  
> "`45+` chained `.Replace()` calls in `StripMarkdown` ... allocates a new string each pass."

**Scores:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
Both catch core issues; skill-guided output gives broader coverage and stronger cross-file synthesis.

**Verdict:** **dotnet-perf-skills**.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills excerpt**  
> **dotnet-perf-skills / performance-analysis.md**  
> "`ContainsKey` + indexer double-lookup (12 instances)"  
> "`ToList()` + `List.Contains()` ... O(n) per lookup"  
> "`Skip(i).Take(5).ToList()` in a loop — O(n²) LINQ"  
> "`List<T>` / `Dictionary<T>` without capacity hints"

**no-skills excerpt**  
> **no-skills / performance-analysis.md**  
> "`allKeys.ToList()` + `.Contains()` ... Should use `HashSet<string>`."  
> "`ContainsKey` + indexer ... Use `TryGetValue`."  
> "`.Distinct().ToList()` ... Could use a `HashSet<string>` from the start."  
> "`errorEntries.Skip(i).Take(5).ToList()` ... O(n²) total."

**Scores:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
Baseline coverage is good; skill-guided output is tighter in severity and breadth.

**Verdict:** **dotnet-perf-skills**.

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills excerpt**  
> **dotnet-perf-skills / performance-analysis.md**  
> "`new HttpClient()` per call — socket exhaustion risk (3 instances)"  
> "Sequential `await` in batch loop"  
> "Unbounded parallelism in `SendBatchParallelAsync`"  
> "Missing `CancellationToken` on async methods (all async methods)"

**no-skills excerpt**  
> **no-skills / performance-analysis.md**  
> "`new HttpClient()` per call ... production incident risk."  
> "Sequential `await` in `SendBatchAsync` loop"  
> "Unbounded parallelism ... 10K concurrent HTTP calls"  
> "`Task.Delay` without `CancellationToken` in retry loop."

**Scores:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
Both are strong; skill-guided output better integrates cancellation and prioritization context.

**Verdict:** **dotnet-perf-skills**.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills excerpt**  
> **dotnet-perf-skills / performance-analysis.md**  
> "Uncached `new JsonSerializerOptions` per call ... Up to 592x slower than cached options."  
> "Uncached reflection ... `GetProperties()`/`GetProperty()`/`SetValue()`/`GetValue()`."

**no-skills excerpt**  
> **no-skills / performance-analysis.md**  
> "`new JsonSerializerOptions` on every call ... expensive."  
> "`GetProperties()` + `SetValue()` on every call ... Reflection is ~100x slower."

**Scores:** dotnet-perf-skills **4/5**, no-skills **3/5**.  
Both identify key anti-patterns; neither substantially develops partial parsing (`Utf8JsonReader`) alternatives. Skill-guided output is better quantified.

**Verdict:** **dotnet-perf-skills**.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills excerpt**  
> **dotnet-perf-skills / performance-analysis.md**  
> "Unsealed leaf classes — 0 of 18 classes are sealed"  
> "Structs without `IEquatable<T>` (2 of 2 structs)"  
> "`static readonly Dictionary<>` — FrozenDictionary candidates (2 instances)"

**no-skills excerpt**  
> **no-skills / performance-analysis.md**  
> "Missing `IEquatable<T>` on Structs (2 files)"  
> "Unsealed Leaf Classes (3 files)"  
> "Static `Dictionary` ... candidate for `FrozenDictionary`."

**Scores:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
Baseline detects all structural themes but with less complete class-level accounting than skill-guided output.

**Verdict:** **dotnet-perf-skills**.

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills excerpt**  
> **dotnet-perf-skills / performance-analysis.md**  
> "🔴 Critical ... `new HttpClient()` per call, uncached `new Regex()` in hot loops, `new JsonSerializerOptions` per call"  
> "Top Priorities: ... per-line/per-call methods ... convert 48 `Compiled` regexes ..."

**no-skills excerpt**  
> **no-skills / performance-analysis.md**  
> "| 🔴 Critical | 6 | Socket exhaustion, O(n²) hot-path allocations, regex per-line instantiation |"  
> "| 🟡 Moderate | 28 | ... missing caching, sequential awaits |"  
> "`new JsonSerializerOptions` on every call" (marked moderate)

**Scores:** dotnet-perf-skills **5/5**, no-skills **3/5**.  
dotnet-perf-skills separates critical hot-path issues more consistently; baseline is useful but flattens severity in places (notably serializer options and broader startup-budget framing).

**Verdict:** **dotnet-perf-skills**.

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills excerpt**  
> **dotnet-perf-skills / performance-analysis.md**  
> "Inject `IHttpClientFactory` or use ... `SocketsHttpHandler { PooledConnectionLifetime ... }`"  
> "Use `StringComparison.OrdinalIgnoreCase` ..."  
> "Convert to `[GeneratedRegex]` ... Make the class `partial`."  
> "Use `Parallel.ForEachAsync` with `MaxDegreeOfParallelism`."

**no-skills excerpt**  
> **no-skills / performance-analysis.md**  
> "After — inject ... `IHttpClientFactory`"  
> "Use `SemaphoreSlim` or `Parallel.ForEachAsync`"  
> "Cache `GetProperties()` result per type"  
> "Use `HashSet<string>`."

**Scores:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
Both are actionable; skill-guided recommendations are more consistently specific and API-precise across all categories.

**Verdict:** **dotnet-perf-skills**.

## Weighted Summary

Weights applied: **Critical × 3**, **High × 2**, **Medium × 1**, **Low × 0.5**.

| Configuration | Critical subtotal (4 dims) | High subtotal (4 dims) | Total weighted score |
|---|---:|---:|---:|
| dotnet-perf-skills | (5+5+5+5)×3 = 60 | (4+5+5+5)×2 = 38 | **98** |
| no-skills | (4+4+4+4)×3 = 48 | (3+4+3+4)×2 = 28 | **76** |

## What All Versions Get Right

- Both identify the most dangerous production issue: **`new HttpClient()` per call**.
- Both flag **hot-path regex instantiation** and recommend migration toward cached/source-generated regex.
- Both detect **loop-driven string allocation** (`+=`, chained replace) and recommend `StringBuilder`/reduced materialization.
- Both call out **collection lookup anti-patterns** (`ContainsKey`+indexer, `List.Contains` vs `HashSet`).
- Both provide concrete, file-specific findings with line references and practical fix direction.

## Summary: Impact of Skills

The strongest impact from skills is in: **(1)** severity calibration and prioritization, **(2)** regex/startup-budget analysis depth, and **(3)** recommendation precision. `dotnet-perf-skills` is the clear top output (98 vs 76 weighted), delivering more consistent critical/high-impact triage and more complete cross-cutting optimization guidance. `no-skills` still performs solidly on core anti-pattern detection but is noisier and less consistently ranked for impact.
