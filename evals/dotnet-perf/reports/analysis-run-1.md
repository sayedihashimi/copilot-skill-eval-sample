# Comparative Analysis: dotnet-perf-skills, dotnet-perf-skills-improved, no-skills

This report compares **3 configurations** on the single `analyze-perf-issues` scenario at `output/{config}/run-1/analyze-perf-issues/`. Configuration mapping came from `gen-notes.md` plus directory naming: `dotnet-perf-skills` and `dotnet-perf-skills-improved` both explicitly report using the `analyzing-dotnet-performance` skill; `no-skills` is baseline/default Copilot (its notes list work performed but no skill usage section).

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | dotnet-perf-skills-improved | no-skills |
|---|---:|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 4 | 5 | 4 |
| String Allocation Detection [CRITICAL] | 4 | 5 | 4 |
| Collection and LINQ Efficiency [CRITICAL] | 4 | 5 | 3 |
| Async and IO Pattern Detection [CRITICAL] | 4 | 5 | 4 |
| Reflection and Serialization Overhead [HIGH] | 4 | 5 | 4 |
| Structural Optimization Detection [HIGH] | 4 | 5 | 4 |
| Severity Classification Accuracy [HIGH] | 3 | 5 | 3 |
| Fix Recommendation Quality [HIGH] | 4 | 5 | 3 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills — `output/dotnet-perf-skills/run-1/analyze-perf-issues/performance-analysis.md`**
> #### 1. Per-Call `new Regex()` in Hot Paths (8 instances)  
> **Impact:** ... `LogAnalyzer.TryParseLine`, this runs per log line — potentially millions of times.

**dotnet-perf-skills-improved — `output/dotnet-perf-skills-improved/run-1/analyze-perf-issues/performance-analysis.md`**
> | Regex: `RegexOptions.Compiled` | startup budget | **48** in MarkdownStripper.cs |  
> **Fix:** Convert each to `[GeneratedRegex]` on a `partial class`.

**no-skills — `output/no-skills/run-1/analyze-perf-issues/performance-analysis.md`**
> 2. `LogAnalyzer.TryParseLine` — `new Regex()` per log line in hot loop (🔴)  
> 5. `MarkdownStripper` — 45+ `RegexOptions.Compiled` instances bloating JIT startup (🟡)

**Scores:** dotnet-perf-skills **4/5** (good detection + GeneratedRegex guidance), dotnet-perf-skills-improved **5/5** (most complete and explicit on startup budget + source-gen), no-skills **4/5** (covers core regex issues well but less systematic than improved).

**Verdict:** **dotnet-perf-skills-improved** is best due to strongest regex breadth and precise remediation.

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills**
> #### 4. String `+=` Concatenation in Loops — O(n²) Allocation (6 sites)  
> **Fix:** Use `StringBuilder`.

**dotnet-perf-skills-improved**
> #### 16. Chained `.Replace()` calls in loop — N×9 intermediate string allocations (1 site)  
> ... a single `GenerateSlug` call creates ~23 intermediate strings.

**no-skills**
> 3. `TemplateEngine.ProcessLoops` — O(n²) string concatenation (🔴)  
> 4. `CsvParser.ParseLine` — char-by-char string concatenation (🔴)

**Scores:** dotnet-perf-skills **4/5**, dotnet-perf-skills-improved **5/5**, no-skills **4/5**.

**Verdict:** **dotnet-perf-skills-improved** best captures both loop concatenation and compound replacement-allocation chains.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills**
> #### 6. `List.Contains()` in Loop — O(n²) Lookup ...  
> #### 16. `Skip(i).Take(5).ToList()` in Loop — O(n²) LINQ

**dotnet-perf-skills-improved**
> #### 7. `Keys.ToList()` + `.Contains()` for O(n) lookups ...  
> | Collections: `.Distinct().ToList()` | dedup materialization | **2** in DataPipeline.cs |

**no-skills**
> | 7 | 🟡 Moderate | 75 | `.ToList()` + `.Contains()` (O(n)) in loop ... |  
> | 8 | 🔴 Critical | 157 | `Skip(i).Take(5).ToList()` in loop | O(n²) sliding window |

**Scores:** dotnet-perf-skills **4/5**, dotnet-perf-skills-improved **5/5**, no-skills **3/5** (good findings, but less coherent prioritization and consistency across collection patterns).

**Verdict:** **dotnet-perf-skills-improved** is strongest on both detection breadth and prioritization quality.

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills**
> #### 2. `new HttpClient()` Per Call — Socket Exhaustion (3 instances)  
> #### 8. Unbounded Parallelism in SendBatchParallelAsync

**dotnet-perf-skills-improved**
> #### 11. Sequential awaits in loop — no parallelism (1 instance)  
> #### 13. Missing cancellation tokens on async operations

**no-skills**
> | 1 | 🔴 Critical | 163, 179, 191 | `new HttpClient()` per call ... |  
> | 5 | 🔴 Critical | 132 | Unbounded parallelism |

**Scores:** dotnet-perf-skills **4/5**, dotnet-perf-skills-improved **5/5**, no-skills **4/5**.

