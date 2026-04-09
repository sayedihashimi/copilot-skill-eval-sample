# Build & Run Verification Report

**Evaluation:** .NET Performance Analysis Skill Evaluation
**Date:** 2026-04-09 19:56 UTC
**Configurations:** 2
**Scenarios:** 1
**Total projects:** 2

## Results

| Configuration | Run | Scenario | Build | Run | Format | Security | Notes |
|---|---|---|---|---|---|---|---|
| no-skills | 1 | analyze-perf-issues | ❌ Fail | ⏭️ Skipped | ⏭️ Skipped | ⏭️ Skipped | MSBUILD : error MSB1003: Specify a project or solution file. The current working directory does not  |
| dotnet-perf-skills | 1 | analyze-perf-issues | ❌ Fail | ⏭️ Skipped | ⏭️ Skipped | ⏭️ Skipped | MSBUILD : error MSB1003: Specify a project or solution file. The current working directory does not  |

## Asset Usage Per Run

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| no-skills | 1 | af06eebf…b044 | claude-opus-4.6 | — | — | ✅ |
| dotnet-perf-skills | 1 | — | — | — | — | ✅ |

## Skill Configurations

| Configuration | Label | Skills | Plugins |
|---|---|---|---|
| no-skills | Baseline (default Copilot) | None | None |
| dotnet-perf-skills | dotnet/skills Performance Skills | None | dotnet-skills:dotnet-diag |
