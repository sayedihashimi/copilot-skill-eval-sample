# Build & Run Verification Report

**Evaluation:** .NET 11 Feature Adoption Evaluation
**Date:** 2026-04-17 20:30 UTC
**Configurations:** 1
**Scenarios:** 4
**Total projects:** 6

## Results

| Configuration | Run | Scenario | Build | Run | Format | Security | Notes |
|---|---|---|---|---|---|---|---|
| dotnet-net11-skill | 1 | console-bcl | ❌ Fail | ⏭️ Skipped | ⏭️ Skipped | ⏭️ Skipped | MSBUILD : error MSB1003: Specify a project or solution file. The current working directory does not  |
| dotnet-net11-skill | 1 | webapi | ❌ Fail | ⏭️ Skipped | ⏭️ Skipped | ⏭️ Skipped | MSBUILD : error MSB1003: Specify a project or solution file. The current working directory does not  |
| dotnet-net11-skill | 1 | blazor | ❌ Fail | ⏭️ Skipped | ⏭️ Skipped | ⏭️ Skipped | MSBUILD : error MSB1003: Specify a project or solution file. The current working directory does not  |
| dotnet-net11-skill | 1 | efcore | ✅ Pass | ✅ Pass | ⏭️ Skipped | ⏭️ Skipped |  |
| dotnet-net11-skill | 2 | console-bcl | ✅ Pass | ✅ Pass | ⏭️ Skipped | ⏭️ Skipped |  |
| dotnet-net11-skill | 3 | webapi | ✅ Pass | ✅ Pass | ⏭️ Skipped | ⏭️ Skipped |  |

## Asset Usage Per Run

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| dotnet-net11-skill | 1 | f050d290…e4cf | claude-opus-4.6 | — | — | ✅ |
| dotnet-net11-skill | 2 | c1add1c3…2cfc | claude-opus-4.6 | — | — | ✅ |
| dotnet-net11-skill | 3 | 66727453…6720 | claude-opus-4.6 | — | — | ✅ |

## Skill Configurations

| Configuration | Label | Skills | Plugins |
|---|---|---|---|
| dotnet-net11-skill | dotnet-net11 Skill | None | dotnet-net11:dotnet-net11 |
