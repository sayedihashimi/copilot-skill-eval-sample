# Analysis Report: .NET Performance Analysis Skill Evaluation

**Runs:** 5 | **Configurations:** 2 | **Dimensions:** 12
**Date:** 2026-05-05 03:07 UTC

---

## Overview

Model: **claude-opus-4.6** | Judge: **gpt-4.1** | Threshold: **50%**

---

## Configurations

| Configuration | Skills Loaded | Tags |
|---|---|---|
| analyze-perf-baseline | customize-cloud-agent | — |
| analyze-perf-with-skills | analyzing-dotnet-performance, microbenchmarking, dotnet-trace-collect, customize-cloud-agent | — |

---

## Scoring Methodology

Each dimension is scored on a **0–10 scale** by an LLM judge. Dimensions are grouped into tiers:

| Tier | Weight | Dimensions |
|---|:---:|:---:|
| MODERATE | ×1 | 12 |

**Maximum possible weighted score: 120**

---

## Executive Summary

Mean dimension scores across runs (0–10 scale, **higher is better**).
± values show standard deviation across runs.

| Dimension [Tier] | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| Regex Anti-Pattern Detection [MODERATE] | 7.6 ± 0.5 | 8.2 ± 1.0 |
| String Allocation Detection [MODERATE] | 7.3 ± 0.6 | 7.6 ± 0.9 |
| Collection and LINQ Efficiency [MODERATE] | 6.9 ± 0.9 | 6.0 ± 1.0 |
| Async and IO Pattern Detection [MODERATE] | 7.1 ± 0.6 | 5.8 ± 0.9 |
| Reflection and Serialization Overhead [MODERATE] | 7.3 ± 0.6 | 8.0 ± 0.5 |
| Structural Optimization Detection [MODERATE] | 7.8 ± 0.0 | 7.3 ± 0.6 |
| Aggregate and Replace Chain Detection [MODERATE] | 4.0 ± 1.0 | 4.4 ± 0.8 |
| Span Usage Consistency [MODERATE] | 5.1 ± 1.7 | 6.0 ± 1.0 |
| Inheritance Sealing Accuracy [MODERATE] | 4.0 ± 1.0 | 4.0 ± 1.7 |
| Severity Classification Accuracy [MODERATE] | 8.0 ± 0.5 | 7.3 ± 1.0 |
| Fix Recommendation Quality [MODERATE] | 7.6 ± 0.9 | 7.3 ± 1.0 |
| Params Overload Optimization [MODERATE] | 2.7 ± 1.7 | 3.6 ± 0.5 |

---

## Final Rankings

Configurations ranked by weighted score — **higher is better**.

| Rank | Configuration | Mean Score ↑ | % of Max (120) | Std Dev ↓ | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | analyze-perf-with-skills | 75.6 | 63.0% | 5.8 | 68.9 | 82.2 |
| 🥈 | analyze-perf-baseline | 75.3 | 62.8% | 3.4 | 72.2 | 78.9 |

---

## Token Usage Summary

Average token consumption per configuration across all runs.

| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time | Δ Input vs Baseline |
|---|---|---|---|---|---|---|
| analyze-perf-baseline | 148,240 | 9,448 | 90,916 | 5 | 2m 35s | — (baseline) |
| analyze-perf-with-skills | 256,877 | 9,279 | 181,928 | 7 | 2m 42s | +73.3% |

### Token Usage Per Run

| Configuration | Run | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time |
|---|---|---|---|---|---|---|
| analyze-perf-baseline | 1 | 149,281 | 10,226 | 82,549 | 5 | 2m 50s |
| analyze-perf-baseline | 2 | 148,273 | 10,614 | 94,408 | 5 | 2m 49s |
| analyze-perf-baseline | 3 | 148,567 | 9,448 | 87,053 | 5 | 2m 37s |
| analyze-perf-baseline | 4 | 148,045 | 8,964 | 95,645 | 5 | 2m 27s |
| analyze-perf-baseline | 5 | 147,034 | 7,988 | 94,927 | 5 | 2m 15s |
| analyze-perf-with-skills | 1 | 244,701 | 7,868 | 168,059 | 7 | 2m 29s |
| analyze-perf-with-skills | 2 | 314,527 | 10,053 | 204,763 | 8 | 2m 45s |
| analyze-perf-with-skills | 3 | 244,915 | 8,799 | 182,074 | 7 | 2m 31s |
| analyze-perf-with-skills | 4 | 236,396 | 9,300 | 177,516 | 7 | 2m 40s |
| analyze-perf-with-skills | 5 | 243,846 | 10,375 | 177,228 | 7 | 3m 4s |


