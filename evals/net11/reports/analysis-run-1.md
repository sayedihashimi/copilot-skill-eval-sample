# Comparative Analysis: dotnet-net11-skill, no-skills

I analyzed **2 configuration outputs** under `output/{config}/run-1`: `dotnet-net11-skill` and `no-skills`. In this run, both configurations generated only `console-bcl` code; `webapi`, `blazor`, and `efcore` app directories are absent in both trees, which heavily affects all ASP.NET Core/Blazor/EF Core dimensions.

## Executive Summary

| Dimension [Tier] | dotnet-net11-skill | no-skills |
|---|---:|---:|
| Zstandard Compression Usage [CRITICAL] | 5 | 5 |
| BFloat16 Type Usage [HIGH] | 5 | 3 |
| Rune-Based String Operations [HIGH] | 5 | 3 |
| HMAC Single-Step Verification [HIGH] | 5 | 4 |
| FrozenDictionary Collection Expressions [HIGH] | 2 | 2 |
| Collection Expression with() Arguments [HIGH] | 1 | 1 |
| Union Type Usage [CRITICAL] | 2 | 1 |
| MediaTypeMap Usage [MEDIUM] | 5 | 2 |
| DivisionRounding Modes [MEDIUM] | 5 | 2 |
| System.Text.Json New Features [CRITICAL] | 5 | 3 |
| RegexOptions.AnyNewLine [MEDIUM] | 5 | 2 |
| File System New APIs [HIGH] | 5 | 3 |
| Base64 Parity APIs [MEDIUM] | 5 | 5 |
| Generic Interlocked Operations [MEDIUM] | 5 | 5 |
| BitArray.PopCount [LOW] | 5 | 5 |
| Native OpenTelemetry Tracing [HIGH] | 1 | 1 |
| OpenAPI Version [MEDIUM] | 1 | 1 |
| Dynamic Output Cache Policy Provider [HIGH] | 1 | 1 |
| Zstandard Response Compression [HIGH] | 1 | 1 |
| Blazor EnvironmentBoundary Component [HIGH] | 1 | 1 |
| Blazor Label and DisplayName Components [HIGH] | 1 | 1 |
| QuickGrid OnRowClick [HIGH] | 1 | 1 |
| RelativeToCurrentUri Navigation [MEDIUM] | 1 | 1 |
| Blazor TempData Support [HIGH] | 1 | 1 |
| Blazor BasePath Component [MEDIUM] | 1 | 1 |
| EF Core GetEntriesForState [HIGH] | 1 | 1 |
| EF Core RemoveDbContext [HIGH] | 1 | 1 |
| EF Core ExcludeForeignKeyFromMigrations [MEDIUM] | 1 | 1 |
| EF Core JSON Query Functions [HIGH] | 1 | 1 |
| SignalR ConfigureConnection [MEDIUM] | 1 | 1 |
| Blazor Virtualize Variable-Height Items [MEDIUM] | 1 | 1 |
| Runtime Async Configuration [MEDIUM] | 1 | 1 |
| ProcessExitStatus Usage [MEDIUM] | 2 | 2 |
| OpenAPI Binary File Response [MEDIUM] | 1 | 1 |
| Brotli and Compression Options [LOW] | 3 | 3 |
| Vector Constants [LOW] | 1 | 1 |
| Overall .NET 11 API Adoption Rate [CRITICAL] | 3 | 2 |

## 1. Zstandard Compression Usage [CRITICAL]
Both use native `System.IO.Compression.ZstandardStream` and `ZstandardCompressionOptions`.

```csharp
// dotnet-net11-skill: Demos/CompressionBenchmark.cs
var advancedOptions = new ZstandardCompressionOptions { Quality = 5, WindowLog = 22, AppendChecksum = true, EnableLongDistanceMatching = true };
using (var zstdStream = new ZstandardStream(advancedOutput, advancedOptions)) { zstdStream.Write(originalData); }
```

