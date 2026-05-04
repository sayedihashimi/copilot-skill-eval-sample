#!/usr/bin/env node
// generate-report.js — Generates an analysis.md report from vally eval results.
//
// Usage:
//   node generate-report.js <results-dir>
//   node generate-report.js results/2026-05-01T17-23-16-400Z
//
// Reads results.jsonl from the given directory and produces analysis.md
// in the same directory.

import { readFileSync, writeFileSync } from "node:fs";
import { join, resolve } from "node:path";

const resultsDir = resolve(process.argv[2] || ".");
const jsonlPath = join(resultsDir, "results.jsonl");
const outputPath = join(resultsDir, "analysis.md");

// ---------------------------------------------------------------------------
// Parse results.jsonl
// ---------------------------------------------------------------------------
const lines = readFileSync(jsonlPath, "utf8").trim().split("\n");
const trajectoryEntries = [];
let runSummary = null;

for (const line of lines) {
  const obj = JSON.parse(line);
  if (obj.type === "run-summary") {
    runSummary = obj;
  } else if (obj.status === "success" && obj.trajectory) {
    trajectoryEntries.push(obj);
  }
}

if (trajectoryEntries.length === 0) {
  console.error("No trajectory entries found in results.jsonl");
  process.exit(1);
}

// ---------------------------------------------------------------------------
// Tier/weight mapping (mirrors the eval.yaml comments)
// ---------------------------------------------------------------------------
const TIER_MAP = {
  "regex-antipattern-detection": { tier: "MODERATE", weight: 1 },
  "string-allocation-detection": { tier: "MODERATE", weight: 1 },
  "collection-linq-efficiency": { tier: "MODERATE", weight: 1 },
  "async-io-pattern-detection": { tier: "MODERATE", weight: 1 },
  "reflection-serialization-overhead": { tier: "MODERATE", weight: 1 },
  "structural-optimization-detection": { tier: "MODERATE", weight: 1 },
  "aggregate-replace-chain-detection": { tier: "MODERATE", weight: 1 },
  "span-usage-consistency": { tier: "MODERATE", weight: 1 },
  "inheritance-sealing-accuracy": { tier: "MODERATE", weight: 1 },
  "severity-classification-accuracy": { tier: "MODERATE", weight: 1 },
  "fix-recommendation-quality": { tier: "MODERATE", weight: 1 },
  "params-overload-optimization": { tier: "MODERATE", weight: 1 },
  "output-file-created": { tier: "GATE", weight: 0 },
};

// Human-readable grader names
const DISPLAY_NAMES = {
  "regex-antipattern-detection": "Regex Anti-Pattern Detection",
  "string-allocation-detection": "String Allocation Detection",
  "collection-linq-efficiency": "Collection and LINQ Efficiency",
  "async-io-pattern-detection": "Async and IO Pattern Detection",
  "reflection-serialization-overhead": "Reflection and Serialization Overhead",
  "structural-optimization-detection": "Structural Optimization Detection",
  "aggregate-replace-chain-detection": "Aggregate and Replace Chain Detection",
  "span-usage-consistency": "Span Usage Consistency",
  "inheritance-sealing-accuracy": "Inheritance Sealing Accuracy",
  "severity-classification-accuracy": "Severity Classification Accuracy",
  "fix-recommendation-quality": "Fix Recommendation Quality",
  "params-overload-optimization": "Params Overload Optimization",
  "output-file-created": "Output File Created",
};

// ---------------------------------------------------------------------------
// Helpers
// ---------------------------------------------------------------------------
function toScale10(score01) {
  return score01 * 10;
}

function fmt(n, decimals = 1) {
  return n.toFixed(decimals);
}

function fmtDuration(ms) {
  const s = Math.round(ms / 1000);
  const m = Math.floor(s / 60);
  const sec = s % 60;
  return m > 0 ? `${m}m ${sec}s` : `${sec}s`;
}

function fmtNumber(n) {
  return n.toLocaleString("en-US");
}

function mean(arr) {
  return arr.reduce((a, b) => a + b, 0) / arr.length;
}

function stddev(arr) {
  if (arr.length < 2) return 0;
  const m = mean(arr);
  return Math.sqrt(arr.reduce((sum, v) => sum + (v - m) ** 2, 0) / (arr.length - 1));
}