**Verdict:** **dotnet-perf-skills-improved** best combines socket-risk, latency, throttling, and cancellation coverage.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills**
> #### 3. Uncached `new JsonSerializerOptions` Per Call (5 instances)  
> #### 11. Uncached Reflection in EntityMapper (6 calls)

**dotnet-perf-skills-improved**
> #### 3. Uncached `new JsonSerializerOptions` per call (4 instances) ... 592x slower  
> #### 8. Uncached reflection `GetProperties()` / `GetProperty()` in hot paths

**no-skills**
> | 1 | 🟡 Moderate | 74, 117, 135, 142 | `new JsonSerializerOptions` per call |  
> | 4 | 🟡 Moderate | 77 | Uncached `GetProperties()` per call |

**Scores:** dotnet-perf-skills **4/5**, dotnet-perf-skills-improved **5/5**, no-skills **4/5**.

**Verdict:** **dotnet-perf-skills-improved** provides the clearest impact framing and best implementation guidance.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills**
> #### 19. 0 of 17 Classes Are Sealed  
> #### 21. `static readonly Dictionary<>` — FrozenDictionary Candidates (2 instances)

**dotnet-perf-skills-improved**
> | Structural: unsealed classes | devirtualization | **17** classes, **0** sealed |  
> | Structural: `IEquatable<T>` on structs | boxing avoidance | **0** of **2** structs implement it |

**no-skills**
> | 1 | ℹ️ Info | 11 | `Dictionary` could be `FrozenDictionary` |  
> ### 6. Unsealed Leaf Classes (3 files)

**Scores:** dotnet-perf-skills **4/5**, dotnet-perf-skills-improved **5/5**, no-skills **4/5**.

**Verdict:** **dotnet-perf-skills-improved** is most complete on the exact structural criteria in the rubric.

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills**
> #### 7. `ContainsKey` + Indexer Double-Lookup (~12 instances)  
> **Impact:** ~2× slower per lookup ... (listed under 🔴 Critical)

**dotnet-perf-skills-improved**
> #### 20. `ContainsKey` + indexer double-lookup pattern (~12 instances)  
> **Impact:** ~2x per-lookup overhead. Minor unless in proven hot path.

**no-skills**
> - 🔴 **Critical**: 7 — socket exhaustion, O(n²) string concatenation in hot paths, regex allocation per log line  
> - 🟡 **Moderate**: 24 — per-call regex instantiation, uncached reflection...

**Scores:** dotnet-perf-skills **3/5** (some over-escalation), dotnet-perf-skills-improved **5/5** (best hot-path vs moderate/info separation), no-skills **3/5** (reasonable top priorities but less consistently tiered).

**Verdict:** **dotnet-perf-skills-improved** has the most accurate severity ranking.

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills**
> **Fix:** Inject `IHttpClientFactory` or use a `static readonly HttpClient` with `PooledConnectionLifetime`.  
> **Fix:** ... use `[GeneratedRegex]` source generator (preferred on .NET 7+).

**dotnet-perf-skills-improved**
> | 1 | Replace `new HttpClient()` with static/injected client | 🔴 ... |  
> | 2 | Cache regex instances in `LogAnalyzer.TryParseLine` as `[GeneratedRegex]` | 🔴 >10x speedup |

**no-skills**
> 1. `NotificationService` — `new HttpClient()` per call → socket exhaustion (🔴)  
> 5. `MarkdownStripper` — 45+ `RegexOptions.Compiled` instances bloating JIT startup (🟡)

**Scores:** dotnet-perf-skills **4/5**, dotnet-perf-skills-improved **5/5**, no-skills **3/5**.

**Verdict:** **dotnet-perf-skills-improved** gives the most specific, API-correct, and execution-ready recommendations.

## Weighted Summary

Weights: **Critical ×3**, **High ×2**.

| Configuration | Critical subtotal | High subtotal | Total weighted score |
|---|---:|---:|---:|
| dotnet-perf-skills | (4+4+4+4)×3 = 48 | (4+4+3+4)×2 = 30 | **78** |
| dotnet-perf-skills-improved | (5+5+5+5)×3 = 60 | (5+5+5+5)×2 = 40 | **100** |
| no-skills | (4+4+3+4)×3 = 45 | (4+4+3+3)×2 = 28 | **73** |

## What All Versions Get Right

- They all identify the top production-risk issues: `new HttpClient()` per call and per-call/hot-path regex allocation.
- They all flag startup/perf concerns around heavy `RegexOptions.Compiled` usage in `MarkdownStripper`.
- They all detect O(n²)-style string-building patterns and recommend `StringBuilder`.
- They all include actionable .NET-specific fixes (e.g., `GeneratedRegex`, `IHttpClientFactory`, `HashSet`, `TryGetValue`).

## Summary: Impact of Skills

**Ranking by weighted score:**  
1. **dotnet-perf-skills-improved (100)**  
2. **dotnet-perf-skills (78)**  
3. **no-skills (73)**

Most impactful differences were (1) better severity calibration, (2) stronger cross-cutting synthesis, and (3) more precise fix ordering in the improved skill output. Baseline (`no-skills`) is still substantial, but it is less consistent in prioritization and recommendation quality. The original skill configuration improves structure and actionability; the improved skill version is clearly the strongest overall.
