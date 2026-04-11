# Comparative Analysis: dotnet-perf-skills, no-skills

This run compares **2 configurations** across the single scenario **`analyze-perf-issues`** using outputs in `output/{config}/run-2/analyze-perf-issues/`. Configuration identity comes from each scenario’s `gen-notes.md`: `dotnet-perf-skills` reflects the performance skill-guided workflow, while `no-skills` is baseline Copilot output.

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

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills excerpt** (`output/dotnet-perf-skills/run-2/analyze-perf-issues/performance-analysis.md`):
> #### 2. `new Regex()` allocated per-call in hot loops (8 instances)  
> **Impact:** LogAnalyzer allocates 2–3 `new Regex()` **per log line** ...  
> **Fix:** ... use `[GeneratedRegex]` with `partial` methods ...

**no-skills excerpt** (`output/no-skills/run-2/analyze-perf-issues/performance-analysis.md`):
> | 1 | 🟡 Moderate | 13–59 | 46 `RegexOptions.Compiled` regex instances ... | On .NET 7+, switch to `[GeneratedRegex]` ... |  
> | 1 | 🔴 Critical | 50 | `new Regex(...)` inside `TryParseLine` — called **per log line** ... |

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best due to stronger quantified coverage (per-call, static `Regex.Replace`, 48 compiled patterns) and explicit .NET 8 `[GeneratedRegex]` framing.

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills excerpt**:
> #### 5. `+=` string concatenation in loops — O(n²) allocation (6 sites)  
> ... CsvParser.ParseLine/SplitLines/FormatCsv all exhibit this pattern.  
> **Fix:** Replace with `StringBuilder`

**no-skills excerpt**:
> | 1 | 🔴 Critical | 51–79 | Character-by-character string `+=` in `ParseLine` — O(n²) ... | Use `StringBuilder` ... |  
> | 1 | 🟡 Moderate | 30 | `.ToLower()` without culture — Turkish-I problem, allocates |

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best for broader cross-file synthesis of loop concatenation + casing + replace-chain allocation patterns.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills excerpt**:
> #### 11. `List.Contains()` for O(n) lookups in a loop (SlugGenerator)  
> #### 12. `.ToList()` + `.Contains()` instead of `HashSet` in Diff (JsonTransformer)  
> #### 13. `Skip(i).Take(5).ToList()` inside a loop (LogAnalyzer)

**no-skills excerpt**:
> | 6 | 🔴 Critical | 75–85 | `.ToList()` + `.Contains()` (O(n)) in a while loop ... | Use `HashSet<string>` ... |  
> | 6 | 🟡 Moderate | 157 | `errorEntries.Skip(i).Take(5).ToList()` inside a for-loop ... |

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best by covering the full requested set (Contains/HashSet, materialization, double lookup, sliding windows) with cleaner impact statements.

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills excerpt**:
> #### 1. `new HttpClient()` per call — socket exhaustion risk (3 instances)  
> #### 14. Sequential `await` in a loop ...  
> #### 15. Unbounded parallelism ...  
> #### 16. Missing `CancellationToken` on async methods

**no-skills excerpt**:
> | 1 | 🔴 Critical | 163, 179, 191 | `new HttpClient()` per call ... **socket exhaustion** ... |  
> | 2 | 🟡 Moderate | 117 | Sequential `await` ... |  
> | 3 | 🟡 Moderate | 132 | Unbounded parallelism ... |

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best due to complete async/IO checklist coverage and stronger cancellation guidance.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills excerpt**:
> #### 4. Uncached `new JsonSerializerOptions` per call (4 instances)  
> #### 7. Uncached reflection `GetProperties()`/`SetValue()`/`GetValue()` per call (5 sites)

