# Comparative Analysis: dotnet-perf-skills, no-skills

Two configurations were evaluated across one shared scenario (`analyze-perf-issues`) using `output\{config}\run-1\analyze-perf-issues\performance-analysis.md`. `dotnet-perf-skills` is explicitly identified via `gen-notes.md` as using the `analyzing-dotnet-performance` skill, while `no-skills` is baseline/default Copilot (from directory labeling and baseline-style gen notes).

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 5 |
| String Allocation Detection [CRITICAL] | 5 | 5 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 5 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 5 |
| Reflection and Serialization Overhead [HIGH] | 4 | 4 |
| Structural Optimization Detection [HIGH] | 4 | 5 |
| Aggregate and Replace Chain Detection [HIGH] | 5 | 5 |
| Span Usage Consistency [HIGH] | 5 | 5 |
| Inheritance Sealing Accuracy [HIGH] | 5 | 5 |
| Params Overload Optimization [MODERATE] | 4 | 5 |
| Severity Classification Accuracy [HIGH] | 5 | 4 |
| Fix Recommendation Quality [HIGH] | 5 | 4 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills** (`output\dotnet-perf-skills\run-1\analyze-perf-issues\performance-analysis.md`)
> \| `new Regex(` (uncached) \| 8 \| LogAnalyzer (4), TemplateEngine (2), CsvParser (1), ValidationEngine (1) \|  
> \| `RegexOptions.Compiled` \| 48 \| All in MarkdownStripper.cs \|  
> \| `[GeneratedRegex]` \| 0 \| None — 0/48 compiled regexes use source gen \|

**no-skills** (`output\no-skills\run-1\analyze-perf-issues\performance-analysis.md`)
> - **Per-call `new Regex(...)`** in `TemplateEngine`, `LogAnalyzer`, `SlugGenerator`, `ValidationEngine`, `CsvParser`  
> - **Excessive `RegexOptions.Compiled`** in `MarkdownStripper` (46 compiled regexes)  
> - **Missing `[GeneratedRegex]`** — none of the files use the .NET 7+ source generator

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**.  
**Verdict:** **Tie** — both catch per-call instantiation, compiled-regex overuse, and `[GeneratedRegex]` modernization.

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills** (`...dotnet-perf-skills...\performance-analysis.md`)
> #### 7. String concatenation (`+=`) in loops — O(n²) allocation (6 instances)  
> #### 9. `.ToLower()`/`.ToUpper()` without culture (17 instances)  
> #### 24. String concatenation for report/summary building (3 instances)

**no-skills** (`...no-skills...\performance-analysis.md`)
> - `+=` in loops across `UnitFormatter`, `TemplateEngine`, `CsvParser`, `LogAnalyzer`, `DataPipeline`, `NotificationService`, `ValidationEngine`  
> - Character-by-character `+=` in `CsvParser` is the worst offender  
> - **Fix pattern:** Use `StringBuilder` or `string.Join`/`string.Create`

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**.  
**Verdict:** **Tie** — both strongly cover loop concat, char-by-char building, and culture/casing concerns.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills** (`...dotnet-perf-skills...\performance-analysis.md`)
> #### 5. `ContainsKey` + indexer double-lookup pattern (~10 instances)  
> #### 21. `Skip(i).Take(5).ToList()` in a loop (1 instance)  
> #### 22. `.ToList()` + `.Contains()` for key set lookups — O(n) per lookup (1 instance)

**no-skills** (`...no-skills...\performance-analysis.md`)
> 27. **🟡 Moderate — `.ToList()` + `.Contains()` O(n) in loop (lines 75-81)**  
> 47. **🟡 Moderate — `Skip(i).Take(5).ToList()` in loop (line 157)**  
> - **Fix pattern:** Use `HashSet<T>` for lookup collections

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**.  
**Verdict:** **Tie** — both identify the required lookup and materialization anti-patterns well.

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills** (`...dotnet-perf-skills...\performance-analysis.md`)
> #### 1. `new HttpClient()` per call — socket exhaustion risk (3 instances)  
> #### 13. Sequential awaits in loop — no parallelism (1 instance)  
> #### 14. Unbounded parallelism — no throttling (1 instance)  
> #### 15. Missing `CancellationToken` on async methods (all async methods)

