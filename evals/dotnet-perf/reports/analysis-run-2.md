# Comparative Analysis: dotnet-perf-skills, no-skills

This run compares **2 configurations** (`dotnet-perf-skills`, `no-skills`) across **1 scenario**: `analyze-perf-issues`, using `output/{config}/run-2/analyze-perf-issues/`. Configuration identification came from `gen-notes.md`: `dotnet-perf-skills` explicitly used `analyzing-dotnet-performance`, while `no-skills` is baseline output with no skill usage section.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 5 |
| String Allocation Detection [CRITICAL] | 5 | 5 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 5 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 5 |
| Reflection and Serialization Overhead [HIGH] | 4 | 4 |
| Structural Optimization Detection [HIGH] | 5 | 5 |
| Severity Classification Accuracy [HIGH] | 4 | 4 |
| Fix Recommendation Quality [HIGH] | 5 | 5 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-2/analyze-perf-issues/performance-analysis.md`)

> `new Regex(...)` inside `TryParseLine` — called **per log line**. For a 1M-line log, this creates 1M regex objects.  
> `47 RegexOptions.Compiled instances` ... use `[GeneratedRegex]` source generators for zero-startup-cost  
> `Missing [GeneratedRegex]`: All files using Regex — none use source generators

**no-skills** (`output/no-skills/run-2/analyze-perf-issues/performance-analysis.md`)

> `new Regex(...)` inside `TryParseLine` — called **per log line**. For a 1M-line log, this creates 1M regex objects.  
> `47 RegexOptions.Compiled instances` ... use `[GeneratedRegex]` source generators for zero-startup-cost  
> `Missing [GeneratedRegex]`: All files using Regex — none use source generators

**Score:** dotnet-perf-skills **5/5**; no-skills **5/5**. Both directly cover per-call instantiation, startup impact from many compiled regexes, and GeneratedRegex guidance.  
**Verdict:** **Tie** — both outputs are equally comprehensive.

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-2/analyze-perf-issues/performance-analysis.md`)

> `Character-by-character += on string in ParseLine — O(n²) for each line.`  
> `47 sequential .Replace() calls — each allocates a new string.`  
> `.ToLower() / .ToUpper() Without Culture ... use StringComparison.OrdinalIgnoreCase or ToLowerInvariant()`

**no-skills** (`output/no-skills/run-2/analyze-perf-issues/performance-analysis.md`)

> `Character-by-character += on string in ParseLine — O(n²) for each line.`  
> `47 sequential .Replace() calls — each allocates a new string.`  
> `.ToLower() / .ToUpper() Without Culture ... use StringComparison.OrdinalIgnoreCase or ToLowerInvariant()`

**Score:** dotnet-perf-skills **5/5**; no-skills **5/5**. Both catch loop concatenation, replace-chain allocations, and casing-allocation/culture risks.  
**Verdict:** **Tie** — no material quality difference.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-2/analyze-perf-issues/performance-analysis.md`)

> `.ToList() then .Contains() (O(n)) in a while loop — O(n²) ... Use HashSet<string>.`  
> `ContainsKey + indexer pattern — use TryGetValue.`  
> `Skip(i).Take(5).ToList() in a loop — O(n²) allocations for sliding window.`

**no-skills** (`output/no-skills/run-2/analyze-perf-issues/performance-analysis.md`)

> `.ToList() then .Contains() (O(n)) in a while loop — O(n²) ... Use HashSet<string>.`  
> `ContainsKey + indexer pattern — use TryGetValue.`  
> `Skip(i).Take(5).ToList() in a loop — O(n²) allocations for sliding window.`

**Score:** dotnet-perf-skills **5/5**; no-skills **5/5**. Both identify the key hot-path collection and materialization issues in the rubric.  
**Verdict:** **Tie** — identical coverage and actionability.

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-2/analyze-perf-issues/performance-analysis.md`)

> `new HttpClient()` ... `causes socket exhaustion under load.`  
> `Task.Delay without CancellationToken in retry loop.`  
> `Sequential await in SendBatchAsync loop.`  
> `Unbounded parallelism in SendBatchParallelAsync`

**no-skills** (`output/no-skills/run-2/analyze-perf-issues/performance-analysis.md`)

> `new HttpClient()` ... `causes socket exhaustion under load.`  
> `Task.Delay without CancellationToken in retry loop.`  
> `Sequential await in SendBatchAsync loop.`  
> `Unbounded parallelism in SendBatchParallelAsync`

