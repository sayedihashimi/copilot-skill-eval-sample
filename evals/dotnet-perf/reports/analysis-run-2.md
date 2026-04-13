# Comparative Analysis: dotnet-perf-skills, no-skills

This run compares **2 configurations** (`dotnet-perf-skills`, `no-skills`) on **1 shared scenario**: `analyze-perf-issues` under `output/{config}/run-2/analyze-perf-issues/`. Configuration mapping came from each scenario’s `gen-notes.md`: `dotnet-perf-skills` explicitly used the `analyzing-dotnet-performance` skill, while `no-skills` is baseline analysis output.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 4 |
| String Allocation Detection [CRITICAL] | 5 | 4 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 5 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 4 |
| Reflection and Serialization Overhead [HIGH] | 5 | 4 |
| Structural Optimization Detection [HIGH] | 5 | 4 |
| Severity Classification Accuracy [HIGH] | 4 | 4 |
| Fix Recommendation Quality [HIGH] | 5 | 4 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills excerpt** (`output/dotnet-perf-skills/run-2/analyze-perf-issues/performance-analysis.md`)
> 2. **Regex instantiation in hot loops** — `new Regex()` called per log line in LogAnalyzer (4 instances)  
> 3. **48 `RegexOptions.Compiled` regexes** in MarkdownStripper with 0 `[GeneratedRegex]` usage project-wide  
> **Fix:** Hoist to `static readonly` fields, or better, use `[GeneratedRegex]`

**no-skills excerpt** (`output/no-skills/run-2/analyze-perf-issues/performance-analysis.md`)
> 2. `LogAnalyzer.TryParseLine` — `new Regex()` per log line → O(n) regex compilations on large files  
> 5. `MarkdownStripper` — 45+ `RegexOptions.Compiled` instances → excessive JIT startup cost  
> **Fix for #1 (on .NET 7+):** ... `[GeneratedRegex(...)]`

**Score:** `dotnet-perf-skills: 5/5`, `no-skills: 4/5` (both catch the key issues; skills output is more explicit on project-wide `[GeneratedRegex]` absence and quantified regex counts).

**Verdict:** **dotnet-perf-skills** is best on regex depth and prioritization.

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills excerpt**
> #### 6. O(n²) String Concatenation via `+=` in Loops (7 files, ~15 sites)  
> ... `CsvParser.ParseLine`, `CsvParser.SplitLines`, ... `TemplateEngine.ProcessLoops`, `TemplateEngine.RenderBatch`  
> #### 8. `.ToLower()`/`.ToUpper()` Without Culture or StringComparison (25 instances)

**no-skills excerpt**
> | 1 | 🔴 Critical | 51–79 | Char-by-char string `+=` (O(n²)) | `ParseLine` builds field values with `current += line[i]` |  
> | 2 | 🔴 Critical | 88–108 | Char-by-char string `+=` (O(n²)) | `SplitLines` has the same O(n²) pattern |  
> | 6 |  |  | `.ToLower()`/`.ToUpper()` calls without `StringComparison` |

**Score:** `dotnet-perf-skills: 5/5`, `no-skills: 4/5` (both strong; skills output is more systematic and counted across files).

**Verdict:** **dotnet-perf-skills** is stronger due to clearer scale framing and consolidated cross-file signal.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills excerpt**
> #### 10. `ContainsKey` + Indexer Double-Lookup (12 instances)  
> #### 19. `.ToList()` for Counting / O(n) `.Contains()` on Lists (5 instances)  
> #### 22. `Skip(i).Take(5).ToList()` in Sliding Window Loop (1 instance)

**no-skills excerpt**
> | 6 | 🔴 Critical | 75–85 | `List.Contains` in while loop (O(n²)) | ... Use a `HashSet<string>`. |  
> | 5 | 🟡 Moderate | 85–89 | `List.Contains` for key lookups (O(n²)) | `JsonTransformer.Diff` ... |  
> | 7 | 🟡 Moderate | 152–157 | `.ToList()` + `Skip(i).Take(5).ToList()` per iteration |

**Score:** `dotnet-perf-skills: 5/5`, `no-skills: 5/5` (both cover all requested hot-path collection/LINQ anti-patterns with concrete fixes).

**Verdict:** **Tie** — both are comprehensive and actionable.

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills excerpt**
> #### 1. `new HttpClient()` Per Call — Socket Exhaustion Risk (3 instances)  
> #### 16. Sequential Awaits in Loop (1 instance)  
> #### 17. Unbounded Parallelism (1 instance)  
> #### 18. Missing `CancellationToken` on Async Methods

**no-skills excerpt**
> | 1 | 🔴 Critical | 163, 179, 191 | `new HttpClient()` per call | ... socket exhaustion |  
> | 2 | 🟡 Moderate | 116–118 | Sequential `await` in loop |  
> | 3 | 🔴 Critical | 130–133 | Unbounded parallelism |  
> | 4 | 🟡 Moderate | 102 | Missing `CancellationToken` |

**Score:** `dotnet-perf-skills: 5/5`, `no-skills: 4/5` (baseline identifies all key patterns, but skills output provides richer impact and fix framing for cancellation/retry behavior).