function pctChange(baseline, other) {
  if (baseline === 0) return "N/A";
  const pct = ((other - baseline) / baseline) * 100;
  return (pct >= 0 ? "+" : "") + fmt(pct, 1) + "%";
}

// ---------------------------------------------------------------------------
// Group entries by stimulus (configuration)
// ---------------------------------------------------------------------------
const byStimulus = new Map();
for (const entry of trajectoryEntries) {
  const name = entry.trajectory.stimulus?.name ?? "unknown";
  if (!byStimulus.has(name)) byStimulus.set(name, []);
  byStimulus.get(name).push(entry);
}

const stimulusNames = [...byStimulus.keys()].sort((a, b) => {
  // Put baseline (no skills) first
  const aBaseline = a.toLowerCase().includes("baseline") ? 0 : 1;
  const bBaseline = b.toLowerCase().includes("baseline") ? 0 : 1;
  return aBaseline - bBaseline;
});
const runs = Math.max(...[...byStimulus.values()].map((v) => v.length));

// Extract eval metadata from run summary
const evalName = runSummary?.evals?.[0]?.name ?? "Vally Evaluation";
const evalModel = runSummary?.evals?.[0]?.model ?? "unknown";
const evalThreshold = runSummary?.evals?.[0]?.threshold ?? 0.5;
const overallScore = runSummary?.evals?.[0]?.overallScore;
const timestamp = new Date().toISOString().replace("T", " ").substring(0, 16) + " UTC";

// ---------------------------------------------------------------------------
// Build per-stimulus score data
// ---------------------------------------------------------------------------
function getStimulusScores(entries) {
  const graderScores = {};
  const metrics = [];

  for (const entry of entries) {
    const gr = entry.gradeResult;
    if (gr?.details) {
      for (const d of gr.details) {
        if (!graderScores[d.name]) graderScores[d.name] = [];
        graderScores[d.name].push({ score: d.score, passed: d.passed, evidence: d.evidence });
      }
    }
    if (entry.trajectory?.metrics) {
      metrics.push(entry.trajectory.metrics);
    }
  }

  return { graderScores, metrics };
}

const stimulusData = new Map();
for (const [name, entries] of byStimulus) {
  stimulusData.set(name, getStimulusScores(entries));
}

// ---------------------------------------------------------------------------
// Compute weighted scores
// ---------------------------------------------------------------------------
function computeWeightedScore(graderScores) {
  let totalWeight = 0;
  let weightedSum = 0;
  for (const [name, scores] of Object.entries(graderScores)) {
    const tier = TIER_MAP[name];
    if (!tier || tier.weight === 0) continue;
    const avgScore = mean(scores.map((s) => toScale10(s.score)));
    weightedSum += avgScore * tier.weight;
    totalWeight += tier.weight;
  }
  return { weightedSum, totalWeight, maxPossible: totalWeight * 10 };
}

// ---------------------------------------------------------------------------
// Generate report
// ---------------------------------------------------------------------------
const out = [];
const w = (s) => out.push(s);

// Header
w(`# Analysis Report: ${evalName}`);
w("");
w(`**Runs:** ${runs} | **Configurations:** ${stimulusNames.length} | **Dimensions:** ${Object.keys(TIER_MAP).filter((k) => TIER_MAP[k].weight > 0).length}`);
w(`**Date:** ${timestamp}`);
w("");
w("---");
w("");

// Overview
if (runSummary?.evals?.[0]) {
  const evalInfo = runSummary.evals[0];
  w("## Overview");
  w("");
  w(`Model: **${evalModel}** | Judge: **${evalInfo.judgeModel ?? "gpt-4.1"}** | Threshold: **${fmt(evalThreshold * 100, 0)}%**`);
  w("");
  w("---");
  w("");
}

// Configurations
w("## Configurations");
w("");
w("| Configuration | Skills Loaded | Tags |");
w("|---|---|---|");
for (const name of stimulusNames) {
  const entries = byStimulus.get(name);
  const skills = entries[0]?.trajectory?.metadata?.skillsLoaded ?? [];
  const tags = entries[0]?.trajectory?.stimulus?.tags ?? {};
  const tagStr = Object.entries(tags).map(([k, v]) => `${k}=${v}`).join(", ") || "—";
  w(`| ${name} | ${skills.length > 0 ? skills.join(", ") : "— (baseline)"} | ${tagStr} |`);
}
w("");
w("---");
w("");

