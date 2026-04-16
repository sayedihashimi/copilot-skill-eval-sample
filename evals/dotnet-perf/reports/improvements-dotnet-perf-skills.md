# Improvement Suggestions: dotnet-perf-skills

## Executive Summary
`dotnet-perf-skills` ranked first overall (98.7/105, 94%) and outperformed baseline on most high-value dimensions, but it has four concrete gaps: **Token Efficiency** (1.7 vs 5.0 baseline), occasional misses in **Collection and LINQ Efficiency** (run-3: 4 vs 5), occasional misses in **Reflection and Serialization Overhead** (run-3: 4 vs 5), and unstable **Severity Classification Accuracy** (run-3: 3 vs 4 baseline).

Top opportunities are: (1) add explicit detection recipes for rubric-specific LINQ anti-patterns, (2) add explicit full-deserialization detection guidance, (3) tighten severity calibration around moderate dictionary/LINQ issues, and (4) enforce concise output controls to recover token efficiency without losing signal.

## Current Performance Snapshot
| Dimension | Tier | dotnet-perf-skills (mean) | no-skills (mean) | Gap/Trail |
|---|---|---:|---:|---|
| Regex Anti-Pattern Detection | CRITICAL ×3 | 5.0 | 4.3 | ✅ leads |
| String Allocation Detection | CRITICAL ×3 | 5.0 | 4.3 | ✅ leads |
| Collection and LINQ Efficiency | CRITICAL ×3 | 4.7 | 4.7 | ⚠ tied mean; lost run-3 (4 vs 5) |
| Async and IO Pattern Detection | CRITICAL ×3 | 5.0 | 4.7 | ✅ leads |
| Reflection and Serialization Overhead | HIGH ×2 | 4.7 | 4.7 | ⚠ tied mean; lost run-3 (4 vs 5) |
| Structural Optimization Detection | HIGH ×2 | 5.0 | 4.0 | ✅ leads |
| Severity Classification Accuracy | HIGH ×2 | 4.3 | 3.7 | ⚠ unstable; lost run-3 (3 vs 4) |
| Fix Recommendation Quality | HIGH ×2 | 5.0 | 4.0 | ✅ leads |
| Token Efficiency | MEDIUM ×1 | 1.7 | 5.0 | ❌ major trailing gap |

## Improvement Suggestions

