# Improvement Suggestions: dotnet-diag-auto-improve

## Executive Summary
`dotnet-diag-auto-improve` is already strong (130/135 weighted, 96% of max), with only two scored gaps: **Collection and LINQ Efficiency (4/5)** and **Reflection and Serialization Overhead (4/5)**. The analysis explicitly calls out missing evidence for `Distinct().ToList()` detection and limited guidance on partial JSON parsing (`Utf8JsonReader`).

The highest-impact improvements are mostly **skill-level** (tightening detection recipes and required output checks), with a smaller but important **plugin-level** layer (agent guardrails and plugin packaging/docs hygiene).  
Note: this run has only one evaluated configuration, so direct “vs baseline/top config” comparison is not available in the current report data.

## Current Performance Snapshot
| Dimension | Tier | Score | Weighted Points | Gap to 5 |
|---|---:|---:|---:|---:|
| Regex Anti-Pattern Detection | CRITICAL (x3) | 5.0 | 15 | 0 |
| String Allocation Detection | CRITICAL (x3) | 5.0 | 15 | 0 |
| **Collection and LINQ Efficiency** | **CRITICAL (x3)** | **4.0** | **12** | **1** |
| Async and IO Pattern Detection | CRITICAL (x3) | 5.0 | 15 | 0 |
| **Reflection and Serialization Overhead** | **HIGH (x2)** | **4.0** | **8** | **1** |
| Structural Optimization Detection | HIGH (x2) | 5.0 | 10 | 0 |
| Aggregate and Replace Chain Detection | HIGH (x2) | 5.0 | 10 | 0 |
| Span Usage Consistency | HIGH (x2) | 5.0 | 10 | 0 |
| Inheritance Sealing Accuracy | HIGH (x2) | 5.0 | 10 | 0 |
| Params Overload Optimization | MODERATE (x1) | 5.0 | 5 | 0 |
| Severity Classification Accuracy | HIGH (x2) | 5.0 | 10 | 0 |
| Fix Recommendation Quality | HIGH (x2) | 5.0 | 10 | 0 |

## Plugin Structure Assessment
### Plugin: `dotnet-diag`
- **Inventory**: 7 skills, 1 agent, 0 hooks, 0 MCP, 0 LSP.
- **Manifest quality**: valid minimal manifest, but missing recommended metadata (`author`, `license`, `keywords`) and discoverability detail.
- **Skill quality**:
  - Performance-analysis skill is strong but still misses two rubric-specific detections in produced output.
  - Several SKILL bodies are very long (context pressure risk).
- **Structural gaps**:
  - No README/CHANGELOG/LICENSE at plugin root.
  - No hook-based guardrails to force rubric-complete output for performance analysis.

## Improvement Suggestions

### Plugin-Level Improvements

#### P1. Add an output-quality guardrail hook for rubric completeness
- **Type**: New hook
- **Dimensions affected**: Collection and LINQ Efficiency, Reflection and Serialization Overhead, Severity Classification Accuracy
- **Problem**: Current output occasionally misses rubric-specific required evidence (`Distinct().ToList()` and partial JSON parse guidance), despite otherwise high coverage.
- **Suggested changes**:

**New File** (`C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\hooks\hooks.json`):
```json
{
  "hooks": [
    {
      "event": "PostToolUse",
      "matcher": "task|skill",
      "command": "pwsh -NoProfile -File \"${CLAUDE_PLUGIN_ROOT}/hooks/validate-performance-output.ps1\""
    }
  ]
}
```

**New File** (`C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\hooks\validate-performance-output.ps1`):
```powershell
[CmdletBinding()]
param()

# Lightweight content guardrail for performance-analysis responses.
$text = $env:CLAUDE_LAST_RESPONSE
if (-not $text) { exit 0 }

$required = @(
  'Distinct().ToList()',
  'Skip(i).Take(',
  'Utf8JsonReader',
  'JsonDocument'
)

$missing = @()
foreach ($item in $required) {
  if ($text -notmatch [Regex]::Escape($item)) { $missing += $item }
}

if ($missing.Count -gt 0) {
  Write-Host "⚠️ dotnet-diag quality gate: missing expected signals -> $($missing -join ', ')"
}
```

