# Improvement Suggestions: dotnet-perf-skills

## Executive Summary
`dotnet-perf-skills` is the top overall configuration (131.0 mean weighted score, 94% of max), but it has three clear gaps: **Token Efficiency** (1.6 vs baseline 5.0), **Inheritance Sealing Accuracy** (4.2 vs 4.8), and **Params Overload Optimization** (4.2 vs 4.4). The highest-impact work is mostly **skill-level** (tightening `analyzing-dotnet-performance` instructions), with secondary **plugin-level** packaging/documentation improvements.

Top opportunities:
1. Reduce verbosity and enforce output budgets in `analyzing-dotnet-performance` and the performance agent.
2. Add a hard “proof of leaf-ness” rule to prevent sealing base classes.
3. Make params guidance explicitly prefer single-argument overloads before `params ReadOnlySpan<T>`.
4. Add golden examples to stabilize responses and reduce over-generation.
5. Fill plugin packaging gaps (README/CHANGELOG/LICENSE + manifest metadata).

## Current Performance Snapshot

| Dimension | dotnet-perf-skills | no-skills | Delta vs baseline | Status |
|---|---:|---:|---:|---|
| Regex Anti-Pattern Detection | 5.0 | 4.4 | +0.6 | Leads |
| String Allocation Detection | 5.0 | 4.6 | +0.4 | Leads |
| Collection and LINQ Efficiency | 5.0 | 4.4 | +0.6 | Leads |
| Async and IO Pattern Detection | 5.0 | 4.6 | +0.4 | Leads |
| Reflection and Serialization Overhead | 4.4 | 4.0 | +0.4 | Leads |
| Structural Optimization Detection | 4.8 | 4.0 | +0.8 | Leads |
| Aggregate and Replace Chain Detection | 5.0 | 4.6 | +0.4 | Leads |
| Span Usage Consistency | 5.0 | 4.6 | +0.4 | Leads |
| **Inheritance Sealing Accuracy** | **4.2** | **4.8** | **-0.6** | **Trails** |
| **Params Overload Optimization** | **4.2** | **4.4** | **-0.2** | **Trails** |
| Severity Classification Accuracy | 4.4 | 3.6 | +0.8 | Leads |
| Fix Recommendation Quality | 4.8 | 4.0 | +0.8 | Leads |
| **Token Efficiency** | **1.6** | **5.0** | **-3.4** | **Major gap** |

## Plugin Structure Assessment

### dotnet-skills\plugins\dotnet-diag
- **Current inventory**: 7 skills, 1 agent, 0 hooks, 0 MCP servers.
- **Manifest quality**: has `name`, `version`, `description`, but missing `author`, `license`, and `keywords`.
- **Skill quality findings**:
  - `analyzing-dotnet-performance\SKILL.md` is instruction-dense and drives long outputs (matches token-efficiency underperformance).
  - Structural guidance exists but does not strongly force base-vs-leaf proof before seal recommendations.
  - Params guidance exists but is framed too generally (`params ReadOnlySpan<T>` emphasis), which likely caused misses on single-arg overload specificity.
- **Packaging/docs gaps**: no `README.md`, `CHANGELOG.md`, or root `LICENSE`.
- **Supporting files**: many `references/` files exist; however, `examples/` are missing in key skills.

## Improvement Suggestions

### Plugin-Level Improvements

#### P1. Add packaging metadata + usage docs to improve discoverability and safer activation
- **Type**: Manifest fix + Documentation
- **Dimensions affected**: Token Efficiency (indirect), Fix Recommendation Quality (consistency), Severity Classification Accuracy (consistency)
- **Problem**: Plugin lacks discoverability metadata and onboarding docs, making behavior less predictable across teams.
- **Suggested changes**:

  **Before** (`dotnet-skills\plugins\dotnet-diag\plugin.json`):
  ```json
  {
    "name": "dotnet-diag",
    "version": "0.1.0",
    "description": "Skills for .NET performance investigations, debugging, and incident analysis.",
    "skills": ["./skills/"],
    "agents": ["./agents/optimizing-dotnet-performance.agent.md"]
  }
  ```

  **After** (`dotnet-skills\plugins\dotnet-diag\plugin.json`):
  ```json
  {
    "name": "dotnet-diag",
    "version": "0.1.1",
    "description": "Skills for .NET performance investigations, debugging, and incident analysis. Use for production diagnostics, performance anti-pattern detection, and symbolication workflows.",
    "author": { "name": ".NET Diagnostics Team" },
    "license": "MIT",
    "keywords": ["dotnet", "performance", "diagnostics", "profiling", "tracing", "symbolication"],
    "skills": ["./skills/"],
    "agents": ["./agents/optimizing-dotnet-performance.agent.md"]
  }
  ```

  **New File** (`dotnet-skills\plugins\dotnet-diag\README.md`):
  ```md
  # dotnet-diag

  Reusable plugin for .NET diagnostics and performance workflows.

  ## Included skills
  - analyzing-dotnet-performance
  - dotnet-trace-collect
  - dump-collect
  - microbenchmarking
  - clr-activation-debugging
  - android-tombstone-symbolication
  - apple-crash-symbolication

  ## Included agent
  - optimizing-dotnet-performance

  ## Recommended usage
  1. Use `analyzing-dotnet-performance` for code anti-pattern scans.
  2. Use `dotnet-trace-collect` and `dump-collect` for runtime data collection.
  3. Use symbolication skills for platform crash artifacts.

  ## Output expectations
  - Prioritize high-impact findings.
  - Keep findings concise and actionable.
  - Include explicit guardrails when a recommendation can affect correctness.
  ```

  **New File** (`dotnet-skills\plugins\dotnet-diag\CHANGELOG.md`):
  ```md
  # Changelog

  ## 0.1.1
  - Added manifest metadata (author/license/keywords).
  - Added plugin README.
  - Added guidance updates for concise perf analysis and inheritance/params precision.
  ```

  **New File** (`dotnet-skills\plugins\dotnet-diag\LICENSE`):
  ```text
  MIT License
  ```