### 1. Add rubric-specific LINQ hotspot recipes (Skip/Take/ToList, Distinct/ToList, key-union materialization)
- **Dimensions affected**: Collection and LINQ Efficiency (CRITICAL), Severity Classification Accuracy (HIGH)
- **Current score → Target score**: 4.7 → 5.0
- **Problem**: Analysis says baseline explicitly called out the rubric’s `Skip().Take().ToList()` sliding window and related materialization patterns, while `dotnet-perf-skills` missed this specificity in at least one run.
- **Root cause**: `collections-and-linq.md` detection recipes are broad (`Select|Where|Cast|Take|Aggregate`) but do not explicitly search for the exact anti-patterns the evaluation rubric rewards.
- **Suggested changes**:

  **Before** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\collections-and-linq.md`):
  ```md
  ## Detection
  
  Scan recipes for collection and LINQ anti-patterns. Run these and report exact counts.
  
  ```bash
  # Static Dictionary not using FrozenDictionary (read-only after init)
  grep -rn --include='*.cs' 'static readonly Dictionary<' --exclude-dir=bin --exclude-dir=obj . | wc -l
  
  # Static FrozenDictionary (already optimized — verify the inverse)
  grep -rn --include='*.cs' 'static readonly FrozenDictionary<' --exclude-dir=bin --exclude-dir=obj . | wc -l
  
  # Per-call List allocation (inside method bodies, not static/readonly fields)
  grep -rn --include='*.cs' 'new List<' --exclude-dir=bin --exclude-dir=obj . | grep -v 'static\|readonly' | wc -l
  
  # Per-call Dictionary allocation (inside method bodies, not static/readonly fields)
  grep -rn --include='*.cs' 'new Dictionary<' --exclude-dir=bin --exclude-dir=obj . | grep -v 'static\|readonly' | wc -l
  
  # StringComparer.CurrentCulture usage (almost always wrong in library code — use Ordinal)
  grep -rn --include='*.cs' 'StringComparer.CurrentCulture' --exclude-dir=bin --exclude-dir=obj . | wc -l
  
  # LINQ chains in extension/hot-path files (.Select, .Where, .Cast, .Take, .Aggregate)
  grep -rn --include='*.cs' -E '\.(Select|Where|Cast|Take|Aggregate)\(' --exclude-dir=bin --exclude-dir=obj . | wc -l
  ```
  ```

  **After** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\collections-and-linq.md`):
  ```md
  ## Detection
  
  Scan recipes for collection and LINQ anti-patterns. Run these and report exact counts.
  
  ```bash
  # Static Dictionary not using FrozenDictionary (read-only after init)
  grep -rn --include='*.cs' 'static readonly Dictionary<' --exclude-dir=bin --exclude-dir=obj . | wc -l
  
  # Static FrozenDictionary (already optimized — verify the inverse)
  grep -rn --include='*.cs' 'static readonly FrozenDictionary<' --exclude-dir=bin --exclude-dir=obj . | wc -l
  
  # Per-call List allocation (inside method bodies, not static/readonly fields)
  grep -rn --include='*.cs' 'new List<' --exclude-dir=bin --exclude-dir=obj . | grep -v 'static\|readonly' | wc -l
  
  # Per-call Dictionary allocation (inside method bodies, not static/readonly fields)
  grep -rn --include='*.cs' 'new Dictionary<' --exclude-dir=bin --exclude-dir=obj . | grep -v 'static\|readonly' | wc -l
  
  # StringComparer.CurrentCulture usage (almost always wrong in library code — use Ordinal)
  grep -rn --include='*.cs' 'StringComparer.CurrentCulture' --exclude-dir=bin --exclude-dir=obj . | wc -l
  
  # LINQ chains in extension/hot-path files (.Select, .Where, .Cast, .Take, .Aggregate)
  grep -rn --include='*.cs' -E '\.(Select|Where|Cast|Take|Aggregate)\(' --exclude-dir=bin --exclude-dir=obj . | wc -l
  
  # Sliding-window materialization (Skip/Take/ToList in loops)
  grep -rn --include='*.cs' -E '\.Skip\([^)]*\)\.Take\([^)]*\)\.ToList\(' --exclude-dir=bin --exclude-dir=obj . | wc -l
  
  # Dedup materialization (.Distinct().ToList())
  grep -rn --include='*.cs' -E '\.Distinct\(\)\.ToList\(' --exclude-dir=bin --exclude-dir=obj . | wc -l
  
  # Key-union allocation pattern (Keys.ToList + Contains in diff/merge code)
  grep -rn --include='*.cs' -E 'Keys\.ToList\(|\.Contains\(' --exclude-dir=bin --exclude-dir=obj . | wc -l
  ```
  ```

- **Expected impact**: Improves recall on rubric-targeted collection/LINQ patterns in hot paths; likely +0.3 on this CRITICAL dimension (weighted +0.9).

### 2. Add explicit “full deserialization when partial parse is enough” detection
- **Dimensions affected**: Reflection and Serialization Overhead (HIGH)
- **Current score → Target score**: 4.7 → 5.0
- **Problem**: In run-3, baseline got credit for identifying unnecessary full deserialization paths; `dotnet-perf-skills` did not consistently surface that pattern.
- **Root cause**: `io-and-serialization.md` only includes detection recipes for `new HttpClient(` and `new JsonSerializerOptions`, with no explicit recipe/guidance for full-deserialization overhead.
- **Suggested changes**:

  **Before** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\io-and-serialization.md`):
  ```md
  ## Detection
  
  Scan recipes for I/O and serialization anti-patterns. Run these and report exact counts.
  
  ```bash
  # new HttpClient() (socket exhaustion risk)
  grep -rn --include='*.cs' 'new HttpClient(' --exclude-dir=bin --exclude-dir=obj . | wc -l
  
  # new JsonSerializerOptions() not cached (592x slower in .NET 6)
  grep -rn --include='*.cs' 'new JsonSerializerOptions' --exclude-dir=bin --exclude-dir=obj . | grep -v 'static\|readonly' | wc -l
  ```
  
  ### Patterns Requiring Manual Review
  
  - **`JsonSerializer.Serialize/Deserialize` without source-gen context**: Can't determine from grep if a context parameter is passed
  ```

  **After** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\io-and-serialization.md`):
  ```md
  ## Detection
  
  Scan recipes for I/O and serialization anti-patterns. Run these and report exact counts.
  
  ```bash
  # new HttpClient() (socket exhaustion risk)
  grep -rn --include='*.cs' 'new HttpClient(' --exclude-dir=bin --exclude-dir=obj . | wc -l
  
  # new JsonSerializerOptions() not cached (592x slower in .NET 6)
  grep -rn --include='*.cs' 'new JsonSerializerOptions' --exclude-dir=bin --exclude-dir=obj . | grep -v 'static\|readonly' | wc -l
  
  # Potential full-deserialization hot paths
  grep -rn --include='*.cs' -E 'JsonSerializer\.Deserialize<|JsonDocument\.Parse\(' --exclude-dir=bin --exclude-dir=obj . | wc -l
  ```
  
  ### Patterns Requiring Manual Review
  
  - **`JsonSerializer.Serialize/Deserialize` without source-gen context**: Can't determine from grep if a context parameter is passed
  - **Full deserialization where partial parse is enough**: For paths that only read 1-2 fields (logging/pretty-print/diff/validation), prefer `Utf8JsonReader` or `JsonDocument` property-level access over full object materialization
  ```

