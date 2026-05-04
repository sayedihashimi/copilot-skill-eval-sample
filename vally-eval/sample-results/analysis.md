# Analysis Report: .NET Performance Analysis Skill Evaluation

**Runs:** 5 | **Configurations:** 2 | **Dimensions:** 12
**Date:** 2026-05-01 19:00 UTC

---

## Overview

Model: **claude-sonnet-4** | Judge: **gpt-4.1** | Threshold: **50%**

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
| CRITICAL | ×3 | 4 |
| HIGH | ×2 | 7 |
| MODERATE | ×1 | 1 |

**Maximum possible weighted score: 270**

---

## Executive Summary

Mean dimension scores across runs (0–10 scale, **higher is better**).
± values show standard deviation across runs.

| Dimension [Tier] | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| Regex Anti-Pattern Detection [CRITICAL] | 9.8 ± 0.5 | 9.1 ± 1.4 |
| String Allocation Detection [CRITICAL] | 8.9 | 9.1 ± 0.5 |
| Collection and LINQ Efficiency [CRITICAL] | 8.9 | 8.7 ± 0.5 |
| Async and IO Pattern Detection [CRITICAL] | 9.3 ± 0.6 | 9.1 ± 0.5 |
| Reflection and Serialization Overhead [HIGH] | 8.7 ± 0.5 | 8.9 ± 0.8 |
| Structural Optimization Detection [HIGH] | 9.8 ± 0.5 | 9.1 ± 0.5 |
| Aggregate and Replace Chain Detection [HIGH] | 4.2 ± 1.8 | 5.3 ± 4.3 |
| Span Usage Consistency [HIGH] | 6.9 ± 0.9 | 6.0 ± 2.2 |
| Inheritance Sealing Accuracy [HIGH] | 9.8 ± 0.5 | 9.6 ± 0.6 |
| Severity Classification Accuracy [HIGH] | 9.1 ± 0.5 | 8.9 |
| Fix Recommendation Quality [HIGH] | 8.9 | 9.3 ± 0.6 |
| Params Overload Optimization [MODERATE] | 3.6 ± 2.4 | 3.8 ± 3.6 |

---

## Final Rankings

Configurations ranked by weighted score — **higher is better**.

| Rank | Configuration | Mean Score ↑ | % of Max (270) | Std Dev ↓ | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | analyze-perf-baseline | 228.9 | 84.8% | 6.7 | 222.2 | 240.0 |
| 🥈 | analyze-perf-with-skills | 226.0 | 83.7% | 19.4 | 204.4 | 255.6 |

---

## Token Usage Summary

Average token consumption per configuration across all runs.

| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time | Δ Input vs Baseline |
|---|---|---|---|---|---|---|
| analyze-perf-baseline | 182,843 | 6,707 | 131,714 | 6 | 2m 23s | — (baseline) |
| analyze-perf-with-skills | 480,717 | 7,133 | 397,997 | 11 | 2m 31s | +162.9% |

### Token Usage Per Run

| Configuration | Run | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time |
|---|---|---|---|---|---|---|
| analyze-perf-baseline | 1 | 177,059 | 8,783 | 117,207 | 6 | 2m 50s |
| analyze-perf-baseline | 2 | 175,235 | 4,892 | 119,168 | 6 | 1m 52s |
| analyze-perf-baseline | 3 | 176,025 | 7,105 | 127,669 | 6 | 2m 33s |
| analyze-perf-baseline | 4 | 209,947 | 5,714 | 168,916 | 7 | 2m 10s |
| analyze-perf-baseline | 5 | 175,951 | 7,041 | 125,612 | 6 | 2m 32s |
| analyze-perf-with-skills | 1 | 439,501 | 7,873 | 372,423 | 11 | 2m 38s |
| analyze-perf-with-skills | 2 | 582,373 | 7,483 | 480,741 | 13 | 2m 41s |
| analyze-perf-with-skills | 3 | 252,621 | 5,475 | 205,574 | 8 | 2m 7s |
| analyze-perf-with-skills | 4 | 509,127 | 7,562 | 405,281 | 11 | 2m 38s |
| analyze-perf-with-skills | 5 | 619,961 | 7,271 | 525,964 | 13 | 2m 34s |


