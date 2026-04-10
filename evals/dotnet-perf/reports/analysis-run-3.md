# Comparative Analysis: dotnet-perf-skills, no-skills

I compared **2 configuration outputs** under `output/{config}/run-3/analyze-perf-issues/` for the single scenario `analyze-perf-issues`. Configuration identity was taken from directory names and validated with `gen-notes.md`: `dotnet-perf-skills` explicitly cites the `analyzing-dotnet-performance` skill, while `no-skills` contains baseline-style generation notes.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 4 |
| String Allocation Detection [CRITICAL] | 5 | 4 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 4 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 4 |
| Reflection and Serialization Overhead [HIGH] | 5 | 4 |
| Structural Optimization Detection [HIGH] | 5 | 4 |
| Severity Classification Accuracy [HIGH] | 4 | 5 |
| Fix Recommendation Quality [HIGH] | 5 | 4 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-3/analyze-perf-issues/performance-analysis.md`)
> | `new Regex(` (uncached per-call) \| 8 \| LogAnalyzer (4), TemplateEngine (2), ValidationEngine (1), CsvParser (1) |  
> | `RegexOptions.Compiled` \| 48 \| All in MarkdownStripper.cs |  
> | `[GeneratedRegex]` \| 0 \| None in codebase |

**no-skills** (`output/no-skills/run-3/analyze-perf-issues/performance-analysis.md`)
> 2. **Regex instantiation per line** in `LogAnalyzer.TryParseLine` (millions of allocations in hot path)  
> | 15 \| 🟡 Moderate \| 13-59 \| Regex \| **46 `RegexOptions.Compiled` instances** ... should use `[GeneratedRegex]` |

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best due to explicit scan counts and ratio-based coverage (`new Regex`, compiled usage, and zero GeneratedRegex adoption).

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills** (`.../performance-analysis.md`)
> | `.ToLower()/.ToUpper()` without culture \| 15 \| EntityMapper (5), CsvParser (6), DataPipeline (1), JsonTransformer (2), LogAnalyzer (2), SlugGenerator (1) |  
> **Impact:** `SlugGenerator.GenerateSlug` ... allocates ~21 intermediate strings.  
> String `+=` concatenation in loops appears in **7 of 10 files**.

**no-skills** (`.../performance-analysis.md`)
> 3. **O(n²) string concatenation** in loops across `TemplateEngine`, `CsvParser`, `LogAnalyzer`, and `DataPipeline`  
> | 57 \| 🔴 Critical \| ... \| **Character-by-character string concatenation** (`current += line[i]`) ... Must use `StringBuilder`. |  
> | 9 \| 🟡 Moderate \| ... \| `.ToLower()` without culture — susceptible to Turkish-I bug. |

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is more comprehensive and quantified; no-skills is good but less systematic.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills** (`.../performance-analysis.md`)
> | `ContainsKey` + indexer (double lookup) \| 12 \| Across 6 files |  
> #### 22. LINQ `Skip(i).Take(5).ToList()` in Loop — O(n²) ...  
> **Fix:** Use `HashSet<string>` instead of `List<string>`.

**no-skills** (`.../performance-analysis.md`)
> | 14 \| 🔴 Critical \| ... \| `existingSlugs.ToList()` ... `.Contains()` is O(n) per call ... Should use `HashSet<string>`. |  
> | 33 \| 🟡 Moderate \| ... \| `Skip(i).Take(5).ToList()` inside a loop — O(n²) allocation for sliding window. |  
> | 32 \| ℹ️ Info \| ... \| `ContainsKey` + indexer pattern instead of `TryGetValue`. |

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** wins on breadth and explicit counts; no-skills still covers key hotspots.

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills** (`.../performance-analysis.md`)
> #### 1. `new HttpClient()` Per Call — Socket Exhaustion (3 instances)  
> #### 16. Unbounded Parallelism in `SendBatchParallelAsync` (1 instance)  
> #### 17. `Task.Delay` Without CancellationToken (1 instance)

**no-skills** (`.../performance-analysis.md`)
> | 18 \| 🔴 Critical \| ... \| **`new HttpClient()` per call** ... socket exhaustion ... |  
> | 21 \| 🟡 Moderate \| ... \| Sequential `await` in `foreach` loop in `SendBatchAsync` ... |  
> | 22 \| 🔴 Critical \| ... \| **Unbounded parallelism** in `SendBatchParallelAsync` ... |

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best due to full pattern set with stronger operational framing.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills** (`.../performance-analysis.md`)
> #### 5. `new JsonSerializerOptions` Per Call — Up to 592x Slower (4 instances)  
> #### 8. Uncached Reflection `GetProperties()`/`GetProperty()`/`SetValue()` (3 instances)  
> **Fix:** Cache `PropertyInfo[]` per type in a `static ConcurrentDictionary<Type, PropertyInfo[]>`.

**no-skills** (`.../performance-analysis.md`)
> | 44 \| 🔴 Critical \| ... \| **Uncached `GetProperties()` and `SetValue()`/`GetValue()`** ... |  
> | 53 \| 🟡 Moderate \| ... \| **`new JsonSerializerOptions { WriteIndented = true }` on every call** ... |

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is stronger, especially on serializer-cost severity and quantified impact.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills** (`.../performance-analysis.md`)
> | Unsealed classes / sealed classes \| 18 / 0 \| 0 of 18 classes are sealed |  
> | `struct` without `IEquatable<T>` \| 2 / 0 \| DeliveryResult, ValidationError ... |  
> #### 18. `static readonly Dictionary<>` — FrozenDictionary Candidates (2 instances)

**no-skills** (`.../performance-analysis.md`)
> | 38 \| ℹ️ Info \| ... \| `Record` class is unsealed ... |  
> | 49 \| ℹ️ Info \| ... \| `ValidationError` struct without `IEquatable<ValidationError>`. |  
> | 46 \| ℹ️ Info \| ... \| Static `Converters` dictionary — candidate for `FrozenDictionary` on .NET 8+. |

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** leads with codebase-level structural quantification, not just local findings.

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills** (`.../performance-analysis.md`)
> | 🔴 Critical \| 10 \| `new HttpClient()` ... uncached `new Regex()` ... `new JsonSerializerOptions` ... |  
> #### 6. `ContainsKey` + Indexer Double-Lookup Pattern (12 instances)

**no-skills** (`.../performance-analysis.md`)
> | 🔴 Critical \| 7 |  
> 1. **Socket exhaustion** from `new HttpClient()` per call ...  
> 2. **Regex instantiation per line** in `LogAnalyzer.TryParseLine` ...

**Score:** dotnet-perf-skills **4/5**, no-skills **5/5**.  
**Verdict:** **no-skills** is slightly better calibrated: it keeps highest severity focused on hot-path and incident-class risks, while dotnet-perf-skills appears to over-escalate some medium-impact patterns (e.g., dictionary double-lookup).

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills** (`.../performance-analysis.md`)
> **Fix:** Inject `IHttpClientFactory` or use a single `static readonly HttpClient` with `PooledConnectionLifetime`.  
> **Fix:** Hoist to `private static readonly` fields, or use `[GeneratedRegex]` (preferred on .NET 8).  
> **Fix:** Convert to `.ToFrozenDictionary()` on .NET 8+.

**no-skills** (`.../performance-analysis.md`)
> // After — use static readonly fields or [GeneratedRegex]:  
> [GeneratedRegex(@"\{\{#if\s+(\w+)\}\}([\s\S]*?)\{\{/if\}\}")]  
> // After — inject IHttpClientFactory or use a static/shared instance:

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** provides the most actionable and API-specific guidance with clearer prioritization rationale.

## Weighted Summary

Weights applied: **Critical ×3**, **High ×2**.

| Configuration | Critical Raw (4 dims) | High Raw (4 dims) | Weighted Total |
|---|---:|---:|---:|
| dotnet-perf-skills | 20 | 19 | **98** |
| no-skills | 16 | 17 | **82** |

## What All Versions Get Right

- Both identify the major production-risk issues: per-call `HttpClient`, per-call/hot-path regex creation, and O(n²) string building.
- Both include concrete .NET-oriented fixes (`StringBuilder`, `[GeneratedRegex]`, `HashSet`, `IHttpClientFactory`, `TryGetValue`).
- Both recognize culture-sensitive casing (`ToLower`/`ToUpper`) as both performance and correctness risk.
- Both surface reflection and serializer caching concerns rather than only micro-optimizations.

## Summary: Impact of Skills

Most impactful differences: **(1)** broader scan coverage with explicit hit counts/ratios, **(2)** stronger cross-file synthesis, and **(3)** more prescriptive modern .NET guidance (`[GeneratedRegex]`, `FrozenDictionary`, quantified serializer cost) in `dotnet-perf-skills`. Overall, `dotnet-perf-skills` delivers the stronger analysis by weighted score (**98 vs 82**), while `no-skills` remains solid and slightly better in severity strictness.
