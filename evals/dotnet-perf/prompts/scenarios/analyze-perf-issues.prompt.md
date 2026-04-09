---
mode: agent
description: "Analyze a .NET class library for performance anti-patterns and optimization opportunities"
tools: ["changes", "codebase", "fetch", "problems", "runner", "terminalLastCommand"]
---

# Analyze .NET Project for Performance Issues

## Task

You are a .NET performance expert. Analyze the .NET class library project located at `./perf01/` for performance anti-patterns, inefficiencies, and optimization opportunities.

## Project Overview

The project is a .NET 8 class library (`Perf01`) containing utility classes for text processing, data parsing, services, and model mapping. The code is functional but may contain performance issues that would impact production workloads.

## What To Do

1. **Read all source files** in the `./perf01/` project directory. The project has this structure:
   - `TextProcessing/` — SlugGenerator.cs, MarkdownStripper.cs, TemplateEngine.cs
   - `Data/` — CsvParser.cs, JsonTransformer.cs
   - `Services/` — LogAnalyzer.cs, NotificationService.cs, DataPipeline.cs
   - `Models/` — ValidationEngine.cs, EntityMapper.cs

2. **Scan for performance anti-patterns** across these categories:
   - **Regex** — per-call instantiation, excessive `RegexOptions.Compiled`, missing `[GeneratedRegex]`
   - **String** — `+=` in loops, `.Replace()` chains, `.ToLower()`/`.ToUpper()` without culture
   - **Memory/Allocation** — boxing structs, `params` arrays, closure captures, unnecessary `.ToList()`
   - **Collections** — `List.Contains` (O(n)), `ContainsKey` + indexer, missing capacity hints, `FrozenDictionary` candidates
   - **Async/IO** — `new HttpClient` per call, sequential awaits in loops, missing cancellation tokens, unbounded parallelism
   - **Reflection** — uncached `GetProperties()`/`SetValue()` in hot paths
   - **Structural** — unsealed leaf classes, structs without `IEquatable<T>`
   - **Serialization** — `new JsonSerializerOptions` per call, repeated deserialization

3. **Classify each finding** by severity:
   - 🔴 **Critical** — >10x performance regression potential or production incident risk (e.g., socket exhaustion, O(n²) in hot paths)
   - 🟡 **Moderate** — 2-10x regression, measurable impact at scale
   - ℹ️ **Info** — Best practice improvement, minor allocation savings

4. **Provide specific fix recommendations** for each finding. Reference concrete .NET APIs and patterns. Include brief code snippets showing the before/after where helpful.

## Output

Write your complete analysis to `./src/performance-analysis.md` with the following structure:

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
