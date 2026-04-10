# Comparative Analysis: dotnet-perf-skills, no-skills

This run compares **2 configurations** (`dotnet-perf-skills`, `no-skills`) on **1 scenario**: `analyze-perf-issues` from `output/{config}/run-4/analyze-perf-issues/performance-analysis.md`. Configuration identity was confirmed via `gen-notes.md` in each run directory (`dotnet-perf-skills` explicitly references the `analyzing-dotnet-performance` skill; `no-skills` is baseline).

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 5 |
| String Allocation Detection [CRITICAL] | 5 | 4 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 4 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 4 |
| Reflection and Serialization Overhead [HIGH] | 4 | 4 |
| Structural Optimization Detection [HIGH] | 5 | 5 |
| Severity Classification Accuracy [HIGH] | 5 | 3 |
| Fix Recommendation Quality [HIGH] | 5 | 4 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills excerpt** (`output/dotnet-perf-skills/run-4/analyze-perf-issues/performance-analysis.md`):
> **Regex instantiation in hot loops** — `LogAnalyzer.TryParseLine` creates 2–3 `Regex` objects per log line (🔴 O(n) compilations)  
> **40+ `RegexOptions.Compiled`** — `MarkdownStripper` bloats JIT startup budget (🟡)  
> On .NET 7+ use `[GeneratedRegex]` source generators

**no-skills excerpt** (`output/no-skills/run-4/analyze-perf-issues/performance-analysis.md`):
> `new Regex()` allocated per log line (4 instances in hot path)  
> `RegexOptions.Compiled` | 48 (all in MarkdownStripper.cs)  
> `[GeneratedRegex]` | 0

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**. Both identify per-call regex allocation, excessive compiled regex use, and recommend `[GeneratedRegex]` on .NET 8.

**Verdict:** **Tie**; both outputs are excellent on regex coverage.

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills excerpt:**
> **O(n²) string concatenation** — `TemplateEngine`, `CsvParser`, `LogAnalyzer`, `DataPipeline` all use `+=` in loops  
> `.ToLower()` without culture — Turkish-I bug  
> Long chain of `.Replace()` calls — each allocates a new string

**no-skills excerpt:**
> `+=` string concatenation in loops — O(n²) allocation (6 instances)  
> Char-by-char `+=` string building in CsvParser (2 instances)  
> `.ToLower()` / `.ToUpper()` without culture (17 instances)

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**. Baseline is strong, but skill output is broader on replace-chain allocation detail and cross-file allocation patterns.

**Verdict:** **dotnet-perf-skills** is better due to deeper allocation-pattern coverage.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills excerpt:**
> `existingSlugs.ToList()` then `.Contains()` in a `while` loop — O(n) per lookup. Use a `HashSet<string>`.  
> `ContainsKey` + indexer pattern — use `TryGetValue`  
> `.ToList()` + `Skip(i).Take(5).ToList()` inside a loop — O(n²) allocations

**no-skills excerpt:**
> `ContainsKey` + indexer double-lookup (8 instances)  
> `List.Contains()` O(n) lookups instead of HashSet (2 instances)  
> Missing collection capacity hints (5+ instances)

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**. Baseline hits core issues but misses some concrete hot-path LINQ materialization details present in skill output.

**Verdict:** **dotnet-perf-skills** wins on specificity and breadth.

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills excerpt:**
> **`new HttpClient()` per call** ... leads to socket exhaustion under load  
> Sequential `await` in `SendBatchAsync` loop — no parallelism  
> `SendBatchParallelAsync` — **unbounded parallelism**  
> `Task.Delay(_retryDelay)` ... without `CancellationToken`

**no-skills excerpt:**
> `new HttpClient()` per call — socket exhaustion risk  
> Sequential awaits in loop — no parallelism  
> Unbounded `Task.WhenAll` parallelism (1 instance)

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**. Both catch the major async/IO failures, but skill output adds explicit cancellation propagation gaps.

