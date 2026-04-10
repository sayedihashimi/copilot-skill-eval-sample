# Comparative Analysis: dotnet-perf-skills, no-skills

I evaluated **2 configurations** (`dotnet-perf-skills`, `no-skills`) on **1 scenario**: `analyze-perf-issues` (`output/{config}/run-1/analyze-perf-issues/`). Configuration identity was taken from directory names and `gen-notes.md`; both produced a `performance-analysis.md`, while `gen-notes.md` differed in how the run was attributed.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 5 |
| String Allocation Detection [CRITICAL] | 5 | 5 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 5 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 5 |
| Reflection and Serialization Overhead [HIGH] | 5 | 5 |
| Structural Optimization Detection [HIGH] | 5 | 5 |
| Severity Classification Accuracy [HIGH] | 4 | 4 |
| Fix Recommendation Quality [HIGH] | 4 | 4 |
| Configuration Attribution Consistency [MEDIUM] | 5 | 1 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills excerpt** (`performance-analysis.md`):
> #### 1. `new Regex()` instantiated per call in hot paths (8 instances)  
> In `LogAnalyzer.TryParseLine()`, 2-3 regexes are created per log line  
> #### 5. 48 `RegexOptions.Compiled` static regexes with 0 `[GeneratedRegex]`  
> **Fix:** Convert all 48 to `[GeneratedRegex]` attributes.

**no-skills excerpt** (`performance-analysis.md`):
> #### 1. `new Regex()` instantiated per call in hot paths (8 instances)  
> In `LogAnalyzer.TryParseLine()`, 2-3 regexes are created per log line  
> #### 5. 48 `RegexOptions.Compiled` static regexes with 0 `[GeneratedRegex]`  
> **Fix:** Convert all 48 to `[GeneratedRegex]` attributes.

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5** — both hit all required regex issues (per-call construction, compiled-budget startup impact, GeneratedRegex recommendation).

**Verdict:** **Tie**; both are comprehensive and specific.

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills excerpt** (`performance-analysis.md`):
> #### 8. String concatenation (`+=`) in loops — O(n²) allocation  
> #### 6. `.ToLower()`/`.ToUpper()` without culture or `StringComparison` (20 instances)  
> #### 20. 47 chained Regex `.Replace()` calls in `StripMarkdown`

**no-skills excerpt** (`performance-analysis.md`):
> #### 8. String concatenation (`+=`) in loops — O(n²) allocation  
> #### 6. `.ToLower()`/`.ToUpper()` without culture or `StringComparison` (20 instances)  
> #### 20. 47 chained Regex `.Replace()` calls in `StripMarkdown`

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5** — both cover loop concat, casing allocations/culture risk, and replace-chain intermediates.

**Verdict:** **Tie**; both outputs are thorough.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills excerpt** (`performance-analysis.md`):
> #### 11. `List.Contains()` for key lookups — O(n) per lookup  
> #### 10. Unnecessary `.ToList()` materializations (18 instances)  
> #### 18. `Skip(i).Take(5).ToList()` in loop — O(n²) LINQ

**no-skills excerpt** (`performance-analysis.md`):
> #### 11. `List.Contains()` for key lookups — O(n) per lookup  
> #### 10. Unnecessary `.ToList()` materializations (18 instances)  
> #### 18. `Skip(i).Take(5).ToList()` in loop — O(n²) LINQ

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5** — all requested collection/LINQ hotspots are explicitly identified and mapped to files.

**Verdict:** **Tie**; both provide strong coverage and concrete fixes.

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills excerpt** (`performance-analysis.md`):
> #### 2. `new HttpClient()` per request — socket exhaustion risk (3 instances)  
> #### 15. Sequential async in loop without parallelism  
> #### 16. Unbounded parallelism in `SendBatchParallelAsync`  
> #### 17. Missing `CancellationToken` on async methods

**no-skills excerpt** (`performance-analysis.md`):
> #### 2. `new HttpClient()` per request — socket exhaustion risk (3 instances)  
> #### 15. Sequential async in loop without parallelism  
> #### 16. Unbounded parallelism in `SendBatchParallelAsync`  
> #### 17. Missing `CancellationToken` on async methods

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5** — both cover all required async/IO anti-patterns.

**Verdict:** **Tie**; complete and operationally relevant.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills excerpt** (`performance-analysis.md`):
> #### 3. Uncached `new JsonSerializerOptions` per call (5 instances)  
> #### 9. Uncached reflection — `GetProperties()`/`SetValue()`/`GetValue()` per call (6 instances)

