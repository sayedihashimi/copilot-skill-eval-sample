# Comparative Analysis: dotnet-perf-skills, no-skills

I evaluated **2 configurations** across **1 scenario** from `run-2`: `analyze-perf-issues` (`output/{config}/run-2/analyze-perf-issues/performance-analysis.md`). Configuration identity came from `gen-notes.md` in each directory: `dotnet-perf-skills` explicitly used the `analyzing-dotnet-performance` skill, while `no-skills` is the baseline default Copilot run.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 4 |
| String Allocation Detection [CRITICAL] | 5 | 4 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 4 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 4 |
| Reflection and Serialization Overhead [HIGH] | 5 | 4 |
| Structural Optimization Detection [HIGH] | 5 | 4 |
| Severity Classification Accuracy [HIGH] | 5 | 3 |
| Fix Recommendation Quality [HIGH] | 5 | 4 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-2/analyze-perf-issues/performance-analysis.md`)
> 2. **Regex instantiation in hot loops** — `new Regex()` called per log line in LogAnalyzer (4 instances)  
> 3. **48 `RegexOptions.Compiled` regexes** in MarkdownStripper with 0 `[GeneratedRegex]` usage project-wide  
> **Fix:** Hoist to `static readonly` fields, or better, use `[GeneratedRegex]`

**no-skills** (`output/no-skills/run-2/analyze-perf-issues/performance-analysis.md`)
> `LogAnalyzer.TryParseLine` — `new Regex()` per log line → O(n) regex compilations on large files  
> `MarkdownStripper` — 45+ `RegexOptions.Compiled` instances → excessive JIT startup cost  
> On .NET 7+ use `[GeneratedRegex]` source generators.

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best; it is more complete and quantified (counts, zero GeneratedRegex adoption, hot-path emphasis).

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills**
> #### 6. O(n²) String Concatenation via `+=` in Loops (7 files, ~15 sites)  
> **Impact:** Each `+=` allocates a new string copying all previous content.  
> 25 instances across 6 files use `.ToLower()` to normalize strings before comparison.

**no-skills**
> `TemplateEngine.ProcessLoops` — string `+=` in loop → O(n²) allocations  
> `CsvParser.ParseLine` — char-by-char string `+=` → O(n²) allocations  
> Use `ToLowerInvariant()` or `StringComparison.OrdinalIgnoreCase` comparisons

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best due to stronger cross-file quantification and explicit culture/correctness framing.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills**
> #### 19. `.ToList()` for Counting / O(n) `.Contains()` on Lists (5 instances)  
> **Files:** ... `existingSlugs.ToList()` + `.Contains()` in loop  
> #### 22. `Skip(i).Take(5).ToList()` in Sliding Window Loop

**no-skills**
> `List.Contains` in while loop (O(n²)) ... Use a `HashSet<string>`.  
> `ContainsKey` + indexer ... use `TryGetValue` for a single dictionary probe.  
> `.ToList()` + `Skip(i).Take(5).ToList()` per iteration ... use array indexing instead.

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best; both are strong, but skills output is broader and better prioritized by impact.

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills**
> #### 1. `new HttpClient()` Per Call — Socket Exhaustion Risk (3 instances)  
> #### 16. Sequential Awaits in Loop  
> #### 17. Unbounded Parallelism  
> #### 18. Missing `CancellationToken` on Async Methods

**no-skills**
> `new HttpClient()` per call ... causes socket exhaustion under load.  
> `SendBatchAsync` awaits each notification sequentially  
> `SendBatchParallelAsync` fires all tasks at once  
> `Task.Delay` in retry loop has no cancellation token

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best because it ties findings to concrete scale/latency impact more consistently.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills**
> #### 4. Uncached `JsonSerializerOptions` Per Call (4 instances)  
> #### 7. Uncached Reflection — `GetProperties()` / `SetValue()` / `GetValue()` Per Call  
> 10-100x slower than compiled delegates or source generators.

**no-skills**
> `new JsonSerializerOptions` per call ... Cache as `static readonly`.  
> Uncached `GetProperties()` reflection ... Cache per type using `ConcurrentDictionary<Type, PropertyInfo[]>`.  
> `SetValue` / `GetValue` reflection hot-path issues identified.

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best with tighter severity treatment and stronger serializer-cache impact framing.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills**
> #### 13. FrozenDictionary Candidates ... never modified.  
> #### 14. Structs Without `IEquatable<T>` (2 instances)  
> #### 15. Unsealed Classes — 0 of 17 Sealed (17 instances)

**no-skills**
> **DataPipeline.Record**, **ValidationEngine.ValidationResult**, **EntityMapper.MappingConfig** — sealing leaf classes helps the JIT devirtualize and inline.  
> **DeliveryResult** and **ValidationError** — implement `IEquatable<T>`  
> FrozenDictionary candidate noted for static dictionaries.

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best because coverage is more systematic (ratios/counts across the codebase).

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills**
> **38 distinct findings**: **7 Critical**, **21 Moderate**, and **10 Info**.  
> | 1 | Reuse HttpClient (socket exhaustion) | 🔴 |  
> | 2 | Cache Regex in LogAnalyzer hot loop | 🔴 |

**no-skills**
> **Total issues found: 52**  
> - 🔴 **Critical**: 7 ...  
> - 🟡 **Moderate**: 23 ...  
> `MarkdownStripper` — 45+ `RegexOptions.Compiled` ... | 🟡 Moderate |

**Score:** dotnet-perf-skills **5/5**; no-skills **3/5**.  
**Verdict:** **dotnet-perf-skills** is clearly best; its hot-path and startup-budget criticality is ranked more consistently, while baseline under-ranks several high-impact items.

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills**
> **Fix:** Inject `IHttpClientFactory` or use a single `static readonly HttpClient`  
> **Fix:** Replace with `StringBuilder`  
> **Fix:** Convert each to `[GeneratedRegex]`. The class must be made `partial`

**no-skills**
> Use `IHttpClientFactory` or a `static HttpClient`.  
> Use `StringBuilder`.  
> On .NET 7+ use `[GeneratedRegex]` source generators.

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best with more precise API-level guidance, caveats, and prioritization context.

## Weighted Summary

Weights used: Critical ×3, High ×2.

| Configuration | Critical subtotal (4 dims) | High subtotal (4 dims) | Total weighted score |
|---|---:|---:|---:|
| dotnet-perf-skills | (5+5+5+5)×3 = 60 | (5+5+5+5)×2 = 40 | **100** |
| no-skills | (4+4+4+4)×3 = 48 | (4+4+3+4)×2 = 30 | **78** |

## What All Versions Get Right

- Both identify the most dangerous production issue: `new HttpClient()` per call.
- Both flag hot-path regex creation (`LogAnalyzer.TryParseLine`) and recommend caching/source generation.
- Both catch O(n²) string concatenation patterns and recommend `StringBuilder`.
- Both recognize key collection inefficiencies (`List.Contains`, `ContainsKey` + indexer, unnecessary `.ToList()`).
- Both include actionable async throttling guidance for unbounded parallelism.

## Summary: Impact of Skills

Most impactful differences, ranked: **(1)** sharper severity calibration, **(2)** stronger quantified coverage across regex/string/collections, **(3)** more precise fix patterns with better .NET 8 framing. Overall, `dotnet-perf-skills` is materially stronger and more decision-ready, while `no-skills` is solid but less consistent in impact ranking and depth.
