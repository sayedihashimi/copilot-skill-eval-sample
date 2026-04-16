# Comparative Analysis: dotnet-perf-skills, no-skills

This run compares **2 configurations** (`dotnet-perf-skills` and `no-skills`) on **1 shared scenario**: `analyze-perf-issues` under `output/{config}/run-3/analyze-perf-issues/`. Configuration mapping was verified from `gen-notes.md`: `dotnet-perf-skills` explicitly used `analyzing-dotnet-performance`, while `no-skills` is the baseline report format.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 4 |
| String Allocation Detection [CRITICAL] | 5 | 4 |
| Collection and LINQ Efficiency [CRITICAL] | 4 | 5 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 5 |
| Reflection and Serialization Overhead [HIGH] | 4 | 5 |
| Structural Optimization Detection [HIGH] | 5 | 4 |
| Severity Classification Accuracy [HIGH] | 3 | 4 |
| Fix Recommendation Quality [HIGH] | 5 | 4 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills — `output/dotnet-perf-skills/run-3/analyze-perf-issues/performance-analysis.md`**
> #### 2. `new Regex()` per log line in hot path (8 instances)  
> **Impact:** `LogAnalyzer.TryParseLine` creates 2–3 `new Regex` objects per line. For a 1M-line log, that's 2–3M regex compilations.  
> #### 8. 48 `RegexOptions.Compiled` without `[GeneratedRegex]` (48 instances)

**no-skills — `output/no-skills/run-3/analyze-perf-issues/performance-analysis.md`**
> 1. 🟡 **47 `RegexOptions.Compiled` static instances** (lines 13–59)  
> - Each `RegexOptions.Compiled` regex JIT-compiles at first use, consuming significant startup time and memory.  
> 1. 🔴 **`new Regex(...)` per log line** (lines 50, 65, 75)

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
dotnet-perf-skills is more complete and quantified (hot-path + startup-budget framing + explicit `[GeneratedRegex]` migration).

**Verdict:** **dotnet-perf-skills** is best for regex coverage depth and prioritization context.

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills — `.../performance-analysis.md`**
> #### 4. String `+=` concatenation in loops — O(n²) (11+ sites)  
> #### 9. `.ToLower()`/`.ToUpper()` without culture (25 instances)  
> #### 19. `.Replace()` chains in loops (SlugGenerator)

**no-skills — `.../performance-analysis.md`**
> ### 2. String Concatenation in Loops (6 files)  
> `TemplateEngine`, `LogAnalyzer`, `DataPipeline`, `NotificationService`, `CsvParser`, `ValidationEngine` all use `+=` string concatenation in loops.  
> ### 3. `.ToLower()` / `.ToUpper()` without Culture (7 files)

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
Both catch the required issues; dotnet-perf-skills is more concrete on counts and specific allocation chains (`SlugGenerator`, `MarkdownStripper`).

**Verdict:** **dotnet-perf-skills** is best on string-allocation specificity.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills — `.../performance-analysis.md`**
> #### 16. Unnecessary `.ToList()` materializations (20 instances)  
> #### 17. `List.Contains` / `allKeys.Contains` — O(n) lookup (3 sites)  
> #### 5. `ContainsKey` + indexer double-lookup (18 instances)

**no-skills — `.../performance-analysis.md`**
> 5. 🟡 **`Skip(i).Take(5).ToList()` in sliding window** (line 157)  
> 5. ℹ️ **`.Distinct().ToList()` for tag dedup** (line 191)  
> 2. 🟡 **`Keys.ToList()` + `.Contains()` for key union in `Diff`** (lines 85–89)

**Score:** dotnet-perf-skills **4/5**; no-skills **5/5**.  
no-skills explicitly calls out the rubric’s sliding-window `Skip().Take().ToList()` issue and multiple concrete LINQ materialization anti-patterns.

**Verdict:** **no-skills** is best for collection/LINQ hot-path granularity.

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills — `.../performance-analysis.md`**
> #### 1. `new HttpClient()` per call — socket exhaustion (3 instances)  
> #### 7. Sequential awaits in loop — no parallelism (1 instance)  
> #### 13. Unbounded parallelism (1 instance)  
> #### 14. Missing `CancellationToken` on async methods

**no-skills — `.../performance-analysis.md`**
> 1. 🔴 **`new HttpClient()` per call — socket exhaustion** (lines 163, 179, 191)  
> 2. 🔴 **Sequential awaits in batch loop** (lines 117–122)  
> 3. 🔴 **Unbounded parallelism in `SendBatchParallelAsync`** (lines 130–133)  
> 5. 🟡 **`Task.Delay` without `CancellationToken`** (line 102)

**Score:** dotnet-perf-skills **5/5**; no-skills **5/5**.  
Both reports cover all required async/IO anti-patterns with concrete fixes (`IHttpClientFactory`, throttling, cancellation propagation).

**Verdict:** **Tie** — both are production-relevant and actionable.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills — `.../performance-analysis.md`**
> #### 3. Uncached `new JsonSerializerOptions` per call (4 instances)  
> #### 15. Uncached reflection — `GetProperties()`/`SetValue()`/`GetValue()` per call (4 sites)  
> **Fix:** Cache `PropertyInfo[]` per type in a `ConcurrentDictionary<Type, PropertyInfo[]>`.