// Scoring methodology
w("## Scoring Methodology");
w("");
w("Each dimension is scored on a **0–10 scale** by an LLM judge. Dimensions are grouped into tiers:");
w("");
w("| Tier | Weight | Dimensions |");
w("|---|:---:|:---:|");
const tierCounts = {};
for (const t of Object.values(TIER_MAP)) {
  if (t.weight === 0) continue;
  tierCounts[t.tier] = (tierCounts[t.tier] || 0) + 1;
}
for (const [tier, count] of Object.entries(tierCounts)) {
  const weight = Object.values(TIER_MAP).find((t) => t.tier === tier)?.weight;
  w(`| ${tier} | ×${weight} | ${count} |`);
}
const maxWeighted = computeWeightedScore(Object.fromEntries(
  Object.keys(TIER_MAP).filter((k) => TIER_MAP[k].weight > 0).map((k) => [k, [{ score: 1.0 }]])
));
w("");
w(`**Maximum possible weighted score: ${fmt(maxWeighted.maxPossible, 0)}**`);
w("");
w("---");
w("");

// Executive summary — dimension scores table
w("## Executive Summary");
w("");
w("Mean dimension scores across runs (0–10 scale, **higher is better**).");
if (runs > 1) w("± values show standard deviation across runs.");
w("");

const dimHeader = ["Dimension [Tier]", ...stimulusNames];
w(`| ${dimHeader.join(" | ")} |`);
w(`|${dimHeader.map(() => "---").join("|")}|`);

const scoredGraders = Object.keys(TIER_MAP).filter((k) => TIER_MAP[k].weight > 0);
for (const graderName of scoredGraders) {
  const display = DISPLAY_NAMES[graderName] || graderName;
  const tier = TIER_MAP[graderName].tier;
  const cells = [
    `${display} [${tier}]`,
    ...stimulusNames.map((sn) => {
      const data = stimulusData.get(sn);
      const scores = data.graderScores[graderName];
      if (!scores || scores.length === 0) return "—";
      const vals = scores.map((s) => toScale10(s.score));
      const m = mean(vals);
      const sd = stddev(vals);
      return runs > 1 && sd > 0 ? `${fmt(m)} ± ${fmt(sd)}` : fmt(m);
    }),
  ];
  w(`| ${cells.join(" | ")} |`);
}
w("");
w("---");
w("");

// Final rankings
w("## Final Rankings");
w("");
w("Configurations ranked by weighted score — **higher is better**.");
w("");

const rankings = stimulusNames
  .map((sn) => {
    const data = stimulusData.get(sn);
    const ws = computeWeightedScore(data.graderScores);
    // Per-run weighted scores for std dev
    const entries = byStimulus.get(sn);
    const perRunScores = entries.map((entry) => {
      const gr = entry.gradeResult;
      if (!gr?.details) return 0;
      let wsum = 0;
      for (const d of gr.details) {
        const tier = TIER_MAP[d.name];
        if (!tier || tier.weight === 0) continue;
        wsum += toScale10(d.score) * tier.weight;
      }
      return wsum;
    });
    return {
      name: sn,
      meanScore: ws.weightedSum,
      maxPossible: ws.maxPossible,
      pctMax: (ws.weightedSum / ws.maxPossible) * 100,
      stdDev: stddev(perRunScores),
      min: Math.min(...perRunScores),
      max: Math.max(...perRunScores),
    };
  })
  .sort((a, b) => b.meanScore - a.meanScore);

const medals = ["🥇", "🥈", "🥉"];
w(`| Rank | Configuration | Mean Score ↑ | % of Max (${fmt(maxWeighted.maxPossible, 0)}) | ${runs > 1 ? "Std Dev ↓ | " : ""}Min | Max |`);
w(`|---|---|---|---|${runs > 1 ? "---|" : ""}---|---|`);
for (let i = 0; i < rankings.length; i++) {
  const r = rankings[i];
  const medal = medals[i] || `${i + 1}`;
  const sdCol = runs > 1 ? `${fmt(r.stdDev)} | ` : "";
  w(`| ${medal} | ${r.name} | ${fmt(r.meanScore)} | ${fmt(r.pctMax)}% | ${sdCol}${fmt(r.min)} | ${fmt(r.max)} |`);
}
w("");
w("---");
w("");

