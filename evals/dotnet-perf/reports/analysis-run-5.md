# Comparative Analysis: no-skills, dotnet-perf-skills

This run compares **2 configurations** across **1 scenario** (`analyze-perf-issues`) using outputs under `output/{config}/run-5/analyze-perf-issues/`. Configuration mapping was taken from directory names and `gen-notes.md`: `no-skills` is baseline Copilot output, and `dotnet-perf-skills` explicitly reports use of the `analyzing-dotnet-performance` skill.

## Executive Summary

| Dimension [Tier] | no-skills | dotnet-perf-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 4 | 5 |
| String Allocation Detection [CRITICAL] | 4 | 5 |
| Collection and LINQ Efficiency [CRITICAL] | 3 | 5 |
| Async and IO Pattern Detection [CRITICAL] | 4 | 5 |
| Reflection and Serialization Overhead [HIGH] | 4 | 4 |
| Structural Optimization Detection [HIGH] | 4 | 5 |
| Severity Classification Accuracy [HIGH] | 3 | 5 |
| Fix Recommendation Quality [HIGH] | 4 | 5 |
| Evidence Quantification and Scan Rigor [HIGH] | 3 | 5 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**no-skills — `output/no-skills/run-5/analyze-perf-issues/performance-analysis.md`**
> 1. **Regex instantiation per call in hot paths** (LogAnalyzer, ValidationEngine, SlugGenerator, TemplateEngine) — O(millions) of regex compilations  
> ...  
> - **Per-call `new Regex()`** is the most damaging pattern, appearing in LogAnalyzer (per-line!), ValidationEngine, CsvParser, TemplateEngine, and SlugGenerator.  
> - **Excessive `RegexOptions.Compiled`** in MarkdownStripper (46 instances) inflates startup time.  
> - **No `[GeneratedRegex]` usage** despite targeting .NET 8.0

**dotnet-perf-skills — `output/dotnet-perf-skills/run-5/analyze-perf-issues/performance-analysis.md`**
> #### 3. Per-Call `new Regex()` in Hot Paths (8 instances)  
> **Impact:** ... In `LogAnalyzer.TryParseLine`, this runs per log line — potentially millions of times.  
> **Fix:** Hoist to `private static readonly Regex` fields, or convert to `[GeneratedRegex]` on .NET 8+.  
> ...  
> #### 5. 47 `RegexOptions.Compiled` Without `[GeneratedRegex]` (47 instances)

**Score:** no-skills **4/5** (strong coverage, but less precise counting and less structured prioritization), dotnet-perf-skills **5/5** (explicit hot-path callout, startup budget framing, and precise count-based treatment).  
**Verdict:** **dotnet-perf-skills** is best due to tighter quantification and stronger .NET 8 `[GeneratedRegex]` guidance.

## 2. String Allocation Detection [CRITICAL]

**no-skills — `.../performance-analysis.md`**
> **String concatenation (`+=`) in loops** (CsvParser, LogAnalyzer, DataPipeline, TemplateEngine) — O(n²) allocation  
> ...  
> Every file that builds output uses `string += ...` instead of `StringBuilder`.  
> ...  
> **`.ToLower()` / `.ToUpper()` Without Culture** ... both a correctness bug (Turkish-I) and a performance issue

**dotnet-perf-skills — `.../performance-analysis.md`**
> #### 6. String `+=` Concatenation in Loops — O(n²) (9 loop sites)  
> ... char-by-char patterns in `CsvParser` are especially severe.  
> ...  
> #### 8. `.ToLower()`/`.ToUpper()` Without Culture or StringComparison (17 instances)  
> ...  
> **2. String concatenation anti-pattern is pervasive.** 9 loop sites and 60+ `.Replace()` call chains

**Score:** no-skills **4/5**, dotnet-perf-skills **5/5**.  
**Verdict:** **dotnet-perf-skills** wins on breadth (loops + casing + replace chains) and stronger quantification.

## 3. Collection and LINQ Efficiency [CRITICAL]

**no-skills — `.../performance-analysis.md`**
> **`.ToList()` + `.Contains()` for key dedup in `Diff()`** — O(n) per lookup. Should use `HashSet<string>`.  
> ...  
> **`.ToList()` materialisation + `Skip(i).Take(5).ToList()` in a loop** ... effectively O(n²) allocation.  
> ...  
> **`ContainsKey` + indexer** pattern repeated multiple times.