**no-skills — `.../performance-analysis.md`**
> 1. 🔴 **Uncached `GetProperties()` reflection per call** (lines 77, 114)  
> 2. 🟡 **Uncached `SetValue` / `GetValue` reflection** (lines 101, 119)  
> 6. ℹ️ **Full deserialization for `PrettyPrint`** (line 140)

**Score:** dotnet-perf-skills **4/5**; no-skills **5/5**.  
Both detect core reflection/serializer issues; no-skills adds explicit note on unnecessary full deserialization path optimization.

**Verdict:** **no-skills** is best on serialization-path completeness.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills — `.../performance-analysis.md`**
> #### 10. Unsealed classes — 17 of 17 (0% sealed)  
> #### 11. Structs without `IEquatable<T>` (2 of 2 structs)  
> #### 12. `static readonly Dictionary<>` — FrozenDictionary candidates (2 instances)

**no-skills — `.../performance-analysis.md`**
> ### 5. Missing `IEquatable<T>` on Structs (2 files)  
> ### 6. Unsealed Classes (3 files)  
> 7. ℹ️ **Static `Dictionary` could be `FrozenDictionary`** (line 11)

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
dotnet-perf-skills provides stronger breadth and quantification, especially on sealing coverage across the whole codebase.

**Verdict:** **dotnet-perf-skills** is best for structural optimization detection.

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills — `.../performance-analysis.md`**
> | 🔴 Critical | 7 | `new HttpClient` per call ..., uncached `new Regex` in hot loops ... |  
> #### 5. `ContainsKey` + indexer double-lookup (18 instances)  
> **Impact:** ~2× slower per lookup ...

**no-skills — `.../performance-analysis.md`**
> | 🔴 Critical | 8 |  
> | 🟡 Moderate | 22 |  
> 4. 🟡 **`ContainsKey` + indexer instead of `TryGetValue`** ...  
> 1. 🔴 **`new HttpClient()` per call — socket exhaustion** ...

**Score:** dotnet-perf-skills **3/5**; no-skills **4/5**.  
Both prioritize true criticals, but dotnet-perf-skills appears to over-escalate `ContainsKey`+indexer as critical where moderate is more consistent with impact.

**Verdict:** **no-skills** is best on severity calibration.

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills — `.../performance-analysis.md`**
> **Fix:** Inject `IHttpClientFactory` or use a `static readonly HttpClient` with `PooledConnectionLifetime`.  
> **Fix:** Hoist to `static readonly` fields, or use `[GeneratedRegex]` on .NET 7+.  
> **Fix:** Use `StringComparison.OrdinalIgnoreCase` for comparisons, or `ToLowerInvariant()`.

**no-skills — `.../performance-analysis.md`**
> **Fix:** Use `Task.WhenAll` with throttling ... `SemaphoreSlim`.  
> **Fix:** Use `TryGetValue` or `CollectionsMarshal.GetValueRefOrAddDefault`.  
> **Recommendation:** Adopt a project-wide policy of `[GeneratedRegex]` ... or `static readonly Regex` fields.

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
Both are actionable; dotnet-perf-skills is more consistently specific to safe, mainstream .NET APIs and keeps suggestions tightly prioritized.

**Verdict:** **dotnet-perf-skills** is best for practical remediation guidance.

## Weighted Summary

Weights used: Critical ×3, High ×2, Medium ×1, Low ×0.5.

| Dimension | Tier | Weight | dotnet-perf-skills | no-skills |
|---|---|---:|---:|---:|
| Regex Anti-Pattern Detection | Critical | 3 | 15 | 12 |
| String Allocation Detection | Critical | 3 | 15 | 12 |
| Collection and LINQ Efficiency | Critical | 3 | 12 | 15 |
| Async and IO Pattern Detection | Critical | 3 | 15 | 15 |
| Reflection and Serialization Overhead | High | 2 | 8 | 10 |
| Structural Optimization Detection | High | 2 | 10 | 8 |
| Severity Classification Accuracy | High | 2 | 6 | 8 |
| Fix Recommendation Quality | High | 2 | 10 | 8 |
| **Total Weighted Score** |  |  | **91** | **88** |

## What All Versions Get Right

- Both identify the highest-risk production issues: per-call `HttpClient`, per-call regex in hot paths, and string `+=` in loops.
- Both recommend modern .NET remediations (`[GeneratedRegex]`, `StringBuilder`, `IHttpClientFactory`, `TryGetValue`, `HashSet`).
- Both detect structural/perf hygiene issues beyond obvious hotspots (`IEquatable<T>`, unsealed classes, `FrozenDictionary` candidates).
- Both provide prioritized fix lists rather than only raw findings.

## Summary: Impact of Skills

1. The biggest skill-driven gain is **coverage consistency and quantification** (counts, ratios, and startup-budget framing), especially for regex and structural categories.
2. The baseline (`no-skills`) is stronger in a few **micro-pattern specifics** (notably `Skip().Take().ToList()` sliding windows and explicit full-deserialization note).
3. Overall, **dotnet-perf-skills** delivers the higher weighted result (**91 vs 88**) and a more systematic, rubric-aligned report, while **no-skills** remains competitive and occasionally more granular in LINQ-specific callouts.