```csharp
// no-skills: Demos/CompressionBenchmark.cs
var zstdOptions = new ZstandardCompressionOptions { Quality = 6, AppendChecksum = true, EnableLongDistanceMatching = true, WindowLog = 22 };
data => Compress(data, stream => new ZstandardStream(stream, zstdOptions))
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 5` — both are fully native and correct.  
**Verdict:** Tie; both align with .NET 11 best practice.

## 2. BFloat16 Type Usage [HIGH]
`dotnet-net11-skill` uses native arithmetic and `BinaryPrimitives` APIs directly; `no-skills` falls back to float-cast arithmetic and `MemoryMarshal`.

```csharp
// dotnet-net11-skill: Demos/BFloat16Demo.cs
BFloat16 sum = a + b;
BinaryPrimitives.WriteBFloat16LittleEndian(buffer, pi);
BFloat16 readBack = BinaryPrimitives.ReadBFloat16LittleEndian(buffer);
```

```csharp
// no-skills: Demos/MlNumericTypes.cs
var sum = (BFloat16)((float)a + (float)b);
MemoryMarshal.Write(leBytes, in a);
var restored = MemoryMarshal.Read<BFloat16>(leBytes);
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 3`.  
**Verdict:** `dotnet-net11-skill` is better; it uses the new primitive more idiomatically.

## 3. Rune-Based String Operations [HIGH]
`dotnet-net11-skill` uses new `string`/`StringBuilder` Rune overloads heavily; `no-skills` uses manual rune loops.

```csharp
// dotnet-net11-skill: Demos/UnicodeTextProcessor.cs
Console.WriteLine($"Contains 🌍: {text.Contains(globe)}");
string replaced = text.Replace(globe, new Rune('X'));
Rune runeAt6 = sb.GetRuneAt(6);
```

```csharp
// no-skills: Demos/UnicodeTextProcessor.cs
foreach (var rune in text.EnumerateRunes()) { ... }
var replaced = ReplaceRune(text, earth, star);
private static List<string> SplitByRune(string text, Rune separator) { ... }
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 3`.  
**Verdict:** `dotnet-net11-skill` best demonstrates .NET 11 Rune API surface.

## 4. HMAC Single-Step Verification [HIGH]
Both use `Verify`, but `dotnet-net11-skill` also uses `VerifyAsync` and `CryptographicOperations.VerifyHmac`; `no-skills` still includes two-step `FixedTimeEquals`.

```csharp
// dotnet-net11-skill: Demos/CryptoHashVerifier.cs
bool valid256 = HMACSHA256.Verify(key, data, hmac256);
bool streamValid = HMACSHA256.VerifyAsync(key, stream, streamHmac).GetAwaiter().GetResult();
bool agnosticValid = CryptographicOperations.VerifyHmac(HashAlgorithmName.SHA256, key, data, agnosticHmac);
```

```csharp
// no-skills: Demos/CryptoHashVerifier.cs
var verify256 = HMACSHA256.HashData(key, data);
Console.WriteLine($"Constant-time verify: {CryptographicOperations.FixedTimeEquals(hash256, verify256)}");
bool verified256 = HMACSHA256.Verify(key, data, hash256);
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 4`.  
**Verdict:** `dotnet-net11-skill` is cleaner and safer by preferring single-step APIs end-to-end.

## 5. FrozenDictionary Collection Expressions [HIGH]
Neither uses C# 15 dictionary collection-expression initialization for `FrozenDictionary`.

```csharp
// dotnet-net11-skill: Demos/ImmutableLookupDemo.cs
FrozenDictionary<string, int> httpCodes = new Dictionary<string, int> { ... }.ToFrozenDictionary(StringComparer.Ordinal);
```

```csharp
// no-skills: Demos/ImmutableLookupTables.cs
var httpStatusCodes = new Dictionary<int, string> { ... }.ToFrozenDictionary();
```

**Score:** `dotnet-net11-skill: 2`, `no-skills: 2`.  
**Verdict:** Tie; both use older multi-step construction.

## 6. Collection Expression with() Arguments [HIGH]
Neither uses `with(...)` collection-expression constructor arguments.

