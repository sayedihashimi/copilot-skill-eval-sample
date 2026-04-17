# Comparative Analysis: dotnet-net11-skill

This run contains **1 configuration** under `output/`: `dotnet-net11-skill` (inferred from directory name; no top-level `gen-notes.md` for the config root). Under `output/dotnet-net11-skill/run-1/`, the discovered app folders are `webapi/`, `efcore/`, and `apicheck/`; expected `console-bcl/` and `blazor/` folders are missing. Configuration evidence is in `webapi/gen-notes.md`, which states use of `net11-json-webapi` and `net11-features`.

## Executive Summary

| Dimension [Tier] | dotnet-net11-skill |
|---|---:|
| Zstandard Compression Usage [CRITICAL] | 5 |
| BFloat16 Type Usage [HIGH] | 1 |
| Rune-Based String Operations [HIGH] | 2 |
| HMAC Single-Step Verification [HIGH] | 1 |
| FrozenDictionary Collection Expressions [HIGH] | 2 |
| Collection Expression with() Arguments [HIGH] | 5 |
| Union Type Usage [CRITICAL] | 1 |
| MediaTypeMap Usage [MEDIUM] | 2 |
| DivisionRounding Modes [MEDIUM] | 1 |
| System.Text.Json New Features [CRITICAL] | 3 |
| RegexOptions.AnyNewLine [MEDIUM] | 1 |
| File System New APIs [HIGH] | 1 |
| Base64 Parity APIs [MEDIUM] | 2 |
| Generic Interlocked Operations [MEDIUM] | 1 |
| BitArray.PopCount [LOW] | 1 |
| Native OpenTelemetry Tracing [HIGH] | 2 |
| OpenAPI Version [MEDIUM] | 5 |
| Dynamic Output Cache Policy Provider [HIGH] | 5 |
| Zstandard Response Compression [HIGH] | 5 |
| Blazor EnvironmentBoundary Component [HIGH] | 1 |
| Blazor Label and DisplayName Components [HIGH] | 1 |
| QuickGrid OnRowClick [HIGH] | 1 |
| RelativeToCurrentUri Navigation [MEDIUM] | 1 |
| Blazor TempData Support [HIGH] | 1 |
| Blazor BasePath Component [MEDIUM] | 1 |
| EF Core GetEntriesForState [HIGH] | 1 |
| EF Core RemoveDbContext [HIGH] | 1 |
| EF Core ExcludeForeignKeyFromMigrations [MEDIUM] | 1 |
| EF Core JSON Query Functions [HIGH] | 1 |
| SignalR ConfigureConnection [MEDIUM] | 1 |
| Blazor Virtualize Variable-Height Items [MEDIUM] | 1 |
| Runtime Async Configuration [MEDIUM] | 1 |
| ProcessExitStatus Usage [MEDIUM] | 1 |
| OpenAPI Binary File Response [MEDIUM] | 5 |
| Brotli and Compression Options [LOW] | 1 |
| Vector Constants [LOW] | 1 |
| Overall .NET 11 API Adoption Rate [CRITICAL] | 2 |

## 1. Zstandard Compression Usage [CRITICAL]
```csharp
// dotnet-net11-skill (webapi/Program.cs)
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<ZstandardCompressionProvider>();
});
```
**Score (dotnet-net11-skill): 5/5** — Uses built-in ASP.NET Core Zstandard provider, not third-party packages.  
**Verdict:** Strong modern usage.

## 2. BFloat16 Type Usage [HIGH]
```md
<!-- dotnet-net11-skill (webapi/gen-notes.md) -->
| `BFloat16` | `System.Numerics.BFloat16` | Not included (demo only) |
```
**Score: 1/5** — No BFloat16 API usage in code.  
**Verdict:** Missing a priority numeric API.

## 3. Rune-Based String Operations [HIGH]
```csharp
// dotnet-net11-skill (webapi/FeatureCoverage/Net11PriorityApiDemo.cs)
var text = "ProductHub \U0001F680 Catalog";
var rocketRune = new Rune(0x1F680);
var runeCount = text.EnumerateRunes().Count();
```
**Score: 2/5** — Rune appears, but not the new `string.*(Rune)` style APIs called out in rubric.  
**Verdict:** Partial Unicode modernization.

