# Comparative Analysis: dotnet-perf-skills, no-skills

This run compares **2 configurations** across **1 scenario**: `analyze-perf-issues` at `output/{config}/run-4/analyze-perf-issues/performance-analysis.md`. Configuration mapping came from `gen-notes.md` where available: `dotnet-perf-skills` explicitly reports the `analyzing-dotnet-performance` skill, while `no-skills` is inferred from the directory name (baseline/default Copilot behavior).

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 5 |
| String Allocation Detection [CRITICAL] | 5 | 5 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 5 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 4 |
| Reflection and Serialization Overhead [HIGH] | 5 | 5 |
| Structural Optimization Detection [HIGH] | 5 | 4 |
| Severity Classification Accuracy [HIGH] | 5 | 3 |
| Fix Recommendation Quality [HIGH] | 5 | 4 |
| Signal-to-Noise / Prioritization Focus [MEDIUM] | 5 | 3 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-4/analyze-perf-issues/performance-analysis.md`):

> **Fix:** Hoist to `private static readonly Regex` fields, or better, use `[GeneratedRegex]` with `partial` methods on .NET 8:  
> [GeneratedRegex(@"\[(\d{4}-\d{2}-\d{2}\s+\d{2}:\d{2}:\d{2}\.\d{3})\]\s+\[(\w+)\]\s+\[(\w+)\]\s+(.+)")]  
> private static partial Regex StructuredLogRegex();

> **Impact:** All 48 compiled regex instances in MarkdownStripper.cs JIT-compile at startup... `[GeneratedRegex]` provides better throughput with near-zero startup cost and supports AOT.

**no-skills** (`output/no-skills/run-4/analyze-perf-issues/performance-analysis.md`):

> 2. **Regex instantiated per line** in `LogAnalyzer.TryParseLine` → O(n) regex compilations for million-line logs  
> 5. **40+ `RegexOptions.Compiled`** instances in `MarkdownStripper` → startup JIT budget blown

> **47 `RegexOptions.Compiled` instances** — each one triggers JIT compilation at first use... migrate to `[GeneratedRegex]` source generators for zero-cost startup.

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**.  
**Verdict:** Tie. Both configurations detect per-call regex creation, excessive compiled regex usage, and recommend `[GeneratedRegex]`.

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills**:

> #### 4. String `+=` Concatenation in Loops — O(n²) Allocation (7 instances)  
> ... CsvParser.ParseLine, and CsvParser.SplitLines all exhibit this pattern.  
> **Fix:** Replace with `StringBuilder`

> #### 11. `.ToLower()/.ToUpper()` Without Culture (12 code sites)  
> **Fix:** Use `StringComparison.OrdinalIgnoreCase` in comparisons, or `ToLowerInvariant()`

**no-skills**:

> **Character-by-character string `+=`** in `ParseLine` — creates a new string on every character... Must use `StringBuilder` or `Span<char>`.

> At least 15 occurrences of `.ToLower()` without specifying `StringComparison.OrdinalIgnoreCase` or using `ToLowerInvariant()`. This is both a correctness issue (Turkish-I) and a performance issue.

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**.  
**Verdict:** Tie. Both identify the high-impact loop concatenation and casing-allocation issues with concrete remediation.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills**:

> #### 6. `ContainsKey` + Indexer Double-Lookup (10 instances)  
> **Impact:** ~2x slower per dictionary access...  
> **Fix:** Replace with `TryGetValue`

> #### 9. Multiple Iterations Over Same Collection... `Skip(i).Take(5).ToList()` inside a loop — O(n²) allocation pattern.

**no-skills**:

> `.Keys.ToList()` + `.Contains(key)` for key aggregation in `Diff` — O(n) lookups. Should use `HashSet<string>` for the union of keys.

> `.Skip(i).Take(5).ToList()` inside a loop — allocates a new list on every sliding-window step. Use index-based access instead.

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**.  
**Verdict:** Tie. Both outputs cover the key collection/LINQ inefficiencies called out in the rubric.

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills**:

> #### 1. `new HttpClient()` Per Call — Socket Exhaustion (3 instances)  
> **Fix:** Inject `IHttpClientFactory` or use a `static readonly HttpClient`

> #### 15. Sequential Async in Loop — No Parallelism ...  
> #### 16. Unbounded Parallelism ...  
> #### 17. Missing Cancellation Tokens in Async Methods

**no-skills**:

> **`new HttpClient()` per call** ... eventually exhausting sockets. Must use `IHttpClientFactory` or a shared static instance.

> Sequential `await` in a loop — each notification blocks until the previous completes.  
> **Unbounded parallelism** in `SendBatchParallelAsync` — fires all tasks at once with no throttling.

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is stronger due to explicit cancellation-token propagation coverage in addition to the core HttpClient and parallelism findings.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills**:

> #### 2. Uncached `new JsonSerializerOptions` Per Call (4 instances)  
> **Fix:** Extract to a `private static readonly JsonSerializerOptions`

> #### 7. Uncached Reflection in Hot Paths (6 instances)  
> **Fix:** Cache `PropertyInfo[]` per type in a `static ConcurrentDictionary`

**no-skills**:

> `new JsonSerializerOptions { WriteIndented = true }` in `Merge` — allocated per call. Should be a `static readonly` field.

> **`typeof(TTarget).GetProperties()` called on every `MapTo<T>` invocation** ... Must cache `PropertyInfo[]` per type.

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**.  
**Verdict:** Tie. Both clearly identify the reflection and serializer-option hot spots and propose correct fixes.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills**:

> #### 12. Unsealed Classes — 0 of 17 Sealed  
> #### 13. Structs Without `IEquatable<T>` — 0 of 2  
> #### 14. `static readonly Dictionary<>` — FrozenDictionary Candidates (2 instances)

**no-skills**:

> `Record`, `MappingConfig`, and `ValidationResult` are non-sealed non-base classes...

> `DeliveryResult` is a `struct` without `IEquatable<DeliveryResult>`...  
> `static readonly Dictionary<string, string>`... candidate for `FrozenDictionary` on .NET 8+

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is more systematic (explicit global counts and inverse checks), while no-skills catches representative cases.

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills**:

> | 🔴 Critical | 9 | `new HttpClient()` per call ..., per-call `new Regex()` in hot loops |  
> | 🟡 Moderate | 10 | 48 `RegexOptions.Compiled` without `[GeneratedRegex]`, 17 unsealed classes... |

> **Top priorities:** Fix `new HttpClient()`..., cache `JsonSerializerOptions`..., hoist `new Regex()`..., replace `+=` loops...

**no-skills**:

> | 🔴 Critical | 8 | >10× regression potential or production incident risk |  
> | 🟡 Moderate | 28 | 2–10× regression, measurable impact at scale |  
> | ℹ️ Info | 26 | Best-practice improvements, minor allocation savings |

**Score:** dotnet-perf-skills **5/5**, no-skills **3/5**.  
**Verdict:** **dotnet-perf-skills** better separates hot-path/incident risks from lower-impact items; no-skills is thorough but noisier, with diluted prioritization.

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills**:

> **Fix:** Inject `IHttpClientFactory` or use a `static readonly HttpClient` with `SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(5) }`.

> **Caveat:** ValidationEngine.AddPattern takes a user-supplied pattern string — cannot use `[GeneratedRegex]`. Cache with `ConcurrentDictionary<string, Regex>` instead.

**no-skills**:

> // After — use [GeneratedRegex] (.NET 8+ source generator) or static readonly fields  
> [GeneratedRegex(@"\{\{#if\s+(\w+)\}\}([\s\S]*?)\{\{/if\}\}")]

> Replace `new HttpClient()` with `IHttpClientFactory` or static shared client...

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is more consistently precise and constraint-aware (explicit caveats, tighter API targeting).

## 9. Signal-to-Noise / Prioritization Focus [MEDIUM]

**dotnet-perf-skills**:

> | 🔴 Critical | 9 | ... |  
> | 🟡 Moderate | 10 | ... |  
> | ℹ️ Info | 4 | ...

**no-skills**:

> This analysis ... identified **62 performance findings** ...  
> | 🔴 Critical | 8 | ... |  
> | 🟡 Moderate | 28 | ... |  
> | ℹ️ Info | 26 | ...

**Score:** dotnet-perf-skills **5/5**, no-skills **3/5**.  
**Verdict:** **dotnet-perf-skills** has higher decision utility per page; no-skills is comprehensive but significantly noisier.

## Weighted Summary

Weights used: **Critical ×3**, **High ×2**, **Medium ×1**, **Low ×0.5**.

| Configuration | Critical subtotal | High subtotal | Medium subtotal | Total weighted score |
|---|---:|---:|---:|---:|
| dotnet-perf-skills | (5+5+5+5)×3 = 60 | (5+5+5+5)×2 = 40 | 5×1 = 5 | **105** |
| no-skills | (5+5+5+4)×3 = 57 | (5+4+3+4)×2 = 32 | 3×1 = 3 | **92** |

## What All Versions Get Right

- Both identify the top production-risk issues: **`new HttpClient()` per call**, **per-call regex in log parsing**, and **O(n²) string concatenation patterns**.
- Both call out **`RegexOptions.Compiled` overuse in MarkdownStripper** and recommend **`[GeneratedRegex]`** for .NET 8.
- Both detect **collection hot-path inefficiencies** (`ContainsKey`+indexer, `List.Contains` in loops, avoidable `.ToList()` allocations).
- Both include actionable .NET-specific fixes (`IHttpClientFactory`, `StringBuilder`, `StringComparison.OrdinalIgnoreCase`, reflection/serializer caching).

## Summary: Impact of Skills

Most impactful differences:
1. **Prioritization quality**: skills output is more focused, with cleaner severity stratification and less noise.
2. **Structural/systematic coverage**: skills output quantifies global gaps (e.g., **0/17 sealed**, **0/2 IEquatable**) rather than isolated examples.
3. **Recommendation precision**: skills output adds stronger caveats and safer API guidance in edge cases.

Overall assessment from weighted scoring: **dotnet-perf-skills (105)** outperforms **no-skills (92)**. The baseline remains strong on raw issue detection, but the skills configuration provides better triage value and implementation-ready guidance.