// Token usage summary
w("## Token Usage Summary");
w("");
w("Average token consumption per configuration across all runs.");
w("");

const baselineMetrics = stimulusData.get(stimulusNames[0])?.metrics ?? [];
const baselineAvgInput = baselineMetrics.length > 0 ? mean(baselineMetrics.map((m) => m.tokenUsage.inputTokens)) : 0;

w("| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time | Δ Input vs Baseline |");
w("|---|---|---|---|---|---|---|");

for (const sn of stimulusNames) {
  const data = stimulusData.get(sn);
  const metrics = data.metrics;
  if (metrics.length === 0) continue;
  const avgInput = mean(metrics.map((m) => m.tokenUsage.inputTokens));
  const avgOutput = mean(metrics.map((m) => m.tokenUsage.outputTokens));
  const avgCache = mean(metrics.map((m) => m.tokenUsage.cacheReadTokens));
  const avgCalls = mean(metrics.map((m) => m.tokenUsage.callCount));
  const avgWall = mean(metrics.map((m) => m.wallTimeMs));
  const delta = sn === stimulusNames[0] ? "— (baseline)" : pctChange(baselineAvgInput, avgInput);
  w(`| ${sn} | ${fmtNumber(Math.round(avgInput))} | ${fmtNumber(Math.round(avgOutput))} | ${fmtNumber(Math.round(avgCache))} | ${fmt(avgCalls, 0)} | ${fmtDuration(avgWall)} | ${delta} |`);
}
w("");

// Token usage per run (if multiple runs)
if (runs > 1) {
  w("### Token Usage Per Run");
  w("");
  w("| Configuration | Run | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time |");
  w("|---|---|---|---|---|---|---|");
  for (const sn of stimulusNames) {
    const entries = byStimulus.get(sn);
    entries.forEach((entry, i) => {
      const m = entry.trajectory.metrics;
      const tu = m.tokenUsage;
      w(`| ${sn} | ${i + 1} | ${fmtNumber(tu.inputTokens)} | ${fmtNumber(tu.outputTokens)} | ${fmtNumber(tu.cacheReadTokens)} | ${tu.callCount} | ${fmtDuration(m.wallTimeMs)} |`);
    });
  }
  w("");
}
w("");

// Tool usage breakdown
w("## Tool Usage Breakdown");
w("");
w("| Configuration | Tool Calls | Turns | Skills Used | Tool Breakdown |");
w("|---|---|---|---|---|");
for (const sn of stimulusNames) {
  const entries = byStimulus.get(sn);
  const m = entries[0].trajectory.metrics;
  const breakdown = Object.entries(m.toolCallBreakdown)
    .sort((a, b) => b[1] - a[1])
    .map(([tool, count]) => `${tool}(${count})`)
    .join(", ");
  const skillsUsed = entries[0].trajectory.metadata?.skillsLoaded?.join(", ") || "—";
  w(`| ${sn} | ${m.toolCallCount} | ${m.turnCount} | ${skillsUsed} | ${breakdown} |`);
}
w("");
w("---");
w("");

// Per-dimension analysis
w("## Per-Dimension Analysis");
w("");

