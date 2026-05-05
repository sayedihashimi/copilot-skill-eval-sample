# Analysis Report: .NET Performance Analysis Skill Evaluation

**Runs:** 5 | **Configurations:** 2 | **Dimensions:** 12
**Date:** 2026-05-04 21:17 UTC

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
| Regex Anti-Pattern Detection [MODERATE] | 7.8 ± 0.8 | 8.4 ± 0.6 |
| String Allocation Detection [MODERATE] | 7.1 ± 0.6 | 6.9 ± 0.9 |
| Collection and LINQ Efficiency [MODERATE] | 6.4 ± 0.5 | 6.0 ± 1.3 |
| Async and IO Pattern Detection [MODERATE] | 6.2 ± 0.6 | 6.0 ± 0.6 |
| Reflection and Serialization Overhead [MODERATE] | 6.9 ± 0.5 | 7.8 ± 0.8 |
| Structural Optimization Detection [MODERATE] | 8.0 ± 0.9 | 7.1 ± 1.0 |
| Aggregate and Replace Chain Detection [MODERATE] | 3.8 ± 1.3 | 4.0 ± 1.0 |
| Span Usage Consistency [MODERATE] | 5.6 ± 1.6 | 6.0 ± 1.3 |
| Inheritance Sealing Accuracy [MODERATE] | 4.2 ± 0.9 | 4.2 ± 0.5 |
| Severity Classification Accuracy [MODERATE] | 7.6 ± 0.9 | 8.2 ± 1.0 |
| Fix Recommendation Quality [MODERATE] | 8.0 ± 0.5 | 7.8 ± 0.8 |
| Params Overload Optimization [MODERATE] | 3.6 ± 0.5 | 4.2 ± 2.4 |

---

## Final Rankings

Configurations ranked by weighted score — **higher is better**.

| Rank | Configuration | Mean Score ↑ | % of Max (120) | Std Dev ↓ | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | analyze-perf-with-skills | 76.7 | 63.9% | 2.2 | 74.4 | 78.9 |
| 🥈 | analyze-perf-baseline | 75.1 | 62.6% | 1.7 | 73.3 | 76.7 |

---

## Token Usage Summary

Average token consumption per configuration across all runs.

| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time | Δ Input vs Baseline |
|---|---|---|---|---|---|---|
| analyze-perf-baseline | 147,439 | 9,342 | 91,306 | 5 | 2m 42s | — (baseline) |
| analyze-perf-with-skills | 233,368 | 8,756 | 157,074 | 7 | 2m 43s | +58.3% |

### Token Usage Per Run

| Configuration | Run | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time |
|---|---|---|---|---|---|---|
| analyze-perf-baseline | 1 | 146,573 | 8,898 | 93,063 | 5 | 2m 40s |
| analyze-perf-baseline | 2 | 148,010 | 8,955 | 91,141 | 5 | 2m 41s |
| analyze-perf-baseline | 3 | 146,514 | 9,437 | 91,092 | 5 | 2m 38s |
| analyze-perf-baseline | 4 | 148,369 | 9,313 | 88,171 | 5 | 2m 43s |
| analyze-perf-baseline | 5 | 147,727 | 10,107 | 93,065 | 5 | 2m 46s |
| analyze-perf-with-skills | 1 | 314,061 | 10,417 | 197,163 | 8 | 3m 1s |
| analyze-perf-with-skills | 2 | 235,939 | 9,060 | 166,348 | 7 | 2m 52s |
| analyze-perf-with-skills | 3 | 185,067 | 6,912 | 124,872 | 6 | 2m 11s |
| analyze-perf-with-skills | 4 | 244,935 | 8,672 | 176,143 | 7 | 2m 46s |
| analyze-perf-with-skills | 5 | 186,839 | 8,718 | 120,846 | 6 | 2m 47s |


## Tool Usage Breakdown

| Configuration | Tool Calls | Turns | Skills Used | Tool Breakdown |
|---|---|---|---|---|
| analyze-perf-baseline | 18 | 1 | customize-cloud-agent | view(14), report_intent(2), glob(1), create(1) |
| analyze-perf-with-skills | 34 | 2 | analyzing-dotnet-performance, microbenchmarking, dotnet-trace-collect, customize-cloud-agent | view(17), grep(12), report_intent(2), skill(1), glob(1), create(1) |

---

## Per-Dimension Analysis

