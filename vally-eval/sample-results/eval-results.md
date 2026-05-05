## Eval Results

> Timestamp: 2026-05-04T21:17:19.464Z

Evaluate how the dotnet/skills performance-related skills (analyzing-dotnet-performance, microbenchmarking, dotnet-trace-collect) improve Copilot's ability to detect performance anti-patterns in existing .NET code compared to baseline Copilot. 

| Stimulus | Environment | Skills | Graders | Pass Rate | pass@k | pass^k | Duration | Tokens | Verdict |
|---|---|---|---|---|---|---|---|---|---|
| analyze-perf-baseline | <details><summary>1 file</summary>Files: `C:\data\mycode\copilot-skill-eval-sample\vally01\samples\perf01` → `perf01`</details> | — | ✅ regex-antipattern-detection 5/5<br>✅ string-allocation-detection 5/5<br>✅ collection-linq-efficiency 5/5<br>✅ async-io-pattern-detection 5/5<br>✅ reflection-serialization-overhead 5/5<br>✅ structural-optimization-detection 5/5<br>❌ aggregate-replace-chain-detection 1/5<br>❌ span-usage-consistency 3/5<br>❌ inheritance-sealing-accuracy 1/5<br>✅ severity-classification-accuracy 5/5<br>✅ fix-recommendation-quality 5/5<br>❌ params-overload-optimization 0/5<br>✅ output-file-created 5/5 | 0/5 | 0.0% | 0.0% | 8m 28s | 156,965 | ❌ <a href="#user-content-fn-1" id="ref-1">[1]</a> |
| analyze-perf-with-skills | <details><summary>1 file · 3 skills</summary>Files: `C:\data\mycode\copilot-skill-eval-sample\vally01\samples\perf01` → `perf01`<br/>Skills: `C:\data\mycode\skills\_main\plugins\dotnet-diag\skills\analyzing-dotnet-performance`, `C:\data\mycode\skills\_main\plugins\dotnet-diag\skills\microbenchmarking`, `C:\data\mycode\skills\_main\plugins\dotnet-diag\skills\dotnet-trace-collect`</details> | `analyzing-dotnet-performance` | ✅ regex-antipattern-detection 5/5<br>✅ string-allocation-detection 5/5<br>❌ collection-linq-efficiency 4/5<br>✅ async-io-pattern-detection 5/5<br>✅ reflection-serialization-overhead 5/5<br>✅ structural-optimization-detection 5/5<br>❌ aggregate-replace-chain-detection 1/5<br>❌ span-usage-consistency 4/5<br>❌ inheritance-sealing-accuracy 0/5<br>✅ severity-classification-accuracy 5/5<br>✅ fix-recommendation-quality 5/5<br>❌ params-overload-optimization 2/5<br>✅ output-file-created 5/5 | 0/5 | 0.0% | 0.0% | 8m 43s | 244,999 | ❌ <a href="#user-content-fn-2" id="ref-2">[2]</a> |

<a href="#user-content-ref-1" id="fn-1"><strong>[1]</strong></a> Grader breakdown: `aggregate-replace-chain-detection` passed 1/5 trials, `span-usage-consistency` passed 3/5 trials, `inheritance-sealing-accuracy` passed 1/5 trials
<a href="#user-content-ref-2" id="fn-2"><strong>[2]</strong></a> Grader breakdown: `collection-linq-efficiency` passed 4/5 trials, `aggregate-replace-chain-detection` passed 1/5 trials, `span-usage-consistency` passed 4/5 trials, `params-overload-optimization` passed 2/5 trials


> Model: claude-opus-4.6 | Judge: gpt-5.4