```csharp
// dotnet-net11-skill: Demos/CollectionExpressionDemo.cs
HashSet<string> caseInsensitive = new(StringComparer.OrdinalIgnoreCase) { "Hello", "World", "HELLO" };
```

```csharp
// no-skills: Demos/CollectionInitialization.cs
Dictionary<string, int> scores = new() { ["Alice"] = 95, ["Bob"] = 87, ["Charlie"] = 92 };
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; both miss the new C# 15 `with()` syntax.

## 7. Union Type Usage [CRITICAL]
Neither uses `union`; both use record hierarchies. `dotnet-net11-skill` keeps an exhaustive switch without default; `no-skills` adds fallback throw arms.

```csharp
// dotnet-net11-skill: Demos/UnionTypesDemo.cs
public abstract record Shape { public sealed record Circle(double Radius) : Shape; ... }
static double ComputeArea(Shape shape) => shape switch { Shape.Circle c => ..., Shape.Rectangle r => ..., Shape.Triangle t => ... };
```

```csharp
// no-skills: Demos/DiscriminatedUnions.cs
public abstract record Shape { ... }
Shape.Triangle t => 0.5 * t.Base * t.Height,
_ => throw new ArgumentOutOfRangeException(nameof(shape))
```

**Score:** `dotnet-net11-skill: 2`, `no-skills: 1`.  
**Verdict:** `dotnet-net11-skill` is slightly better, but both miss the requested C# 15 union model.

## 8. MediaTypeMap Usage [MEDIUM]
`dotnet-net11-skill` correctly uses `MediaTypeMap`; `no-skills` uses manual dictionaries plus `MediaTypeNames`.

```csharp
// dotnet-net11-skill: Demos/MimeTypeResolver.cs
string? mediaType = MediaTypeMap.GetMediaType(ext);
string? ext = MediaTypeMap.GetExtension(mime);
```

```csharp
// no-skills: Demos/MimeTypeResolver.cs
private static readonly Dictionary<string, string> ExtensionToMime = new(StringComparer.OrdinalIgnoreCase) { [".json"] = MediaTypeNames.Application.Json, ... };
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 2`.  
**Verdict:** `dotnet-net11-skill` is decisively better.

## 9. DivisionRounding Modes [MEDIUM]
`dotnet-net11-skill` uses `DivisionRounding`; `no-skills` uses manual formulas.

```csharp
// dotnet-net11-skill: Demos/IntegerMathToolkit.cs
foreach (DivisionRounding mode in Enum.GetValues<DivisionRounding>()) { ... }
static T Divide<T>(T left, T right, DivisionRounding mode) where T : IBinaryInteger<T> => T.Divide(left, right, mode);
```

```csharp
// no-skills: Demos/IntegerMathToolkit.cs
var ceilResult = (int)Math.Ceiling((double)dividend / divisor);
var euclideanRemainder = ((dividend % divisor) + divisor) % divisor;
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 2`.  
**Verdict:** `dotnet-net11-skill` follows the new safe API.

## 10. System.Text.Json New Features [CRITICAL]
`dotnet-net11-skill` adopts almost the full new feature set; `no-skills` only partial.

```csharp
// dotnet-net11-skill: Demos/JsonSerializationShowcase.cs
[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
[JsonNamingPolicy(JsonKnownNamingPolicy.CamelCase)]
IReadOnlySet<string> tags = new HashSet<string> { "dotnet", "csharp", "net11" };
var typeInfo = metaOptions.GetTypeInfo<EventData>();
```

```csharp
// no-skills: Demos/JsonSerializationShowcase.cs
PropertyNamingPolicy = JsonNamingPolicy.PascalCase,
DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
var typeInfo = options.GetTypeInfo(typeof(UserProfile));
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 3`.  
**Verdict:** `dotnet-net11-skill` is clearly superior and more modern.

## 11. RegexOptions.AnyNewLine [MEDIUM]
`dotnet-net11-skill` uses `RegexOptions.AnyNewLine`; `no-skills` manually splits newline classes.

```csharp
// dotnet-net11-skill: Demos/UniversalNewlineRegex.cs
Regex.Matches(text, @"^line\d$", RegexOptions.Multiline | RegexOptions.AnyNewLine);
```

```csharp
// no-skills: Demos/UniversalNewlineRegex.cs
var universalNewlinePattern = new Regex(@"\r\n|\r|\n|\u0085|\u2028|\u2029");
var lines = universalNewlinePattern.Split(text);
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 2`.  
**Verdict:** `dotnet-net11-skill` uses the intended .NET 11 API directly.

## 12. File System New APIs [HIGH]
`dotnet-net11-skill` uses all requested APIs; `no-skills` only partially.

```csharp
// dotnet-net11-skill: Demos/FileSystemUtilities.cs
FileSystemInfo link = File.CreateHardLink(linkPath, originalPath);
using SafeFileHandle nullHandle = File.OpenNullHandle();
SafeFileHandle.CreateAnonymousPipe(out SafeFileHandle readEnd, out SafeFileHandle writeEnd, asyncRead: true, asyncWrite: false);
```

```csharp
// no-skills: Demos/FileSystemUtilities.cs
File.CreateHardLink(hardLinkPath, originalPath);
using var nullHandle = File.OpenHandle(OperatingSystem.IsWindows() ? "NUL" : "/dev/null", FileMode.Open, FileAccess.Write);
using var server = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.None);
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 3`.  
**Verdict:** `dotnet-net11-skill` better replaces legacy patterns with new BCL APIs.

## 13. Base64 Parity APIs [MEDIUM]
Both implement modern Base64 APIs correctly.

```csharp
// dotnet-net11-skill: Demos/Base64Utilities.cs
string encoded = Base64.EncodeToString(data);
byte[] decoded = Base64.DecodeFromChars(encoded);
char[] encodedChars = Base64.EncodeToChars(data);
```

```csharp
// no-skills: Demos/Base64Utilities.cs
var base64String = Base64.EncodeToString(originalBytes);
var decoded = Base64.DecodeFromChars(base64String);
var charsWritten = Base64.EncodeToChars(originalBytes, charBuffer);
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 5`.  
**Verdict:** Tie; both are excellent.

## 14. Generic Interlocked Operations [MEDIUM]
Both use generic enum overloads directly.

```csharp
// dotnet-net11-skill: Demos/ConcurrentFlagDemo.cs
FilePermissions oldValue = Interlocked.Or(ref permissions, FilePermissions.Write);
oldValue = Interlocked.And(ref permissions, ~FilePermissions.Write);
```

```csharp
// no-skills: Demos/ConcurrentFlagOperations.cs
Interlocked.Or(ref permissions, Permissions.Write);
Interlocked.And(ref permissions, ~Permissions.Read);
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 5`.  
**Verdict:** Tie; both follow best practice.

## 15. BitArray.PopCount [LOW]
Both correctly use `PopCount()`.

```csharp
// dotnet-net11-skill: Demos/BitCountingDemo.cs
int setBitCount = bits.PopCount();
Console.WriteLine($"  PopCount: {largeBits.PopCount()}");
```

```csharp
// no-skills: Demos/BitCounting.cs
var popCount = bits.PopCount();
Console.WriteLine($"  Large ... PopCount: {large.PopCount()}");
```

**Score:** `dotnet-net11-skill: 5`, `no-skills: 5`.  
**Verdict:** Tie; both adopt the new API.

## 16. Native OpenTelemetry Tracing [HIGH]
No `webapi` app exists in either run; there is no `AddOpenTelemetry()` usage to evaluate.

```text
# dotnet-net11-skill: output/dotnet-net11-skill/run-1
console-bcl
copilot-chat.md
events.jsonl
```

```text
# no-skills: output/no-skills/run-1
console-bcl
copilot-chat.md
events.jsonl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

