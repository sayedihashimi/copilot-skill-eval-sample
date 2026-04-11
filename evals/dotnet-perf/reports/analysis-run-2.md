# Comparative Analysis: dotnet-perf-skills, no-skills

This run compares **2 configurations** for one shared scenario: `analyze-perf-issues` under `output/{config}/run-2/analyze-perf-issues/`. Configuration mapping was derived from directory names and per-scenario notes: `dotnet-perf-skills` explicitly references the `analyzing-dotnet-performance` skill in `gen-notes.md`, while `no-skills` is the baseline configuration.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 5 |
| String Allocation Detection [CRITICAL] | 5 | 5 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 4 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 4 |
| Reflection and Serialization Overhead [HIGH] | 4 | 4 |
| Structural Optimization Detection [HIGH] | 5 | 4 |
| Severity Classification Accuracy [HIGH] | 5 | 2 |
| Fix Recommendation Quality [HIGH] | 5 | 3 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills — `output/dotnet-perf-skills/run-2/analyze-perf-issues/performance-analysis.md`**
> **Three `new Regex(...)` per log line** — this is the single hottest path in the class.
>  
> **40+ `RegexOptions.Compiled` static fields** ... **Fix:** On .NET 8+, use `[GeneratedRegex]`

**no-skills — `output/no-skills/run-2/analyze-perf-issues/performance-analysis.md`**
> **Per-Call `new Regex()` in Hot Paths (8 instances)** ... In `LogAnalyzer.TryParseLine()`, this runs per log line
>  
> **48 `RegexOptions.Compiled` Without `[GeneratedRegex]`**

**Score:** dotnet-perf-skills **5/5**; no-skills **5/5**. Both clearly identify per-call regex construction, compiled-regex startup cost, and recommend `[GeneratedRegex]`.

**Verdict:** **Tie** — both outputs are excellent and specific on regex risks.

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills**
> **`string +=` character-by-character** in a loop ... O(N²) allocations.
>  
> **Chain of 40+ `.Replace()` calls**, each allocating a new string.
>  
> **`.ToLower()` without `StringComparison`** ... Turkish-I problem.

**no-skills**
> String Concatenation (`+=`) in Loops — O(n²) (7 sites)
>  
> `.ToLower()`/`.ToUpper()` Without Culture (16 instances)
>  
> Chained `Regex.Replace` Allocations in `SlugGenerator` (14 calls)

**Score:** dotnet-perf-skills **5/5**; no-skills **5/5**. Both cover loop concatenation, casing allocations/correctness, and replace-chain allocation pressure.

**Verdict:** **Tie** — both are comprehensive and actionable.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills**
> `allKeys.ToList()` + `.Contains(key)` ... O(n²) overall. **Fix:** Use `HashSet<string>`
>  
> `ContainsKey` + indexer double lookup.
>  
> `Skip(i).Take(5).ToList()` inside a loop — O(n²) allocation for the sliding window.

**no-skills**
> `ContainsKey` + Indexer Double-Lookup (12+ instances)
>  
> `.ToList()` + `.Contains()` — O(n) Lookups (2 instances)
>  
> `.Where().ToList()` Just for Counting (2 instances)

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**. Baseline is strong, but skill-backed output is more complete for hot-path LINQ/window patterns (explicit `Skip/Take/ToList` sliding-window issue).

**Verdict:** **dotnet-perf-skills wins** on depth in hot-path collection/LINQ cases.

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills**
> **`new HttpClient()` per call** ... socket exhaustion risk.
>  
> **Unbounded parallelism** ... 10K notifications = 10K concurrent HTTP calls.
>  
> **Sequential `await` in a loop**
>  
> `Task.Delay` without `CancellationToken`

**no-skills**
> `new HttpClient()` Per Call — Socket Exhaustion (3 instances)
>  
> Sequential Awaits in Batch Loop (1 instance)
>  
> the parallel version ... has unbounded parallelism (separate concern).

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**. Both catch major IO issues, but dotnet-perf-skills is more direct and explicit on cancellation propagation.

**Verdict:** **dotnet-perf-skills wins** with more complete async hygiene coverage.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills**
> `typeof(T).GetProperties()` on every call ... **Fix:** Cache `PropertyInfo[]`
>  
> `new JsonSerializerOptions` per call ... recreating them discards the cache

