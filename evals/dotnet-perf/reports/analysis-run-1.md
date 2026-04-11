# Comparative Analysis: dotnet-perf-skills, no-skills

This run compares **2 configurations** across **1 scenario** (`analyze-perf-issues`): `output/dotnet-perf-skills/run-1/analyze-perf-issues` and `output/no-skills/run-1/analyze-perf-issues`. Configuration identity was taken from each scenario’s `gen-notes.md` (skill-driven notes for `dotnet-perf-skills`, baseline analysis notes for `no-skills`).

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 5 |
| String Allocation Detection [CRITICAL] | 5 | 5 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 4 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 4 |
| Reflection and Serialization Overhead [HIGH] | 5 | 4 |
| Structural Optimization Detection [HIGH] | 5 | 4 |
| Severity Classification Accuracy [HIGH] | 3 | 4 |
| Fix Recommendation Quality [HIGH] | 5 | 5 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-1/analyze-perf-issues/performance-analysis.md`)
> `new Regex(` per-call | 8  
> `RegexOptions.Compiled` | 48 (all in MarkdownStripper.cs)  
> `[GeneratedRegex]` | 0 (1 hit was a comment)

> `new Regex()` per log line in `LogAnalyzer.TryParseLine` (3 instances)  
> ...for a 100K-line log file, this creates 300K+ `Regex` objects.

**no-skills** (`output/no-skills/run-1/analyze-perf-issues/performance-analysis.md`)
> **`new Regex(...)` inside `TryParseLine`** — called for **every log line**...  
> `MarkdownStripper` — 45+ `RegexOptions.Compiled` instances → excessive JIT startup cost

> **Recommendation**: ...On .NET 7+, use `[GeneratedRegex]` source generators...

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**.  
**Verdict:** **Tie**; both correctly capture per-call regex construction, compiled-regex budget overuse, and GeneratedRegex migration.

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills**
> String `+=` concatenation in loops — O(n²) allocation (8 sites)  
> Files: ... `CsvParser` ... `LogAnalyzer` ... `TemplateEngine` ...

> `.ToLower()`/`.ToUpper()` without culture...  
> ...Turkish-I problem...

**no-skills**
> **Char-by-char `current += line[i]` in `ParseLine`** — O(n²) string allocation  
> String `+=` concatenation for CSV output in `FormatCsv`

> **Recommendation**: Use `ToLowerInvariant()` ... or `StringComparison.OrdinalIgnoreCase`.

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**.  
**Verdict:** **Tie**; both are strong on loop concatenation, chained replacements, and culture/casing correctness.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills**
> `ContainsKey` (potential double-lookup) | 21 (12 are ContainsKey+indexer)  
> `.ToList()` | 20

> `Skip(i).Take(5).ToList()` in a loop — O(n²) allocation...  
> `List.Contains()` for repeated O(n) lookups... use `HashSet<string>`.

**no-skills**
> `ContainsKey` + indexer pattern... use `TryGetValue`  
> `flat1.Keys.ToList()` + `.Contains()` for key set union in `Diff` — O(n²)

> `.Distinct().ToList()` allocates two collections...

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills wins** on breadth and quantification (explicit counts and stronger hot-path framing).

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills**
> `new HttpClient()` per call — socket exhaustion risk (3 instances)  
> Sequential `await` in loop — no parallelism  
> Unbounded parallelism in `SendBatchParallelAsync`

> Missing `CancellationToken` on async methods (all async methods)

**no-skills**
> **`new HttpClient()` in `SendEmailAsync`**... socket exhaustion  
> Sequential `await` in `SendBatchAsync` loop  
> **Unbounded parallelism** in `SendBatchParallelAsync`

> `Task.Delay(_retryDelay)` ... **no cancellation token**

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills wins** with more complete cancellation-propagation coverage.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills**
> Uncached `new JsonSerializerOptions` per call (5 instances)  
> ...up to 592× slower...

> Uncached reflection: `GetProperties()` + `SetValue()`/`GetValue()` per call

**no-skills**
> `typeof(TTarget).GetProperties()` on every `MapTo<T>` call... should be cached  
> `new JsonSerializerOptions { WriteIndented = true }`... allocated per call

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills wins** by tying findings to stronger quantified impact.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills**
> Unsealed non-abstract non-static classes | 17  
> `public struct` without `IEquatable<T>` | 2 of 2  
> `static readonly FrozenDictionary<` | 0

> 17 unsealed leaf classes...  
> `static readonly Dictionary<>` — FrozenDictionary candidates (2 instances)

**no-skills**
> Appears in: `DataPipeline.Record`, `ValidationEngine.ValidationResult`, `EntityMapper.MappingConfig`  
> ...Structs Without `IEquatable<T>` ... `DeliveryResult`, `ValidationError`

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills wins** with full-project counting and clearer systematic-gap quantification.

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills**
> 🔴 Critical | 8 | ... uncached `JsonSerializerOptions`, ...  
> #### 5. `ContainsKey` + indexer double-lookup pattern (12 instances)

**no-skills**
> 🔴 **Critical**: 7 issues — socket exhaustion, O(n²) string concatenation in hot paths, regex allocation per log line  
> ℹ️ **Info**: ... missing capacity hints, `ContainsKey` + indexer patterns...

**Score:** dotnet-perf-skills **3/5**, no-skills **4/5**.  
**Verdict:** **no-skills wins**; its priority ordering better separates existential hot-path risks from moderate/info micro-optimizations.

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills**
> **Fix:** Inject `IHttpClientFactory` or use a `static readonly HttpClient`...  
> **Fix:** Hoist to `private static readonly Regex` fields, or use `[GeneratedRegex]`

**no-skills**
> **Fix for #1-#3** — Inject `IHttpClientFactory`  
> **Fix for #1** — Convert to `[GeneratedRegex]`...

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**.  
**Verdict:** **Tie**; both are specific, API-correct, and actionable with concrete implementation patterns.

## Weighted Summary

Weights applied: **CRITICAL ×3**, **HIGH ×2**.

| Configuration | Weighted Total |
|---|---:|
| dotnet-perf-skills | 96 |
| no-skills | 88 |

## What All Versions Get Right

- Both identify the top production risks: per-call `HttpClient`, per-call regex in `LogAnalyzer.TryParseLine`, and O(n²) string-building loops.
- Both call out excessive `RegexOptions.Compiled` usage in `MarkdownStripper` and recommend `[GeneratedRegex]`.
- Both detect reflection (`GetProperties`/`SetValue`) and serializer-options caching opportunities.
- Both provide practical fix patterns (`StringBuilder`, `TryGetValue`, `HashSet`, `IHttpClientFactory`).

## Summary: Impact of Skills

The most impactful differences are **coverage depth and quantification**: `dotnet-perf-skills` gives stronger systematic counts (e.g., regex/casing/sealed/contains patterns), more explicit scan evidence, and broader structural coverage. Baseline `no-skills` remains strong and in one area (severity calibration) is slightly better balanced. Overall ranking by weighted score is: **1) dotnet-perf-skills (96)**, **2) no-skills (88)**.