**no-skills** (`...no-skills...\performance-analysis.md`)
> 32. **🔴 Critical — `new HttpClient()` per call**  
> 35. **🟡 Moderate — Sequential awaits in loop**  
> 36. **🔴 Critical — Unbounded parallelism**  
> 34. **🟡 Moderate — `Task.Delay` without `CancellationToken`**

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**.  
**Verdict:** **Tie** — both outputs catch the full async/IO risk set.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills** (`...dotnet-perf-skills...\performance-analysis.md`)
> #### 4. Uncached `new JsonSerializerOptions` per call (4 instances)  
> #### 6. Uncached reflection `GetProperties()`/`GetProperty()`/`SetValue()` in hot paths (3 instances)  
> **Fix:** Cache `PropertyInfo[]` per type in a `ConcurrentDictionary<Type, PropertyInfo[]>`

**no-skills** (`...no-skills...\performance-analysis.md`)
> 60. **🟡 Moderate — `new JsonSerializerOptions` on every call**  
> 70. **🔴 Critical — Uncached `GetProperties()` reflection per call**  
> 71. **🟡 Moderate — Reflection `SetValue`/`GetValue` per property**

**Score:** dotnet-perf-skills **4/5**, no-skills **4/5**.  
**Verdict:** **Tie** — strong detection of uncached reflection/options, but neither deeply pushes partial-parse alternatives (`Utf8JsonReader`) for repeated full deserialization.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills** (`...dotnet-perf-skills...\performance-analysis.md`)
> #### 10. 0/26 non-abstract classes are sealed — 3 are true leaf classes  
> #### 26. 3 static readonly `Dictionary<>` — FrozenDictionary candidates  
> #### 27. 0/3 structs implement `IEquatable<T>` (3 instances)

**no-skills** (`...no-skills...\performance-analysis.md`)
> - `EnglishOrdinalizer`, `GermanOrdinalizer`, `SpanishOrdinalizer` (should be sealed)  
> - `DefaultOrdinalizer` and `Ordinalizer` are correctly unsealed (base classes)  
> - `Record`, `MappingConfig`, `ValidationResult` in service/model code

**Score:** dotnet-perf-skills **4/5**, no-skills **5/5**.  
**Verdict:** **no-skills wins** by explicitly broadening leaf-class candidates while still covering `IEquatable<T>`/FrozenDictionary-related structural patterns elsewhere.

## 7. Aggregate and Replace Chain Detection [HIGH]

**dotnet-perf-skills** (`...dotnet-perf-skills...\performance-analysis.md`)
> #### 11. `.Aggregate()` with `.Replace()` — 16 intermediate string allocations  
> #### 12. `char.ToString()` allocation in loop

**no-skills** (`...no-skills...\performance-analysis.md`)
> 2. **🟡 Moderate — `.Aggregate()` with `.Replace()` creates 16 intermediate strings**  
> 3. **ℹ️ Info — `char.ToString()` allocation (line 63)**

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**.  
**Verdict:** **Tie** — both detect the subtle compound allocation pattern and its per-iteration string allocation.

## 8. Span Usage Consistency [HIGH]

**dotnet-perf-skills** (`...dotnet-perf-skills...\performance-analysis.md`)
> #### 17. `.Substring()` allocations where `AsSpan` would suffice  
> #### 18. Cross-file inconsistency: `AsSpan` vs `Substring` in sibling truncators  
> #### 19. `value[..n].TrimEnd()` — double allocation

**no-skills** (`...no-skills...\performance-analysis.md`)
> 9. **🟡 Moderate — Double allocation: `value[..n].TrimEnd()`**  
> 11. **🟡 Moderate — Inconsistent Span usage: `Substring` vs `AsSpan`**  
> 8. **🟡 Moderate — `List<char>` instead of `ReadOnlySpan<char>` or arrays**

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**.  
**Verdict:** **Tie** — both identify double-allocation and inconsistent Span adoption across truncators.

## 9. Inheritance Sealing Accuracy [HIGH]

**dotnet-perf-skills** (`...dotnet-perf-skills...\performance-analysis.md`)
> **Files:** EnglishOrdinalizer..., GermanOrdinalizer..., SpanishOrdinalizer...  
> **Caveat:** ... `DefaultOrdinalizer` and `Ordinalizer` must NOT be sealed (they are base classes).

**no-skills** (`...no-skills...\performance-analysis.md`)
> `EnglishOrdinalizer`, `GermanOrdinalizer`, and `SpanishOrdinalizer` are leaf classes that should be `sealed`  
> `DefaultOrdinalizer` and `Ordinalizer` are correctly unsealed (base classes)

