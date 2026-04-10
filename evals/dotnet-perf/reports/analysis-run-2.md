# Comparative Analysis: dotnet-perf-skills, no-skills

This run compares **2 configurations** on **1 shared scenario**: `analyze-perf-issues` under `output/{config}/run-2/analyze-perf-issues/`. Configuration mapping came from `gen-notes.md` plus directory naming: `dotnet-perf-skills` used the performance skill workflow, while `no-skills` is the baseline Copilot output.

## Executive Summary

| Dimension [Tier] | dotnet-perf-skills | no-skills |
|---|---:|---:|
| Regex Anti-Pattern Detection [CRITICAL] | 5 | 4 |
| String Allocation Detection [CRITICAL] | 5 | 4 |
| Collection and LINQ Efficiency [CRITICAL] | 5 | 4 |
| Async and IO Pattern Detection [CRITICAL] | 5 | 4 |
| Reflection and Serialization Overhead [HIGH] | 4 | 4 |
| Structural Optimization Detection [HIGH] | 5 | 3 |
| Severity Classification Accuracy [HIGH] | 3 | 4 |
| Fix Recommendation Quality [HIGH] | 5 | 4 |
| Evidence & Quantification Rigor [MEDIUM] | 5 | 4 |

## 1. Regex Anti-Pattern Detection [CRITICAL]

**dotnet-perf-skills** (`output/dotnet-perf-skills/run-2/analyze-perf-issues/performance-analysis.md`):
> | `RegexOptions.Compiled` | 48 | All in MarkdownStripper.cs |  
> | `[GeneratedRegex]` | 0 | None — 0/48 compiled regex use source-gen |  
> **Impact:** `new Regex()` ... In `LogAnalyzer.TryParseLine`, this happens **per log line**  

**no-skills** (`output/no-skills/run-2/analyze-perf-issues/performance-analysis.md`):
> **Regex per-line allocation** in `LogAnalyzer.TryParseLine` — new `Regex` on every log line  
> **40+ `RegexOptions.Compiled`** instances in `MarkdownStripper` — excessive JIT startup cost  
> **Missing `[GeneratedRegex]`** — .NET 8 project should use source-generated regex  

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is best due to exact counts and inverse checks (`0/48`), not just qualitative flags.

## 2. String Allocation Detection [CRITICAL]

**dotnet-perf-skills**:
> String `+=` Concatenation in Loops (~10 methods)  
> In `CsvParser.ParseLine`, this is **character-by-character** concatenation ... catastrophically slow  
> `.ToLower()`/`.ToUpper()` Without Culture (15 instances)  

**no-skills**:
> **Character-by-character string concatenation** ... `current += line[i]` ... catastrophically slow  
> `.ToLower()` / `.ToUpper()` Without Culture (appears in 6/10 files)  
> **Long chain of `.Replace()` calls** ... 40+ sequential Replace calls  

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is slightly stronger by combining broader counts with clearer cross-cutting prioritization.

## 3. Collection and LINQ Efficiency [CRITICAL]

**dotnet-perf-skills**:
> `ContainsKey` (double-lookup) | 12 | Across 5 files  
> `List.Contains` (O(n) in loop) | 3 | SlugGenerator, JsonTransformer  
> `.ToList()` (potentially unnecessary) | 20 | Across 6 files  

**no-skills**:
> `existingSlugs.ToList()` + `.Contains()` in a while loop ... Should use `HashSet<string>`  
> `Skip(i).Take(5).ToList()` in a loop — O(n²) allocations for sliding window  
> `ContainsKey` + indexer ... Use `TryGetValue` for a single lookup  

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** wins on consistency and breadth; both catch the major hotspots.

## 4. Async and IO Pattern Detection [CRITICAL]

**dotnet-perf-skills**:
> `new HttpClient(` | 3 | All in NotificationService  
> Sequential Async Awaits in Loop ... `SendBatchAsync`  
> Unbounded Parallelism ... `SendBatchParallelAsync`  
> Missing `CancellationToken` in Async Methods  

**no-skills**:
> `new HttpClient()` per call ... causes **socket exhaustion**  
> Sequential `await` in `SendBatchAsync` loop  
> Unbounded parallelism in `SendBatchParallelAsync`  
> `Task.Delay` without `CancellationToken`  

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is more complete on cancellation propagation and end-to-end async API design.

## 5. Reflection and Serialization Overhead [HIGH]

**dotnet-perf-skills**:
> Uncached Reflection — `GetProperties()`/`SetValue()`/`GetValue()`  
> Reflection is ~100× slower than direct property access  
> Uncached `new JsonSerializerOptions` Per Call (4 instances)  