**Score:** dotnet-perf-skills **5/5**; no-skills **5/5**. Both hit all required async/IO anti-patterns, including cancellation propagation and concurrency control concerns.  
**Verdict:** **Tie** — both are complete.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-2/analyze-perf-issues/performance-analysis.md`)

> `typeof(T).GetProperties() called on every MapTo/MapFrom invocation... Cache PropertyInfo[]`  
> `new JsonSerializerOptions { WriteIndented = true } allocated on every call... Cache as static readonly`

**no-skills** (`output/no-skills/run-2/analyze-perf-issues/performance-analysis.md`)

> `typeof(T).GetProperties() called on every MapTo/MapFrom invocation... Cache PropertyInfo[]`  
> `new JsonSerializerOptions { WriteIndented = true } allocated on every call... Cache as static readonly`

**Score:** dotnet-perf-skills **4/5**; no-skills **4/5**. Strong on reflection and serializer-options caching; weaker on explicitly recommending partial parsing alternatives (e.g., `Utf8JsonReader`) in places where full deserialization may be excessive.  
**Verdict:** **Tie** — both are strong but not fully exhaustive for this dimension.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-2/analyze-perf-issues/performance-analysis.md`)

> `Record class is unsealed — JIT cannot devirtualize.`  
> `ValidationError struct without IEquatable<ValidationError>.`  
> `ReplacementMap ... candidate for FrozenDictionary on .NET 8+.`

**no-skills** (`output/no-skills/run-2/analyze-perf-issues/performance-analysis.md`)

> `Record class is unsealed — JIT cannot devirtualize.`  
> `ValidationError struct without IEquatable<ValidationError>.`  
> `ReplacementMap ... candidate for FrozenDictionary on .NET 8+.`

**Score:** dotnet-perf-skills **5/5**; no-skills **5/5**. Both cover all three required structural themes: sealing, struct equality/boxing, and FrozenDictionary candidates.  
**Verdict:** **Tie** — complete and correct in both outputs.

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-2/analyze-perf-issues/performance-analysis.md`)

> `Top priorities: Socket exhaustion ... Regex instantiation in hot loops ... O(n²) string concatenation ... Unbounded parallelism ... Uncached reflection`  
> `| 🔴 Critical | 8 |  🟡 Moderate | 26 | ℹ️ Info | 24 |`

**no-skills** (`output/no-skills/run-2/analyze-perf-issues/performance-analysis.md`)

> `Top priorities: Socket exhaustion ... Regex instantiation in hot loops ... O(n²) string concatenation ... Unbounded parallelism ... Uncached reflection`  
> `| 🔴 Critical | 8 |  🟡 Moderate | 26 | ℹ️ Info | 24 |`

**Score:** dotnet-perf-skills **4/5**; no-skills **4/5**. Prioritization is generally accurate and hot-path-aware; minor gaps remain in explicitly separating startup/cold-path impacts from steady-state hot-path costs in a few moderate classifications.  
**Verdict:** **Tie** — high-quality triage with minor calibration gaps.

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-2/analyze-perf-issues/performance-analysis.md`)

> `After — inject via constructor ... IHttpClientFactory`  
> `After (.NET 7+) [GeneratedRegex(...)] private static partial Regex ...`  
> `Replace char-by-char += with StringBuilder`  
> `Parallel.ForEachAsync ... MaxDegreeOfParallelism = 10`

**no-skills** (`output/no-skills/run-2/analyze-perf-issues/performance-analysis.md`)

> `After — inject via constructor ... IHttpClientFactory`  
> `After (.NET 7+) [GeneratedRegex(...)] private static partial Regex ...`  
> `Replace char-by-char += with StringBuilder`  
> `Parallel.ForEachAsync ... MaxDegreeOfParallelism = 10`

**Score:** dotnet-perf-skills **5/5**; no-skills **5/5**. Both provide specific APIs, concrete replacement patterns, and practical code snippets without introducing unsafe or incorrect guidance.  
**Verdict:** **Tie** — equally actionable and implementation-ready.

## Weighted Summary

Weights applied: Critical ×3, High ×2.

| Configuration | Critical subtotal | High subtotal | Total weighted score |
|---|---:|---:|---:|
| dotnet-perf-skills | 60 | 36 | **96** |
| no-skills | 60 | 36 | **96** |

## What All Versions Get Right

- Clearly identify the highest-impact issues (`HttpClient` per call, hot-path regex allocation, O(n²) string building, unbounded parallelism).
- Provide file-level, line-specific findings and concrete remediation patterns.
- Cover regex, strings, collections/LINQ, async/IO, reflection/serialization, and structural concerns in one report.
- Include practical .NET APIs in recommendations (`GeneratedRegex`, `IHttpClientFactory`, `StringBuilder`, `TryGetValue`, `HashSet`, `FrozenDictionary` candidates).

## Summary: Impact of Skills

The scenario outputs are effectively equivalent in technical quality and scoring (**96 vs 96 weighted**), so skills did **not** produce a measurable advantage in issue detection or fix quality for run-2. The only visible impact is metadata depth: `dotnet-perf-skills` `gen-notes.md` documents the skill-driven workflow and reference framework, while `no-skills` `gen-notes.md` is concise and outcome-focused. In delivered analysis content (`performance-analysis.md`), both configurations are tied across all required dimensions.
