# Comparative Analysis: dotnet-perf-skills, dotnet-perf-skills-improved, no-skills

This run compares **3 configurations** on **1 shared scenario**: `analyze-perf-issues` under each `output/{config}/run-2/analyze-perf-issues/`. Configuration identity was confirmed from each scenario-level `gen-notes.md`: `dotnet-perf-skills` (Performance Skills), `dotnet-perf-skills-improved` (Performance Skills improved), and `no-skills` (baseline/default Copilot).

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | dotnet-perf-skills-improved | no-skills |
|---|---:|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 5 | 4 |
| String Allocation Detection [CRITICAL] | 5 | 5 | 4 |
| Collection and LINQ Efficiency [CRITICAL] | 4 | 5 | 4 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 4 | 4 |
| Reflection and Serialization Overhead [HIGH] | 4 | 4 | 3 |
| Structural Optimization Detection [HIGH] | 5 | 5 | 4 |
| Severity Classification Accuracy [HIGH] | 4 | 5 | 3 |
| Fix Recommendation Quality [HIGH] | 4 | 5 | 3 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-2/analyze-perf-issues/performance-analysis.md`):  
> 2. **Regex instantiation in hot loops** — `new Regex()` called per log line in LogAnalyzer (4 instances)  
> 3. **48 `RegexOptions.Compiled` regexes** in MarkdownStripper with 0 `[GeneratedRegex]` usage project-wide

**dotnet-perf-skills-improved** (`output/dotnet-perf-skills-improved/run-2/analyze-perf-issues/performance-analysis.md`):  
> 2. 🔴 `new Regex()` in hot paths — per-call regex compilation (8 instances)  
> 5. 🔴 48 `RegexOptions.Compiled` instances with 0 `[GeneratedRegex]` — startup budget blown

**no-skills** (`output/no-skills/run-2/analyze-perf-issues/performance-analysis.md`):  
> 2. `LogAnalyzer.TryParseLine` — `new Regex()` per log line → O(n) regex compilations on large files  
> 5. `MarkdownStripper` — 45+ `RegexOptions.Compiled` instances → excessive JIT startup cost

**Score:** dotnet-perf-skills **5**, dotnet-perf-skills-improved **5**, no-skills **4**.  
**Verdict:** **Tie between dotnet-perf-skills and improved**; both explicitly connect hot-path per-call regex + Compiled overuse + `[GeneratedRegex]` migration.

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills**:  
> #### 6. O(n²) String Concatenation via `+=` in Loops (7 files, ~15 sites)  
> #### 8. `.ToLower()`/`.ToUpper()` Without Culture or StringComparison (25 instances)

**dotnet-perf-skills-improved**:  
> #### 4. String `+=` concatenation in loops — O(n²) allocation (6+ sites)  
> #### 6. `.ToLower()` / `.ToUpper()` without culture/ordinal (17 instances)

**no-skills**:  
> 4. `CsvParser.ParseLine` — char-by-char string `+=` → O(n²) allocations  
> 3. `.ToLower()` Without Culture ... avoid Turkish-I bugs and unnecessary allocations.

**Score:** dotnet-perf-skills **5**, dotnet-perf-skills-improved **5**, no-skills **4**.  
**Verdict:** **dotnet-perf-skills and improved are strongest**; both are systematic and quantify impact clearly.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills**:  
> #### 10. `ContainsKey` + Indexer Double-Lookup (12 instances)  
> #### 19. `.ToList()` for Counting / O(n) `.Contains()` on Lists (5 instances)

**dotnet-perf-skills-improved**:  
> #### 8. `ContainsKey` + indexer double-lookup pattern (10+ instances)  
> #### 11. `Distinct().ToList()` and `Skip().Take().ToList()` in loops (2 instances)

**no-skills**:  
> ### 4. `ContainsKey` + Indexer (Double Lookup) ... Replace with `TryGetValue`  
> | 6 | 🔴 Critical | 75–85 | `List.Contains` in while loop (O(n²)) |

**Score:** dotnet-perf-skills **4**, dotnet-perf-skills-improved **5**, no-skills **4**.  
**Verdict:** **dotnet-perf-skills-improved wins** on focused coverage of hot-path LINQ materialization patterns plus dictionary/list lookup complexity.

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills**:  
> #### 1. `new HttpClient()` Per Call — Socket Exhaustion Risk (3 instances)  
> #### 16. Sequential Awaits in Loop ... #### 17. Unbounded Parallelism ... #### 18. Missing `CancellationToken`

**dotnet-perf-skills-improved**:  
> 1. 🔴 `new HttpClient()` per call — socket exhaustion risk (3 instances)  
> #### 16. Sequential `await` in loop ... The parallel variant ... has unbounded parallelism.

**no-skills**:  
> | 1 | 🔴 Critical | 163, 179, 191 | `new HttpClient()` per call |  
> | 3 | 🔴 Critical | 130–133 | Unbounded parallelism |

**Score:** dotnet-perf-skills **5**, dotnet-perf-skills-improved **4**, no-skills **4**.  
**Verdict:** **dotnet-perf-skills is best**; it is the clearest on the full async lifecycle (resource reuse + concurrency model + cancellation propagation).

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills**:  
> #### 4. Uncached `JsonSerializerOptions` Per Call (4 instances)  
> #### 7. Uncached Reflection — `GetProperties()` / `SetValue()` / `GetValue()` Per Call (4 instances)

**dotnet-perf-skills-improved**:  
> #### 3. Uncached `JsonSerializerOptions` — up to 592x slower (4 instances)  
> #### 7. Uncached reflection — `GetProperties()` / `GetValue()` / `SetValue()` per call (3+3 instances)

**no-skills**:  
> | 1 | 🔴 Critical | 77 | Uncached `GetProperties()` reflection |  
> | 1 | 🟡 Moderate | 74 | `new JsonSerializerOptions` per call |

**Score:** dotnet-perf-skills **4**, dotnet-perf-skills-improved **4**, no-skills **3**.  
**Verdict:** **Tie between dotnet-perf-skills and improved**; both are concrete and API-specific, while no-skills is broader but less calibrated.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills**:  
> | Unsealed classes | 17 of 17 (0 sealed) |  
> | Structs without `IEquatable<T>` | 2 of 2 (0 implement it) |

**dotnet-perf-skills-improved**:  
> #### 12. Structs without `IEquatable<T>` (2 of 2 structs)  
> #### 13. `FrozenDictionary` candidates — 2 static readonly dictionaries never mutated

**no-skills**:  
> ### 5. Unsealed Classes  
> **DataPipeline.Record**, **ValidationEngine.ValidationResult**, **EntityMapper.MappingConfig**

**Score:** dotnet-perf-skills **5**, dotnet-perf-skills-improved **5**, no-skills **4**.  
**Verdict:** **Tie between dotnet-perf-skills and improved**; both fully hit sealed/IEquatable/FrozenDictionary with strong evidence.

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills**:  
> **38 distinct findings**: **7 Critical**, **21 Moderate**, and **10 Info**.  
> | 8 | SlugGenerator.cs | `List.Contains` in while loop → `HashSet` | 🔴 Critical |

**dotnet-perf-skills-improved**:  
> **Total findings:** 42 issues (5 🔴 Critical, 8 🟡 Moderate, 5 ℹ️ Info)  
> 1. 🔴 `new HttpClient()` per call ... 2. 🔴 `new Regex()` in hot paths ...

**no-skills**:  
> **Total issues found: 52**  
> - 🔴 **Critical**: 7 ... - 🟡 **Moderate**: 23 ... - ℹ️ **Info**: 22

**Score:** dotnet-perf-skills **4**, dotnet-perf-skills-improved **5**, no-skills **3**.  
**Verdict:** **dotnet-perf-skills-improved is best**; prioritization is tighter and less diluted by medium/low-impact noise.

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills**:  
> **Fix:** Inject `IHttpClientFactory` ... `SocketsHttpHandler { PooledConnectionLifetime = ... }`  
> **Fix:** Replace `+=` string loops with `StringBuilder`

**dotnet-perf-skills-improved**:  
> **Fix:** Hoist patterns ... preferably use `[GeneratedRegex]` (the project targets .NET 8)  
> **Fix:** Convert to `FrozenDictionary` ... `.ToFrozenDictionary();`

**no-skills**:  
> **Fix for #1:** ... inject via constructor `IHttpClientFactory`  
> **Fix for #1 (on .NET 7+):** `[GeneratedRegex(...)] private static partial Regex ...`

**Score:** dotnet-perf-skills **4**, dotnet-perf-skills-improved **5**, no-skills **3**.  
**Verdict:** **dotnet-perf-skills-improved is best**; recommendations are specific, modern, and consistently mapped to concrete APIs with fewer questionable side suggestions.

## Weighted Summary

Weights used: **Critical ×3**, **High ×2**.

| Configuration | Critical subtotal (4 dims) | High subtotal (4 dims) | Total weighted score |
|---|---:|---:|---:|
| dotnet-perf-skills | 57 | 34 | **91** |
| dotnet-perf-skills-improved | 57 | 38 | **95** |
| no-skills | 48 | 26 | **74** |

## What All Versions Get Right

- All three identify the highest-risk production issue: **`new HttpClient()` per call**.
- All three call out **per-call regex in hot paths** and the **MarkdownStripper `RegexOptions.Compiled` concentration**.
- All three flag **string `+=` in loops** and **culture-sensitive casing** (`ToLower`/`ToUpper`) as systemic allocation problems.
- All three surface **dictionary/list lookup inefficiencies** and recommend `TryGetValue`/`HashSet` patterns.

## Summary: Impact of Skills

Most impactful differences ranked:
1. **Prioritization quality:** improved skills produce the cleanest critical-vs-moderate separation.
2. **Actionability density:** improved skills give concise, high-signal fixes tied to .NET 8 APIs.
3. **Coverage consistency:** both skill-enabled runs are more systematic on structural and regex categories.

Overall assessment by weighted score: **dotnet-perf-skills-improved (95) > dotnet-perf-skills (91) > no-skills (74)**. Skills materially improve signal quality and prioritization, with the improved variant delivering the best balance of breadth, severity calibration, and actionable remediation guidance.
