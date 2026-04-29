# Build & Run Verification Report

**Evaluation:** .NET 11 Feature Adoption Evaluation
**Date:** 2026-04-21 17:13 UTC
**Configurations:** 1
**Scenarios:** 4
**Total projects:** 4

## Results

| Configuration | Run | Scenario | Build | Run | Format | Security | Notes |
|---|---|---|---|---|---|---|---|
| dotnet-net11-skill | 1 | console-bcl | ❌ Fail | ⏭️ Skipped | ⏭️ Skipped | ⏭️ Skipped | MSBUILD : error MSB1003: Specify a project or solution file. The current working directory does not  |
| dotnet-net11-skill | 1 | webapi | ✅ Pass | ✅ Pass | ⏭️ Skipped | ⏭️ Skipped |  |
| dotnet-net11-skill | 1 | blazor | ❌ Fail | ⏭️ Skipped | ⏭️ Skipped | ⏭️ Skipped | MSBUILD : error MSB1003: Specify a project or solution file. The current working directory does not  |
| dotnet-net11-skill | 1 | efcore | ❌ Fail | ⏭️ Skipped | ⏭️ Skipped | ⏭️ Skipped | MSBUILD : error MSB1003: Specify a project or solution file. The current working directory does not  |

## Asset Usage Per Run

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| dotnet-net11-skill | 1 | b953c0e1…1c7f | claude-opus-4.6 | — | — | ✅ |

## Skill Configurations

| Configuration | Label | Skills | Plugins |
|---|---|---|---|
| dotnet-net11-skill | dotnet-net11 Skill | None | dotnet-net11:dotnet-net11 |
