# Auto-Improve Bug Report — net11 eval

**Date:** 2026-04-21  
**Eval:** `_main\evals\net11\eval.yaml`  
**Configuration:** `dotnet-net11-skill`  
**Command:** `python -m skill_eval auto-improve -c dotnet-net11-skill --target-score 700`  
**CWD:** `C:\data\mycode\copilot-skill-eval-sample\_main\evals\net11`  
**Total runtime:** 2h 9m 12s  

---

## Summary

The auto-improve loop completed 2 turns (with 3 retries on Turn 2) and plateaued.
The best score was **152.5** (Turn 1), far below the 700 target. Turn 2 scored 141.0, 
all 3 retries also scored below Turn 1, so the system rolled back to Turn 1's state.

---

## Bug 1: Only 1 of 4 scenarios generated per iteration (runs-per-iteration=1)

**Severity:** High  
**Impact:** Scores on 20+ dimensions are effectively 1/5 because the relevant scenario was never generated.

The auto-improve loop defaults to `--runs-per-iteration 1`, but there are 4 scenarios
(console-bcl, webapi, blazor, efcore). With round-robin selection, each turn only generates
1 scenario, leaving 3 scenarios un-generated. This means:

- Turn 1 generated `console-bcl` only → all webapi/blazor/efcore dimensions scored 1
- Turn 2 generated `webapi` only → all console-bcl/blazor/efcore dimensions scored 1

The warning is printed: `⚠️ Warning: --runs (1) < scenarios (4) — 3 scenario(s) will NOT be tested.`
but auto-improve doesn't automatically increase runs-per-iteration.

**Suggestion:** Auto-improve should default `--runs-per-iteration` to the number of scenarios
when not explicitly set, or at minimum warn more prominently and suggest the correct value.

---

## Bug 2: Copilot CLI exits with code 1 but treated as success

**Severity:** Medium  
**Impact:** Potential silent failures in code generation.

The generation step logged:
```
⚠️  Copilot exited with code 1 but output files exist — treating as success
```

While the heuristic of checking for output files is reasonable, exit code 1 may indicate
partial generation or errors. The generated code for 3 of 4 scenarios (webapi, blazor, efcore)
contained only placeholder/stub files (`.csproj.lscache` files, no actual source code),
suggesting the Copilot CLI may have failed to generate these.

**Suggestion:** Investigate why Copilot CLI returns exit code 1. Consider validating that
generated output contains actual source files (not just cache stubs) before declaring success.

---

## Bug 3: Build failures on 3 of 4 scenarios (webapi, blazor, efcore) in Turn 1

**Severity:** High  
**Impact:** Dimensions relying on those scenarios cannot score above 1.

Build results for Turn 1:
- `console-bcl`: Build ✅ Pass, Run ✅ Pass
- `webapi`: Build ❌ Fail, Run ⏭️ Skipped
- `blazor`: Build ❌ Fail, Run ⏭️ Skipped
- `efcore`: Build ❌ Fail, Run ⏭️ Skipped

This appears related to Bug 2 — the scenarios only had stub/cache files rather than
actual source code. The Copilot CLI session only generated the `console-bcl` project.

---

## Bug 4: Score plateau despite significant room for improvement

**Severity:** Medium  
**Impact:** Auto-improve cannot make progress toward the 700 target.

After Turn 1 scored 152.5/700, the improvement suggestions were generated and applied,
but Turn 2 scored only 141.0 (a regression). All 3 retries also failed to improve.
The effective improvement was 0%.

Root cause analysis:
1. With only 1 scenario per turn, improvements to the skill for one scenario may
   regress another scenario's generation.
2. The improvement model's suggestions may not be effective for the plugin structure.
3. The scoring scale (max 185 possible with 37 dimensions × 5 max × weighted tiers)
   versus the 700 target suggests a mismatch. The max possible weighted score with
   37 dimensions appears to be around 185 (critical×3 + high×2 + medium×1 + low×0.5),
   making a target of 700 unreachable.

**Suggestion:** Verify the scoring scale. If max possible is ~185, then `--target-score 700`
is impossible to reach. The loop will always exhaust all turns and retries.

---

## Bug 5: No skills loaded despite plugin being configured

**Severity:** Medium  
**Impact:** The plugin may not be properly activating during generation.

The generation log shows:
```
📋 No skills loaded
✅ All skills/plugins match config
```

This is contradictory — the config specifies `dotnet-net11` plugin, but "No skills loaded"
is reported. The `All skills/plugins match config` check passes because the config
uses `plugins:` not `skills:`, but the plugin's skills directory may not be loaded by
Copilot CLI.

**Suggestion:** Investigate whether the plugin's skills are actually being loaded and
influencing code generation. If skills aren't loaded, the eval is comparing
"Copilot without skill" vs "Copilot without skill" (no difference from baseline).

---

## Generated Files Verification

### Reports directory (all expected files present ✅)

| File | Size | Status |
|------|------|--------|
| `analysis.md` | 28,271 | ✅ Present |
| `analysis-run-1.md` | 19,397 | ✅ Present |
| `auto-improve-results.md` | 14,258 | ✅ Present |
| `auto-improve-history.json` | 4,428 | ✅ Present |
| `auto-improve-dotnet-net11-skill.patch` | 35,917 | ✅ Present (from prior run) |
| `build-notes.md` | 1,418 | ✅ Present |
| `generation-usage.json` | 684 | ✅ Present |
| `improvements-dotnet-net11-skill.md` | 20,830 | ✅ Present |
| `lessons-learned-dotnet-net11-skill.json` | 20,315 | ✅ Present |
| `scores-data.json` | 3,573 | ✅ Present |
| `verification-data.json` | 2,595 | ✅ Present |

### Output directory (generated code)

| Scenario | Source Files | Build | Run |
|----------|-------------|-------|-----|
| `console-bcl` | ✅ Full project | ✅ Pass | ✅ Pass |
| `webapi` | ⚠️ Stubs only (Turn 1) | ❌ Fail | ⏭️ Skip |
| `blazor` | ⚠️ Stubs only | ❌ Fail | ⏭️ Skip |
| `efcore` | ⚠️ Stubs only | ❌ Fail | ⏭️ Skip |

### Backup directory

| Backup | Status |
|--------|--------|
| `best-turn-1` | ✅ Present |
| `turn-1` | ✅ Present |
| `turn-2` | ✅ Present |
| `turn-2-retry-1` | ✅ Present |
| `turn-2-retry-2` | ✅ Present |
| `turn-2-retry-3` | ✅ Present |

---

## Score Progression

| Turn | Score | Delta | Status |
|-----:|------:|------:|--------|
| 1 | 152.50 | — | ✅ Improvements applied |
| 2 | 141.00 | -11.50 | 📊 Plateau (all 3 retries exhausted) |

**Final effective score:** 152.50 (rolled back to Turn 1)