- **Expected impact**: Better completeness on serialization-path overhead; likely +0.3 on this HIGH dimension (weighted +0.6).

### 3. Recalibrate severity guidance for `ContainsKey + indexer` and similar moderate hot-path patterns
- **Dimensions affected**: Severity Classification Accuracy (HIGH), Collection and LINQ Efficiency (CRITICAL)
- **Current score → Target score**: 4.3 → 5.0
- **Problem**: Analysis explicitly noted over-escalation risk: `ContainsKey + indexer` was sometimes treated as critical, while baseline treated it as moderate.
- **Root cause**: The pattern is framed as 🔴 in `critical-patterns.md`, which conflicts with Step 4 criteria in `SKILL.md` (`>10x` or correctness failures for critical).
- **Suggested changes**:

  **Before** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\critical-patterns.md`):
  ```md
  ### Use TryGetValue Instead of ContainsKey + Indexer
  🔴 **DO** | .NET Core+
  
  ❌
  ```csharp
  if (dict.ContainsKey(key))
      Use(dict[key]);
  ```
  ✅
  ```csharp
  if (dict.TryGetValue(key, out var value))
      Use(value);
  ```
  **Impact: ~2x faster (50% reduction in lookup time).**
  ```

  **After** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\critical-patterns.md`):
  ```md
  ### Use TryGetValue Instead of ContainsKey + Indexer
  🟡 **DO** | .NET Core+
  
  ❌
  ```csharp
  if (dict.ContainsKey(key))
      Use(dict[key]);
  ```
  ✅
  ```csharp
  if (dict.TryGetValue(key, out var value))
      Use(value);
  ```
  **Impact: ~2x faster (50% reduction in lookup time). Treat as 🔴 only when this is in a proven top hot path with very high call frequency.**
  ```

  **Before** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\SKILL.md`):
  ```md
  **Prioritization rules:**
  1. If the user identified hot-path code, elevate all findings in that code to their maximum severity
  2. If hot-path context is unknown, report 🔴 Critical findings unconditionally; report 🟡 Moderate findings with a note: _"Impactful if this code is on a hot path"_
  3. Never suggest micro-optimizations on code that is clearly not performance-sensitive
  ```

  **After** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\SKILL.md`):
  ```md
  **Prioritization rules:**
  1. If the user identified hot-path code, elevate findings in that code to their maximum severity
  2. If hot-path context is unknown, report 🔴 Critical findings unconditionally; report 🟡 Moderate findings with a note: _"Impactful if this code is on a hot path"_
  3. Never suggest micro-optimizations on code that is clearly not performance-sensitive
  4. Do **not** classify pure throughput micro-optimizations (~2x local speedups, e.g., `ContainsKey`+indexer → `TryGetValue`) as 🔴 unless they are in a demonstrated top hot path or have multiplicative system impact
  ```

- **Expected impact**: Better severity consistency and reduced run-to-run variance; likely +0.7 on this HIGH dimension (weighted +1.4).

