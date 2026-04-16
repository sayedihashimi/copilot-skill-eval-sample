# Improvement Suggestions: dotnet-perf-skills

## Executive Summary
`dotnet-perf-skills` performed strongly overall (90.7/105 mean weighted score, rank #2), but it trails the top configuration in the highest-weight detection dimensions where breadth and consistency matter most: **Collection/LINQ**, **Async/IO**, **Structural**, and slightly in **Reflection/Serialization**. It also has a major efficiency gap vs baseline in **Token Efficiency** (2.0 vs 5.0).

Top opportunities:
1. Add explicit detection rules for async production anti-patterns currently under-covered (sequential awaits, unbounded fan-out, cancellation propagation).
2. Expand collection/LINQ recipes to detect concrete hot-path allocation patterns used in the eval (List.Contains lookups, materialization chains, sliding windows).
3. Add explicit reflection/serialization and structural “absence pattern” census rules (IEquatable, FrozenDictionary candidates) to improve consistency to 5/5.
4. Introduce a concise reporting mode to reduce token overhead without losing critical findings.

## Current Performance Snapshot

| Dimension | no-skills | dotnet-perf-skills | dotnet-perf-skills-improved | Gap vs top config |
|---|---:|---:|---:|---:|
| Regex Anti-Pattern Detection | 4.0 | **4.7** | **5.0** | -0.3 |
| String Allocation Detection | 4.3 | **4.7** | **5.0** | -0.3 |
| Collection and LINQ Efficiency | 3.7 | **4.3** | **5.0** | -0.7 |
| Async and IO Pattern Detection | 4.0 | **4.3** | **5.0** | -0.7 |
| Reflection and Serialization Overhead | 4.0 | **4.0** | **4.3** | -0.3 |
| Structural Optimization Detection | 3.3 | **4.3** | **5.0** | -0.7 |
| Severity Classification Accuracy | 3.0 | **4.3** | 4.0 | +0.3 (strength) |
| Fix Recommendation Quality | 3.3 | **4.7** | 4.3 | +0.4 (strength) |
| Token Efficiency | **5.0** | 2.0 | 2.0 | 0.0 vs top, -3.0 vs baseline |

## Improvement Suggestions

### 1. Add explicit async/IO anti-pattern recipes used in production incident triage
- **Dimensions affected**: Async and IO Pattern Detection (CRITICAL), Severity Classification Accuracy (HIGH), Fix Recommendation Quality (HIGH)
- **Current score → Target score**: 4.3 → 5.0
- **Problem**: Analysis says dotnet-perf-skills catches HttpClient misuse and batch issues, but improved is “best for explicit, complete async/IO anti-pattern coverage.” Missing explicit recipes reduce consistency.
- **Root cause**: `async-patterns.md` focuses on generic async guidance (Task.Run wrappers, ValueTask, channels, false sharing) but not the concrete patterns in this eval dimension (sequential awaits, unbounded parallelism, cancellation propagation, uncancellable Task.Delay).
- **Suggested changes**:

  **Before** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\async-patterns.md`):
  ```md
  ### Don't Expose Async Wrappers for Sync Methods
  🟡 **AVOID** wrapping sync methods with `Task.Run` in libraries | .NET Core+
  ...
  ### Use Channels for Producer/Consumer
  🟡 **DO** use `System.Threading.Channels` for producer-consumer patterns | .NET Core 3.0+
  ...
  ## Detection
  ...
  # async void methods (correctness issue — crashes on exception)
  grep -rn --include='*.cs' 'async void' --exclude-dir=bin --exclude-dir=obj . | grep -v 'event' | wc -l
  ```

  **After** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\async-patterns.md`):
  ```md
  ### Reuse HttpClient Instances (no per-call construction)
  🔴 **DO** use `IHttpClientFactory` or shared/static `HttpClient` | .NET Core 2.1+
  
  ### Avoid Sequential Awaits in Throughput Loops
  🔴 **AVOID** `await` per-item in loops when calls are independent | .NET Core+
  
  ### Bound Parallel Fan-Out
  🔴 **DO** use `SemaphoreSlim` or `Parallel.ForEachAsync` with `MaxDegreeOfParallelism` | .NET 6+
  
  ### Propagate CancellationToken End-to-End
  🟡 **DO** add/pass `CancellationToken` through async APIs and retries | .NET Core+
  
  ### Use Cancellable Delays
  🟡 **DO** pass `CancellationToken` to `Task.Delay(...)` in retry/backoff loops | .NET Core+
  
  ## Detection
  ```bash
  # Per-call HttpClient construction
  grep -rn --include='*.cs' 'new HttpClient(' --exclude-dir=bin --exclude-dir=obj . | wc -l
  
  # Sequential await in loops (manual review for dependency constraints)
  grep -rn --include='*.cs' -E 'for(each)?\s*\(.*\).*await ' --exclude-dir=bin --exclude-dir=obj . | wc -l
  
  # Unbounded fan-out
  grep -rn --include='*.cs' -E 'Task\.WhenAll|Select\(.*async' --exclude-dir=bin --exclude-dir=obj . | wc -l
  
  # Missing cancellation token usage in async signatures
  grep -rn --include='*.cs' -E 'async Task|ValueTask' --exclude-dir=bin --exclude-dir=obj . | wc -l
  
  # Uncancellable delay
  grep -rn --include='*.cs' 'Task.Delay(' --exclude-dir=bin --exclude-dir=obj . | wc -l
  ```
  ```