- **Expected impact**: Small direct score lift, medium consistency gain across runs; helps keep behavior predictable and reduces accidental broad activation.

#### P2. Add golden examples for the core performance skill
- **Type**: New supporting files
- **Dimensions affected**: Inheritance Sealing Accuracy, Params Overload Optimization, Severity Classification Accuracy, Token Efficiency
- **Problem**: No `examples/` in the main analysis skill means less anchoring for concise, precise outputs.
- **Suggested changes**:

  **New File** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\examples\analyze-perf-issues-golden.md`):
  ```md
  # Golden Example: analyze-perf-issues

  ## Input shape
  - Multi-file C# codebase with regex/string/collections/async/serialization patterns.

  ## Expected response shape
  1. Top findings only (max 12).
  2. Each finding: title, impact, files, fix.
  3. Explicit "do not seal base class" checks for inheritance findings.
  4. Explicit single-arg overload recommendation for params findings.
  5. Summary table with severity counts.

  ## Mandatory accuracy checks
  - If class has subclasses in repo, classify as base and do not recommend sealing.
  - If params method is typically called with one argument, recommend a concrete one-arg overload first.
  ```

- **Expected impact**: Moderate consistency gain (especially run-to-run variance in Inheritance Sealing Accuracy).

### Skill-Level Improvements

### S1. Reduce response verbosity and enforce concise scan/output contract
- **Dimensions affected**: Token Efficiency
- **Current score → Target score**: 1.6 → 3.5+
- **Problem**: Reports are high quality but overlong; token usage is +143% input vs baseline.
- **Root cause**: The skill mandates broad scan checklists and verbose multi-step output; the agent enforces a two-pass approach every time.
- **Suggested changes**:

  **Before** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\SKILL.md`):
  ```md
  **Rules:**
  - Run every relevant recipe for the detected pattern categories
  - **Emit a scan execution checklist** before classifying findings — list each recipe and the hit count
  - A result of **0 hits** is valid and valuable (confirms good practice)
  - If reference files were loaded, also run their `## Detection` recipes
  ```

  **After** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\SKILL.md`):
  ```md
  **Rules:**
  - Run relevant recipes and keep internal counts, but only report **top actionable findings**.
  - Include a compact checklist summary only when the user asks for full audit mode.
  - Report 0-hit categories in one line total (not per-recipe detail).
  - Default response budget: max 12 findings + 1 summary table.
  ```

  **Before** (`dotnet-skills\plugins\dotnet-diag\agents\optimizing-dotnet-performance.agent.md`):
  ```md
  ## Two-Pass Analysis

  Every performance review uses two passes. Both are mandatory — do not skip Pass 2.
  ```

  **After** (`dotnet-skills\plugins\dotnet-diag\agents\optimizing-dotnet-performance.agent.md`):
  ```md
  ## Analysis Mode

  Use single-pass analysis by default for focused code reviews.
  Enable two-pass analysis only for broad repository audits or when requested.
  ```

- **Expected impact**: Large gain in Token Efficiency without sacrificing critical-dimension detection quality.

