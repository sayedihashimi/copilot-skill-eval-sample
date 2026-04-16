# Comparative Analysis: dotnet-perf-skills, dotnet-perf-skills-improved, no-skills

This run compares **3 configurations** on **1 shared scenario**: `analyze-perf-issues` in `output/{config}/run-2/analyze-perf-issues/performance-analysis.md`. Configuration identity was taken from `gen-notes.md` where present and otherwise inferred from folder name: `dotnet-perf-skills` (Performance Skills), `dotnet-perf-skills-improved` (Performance Skills improved), and `no-skills` (baseline/default Copilot behavior).

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | dotnet-perf-skills-improved | no-skills |
|---|---:|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 5 | 4 |
| String Allocation Detection [CRITICAL] | 5 | 5 | 5 |
| Collection and LINQ Efficiency [CRITICAL] | 4 | 5 | 4 |
| Async and IO Pattern Detection [CRITICAL] | 4 | 5 | 4 |
| Reflection and Serialization Overhead [HIGH] | 4 | 4 | 4 |
| Structural Optimization Detection [HIGH] | 4 | 5 | 3 |
| Severity Classification Accuracy [HIGH] | 5 | 3 | 3 |
| Fix Recommendation Quality [HIGH] | 5 | 4 | 4 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

`dotnet-perf-skills` covers per-call regex, compiled-regex startup budget, and `[GeneratedRegex]` migration very explicitly.

**Excerpt — dotnet-perf-skills (`output/dotnet-perf-skills/run-2/analyze-perf-issues/performance-analysis.md`)**
> #### 2. Uncached `new Regex()` in per-line hot path (8 instances)  
> **Impact:** `LogAnalyzer.TryParseLine` creates 2-3 new `Regex` objects per log line...  
> **Fix:** Hoist to `static readonly` fields, or use `[GeneratedRegex]` on .NET 8+.

**Excerpt — dotnet-perf-skills-improved (`output/dotnet-perf-skills-improved/run-2/analyze-perf-issues/performance-analysis.md`)**
> #### 13. 48 `RegexOptions.Compiled` without `[GeneratedRegex]` (MarkdownStripper)  
> **Impact:** 48 compiled regex instances in one class...  
> **Fix:** Convert to `[GeneratedRegex]` partial methods.

**Excerpt — no-skills (`output/no-skills/run-2/analyze-perf-issues/performance-analysis.md`)**
> ### 1. Regex Anti-Patterns (affects 5/10 files)  
> - **Per-call `new Regex`**: `TemplateEngine`, `SlugGenerator`, `CsvParser`, `ValidationEngine`, `LogAnalyzer`  
> - **Excessive `RegexOptions.Compiled`**: `MarkdownStripper` (45 compiled regexes)  
> - **Missing `[GeneratedRegex]`**: All files

**Score:** dotnet-perf-skills **5/5** · dotnet-perf-skills-improved **5/5** · no-skills **4/5**  
**Verdict:** **Tie between dotnet-perf-skills and dotnet-perf-skills-improved**. Both are comprehensive and hot-path aware; baseline is strong but less precise/consistent in counts and rigor.

## 2. String Allocation Detection [CRITICAL]

All three detect loop concatenation and casing-allocation issues; skills variants add stronger impact framing and broader pattern grouping.

**Excerpt — dotnet-perf-skills**
> #### 4. O(n²) string concatenation in loops (5 sites)  
> **Impact:** `+=` on strings in loops creates a new string on every iteration...  
> **Fix:** Replace with `StringBuilder`.

**Excerpt — dotnet-perf-skills-improved**
> #### 14. `.ToLower()`/`.ToUpper()` without culture or ordinal (17 instances)  
> **Impact:** Culture-sensitive by default (Turkish-I problem), each call allocates a new string.  
> **Fix:** Use `StringComparison.OrdinalIgnoreCase`...

**Excerpt — no-skills**
> | 1 | 🔴 Critical | 51-79 | **Character-by-character string concatenation** in `ParseLine`... | Use `StringBuilder` or `ReadOnlySpan<char>` slicing. |  
> | 3 | 🟡 Moderate | 38 | **`.ToLower()` without ordinal on header keys**... | Lowercase headers once and reuse... |

**Score:** dotnet-perf-skills **5/5** · dotnet-perf-skills-improved **5/5** · no-skills **5/5**  
**Verdict:** **Three-way tie**. All outputs clearly capture the highest-impact string-allocation problems and provide concrete remediation.

## 3. Collection and LINQ Efficiency [CRITICAL]

Improved skills output is the most exhaustive on O(n) lookup and materialization patterns; baseline and standard skills are good but less complete.