- **Expected impact**: Improves consistency on the two underperforming dimensions; expected **+3 to +5 weighted points** in strict-rubric evals (if misses are prevented).

#### P2. Tighten the performance agent’s Pass 2 completion criteria
- **Type**: Agent update
- **Dimensions affected**: Collection and LINQ Efficiency, Reflection and Serialization Overhead, Fix Recommendation Quality
- **Problem**: Agent currently mandates running the skill but does not enforce a rubric-coverage checklist.
- **Suggested changes**:

**Before** (`C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\agents\optimizing-dotnet-performance.agent.md`):
```md
### Pass 2: Skill-Based Deep Scan

**Always execute after Pass 1.** Do not ask whether to proceed.

1. Load the **analyzing-dotnet-performance** skill
2. Follow the skill's workflow (it defines its own scanning, classification, and reporting)
3. Deduplicate against Pass 1 — only report new findings
4. Label this section **"Pass 2: Deep Pattern Scan"**
```

**After** (`C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\agents\optimizing-dotnet-performance.agent.md`):
```md
### Pass 2: Skill-Based Deep Scan

**Always execute after Pass 1.** Do not ask whether to proceed.

1. Load the **analyzing-dotnet-performance** skill
2. Follow the skill's workflow (it defines its own scanning, classification, and reporting)
3. Deduplicate against Pass 1 — only report new findings
4. Label this section **"Pass 2: Deep Pattern Scan"**
5. Add a required subsection **"Rubric Coverage Check"** with explicit yes/no evidence for:
   - `Distinct().ToList()` and `Skip().Take().ToList()` allocation patterns
   - repeated `JsonSerializer.Deserialize*` on hot paths
   - partial parse alternatives (`Utf8JsonReader` or `JsonDocument`)
```

- **Expected impact**: Raises floor quality by forcing explicit coverage on known blind spots; likely **+1 score** on each weak dimension when previously missed.

#### P3. Upgrade plugin packaging metadata and documentation
- **Type**: Manifest fix + Documentation
- **Dimensions affected**: Indirect (activation reliability and maintainability)
- **Problem**: Plugin is usable but lacks recommended metadata and top-level docs (README, CHANGELOG, LICENSE), reducing discoverability and maintainability.
- **Suggested changes**:

**Before** (`C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\plugin.json`):
```json
{
  "name": "dotnet-diag",
  "version": "0.1.0",
  "description": "Skills for .NET performance investigations, debugging, and incident analysis.",
  "skills": ["./skills/"],
  "agents": ["./agents/optimizing-dotnet-performance.agent.md"]
}
```

**After** (`C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\plugin.json`):
```json
{
  "name": "dotnet-diag",
  "version": "0.2.0",
  "description": "Skills for .NET performance investigations, diagnostics collection, crash symbolication, and incident triage.",
  "author": { "name": ".NET Diagnostics Team" },
  "license": "MIT",
  "keywords": ["dotnet", "performance", "diagnostics", "profiling", "tracing", "crash-analysis"],
  "skills": ["./skills/"],
  "agents": ["./agents/optimizing-dotnet-performance.agent.md"]
}
```

**New File** (`C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\README.md`):
```md
# dotnet-diag plugin

Reusable .NET diagnostics plugin focused on:
- performance anti-pattern analysis
- trace/dump collection workflows
- Android/Apple crash symbolication
- CLR activation troubleshooting

## Included skills
- analyzing-dotnet-performance
- dotnet-trace-collect
- dump-collect
- microbenchmarking
- apple-crash-symbolication
- android-tombstone-symbolication
- clr-activation-debugging

## Included agent
- optimizing-dotnet-performance

## Usage
Use this plugin when diagnosing .NET performance or production incidents across Windows/Linux/macOS and containerized environments.
```