**dotnet-perf-skills — `.../performance-analysis.md`**
> #### 7. `ContainsKey` + Indexer Double-Lookup (13 instances)  
> **Impact:** ~2× slower per lookup ... many in inner loops  
> ...  
> #### 16. LINQ `Skip(i).Take(5).ToList()` in Loop — O(n²)  
> ...  
> #### 18. `List.Contains` O(n) Lookups in Loop (2 sites)

**Score:** no-skills **3/5** (good findings but weaker prioritization and less systematic aggregation), dotnet-perf-skills **5/5** (comprehensive and explicitly hot-path-aware).  
**Verdict:** **dotnet-perf-skills** clearly best.

## 4. Async and IO Pattern Detection [CRITICAL]

**no-skills — `.../performance-analysis.md`**
> **`new HttpClient` per request** (NotificationService) — socket exhaustion risk in production  
> ...  
> **Unbounded parallelism in `SendBatchParallelAsync()`** — fires all tasks simultaneously  
> ...  
> **Sequential `await` in loop** ...  
> **`Task.Delay` without `CancellationToken`**

**dotnet-perf-skills — `.../performance-analysis.md`**
> #### 1. `new HttpClient()` Per Call — Socket Exhaustion (3 instances)  
> ...  
> #### 14. Sequential `await` in Loop — No Parallelism (1 instance)  
> ...  
> #### 15. Unbounded `Task.WhenAll` Parallelism (1 instance)  
> ...  
> #### 21. Missing `CancellationToken` on `Task.Delay` (1 instance)

**Score:** no-skills **4/5**, dotnet-perf-skills **5/5**.  
**Verdict:** **dotnet-perf-skills** is stronger due to tighter issue framing and cleaner severity split.

## 5. Reflection and Serialization Overhead [HIGH]

**no-skills — `.../performance-analysis.md`**
> **Uncached `typeof(T).GetProperties()` and `.SetValue()`/`.GetValue()` on every call** ... Reflection is 100–1000× slower  
> ...  
> **`new JsonSerializerOptions { WriteIndented = true }` on every call** ... 10–100× slower than a cached instance

**dotnet-perf-skills — `.../performance-analysis.md`**
> #### 2. Uncached `new JsonSerializerOptions` Per Call (4 instances)  
> **Impact:** Up to 592× slower than cached options  
> ...  
> #### 12. Uncached Reflection `GetProperties()`/`GetProperty()`/`SetValue()`/`GetValue()` (6 instances)

**Score:** no-skills **4/5**, dotnet-perf-skills **4/5**.  
**Verdict:** **Tie**. Both detect the core high-impact reflection/serialization problems and provide actionable caching fixes.

## 6. Structural Optimization Detection [HIGH]

**no-skills — `.../performance-analysis.md`**
> **Structs Without `IEquatable<T>`** (affects 2 files)  
> ...  
> **Missing `FrozenDictionary` for Static Data** (affects 2 files)  
> ...  
> **Unsealed nested class `MappingConfig`**

**dotnet-perf-skills — `.../performance-analysis.md`**
> | `sealed class` | **0** |  
> | Unsealed non-abstract, non-static classes | 18 |  
> | `: IEquatable` on structs | **0** (2 structs without it) |  
> ...  
> #### 10. Unsealed Classes — 18 of 18 (systematic)

**Score:** no-skills **4/5**, dotnet-perf-skills **5/5**.  
**Verdict:** **dotnet-perf-skills** is better because it treats structural issues systematically (ratio-based, codebase-wide view).

## 7. Severity Classification Accuracy [HIGH]

**no-skills — `.../performance-analysis.md`**
> | 🔴 Critical | 7 |  
> | 🟡 Moderate | 22 |  
> | ℹ️ Info | 23 |  
> ...  
> | 8 | **Convert 46 `RegexOptions.Compiled` to `[GeneratedRegex]`** | 🟡 Moderate |