**no-skills**
> Uncached `new JsonSerializerOptions` Per Call ... **Up to 592× slower**
>  
> Uncached Reflection ... `GetProperties()`/`GetProperty()`/`SetValue()` in hot paths

**Score:** dotnet-perf-skills **4/5**; no-skills **4/5**. Both identify the core reflection/options problems and propose caching. Neither output deeply develops partial parsing (`Utf8JsonReader`) guidance.

**Verdict:** **Tie** — comparable quality on this dimension.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills**
> `MappingConfig` ... **Unsealed class**
>  
> `ValidationError` ... **Struct without `IEquatable<ValidationError>`**
>  
> Static `Dictionary` ... candidate for `FrozenDictionary` on .NET 8+

**no-skills**
> 17 Unsealed Classes (17 of 17, 0 sealed)
>  
> Structs Without `IEquatable<T>` (2 structs, 0 implementing)
>  
> `FrozenDictionary` Candidates (2 instances)

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**. Baseline detects all categories but is more blanket/less selective (e.g., “seal all 17 classes”).

**Verdict:** **dotnet-perf-skills wins** for targeted, lower-risk structural recommendations.

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills**
> **Top priorities:** `new HttpClient` per call ... regex in hot paths ... string `+=` in loops ... unbounded parallelism
>  
> 40+ `RegexOptions.Compiled` ... startup cost ... 🟡 Moderate

**no-skills**
> 🔴 **Critical**: ... `ContainsKey` + indexer double-lookups, and `.ToList()` + `.Contains()` O(n) lookups
>  
> **Fix `new HttpClient()` ... cache `JsonSerializerOptions` ... hoist `new Regex()`**

**Score:** dotnet-perf-skills **5/5**; no-skills **2/5**. dotnet-perf-skills mostly separates production-critical hot-path issues from moderate optimization items. no-skills over-classifies several medium-impact collection patterns as critical, reducing prioritization signal.

**Verdict:** **dotnet-perf-skills clearly wins** on practical triage quality.

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills**
> **Fix:** Inject `IHttpClientFactory` or use a single `static HttpClient`.
>  
> **Fix:** Use `StringComparison.OrdinalIgnoreCase` ... or `ToLowerInvariant()`.
>  
> **Fix:** use `[GeneratedRegex]` partial methods

**no-skills**
> **Fix:** Hoist to `private static readonly Regex` ... or preferably `[GeneratedRegex]`
>  
> **Fix:** Add `sealed` keyword to all leaf classes. None ... are subclassed within the project.
>  
> **Fix:** ... on .NET 9+ use `params ReadOnlySpan<T>`

**Score:** dotnet-perf-skills **5/5**; no-skills **3/5**. Baseline includes many concrete APIs, but several recommendations are over-broad or high-confidence without sufficient safety qualifiers.

**Verdict:** **dotnet-perf-skills wins** for consistently precise, context-safe guidance.

## Weighted Summary

Weights applied: **Critical ×3**, **High ×2**, **Medium ×1**, **Low ×0.5**.

| Configuration | Critical subtotal (4 dims) | High subtotal (4 dims) | Total weighted score |
|---|---:|---:|---:|
| dotnet-perf-skills | (5+5+5+5)×3 = **60** | (4+5+5+5)×2 = **38** | **98** |
| no-skills | (5+5+4+4)×3 = **54** | (4+4+2+3)×2 = **26** | **80** |

## What All Versions Get Right

- Both clearly identify the highest-risk runtime issue: **`new HttpClient()` per call**.
- Both detect **regex hot-path misuse** and recommend **`[GeneratedRegex]`** for .NET 8.
- Both flag **string `+=` loops** as O(n²) allocation patterns.
- Both call out **reflection caching** and **`JsonSerializerOptions` reuse** as important fixes.

## Summary: Impact of Skills

Most impactful differences are: (1) **better severity calibration** (hot-path critical vs moderate cleanup), (2) **more targeted structural advice**, and (3) **more consistently safe fix guidance** in `dotnet-perf-skills`. By weighted score, `dotnet-perf-skills` ranks first (**98**) vs `no-skills` (**80**), with the gain coming less from raw issue detection and more from prioritization quality and recommendation reliability.
