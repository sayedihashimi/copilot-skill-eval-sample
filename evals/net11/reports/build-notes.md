# Build & Run Verification Report

**Evaluation:** .NET 11 Feature Adoption Evaluation
**Date:** 2026-04-16 23:59 UTC
**Configurations:** 2
**Scenarios:** 4
**Total projects:** 2

## Results

| Configuration | Run | Scenario | Build | Run | Format | Security | Notes |
|---|---|---|---|---|---|---|---|
| no-skills | 1 | console-bcl | ✅ Pass | ✅ Pass | ⏭️ Skipped | ⏭️ Skipped |  |
| dotnet-net11-skill | 1 | console-bcl | ✅ Pass | ✅ Pass | ⏭️ Skipped | ⏭️ Skipped |  |

## Automated Metrics

### Build Warnings by Category

| Configuration | Scenario | Total | Naming | Performance | Reliability | Security | Usage | Style | Other |
|---|---|---|---|---|---|---|---|---|---|
| dotnet-net11-skill | console-bcl | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 2 |

## Asset Usage Per Run

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| no-skills | 1 | f7c66703…5724 | claude-opus-4.6 | — | — | ✅ |
| dotnet-net11-skill | 1 | 5dcd99cd…377e | claude-opus-4.6 | — | — | ✅ |

## Skill Configurations

| Configuration | Label | Skills | Plugins |
|---|---|---|---|
| no-skills | Baseline (default Copilot) | None | None |
| dotnet-net11-skill | dotnet-net11 Skill | None | dotnet-net11:dotnet-net11 |