## Tool Usage Breakdown

| Configuration | Tool Calls | Turns | Skills Used | Tool Breakdown |
|---|---|---|---|---|
| analyze-perf-baseline | 18 | 1 | customize-cloud-agent | view(14), report_intent(2), glob(1), create(1) |
| analyze-perf-with-skills | 21 | 2 | analyzing-dotnet-performance, microbenchmarking, dotnet-trace-collect, customize-cloud-agent | view(16), report_intent(2), skill(1), glob(1), create(1) |

---

## Per-Dimension Analysis

### 1. Regex Anti-Pattern Detection [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 7.8 | 6.7 |
| 2 | 7.8 | 7.8 |
| 3 | 6.7 | 8.9 |
| 4 | 7.8 | 8.9 |
| 5 | 7.8 | 8.9 |
| **Mean** | **7.6** | **8.2** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 7.6/10
- ✅ **analyze-perf-with-skills**: 8.2/10

**Verdict:** **analyze-perf-with-skills** leads (+0.7 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> This is a solid analysis pass that demonstrably identified at least the most important regex hot-path issue and produced a structured report in the correct location. However, because the evidence shown does not explicitly confirm detection of the `MarkdownStripper` compiled-regex startup problem or a `[GeneratedRegex]` recommendation, it falls short of top marks for regex anti-pattern detection.

**analyze-perf-with-skills:**
> This looks like a solid performance review with at least one important regex anti-pattern correctly identified and prioritized. Still, the evidence shown is not comprehensive enough to award top marks for regex analysis because the key `RegexOptions.Compiled` startup-budget issue and explicit `[GeneratedRegex]` guidance are not clearly demonstrated in the visible output.

---

### 2. String Allocation Detection [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 7.8 | 8.9 |
| 2 | 7.8 | 7.8 |
| 3 | 6.7 | 6.7 |
| 4 | 7.8 | 6.7 |
| 5 | 6.7 | 7.8 |
| **Mean** | **7.3** | **7.6** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 7.3/10
- ✅ **analyze-perf-with-skills**: 7.6/10

**Verdict:** **analyze-perf-with-skills** leads (+0.2 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> This was a strong task completion: the agent inspected the codebase properly and produced a structured report in the requested file. The main weakness, based on the evidence provided, is incomplete string-allocation coverage—especially around Replace-chain allocations and casing/culture issues—so the overall result is good but not top-tier.

**analyze-perf-with-skills:**
> The agent very likely delivered a strong, well-organized performance analysis that adhered closely to the task instructions and produced the required output file. The main limitation is that the full report is not shown, so a perfect score would overstate what can be verified from the evidence.

---

### 3. Collection and LINQ Efficiency [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 5.6 | 6.7 |
| 2 | 6.7 | 5.6 |
| 3 | 6.7 | 4.4 |
| 4 | 7.8 | 6.7 |
| 5 | 7.8 | 6.7 |
| **Mean** | **6.9** | **6.0** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 6.9/10
- ❌ **analyze-perf-with-skills**: 6.0/10

**Verdict:** **analyze-perf-baseline** leads (+0.9 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> The agent likely produced a competent general performance review, but for the specific collection/LINQ dimension the evidence is incomplete and misses several rubric-critical patterns. Its task execution and structure look solid, yet the collection efficiency analysis shown here is only moderate rather than comprehensive.

**analyze-perf-with-skills:**
> This looks like a competently executed analysis with solid structure and broad performance coverage. But because the provided evidence does not specifically demonstrate comprehensive collection/LINQ issue detection, the score should remain moderate rather than high for this evaluation dimension.

---

### 4. Async and IO Pattern Detection [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 7.8 | 6.7 |
| 2 | 6.7 | 4.4 |
| 3 | 6.7 | 5.6 |
| 4 | 7.8 | 5.6 |
| 5 | 6.7 | 6.7 |
| **Mean** | **7.1** | **5.8** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 7.1/10
- ❌ **analyze-perf-with-skills**: 5.8/10

**Verdict:** **analyze-perf-baseline** leads (+1.3 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> The agent successfully completed the task and produced a well-structured report, but the available evidence does not show comprehensive async/IO pattern detection. It appears strong overall, yet falls short of top marks because several important async-specific findings are not explicitly demonstrated.

**analyze-perf-with-skills:**
> This looks like a solid and well-presented analysis, and the agent clearly completed the requested artifact generation. Still, for the async/IO evaluation dimension, the visible evidence is incomplete beyond the `HttpClient` finding, so a top-tier score is not justified.

---

### 5. Reflection and Serialization Overhead [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 6.7 | 7.8 |
| 2 | 6.7 | 7.8 |
| 3 | 7.8 | 7.8 |
| 4 | 7.8 | 7.8 |
| 5 | 7.8 | 8.9 |
| **Mean** | **7.3** | **8.0** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 7.3/10
- ✅ **analyze-perf-with-skills**: 8.0/10

**Verdict:** **analyze-perf-with-skills** leads (+0.7 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> The agent likely delivered a solid overall performance report and clearly structured it well, but the evidence for this specific reflection and serialization criterion is only partial. It gets credit for identifying reflection overhead at a high level, yet there is not enough visible proof that it fully captured the important JsonSerializerOptions and partial-deserialization issues.

**analyze-perf-with-skills:**
> This appears to be a solid and mostly correct performance review with good structure and clear prioritization. Still, the available evidence does not fully confirm that all required reflection/serialization findings were captured, and the source-file count inconsistency prevents a higher score.

---

### 6. Structural Optimization Detection [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 7.8 | 6.7 |
| 2 | 7.8 | 7.8 |
| 3 | 7.8 | 6.7 |
| 4 | 7.8 | 7.8 |
| 5 | 7.8 | 7.8 |
| **Mean** | **7.8** | **7.3** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 7.8/10
- ✅ **analyze-perf-with-skills**: 7.3/10

**Verdict:** **analyze-perf-baseline** leads (+0.4 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> This was a strong submission that appears to satisfy the task and produce a comprehensive, structured performance report. The main limitation is evidentiary: while structural optimization themes were detected, the provided transcript does not prove that every specific class/struct/dictionary target in the rubric was explicitly identified.

**analyze-perf-with-skills:**
> This is a solid analysis pass with strong process adherence and good report structure. Still, the evidence provided is insufficient to confirm full coverage of the key structural optimization findings the rubric emphasizes, so the work is good but not fully demonstrated as complete.

---

### 7. Aggregate and Replace Chain Detection [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 3.3 | 4.4 |
| 2 | 3.3 | 3.3 |
| 3 | 5.6 | 4.4 |
| 4 | 3.3 | 5.6 |
| 5 | 4.4 | 4.4 |
| **Mean** | **4.0** | **4.4** |

#### Score Comparison

- ❌ **analyze-perf-baseline**: 4.0/10
- ❌ **analyze-perf-with-skills**: 4.4/10

**Verdict:** **analyze-perf-with-skills** leads (+0.4 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> The agent likely produced a generally well-structured performance report, but there is insufficient evidence that it caught the subtle `Aggregate` + `Replace` allocation chain and per-iteration `char.ToString()` allocation that this evaluation targets. Given that gap, the submission rates below average for this specific dimension.

**analyze-perf-with-skills:**
> The agent likely completed a broadly competent performance review and produced a structured report, but the provided evidence does not demonstrate detection of the subtle Aggregate/Replace-chain allocation pattern this rubric is specifically testing. Because that omission goes to the heart of the evaluation target, the overall score should be below average despite decent presentation.

---

### 8. Span Usage Consistency [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 5.6 | 6.7 |
| 2 | 5.6 | 4.4 |
| 3 | 2.2 | 5.6 |
| 4 | 5.6 | 6.7 |
| 5 | 6.7 | 6.7 |
| **Mean** | **5.1** | **6.0** |

#### Score Comparison

- ❌ **analyze-perf-baseline**: 5.1/10
- ❌ **analyze-perf-with-skills**: 6.0/10

**Verdict:** **analyze-perf-with-skills** leads (+0.9 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> The agent appears to have completed the analysis task and produced a structured report, but the evidence provided is too limited to verify the critical span-usage consistency findings this rubric focuses on. Because only a high-level summary is visible, the assessment must stay conservative.

**analyze-perf-with-skills:**
> The agent appears to have completed the analysis workflow and produced a well-organized report, but the evidence shown does not sufficiently confirm strong coverage of the span-usage consistency findings being evaluated here. As a result, the submission is solid in format and execution but only moderately convincing on the specific performance-review dimension.

---

### 9. Inheritance Sealing Accuracy [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 3.3 | 4.4 |
| 2 | 3.3 | 2.2 |
| 3 | 5.6 | 3.3 |
| 4 | 3.3 | 3.3 |
| 5 | 4.4 | 6.7 |
| **Mean** | **4.0** | **4.0** |

#### Score Comparison

- ❌ **analyze-perf-baseline**: 4.0/10
- ❌ **analyze-perf-with-skills**: 4.0/10

**Verdict:** Tie — all configurations scored equally.

#### Judge Evidence

**analyze-perf-baseline:**
> The agent seems to have completed the mechanics of the task and produced a structured report, but the key evaluation dimension here is inheritance sealing accuracy, and the provided evidence is insufficient to confirm it got that right. Because avoiding false positives on DefaultOrdinalizer is explicitly critical and unproven here, the overall score should remain low.

**analyze-perf-with-skills:**
> The agent seems to have produced a generally well-organized and comprehensive performance analysis, but the evidence provided is insufficient to verify the critical inheritance-sealing distinction this evaluation targets. Because false positives on sealing base classes would be a serious error and the report content for those classes is not shown, the assessment must remain cautious.

---

### 10. Severity Classification Accuracy [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 7.8 | 7.8 |
| 2 | 7.8 | 6.7 |
| 3 | 7.8 | 6.7 |
| 4 | 7.8 | 8.9 |
| 5 | 8.9 | 6.7 |
| **Mean** | **8.0** | **7.3** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 8.0/10
- ✅ **analyze-perf-with-skills**: 7.3/10

**Verdict:** **analyze-perf-baseline** leads (+0.7 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> The agent appears to have completed the analysis workflow correctly and produced a structured report with generally sensible severity prioritization. Still, the lack of full report visibility and a small inconsistency in the summary prevent a higher confidence score.

**analyze-perf-with-skills:**
> This is a strong result with good evidence of methodical code inspection and a clearly organized deliverable. The main limitation is that only a summary and partial report content are available, so some caution is warranted before awarding a top score for correctness and severity calibration.

---

### 11. Fix Recommendation Quality [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 6.7 | 7.8 |
| 2 | 6.7 | 7.8 |
| 3 | 7.8 | 5.6 |
| 4 | 7.8 | 7.8 |
| 5 | 8.9 | 7.8 |
| **Mean** | **7.6** | **7.3** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 7.6/10
- ✅ **analyze-perf-with-skills**: 7.3/10

**Verdict:** **analyze-perf-baseline** leads (+0.2 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> The agent appears to have completed the analysis workflow and produced a well-structured report with plausible performance findings. Still, the evidence for strong, API-specific fix guidance is limited, and the file-count inconsistency prevents a higher overall score.

**analyze-perf-with-skills:**
> This is a strong result that likely satisfies most of the task requirements, with evidence of thorough code reading and a structured written deliverable. The main limitation is that the full report is not available for verification and there is a minor inconsistency in the reported file count, which prevents a top score.

---

### 12. Params Overload Optimization [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 2.2 | 3.3 |
| 2 | 4.4 | 4.4 |
| 3 | 1.1 | 3.3 |
| 4 | 4.4 | 3.3 |
| 5 | 1.1 | 3.3 |
| **Mean** | **2.7** | **3.6** |

#### Score Comparison

- ❌ **analyze-perf-baseline**: 2.7/10
- ❌ **analyze-perf-with-skills**: 3.6/10

**Verdict:** **analyze-perf-with-skills** leads (+0.9 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> Although the agent seems to have generated a structured performance report, the evidence provided does not show the key params-allocation finding required by this rubric. Because the benchmark specifically values detection of the single-argument `params` overload opportunity, the submission should score low overall.

**analyze-perf-with-skills:**
> The agent appears to have produced a generally well-structured performance report, but the evidence provided does not show that it caught the specific `params`-array allocation pattern this rubric is testing. Because that optimization target is central to the evaluation, the overall score should remain low despite otherwise solid formatting.

---

## Consistency Analysis

Score σ (standard deviation) measures how much a configuration's weighted score varies across runs — **lower is better**.

| Configuration | Score σ | Most Consistent Dim (σ) | Most Variable Dim (σ) |
|---|---|---|---|
| analyze-perf-with-skills | 5.8 | Reflection and Serialization Overhead (0.5) | Inheritance Sealing Accuracy (1.7) |
| analyze-perf-baseline | 3.4 | Structural Optimization Detection (0.0) | Params Overload Optimization (1.7) |

---

## Summary

**Overall eval score: 65.7%** (threshold: 50%) — ✅ PASSED

- **analyze-perf-with-skills**: weighted score 75.6/120 (63.0%)
- **analyze-perf-baseline**: weighted score 75.3/120 (62.8%)

---
*Generated from vally eval results at C:\data\mycode\copilot-skill-eval-sample\vally01\vally-eval\results\2026-05-05T02-31-30-650Z*
