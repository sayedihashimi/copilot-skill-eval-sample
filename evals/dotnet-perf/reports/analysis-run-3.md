# Comparative Analysis: dotnet-perf-skills, no-skills

This run compares **2 configurations** across **1 scenario**: `analyze-perf-issues` at `output/{config}/run-3/analyze-perf-issues/performance-analysis.md`. Configuration identity was confirmed from scenario-level `gen-notes.md`: `dotnet-perf-skills` explicitly lists the `analyzing-dotnet-performance` skill, while `no-skills` contains generic notes without skill references.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 4 |
| String Allocation Detection [CRITICAL] | 5 | 5 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 5 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 5 |
| Reflection and Serialization Overhead [HIGH] | 4 | 4 |
| Structural Optimization Detection [HIGH] | 5 | 4 |
| Severity Classification Accuracy [HIGH] | 5 | 3 |
| Fix Recommendation Quality [HIGH] | 5 | 4 |
| Signal-to-Noise / Report Focus [MEDIUM] | 5 | 2 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills excerpt**
> **Top priorities:** ... per-call `new Regex()` in line-by-line parsing ... and 48 `RegexOptions.Compiled` instances that should use `[GeneratedRegex]` on .NET 8.  
> #### 3. `new Regex()` in per-line hot path (8 instances)  
> **Impact:** `TryParseLine` in LogAnalyzer is called per log line — potentially millions of times.

**no-skills excerpt**
> 2. **Regex per-line allocation** in `LogAnalyzer.TryParseLine` — creates 2-3 `Regex` objects per log line in a hot parse loop  
> 5. **40+ `RegexOptions.Compiled`** static fields in `MarkdownStripper` — excessive JIT startup cost  
> | 31 | 🟡 Moderate | 13-59 | **46 `RegexOptions.Compiled` static fields** ... should use `[GeneratedRegex]` |

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best because it is complete and more consistent (counts and severity framing are tighter, including explicit caveat for dynamic patterns).

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills excerpt**
> #### 7. String `+=` concatenation in loops — O(n²) allocation (7 sites)  
> #### 8. `.ToLower()`/`.ToUpper()` without culture (19 instances)  
> **Fix:** Use `StringComparison.OrdinalIgnoreCase` comparisons or `ToLowerInvariant()`.

**no-skills excerpt**
> 3. **O(n²) string concatenation** — found in 8+ methods across all files (`+=` in loops)  
> | 39 | 🔴 Critical | 51-77 | **Character-by-character string `+=` in `ParseLine`** — O(n²) ...  
> | 27 | 🟡 Moderate | 33-36 | **Sequential `.Replace()` chain** — 9 string allocations per call |

**Score:** dotnet-perf-skills **5/5**; no-skills **5/5**.  
**Verdict:** **Tie**. Both catch loop concatenation, casing allocations/correctness, and replace-chain allocation patterns.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills excerpt**
> #### 6. `ContainsKey` + indexer double-lookup (10 instances)  
> #### 12. `List.Contains()` O(n) lookups in loops (2 sites)  
> ... `Skip(i).Take(5).ToList()` inside a loop creates a new list per iteration.

**no-skills excerpt**
> | 28 | 🟡 Moderate | 75-81 | **`.ToList()` + `.Contains()` ... Should use `HashSet<string>`.**  
> | 35 | 🟡 Moderate | 85-89 | **`.ToList()` + `.Contains()` for key deduplication** ... O(n²)  
> | 61 | 🟡 Moderate | 157 | **`Skip(i).Take(5).ToList()`** in a loop — O(n²) sliding window ...

**Score:** dotnet-perf-skills **5/5**; no-skills **5/5**.  
**Verdict:** **Tie**. Both cover the requested high-impact collection/LINQ anti-patterns with concrete alternatives (`TryGetValue`, `HashSet`, avoid materialization).

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills excerpt**
> #### 1. `new HttpClient()` per call — socket exhaustion (3 instances)  
> #### 13. Sequential awaits in loop — no parallelism (1 site)  
> #### 14. Unbounded parallelism — all tasks fired at once (1 site)

**no-skills excerpt**
> | 45 | 🔴 Critical | 163, 179, 191 | **`new HttpClient()` per call** ... causes socket exhaustion under load. |  
> | 46 | 🔴 Critical | 117 | **Sequential `await` in loop** in `SendBatchAsync` ... |  
> | 48 | 🟡 Moderate | 102 | **`Task.Delay` without `CancellationToken`** ... |

**Score:** dotnet-perf-skills **5/5**; no-skills **5/5**.  
**Verdict:** **Tie**. Both identify all required async/IO risks and actionable mitigation patterns.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills excerpt**
> #### 2. Uncached `new JsonSerializerOptions` per call (5 instances)  
> #### 16. Uncached reflection `GetProperties()`/`SetValue()`/`GetValue()` (6 instances)  
> **Fix:** Cache `PropertyInfo[]` per type in a `ConcurrentDictionary<...>`.