**Verdict:** **dotnet-perf-skills** is more complete on async robustness.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills excerpt:**
> **`typeof(TTarget).GetProperties()` + `prop.SetValue(target, value)` per call** ... Cache `PropertyInfo[]`  
> **`new JsonSerializerOptions { WriteIndented = true }` per call** in `Merge` ... should be a `static readonly` field

**no-skills excerpt:**
> Uncached `new JsonSerializerOptions` per call (4 instances)  
> Uncached reflection `GetProperties()` / `GetProperty()` / `SetValue()` (3 call sites)

**Score:** dotnet-perf-skills **4/5**, no-skills **4/5**. Both cover key reflection/serializer issues well; neither strongly develops partial-deserialization alternatives.

**Verdict:** **Tie**; both are good and actionable.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills excerpt:**
> `Record` class is unsealed — JIT cannot devirtualize  
> `DeliveryResult` struct does not implement `IEquatable<DeliveryResult>`  
> Static `Dictionary` that never mutates — candidate for `FrozenDictionary` on .NET 8+

**no-skills excerpt:**
> Unsealed classes — 17 of 17 classes are unsealed  
> Structs without `IEquatable<T>` (2 of 2)  
> `static readonly Dictionary<>` — FrozenDictionary candidates (2 instances)

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**. Both provide complete structural findings aligned to the rubric.

**Verdict:** **Tie**; both are comprehensive.

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills excerpt:**
> Top priorities: `new HttpClient()` per call ...  
> `LogAnalyzer.TryParseLine` creates 2–3 `Regex` objects per log line ...  
> 40+ `RegexOptions.Compiled` ... startup budget (🟡)

**no-skills excerpt:**
> #### 7. `ContainsKey` + indexer double-lookup (8 instances)  
> **Impact:** ... ~2x slower per access. **Critical** in LogAnalyzer...

**Score:** dotnet-perf-skills **5/5**, no-skills **3/5**. Skill output consistently prioritizes production/hot-path failures above moderate inefficiencies; baseline over-elevates some medium-impact lookup patterns to critical.

**Verdict:** **dotnet-perf-skills** clearly leads on prioritization fidelity.

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills excerpt:**
> use `IHttpClientFactory` or a shared `static HttpClient`  
> Add `SemaphoreSlim` throttling to `SendBatchParallelAsync`  
> Convert 40+ `RegexOptions.Compiled` to `[GeneratedRegex]`

**no-skills excerpt:**
> Inject a shared `HttpClient` ... or use `IHttpClientFactory`  
> Convert all to `[GeneratedRegex]` partial methods  
> Replace with `TryGetValue`

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**. Both are actionable and API-specific; skill output is slightly stronger in consistency and fit to issue severity.

**Verdict:** **dotnet-perf-skills** provides the highest-quality fix guidance overall.

## Weighted Summary

Weights used: **CRITICAL ×3**, **HIGH ×2**.

| Configuration | Critical subtotal | High subtotal | Total weighted score |
|---|---:|---:|---:|
| dotnet-perf-skills | (5+5+5+5)×3 = 60 | (4+5+5+5)×2 = 38 | **98** |
| no-skills | (5+4+4+4)×3 = 51 | (4+5+3+4)×2 = 32 | **83** |

## What All Versions Get Right

- Both correctly identify the highest-risk issue: **`new HttpClient()` per call**.
- Both detect key regex problems: **per-call `new Regex(...)`** and **large `RegexOptions.Compiled` footprint**.
- Both call out string hot-path issues (`+=` loops, casing allocations) and propose concrete .NET APIs.
- Both include cross-cutting remediation themes (regex caching/source-gen, `TryGetValue`, cached serializer options).

## Summary: Impact of Skills

Most impactful differences, ranked:  
1. **Severity calibration** (skill output avoids over-prioritizing medium-impact issues).  
2. **Critical-path completeness** (skill output captures cancellation and broader hot-path allocation context).  
3. **Collection/LINQ detail** (skill output surfaces more concrete materialization and sliding-window costs).

Overall, `dotnet-perf-skills` is the stronger configuration for this run (**98 vs 83 weighted**): both are competent, but skills provide better prioritization and slightly deeper, more consistently high-impact guidance.