### 1. Regex Anti-Pattern Detection [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 7.8 | 8.9 |
| 2 | 6.7 | 8.9 |
| 3 | 8.9 | 7.8 |
| 4 | 7.8 | 7.8 |
| 5 | 7.8 | 8.9 |
| **Mean** | **7.8** | **8.4** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 7.8/10
- ✅ **analyze-perf-with-skills**: 8.4/10

**Verdict:** **analyze-perf-with-skills** leads (+0.7 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> This was a strong performance-analysis pass that appears to satisfy the core workflow and catches the major regex anti-patterns. It falls short of a top score because the evidence does not clearly confirm the most specific regex remediation guidance, especially an explicit `[GeneratedRegex]` recommendation.

**analyze-perf-with-skills:**
> The agent appears to have performed a strong and targeted regex performance review, catching the major anti-patterns the rubric emphasizes and tying them to appropriate modern .NET fixes. While the full report is not visible end-to-end, the available evidence supports a high-confidence assessment that the regex analysis was comprehensive and useful.

---

### 2. String Allocation Detection [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 7.8 | 7.8 |
| 2 | 6.7 | 5.6 |
| 3 | 6.7 | 7.8 |
| 4 | 6.7 | 6.7 |
| 5 | 7.8 | 6.7 |
| **Mean** | **7.1** | **6.9** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 7.1/10
- ✅ **analyze-perf-with-skills**: 6.9/10

**Verdict:** **analyze-perf-baseline** leads (+0.2 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> This appears to be a strong performance review that followed the workflow correctly and produced a structured report with concrete findings. Still, based on the evidence shown, the string-allocation analysis is not provably exhaustive—especially around `.Replace()` chains—so the work falls short of top-tier completeness.

**analyze-perf-with-skills:**
> This is a strong result: the agent appears to have executed the required analysis workflow and produced a structured report with concrete findings. Still, the visible inconsistency in file/directory counts introduces enough doubt about precision that the work falls short of excellent.

---

### 3. Collection and LINQ Efficiency [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 5.6 | 4.4 |
| 2 | 6.7 | 5.6 |
| 3 | 6.7 | 7.8 |
| 4 | 6.7 | 5.6 |
| 5 | 6.7 | 6.7 |
| **Mean** | **6.4** | **6.0** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 6.4/10
- ❌ **analyze-perf-with-skills**: 6.0/10

**Verdict:** **analyze-perf-baseline** leads (+0.4 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> The agent likely produced a generally solid performance review, but the evidence for collection and LINQ efficiency coverage is incomplete. It seems to catch some obvious collection issues, yet there is not enough proof that it identified the full set of important lookup and materialization anti-patterns expected for a high score.

**analyze-perf-with-skills:**
> The agent followed the workflow and delivered a structured report, but based on the available evidence it does not show comprehensive collection/LINQ efficiency analysis. It caught at least one important dictionary pattern, yet appears to miss several of the higher-value collection issues the rubric explicitly expected.

---

### 4. Async and IO Pattern Detection [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 5.6 | 5.6 |
| 2 | 6.7 | 6.7 |
| 3 | 6.7 | 5.6 |
| 4 | 6.7 | 6.7 |
| 5 | 5.6 | 5.6 |
| **Mean** | **6.2** | **6.0** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 6.2/10
- ✅ **analyze-perf-with-skills**: 6.0/10

**Verdict:** **analyze-perf-baseline** leads (+0.2 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> This appears to be a competent report with good structure and some meaningful async/IO findings, especially the high-risk `HttpClient` and parallelism issues. But for the specific async/IO detection dimension, the provided evidence falls short of comprehensive coverage because several expected cancellation and sequential-await problems are not explicitly shown.

**analyze-perf-with-skills:**
> This was a competent analysis pass with correct file handling and a structured report, but the evidence for async/IO pattern detection is incomplete. It clearly caught the `HttpClient` issue, yet the record does not demonstrate comprehensive coverage of cancellation, loop awaits, and parallelism risks, which are central to this evaluation.

---

### 5. Reflection and Serialization Overhead [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 6.7 | 7.8 |
| 2 | 6.7 | 7.8 |
| 3 | 6.7 | 6.7 |
| 4 | 7.8 | 8.9 |
| 5 | 6.7 | 7.8 |
| **Mean** | **6.9** | **7.8** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 6.9/10
- ✅ **analyze-perf-with-skills**: 7.8/10

**Verdict:** **analyze-perf-with-skills** leads (+0.9 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> This is a solid analysis pass with good structure and clear evidence that major performance issues were found, especially the reflection hotspot in `EntityMapper`. However, for the specific reflection and serialization overhead dimension, the available evidence does not fully confirm that the agent caught the `JsonSerializerOptions` caching issue and repeated full deserialization pattern, so the result should not score at the top end.

**analyze-perf-with-skills:**
> This is a strong performance-analysis response with good evidence of systematic code reading and identification of the major reflection/serialization overheads. The main limitation is that only a summary and partial creation diff are shown, so some details of the final report cannot be fully validated from the evidence provided.

---

### 6. Structural Optimization Detection [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 8.9 | 7.8 |
| 2 | 7.8 | 6.7 |
| 3 | 6.7 | 5.6 |
| 4 | 7.8 | 7.8 |
| 5 | 8.9 | 7.8 |
| **Mean** | **8.0** | **7.1** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 8.0/10
- ✅ **analyze-perf-with-skills**: 7.1/10

**Verdict:** **analyze-perf-baseline** leads (+0.9 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> The agent appears to have performed the required code-reading workflow and produced a structured analysis file that matches the requested format. Based on the available evidence, the work is strong and likely covers the targeted structural optimization issues, but the absence of the full report prevents a definitive top score.

**analyze-perf-with-skills:**
> This is a solid performance analysis with good structural-optimization coverage and strong organization. It appears to satisfy the main task requirements, but some count inconsistencies and lack of explicit evidence for every expected named structural case prevent a top score.

---

### 7. Aggregate and Replace Chain Detection [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 3.3 | 3.3 |
| 2 | 5.6 | 4.4 |
| 3 | 3.3 | 5.6 |
| 4 | 2.2 | 3.3 |
| 5 | 4.4 | 3.3 |
| **Mean** | **3.8** | **4.0** |

#### Score Comparison

- ❌ **analyze-perf-baseline**: 3.8/10
- ❌ **analyze-perf-with-skills**: 4.0/10

**Verdict:** **analyze-perf-with-skills** leads (+0.2 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> The agent likely produced a broadly organized performance report, but the evidence provided does not show detection of the subtle `Aggregate` + `Replace` chain or the per-iteration `char.ToString()` allocation. Since that is the core of this rubric dimension, the submission should score below average despite acceptable formatting.

**analyze-perf-with-skills:**
> Although the agent appears to have produced a structured analysis file, the evidence provided does not confirm the subtle aggregate/replace allocation pattern that this evaluation specifically targets. Since that is the core of the rubric dimension, the submission should receive a below-average score overall.

---

### 8. Span Usage Consistency [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 4.4 | 6.7 |
| 2 | 6.7 | 4.4 |
| 3 | 6.7 | 5.6 |
| 4 | 6.7 | 7.8 |
| 5 | 3.3 | 5.6 |
| **Mean** | **5.6** | **6.0** |

#### Score Comparison

- ❌ **analyze-perf-baseline**: 5.6/10
- ❌ **analyze-perf-with-skills**: 6.0/10

**Verdict:** **analyze-perf-with-skills** leads (+0.4 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> The agent likely produced a generally well-organized performance report, but for the specific span-usage-consistency dimension there is insufficient evidence that it caught the required issues. Because this evaluation is focused on those omissions, the overall assessment is below average despite acceptable formatting.

**analyze-perf-with-skills:**
> The agent appears to have completed the analysis workflow and produced a well-structured report, but the provided evidence is insufficient to verify strong coverage of the Span Usage Consistency issues specifically targeted by this evaluation. As a result, the work looks competent overall but only partially proven on the most important dimension.

---

### 9. Inheritance Sealing Accuracy [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 4.4 | 4.4 |
| 2 | 5.6 | 4.4 |
| 3 | 3.3 | 4.4 |
| 4 | 4.4 | 4.4 |
| 5 | 3.3 | 3.3 |
| **Mean** | **4.2** | **4.2** |

#### Score Comparison

- ❌ **analyze-perf-baseline**: 4.2/10
- ❌ **analyze-perf-with-skills**: 4.2/10

**Verdict:** Tie — all configurations scored equally.

#### Judge Evidence

**analyze-perf-baseline:**
> The agent likely produced a generally usable performance report with reasonable structure, but the evidence provided does not demonstrate the critical inheritance-sealing judgment this evaluation is targeting. Because avoiding false positives on base classes like DefaultOrdinalizer is essential and unverified here, the assessment must be conservative.

**analyze-perf-with-skills:**
> The agent likely produced a generally well-organized performance report, but the evidence is insufficient to establish accurate inheritance sealing recommendations, which is the critical evaluation focus here. Given the lack of confirmation on the required true positives and false-positive avoidance, the work cannot be scored highly overall.

---

### 10. Severity Classification Accuracy [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 7.8 | 6.7 |
| 2 | 6.7 | 8.9 |
| 3 | 6.7 | 7.8 |
| 4 | 7.8 | 8.9 |
| 5 | 8.9 | 8.9 |
| **Mean** | **7.6** | **8.2** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 7.6/10
- ✅ **analyze-perf-with-skills**: 8.2/10

**Verdict:** **analyze-perf-with-skills** leads (+0.7 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> This is a strong result: the agent appears to have completed the analysis workflow correctly and produced a well-organized report. The main limitation is that only a summary is visible, so the full consistency and accuracy of all severity classifications cannot be fully confirmed from the provided evidence.

**analyze-perf-with-skills:**
> This looks like a solid completion with the right process and a report that was likely organized well. Still, the lack of full report visibility and a few difficult-to-verify counts prevent a higher score.

---

### 11. Fix Recommendation Quality [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 7.8 | 6.7 |
| 2 | 7.8 | 7.8 |
| 3 | 7.8 | 8.9 |
| 4 | 8.9 | 7.8 |
| 5 | 7.8 | 7.8 |
| **Mean** | **8.0** | **7.8** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 8.0/10
- ✅ **analyze-perf-with-skills**: 7.8/10

**Verdict:** **analyze-perf-baseline** leads (+0.2 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> This appears to be a strong completion: the agent inspected the codebase methodically and produced the requested report file in the specified format. The main limitation is that the full report body is not available in the evidence, so the technical quality of every recommendation cannot be confirmed with complete confidence.

**analyze-perf-with-skills:**
> This was a mostly successful completion: the agent used the right inspection steps and produced the requested markdown report. The main limitation is reliability—visible counting/structure inconsistencies make the final analysis less trustworthy than a fully precise performance review.

---

### 12. Params Overload Optimization [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 3.3 | 6.7 |
| 2 | 3.3 | 3.3 |
| 3 | 3.3 | 1.1 |
| 4 | 3.3 | 3.3 |
| 5 | 4.4 | 6.7 |
| **Mean** | **3.6** | **4.2** |

#### Score Comparison

- ❌ **analyze-perf-baseline**: 3.6/10
- ❌ **analyze-perf-with-skills**: 4.2/10

**Verdict:** **analyze-perf-with-skills** leads (+0.7 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> The agent likely produced a generally organized performance report, but the evidence provided does not show the crucial `params` overload optimization finding or the recommended single-argument overload. Since that omission directly affects the target evaluation dimension, the overall assessment is below average.

**analyze-perf-with-skills:**
> The agent likely recognized the params-array allocation pattern, as shown by the summary mention of "params without fast-path," and it seems to have delivered a structured performance report. But the evidence stops short of proving that it identified the exact `Apply(..., params ITruncator[])` hotspot and prescribed the specific single-argument overload optimization, so the score should not be top-tier.

---

## Consistency Analysis

Score σ (standard deviation) measures how much a configuration's weighted score varies across runs — **lower is better**.

| Configuration | Score σ | Most Consistent Dim (σ) | Most Variable Dim (σ) |
|---|---|---|---|
| analyze-perf-with-skills | 2.2 | Inheritance Sealing Accuracy (0.5) | Params Overload Optimization (2.4) |
| analyze-perf-baseline | 1.7 | Collection and LINQ Efficiency (0.5) | Span Usage Consistency (1.6) |

---

## Summary

**Overall eval score: 66.1%** (threshold: 50%) — ✅ PASSED

- **analyze-perf-with-skills**: weighted score 76.7/120 (63.9%)
- **analyze-perf-baseline**: weighted score 75.1/120 (62.6%)

---
*Generated from vally eval results at C:\data\mycode\copilot-skill-eval-sample\vally01\vally-eval\results\2026-05-04T20-34-03-972Z*
