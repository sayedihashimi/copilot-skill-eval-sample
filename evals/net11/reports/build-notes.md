# Build & Run Verification Report

**Evaluation:** .NET 11 Feature Adoption Evaluation
**Date:** 2026-04-17 05:02 UTC
**Configurations:** 1
**Scenarios:** 4
**Total projects:** 2

## Results

| Configuration | Run | Scenario | Build | Run | Format | Security | Notes |
|---|---|---|---|---|---|---|---|
| dotnet-net11-skill | 1 | webapi | ✅ Pass | ✅ Pass | ⏭️ Skipped | ⏭️ Skipped |  |
| dotnet-net11-skill | 1 | efcore | ❌ Fail | ⏭️ Skipped | ⏭️ Skipped | ⏭️ Skipped | MSBUILD : error MSB1003: Specify a project or solution file. The current working directory does not  |

## Asset Usage Per Run

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| dotnet-net11-skill | 1 | a1c49ea3…62a3 | claude-opus-4.6 | — | — | ✅ |

## Skill Configurations

| Configuration | Label | Skills | Plugins |
|---|---|---|---|
| dotnet-net11-skill | dotnet-net11 Skill | None | dotnet-net11:dotnet-net11 |
