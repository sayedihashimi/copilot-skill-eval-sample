# Comparative Analysis: dotnet-perf-skills, dotnet-perf-skills-improved, no-skills

This run compares **3 configurations** over **1 scenario** (`analyze-perf-issues`): `dotnet-perf-skills`, `dotnet-perf-skills-improved`, and `no-skills`, using outputs under `output/{config}/run-3/analyze-perf-issues/`. Configuration identity was confirmed via each directory’s `gen-notes.md` (skill-enabled notes for both skills variants; generic notes for baseline).

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | dotnet-perf-skills-improved | no-skills |
|---|---:|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 4 | 5 | 4 |
| String Allocation Detection [CRITICAL] | 5 | 4 | 5 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 5 | 4 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 5 | 5 |
| Reflection and Serialization Overhead [HIGH] | 5 | 4 | 5 |
| Structural Optimization Detection [HIGH] | 5 | 5 | 3 |
| Severity Classification Accuracy [HIGH] | 3 | 5 | 2 |
| Fix Recommendation Quality [HIGH] | 4 | 5 | 3 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-3/analyze-perf-issues/performance-analysis.md`):
> #### 2. `new Regex()` per log line in hot path (8 instances)  
> #### 8. 48 `RegexOptions.Compiled` without `[GeneratedRegex]` (48 instances)  
> **Fix:** Hoist to `static readonly` fields, or use `[GeneratedRegex]` on .NET 7+.

**dotnet-perf-skills-improved** (`output/dotnet-perf-skills-improved/run-3/analyze-perf-issues/performance-analysis.md`):
> #### 3. `new Regex()` inside per-line parsing loop (4 instances in hot path)  
> #### 4. 48 `RegexOptions.Compiled` without `[GeneratedRegex]` (MarkdownStripper)  
> On .NET 8+, `[GeneratedRegex]` source-generates the regex at build time with zero startup cost.

**no-skills** (`output/no-skills/run-3/analyze-perf-issues/performance-analysis.md`):
> 1. 🔴 **Per-call Regex instantiation in hot path** (lines 22, 30, 43, 73)  
> 1. 🟡 **47 `RegexOptions.Compiled` static instances** (lines 13–59)  
> **Fix:** ... use `[GeneratedRegex]` source generators

**Score:** dotnet-perf-skills **4/5**, dotnet-perf-skills-improved **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills-improved** is best for explicitly separating hot-path per-call regex from startup-budget compiled-regex overload with precise severity framing.

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills**:
> #### 4. String `+=` concatenation in loops — O(n²) (11+ sites)  
> #### 9. `.ToLower()`/`.ToUpper()` without culture (25 instances)  
> #### 18. Chain of 48 `Regex.Replace` calls ... 47 intermediate string allocations.

**dotnet-perf-skills-improved**:
> #### 5. O(n²) `+=` string concatenation in loops (7+ files, ~15 sites)  
> #### 6. `.ToLower()`/`.ToUpper()` without culture/ordinal (18 instances)  
> #### 9. Static `Regex.Replace()` ... 15 calls per invocation

**no-skills**:
> 1. 🔴 **String concatenation in loop — O(n²) allocation**  
> 2. 🟡 **Sequential `.Replace()` chain** ... (9 allocations)  
> 2. 🟡 **47-step string replacement chain** ... 47 intermediate string allocations.

**Score:** dotnet-perf-skills **5/5**, dotnet-perf-skills-improved **4/5**, no-skills **5/5**.  
**Verdict:** **tie: dotnet-perf-skills and no-skills**. Both strongly capture `+=`, case-normalization allocations, and chained replacement allocation pressure.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills**:
> #### 5. `ContainsKey` + indexer double-lookup (18 instances)  
> #### 16. Unnecessary `.ToList()` materializations (20 instances)  
> #### 17. `List.Contains` / `allKeys.Contains` — O(n) lookup (3 sites)

**dotnet-perf-skills-improved**:
> #### 7. `ContainsKey` + indexer double-lookup pattern (12 instances)  
> #### 10. `List.Contains()` ... Should use `HashSet<string>`  
> #### 11. `Skip(i).Take(5).ToList()` in loop — O(n²) sliding window

**no-skills**:
> 2. 🟡 **`Keys.ToList()` + `.Contains()` for key union in `Diff`**  
> 5. 🟡 **`Skip(i).Take(5).ToList()` in sliding window**  
> 5. ℹ️ **`.Distinct().ToList()` for tag dedup**

**Score:** dotnet-perf-skills **5/5**, dotnet-perf-skills-improved **5/5**, no-skills **4/5**.  
**Verdict:** **tie: dotnet-perf-skills and dotnet-perf-skills-improved** for breadth and explicit complexity-level guidance.

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills**:
> #### 1. `new HttpClient()` per call — socket exhaustion (3 instances)  
> #### 7. Sequential awaits in loop — no parallelism  
> #### 13. Unbounded parallelism ... #### 14. Missing `CancellationToken`

**dotnet-perf-skills-improved**:
> #### 1. `new HttpClient()` per call — socket exhaustion (3 instances)  
> #### 12. Sequential `await` in loop — no parallelism  
> #### 13. Unbounded parallelism ... #### 14. Missing `CancellationToken` in async methods

**no-skills**:
> 1. 🔴 **`new HttpClient()` per call — socket exhaustion**  
> 2. 🔴 **Sequential awaits in batch loop**  
> 3. 🔴 **Unbounded parallelism** ... 5. 🟡 **`Task.Delay` without `CancellationToken`**

**Score:** dotnet-perf-skills **5/5**, dotnet-perf-skills-improved **5/5**, no-skills **5/5**.  
**Verdict:** **three-way tie**; all outputs cover the full requested async/IO risk set with concrete fixes.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills**:
> #### 3. Uncached `new JsonSerializerOptions` per call (4 instances)  
> #### 15. Uncached reflection — `GetProperties()`/`SetValue()`/`GetValue()` per call (4 sites)  
> **Fix:** Cache `PropertyInfo[]` per type in a `ConcurrentDictionary`

**dotnet-perf-skills-improved**:
> #### 2. Uncached `new JsonSerializerOptions` per call (5 instances)  
> #### 15. Uncached `GetProperties()` / `SetValue()` / `GetValue()` via reflection (4 instances)

**no-skills**:
> 1. 🔴 **Uncached `GetProperties()` reflection per call**  
> 1. 🟡 **`new JsonSerializerOptions` per call** (lines 74, 117, 135, 141)  
> 6. ℹ️ **Full deserialization for `PrettyPrint`** ... use `Utf8JsonWriter` with `JsonDocument`.

**Score:** dotnet-perf-skills **5/5**, dotnet-perf-skills-improved **4/5**, no-skills **5/5**.  
**Verdict:** **tie: dotnet-perf-skills and no-skills**; both hit uncached reflection, per-call serializer options, and broader serialization-path costs.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills**:
> #### 10. Unsealed classes — 17 of 17 (0% sealed)  
> #### 11. Structs without `IEquatable<T>` (2 of 2 structs)  
> #### 12. `static readonly Dictionary<>` — FrozenDictionary candidates (2 instances)

**dotnet-perf-skills-improved**:
> #### 16. Structs without `IEquatable<T>` (2 of 2 structs)  
> #### 17. `static readonly Dictionary<>` — FrozenDictionary candidates (2 instances)  
> #### 18. Unsealed classes — 0 of 17 classes sealed

**no-skills**:
> 6. ℹ️ **Unsealed `ValidationResult` class** (line 23)  
> 6. ℹ️ **Static `Converters` dictionary could be `FrozenDictionary`**  
> ### 6. Unsealed Classes (3 files)

**Score:** dotnet-perf-skills **5/5**, dotnet-perf-skills-improved **5/5**, no-skills **3/5**.  
**Verdict:** **tie: dotnet-perf-skills and dotnet-perf-skills-improved**; baseline detects pieces but is much less systematic on unsealed-class coverage.

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills**:
> | 🔴 Critical | 7 | ... `new Regex` in hot loops ... |  
> #### 5. `ContainsKey` + indexer double-lookup (18 instances)  
> ### 🔴 Critical

**dotnet-perf-skills-improved**:
> **Breakdown:** 🔴 Critical: 5 | 🟡 Moderate: 13 | ℹ️ Info: 5  
> #### 3. `new Regex()` inside per-line parsing loop (hot path)  
> #### 8. `new Regex()` per call in non-loop methods ... Less severe than finding #3

**no-skills**:
> | 🔴 Critical | 8 |  
> 2. 🔴 **Sequential awaits in batch loop**  
> 1. 🟡 **`new JsonSerializerOptions` per call**

**Score:** dotnet-perf-skills **3/5**, dotnet-perf-skills-improved **5/5**, no-skills **2/5**.  
**Verdict:** **dotnet-perf-skills-improved** is clearly best; it distinguishes hot-path vs non-hot-path and avoids inflating moderate findings into critical.

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills**:
> **Fix:** Inject `IHttpClientFactory` ... `PooledConnectionLifetime`  
> **Fix:** Use `[GeneratedRegex]` ...  
> **Fix:** Replace with `StringBuilder` (or `Span<char>`/`string.Create` for advanced cases).

**dotnet-perf-skills-improved**:
> **Fix:** ... use `[GeneratedRegex]` ... **Caveat:** Requires the class to be declared `partial`.  
> **Fix:** Use `StringComparison.OrdinalIgnoreCase` ... or `ToLowerInvariant()`  
> **Fix:** Add `CancellationToken ...`; pass to `Task.Delay` and `HttpClient` calls.

**no-skills**:
> **Fix:** Use `CollectionsMarshal.GetValueRefOrAddDefault` or `TryGetValue`  
> **Fix:** Use `Result<T>` pattern ...  
> **Fix:** Add a `ReadOnlySpan<DeliveryResult>` overload ...

**Score:** dotnet-perf-skills **4/5**, dotnet-perf-skills-improved **5/5**, no-skills **3/5**.  
**Verdict:** **dotnet-perf-skills-improved** has the most consistently actionable and context-safe recommendations with strong API specificity and caveats.

## Weighted Summary

Weights applied: Critical ×3, High ×2, Medium ×1, Low ×0.5.

| Configuration | Critical subtotal (4 dims) | High subtotal (4 dims) | Total weighted score |
|---|---:|---:|---:|
| dotnet-perf-skills | (4+5+5+5)×3 = 57 | (5+5+3+4)×2 = 34 | **91** |
| dotnet-perf-skills-improved | (5+4+5+5)×3 = 57 | (4+5+5+5)×2 = 38 | **95** |
| no-skills | (4+5+4+5)×3 = 54 | (5+3+2+3)×2 = 26 | **80** |

## What All Versions Get Right

- All three detect the highest-impact async/IO risks: per-call `HttpClient`, sequential-await latency, and unbounded parallel sends.
- All three identify regex modernization needs (`new Regex(...)` hot-path costs and migration toward `[GeneratedRegex]`).
- All three call out O(n²) string concatenation patterns and recommend `StringBuilder`.
- All three include concrete fix patterns with .NET API references rather than only generic advice.

## Summary: Impact of Skills

Most impactful differences, ranked:

1. **Severity precision**: `dotnet-perf-skills-improved` best separates truly critical hot-path/incident risks from moderate hygiene issues.
2. **Structural completeness**: both skill-enabled variants systematically quantify sealed/unsealed and `IEquatable<T>` gaps; baseline is patchier.
3. **Recommendation safety and clarity**: improved skills output provides clearer caveats and fewer “over-advanced by default” suggestions.

Overall assessment by weighted score: **dotnet-perf-skills-improved (95) > dotnet-perf-skills (91) > no-skills (80)**. The skills are net-positive, and the improved variant delivers the strongest prioritization signal and most reliable engineering guidance.
