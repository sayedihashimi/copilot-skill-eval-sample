---
mode: agent
description: "Analyze a .NET class library for performance anti-patterns and optimization opportunities"
tools: ["changes", "codebase", "fetch", "problems", "runner", "terminalLastCommand"]
---

# Analyze .NET Project for Performance Issues

## Task

You are a .NET performance expert. There is an **existing** .NET class library project already on disk at `./perf01/`. **Do NOT create any source files.** The project and all its source code already exist. Your job is to read the existing code and analyze it for performance issues.

## Instructions

1. **Read the existing source files** from the `./perf01/` directory. Start by listing the files with a glob or directory listing, then read each `.cs` file.

2. **Scan for performance anti-patterns** across these categories:
   - **Regex** — per-call instantiation, excessive `RegexOptions.Compiled`, missing `[GeneratedRegex]`
   - **String** — `+=` in loops, `.Replace()` chains, `.ToLower()`/`.ToUpper()` without culture
   - **Memory/Allocation** — boxing structs, `params` arrays, closure captures, unnecessary `.ToList()`
   - **Collections** — `List.Contains` (O(n)), `ContainsKey` + indexer, missing capacity hints, `FrozenDictionary` candidates
   - **Async/IO** — `new HttpClient` per call, sequential awaits in loops, missing cancellation tokens, unbounded parallelism
   - **Reflection** — uncached `GetProperties()`/`SetValue()` in hot paths
   - **Structural** — unsealed leaf classes, structs without `IEquatable<T>`
   - **Serialization** — `new JsonSerializerOptions` per call, repeated deserialization
   - **Span** — inconsistent `AsSpan` usage across similar classes, `value[..n].TrimEnd()` double allocation, `List<char>[]` vs `ReadOnlySpan<char>`
   - **Aggregate** — `.Aggregate()` with `.Replace()` creating intermediate string allocations, `char.ToString()` in loops
   - **Inheritance** — leaf classes that should be `sealed` vs. base classes that must remain unsealed
   - **Params** — `params` methods without single-argument fast-path overloads

3. **Classify each finding** by severity:
   - 🔴 **Critical** — >10x performance regression potential or production incident risk (e.g., socket exhaustion, O(n²) in hot paths)
   - 🟡 **Moderate** — 2-10x regression, measurable impact at scale
   - ℹ️ **Info** — Best practice improvement, minor allocation savings

4. **Provide specific fix recommendations** for each finding. Reference concrete .NET APIs and patterns. Include brief code snippets showing the before/after where helpful.

## Output

Write your complete analysis to a file named `performance-analysis.md` in the output directory specified in the instructions above. Use the following structure:

```markdown
# Performance Analysis Report

## Executive Summary
(Brief overview: total issues found, breakdown by severity, top priorities)

## Findings by File

### [filename]
(For each file, list findings with severity, description, and fix recommendation)

## Cross-Cutting Concerns
(Patterns that appear across multiple files)

## Prioritized Fix Recommendations
(Top 10 fixes ranked by impact, with effort estimates: quick-fix / moderate / significant)
```