## 4. HMAC Single-Step Verification [HIGH]
```md
<!-- dotnet-net11-skill (webapi/gen-notes.md) -->
| `HMACSHA256.Verify` | Single-step HMAC verify | Not included (demo only) |
```
**Score: 1/5** — Not implemented.  
**Verdict:** Security-related priority API missing.

## 5. FrozenDictionary Collection Expressions [HIGH]
```csharp
// dotnet-net11-skill (webapi/Services/ProductService.cs)
private static readonly FrozenDictionary<string, decimal> DefaultPrices =
    new Dictionary<string, decimal> { ["Electronics"] = 99.99m }.ToFrozenDictionary();
```
**Score: 2/5** — Uses `FrozenDictionary`, but old `Dictionary + ToFrozenDictionary()` path.  
**Verdict:** Functional but not .NET 11-first idiom.

## 6. Collection Expression with() Arguments [HIGH]
```csharp
// dotnet-net11-skill (webapi/FeatureCoverage/Net11PriorityApiDemo.cs)
List<int> values = [with(capacity: 16), ..sourceValues];
```
**Score: 5/5** — Correct C# 15 collection expression with `with(capacity:)`.  
**Verdict:** Excellent adoption.

## 7. Union Type Usage [CRITICAL]
```csharp
// dotnet-net11-skill (webapi/Models/ApiResult.cs)
public abstract record ApiResult<T>
{
    public sealed record Success(T Value) : ApiResult<T>;
}
```
**Score: 1/5** — Uses class hierarchy fallback; no `union` keyword/types.  
**Verdict:** Misses core language feature target.

## 8. MediaTypeMap Usage [MEDIUM]
```csharp
// dotnet-net11-skill (webapi/Endpoints/MimeEndpoints.cs)
private static readonly FileExtensionContentTypeProvider ContentTypeProvider = new();
```
**Score: 2/5** — Built-in ASP.NET provider, but not `MediaTypeMap.GetMediaType/GetExtension`.  
**Verdict:** Acceptable fallback, not target API.

## 9. DivisionRounding Modes [MEDIUM]
```md
<!-- dotnet-net11-skill (webapi/gen-notes.md) -->
| `DivisionRounding` | `int.Divide(..., DivisionRounding.*)` | Not included (demo only) |
```
**Score: 1/5** — Not implemented.  
**Verdict:** Missing.

## 10. System.Text.Json New Features [CRITICAL]
```csharp
// dotnet-net11-skill (webapi/Models/Product.cs)
[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
public IReadOnlySet<string> Tags { get; set; } = new HashSet<string>();
[JsonPropertyName("metadata")] public Dictionary<string, object>? Metadata { get; set; }
```
```csharp
// dotnet-net11-skill (webapi/Endpoints/ProductEndpoints.cs)
var typeInfo = JsonSerializerOptions.Default.GetTypeInfo<Product>();
```
**Score: 3/5** — Good coverage of `IReadOnlySet`, type-level ignore, and generic `GetTypeInfo<T>()`; does not use `JsonNamingPolicy.PascalCase`/`[JsonNamingPolicy]`.  
**Verdict:** Mixed; solid partial adoption.

## 11. RegexOptions.AnyNewLine [MEDIUM]
```text
// dotnet-net11-skill (run-1/webapi)
No RegexOptions.AnyNewLine usage found in source files.
```
**Score: 1/5** — Missing.  
**Verdict:** No Unicode newline handling via new regex option.

## 12. File System New APIs [HIGH]
```text
// dotnet-net11-skill (run-1/webapi)
No File.CreateHardLink / File.OpenNullHandle / CreateAnonymousPipe usage found.
```
**Score: 1/5** — Missing.  
**Verdict:** No new FS API adoption.

## 13. Base64 Parity APIs [MEDIUM]
```csharp
// dotnet-net11-skill (webapi/Endpoints/ExportEndpoints.cs)
var base64 = Convert.ToBase64String(csvBytes);
```
**Score: 2/5** — Base64 functionality exists, but legacy `Convert` API is used.  
**Verdict:** Works, but not new parity APIs.

## 14. Generic Interlocked Operations [MEDIUM]
```csharp
// dotnet-net11-skill (webapi/Services/ProductService.cs)
Id = Interlocked.Increment(ref _nextId),
```
**Score: 1/5** — Uses classic integer interlocked only; no generic enum `Interlocked.And/Or`.  
**Verdict:** Missing target pattern.

## 15. BitArray.PopCount [LOW]
```text
// dotnet-net11-skill (run-1/webapi)
No BitArray.PopCount() usage found.
```
**Score: 1/5** — Missing.  
**Verdict:** Low-priority API not adopted.