### 4. Add explicit token-budget mode and make scan checklist compact by default
- **Dimensions affected**: Token Efficiency (MEDIUM), with guardrails for Fix Recommendation Quality (HIGH)
- **Current score → Target score**: 1.7 → 4.0+
- **Problem**: Token usage is +130% input vs baseline and Token Efficiency score is 1.7; current workflow requires exhaustive checklist output and broad full-file reads.
- **Root cause**: `SKILL.md` currently mandates full-file reads for `<500` lines and a full recipe checklist before classification, which drives verbose outputs.
- **Suggested changes**:

  **Before** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\SKILL.md`):
  ```md
  **For files under 500 lines, read the entire file first** — you'll spot most patterns faster than running individual grep recipes. Use grep to confirm counts and catch patterns you might miss visually.
  
  For each relevant pattern category, run the detection recipes below. Report exact counts, not estimates.
  ...
  **Rules:**
  - Run every relevant recipe for the detected pattern categories
  - **Emit a scan execution checklist** before classifying findings — list each recipe and the hit count
  - A result of **0 hits** is valid and valuable (confirms good practice)
  - If reference files were loaded, also run their `## Detection` recipes
  ```

  **After** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\SKILL.md`):
  ```md
  For each relevant pattern category, run targeted detection recipes and report exact counts.
  
  Prefer targeted searches first; only read entire files when a pattern requires multi-line/manual confirmation.
  ...
  **Rules:**
  - Run relevant recipes for detected pattern categories
  - Emit a **compact scan checklist** (one line per category with aggregated counts), not a full command-by-command dump
  - A result of **0 hits** is valid and valuable (confirms good practice)
  - If reference files were loaded, prioritize their highest-signal recipes; include lower-signal recipes only when needed to confirm ambiguous findings
  ```

  **Before** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\SKILL.md`):
  ```md
  **Rules for compact output:**
  - **No ❌/✅ code blocks** for trivial fixes (adding a keyword, parameter, or type change). A one-line fix description suffices.
  - **Only include code blocks** for non-obvious transformations (e.g., replacing a LINQ chain with a foreach loop, or hoisting a closure).
  - **File locations as inline comma-separated list**, not a table. Use `File.cs:L42` format.
  - **No explanatory prose** beyond the Impact line — the severity icon already conveys urgency.
  - **Merge related findings** that share the same fix (e.g., all `.ToLower()` calls go in one finding, not split by file).
  - **Positive findings** in a bullet list, not a table. One line per pattern: `✅ Pattern — evidence`.
  ```

  **After** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\SKILL.md`):
  ```md
  **Rules for compact output:**
  - **No ❌/✅ code blocks** for trivial fixes (adding a keyword, parameter, or type change). A one-line fix description suffices.
  - **Only include code blocks** for non-obvious transformations (e.g., replacing a LINQ chain with a foreach loop, or hoisting a closure).
  - **File locations as inline comma-separated list**, not a table. Use `File.cs:L42` format.
  - **No explanatory prose** beyond the Impact line — the severity icon already conveys urgency.
  - **Merge related findings** that share the same fix (e.g., all `.ToLower()` calls go in one finding, not split by file).
  - **Positive findings** in a bullet list, not a table. One line per pattern: `✅ Pattern — evidence`.
  - Cap output to top-impact findings per severity (default: up to 5 🔴, 8 🟡, 5 ℹ️) and summarize additional matches in one line.
  ```

- **Expected impact**: Largest improvement opportunity in a weak dimension; realistic +2 to +3 in Token Efficiency (weighted +2 to +3) while preserving core quality.

## Summary of Recommended Changes
| File | Change summary |
|---|---|
| `dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\collections-and-linq.md` | Add explicit recipes for `Skip().Take().ToList()`, `.Distinct().ToList()`, and key-union materialization patterns. |
| `dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\io-and-serialization.md` | Add full-deserialization detection recipe and manual-review guidance for partial parsing alternatives. |
| `dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\critical-patterns.md` | Reclassify `ContainsKey + indexer` from default critical to moderate with conditional escalation guidance. |
| `dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\SKILL.md` | Tighten severity calibration rule and introduce compact checklist/token-budget output behavior. |

## Risks and Trade-offs
- Tightening severity calibration can under-prioritize micro-optimizations in genuinely ultra-hot paths unless the “demonstrated hot path” clause is consistently applied.
- Token caps can hide long-tail findings; mitigate by summarizing extra matches and allowing a “comprehensive mode” override.
- Additional detection recipes may increase scan time slightly; this is offset by replacing verbose checklist output with compact category-level summaries.