**no-skills excerpt**
> | 34 | 🟡 Moderate | 74, 117, 135, 142 | **`new JsonSerializerOptions` per call** ... |  
> | 7 | 🔴 Critical | 77 | **`GetProperties()` via reflection on every `MapTo<T>` call** ... |  
> | 8 | 🔴 Critical | 101 | **`prop.SetValue()` via reflection per property** ... |

**Score:** dotnet-perf-skills **4/5**; no-skills **4/5**.  
**Verdict:** **Tie**. Both are strong on reflection and serializer-options caching; neither goes deeper into partial parsing (`Utf8JsonReader`) opportunities.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills excerpt**
> #### 9. Unsealed classes — 18 of 18 classes (0 sealed)  
> #### 10. Structs without `IEquatable<T>` (2 instances)  
> #### 11. Static `Dictionary` — FrozenDictionary candidates (2 of 2, 0 optimized)

**no-skills excerpt**
> | 5 | ℹ️ Info | 24 | **Unsealed class `ValidationResult`** ... |  
> | 4 | ℹ️ Info | 11-20 | **Struct `ValidationError` without `IEquatable<T>`** ... |  
> | 13 | ℹ️ Info | 10 | **Static `Dictionary` could be `FrozenDictionary`** ... |

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is better due to systematic aggregation and stronger structural prioritization across the codebase.

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills excerpt**
> | 🔴 Critical | 8 | ... `new HttpClient()` per call ... per-call `new Regex()` in hot loops |  
> | 🟡 Moderate | 10 | 18/18 classes unsealed, uncached reflection, unbounded parallelism ... |  
> | Rank | Finding | Severity | Effort | Impact |

**no-skills excerpt**
> **Total issues found: 68**  
> | 🔴 Critical | 8 |  
> | 🟡 Moderate | 30 |  
> | ℹ️ Info | 30 |

**Score:** dotnet-perf-skills **5/5**; no-skills **3/5**.  
**Verdict:** **dotnet-perf-skills** is best: hot-path issues are clearly elevated and prioritization is cleaner. `no-skills` dilutes signal with many low-impact findings and less consistent severity pressure (e.g., very large `RegexOptions.Compiled` cluster kept moderate).

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills excerpt**
> **Fix:** Inject `IHttpClientFactory` or use a `static readonly HttpClient` with `PooledConnectionLifetime`.  
> **Fix:** Convert all to `[GeneratedRegex]` with `partial` class.  
> **Caveat:** ... dynamic pattern ... cannot use `[GeneratedRegex]`.

**no-skills excerpt**
> // Issue 45: Use IHttpClientFactory or a single static instance  
> // Issue 35: Use HashSet for key deduplication  
> | 8 | **Replace `.ToLower()`/`.ToUpper()` with `StringComparison.OrdinalIgnoreCase`** ... |

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** provides more consistently high-quality recommendations, especially where caveats and API choices matter.

## 9. Signal-to-Noise / Report Focus [MEDIUM]

**dotnet-perf-skills excerpt**
> **Total issues found: 22 findings** across 10 files spanning all 8 scanned categories.  
> **Top priorities:** ...  
> ## Prioritized Fix Recommendations

**no-skills excerpt**
> **Total issues found: 68**  
> ## Findings by File  
> ... (70 numbered findings with many low-impact entries)

**Score:** dotnet-perf-skills **5/5**; no-skills **2/5**.  
**Verdict:** **dotnet-perf-skills** is substantially more consumable for engineering prioritization; `no-skills` is exhaustive but noisy.

## Weighted Summary

Weights used: Critical ×3, High ×2, Medium ×1.

| Dimension | Tier | dotnet-perf-skills | no-skills |
|---|---|---:|---:|
| Regex Anti-Pattern Detection | Critical | 15 | 12 |
| String Allocation Detection | Critical | 15 | 15 |
| Collection and LINQ Efficiency | Critical | 15 | 15 |
| Async and IO Pattern Detection | Critical | 15 | 15 |
| Reflection and Serialization Overhead | High | 8 | 8 |
| Structural Optimization Detection | High | 10 | 8 |
| Severity Classification Accuracy | High | 10 | 6 |
| Fix Recommendation Quality | High | 10 | 8 |
| Signal-to-Noise / Report Focus | Medium | 5 | 2 |
| **Total Weighted Score** |  | **103** | **89** |

## What All Versions Get Right

- Both detect the highest-risk production issue: `new HttpClient()` per call.
- Both identify hot-path regex construction in `LogAnalyzer.TryParseLine`.
- Both call out string `+=` loop allocation problems and recommend `StringBuilder`.
- Both flag collection hot-path inefficiencies (`ContainsKey`+indexer, `List.Contains`, `.ToList()` overuse).
- Both include concrete .NET-centric fixes (`IHttpClientFactory`, `[GeneratedRegex]`, `TryGetValue`, `HashSet`).

## Summary: Impact of Skills

Most impactful differences, in order: **(1)** better severity/prioritization signal, **(2)** tighter regex modernization framing (`GeneratedRegex` + startup budget), **(3)** much better report focus and triage usability.  
Overall, `dotnet-perf-skills` is the stronger output for practical remediation planning (**103 vs 89 weighted**). `no-skills` still surfaces most core issues, but with more noise and weaker prioritization fidelity.