## 16. Native OpenTelemetry Tracing [HIGH]
```csharp
// dotnet-net11-skill (webapi/Program.cs)
builder.Logging.AddConsole();
var activitySource = new System.Diagnostics.ActivitySource("ProductHub");
builder.Services.AddSingleton(activitySource);
```
**Score: 2/5** — Tracing intent exists, but does not show `AddSource("Microsoft.AspNetCore")` path from rubric.  
**Verdict:** Partial observability; misses expected native integration style.

## 17. OpenAPI Version [MEDIUM]
```csharp
// dotnet-net11-skill (webapi/Program.cs)
options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_2;
```
**Score: 5/5** — Exact OpenAPI 3.2 target used.  
**Verdict:** Excellent.

## 18. Dynamic Output Cache Policy Provider [HIGH]
```csharp
// dotnet-net11-skill (webapi/Program.cs)
builder.Services.AddOutputCache();
builder.Services.AddSingleton<IOutputCachePolicyProvider, CatalogOutputCachePolicyProvider>();
```
```csharp
// dotnet-net11-skill (webapi/Services/CatalogOutputCachePolicyProvider.cs)
public ValueTask<IOutputCachePolicy?> GetPolicyAsync(string policyName) { ... }
```
**Score: 5/5** — Correct provider interface and DI-based dynamic resolution.  
**Verdict:** Best-practice architecture.

## 19. Zstandard Response Compression [HIGH]
```csharp
// dotnet-net11-skill (webapi/Program.cs)
options.Providers.Add<ZstandardCompressionProvider>();
builder.Services.AddRequestDecompression();
app.UseRequestDecompression();
app.UseResponseCompression();
```
**Score: 5/5** — Response and request compression are wired with Zstandard provider.  
**Verdict:** Strong modern ASP.NET compression setup.

## 20. Blazor EnvironmentBoundary Component [HIGH]
```text
// dotnet-net11-skill (run-1 contents)
apicheck/
copilot-chat.md
efcore/
events.jsonl
webapi/
```
**Score: 1/5** — No `blazor/` app present; feature absent.  
**Verdict:** Not implemented.

## 21. Blazor Label and DisplayName Components [HIGH]
```text
// dotnet-net11-skill (run-1 contents)
No Blazor component source files present.
```
**Score: 1/5** — Missing with absent Blazor scenario.  
**Verdict:** Not implemented.

## 22. QuickGrid OnRowClick [HIGH]
```text
// dotnet-net11-skill (run-1 contents)
No .razor files detected under run-1.
```
**Score: 1/5** — No QuickGrid usage exists.  
**Verdict:** Not implemented.

## 23. RelativeToCurrentUri Navigation [MEDIUM]
```text
// dotnet-net11-skill (run-1 contents)
No Blazor navigation code found.
```
**Score: 1/5** — Missing due absent Blazor app.  
**Verdict:** Not implemented.

## 24. Blazor TempData Support [HIGH]
```text
// dotnet-net11-skill (run-1 contents)
No Blazor SSR TempData usage found.
```
**Score: 1/5** — Missing.  
**Verdict:** Not implemented.

## 25. Blazor BasePath Component [MEDIUM]
```text
// dotnet-net11-skill (run-1 contents)
No Blazor app files; <BasePath /> not present.
```
**Score: 1/5** — Missing.  
**Verdict:** Not implemented.

## 26. EF Core GetEntriesForState [HIGH]
```ini
; dotnet-net11-skill (efcore/OrderVault/OrderVault.csproj.lscache)
[sourceFiles]
obj/Debug/net11.0/
 .NETCoreApp,Version=v11.0.AssemblyAttributes.cs
 OrderVault.AssemblyInfo.cs
 OrderVault.GlobalUsings.g.cs
```
**Score: 1/5** — EF Core source implementation is absent; target API not demonstrated.  
**Verdict:** Not implemented.

## 27. EF Core RemoveDbContext [HIGH]
```ini
; dotnet-net11-skill (efcore/OrderVault/OrderVault.csproj.lscache)
[sourceFiles]
obj/Debug/net11.0/
 OrderVault.AssemblyInfo.cs
```
**Score: 1/5** — No `RemoveDbContext<T>()` pattern in available code.  
**Verdict:** Not implemented.