## 17. OpenAPI Version [MEDIUM]
No Web API project means no `AddOpenApi(...OpenApi3_2)` usage.

```text
# dotnet-net11-skill run-1
console-bcl
copilot-chat.md
events.jsonl
```

```text
# no-skills run-1
console-bcl
copilot-chat.md
events.jsonl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

## 18. Dynamic Output Cache Policy Provider [HIGH]
No Web API project; no `IOutputCachePolicyProvider` implementation.

```text
# dotnet-net11-skill run-1: no webapi directory
console-bcl
```

```text
# no-skills run-1: no webapi directory
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

## 19. Zstandard Response Compression [HIGH]
No ASP.NET Core pipeline; no `AddResponseCompression()`/`ZstandardCompressionProviderOptions`.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

## 20. Blazor EnvironmentBoundary Component [HIGH]
No Blazor project; no `<EnvironmentBoundary ...>` usage.

```text
# dotnet-net11-skill run-1: no blazor directory
console-bcl
```

```text
# no-skills run-1: no blazor directory
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

## 21. Blazor Label and DisplayName Components [HIGH]
No Blazor forms/tables; no `<Label>`/`<DisplayName>`.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

## 22. QuickGrid OnRowClick [HIGH]
No Blazor QuickGrid; no `OnRowClick`.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

## 23. RelativeToCurrentUri Navigation [MEDIUM]
No Blazor navigation code.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

## 24. Blazor TempData Support [HIGH]
No Blazor SSR code; no `ITempData`.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

## 25. Blazor BasePath Component [MEDIUM]
No Blazor layout/head rendering code; no `<BasePath />`.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

## 26. EF Core GetEntriesForState [HIGH]
No EF Core app exists; no `ChangeTracker.GetEntriesForState(...)`.

```text
# dotnet-net11-skill run-1: no efcore directory
console-bcl
```

```text
# no-skills run-1: no efcore directory
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