## Tool Usage Breakdown

| Configuration | Tool Calls | Turns | Skills Used | Tool Breakdown |
|---|---|---|---|---|
| analyze-perf-baseline | 16 | 1 | customize-cloud-agent | view(13), report_intent(1), glob(1), create(1) |
| analyze-perf-with-skills | 40 | 2 | analyzing-dotnet-performance, microbenchmarking, dotnet-trace-collect, customize-cloud-agent | grep(22), view(13), report_intent(2), skill(1), glob(1), create(1) |

---

## Per-Dimension Analysis

### 1. Regex Anti-Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 10.0 | 6.7 |
| 2 | 10.0 | 10.0 |
| 3 | 10.0 | 10.0 |
| 4 | 8.9 | 10.0 |
| 5 | 10.0 | 8.9 |
| **Mean** | **9.8** | **9.1** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 9.8/10
- ✅ **analyze-perf-with-skills**: 9.1/10

**Verdict:** **analyze-perf-baseline** leads (+0.7 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> The agent's regex anti-pattern detection is comprehensive and accurate, flagging all major issues (per-call instantiation, excessive Compiled usage) and recommending modern solutions like [GeneratedRegex]. The analysis is clear, actionable, and prioritizes regex issues appropriately, fully meeting the expectations for this dimension.

**analyze-perf-with-skills:**
> The analysis is strong in identifying excessive RegexOptions.Compiled usage and recommends modern alternatives like [GeneratedRegex], but it misses the crucial per-call Regex allocation anti-pattern, especially in hot paths. The recommendations are generally sound, but a truly comprehensive regex performance review would have flagged all three major issues in detail.

---

### 2. String Allocation Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 8.9 | 8.9 |
| 2 | 8.9 | 10.0 |
| 3 | 8.9 | 8.9 |
| 4 | 8.9 | 8.9 |
| 5 | 8.9 | 8.9 |
| **Mean** | **8.9** | **9.1** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 8.9/10
- ✅ **analyze-perf-with-skills**: 9.1/10

**Verdict:** **analyze-perf-with-skills** leads (+0.2 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> The report demonstrates a comprehensive understanding of string allocation issues, identifying all major patterns (string concatenation in loops, Replace chains, and culture-unaware casing). It provides actionable, specific recommendations and code examples. While a few details could be more granular, the coverage and clarity are excellent, justifying a high score.

**analyze-perf-with-skills:**
> The agent's analysis is highly comprehensive regarding string allocation issues. It identifies all major anti-patterns listed in the rubric, including string concatenation in loops, Replace chains, and culture-unaware casing. The findings are prioritized and mapped to specific files, and the summary highlights these as top priorities. The only minor deduction is for not always providing explicit file/line counts for every pattern in the summary, but the coverage and recommendations are otherwise exemplary.

---

### 3. Collection and LINQ Efficiency [CRITICAL × 3]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 8.9 | 8.9 |
| 2 | 8.9 | 8.9 |
| 3 | 8.9 | 8.9 |
| 4 | 8.9 | 8.9 |
| 5 | 8.9 | 7.8 |
| **Mean** | **8.9** | **8.7** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 8.9/10
- ✅ **analyze-perf-with-skills**: 8.7/10

**Verdict:** **analyze-perf-baseline** leads (+0.2 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> The agent's report provides a comprehensive and detailed analysis of collection and LINQ performance anti-patterns, identifying both obvious and subtle issues. It offers concrete recommendations and demonstrates awareness of the impact of collection misuse in hot paths. The only minor gap is the lack of explicit mention of sliding window inefficiencies, but overall, the coverage is excellent and actionable.

**analyze-perf-with-skills:**
> The analysis is comprehensive and demonstrates expert-level understanding of collection and LINQ performance pitfalls in .NET. It identifies both obvious and more nuanced inefficiencies, provides actionable fixes, and presents findings in a clear, structured manner. Minor omissions of the most subtle LINQ allocation patterns prevent a perfect score, but the report is highly effective and actionable.

---

### 4. Async and IO Pattern Detection [CRITICAL × 3]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 8.9 | 8.9 |
| 2 | 8.9 | 10.0 |
| 3 | 10.0 | 8.9 |
| 4 | 8.9 | 8.9 |
| 5 | 10.0 | 8.9 |
| **Mean** | **9.3** | **9.1** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 9.3/10
- ✅ **analyze-perf-with-skills**: 9.1/10

**Verdict:** **analyze-perf-baseline** leads (+0.2 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> The analysis demonstrates a thorough understanding of async/IO performance pitfalls in .NET. All major async/IO anti-patterns are detected and prioritized, with clear explanations and practical fixes. The report's structure and clarity further support its effectiveness, making it a strong example of expert-level async/IO performance review.

**analyze-perf-with-skills:**
> The agent provides a thorough and accurate analysis of async and IO anti-patterns, including all major issues (HttpClient misuse, sequential awaits, unbounded parallelism, and missing cancellation). The only minor gap is the lack of explicit mention of Task.Delay without cancellation, but this does not significantly detract from the overall quality. The structure and clarity of the output are excellent, supporting a high overall score.

---

### 5. Reflection and Serialization Overhead [HIGH × 2]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 7.8 | 7.8 |
| 2 | 8.9 | 8.9 |
| 3 | 8.9 | 8.9 |
| 4 | 8.9 | 8.9 |
| 5 | 8.9 | 10.0 |
| **Mean** | **8.7** | **8.9** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 8.7/10
- ✅ **analyze-perf-with-skills**: 8.9/10

**Verdict:** **analyze-perf-with-skills** leads (+0.2 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> The report demonstrates strong awareness of reflection and serialization overhead, correctly flags the most important issues, and provides actionable recommendations. The structure is clear and the prioritization is appropriate. The only deduction is for the lack of detailed, file-specific examples in the sample output, which would further strengthen the analysis and make it easier for developers to act on the findings.

**analyze-perf-with-skills:**
> The agent demonstrates a solid understanding of .NET reflection and serialization performance pitfalls, correctly flags the key issues, and provides actionable recommendations. The analysis is clear and well-structured, though it could be improved with more specific evidence and prioritization for these particular overheads. Overall, the report meets expectations for a performance expert review.

---

### 6. Structural Optimization Detection [HIGH × 2]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 8.9 | 8.9 |
| 2 | 10.0 | 8.9 |
| 3 | 10.0 | 10.0 |
| 4 | 10.0 | 8.9 |
| 5 | 10.0 | 8.9 |
| **Mean** | **9.8** | **9.1** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 9.8/10
- ✅ **analyze-perf-with-skills**: 9.1/10

**Verdict:** **analyze-perf-baseline** leads (+0.7 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> The analysis demonstrates a high level of awareness of structural optimization opportunities, including unsealed classes, struct boxing, and static dictionary optimizations. While not every single instance is named, the patterns are clearly identified, explained, and actionable recommendations are provided. The report shows expert-level understanding of .NET performance best practices in this area.

**analyze-perf-with-skills:**
> The report demonstrates strong detection of structural optimization issues, including unsealed leaf classes, missing IEquatable<T> on structs, and opportunities to use FrozenDictionary. It provides actionable recommendations and shows a clear understanding of why these patterns matter for .NET performance. Minor improvements could be made in providing more granular code examples and ensuring every instance is exhaustively listed, but overall the analysis is highly effective.

---

### 7. Aggregate and Replace Chain Detection [HIGH × 2]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 5.6 | 1.1 |
| 2 | 6.7 | 10.0 |
| 3 | 2.2 | 2.2 |
| 4 | 3.3 | 3.3 |
| 5 | 3.3 | 10.0 |
| **Mean** | **4.2** | **5.3** |

#### Score Comparison

- ❌ **analyze-perf-baseline**: 4.2/10
- ❌ **analyze-perf-with-skills**: 5.3/10

**Verdict:** **analyze-perf-with-skills** leads (+1.1 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> While the report demonstrates general awareness of string allocation issues and O(n²) patterns, it fails to explicitly identify and explain the .Aggregate() + .Replace() chain and the char.ToString() allocation in MetricFormatter.ExpandPrefixSymbols. This omission is significant given the subtlety and impact of this anti-pattern in a hot path. The structure and general clarity of the report are strong, but the lack of precise detection and explanation of this specific issue limits the overall score.

**analyze-perf-with-skills:**
> The agent failed to detect the subtle but important Aggregate+Replace chain and char.ToString() allocation issues in MetricFormatter.ExpandPrefixSymbols. These are high-impact, nuanced allocation patterns in hot code paths, and their omission is a significant gap in the analysis. The report only covers more generic string allocation issues and does not demonstrate awareness of this specific anti-pattern.

---

### 8. Span Usage Consistency [HIGH × 2]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 5.6 | 6.7 |
| 2 | 7.8 | 7.8 |
| 3 | 6.7 | 6.7 |
| 4 | 6.7 | 2.2 |
| 5 | 7.8 | 6.7 |
| **Mean** | **6.9** | **6.0** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 6.9/10
- ❌ **analyze-perf-with-skills**: 6.0/10

**Verdict:** **analyze-perf-baseline** leads (+0.9 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> While the report is generally well-structured and covers a broad range of performance issues, it misses critical span usage consistency findings required by the rubric. The absence of discussion around double allocations, inconsistent AsSpan usage, and heap-allocated List<char>[] means the analysis does not fully meet expectations for this dimension.

**analyze-perf-with-skills:**
> The analysis demonstrates awareness of Span usage consistency and related allocation issues, referencing key anti-patterns and at least one concrete example (List<char>[]). However, the summary lacks explicit, class-level detail for all truncator classes and does not directly cite the double-allocation in FixedLengthTruncator. The report is solid but would benefit from more precise, file-specific evidence and explicit cross-class comparison.

---

### 9. Inheritance Sealing Accuracy [HIGH × 2]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 10.0 | 8.9 |
| 2 | 10.0 | 10.0 |
| 3 | 10.0 | 10.0 |
| 4 | 10.0 | 8.9 |
| 5 | 8.9 | 10.0 |
| **Mean** | **9.8** | **9.6** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 9.8/10
- ✅ **analyze-perf-with-skills**: 9.6/10

**Verdict:** **analyze-perf-baseline** leads (+0.2 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> The agent demonstrates precise and accurate identification of which classes should be sealed, correctly flagging only the appropriate unsealed leaf classes and avoiding false positives on base or abstract classes. This shows a strong understanding of .NET inheritance best practices and the performance implications of sealing. No critical mistakes were made in this area.

**analyze-perf-with-skills:**
> The agent demonstrates strong accuracy in inheritance sealing recommendations, especially by not flagging DefaultOrdinalizer or the abstract base for sealing. While the summary does not explicitly list the three language-specific Ordinalizer subclasses as findings, there is no evidence they were missed, and the structural analysis appears thorough. The agent avoids critical false positives, showing a high level of precision.

---

### 10. Severity Classification Accuracy [HIGH × 2]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 10.0 | 8.9 |
| 2 | 8.9 | 8.9 |
| 3 | 8.9 | 8.9 |
| 4 | 8.9 | 8.9 |
| 5 | 8.9 | 8.9 |
| **Mean** | **9.1** | **8.9** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 9.1/10
- ✅ **analyze-perf-with-skills**: 8.9/10

**Verdict:** **analyze-perf-baseline** leads (+0.2 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> The agent's severity classification is exemplary: critical issues are correctly identified and prioritized, while moderate and info-level issues are appropriately ranked lower. The distinctions are clear, actionable, and reflect real-world performance impact, ensuring developers can effectively prioritize their efforts.

**analyze-perf-with-skills:**
> The agent demonstrates a strong grasp of severity classification, correctly elevating critical issues and distinguishing them from moderate and informational findings. The prioritization aligns with real-world performance impact, ensuring developers can focus on the most urgent problems. Minor improvements could be made in the granularity of moderate classifications, but the overall severity assessment is highly reliable.

---

### 11. Fix Recommendation Quality [HIGH × 2]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 8.9 | 8.9 |
| 2 | 8.9 | 10.0 |
| 3 | 8.9 | 8.9 |
| 4 | 8.9 | 10.0 |
| 5 | 8.9 | 8.9 |
| **Mean** | **8.9** | **9.3** |

#### Score Comparison

- ✅ **analyze-perf-baseline**: 8.9/10
- ✅ **analyze-perf-with-skills**: 9.3/10

**Verdict:** **analyze-perf-with-skills** leads (+0.4 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> The fix recommendations are highly actionable, reference the correct .NET APIs, and avoid unsafe or incorrect advice. The structure and clarity of the report are excellent. The only minor deduction is for a few info-level findings that could be more specific or include code, but all critical and moderate issues are addressed with strong, concrete recommendations.

**analyze-perf-with-skills:**
> The agent's fix recommendations are highly actionable, reference the correct .NET APIs, and include code examples, which accelerates developer productivity. The structure is clear and matches the requested format. The only minor deduction is due to the lack of direct evidence for every single recommendation, but the overall quality is excellent and avoids vague or incorrect advice.

---

### 12. Params Overload Optimization [MODERATE × 1]

#### Scores Across Runs

| Run | analyze-perf-baseline | analyze-perf-with-skills |
|---|---|---|
| 1 | 2.2 | 2.2 |
| 2 | 7.8 | 10.0 |
| 3 | 3.3 | 3.3 |
| 4 | 2.2 | 2.2 |
| 5 | 2.2 | 1.1 |
| **Mean** | **3.6** | **3.8** |

#### Score Comparison

- ❌ **analyze-perf-baseline**: 3.6/10
- ❌ **analyze-perf-with-skills**: 3.8/10

**Verdict:** **analyze-perf-with-skills** leads (+0.2 vs others).

#### Judge Evidence

**analyze-perf-baseline:**
> The analysis fails to address the params overload optimization issue, which is a key .NET performance pattern and was explicitly required by the rubric. While the report is otherwise clear and thorough, this omission represents a critical gap in the agent's performance review for this dimension.

**analyze-perf-with-skills:**
> While the report is generally well-structured and covers many performance issues, it fails to address the specific and important params overload optimization for TruncationPipeline.Apply. This omission means the analysis does not meet expectations for this dimension, resulting in a low overall score.

---

## Consistency Analysis

Score σ (standard deviation) measures how much a configuration's weighted score varies across runs — **lower is better**.

| Configuration | Score σ | Most Consistent Dim (σ) | Most Variable Dim (σ) |
|---|---|---|---|
| analyze-perf-baseline | 6.7 | String Allocation Detection (0.0) | Params Overload Optimization (2.4) |
| analyze-perf-with-skills | 19.4 | Severity Classification Accuracy (0.0) | Aggregate and Replace Chain Detection (4.3) |

---

## Summary

**Overall eval score: 82.6%** (threshold: 50%) — ✅ PASSED

- **analyze-perf-baseline**: weighted score 228.9/270 (84.8%)
- **analyze-perf-with-skills**: weighted score 226.0/270 (83.7%)

---
*Generated from vally eval results at C:\data\mycode\copilot-skill-eval-sample\vally01\vally-eval\results\2026-05-01T18-28-46-443Z*