**no-skills excerpt** (`performance-analysis.md`):
> #### 3. Uncached `new JsonSerializerOptions` per call (5 instances)  
> #### 9. Uncached reflection — `GetProperties()`/`SetValue()`/`GetValue()` per call (6 instances)

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5** — both identify serializer-option churn and reflection hot-path costs with targeted fixes.

**Verdict:** **Tie**.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills excerpt** (`performance-analysis.md`):
> #### 12. `static readonly Dictionary<>` — FrozenDictionary candidates (2 instances)  
> #### 13. Structs without `IEquatable<T>` (2 instances)  
> #### 14. Unsealed leaf classes — 0 of 18 classes sealed

**no-skills excerpt** (`performance-analysis.md`):
> #### 12. `static readonly Dictionary<>` — FrozenDictionary candidates (2 instances)  
> #### 13. Structs without `IEquatable<T>` (2 instances)  
> #### 14. Unsealed leaf classes — 0 of 18 classes sealed

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5** — all required structural opportunities are explicitly covered.

**Verdict:** **Tie**.

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills excerpt** (`performance-analysis.md`):
> Top priorities are the per-line `new Regex()` allocations in `LogAnalyzer`... `new HttpClient()` per request... uncached `JsonSerializerOptions`  
> #### 4. `ContainsKey` + indexer double-lookup pattern (13 instances)  
> ### 🔴 Critical

**no-skills excerpt** (`performance-analysis.md`):
> Top priorities are the per-line `new Regex()` allocations in `LogAnalyzer`... `new HttpClient()` per request... uncached `JsonSerializerOptions`  
> #### 4. `ContainsKey` + indexer double-lookup pattern (13 instances)  
> ### 🔴 Critical

**Score:** dotnet-perf-skills **4/5**, no-skills **4/5** — high-impact hotspots are correctly elevated, but `ContainsKey + indexer` appears slightly over-classified as Critical versus Moderate in most perf triage schemes.

**Verdict:** **Tie**; strong prioritization overall with a minor calibration gap.

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills excerpt** (`performance-analysis.md`):
> **Fix:** Inject `IHttpClientFactory` or use a `static readonly HttpClient`  
> **Fix:** Use `StringComparison.OrdinalIgnoreCase` comparisons  
> **Fix:** Convert all 48 to `[GeneratedRegex]` attributes.

**no-skills excerpt** (`performance-analysis.md`):
> **Fix:** Inject `IHttpClientFactory` or use a `static readonly HttpClient`  
> **Fix:** Use `StringComparison.OrdinalIgnoreCase` comparisons  
> **Fix:** Convert all 48 to `[GeneratedRegex]` attributes.

**Score:** dotnet-perf-skills **4/5**, no-skills **4/5** — recommendations are concrete and mostly correct, with API-level specificity; minor deductions for occasionally aggressive low-level suggestions relative to baseline fixes.

**Verdict:** **Tie**; both are actionable and practical.

## 9. Configuration Attribution Consistency [MEDIUM]

**dotnet-perf-skills excerpt** (`gen-notes.md`):
> ## Work Performed  
> Analyzed 10 C# source files in the `perf01/` .NET class library for performance anti-patterns.

**no-skills excerpt** (`gen-notes.md`):
> ### `analyzing-dotnet-performance` (primary skill)  
> This skill was the core driver of the analysis.

**Score:** dotnet-perf-skills **5/5**, no-skills **1/5** — the `no-skills` run notes explicitly claim primary skill usage, which conflicts with the expected baseline interpretation and weakens run-to-run attribution trust.

**Verdict:** **dotnet-perf-skills wins** on metadata consistency.

## Weighted Summary

Weights used: Critical ×3, High ×2, Medium ×1.

| Configuration | Critical subtotal | High subtotal | Medium subtotal | Total weighted score |
|---|---:|---:|---:|---:|
| dotnet-perf-skills | 60 | 36 | 5 | **101** |
| no-skills | 60 | 36 | 1 | **97** |

## What All Versions Get Right

- Strong coverage of core .NET perf categories (regex, strings, collections/LINQ, async/IO, reflection).
- Concrete fix paths tied to modern APIs (`[GeneratedRegex]`, `IHttpClientFactory`, `StringComparison.OrdinalIgnoreCase`, `FrozenDictionary`).
- Good file-level traceability with counts and hotspot-oriented framing.

## Summary: Impact of Skills

1. **Primary impact difference was metadata consistency, not technical analysis depth** — `performance-analysis.md` quality is effectively equivalent.
2. **Both configurations delivered high-value technical findings** with near-identical coverage and recommendations.
3. **Overall assessment:** by weighted score, **dotnet-perf-skills ranks first (101)** vs **no-skills (97)**, driven by attribution reliability in `gen-notes.md`, not by materially better finding quality in the scenario report.