## 27. EF Core RemoveDbContext [HIGH]
No EF registration/testing setup; no `RemoveDbContext<T>()`.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

## 28. EF Core ExcludeForeignKeyFromMigrations [MEDIUM]
No model configuration with `ExcludeForeignKeyFromMigrations(true)`.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

## 29. EF Core JSON Query Functions [HIGH]
No EF query layer; no `EF.Functions.JsonContains`/`JsonPathExists`.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

## 30. SignalR ConfigureConnection [MEDIUM]
No interactive server components; no `ConfigureConnection`.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

## 31. Blazor Virtualize Variable-Height Items [MEDIUM]
No `<Virtualize>` usage to evaluate.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

## 32. Runtime Async Configuration [MEDIUM]
Neither project has `<Features>runtime-async=on</Features>`.

```xml
<!-- dotnet-net11-skill: DevToolkit.csproj -->
<TargetFramework>net11.0</TargetFramework>
<LangVersion>preview</LangVersion>
```

```xml
<!-- no-skills: bcl-showcase.csproj -->
<TargetFramework>net11.0</TargetFramework>
<LangVersion>preview</LangVersion>
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; feature not configured.

## 33. ProcessExitStatus Usage [MEDIUM]
Both use legacy `ExitCode`/`HasExited` instead of `ProcessExitStatus`.

```csharp
// dotnet-net11-skill: Demos/ProcessExitDemo.cs
successProcess.WaitForExit();
Console.WriteLine($"  Exit code: {successProcess.ExitCode}");
```

```csharp
// no-skills: Demos/ProcessExitInfo.cs
await process.WaitForExitAsync();
Console.WriteLine($"  Exit code: {process.ExitCode}");
```

**Score:** `dotnet-net11-skill: 2`, `no-skills: 2`.  
**Verdict:** Tie; both still use old process-exit model.

## 34. OpenAPI Binary File Response [MEDIUM]
No Web API endpoints are present, so no `.Produces<FileContentResult>(...)` coverage.

```text
# dotnet-net11-skill run-1
console-bcl
```

```text
# no-skills run-1
console-bcl
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; missing scenario.