**New File** (`C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\CHANGELOG.md`):
```md
# Changelog

## 0.2.0
- Added metadata fields to plugin manifest (author, license, keywords)
- Added output-quality hook scaffold for performance analysis completeness
- Tightened performance-agent rubric coverage requirements
- Added plugin README and LICENSE

## 0.1.0
- Initial plugin release with 7 skills and 1 optimization agent
```

**New File** (`C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\LICENSE`):
```text
MIT License

Copyright (c) .NET Diagnostics Team

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

- **Expected impact**: Better plugin activation/discovery and maintainability; low direct score lift but high operational value.

### Skill-Level Improvements

### S1. Add explicit LINQ allocation pattern detection (`Distinct().ToList()`, sliding windows)
- **Dimensions affected**: Collection and LINQ Efficiency
- **Current score → Target score**: 4.0 → 5.0
- **Problem**: Analysis notes a missing explicit `Distinct().ToList()` detection signal.
- **Root cause**: Collection reference detection recipes do not explicitly scan for `Distinct().ToList()` or `Skip().Take().ToList()` in one place; reliance on generic LINQ-chain detection can miss rubric-specific reporting.
- **Suggested changes**:

**Before** (`C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\collections-and-linq.md`):
````md
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
````

**After** (`C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\collections-and-linq.md`):
````md
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

# Distinct + ToList materialization chain (rubric-critical)
grep -rn --include='*.cs' -E '\.Distinct\(\)\.ToList\(\)' --exclude-dir=bin --exclude-dir=obj . | wc -l

# Sliding-window allocation chain (Skip/Take/ToList)
grep -rn --include='*.cs' -E '\.Skip\([^)]*\)\.Take\([^)]*\)\.ToList\(\)' --exclude-dir=bin --exclude-dir=obj . | wc -l
```
````

- **Expected impact**: Removes known coverage blind spot; likely +1 on this CRITICAL dimension (**+3 weighted**).

### S2. Add explicit partial-JSON parsing guidance and detection
- **Dimensions affected**: Reflection and Serialization Overhead, Fix Recommendation Quality
- **Current score → Target score**: 4.0 → 5.0
- **Problem**: Analysis was strong on options/reflection caching, but less explicit on replacing repeated full deserialization with `Utf8JsonReader`/partial parsing.
- **Root cause**: I/O reference only detects `new HttpClient` and `new JsonSerializerOptions`; no explicit deserialization hot-path scan or required fix pattern.
- **Suggested changes**:

**Before** (`C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\io-and-serialization.md`):
````md
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
````

**After** (`C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\io-and-serialization.md`):
````md
## Detection

Scan recipes for I/O and serialization anti-patterns. Run these and report exact counts.

```bash
# new HttpClient() (socket exhaustion risk)
grep -rn --include='*.cs' 'new HttpClient(' --exclude-dir=bin --exclude-dir=obj . | wc -l

# new JsonSerializerOptions() not cached (592x slower in .NET 6)
grep -rn --include='*.cs' 'new JsonSerializerOptions' --exclude-dir=bin --exclude-dir=obj . | grep -v 'static\|readonly' | wc -l

# Full deserialize calls in hot-path candidates
grep -rn --include='*.cs' -E 'JsonSerializer\.Deserialize(|Async)\<' --exclude-dir=bin --exclude-dir=obj . | wc -l

# Document/reader-based partial parse usage (verify the inverse)
grep -rn --include='*.cs' -E 'Utf8JsonReader|JsonDocument\.Parse|JsonNode\.Parse' --exclude-dir=bin --exclude-dir=obj . | wc -l
```

### Patterns Requiring Manual Review

