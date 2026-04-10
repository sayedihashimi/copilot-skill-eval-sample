# Comparative Analysis: dotnet-perf-skills, no-skills

This report compares **2 configurations** across **1 scenario** (`analyze-perf-issues`) using outputs from `output/{config}/run-2/analyze-perf-issues/`. Configuration identity came from each scenario’s `gen-notes.md`: `dotnet-perf-skills` explicitly used the `analyzing-dotnet-performance` skill, while `no-skills` is baseline Copilot output.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 4 |
| String Allocation Detection [CRITICAL] | 5 | 4 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 4 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 4 |
| Reflection and Serialization Overhead [HIGH] | 5 | 4 |
| Structural Optimization Detection [HIGH] | 5 | 3 |
| Severity Classification Accuracy [HIGH] | 4 | 3 |
| Fix Recommendation Quality [HIGH] | 5 | 4 |
| Quantification & Traceability [MEDIUM] | 5 | 3 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-2/analyze-perf-issues/performance-analysis.md`)
> | `new Regex(` uncached | 8 | LogAnalyzer×4, TemplateEngine×2, ValidationEngine×1, CsvParser×1 |  
> | `RegexOptions.Compiled` | 48 | All in MarkdownStripper.cs |  
> | `[GeneratedRegex]` | 0 | None in codebase |  
> **Fix:** Hoist to `private static readonly Regex` fields or use `[GeneratedRegex]` on .NET 7+.

**no-skills** (`output/no-skills/run-2/analyze-perf-issues/performance-analysis.md`)
> | 16 | 🟡 Moderate | 13-59 | Regex | **46 `RegexOptions.Compiled` instances** ... should use `[GeneratedRegex]` ... |  
> | 35 | 🔴 Critical | 50-51 | Regex | `new Regex(...)` **on every log line** in `TryParseLine` ... |  
> The fix is consistent: promote to `static readonly` fields, or better on .NET 8+, use `[GeneratedRegex]` source generators.

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is stronger due to explicit scan counts, broader regex census, and clearer startup-budget framing.

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills**
> #### 4. O(n²) string concatenation (`+=`) in loops (8 sites)  
> **Files:** ... `CsvParser.cs:L51 (ParseLine char-by-char), L89 (SplitLines char-by-char)` ...  
> #### 15. Chained `Regex.Replace()` / `.Replace()` in SlugGenerator (12+9 = 21 calls)  
> **Impact:** ... 21 intermediate string allocations per invocation.

**no-skills**
> | 21 | 🔴 Critical | 51-78 | String | Character-by-character `current += line[i]` in `ParseLine` — O(n²) ... |  
> | 22 | 🔴 Critical | 89-107 | String | Same `currentLine += content[i]` in `SplitLines` — O(n²) ... |  
> `.ToLower()`/`.ToUpper()` Without Culture ... both a correctness bug (Turkish-I) and a performance issue.

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is more complete by quantifying compound allocation chains and cross-method allocation patterns.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills**
> #### 5. `ContainsKey` + indexer double-lookup (12 instances)  
> #### 16. `List.Contains()` O(n) in a loop (1 instance)  
> #### 17. LINQ `Skip(i).Take(5).ToList()` in a loop — O(n²) (1 instance)

**no-skills**
> | 14 | 🔴 Critical | 75-85 | Collections | ... `existing.Contains(baseSlug)` is O(n) per check inside a `while` loop ... |  
> | 32 | 🔴 Critical | 85-89 | Collections | `flat1.Keys.ToList()` then `.Contains(key)` ... O(n·m) ... |  
> `ContainsKey` + indexer Anti-Pattern ... performs two lookups. Use `TryGetValue`.

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** wins on breadth and explicit hot-path complexity framing, though both detect the key collection risks.

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills**
> #### 1. `new HttpClient()` per call — socket exhaustion (3 instances)  
> #### 13. Sequential awaits in loop (1 instance)  
> #### 14. Unbounded parallelism (1 instance)  
> #### 18. Missing `CancellationToken` on async methods

**no-skills**
> | 49 | 🔴 Critical | 163, 179, 192 | Async/IO | `new HttpClient()` ... socket exhaustion ... |  
> | 50 | 🟡 Moderate | 117 | Async/IO | `SendBatchAsync` awaits sequentially ... |  
> | 51 | 🟡 Moderate | 132 | Async/IO | `SendBatchParallelAsync` fires **all** tasks unbounded ... |

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is better because it also calls out missing cancellation-token propagation as a first-class issue.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills**
> #### 2. Uncached `new JsonSerializerOptions` per call (5 instances)  
> **Impact:** Up to 592× slower in .NET 6+ ...  
> #### 6. Uncached reflection in hot paths (6 call sites)

**no-skills**
> | 31 | 🟡 Moderate | 74 | Serialization | `new JsonSerializerOptions` ... should be a `static readonly` field. |  
> | 65 | 🟡 Moderate | 77 | Reflection | `typeof(TTarget).GetProperties()` on every `MapTo<T>()` call ... cache per type ... |

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** provides stronger impact quantification and clearer prioritization.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills**
> | `sealed class` | 0 | None sealed |  
> | Unsealed non-abstract, non-static classes | 18 | 0 of 18 sealed |  
> | `: IEquatable` | 0 | None |  
> | `public struct` | 2 | Both missing IEquatable |

**no-skills**
> | 44 | ℹ️ Info | 10 | Structural | `Record` class is not `sealed` ... |  
> | 53 | ℹ️ Info | 25-49 | Structural | `DeliveryResult` struct does not implement `IEquatable<DeliveryResult>` ... |  
> | 60 | ℹ️ Info | 23 | Structural | `ValidationResult` is unsealed. |

**Score:** dotnet-perf-skills **5/5**, no-skills **3/5**.  
**Verdict:** **dotnet-perf-skills** is clearly better; it treats structural optimization as a systematic codebase-level pattern, not isolated notes.

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills**
> **Top priorities:** Socket exhaustion from `new HttpClient()` ... per-line regex instantiation ... O(n²) string concatenation ...  
> Severity | Count  
> 🔴 Critical | 6  
> 🟡 Moderate | 12

**no-skills**
> **Top priorities:** ... `new HttpClient()` ... regex in `LogAnalyzer.TryParseLine()` ... `string +=` loops ...  
> - 🔴 **Critical**: 7 ...  
> - 🟡 **Moderate**: 22 ...  
> - ℹ️ **Info**: 19 ...

**Score:** dotnet-perf-skills **4/5**, no-skills **3/5**.  
**Verdict:** **dotnet-perf-skills** is better overall on hot-path prioritization, but both have some tiering inconsistencies on medium-impact collection patterns.

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills**
> **Fix:** Inject `IHttpClientFactory` or use a `static readonly HttpClient` with `PooledConnectionLifetime`.  
> **Fix:** ... use `[GeneratedRegex]` on .NET 7+.  
> **Fix:** ... `StringComparison.OrdinalIgnoreCase` ... `FrozenDictionary` ... `Parallel.ForEachAsync`.

**no-skills**
> **Fix recommendations:**  
> // After: static field or [GeneratedRegex] on .NET 8+  
> // After: HashSet for O(1) lookup  
> // After: cache per type

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is more specific and framework-aware across categories; baseline is still solid and actionable.

## 9. Quantification & Traceability [MEDIUM]

**dotnet-perf-skills**
> ## Scan Execution Checklist  
> | Recipe | Hits | Notes |  
> | `new Regex(` uncached | 8 | ... |  
> | `.ToList()` materializations | 17 | 7 files |

**no-skills**
> ## Findings by File  
> | # | Severity | Line(s) | Category | Description |  
> ... (file-by-file findings with line ranges and examples)

**Score:** dotnet-perf-skills **5/5**, no-skills **3/5**.  
**Verdict:** **dotnet-perf-skills** provides better auditability via explicit recipe-hit accounting and inverse checks.

## Weighted Summary

Weights used: Critical ×3, High ×2, Medium ×1, Low ×0.5.

| Dimension | Tier | Weight | dotnet-perf-skills | no-skills |
|---|---|---:|---:|---:|
| Regex Anti-Pattern Detection | CRITICAL | 3 | 15 | 12 |
| String Allocation Detection | CRITICAL | 3 | 15 | 12 |
| Collection and LINQ Efficiency | CRITICAL | 3 | 15 | 12 |
| Async and IO Pattern Detection | CRITICAL | 3 | 15 | 12 |
| Reflection and Serialization Overhead | HIGH | 2 | 10 | 8 |
| Structural Optimization Detection | HIGH | 2 | 10 | 6 |
| Severity Classification Accuracy | HIGH | 2 | 8 | 6 |
| Fix Recommendation Quality | HIGH | 2 | 10 | 8 |
| Quantification & Traceability | MEDIUM | 1 | 5 | 3 |
| **Total Weighted Score** |  |  | **103** | **79** |

## What All Versions Get Right

- Both identify the top production-risk async issue: per-call `new HttpClient()`.
- Both flag hot-path regex construction in `LogAnalyzer.TryParseLine`.
- Both catch O(n²) string concatenation in loop-heavy paths.
- Both provide concrete, API-level remediations (e.g., `StringBuilder`, `HashSet`, regex caching).
- Both include useful line-level references for implementation follow-up.

## Summary: Impact of Skills

The skill-enabled output is best overall because it materially improves **coverage depth**, **scan quantification**, and **cross-category prioritization** while keeping recommendations concrete and mostly correct. The largest impact deltas are: (1) systematic regex accounting (`Compiled` + uncached + `[GeneratedRegex]` gap), (2) stronger structural analysis (0/18 sealed, 0/2 `IEquatable<T>`), and (3) better traceable evidence via recipe hit tables.

Overall ranking by weighted score: **1) dotnet-perf-skills (103)**, **2) no-skills (79)**.