**Verdict:** **dotnet-perf-skills** is better on async/IO operational risk framing.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills excerpt**
> #### 4. Uncached `JsonSerializerOptions` Per Call (4 instances)  
> #### 7. Uncached Reflection — `GetProperties()` / `SetValue()` / `GetValue()` Per Call (4 instances)  
> **Fix:** Cache `PropertyInfo[]` per type ... `ConcurrentDictionary<Type, PropertyInfo[]>`

**no-skills excerpt**
> | 1 | 🟡 Moderate | 74 | `new JsonSerializerOptions` per call | ... Cache as `static readonly`. |  
> | 1 | 🔴 Critical | 77 | Uncached `GetProperties()` reflection | ... Cache per type using `ConcurrentDictionary<Type, PropertyInfo[]>`. |

**Score:** `dotnet-perf-skills: 5/5`, `no-skills: 4/5` (both identify the core problems; skills output is broader and more quantified).

**Verdict:** **dotnet-perf-skills** is stronger on serialization + reflection breadth.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills excerpt**
> #### 13. FrozenDictionary Candidates — `static readonly Dictionary<>` Never Mutated (2 instances)  
> #### 14. Structs Without `IEquatable<T>` (2 instances)  
> #### 15. Unsealed Classes — 0 of 17 Sealed (17 instances)

**no-skills excerpt**
> ### 5. Unsealed Classes  
> **DataPipeline.Record**, **ValidationEngine.ValidationResult**, **EntityMapper.MappingConfig** ...  
> ### 6. Structs Without `IEquatable<T>`  
> ... **DeliveryResult** and **ValidationError**

**Score:** `dotnet-perf-skills: 5/5`, `no-skills: 4/5` (baseline catches structural themes but is less exhaustive on FrozenDictionary and global class-count framing).

**Verdict:** **dotnet-perf-skills** wins on structural completeness.

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills excerpt**
> | Rank | Finding | Severity | Effort | Impact |  
> | 1 | Reuse HttpClient (socket exhaustion) | 🔴 | Quick-fix | Prevents production incidents |  
> | 2 | Cache Regex in LogAnalyzer hot loop | 🔴 | Quick-fix | >10x parsing speedup |

**no-skills excerpt**
> | Rank | File | Issue | Severity | Effort | Impact |  
> | 1 | NotificationService.cs | `new HttpClient()` per call — socket exhaustion | 🔴 Critical | Moderate | Prevents production outages |  
> | 2 | LogAnalyzer.cs | `new Regex()` per log line in `TryParseLine` | 🔴 Critical | Quick-fix | 100x+ speedup |

**Score:** `dotnet-perf-skills: 4/5`, `no-skills: 4/5` (both prioritize true hot-path/incident issues correctly above moderate cleanup items).

**Verdict:** **Tie** — both rank critical production risks first with reasonable impact ordering.

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills excerpt**
> **Fix:** Inject `IHttpClientFactory` or use a single `static readonly HttpClient` ...  
> **Fix:** ... use `[GeneratedRegex]` ...  
> **Fix:** Replace with `TryGetValue` ... `FrozenDictionary` ... `StringComparison.OrdinalIgnoreCase`

**no-skills excerpt**
> The fix is consistent: promote to `static readonly Regex` fields, or use `[GeneratedRegex]` on .NET 7+.  
> Always replace with `StringBuilder`.  
> Replace with `TryGetValue` ... Cache as `static readonly`.

**Score:** `dotnet-perf-skills: 5/5`, `no-skills: 4/5` (both actionable; skills output is more API-specific and consistently ties fixes to .NET 8 capabilities).

**Verdict:** **dotnet-perf-skills** provides higher-quality, more implementation-ready recommendations.

## Weighted Summary

Weights used: **Critical ×3**, **High ×2**.

| Configuration | Critical subtotal (max 60) | High subtotal (max 40) | Total weighted (max 100) |
|---|---:|---:|---:|
| dotnet-perf-skills | 60 | 38 | **98** |
| no-skills | 51 | 32 | **83** |

## What All Versions Get Right

- Correctly flag **per-call `HttpClient`** as a top critical risk.
- Catch **regex hot-path misuse** (especially in `LogAnalyzer`) and recommend caching/source generation.
- Identify **string `+=` in loops** and recommend `StringBuilder`.
- Call out **collection lookup inefficiencies** (`List.Contains`, `ContainsKey`+indexer, unnecessary materialization).
- Surface **reflection/serialization cache issues** and propose static/shared options.

## Summary: Impact of Skills

Most impactful differences, ranked:
1. **Regex analysis depth**: skills output better quantified compiled-regex startup debt and `[GeneratedRegex]` adoption gap.
2. **Structural completeness**: skills output more systematically covered unsealed classes, structs without `IEquatable<T>`, and FrozenDictionary opportunities.
3. **Fix precision**: skills output gave more consistently API-specific guidance aligned to .NET 8.

Overall, both outputs are useful, but **dotnet-perf-skills** is clearly stronger on completeness, precision, and prioritization consistency (**98 vs 83 weighted**). **no-skills** remains competent but less exhaustive in critical-depth and high-tier structural/fix detail.