**dotnet-perf-skills — `.../performance-analysis.md`**
> **Total findings: 22** — 7 Critical, 11 Moderate, 4 Info  
> ...  
> **Top 3 priorities:** Fix `new HttpClient()` ..., cache `JsonSerializerOptions`, and hoist per-line `new Regex()`  
> ...  
> #### 5. 47 `RegexOptions.Compiled` Without `[GeneratedRegex]` ... **🔴 Critical**

**Score:** no-skills **3/5** (mostly correct criticals, but several high-impact items are diluted in moderate/info buckets), dotnet-perf-skills **5/5** (clear hot-path prioritization and consistent severity logic).  
**Verdict:** **dotnet-perf-skills** is significantly better for prioritization signal.

## 8. Fix Recommendation Quality [HIGH]

**no-skills — `.../performance-analysis.md`**
> **Fix for #31 — Use `IHttpClientFactory` or static `HttpClient`**  
> ...  
> **Fix for #11 — Cache reflection with `ConcurrentDictionary`**  
> ...  
> **Fix for #46 — Use `HashSet`**

**dotnet-perf-skills — `.../performance-analysis.md`**
> **Fix:** Inject `IHttpClientFactory` or use a single `static readonly HttpClient` with `PooledConnectionLifetime`.  
> ...  
> **Fix:** Convert all static literal patterns to `[GeneratedRegex]` source-generated methods.  
> ...  
> `CollectionsMarshal.GetValueRefOrAddDefault` ... `FrozenDictionary` ... `StringComparison.OrdinalIgnoreCase`

**Score:** no-skills **4/5**, dotnet-perf-skills **5/5**.  
**Verdict:** **dotnet-perf-skills** provides more specific modern .NET API guidance and stronger path-to-fix clarity.

## 9. Evidence Quantification and Scan Rigor [HIGH]

**no-skills — `.../gen-notes.md` / `.../performance-analysis.md`**
> `performance-analysis.md` — Full report with **52 findings** ...  
> ...  
> **Files Analyzed:** 9 source files ...

**dotnet-perf-skills — `.../gen-notes.md` / `.../performance-analysis.md`**
> **Scan Execution Checklist** ...  
> `RegexOptions.Compiled` | 47 (all in MarkdownStripper.cs)  
> `[GeneratedRegex]` | **0**  
> ...  
> **Verify-the-inverse rule** ... reporting ratios (e.g., "0 of 18 classes sealed")

**Score:** no-skills **3/5**, dotnet-perf-skills **5/5**.  
**Verdict:** **dotnet-perf-skills** has materially stronger methodological rigor (explicit scan recipes, inverse checks, and count-based evidence).

## Weighted Summary

Weights used: **Critical ×3**, **High ×2**, **Medium ×1**, **Low ×0.5**.

| Dimension | Tier | no-skills | dotnet-perf-skills |
|---|---|---:|---:|
| Regex Anti-Pattern Detection | Critical | 12 | 15 |
| String Allocation Detection | Critical | 12 | 15 |
| Collection and LINQ Efficiency | Critical | 9 | 15 |
| Async and IO Pattern Detection | Critical | 12 | 15 |
| Reflection and Serialization Overhead | High | 8 | 8 |
| Structural Optimization Detection | High | 8 | 10 |
| Severity Classification Accuracy | High | 6 | 10 |
| Fix Recommendation Quality | High | 8 | 10 |
| Evidence Quantification and Scan Rigor | High | 6 | 10 |
| **Total Weighted Score** |  | **81** | **108** |

## What All Versions Get Right

- Both identify the biggest production-risk items: per-call `HttpClient`, per-call regex in hot paths, and uncached serializer options.
- Both detect major allocation anti-patterns (`string +=` loops, repeated materialization).
- Both provide concrete fix direction rather than only descriptive critique.
- Both recognize structural opportunities (`IEquatable<T>`, `FrozenDictionary`, sealing classes), even with different depth.

## Summary: Impact of Skills

Most impactful differences, in order: **(1)** evidence rigor and quantification, **(2)** severity prioritization accuracy, **(3)** comprehensive collection/LINQ hot-path analysis, and **(4)** stronger .NET 8-specific regex modernization guidance. The baseline is competent and catches many core issues, but the skills-enabled output is more selective, more measurable, and more actionable for implementation planning. Overall assessment by weighted score: **dotnet-perf-skills (108) > no-skills (81)**.