- **Expected impact**: +0.7 in a CRITICAL tier dimension (high weighted gain), plus better run-to-run consistency.

### 2. Expand collection/LINQ detection to include the exact hot-path allocation patterns in the eval
- **Dimensions affected**: Collection and LINQ Efficiency (CRITICAL), String Allocation Detection (CRITICAL), Severity Classification Accuracy (HIGH)
- **Current score → Target score**: 4.3 → 5.0
- **Problem**: dotnet-perf-skills is “good but less complete” than improved; improved wins due to broader materialization and complexity framing.
- **Root cause**: Current recipes emphasize broad LINQ presence and collection allocations, but under-specify concrete anti-patterns evaluated here (`List.Contains` lookup misuse, `.Distinct().ToList()`, `.Skip().Take().ToList()` sliding windows, eager `.ToList()` materialization chains).
- **Suggested changes**:

  **Before** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\collections-and-linq.md`):
  ```md
  ## Detection
  
  # Static Dictionary not using FrozenDictionary (read-only after init)
  grep -rn --include='*.cs' 'static readonly Dictionary<' --exclude-dir=bin --exclude-dir=obj . | wc -l
  ...
  # LINQ chains in extension/hot-path files (.Select, .Where, .Cast, .Take, .Aggregate)
  grep -rn --include='*.cs' -E '\.(Select|Where|Cast|Take|Aggregate)\(' --exclude-dir=bin --exclude-dir=obj . | wc -l
  ```

  **After** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\collections-and-linq.md`):
  ```md
  ## Detection
  
  # O(n) lookup patterns
  grep -rn --include='*.cs' -E 'List<.*>\.Contains\(|\.Contains\(' --exclude-dir=bin --exclude-dir=obj . | wc -l
  
  # Dictionary double-lookup
  grep -rn --include='*.cs' 'ContainsKey(' --exclude-dir=bin --exclude-dir=obj . | wc -l
  
  # Eager materialization and chained allocations
  grep -rn --include='*.cs' -E '\.ToList\(\)|\.Distinct\(\)\.ToList\(\)|\.Skip\(.+\)\.Take\(.+\)\.ToList\(\)' --exclude-dir=bin --exclude-dir=obj . | wc -l
  
  # Repeated list growth without capacity hints
  grep -rn --include='*.cs' -E 'new List<|EnsureCapacity\(' --exclude-dir=bin --exclude-dir=obj . | wc -l
  
  # Frozen collection candidates
  grep -rn --include='*.cs' 'static readonly Dictionary<' --exclude-dir=bin --exclude-dir=obj . | wc -l
  grep -rn --include='*.cs' 'FrozenDictionary<' --exclude-dir=bin --exclude-dir=obj . | wc -l
  ```
  ```

- **Expected impact**: +0.7 in a CRITICAL tier dimension, and better capture of high-impact hot-path complexity issues.

### 3. Add mandatory reflection/serialization and structural “inverse ratio” checks to Step 2/3 workflow
- **Dimensions affected**: Reflection and Serialization Overhead (HIGH), Structural Optimization Detection (HIGH)
- **Current score → Target score**: Reflection 4.0 → 5.0; Structural 4.3 → 5.0
- **Problem**: Reflection/serialization is only tied baseline and slightly below improved; structural is good but not as systematic as improved’s full census.
- **Root cause**: `SKILL.md` signal table and core recipes are broad, but the workflow does not force explicit ratio-based checks for: uncached reflection accessor use, per-call serializer options, `IEquatable<T>` coverage on structs, and FrozenDictionary candidate ratios.
- **Suggested changes**:

  **Before** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\SKILL.md`):
  ```md
  | Signal in Code | Topic |
  |----------------|-------|
  | `JsonSerializer`, `HttpClient`, `Stream`, `FileStream` | I/O & serialization |
  
  Always check structural patterns (unsealed classes) regardless of signals.
  ```

  **After** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\SKILL.md`):
  ```md
  | Signal in Code | Topic |
  |----------------|-------|
  | `JsonSerializer`, `JsonSerializerOptions`, `GetProperties(`, `SetValue(`, `GetValue(`, `HttpClient`, `Stream`, `FileStream` | I/O, reflection & serialization |
  
  Always run structural inverse checks regardless of signals:
  - Unsealed leaf classes ratio: `sealed` vs non-sealed
  - Struct equality ratio: structs implementing `IEquatable<T>` vs total structs used in generic collections
  - Frozen collections ratio: `static readonly Dictionary/HashSet` vs `FrozenDictionary/FrozenSet`
  ```

  **Before** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\structural-patterns.md`):
  ```md
  ### Seal Classes for Devirtualization
  ...
  **Scale-based severity:**
  - 1-10 unsealed leaf classes → ℹ️ Info
  - 11-50 unsealed leaf classes → 🟡 Moderate
  - 50+ unsealed leaf classes → 🟡 Moderate (elevated priority)
  ```

  **After** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\structural-patterns.md`):
  ```md
  ### Seal Classes for Devirtualization
  ...
  ### Implement IEquatable<T> on hot structs
  🟡 **DO** implement `IEquatable<T>` for structs used in dictionaries/sets or compared frequently.
  
  ### Replace read-only static Dictionary/HashSet with Frozen collections
  🟡 **DO** migrate immutable lookup tables to `FrozenDictionary/FrozenSet` on .NET 8+.
  
  ## Detection
  ```bash
  # Unsealed vs sealed class census
  grep -rn --include='*.cs' -E 'class ' --exclude-dir=bin --exclude-dir=obj . | wc -l
  grep -rn --include='*.cs' 'sealed class' --exclude-dir=bin --exclude-dir=obj . | wc -l
  
  # Struct equality census
  grep -rn --include='*.cs' -E 'struct ' --exclude-dir=bin --exclude-dir=obj . | wc -l
  grep -rn --include='*.cs' ': IEquatable<' --exclude-dir=bin --exclude-dir=obj . | wc -l
  
  # Frozen collection census
  grep -rn --include='*.cs' 'static readonly Dictionary<' --exclude-dir=bin --exclude-dir=obj . | wc -l
  grep -rn --include='*.cs' 'FrozenDictionary<' --exclude-dir=bin --exclude-dir=obj . | wc -l
  ```
  ```

- **Expected impact**: +0.3 (reflection) and +0.7 (structural) in HIGH tier dimensions.

### 4. Add concise output mode to reduce token inflation while preserving critical coverage
- **Dimensions affected**: Token Efficiency (MEDIUM), Collection/Async consistency (indirect)
- **Current score → Target score**: 2.0 → 3.5+ (practical target)
- **Problem**: Token use is +114.9% vs baseline and scored 2.0 in every run; this reduces operational efficiency.
- **Root cause**: The workflow currently mandates verbose artifacts even when not needed (full scan execution checklist, extensive findings formatting), and the agent enforces a two-pass structure by default.
- **Suggested changes**:

  **Before** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\SKILL.md`):
  ```md
  - **Emit a scan execution checklist** before classifying findings — list each recipe and the hit count
  ...
  End with a summary table and disclaimer:
  ```

  **After** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\SKILL.md`):
  ```md
  - Emit a scan execution checklist only when `scan depth = comprehensive` or when explicitly requested.
  - Default mode (`standard`) should report only: findings, exact counts for triggered patterns, and summary table.
  ...
  End with a summary table and disclaimer.
  ```

  **Before** (`dotnet-skills\plugins\dotnet-diag\agents\optimizing-dotnet-performance.agent.md`):
  ```md
  ## Two-Pass Analysis
  
  Every performance review uses two passes. Both are mandatory — do not skip Pass 2.
  ...
  1. Ask clarifying questions about workload, constraints, and what "slow" means
  ```

  **After** (`dotnet-skills\plugins\dotnet-diag\agents\optimizing-dotnet-performance.agent.md`):
  ```md
  ## Two-Pass Analysis
  
  Use two passes by default for open-ended consultations. For bounded code-audit tasks (explicit file set and fixed rubric), run a single merged pass to minimize token cost.
  ...
  1. Ask clarifying questions only when missing context blocks severity ranking or fix safety.
  ```

- **Expected impact**: Material token reduction while preserving (or improving) detection quality on rubric-based evaluations.

## Summary of Recommended Changes

| File | Change summary |
|---|---|
| `dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\async-patterns.md` | Add explicit production async/IO anti-pattern sections and grep recipes for sequential await, fan-out, cancellation, HttpClient lifetime. |
| `dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\collections-and-linq.md` | Add concrete hot-path detection recipes for List.Contains, Distinct/ToList materialization, sliding windows, and capacity hints. |
| `dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\SKILL.md` | Expand signal table for reflection patterns; require structural inverse-ratio checks; reduce default verbosity requirements. |
| `dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\structural-patterns.md` | Add IEquatable and Frozen collection checks plus explicit census detection commands. |
| `dotnet-skills\plugins\dotnet-diag\agents\optimizing-dotnet-performance.agent.md` | Allow single-pass bounded audits and conditional clarifying questions to improve token efficiency. |

## Risks and Trade-offs
- Tightening recipes around concrete patterns can increase false positives unless manual-review notes remain explicit (especially regex/grep-based “in loop” inference).
- Reducing checklist verbosity improves token efficiency, but if reduced too aggressively it may hide audit traceability; keep full checklist available in comprehensive mode.
- Expanding structural checks may bias toward style-like findings unless severity rules continue prioritizing hot-path, high-impact issues first.