**no-skills**:
> `typeof(TTarget).GetProperties()` on every `MapTo<T>` call  
> `prop.SetValue()` / `prop.GetValue()` in loops  
> `new JsonSerializerOptions { WriteIndented = true }` on every call  

**Score:** dotnet-perf-skills **4/5**, no-skills **4/5**.  
**Verdict:** **Tie** — both are strong and actionable; neither deeply pushes partial JSON parsing (`Utf8JsonReader`) opportunities.

## 6. Structural Optimization Detection [HIGH]

**dotnet-perf-skills**:
> `sealed class` | 0 | 0 of 18 classes sealed  
> Unsealed classes | 18 | All classes are unsealed  
> `: IEquatable` | 0 | 0 of 2 structs implement it  
> Static `readonly Dictionary<>` — FrozenDictionary Candidates (2 instances)  

**no-skills**:
> Unsealed Classes (appears in 3/10 files)  
> Files: `ValidationEngine.ValidationResult`, `EntityMapper.MappingConfig`, `DataPipeline.Record`  
> `ValidationError` struct without `IEquatable<ValidationError>`  

**Score:** dotnet-perf-skills **5/5**, no-skills **3/5**.  
**Verdict:** **dotnet-perf-skills** is clearly better; baseline under-detects unsealed-class scope (3 files vs full 18-class inventory).

## 7. Severity Classification Accuracy [HIGH]

**dotnet-perf-skills**:
> 🔴 Critical | 7 findings (~35 instances) ...  
> #### 6. `ContainsKey` + Indexer Double-Lookup (12 instances)  
> **Impact:** ... ~2× slower per lookup  

**no-skills**:
> 🔴 Critical | 8  
> `ContainsKey` + indexer ... **ℹ️ Info**  
> Top priorities: socket exhaustion, regex-per-line allocation, O(n²) concatenation  

**Score:** dotnet-perf-skills **3/5**, no-skills **4/5**.  
**Verdict:** **no-skills** is better calibrated here; `dotnet-perf-skills` over-escalates some moderate issues (notably `ContainsKey`+indexer) into critical.

## 8. Fix Recommendation Quality [HIGH]

**dotnet-perf-skills**:
> Fix: Inject `IHttpClientFactory` or use a `static readonly HttpClient` with `SocketsHttpHandler.PooledConnectionLifetime`.  
> Convert all 48 patterns to `[GeneratedRegex]` on a `partial class`.  
> Replace with `TryGetValue`.  

**no-skills**:
> After — use `[GeneratedRegex]` on .NET 8+  
> After — throttle with `SemaphoreSlim`  
> Cache reflection metadata in `ConcurrentDictionary<Type, PropertyInfo[]>`  

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** has more consistently precise API-level guidance and stronger prioritization framing.

## 9. Evidence & Quantification Rigor [MEDIUM]

**dotnet-perf-skills**:
> Scan Execution Checklist  
> `RegexOptions.Compiled` | 48 ...  
> `[GeneratedRegex]` | 0 ...  
> Unsealed classes | 18 ...  

**no-skills**:
> Analysis ... revealed **52 performance findings**  
> Findings by File ... (detailed tables per file)  
> Regex Anti-Patterns (appears in 6/10 files)  

**Score:** dotnet-perf-skills **5/5**, no-skills **4/5**.  
**Verdict:** **dotnet-perf-skills** is more audit-friendly due to explicit inverse metrics and hit-list style evidence.

## Weighted Summary

Weights used: Critical ×3, High ×2, Medium ×1.

| Configuration | Critical subtotal | High subtotal | Medium subtotal | Total weighted score |
|---|---:|---:|---:|---:|
| dotnet-perf-skills | 60 | 34 | 5 | **99** |
| no-skills | 48 | 30 | 4 | **82** |

## What All Versions Get Right

- Both identify the top production-risk issues: per-call `HttpClient`, per-call regex creation in log parsing, and O(n²) string concatenation.
- Both recommend modern .NET fixes (`[GeneratedRegex]`, `StringBuilder`, `HashSet`, `TryGetValue`, cached `JsonSerializerOptions`).
- Both provide concrete, file/line-grounded findings rather than generic advice.
- Both include prioritization sections that make remediation sequencing possible.

## Summary: Impact of Skills

The largest skill-driven gains are: **(1)** structural coverage depth (full-class inventory vs partial), **(2)** regex/anti-pattern quantification rigor (`0/48`, `0/18`, checklist format), and **(3)** recommendation precision consistency. The baseline is still strong and slightly better on one dimension (severity calibration), but overall weighted performance is decisively higher for **dotnet-perf-skills (99 vs 82)**, making it the better output for comprehensive performance triage.