for (let di = 0; di < scoredGraders.length; di++) {
  const graderName = scoredGraders[di];
  const display = DISPLAY_NAMES[graderName] || graderName;
  const tier = TIER_MAP[graderName];

  w(`### ${di + 1}. ${display} [${tier.tier} × ${tier.weight}]`);
  w("");

  // Scores across runs
  if (runs > 1) {
    w("#### Scores Across Runs");
    w("");
    w(`| Run | ${stimulusNames.join(" | ")} |`);
    w(`|---|${stimulusNames.map(() => "---").join("|")}|`);
    for (let r = 0; r < runs; r++) {
      const cells = stimulusNames.map((sn) => {
        const data = stimulusData.get(sn);
        const scores = data.graderScores[graderName];
        return scores?.[r] ? fmt(toScale10(scores[r].score)) : "—";
      });
      w(`| ${r + 1} | ${cells.join(" | ")} |`);
    }
    const meanCells = stimulusNames.map((sn) => {
      const data = stimulusData.get(sn);
      const scores = data.graderScores[graderName];
      if (!scores) return "—";
      return `**${fmt(mean(scores.map((s) => toScale10(s.score))))}**`;
    });
    w(`| **Mean** | ${meanCells.join(" | ")} |`);
    w("");
  }

  // Score comparison (single or multi-run)
  w("#### Score Comparison");
  w("");
  const scoreEntries = stimulusNames.map((sn) => {
    const data = stimulusData.get(sn);
    const scores = data.graderScores[graderName] ?? [];
    const vals = scores.map((s) => toScale10(s.score));
    return { name: sn, mean: vals.length > 0 ? mean(vals) : 0, passed: scores.every((s) => s.passed) };
  });
  const best = scoreEntries.reduce((a, b) => (a.mean >= b.mean ? a : b));
  const allEqual = scoreEntries.every((s) => Math.abs(s.mean - best.mean) < 0.01);

  for (const se of scoreEntries) {
    const icon = se.passed ? "✅" : "❌";
    w(`- ${icon} **${se.name}**: ${fmt(se.mean)}/10`);
  }
  w("");
  if (allEqual) {
    w(`**Verdict:** Tie — all configurations scored equally.`);
  } else {
    const delta = best.mean - mean(scoreEntries.filter((s) => s !== best).map((s) => s.mean));
    w(`**Verdict:** **${best.name}** leads (+${fmt(delta)} vs others).`);
  }
  w("");

  // Evidence from graders
  w("#### Judge Evidence");
  w("");
  for (const sn of stimulusNames) {
    const data = stimulusData.get(sn);
    const scores = data.graderScores[graderName] ?? [];
    if (scores.length > 0 && scores[0].evidence) {
      const evidence = scores[0].evidence
        .split("\n")
        .map((l) => `> ${l}`)
        .join("\n");
      w(`**${sn}:**`);
      w(evidence);
      w("");
    }
  }
  w("---");
  w("");
}

// Consistency analysis (multi-run only)
if (runs > 1) {
  w("## Consistency Analysis");
  w("");
  w("Score σ (standard deviation) measures how much a configuration's weighted score varies across runs — **lower is better**.");
  w("");
  w("| Configuration | Score σ | Most Consistent Dim (σ) | Most Variable Dim (σ) |");
  w("|---|---|---|---|");
  for (const r of rankings) {
    const data = stimulusData.get(r.name);
    let minSd = Infinity, maxSd = -Infinity, minDim = "", maxDim = "";
    for (const gn of scoredGraders) {
      const scores = data.graderScores[gn];
      if (!scores || scores.length < 2) continue;
      const sd = stddev(scores.map((s) => toScale10(s.score)));
      if (sd < minSd) { minSd = sd; minDim = DISPLAY_NAMES[gn]; }
      if (sd > maxSd) { maxSd = sd; maxDim = DISPLAY_NAMES[gn]; }
    }
    w(`| ${r.name} | ${fmt(r.stdDev)} | ${minDim} (${fmt(minSd)}) | ${maxDim} (${fmt(maxSd)}) |`);
  }
  w("");
  w("---");
  w("");
}

// Summary
w("## Summary");
w("");
if (overallScore !== undefined) {
  w(`**Overall eval score: ${fmt(overallScore * 100, 1)}%** (threshold: ${fmt(evalThreshold * 100, 0)}%) — ${runSummary.passed ? "✅ PASSED" : "❌ FAILED"}`);
  w("");
}
for (const r of rankings) {
  w(`- **${r.name}**: weighted score ${fmt(r.meanScore)}/${fmt(r.maxPossible, 0)} (${fmt(r.pctMax)}%)`);
}
w("");
w("---");
w(`*Generated from vally eval results at ${resultsDir}*`);

// ---------------------------------------------------------------------------
// Write output
// ---------------------------------------------------------------------------
writeFileSync(outputPath, out.join("\n") + "\n");
console.log(`Report written to ${outputPath}`);