## 35. Brotli and Compression Options [LOW]
Both configure Zstandard options well, but neither uses `BrotliCompressionOptions.WindowLog`.

```csharp
// dotnet-net11-skill: Demos/CompressionBenchmark.cs
var advancedOptions = new ZstandardCompressionOptions { Quality = 5, WindowLog = 22, AppendChecksum = true, EnableLongDistanceMatching = true };
```

```csharp
// no-skills: Demos/CompressionBenchmark.cs
var zstdOptions = new ZstandardCompressionOptions { Quality = 6, AppendChecksum = true, EnableLongDistanceMatching = true, WindowLog = 22 };
```

**Score:** `dotnet-net11-skill: 3`, `no-skills: 3`.  
**Verdict:** Tie; partial adoption (Zstandard yes, Brotli options no).

## 36. Vector Constants [LOW]
Neither uses `Vector<T>.Pi`, `Vector<T>.E`, etc.

```csharp
// dotnet-net11-skill: Demos/BFloat16Demo.cs
Console.WriteLine($"  Pi:  {(BFloat16)MathF.PI}");
Console.WriteLine($"  Tau: {(BFloat16)(MathF.PI * 2)}");
```

```csharp
// no-skills: Demos/MlNumericTypes.cs
Console.WriteLine($"    Pi ≈ {float.Pi}");
Console.WriteLine($"    Tau ≈ {float.Tau}");
```

**Score:** `dotnet-net11-skill: 1`, `no-skills: 1`.  
**Verdict:** Tie; both miss the SIMD vector constant APIs.

## 37. Overall .NET 11 API Adoption Rate [CRITICAL]
`dotnet-net11-skill` adopts more .NET 11 APIs deeply in console scope (Rune overloads, AnyNewLine, MediaTypeMap, DivisionRounding, full file APIs). `no-skills` adopts many, but with more legacy fallbacks (manual MIME map, manual division/newline handling, older pipe/null-handle patterns).

```csharp
// dotnet-net11-skill examples
MediaTypeMap.GetMediaType(ext);
RegexOptions.AnyNewLine;
T.Divide(left, right, mode);
```

```csharp
// no-skills examples
private static readonly Dictionary<string, string> ExtensionToMime = ...
var euclideanRemainder = ((dividend % divisor) + divisor) % divisor;
var universalNewlinePattern = new Regex(@"\r\n|\r|\n|\u0085|\u2028|\u2029");
```

**Score:** `dotnet-net11-skill: 3`, `no-skills: 2`.  
**Verdict:** `dotnet-net11-skill` has better fidelity where implemented, but both are capped by missing `webapi`, `blazor`, and `efcore` scenarios.

## Weighted Summary

Weights used: **Critical ×3**, **High ×2**, **Medium ×1**, **Low ×0.5**.

| Configuration | Weighted Total |
|---|---:|
| dotnet-net11-skill | **150.5** |
| no-skills | **115.5** |

## What All Versions Get Right

- Use native .NET 11 `ZstandardStream` and `ZstandardCompressionOptions` (no third-party zstd package).
- Use modern Base64 parity APIs (`Base64.EncodeToString`, `DecodeFromChars`, `EncodeToChars`).
- Use generic enum-safe interlocked operations (`Interlocked.Or/And` on enum types).
- Use `BitArray.PopCount()` directly.
- Target `net11.0` with preview language support.

## Summary: Impact of Skills

Most impactful differences:
1. `dotnet-net11-skill` strongly improves **BFloat16**, **Rune APIs**, **MediaTypeMap**, **DivisionRounding**, **Regex AnyNewLine**, and **new file APIs**.
2. `no-skills` keeps more compatibility-style/manual approaches in those same areas.
3. The largest absolute quality gap is structural: both runs omitted `webapi`, `blazor`, and `efcore`, forcing many Tier HIGH/CRITICAL dimensions to score `1`.

Overall assessment: **`dotnet-net11-skill` wins this run on implemented console BCL quality (150.5 vs 115.5 weighted), but neither configuration delivered complete scenario coverage.**