- **`JsonSerializer.Serialize/Deserialize` without source-gen context**: Can't determine from grep if a context parameter is passed
- **Repeated full deserialization in loops or hot paths**: recommend `Utf8JsonReader`/`JsonDocument` for partial-field reads when full object materialization is unnecessary
````

- **Expected impact**: Better completeness and API-specific remediation quality; likely +1 on this HIGH dimension (**+2 weighted**).

### S3. Make rubric-coverage checks mandatory in the core skill workflow
- **Dimensions affected**: Collection and LINQ Efficiency, Reflection and Serialization Overhead, Severity Classification Accuracy
- **Current score → Target score**: 4.0/4.0 → 5.0/5.0
- **Problem**: Core workflow does not force explicit “must-report” checks for the two weaker areas.
- **Root cause**: Step 3 and Step 5 are strong but generic; they do not require a coverage ledger for rubric-critical patterns.
- **Suggested changes**:

**Before** (`C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\skills\analyzing-dotnet-performance\SKILL.md`):
```md
### Step 3: Scan and Report

**For files under 500 lines, read the entire file first** — you'll spot most patterns faster than running individual grep recipes. Use grep to confirm counts and catch patterns you might miss visually.

For each relevant pattern category, run the detection recipes below. Report exact counts, not estimates.
```

**After** (`C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\skills\analyzing-dotnet-performance\SKILL.md`):
```md
### Step 3: Scan and Report

**For files under 500 lines, read the entire file first** — you'll spot most patterns faster than running individual grep recipes. Use grep to confirm counts and catch patterns you might miss visually.

For each relevant pattern category, run the detection recipes below. Report exact counts, not estimates.

**Rubric-critical coverage checks (always required):**
1. Report explicit hit counts for `Distinct().ToList()` and `Skip().Take().ToList()`.
2. Report explicit hit counts for full `JsonSerializer.Deserialize*` usage in hot-path candidates.
3. Report whether `Utf8JsonReader`/`JsonDocument` appears, and if absent, state whether partial parsing is a valid fix.
```

**Before** (`C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\skills\analyzing-dotnet-performance\SKILL.md`):
```md
### Step 5: Generate Findings

**Keep findings compact.** Each finding is one short block — not an essay. Group by severity (🔴 → 🟡 → ℹ️), not by file.
```

**After** (`C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\skills\analyzing-dotnet-performance\SKILL.md`):
```md
### Step 5: Generate Findings

**Keep findings compact.** Each finding is one short block — not an essay. Group by severity (🔴 → 🟡 → ℹ️), not by file.

Add a final **Coverage Ledger** block:
- `Distinct().ToList()` hits: N
- `Skip().Take().ToList()` hits: N
- Full `Deserialize*` hot-path hits: N
- `Utf8JsonReader`/`JsonDocument` usage sites: N
```

- **Expected impact**: Makes misses auditable and far less likely; supports reliable 5/5 on both weaker dimensions.

## Summary of Recommended Changes
### Modified files
- `C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\plugin.json`
- `C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\agents\optimizing-dotnet-performance.agent.md`
- `C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\skills\analyzing-dotnet-performance\SKILL.md`
- `C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\collections-and-linq.md`
- `C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\skills\analyzing-dotnet-performance\references\io-and-serialization.md`

### New files
- `C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\hooks\hooks.json`
- `C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\hooks\validate-performance-output.ps1`
- `C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\README.md`
- `C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\CHANGELOG.md`
- `C:\data\mycode\skills\dotnet-diag-auto-improve\plugins\dotnet-diag\LICENSE`

## Risks and Trade-offs
- Hook-based validation can increase false positives/noise if response text intentionally omits exact string forms; keep it warning-only at first.
- Adding mandatory coverage ledger slightly increases output length, which may raise token usage.
- Tightening rubric checks can over-prioritize benchmark-specific patterns; keep severity calibration rules to avoid inflating low-impact findings.