**Score:** dotnet-perf-skills **5/5**, no-skills **5/5**.  
**Verdict:** **Tie** — both are precise and avoid the dangerous base-class false positive.

## 10. Params Overload Optimization [MODERATE]

**dotnet-perf-skills** (`...dotnet-perf-skills...\performance-analysis.md`)
> #### 29. `params` methods without single-argument fast-path overloads (4 instances)  
> **Files:** TextTruncation.cs:L107,L117; NotificationService.cs:L196; JsonTransformer.cs:L56

**no-skills** (`...no-skills...\performance-analysis.md`)
> 13. **🟡 Moderate — `params` without single-argument fast-path (line 107)**  
> `TruncationPipeline.Apply` uses `params ITruncator[]` ...  
> **Fix:** Add a single-argument overload

**Score:** dotnet-perf-skills **4/5**, no-skills **5/5**.  
**Verdict:** **no-skills wins** on specificity; it explicitly calls out `TruncationPipeline.Apply(...)`.

## 11. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills** (`...dotnet-perf-skills...\performance-analysis.md`)
> | 🔴 Critical | 10 | `new HttpClient()` per call..., per-call `new Regex()`..., uncached `JsonSerializerOptions` |  
> | 🟡 Moderate | 26 | `+= string` in loops, `.ToLower()` without culture... |  
> | ℹ️ Info | 12 | Missing List capacity hints, `params` without fast-path overloads...

**no-skills** (`...no-skills...\performance-analysis.md`)
> 60. **🟡 Moderate — `new JsonSerializerOptions` on every call**  
> 56. **🔴 Critical — Character-by-character string concatenation**  
> 6. **ℹ️ Info — Unsealed leaf classes**

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills wins** with cleaner hot-path prioritization; no-skills is good but has a few debatable tier placements.

## 12. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills** (`...dotnet-perf-skills...\performance-analysis.md`)
> **Fix:** Inject `IHttpClientFactory` or use a single `static readonly HttpClient`...  
> **Fix:** ...use `[GeneratedRegex]` on .NET 8.  
> **Fix:** Use `StringComparison.OrdinalIgnoreCase`...  
> | 10 | Convert 3 static `Dictionary<>` to `FrozenDictionary` |

**no-skills** (`...no-skills...\performance-analysis.md`)
> | 1 | Replace `new HttpClient()` with `IHttpClientFactory` or static instance |  
> | 6 | Convert 46 `RegexOptions.Compiled` to `[GeneratedRegex]` |  
> | 8 | Replace `ContainsKey`+indexer with `TryGetValue` everywhere |

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills wins** on consistency and precision of API-level guidance (with clearer quantified impact framing throughout).

## Weighted Summary

Weights used: Critical ×3, High ×2, Moderate ×1.

| Configuration | Weighted Total |
|---|---:|
| dotnet-perf-skills | 130 |
| no-skills | 129 |

| Tier contribution | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Critical subtotal (max 60) | 60 | 60 |
| High subtotal (max 70) | 66 | 64 |
| Moderate subtotal (max 5) | 4 | 5 |

## What All Versions Get Right

- Both identify the highest-risk runtime failures: per-call `HttpClient`, per-call regex, and unbounded parallelism.
- Both detect the key memory/allocation hotspots: loop `+=`, Aggregate+Replace chains, and list-based O(n) lookup misuse.
- Both include concrete modern .NET fix patterns: `[GeneratedRegex]`, `TryGetValue`, `StringBuilder`, `HashSet<T>`, and cancellation propagation.
- Both correctly handle inheritance sealing precision by preserving base classes (`DefaultOrdinalizer`/`Ordinalizer`) as unsealed.

## Summary: Impact of Skills

Most impactful differences, ranked:
1. **Severity calibration and prioritization quality:** `dotnet-perf-skills` is slightly better at critical-vs-moderate placement and hot-path framing.
2. **Structural breadth and params specificity:** `no-skills` is slightly better at naming additional leaf-class candidates and explicitly calling out `TruncationPipeline.Apply` overload optimization.
3. **Overall actionable guidance quality:** both are strong, but `dotnet-perf-skills` is marginally more consistent in quantified, API-specific recommendations.

Overall assessment: both outputs are high quality and nearly tied; by weighted score, **dotnet-perf-skills (130)** narrowly leads **no-skills (129)** due to stronger high-tier prioritization consistency.
