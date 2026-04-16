# Perf01 — .NET Performance Anti-Pattern Sample

A sample .NET 8 class library containing realistic code with a broad range of
performance issues. Designed for evaluating whether performance analysis tooling
detects (or misses) various anti-patterns.

## Structure

| File | Key Performance Issues |
|------|----------------------|
| `TextProcessing/SlugGenerator.cs` | Per-call `Regex.Replace`, `.ToLower()` without culture, `.Replace()` chains, `List.Contains` in loop |
| `TextProcessing/MarkdownStripper.cs` | 40+ `RegexOptions.Compiled` instances (startup budget), long `.Replace()` chain |
| `TextProcessing/TemplateEngine.cs` | `new Regex` per call, `string +=` in loops, `new Dictionary` per iteration, reflection in hot path |
| `Data/CsvParser.cs` | Char-by-char `string +=`, `.ToLower()` without ordinal, `new Regex` per call, string-based CSV formatting |
| `Data/JsonTransformer.cs` | `new JsonSerializerOptions` per call, `List.Contains` for key lookup, repeated deserialization, boxing |
| `Services/LogAnalyzer.cs` | `new Regex` per log line (×millions), multiple passes over same data, `Skip().Take().ToList()` in loop, `double.Parse` in LINQ |
| `Services/NotificationService.cs` | `new HttpClient` per call (socket exhaustion), struct without `IEquatable<T>`, `params` array allocation, unbounded parallelism, no cancellation tokens |
| `Services/DataPipeline.cs` | Unsealed classes (devirtualization), exception-driven flow, `.Distinct().ToList()`, `ContainsKey` + indexer |
| `Models/ValidationEngine.cs` | `new Regex` inside validation closure (called per record), struct without `IEquatable<T>`, `string +=` |
| `Models/EntityMapper.cs` | `GetProperties()` reflection per call (not cached), `.ToLower()` for case-insensitive compare, `FrozenDictionary` candidate |
| `TextProcessing/TextTruncation.cs` | `value[..n].TrimEnd()` double allocation, inconsistent `AsSpan` usage, `List<char>[]` vs `ReadOnlySpan<char>`, `params` without 1-arg fast path |
| `TextProcessing/UnitFormatter.cs` | `.Aggregate()` + `.Replace()` chain (16 intermediate strings), `char.ToString()`, struct without `IEquatable<T>`, unsealed leaf classes in ordinalizer hierarchy |

## Categories of Issues

- **Regex** — per-call instantiation, excessive `Compiled`, missing `[GeneratedRegex]`
- **String** — `+=` in loops, `.Replace()` chains, `.ToLower()`/`.ToUpper()` without culture
- **Memory/Allocation** — boxing structs, `params` arrays, closure captures, unnecessary `.ToList()`
- **Collections** — `List.Contains` (O(n)), `ContainsKey` + indexer, missing capacity hints, `FrozenDictionary` candidates
- **Async/IO** — `new HttpClient` per call, sequential awaits in loops, no cancellation tokens, unbounded parallelism
- **Reflection** — uncached `GetProperties()`/`SetValue()` in hot paths
- **Structural** — unsealed leaf classes, structs without `IEquatable<T>`
- **Algorithmic** — multiple passes over same data, `Skip().Take()` sliding window, exception-driven control flow
- **Serialization** — `new JsonSerializerOptions` per call, repeated deserialization
- **Span** — inconsistent `AsSpan` usage, `value[..n].TrimEnd()` double allocation, `List<char>[]` vs `ReadOnlySpan<char>`
- **Aggregate** — `.Aggregate()` + `.Replace()` chain creating intermediate strings, `char.ToString()` in loops
- **Inheritance** — unsealed leaf classes vs. base classes that must remain unsealed (sealing accuracy)
- **Params** — `params` methods without single-argument fast-path overloads

## Build

```bash
dotnet build
```
