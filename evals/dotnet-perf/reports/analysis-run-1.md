# Comparative Analysis: dotnet-diag-auto-improve

This run contains **1 configuration directory**: `output/dotnet-diag-auto-improve/run-1/`. It includes the expected scenario `analyze-perf-issues/`, and configuration evidence is provided in `performance-analysis.md` and `gen-notes.md` (skill: `analyzing-dotnet-performance`). Because only one configuration is present, scoring is absolute against the rubric rather than relative across multiple variants.

## Executive Summary

| Dimension [Tier] | dotnet-diag-auto-improve |
|---|---|
| Regex Anti-Pattern Detection [CRITICAL] | 5 |
| String Allocation Detection [CRITICAL] | 5 |
| Collection and LINQ Efficiency [CRITICAL] | 5 |
| Async and IO Pattern Detection [CRITICAL] | 5 |
| Reflection and Serialization Overhead [HIGH] | 5 |
| Structural Optimization Detection [HIGH] | 5 |
| Aggregate and Replace Chain Detection [HIGH] | 5 |
| Span Usage Consistency [HIGH] | 5 |
| Inheritance Sealing Accuracy [HIGH] | 5 |
| Params Overload Optimization [MODERATE] | 5 |
| Severity Classification Accuracy [HIGH] | 4 |
| Fix Recommendation Quality [HIGH] | 5 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-diag-auto-improve** (`output/dotnet-diag-auto-improve/run-1/analyze-perf-issues/performance-analysis.md`):
> | `RegexOptions.Compiled` | 48 (all in MarkdownStripper) |
> | `[GeneratedRegex]` | 0 |
> | `new Regex(` (uncached, per-call) | 8 (...) |
> **Fix:** Hoist to `private static readonly Regex` fields or use `[GeneratedRegex]` (preferred on .NET 8).

**Score:** dotnet-diag-auto-improve = **5/5** (detects per-call instantiation, compiled overuse, and gives .NET 8+ GeneratedRegex guidance).  
**Verdict:** **dotnet-diag-auto-improve** is comprehensive and precise.

## 2. String Allocation Detection [CRITICAL]

**dotnet-diag-auto-improve**:
> #### 6. String Concatenation `+=` in Loops — O(n²) Allocation (7 sites)
> #### 9. `.ToLower()`/`.ToUpper()` Without Culture or `StringComparison` (17 instances)
> #### 12. Sequential `.Replace()` Chain in SlugGenerator (...)
> **Fix:** Use `StringBuilder` ... `StringComparison.OrdinalIgnoreCase` ... `ToLowerInvariant()`.

**Score:** dotnet-diag-auto-improve = **5/5** (covers all requested string categories with counts and fixes).  
**Verdict:** **dotnet-diag-auto-improve** clearly addresses high-impact string allocation issues.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-diag-auto-improve**:
> #### 7. `ContainsKey` + Indexer Double-Lookup (10+ sites)
> #### 16. `.ToList()` + `.Contains()` for Key Lookups — O(n²) (2 instances)
> #### 17. `Skip(i).Take(5).ToList()` in Loop — Sliding Window Allocation (1 instance)
> #### 18. `Distinct().ToList()` Allocation (1 instance)

**Score:** dotnet-diag-auto-improve = **5/5** (captures the full LINQ/collection hot-path set, including explicit Distinct/Skip+Take counts).  
**Verdict:** **dotnet-diag-auto-improve** provides excellent collection/LINQ coverage.

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-diag-auto-improve**:
> #### 1. `new HttpClient()` Per Call — Socket Exhaustion (3 instances)
> #### 19. Sequential `await` in Loop — No Parallelism
> #### 20. Unbounded Parallelism in `SendBatchParallelAsync`
> #### 21. Missing Cancellation Tokens in Async Methods

**Score:** dotnet-diag-auto-improve = **5/5** (identifies all required async/IO anti-patterns and includes mitigation patterns).  
**Verdict:** **dotnet-diag-auto-improve** is strong and production-relevant in async/IO analysis.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-diag-auto-improve**:
> #### 2. Uncached `new JsonSerializerOptions` Per Call (5 instances)
> #### 8. Uncached Reflection — `GetProperties()`/`GetProperty()`/`SetValue()` Per Call
> - Full `Deserialize*` hot-path hits: **4**
> - `Utf8JsonReader`/`JsonDocument` usage sites: **0** — partial parsing ... is a valid optimization

**Score:** dotnet-diag-auto-improve = **5/5** (matches all reflection/serialization targets, including partial-parse guidance).  
**Verdict:** **dotnet-diag-auto-improve** is complete and actionable here.

## 6. Structural Optimization Detection [HIGH]

**dotnet-diag-auto-improve**:
> #### 23. Unsealed Leaf Classes — 0 of 26 Non-Abstract Classes Are Sealed
> #### 24. Structs Without `IEquatable<T>` — 0 of 3 Structs Implement It
> #### 27. `static readonly Dictionary<>` — FrozenDictionary Candidates (3 instances)