### S2. Add explicit inheritance sealing guardrail with required base/leaf proof
- **Dimensions affected**: Inheritance Sealing Accuracy, Severity Classification Accuracy
- **Current score → Target score**: 4.2 → 4.8-5.0
- **Problem**: One run dropped to 2/5 due to weaker base-class protection language.
- **Root cause**: Current structural instructions mention exclusions, but do not require explicit proof before recommending `sealed`.
- **Suggested changes**:

  **Before** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\structural-patterns.md`):
  ```md
  **Exclusions:** Do not seal classes that are subclassed elsewhere in the codebase. Identifying base classes requires manual review — grep for `: ClassName` patterns and cross-reference, but expect false positives from interface implementations and generic constraints.
  ```

  **After** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\structural-patterns.md`):
  ```md
  **Exclusions (mandatory):** Do not recommend sealing unless you prove the class is a leaf.
  Required proof for each seal recommendation:
  1. `ClassName` has no subclasses in the scanned codebase.
  2. `ClassName` is not used as an extensibility base (documented or framework-facing inheritance point).
  3. If any subclass exists, explicitly mark: "Base class — do not seal."

  Always include one explicit "do not seal base class" example when inheritance findings are present.
  ```

  **Before** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\SKILL.md`):
  ```md
  **Verify-the-Inverse Rule:** For absence patterns, always count both sides and report the ratio (e.g., "N of M classes are sealed"). The ratio determines severity — 0/185 is systematic, 12/15 is a consistency fix.
  ```

  **After** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\SKILL.md`):
  ```md
  **Verify-the-Inverse Rule:** For absence patterns, count both sides and report ratio.
  For class sealing, add a **Base-Class Safety Check**:
  - Do not recommend sealing any class with detected subclasses.
  - Explicitly list base classes that must remain unsealed.
  ```

- **Expected impact**: High confidence fix for the highest-variance high-tier dimension.

### S3. Make params optimization recommendation concrete and aligned to common single-arg usage
- **Dimensions affected**: Params Overload Optimization, Fix Recommendation Quality
- **Current score → Target score**: 4.2 → 4.8
- **Problem**: Responses sometimes suggest generic alternatives instead of the concrete single-arg fast path expected by the benchmark.
- **Root cause**: Existing guidance emphasizes `params ReadOnlySpan<T>` and can under-prioritize explicit one-argument overloads.
- **Suggested changes**:

  **Before** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\memory-and-strings.md`):
  ```md
  ### Use params ReadOnlySpan<T> to Eliminate Array Allocations
  🟡 **DO** add `params ReadOnlySpan<T>` overloads to library methods | C# 13 / .NET 9+
  ```

  **After** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\memory-and-strings.md`):
  ```md
  ### Optimize params Calls with Explicit Fast Paths
  🟡 **DO** add concrete one-argument overloads first; optionally add `params ReadOnlySpan<T>` on .NET 9+

  Priority order:
  1. Add `Method(..., T item)` single-arg overload for common call shape.
  2. Add 2-arg overload where common.
  3. Keep existing `params T[]` for compatibility.
  4. For .NET 9+, consider `params ReadOnlySpan<T>` as an additional optimization.
  ```

- **Expected impact**: Medium-high improvement on the moderate-tier dimension with better alignment to required fix specificity.

### S4. Improve reflection/serialization depth with explicit partial-deserialization rule
- **Dimensions affected**: Reflection and Serialization Overhead, Fix Recommendation Quality
- **Current score → Target score**: 4.4 → 4.8
- **Problem**: Dimension is only slightly above baseline and often ties; analysis calls out limited development of `Utf8JsonReader` alternatives.
- **Root cause**: Guidance focuses on caching options/reflection but not strongly on partial parsing strategy selection.
- **Suggested changes**:

  **Before** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\io-and-serialization.md`):
  ```md
  ### Patterns Requiring Manual Review

  - **`JsonSerializer.Serialize/Deserialize` without source-gen context**: Can't determine from grep if a context parameter is passed
  ```

  **After** (`dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\io-and-serialization.md`):
  ```md
  ### Patterns Requiring Manual Review

  - **`JsonSerializer.Serialize/Deserialize` without source-gen context**: verify context usage.
  - **Full-object deserialization on partial-read scenarios**: if only 1-3 fields are used, recommend `Utf8JsonReader` (or `JsonDocument` selective reads) instead of full deserialization.
  - Include this as a performance finding when it appears in hot paths.
  ```

- **Expected impact**: Moderate improvement in a high-tier dimension currently under-realized.

## Summary of Recommended Changes

### Modified files
- `dotnet-skills\plugins\dotnet-diag\plugin.json`
- `dotnet-skills\plugins\dotnet-diag\agents\optimizing-dotnet-performance.agent.md`
- `dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\SKILL.md`
- `dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\structural-patterns.md`
- `dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\memory-and-strings.md`
- `dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\io-and-serialization.md`

### New files
- `dotnet-skills\plugins\dotnet-diag\README.md`
- `dotnet-skills\plugins\dotnet-diag\CHANGELOG.md`
- `dotnet-skills\plugins\dotnet-diag\LICENSE`
- `dotnet-skills\plugins\dotnet-diag\skills\analyzing-dotnet-performance\examples\analyze-perf-issues-golden.md`

## Risks and Trade-offs
- Tightening output budgets can hide lower-priority findings; mitigate with an explicit “full audit mode.”
- Stronger inheritance guardrails may reduce number of seal recommendations (fewer false positives, but potentially fewer true positives if scan scope is too narrow).
- Prioritizing one/two-arg overloads improves common-case allocation but can increase API surface area.
