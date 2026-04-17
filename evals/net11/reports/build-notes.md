# Build & Run Verification Report

**Evaluation:** .NET 11 Feature Adoption Evaluation
**Date:** 2026-04-17 05:42 UTC
**Configurations:** 2
**Scenarios:** 4
**Total projects:** 2

## Results

| Configuration | Run | Scenario | Build | Run | Format | Security | Notes |
|---|---|---|---|---|---|---|---|
| no-skills | 1 | efcore | ✅ Pass | ✅ Pass | ⏭️ Skipped | ⏭️ Skipped |  |
| dotnet-net11-skill | 1 | blazor | ❌ Fail | ⏭️ Skipped | ⏭️ Skipped | ⏭️ Skipped |   Determining projects to restore...   Restored C:\data\mycode\copilot-skill-eval-sample\_main\evals |

## Asset Usage Per Run

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| no-skills | 1 | 55554d8c…c97c | claude-opus-4.6 | — | — | ✅ |
| dotnet-net11-skill | 1 | c9a6c88a…0157 | claude-opus-4.6 | — | — | ✅ |

## Skill Configurations

| Configuration | Label | Skills | Plugins |
|---|---|---|---|
| no-skills | Baseline (default Copilot) | None | None |
| dotnet-net11-skill | dotnet-net11 Skill | None | dotnet-net11:dotnet-net11 |