**Excerpt — dotnet-perf-skills**
> #### 8. `ContainsKey` + indexer double-lookup pattern (10 actionable sites)  
> **Fix:** Replace with `TryGetValue`.  
> #### 11. `Skip(i).Take(5).ToList()` sliding window in loop (1 instance)

**Excerpt — dotnet-perf-skills-improved**
> #### 9. `List.Contains()` used as lookup — O(n) per check (2 sites)  
> **Fix:** Use `HashSet<string>` for O(1) lookups.  
> #### 24. Three separate iterations over same list (LogAnalyzer.Analyze)

**Excerpt — no-skills**
> | 6 | 🔴 Critical | 75-85 | **`.ToList()` + `.Contains()` (O(n)) in a loop**... | Use a `HashSet<string>`... |  
> ### 4. `ContainsKey` + Indexer Double Lookup (affects 4/10 files)  
> Found in `EntityMapper`, `ValidationEngine`, `LogAnalyzer`, `DataPipeline`.

**Score:** dotnet-perf-skills **4/5** · dotnet-perf-skills-improved **5/5** · no-skills **4/5**  
**Verdict:** **dotnet-perf-skills-improved wins** due to broader LINQ/materialization coverage and clearer complexity framing.

## 4. Async and IO Pattern Detection [CRITICAL]

All outputs catch `HttpClient` misuse and batch-send async issues; improved skills has the clearest end-to-end coverage including cancellation.

**Excerpt — dotnet-perf-skills**
> #### 1. `new HttpClient()` per call — socket exhaustion risk (3 instances)  
> #### 12. Sequential awaits in loop ... + unbounded parallelism ...  
> **Fix:** Use `Parallel.ForEachAsync` ... Add `CancellationToken` parameters.

**Excerpt — dotnet-perf-skills-improved**
> #### 18. Sequential awaits in batch loop (1 instance)  
> #### 19. Unbounded parallelism in `SendBatchParallelAsync` (1 instance)  
> #### 20. Missing cancellation tokens in async methods

**Excerpt — no-skills**
> | 1 | 🔴 Critical | 163, 179, 191 | **`new HttpClient` per call**... | Use `IHttpClientFactory`... |  
> | 2 | 🟡 Moderate | 116-123 | **Sequential `await` in loop**... | Use `Task.WhenAll` with throttling... |  
> | 3 | 🟡 Moderate | 130-133 | **Unbounded parallelism**... |

**Score:** dotnet-perf-skills **4/5** · dotnet-perf-skills-improved **5/5** · no-skills **4/5**  
**Verdict:** **dotnet-perf-skills-improved is best** for explicit, complete async/IO anti-pattern coverage.

## 5. Reflection and Serialization Overhead [HIGH]

All three detect uncached reflection and per-call serializer options strongly; none deeply push partial parsing (`Utf8JsonReader`) opportunities.

**Excerpt — dotnet-perf-skills**
> #### 3. Uncached `new JsonSerializerOptions` per call (4 instances)  
> #### 7. Uncached reflection `GetProperties()`...`SetValue()`...`GetValue()` (6 instances)

**Excerpt — dotnet-perf-skills-improved**
> #### 2. Uncached `new JsonSerializerOptions` per call (5 instances)  
> #### 11. Uncached reflection `GetProperties()`/`GetProperty()` in hot paths (3 instances)  
> #### 12. Uncached reflection `SetValue()`/`GetValue()` in mapping loops (3 instances)

**Excerpt — no-skills**
> | 1 | 🔴 Critical | 74 | **`new JsonSerializerOptions` per call in `Merge`**... | Cache as a `private static readonly JsonSerializerOptions`. |  
> | 1 | 🔴 Critical | 77, 101-102 | **Uncached `GetProperties()` + `SetValue()` via reflection**... |

**Score:** dotnet-perf-skills **4/5** · dotnet-perf-skills-improved **4/5** · no-skills **4/5**  
**Verdict:** **Tie**. Coverage is strong across all three, with a common gap on recommending selective parsing strategies.

## 6. Structural Optimization Detection [HIGH]

Improved skills output is strongest on sealed-class census + `IEquatable<T>` + `FrozenDictionary` specifics.

**Excerpt — dotnet-perf-skills**
> #### 14. Unsealed leaf classes (17 of 17 classes unsealed, 0 sealed)  
> #### 9. Structs without `IEquatable<T>` (2 of 2 structs)  
> #### 10. `static readonly Dictionary<>` → `FrozenDictionary` candidates (2 instances)

**Excerpt — dotnet-perf-skills-improved**
> #### 16. Unsealed classes — 18 of 18 classes are unsealed (0 sealed)  
> #### 17. Structs without `IEquatable<T>` (2 of 2 structs)  
> #### 15. `static readonly Dictionary<>` — FrozenDictionary candidates (2 instances)

