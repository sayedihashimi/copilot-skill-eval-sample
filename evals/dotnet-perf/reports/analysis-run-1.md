# Comparative Analysis: dotnet-perf-skills, no-skills

This run compares **2 configurations** across **1 scenario** (`analyze-perf-issues` in `output/{config}/run-1/analyze-perf-issues/`). Configuration identity comes from each `gen-notes.md`: `dotnet-perf-skills` explicitly reports the `analyzing-dotnet-performance` plugin skill, while `no-skills` provides generic generation notes without skill references.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 5 |
| String Allocation Detection [CRITICAL] | 5 | 5 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 5 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 5 |
| Reflection and Serialization Overhead [HIGH] | 5 | 5 |
| Structural Optimization Detection [HIGH] | 5 | 4 |
| Severity Classification Accuracy [HIGH] | 5 | 4 |
| Fix Recommendation Quality [HIGH] | 5 | 4 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**Coverage**

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-1/analyze-perf-issues/performance-analysis.md`):
> #### 2. `new Regex()` Per Log Line in Hot Path (4 instances)  
> **Impact:** `TryParseLine` is called per log line — potentially millions of times.  
> #### 10. 48 `RegexOptions.Compiled` Without `[GeneratedRegex]` (48 instances)

**no-skills** (`output/no-skills/run-1/analyze-perf-issues/performance-analysis.md`):
> **26. 🔴 Critical — `new Regex()` per log line in `TryParseLine`**  
> This is a **hot path** — called once per log line. For a 1M-line log file, this creates 3 million `Regex` objects.  
> **12. 🟡 Moderate — 46 `RegexOptions.Compiled` instances**

**Score**

| Configuration | Score | Why |
|---|---:|---|
| dotnet-perf-skills | 5 | Explicitly catches both per-call regex and compiled-regex startup budget; strongly recommends `[GeneratedRegex]`. |
| no-skills | 5 | Also clearly catches both required regex categories and recommends `[GeneratedRegex]`. |

**Verdict:** Tie. Both outputs comprehensively detect the critical regex issues.

## 2. String Allocation Detection [CRITICAL]

**Coverage**

**dotnet-perf-skills**:
> #### 12. String `+=` Concatenation in Loops — O(n²) Allocation (9 instances)  
> #### 11. `.ToLower()`/`.ToUpper()` Without StringComparison (25 instances)  
> #### 19. Chained `.Replace()` in MarkdownStripper.StripMarkdown (44 regex replacements)

**no-skills**:
> **58. 🔴 Critical — Character-by-character string concatenation**  
> `current += line[i]` creates a new string for every character... this is O(n²) allocations.  
> **13. 🟡 Moderate — Long chain of `.Replace()` calls**

**Score**

| Configuration | Score | Why |
|---|---:|---|
| dotnet-perf-skills | 5 | Captures loop concatenation, casing allocations, and replace-chain intermediates with breadth and counts. |
| no-skills | 5 | Finds the same high-impact string issues with clear examples and concrete fixes. |

**Verdict:** Tie. Both are strong on string-allocation anti-patterns.

## 3. Collection and LINQ Efficiency [CRITICAL]

**Coverage**

**dotnet-perf-skills**:
> #### 8. `List.Contains()` for Key Lookups — O(n²) in Diff  
> #### 13. `ContainsKey` + Indexer Double-Lookup (12 instances)  
> #### 17. `Skip(i).Take(5).ToList()` in Loop — O(n²) Sliding Window

**no-skills**:
> **9. 🟡 Moderate — `List.Contains()` in a loop**  
> **29. 🟡 Moderate — `ContainsKey` + indexer pattern**  
> **30. 🟡 Moderate — `Skip(i).Take(5).ToList()` in loop**

**Score**

| Configuration | Score | Why |
|---|---:|---|
| dotnet-perf-skills | 5 | Covers all requested collection/LINQ hot-path issues, including materialization overhead. |
| no-skills | 5 | Also identifies all required collection misuse patterns with correct alternatives. |

**Verdict:** Tie. Coverage is complete in both outputs.

## 4. Async and IO Pattern Detection [CRITICAL]

**Coverage**

**dotnet-perf-skills**:
> #### 1. `new HttpClient()` Per Call — Socket Exhaustion (3 instances)  
> #### 14. Unbounded Parallelism in `SendBatchParallelAsync`  
> #### 15. Missing `CancellationToken` on Async Methods

**no-skills**:
> **15. 🔴 Critical — `new HttpClient()` per call**  
> **16. 🔴 Critical — Unbounded parallelism in `SendBatchParallelAsync`**  
> **18. 🟡 Moderate — Missing `CancellationToken` on all async methods**

**Score**

| Configuration | Score | Why |
|---|---:|---|
| dotnet-perf-skills | 5 | Fully covers socket exhaustion, sequential vs bounded concurrency, and cancellation propagation. |
| no-skills | 5 | Fully covers the same async/IO anti-pattern set with direct fixes. |

**Verdict:** Tie. Both outputs strongly detect production-relevant async/IO risks.

## 5. Reflection and Serialization Overhead [HIGH]

**Coverage**

**dotnet-perf-skills**:
> #### 4. Uncached `new JsonSerializerOptions` Per Call (6 instances)  
> #### 7. Uncached Reflection: `GetProperties()`/`SetValue()`/`GetValue()` Per Call  
> **Impact:** Reflection is ~100x slower...

**no-skills**:
> **47. 🔴 Critical — Uncached `GetProperties()` and `SetValue()` reflection**  
> **53. 🟡 Moderate — `new JsonSerializerOptions` per call**  
> ...Creating one per call wastes that work.

**Score**

| Configuration | Score | Why |
|---|---:|---|
| dotnet-perf-skills | 5 | Strongly identifies both reflection and serializer-caching hot spots, with impact framing. |
| no-skills | 5 | Detects both required categories and provides suitable mitigation patterns. |

**Verdict:** Tie. Both analyses are solid on reflection/serialization overhead.

## 6. Structural Optimization Detection [HIGH]

**Coverage**

**dotnet-perf-skills**:
> #### 26. `static readonly Dictionary<>` — FrozenDictionary Candidates (2 instances)  
> #### 27. Structs Without `IEquatable<T>` (2 of 2 structs)  
> #### 28. Unsealed Leaf Classes (14 of 17 classes)

**no-skills**:
> `Record`, `ValidationResult`, `MappingConfig` — prevents JIT devirtualization.  
> `DeliveryResult` and `ValidationError` ... don't implement `IEquatable<T>`.  
> `Converters` dictionary could be `FrozenDictionary`.

**Score**

| Configuration | Score | Why |
|---|---:|---|
| dotnet-perf-skills | 5 | Broad, quantified structural scan (classes/structs) plus FrozenDictionary targeting. |
| no-skills | 4 | Correct findings, but narrower structural breadth and less systematic quantification. |

**Verdict:** **dotnet-perf-skills** is better due to fuller structural coverage and stronger scale analysis.

## 7. Severity Classification Accuracy [HIGH]

**Coverage**

**dotnet-perf-skills**:
> | 🔴 Critical | 9 | `new HttpClient()` per call... uncached `new Regex()`... uncached `JsonSerializerOptions` |  
> | 🟡 Moderate | 22 | 48 `RegexOptions.Compiled`... `.ToLower()`/`.ToUpper()`... |

**no-skills**:
> **Top priorities:**  
> 1. `new HttpClient` per call...  
> 2. Regex instantiation per log line...

**Score**

| Configuration | Score | Why |
|---|---:|---|
| dotnet-perf-skills | 5 | Better hot-path impact calibration with numeric impact framing and severity rationale. |
| no-skills | 4 | Generally correct ordering, but less explicit benchmark-based calibration and lower consistency. |

**Verdict:** **dotnet-perf-skills** provides more reliable prioritization signal.

## 8. Fix Recommendation Quality [HIGH]

**Coverage**

**dotnet-perf-skills**:
> **Fix:** Inject `IHttpClientFactory` or use a `static readonly HttpClient`...  
> **Fix:** Convert all 48 static `Regex` fields to `[GeneratedRegex]` partial methods.  
> **Caveat:** Consolidating 44 regex replacements into a single pass is a significant refactor.

**no-skills**:
> **Fix:** Use `HashSet<string>`.  
> **Fix:** Add `CancellationToken cancellationToken = default`...  
> **Fix:** Cache property info per type and use compiled delegates.

**Score**

| Configuration | Score | Why |
|---|---:|---|
| dotnet-perf-skills | 5 | Specific APIs, concrete patterns, and practical caveats reduce risk of bad implementation choices. |
| no-skills | 4 | Actionable and mostly correct, but less consistently scoped/qualified than the skills output. |

**Verdict:** **dotnet-perf-skills** gives the most actionable, implementation-safe guidance.

## Weighted Summary

Weights: **Critical ×3**, **High ×2**.

| Configuration | Critical subtotal (max 60) | High subtotal (max 40) | Total weighted score (max 100) |
|---|---:|---:|---:|
| dotnet-perf-skills | 60 | 40 | **100** |
| no-skills | 60 | 34 | **94** |

## What All Versions Get Right

- Both flag the highest-risk production issue: per-call `HttpClient` and socket exhaustion.
- Both strongly detect regex misuse (per-call instantiation and excessive compiled-regex startup cost).
- Both identify major string-allocation hot spots (`+=` loops, replace chains, casing allocations).
- Both call out reflection and serializer caching issues with practical .NET fix patterns.

## Summary: Impact of Skills

Most impactful differences:
1. **Stronger structural and scale-based coverage** in `dotnet-perf-skills` (quantified class/struct audits, checklist hit counts).
2. **Better severity calibration** in `dotnet-perf-skills` via explicit impact framing and prioritization logic.
3. **Higher fix precision** in `dotnet-perf-skills` through caveats and tighter API-level guidance.

Overall assessment: both configurations perform very well on core performance anti-pattern detection in this scenario, but **`dotnet-perf-skills` ranks first (100 vs 94 weighted)** because it adds clearer prioritization and more implementation-ready recommendations on top of similarly strong baseline detection.