**Score:** dotnet-diag-auto-improve = **5/5** (covers sealing, struct equality, and FrozenDictionary opportunities).  
**Verdict:** **dotnet-diag-auto-improve** provides strong structural optimization detection.

## 7. Aggregate and Replace Chain Detection [HIGH]

**dotnet-diag-auto-improve**:
> #### 11. `.Aggregate()` with `.Replace()` — 16 Intermediate String Allocations
> #### 22. `char.ToString()` Allocation in Loop
> **Files:** UnitFormatter.cs:L60-L66 ... UnitFormatter.cs:L64

**Score:** dotnet-diag-auto-improve = **5/5** (explicitly catches both aggregate-replace chaining and per-iteration `char.ToString()` allocations).  
**Verdict:** **dotnet-diag-auto-improve** handles this subtle pattern very well.

## 8. Span Usage Consistency [HIGH]

**dotnet-diag-auto-improve**:
> #### 14. `value[..n].TrimEnd()` Double Allocation
> #### 15. Cross-File Inconsistency: `Substring` vs `AsSpan` in Truncators
> #### 13. `List<char>` Where `ReadOnlySpan<char>` or String Would Suffice

**Score:** dotnet-diag-auto-improve = **5/5** (detects double-allocation, inconsistent Span adoption, and static char-set storage inefficiency).  
**Verdict:** **dotnet-diag-auto-improve** is comprehensive on Span-related consistency issues.

## 9. Inheritance Sealing Accuracy [HIGH]

**dotnet-diag-auto-improve**:
> **Fix:** Add `sealed` keyword to all leaf classes. Leave `Ordinalizer` and `DefaultOrdinalizer` unsealed (they are base classes).
> - Did **not** suggest sealing `Ordinalizer` or `DefaultOrdinalizer` (they are base classes with subclasses)

**Score:** dotnet-diag-auto-improve = **5/5** (correctly identifies leaf classes while avoiding base-class false positives).  
**Verdict:** **dotnet-diag-auto-improve** shows high precision in inheritance-aware sealing advice.

## 10. Params Overload Optimization [MODERATE]

**dotnet-diag-auto-improve**:
> #### 26. `params` Without Single-Argument Fast-Path Overloads (3 methods)
> **Impact:** Always allocates a `params` array even for the common 1-argument case.
> **Fix:** Add 1-argument and 2-argument overloads ...

**Score:** dotnet-diag-auto-improve = **5/5** (finds exactly the expected params allocation pattern and recommends the standard fix).  
**Verdict:** **dotnet-diag-auto-improve** fully satisfies this dimension.

## 11. Severity Classification Accuracy [HIGH]

**dotnet-diag-auto-improve**:
> | 🔴 Critical | 8 | `new HttpClient()` per call ... per-line `new Regex()` ... uncached `JsonSerializerOptions` |
> | 🟡 Moderate | 14 | `+=` ... `RegexOptions.Compiled` ... `ContainsKey` + indexer ... |
> | ℹ️ Info | 8 | Missing `sealed` ... `List` without capacity hints ... `params` ... |

**Score:** dotnet-diag-auto-improve = **4/5** (mostly well-prioritized with strong hot-path emphasis; minor over-severity risk where `ContainsKey+indexer` is promoted to critical in some contexts).  
**Verdict:** **dotnet-diag-auto-improve** is strong overall, with small prioritization calibration room.

## 12. Fix Recommendation Quality [HIGH]

**dotnet-diag-auto-improve**:
> **Fix:** Inject `IHttpClientFactory` or use a single `static readonly HttpClient`...
> **Fix:** ... use `[GeneratedRegex]` ...
> **Fix:** ... `StringComparison.OrdinalIgnoreCase` ...
> **Fix:** ... `.ToFrozenDictionary()` ... `using System.Collections.Frozen;`

**Score:** dotnet-diag-auto-improve = **5/5** (specific APIs, concrete patterns, and no unsafe/incorrect recommendations).  
**Verdict:** **dotnet-diag-auto-improve** delivers highly actionable, technically correct fixes.

## Weighted Summary

Weights used: Critical ×3, High ×2, Moderate ×1, Low ×0.5.

| Configuration | Critical subtotal | High subtotal | Moderate subtotal | Total weighted score |
|---|---:|---:|---:|---:|
| dotnet-diag-auto-improve | 60 | 68 | 5 | **133** |

## What All Versions Get Right

- With only one discovered configuration in this run, there are no cross-version commonalities to compare.
- The available output consistently provides exact hit counts, file/line evidence, and concrete fix directions.

## Summary: Impact of Skills

Ranked by impact, the strongest differentiators in this output are: **(1)** broad critical-pattern coverage with exact counts, **(2)** high-quality API-level remediation guidance, and **(3)** precision safeguards that avoid key false positives (not sealing base classes, not misusing GeneratedRegex on dynamic patterns).  
Overall, `dotnet-diag-auto-improve` performs at an **excellent** level (133 weighted), with only minor severity-ranking tuning needed.
