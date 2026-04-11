# Comparative Analysis: dotnet-perf-skills, no-skills

This report compares **2 configurations** for **1 scenario** (`analyze-perf-issues`) using outputs from `output/{config}/run-1/analyze-perf-issues/`. Configuration mapping was taken from `gen-notes.md`: `dotnet-perf-skills` used the **analyzing-dotnet-performance plugin skill**, while `no-skills` is the baseline Copilot output.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 4 |
| String Allocation Detection [CRITICAL] | 5 | 4 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 4 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 4 |
| Reflection and Serialization Overhead [HIGH] | 4 | 4 |
| Structural Optimization Detection [HIGH] | 5 | 4 |
| Severity Classification Accuracy [HIGH] | 4 | 3 |
| Fix Recommendation Quality [HIGH] | 5 | 3 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-1/analyze-perf-issues/performance-analysis.md`):
> **Impact:** Regex construction parses the pattern and builds an automaton every time. In `LogAnalyzer.TryParseLine`, this happens per log line — potentially millions of times.  
> **Files:** LogAnalyzer.cs:L50, L65, L75, L197  
> **Fix:** Hoist to `static readonly` fields or use `[GeneratedRegex]` (preferred on .NET 8).

> **Impact:** Each `Compiled` regex JIT-compiles at first use. 48 compiled regexes is a significant startup cost. On .NET 8, `[GeneratedRegex]` source-generates the matching code with zero startup cost.

**no-skills** (`output/no-skills/run-1/analyze-perf-issues/performance-analysis.md`):
> **`new Regex(...)` on every log line in `TryParseLine`** — this is called per line, potentially millions of times.  
> **Worst single issue in the codebase.**

> **40+ `RegexOptions.Compiled` static instances** — each JIT-compiles at startup, increasing cold-start time significantly. On .NET 8+, these should use `[GeneratedRegex]` source generators for AOT-friendly compilation.

**Score:** dotnet-perf-skills **5/5** (full coverage including per-call allocation, startup budget impact, and .NET 8 guidance); no-skills **4/5** (strong detection, slightly less consistent precision across counts/recommendation details).

**Verdict:** **dotnet-perf-skills** is best due to tighter coverage and clearer distinction between hot-path and startup regex costs.

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills** (`performance-analysis.md`):
> **Impact:** Each `+=` allocates a new string copying all previous content. For N items, total allocation is O(n²).  
> **Files:** TemplateEngine.cs... DataPipeline.cs... LogAnalyzer.cs... NotificationService.cs... CsvParser.cs...

> **Impact:** Each `.Replace()` allocates a new string... Total: ~21 string allocations per `GenerateSlug` call.

> **Impact:** Allocates a new string + uses culture-sensitive rules (Turkish-I bug). 2-3x slower than `StringComparison.OrdinalIgnoreCase`.

**no-skills** (`performance-analysis.md`):
> **Char-by-char `+=` string concatenation in `ParseLine`**... O(n²) for long fields.

> **Long chain of `.Replace()` calls** — each allocates a new string. For large documents this creates ~40 intermediate string allocations.

> **`.ToLower()` without `StringComparison.OrdinalIgnoreCase`** — allocates a new string each time, and is locale-sensitive (Turkish-I bug).

**Score:** dotnet-perf-skills **5/5** (covers `+=` loops, replacement chains, and casing allocation/culture risks comprehensively); no-skills **4/5** (good detection, but less complete linkage of allocation chains across components).

**Verdict:** **dotnet-perf-skills** provides broader, more systematic allocation analysis.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills** (`performance-analysis.md`):
> **`ContainsKey` + indexer double-lookup (10 actionable instances)**... Two hash lookups where one `TryGetValue` suffices.

> **`.ToList()` materializing unnecessarily**... `LogAnalyzer.cs:L152, L157 (Skip/Take/ToList per iteration)`...

> **`List.Contains()` — O(n) lookups in loops**... `SlugGenerator.GenerateUniqueSlug`...

**no-skills** (`performance-analysis.md`):
> **`allKeys.ToList()` + `.Contains()` for key lookups in `Diff`** — O(n) per lookup instead of O(1) with `HashSet<string>`.

> **`errorEntries.Skip(i).Take(5).ToList()` in a loop** — O(n²) LINQ allocations for sliding window.

> **`ContainsKey` + indexer double lookup** — use `TryGetValue`.

**Score:** dotnet-perf-skills **5/5** (hits all key patterns in rubric, including materialization and sliding-window LINQ costs); no-skills **4/5** (strong coverage but less cross-cutting prioritization and consistency).

**Verdict:** **dotnet-perf-skills** is best for complete and prioritized collection/LINQ guidance.

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills** (`performance-analysis.md`):
> **`new HttpClient()` per call — socket exhaustion (3 instances)**...  
> **Fix:** Inject `IHttpClientFactory` or use a single `static readonly HttpClient`...

> **Sequential awaits in loop — no parallelism**...  
> **Unbounded parallelism in `SendBatchParallelAsync`**...

> **Missing `CancellationToken` on async methods**... `Task.Delay` in retry loops cannot be cancelled.

**no-skills** (`performance-analysis.md`):
> **`new HttpClient()` per call... causes socket exhaustion under load.**

> **Sequential `await` in `SendBatchAsync` loop**...  
> **Unbounded parallelism in `SendBatchParallelAsync`**...

> **`Task.Delay` without `CancellationToken`** in retry loop.

**Score:** dotnet-perf-skills **5/5** (covers all specified anti-patterns including token propagation); no-skills **4/5** (captures core issues, but cancellation propagation is less fully treated).

**Verdict:** **dotnet-perf-skills** is best due to clearer end-to-end async hygiene guidance.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills** (`performance-analysis.md`):
> **`new JsonSerializerOptions` per call (4 instances)**... Each construction rebuilds type metadata caches.

> **Uncached reflection in hot paths (6 instances)**... `GetProperties()` and `GetProperty()` perform metadata lookups every call.

**no-skills** (`performance-analysis.md`):
> **`new JsonSerializerOptions { WriteIndented = true }` on every call** — options objects are expensive; they cache metadata internally.

> **`typeof(TTarget).GetProperties()` called on every `MapTo<T>` invocation** — reflection is extremely expensive in hot paths.

**Score:** dotnet-perf-skills **4/5**; no-skills **4/5**. Both detect uncached reflection and per-call serializer options well. Neither output strongly develops partial parsing (`Utf8JsonReader`) opportunities.

**Verdict:** **Tie** — both are strong on the core overhead patterns.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills** (`performance-analysis.md`):
> **Unsealed leaf classes (16 of 16 classes are unsealed)**... 0 of 16 classes are sealed.

> **Structs without `IEquatable<T>` (2 structs, 0 implement it)**...

> **`static readonly Dictionary<>` — FrozenDictionary candidates (2 instances)**...

**no-skills** (`performance-analysis.md`):
> **`MappingConfig` is an unsealed nested class**...

> **`ValidationError` struct without `IEquatable<ValidationError>`**...

> **Static `Converters` dictionary — candidate for `FrozenDictionary` on .NET 8+.**

**Score:** dotnet-perf-skills **5/5** (systematic/codebase-level structural analysis); no-skills **4/5** (good pattern detection, less complete breadth and quantification).

**Verdict:** **dotnet-perf-skills** is best for completeness and quantified structural gaps.

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills** (`performance-analysis.md`):
> | 🔴 Critical | 8 | `new HttpClient()` per call..., uncached `new Regex()` in hot loops, `new JsonSerializerOptions` per call, uncached reflection |

> **Top 3 priorities:**  
> 1. 🔴 `new HttpClient()`...  
> 2. 🔴 `new Regex()` per line...  
> 3. 🔴 `new JsonSerializerOptions` per call...

**no-skills** (`performance-analysis.md`):
> | 🔴 Critical | 6 | Socket exhaustion, O(n²) hot paths, per-line regex compilation |

> **Top priorities:** Fix `new HttpClient` per call..., cache regex in `LogAnalyzer.TryParseLine`..., replace char-by-char `+=`...

**Score:** dotnet-perf-skills **4/5** (good hot-path prioritization, minor over-escalation on some patterns); no-skills **3/5** (reasonable but less consistently tiered by impact and scale across categories).

**Verdict:** **dotnet-perf-skills** better separates urgent production risks from secondary optimizations.

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills** (`gen-notes.md`, `performance-analysis.md`):
> **Common Pitfalls guidance:** ...  
> - Not suggesting `[GeneratedRegex]` for `ValidationEngine.AddPattern` (dynamic pattern string)  
> - Not suggesting `Span<T>` in async methods

> **Fix:** Inject `IHttpClientFactory`... use `[GeneratedRegex]`... `StringComparison.OrdinalIgnoreCase`... `FrozenDictionary`...

**no-skills** (`performance-analysis.md`):
> // #3: Cache regex or use GeneratedRegex  
> `[GeneratedRegex(pattern, RegexOptions.IgnoreCase)]`

> **Fix recommendations** include good API choices (`HashSet`, `StringBuilder`, `Parallel.ForEachAsync`) but contain at least one less-safe suggestion pattern for dynamic regex usage.

**Score:** dotnet-perf-skills **5/5** (specific, modern, and guarded against common incorrect advice); no-skills **3/5** (often actionable, but includes weaker/incorrect pattern suggestions in places).

**Verdict:** **dotnet-perf-skills** is clearly best due to higher correctness and API specificity.

## Weighted Summary

Weights used: **Critical ×3**, **High ×2**.

| Dimension | Tier | Weight | dotnet-perf-skills | no-skills |
|---|---|---:|---:|---:|
| Regex Anti-Pattern Detection | Critical | 3 | 5×3 = 15 | 4×3 = 12 |
| String Allocation Detection | Critical | 3 | 5×3 = 15 | 4×3 = 12 |
| Collection and LINQ Efficiency | Critical | 3 | 5×3 = 15 | 4×3 = 12 |
| Async and IO Pattern Detection | Critical | 3 | 5×3 = 15 | 4×3 = 12 |
| Reflection and Serialization Overhead | High | 2 | 4×2 = 8 | 4×2 = 8 |
| Structural Optimization Detection | High | 2 | 5×2 = 10 | 4×2 = 8 |
| Severity Classification Accuracy | High | 2 | 4×2 = 8 | 3×2 = 6 |
| Fix Recommendation Quality | High | 2 | 5×2 = 10 | 3×2 = 6 |
| **Total Weighted Score** |  |  | **96** | **76** |

## What All Versions Get Right

- Both identify the highest-risk production issue: per-call `HttpClient` creation.
- Both flag hot-path per-call regex instantiation in `LogAnalyzer`.
- Both call out excessive `RegexOptions.Compiled` usage in `MarkdownStripper` and recommend `[GeneratedRegex]` directionally.
- Both detect O(n²)-style string concatenation (`+=`) patterns in loops.
- Both highlight reflection and serializer-options caching opportunities.

## Summary: Impact of Skills

Most impactful differences: **(1)** stronger regex + async rigor, **(2)** better structural/systematic quantification, and **(3)** higher-quality, safer fix guidance.  
Overall assessment by weighted score: **dotnet-perf-skills (96)** provides the more reliable and implementation-ready analysis; **no-skills (76)** is still useful and broad, but less consistent in severity calibration and fix correctness.