**Excerpt — no-skills**
> ### 6. Unsealed Leaf Classes (affects 3/10 files)  
> `ValidationResult`, `MappingConfig`, `Record`...  
> ### 7. Structs Without `IEquatable<T>` (affects 2/10 files)

**Score:** dotnet-perf-skills **4/5** · dotnet-perf-skills-improved **5/5** · no-skills **3/5**  
**Verdict:** **dotnet-perf-skills-improved wins** with the most systematic structural analysis.

## 7. Severity Classification Accuracy [HIGH]

Standard skills output has the cleanest impact-tier calibration. Improved and baseline over-escalate some moderate patterns.

**Excerpt — dotnet-perf-skills**
> | 🔴 Critical | 5 | `new HttpClient()` per call..., uncached `new Regex()` in per-line parser... |  
> | 🟡 Moderate | 8 | `.ToLower()`/`.ToUpper()`..., `ContainsKey`+indexer..., structs without `IEquatable<T>`... |

**Excerpt — dotnet-perf-skills-improved**
> | 🔴 Critical | 12 | Per-call `new Regex()`..., `new HttpClient()`..., uncached `JsonSerializerOptions`... |  
> #### 10. ContainsKey + indexer double-lookup (10 instances)  
> **Impact:** ~2× slower per dictionary access...

**Excerpt — no-skills**
> | 🔴 Critical | 7 |  
> | 🟡 Moderate | 22 |  
> | ℹ️ Info | 23 |  
> | 4 | ℹ️ Info | 126-133 | **`ContainsKey` + indexer** for tag counting. |

**Score:** dotnet-perf-skills **5/5** · dotnet-perf-skills-improved **3/5** · no-skills **3/5**  
**Verdict:** **dotnet-perf-skills is best** because it consistently keeps hot-path/regression-risk issues above lower-impact micro-optimizations.

## 8. Fix Recommendation Quality [HIGH]

All three are actionable, but standard skills output has the best balance of API-specific fixes plus guardrails against bad advice.

**Excerpt — dotnet-perf-skills**
> **Fix:** Inject `IHttpClientFactory` or use a single `static readonly HttpClient`...  
> **Fix:** ...use `[GeneratedRegex]` on .NET 8+...  
> **Fix:** Use `StringComparison.OrdinalIgnoreCase`... `ToLowerInvariant()`...

**Excerpt — dotnet-perf-skills-improved**
> **Fix:** Use `Parallel.ForEachAsync` with `MaxDegreeOfParallelism`, or `SemaphoreSlim`...  
> **Fix:** Add `CancellationToken` parameters and pass to `Task.Delay`...

**Excerpt — no-skills**
> | Rank | 1 | Replace `new HttpClient()` with `IHttpClientFactory` or static instance |  
> | Rank | 7 | Migrate 45 `RegexOptions.Compiled` to `[GeneratedRegex]` |

**Score:** dotnet-perf-skills **5/5** · dotnet-perf-skills-improved **4/5** · no-skills **4/5**  
**Verdict:** **dotnet-perf-skills wins** on precision and correctness of fix guidance.

## Weighted Summary

Weights used: **Critical ×3**, **High ×2**, **Medium ×1**, **Low ×0.5**.

| Configuration | Critical subtotal | High subtotal | Total weighted score |
|---|---:|---:|---:|
| dotnet-perf-skills | (5+5+4+4)×3 = 54 | (4+4+5+5)×2 = 36 | **90** |
| dotnet-perf-skills-improved | (5+5+5+5)×3 = 60 | (4+5+3+4)×2 = 32 | **92** |
| no-skills | (4+5+4+4)×3 = 51 | (4+3+3+4)×2 = 28 | **79** |

## What All Versions Get Right

- All three identify the highest-risk production issue: **`new HttpClient()` per call**.
- All three call out **regex misuse** in hot paths and recommend hoisting/caching.
- All three detect **string `+=` in loops** and recommend `StringBuilder`.
- All three detect **reflection + serializer options caching** opportunities.
- All three provide concrete, .NET-specific APIs in many recommendations.

## Summary: Impact of Skills

Most impactful differences, ranked:

1. **Coverage breadth and systematization:** both skill-enabled runs are more complete and more explicitly quantified than baseline.
2. **Structural analysis depth:** improved skills most clearly identifies whole-codebase structural patterns (sealed ratio, structs, frozen collections).
3. **Severity calibration quality:** standard skills is best at prioritization discipline; improved over-promotes several moderate issues.

Overall assessment from weighted scores: **dotnet-perf-skills-improved (92) > dotnet-perf-skills (90) >> no-skills (79)**. The skills primarily increase consistency, category coverage, and actionability; the improved variant adds breadth but should tighten severity calibration to maximize prioritization signal.