**no-skills excerpt**:
> | 1 | 🟡 Moderate | 74 | `new JsonSerializerOptions { WriteIndented = true }` in `Merge` ... |  
> | 1 | 🟡 Moderate | 77 | `typeof(TTarget).GetProperties()` via reflection on every `MapTo<T>` call |

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best with clearer hot-path emphasis and stronger “cache metadata/options” rationale.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills excerpt**:
> #### 17. Unsealed classes — 0 of 17 classes are sealed  
> #### 18. Static `Dictionary` candidates for `FrozenDictionary` (2 instances)  
> #### 21. Structs without `IEquatable<T>` (2 instances)

**no-skills excerpt**:
> ### 5. Unsealed Leaf Classes (3 files)  
> **DataPipeline.Record**, **ValidationEngine.ValidationResult**, **EntityMapper.MappingConfig** ...  
> ### 6. Structs Without `IEquatable<T>` (2 files)

**Score:** dotnet-perf-skills **5/5**; no-skills **3/5**.  
**Verdict:** **dotnet-perf-skills** is decisively best because it measures structural issues codebase-wide (0/17 sealed), while baseline only spotlights a subset.

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills excerpt**:
> | 🔴 Critical | 10 | Per-call `new Regex()` in hot loops ..., `new HttpClient()` per call ... |  
> | 🟡 Moderate | 22 | `+=` string concatenation in loops ..., 48 `RegexOptions.Compiled` ... |

**no-skills excerpt**:
> - 🔴 **Critical**: 6 issues — socket exhaustion, O(n²) string concatenation in hot paths ...  
> - 🟡 **Moderate**: 22 issues — per-call regex instantiation, uncached reflection ...  
> - ℹ️ **Info**: 20 issues — ... `ContainsKey`+indexer patterns, unsealed classes ...

**Score:** dotnet-perf-skills **4/5**; no-skills **3/5**.  
**Verdict:** **dotnet-perf-skills** is better tiering hot-path risks higher and startup/setup issues lower; both have some debatable placements, but baseline has more low/high mixing.

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills excerpt**:
> **Fix:** Inject `IHttpClientFactory` or use a single `static readonly HttpClient` ...  
> **Fix:** ... use `[GeneratedRegex]` with `partial` methods ...  
> **Fix:** Replace with `StringBuilder` ...  
> **Fix:** Convert to `FrozenDictionary` ...

**no-skills excerpt**:
> **Before/After — HttpClient fix:** ... `IHttpClientFactory` ...  
> **Before/After — Reflection caching:** `ConcurrentDictionary<Type, PropertyInfo[]>` ...  
> **Before/After — HashSet fix:** ...

**Score:** dotnet-perf-skills **5/5**; no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best on API precision and prioritization; baseline is still strong thanks to concrete before/after snippets.

## Weighted Summary

Weights applied: Critical ×3, High ×2.

| Configuration | Critical Raw (4 dims) | Critical Weighted | High Raw (4 dims) | High Weighted | Total Weighted |
|---|---:|---:|---:|---:|---:|
| dotnet-perf-skills | 20 | 60 | 19 | 38 | **98** |
| no-skills | 16 | 48 | 14 | 28 | **76** |

## What All Versions Get Right

- Correctly identify **`new HttpClient()` per call** as a production-grade risk.
- Flag **regex misuse** in hot paths (especially `LogAnalyzer`) and mention `[GeneratedRegex]`.
- Detect **O(n²) string concatenation** and recommend `StringBuilder`.
- Catch **JsonSerializerOptions caching** and **reflection metadata caching** opportunities.
- Provide largely actionable, .NET-specific remediation advice rather than generic “optimize” guidance.

## Summary: Impact of Skills

Most impactful differences, ranked:  
1. **Coverage depth for structural/systematic issues** (skills: full-codebase counts; baseline: partial spotlight).  
2. **Critical-path regex and allocation analysis precision** (skills: tighter quantification and startup/runtime split).  
3. **Priority framing and remediation ordering** (skills: clearer top-fix sequencing and impact statements).

Overall, **dotnet-perf-skills** delivers the stronger report (**98 vs 76 weighted**) by combining broader detection coverage with more consistent severity framing and highly targeted .NET fix guidance. **no-skills** remains useful and technically solid, but is less comprehensive and less consistent in prioritization.