## 28. EF Core ExcludeForeignKeyFromMigrations [MEDIUM]
```text
// dotnet-net11-skill (efcore app folder)
No migration/model source files present to show FK exclusion API.
```
**Score: 1/5** — Missing.  
**Verdict:** Not implemented.

## 29. EF Core JSON Query Functions [HIGH]
```text
// dotnet-net11-skill (efcore app folder)
No EF.Functions.JsonContains / JsonPathExists usage found.
```
**Score: 1/5** — Missing.  
**Verdict:** Not implemented.

## 30. SignalR ConfigureConnection [MEDIUM]
```text
// dotnet-net11-skill (run-1 contents)
No Blazor interactive server configuration found.
```
**Score: 1/5** — Missing.  
**Verdict:** Not implemented.

## 31. Blazor Virtualize Variable-Height Items [MEDIUM]
```text
// dotnet-net11-skill (run-1 contents)
No <Virtualize> components present.
```
**Score: 1/5** — Missing.  
**Verdict:** Not implemented.

## 32. Runtime Async Configuration [MEDIUM]
```xml
<!-- dotnet-net11-skill (webapi/ProductHub.csproj) -->
<PropertyGroup>
  <TargetFramework>net11.0</TargetFramework>
  <LangVersion>preview</LangVersion>
</PropertyGroup>
```
**Score: 1/5** — No `<Features>runtime-async=on</Features>` configuration.  
**Verdict:** Not implemented.

## 33. ProcessExitStatus Usage [MEDIUM]
```text
// dotnet-net11-skill (run-1/webapi)
No process.ExitStatus access patterns found.
```
**Score: 1/5** — Missing structured process status usage.  
**Verdict:** Not implemented.

## 34. OpenAPI Binary File Response [MEDIUM]
```csharp
// dotnet-net11-skill (webapi/Endpoints/ExportEndpoints.cs)
.Produces<FileContentResult>(200, contentType: "application/octet-stream");
```
**Score: 5/5** — Correct binary download metadata for OpenAPI.  
**Verdict:** Excellent API docs practice.

## 35. Brotli and Compression Options [LOW]
```csharp
// dotnet-net11-skill (webapi/Program.cs)
options.Providers.Add<BrotliCompressionProvider>();
```
**Score: 1/5** — Brotli provider enabled, but no new options like `WindowLog`.  
**Verdict:** Legacy/basic usage only.

## 36. Vector Constants [LOW]
```text
// dotnet-net11-skill (run-1/webapi)
No Vector<float>.Pi/E/Tau/etc usage found.
```
**Score: 1/5** — Missing.  
**Verdict:** Low-priority SIMD constants not adopted.

## 37. Overall .NET 11 API Adoption Rate [CRITICAL]
```md
<!-- dotnet-net11-skill -->
Strong: OpenAPI 3.2, dynamic output cache provider, Zstd response/request compression, collection with().
Weak/missing: union keyword, BFloat16, HMAC verify, DivisionRounding, MediaTypeMap, EF/Blazor scenario features.
```
**Score: 2/5** — Some high-value APIs are correct in `webapi`, but large portions of requested .NET 11 surface are absent or fallback-based, and two scenario apps are missing.  
**Verdict:** Partial adoption with major coverage gaps.

## Weighted Summary

Weights: **Critical ×3**, **High ×2**, **Medium ×1**, **Low ×0.5**.

| Configuration | Weighted Total | Max Possible | Percent |
|---|---:|---:|---:|
| dotnet-net11-skill | 120.5 | 297.5 | 40.5% |

## What All Versions Get Right

- The generated `webapi` project correctly targets **`net11.0`**.
- OpenAPI metadata quality is strong where implemented (`OpenApi3_2`, binary response metadata).
- Output caching architecture uses a clean **provider-based DI model** rather than only static policies.
- Compression stack includes built-in **Zstandard provider** without external Zstd NuGet dependencies.

## Summary: Impact of Skills

Most impactful positive differences in this run are: **(1)** correct OpenAPI 3.2 adoption, **(2)** dynamic output cache policy provider architecture, and **(3)** Zstandard response/request compression wiring. The largest negative impact is **coverage incompleteness**: missing `console-bcl` and `blazor` scenario apps plus absent EF Core feature code significantly depress overall adoption.

Overall assessment: **dotnet-net11-skill is strong in web API plumbing but incomplete as an end-to-end net11 feature showcase**, with a weighted score of **120.5/297.5 (40.5%)**.
