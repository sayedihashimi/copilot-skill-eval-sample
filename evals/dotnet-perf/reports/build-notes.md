# Build & Run Verification Report

**Evaluation:** .NET Performance Analysis Skill Evaluation
**Date:** 2026-04-09 20:55 UTC
**Configurations:** 2
**Scenarios:** 1
**Total projects:** 6

## Results

| Configuration | Run | Scenario | Build | Run | Format | Security | Notes |
|---|---|---|---|---|---|---|---|
| no-skills | 1 | analyze-perf-issues | ❌ Fail | ⏭️ Skipped | ⏭️ Skipped | ⏭️ Skipped | MSBUILD : error MSB1003: Specify a project or solution file. The current working directory does not  |
| no-skills | 2 | analyze-perf-issues | ❌ Fail | ⏭️ Skipped | ⏭️ Skipped | ⏭️ Skipped | MSBUILD : error MSB1003: Specify a project or solution file. The current working directory does not  |
| no-skills | 3 | analyze-perf-issues | ❌ Fail | ⏭️ Skipped | ⏭️ Skipped | ⏭️ Skipped | MSBUILD : error MSB1003: Specify a project or solution file. The current working directory does not  |
| dotnet-perf-skills | 1 | analyze-perf-issues | ❌ Fail | ⏭️ Skipped | ⏭️ Skipped | ⏭️ Skipped | MSBUILD : error MSB1003: Specify a project or solution file. The current working directory does not  |
| dotnet-perf-skills | 2 | analyze-perf-issues | ❌ Fail | ⏭️ Skipped | ⏭️ Skipped | ⏭️ Skipped | MSBUILD : error MSB1003: Specify a project or solution file. The current working directory does not  |
| dotnet-perf-skills | 3 | analyze-perf-issues | ❌ Fail | ⏭️ Skipped | ⏭️ Skipped | ⏭️ Skipped | MSBUILD : error MSB1003: Specify a project or solution file. The current working directory does not  |

## Asset Usage Per Run

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| no-skills | 2 | 1c610bb9…9d2b | claude-opus-4.6 | — | — | ✅ |
| no-skills | 3 | a6c39e40…7b35 | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills | 1 | — | — | — | — | ✅ |
| dotnet-perf-skills | 2 | 1e857169…6a28 | claude-opus-4.6 | analyzing-dotnet-performance | dotnet-diag | ✅ |
| dotnet-perf-skills | 3 | — | — | — | — | ✅ |

## Skill Configurations

| Configuration | Label | Skills | Plugins |
|---|---|---|---|
| no-skills | Baseline (default Copilot) | None | None |
| dotnet-perf-skills | dotnet/skills Performance Skills | None | dotnet-skills:dotnet-diag |
