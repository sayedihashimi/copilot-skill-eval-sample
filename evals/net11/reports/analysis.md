# Aggregated Analysis: .NET 11 Feature Adoption Evaluation

**Runs:** 1 | **Configurations:** 1 | **Scenarios:** 4 | **Dimensions:** 37
**Date:** 2026-04-18 23:20 UTC

---

## Overview

Evaluate how the dotnet-net11 skill improves Copilot's ability to generate code that correctly uses new .NET 11 APIs, C# language features, and framework capabilities instead of falling back to older patterns or third-party packages.

---

## What Was Tested

### Scenarios

Each run generates one of the following application scenarios (randomly selected per run):

| Scenario | Description |
|---|---|
| console-bcl | Create a console developer toolkit app that exercises new BCL features including Zstandard compression, BFloat16, Rune-based text processing, HMAC verification, frozen collections, MIME type mapping, hard links, division rounding, JSON serialization improvements, Base64 parity, regex enhancements, anonymous pipes, union types, and collection expression arguments.
 |
| webapi | Create a product catalog Web API that exercises new ASP.NET Core features including native OpenTelemetry tracing, OpenAPI 3.2, dynamic output cache policies, Zstandard response compression, and modern JSON serialization (PascalCase, per-member naming, type-level ignore, IReadOnlySet, generic GetTypeInfo).
 |
| blazor | Create a Blazor task management app that exercises new Blazor components and navigation features including EnvironmentBoundary, Label, DisplayName, QuickGrid OnRowClick, relative-to-current-URI navigation, hash fragment URIs, TempData, BasePath, variable-height virtualization, and SignalR connection configuration.
 |
| efcore | Create an EF Core order management demo that exercises new EF Core features including efficient state-based change tracking, clean DbContext replacement for testing, foreign key migration exclusion, and JSON column queries.
 |

### Configurations

Each configuration gives Copilot different custom skills or plugins. The **no-skills** baseline uses default Copilot with no custom instructions.

| Configuration | Description | Skills | Plugins |
|---|---|---|---|
| dotnet-net11-skill | dotnet-net11 Skill | — | dotnet-net11:dotnet-net11 |

### How It Works

1. **Generate** — For each configuration, Copilot CLI (`copilot --yolo`) is given a scenario prompt and generates a complete project from scratch. One scenario is randomly selected per run.
2. **Verify** — Each generated project is built (`dotnet build`), run, format-checked, and scanned for vulnerabilities.
3. **Analyze** — An AI judge reviews the source code of all configurations side-by-side and scores each across 37 quality dimensions.

Generation model: **claude-opus-4.6**
Analysis model: **gpt-5.3-codex**

---

## Scoring Methodology

Each dimension is scored on a **1–5 scale**:

| Score | Meaning |
|:---:|---|
| 5 | Excellent — follows all best practices |
| 4 | Good — minor gaps only |
| 3 | Acceptable — some issues present |
| 2 | Below average — significant gaps |
| 1 | Poor — missing or fundamentally wrong |

Dimensions are grouped into **tiers** that determine their weight in the final weighted score:

| Tier | Weight | Dimensions |
|---|:---:|:---:|
| CRITICAL | ×3 | 4 |
| HIGH | ×2 | 16 |
| MEDIUM | ×1 | 14 |
| LOW | ×0.5 | 3 |

**Maximum possible weighted score: 297.5** (all dimensions scoring 5).
Scores shown as **mean ± standard deviation** across runs.

---

## Executive Summary

Mean dimension scores across runs (1–5 scale, **higher is better**). ± values show standard deviation across runs.

| Dimension [Tier] | dotnet-net11-skill |
|---|---|
| Zstandard Compression Usage [CRITICAL] | 1.0 |
| BFloat16 Type Usage [HIGH] | 1.0 |
| Rune-Based String Operations [HIGH] | 1.0 |
| HMAC Single-Step Verification [HIGH] | 1.0 |
| FrozenDictionary Collection Expressions [HIGH] | 1.0 |
| Collection Expression with() Arguments [HIGH] | 1.0 |
| Union Type Usage [CRITICAL] | 1.0 |
| MediaTypeMap Usage [MEDIUM] | 1.0 |
| DivisionRounding Modes [MEDIUM] | 1.0 |
| System.Text.Json New Features [CRITICAL] | 1.0 |
| RegexOptions.AnyNewLine [MEDIUM] | 1.0 |
| File System New APIs [HIGH] | 1.0 |
| Base64 Parity APIs [MEDIUM] | 1.0 |
| Generic Interlocked Operations [MEDIUM] | 1.0 |
| BitArray.PopCount [LOW] | 1.0 |
| Native OpenTelemetry Tracing [HIGH] | 1.0 |
| OpenAPI Version [MEDIUM] | 1.0 |
| Dynamic Output Cache Policy Provider [HIGH] | 1.0 |
| Zstandard Response Compression [HIGH] | 1.0 |
| Blazor EnvironmentBoundary Component [HIGH] | 1.0 |
| Blazor Label and DisplayName Components [HIGH] | 1.0 |
| QuickGrid OnRowClick [HIGH] | 1.0 |
| RelativeToCurrentUri Navigation [MEDIUM] | 1.0 |
| Blazor TempData Support [HIGH] | 1.0 |
| Blazor BasePath Component [MEDIUM] | 1.0 |
| EF Core GetEntriesForState [HIGH] | 5.0 |
| EF Core RemoveDbContext [HIGH] | 5.0 |
| EF Core ExcludeForeignKeyFromMigrations [MEDIUM] | 5.0 |
| EF Core JSON Query Functions [HIGH] | 4.0 |
| SignalR ConfigureConnection [MEDIUM] | 1.0 |
| Blazor Virtualize Variable-Height Items [MEDIUM] | 1.0 |
| Runtime Async Configuration [MEDIUM] | 1.0 |
| ProcessExitStatus Usage [MEDIUM] | 1.0 |
| OpenAPI Binary File Response [MEDIUM] | 1.0 |
| Brotli and Compression Options [LOW] | 1.0 |
| Vector Constants [LOW] | 1.0 |
| Overall .NET 11 API Adoption Rate [CRITICAL] | 2.0 |

---

## Final Rankings

Configurations ranked by mean weighted score — **higher is better**. Std Dev shows run-to-run variability (lower = more consistent).

| Rank | Configuration | Mean Score ↑ | % of Max (297.5) | Std Dev ↓ | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | dotnet-net11-skill | 88.5 | 30% | 0.0 | 88.5 | 88.5 |

---

## Weighted Score per Run

| Run | dotnet-net11-skill |
|---|---|
| 1 | 88.5 |
| **Mean** | **88.5** |

---

## Verification Summary (All Runs)

| Configuration | Build Pass Rate | Run Pass Rate | Avg Warnings |
|---|---|---|---|
| dotnet-net11-skill | 1/1 (100%) | 1/1 (100%) | 0.0 |

---

## Token Usage Summary

Average token consumption per configuration across all runs.

| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time |
|---|---|---|---|---|---|
| dotnet-net11-skill | 2,470,389 | 24,537 | 2,368,378 | 47 | 10m 20s |

---

## Token Usage Per Run

| Configuration | Run | Scenario | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time | Note |
|---|---|---|---|---|---|---|---|---|
| dotnet-net11-skill | 1 | efcore | 2,470,389 | 24,537 | 2,368,378 | 47 | 10m 20s |  |


---

## Per-Dimension Analysis

### 1. Zstandard Compression Usage [CRITICAL × 3]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

`dotnet-net11-skill` does not include console or web API compression code in run-1; the entry point only orchestrates EF demos.

```csharp
// dotnet-net11-skill: efcore-showcase/Program.cs
ChangeTrackingDemo.Run(db);
DbContextReplacementDemo.Run();
ForeignKeyExclusionDemo.Run(db);
JsonQueryDemo.Run(db);
```

**Score:** 1/5 — no `Zstandard*` APIs and no compression implementation surface.  
**Verdict:** Missing; best practice is built-in .NET 11 Zstandard APIs over third-party packages.

### 2. BFloat16 Type Usage [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No BFloat16 usage appears in the provided EF-only codebase.

```csharp
// dotnet-net11-skill: efcore-showcase/Program.cs
const string DbPath = "ordervault_demo.db";
var options = new DbContextOptionsBuilder<OrderVaultContext>()
    .UseSqlite($"Data Source={DbPath}")
    .Options;
```

**Score:** 1/5 — no `System.Numerics.BFloat16` or related APIs.  
**Verdict:** Missing; native BFloat16 should replace manual bit-level work for ML-friendly numeric paths.

### 3. Rune-Based String Operations [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No Unicode Rune APIs are used.

```csharp
// dotnet-net11-skill: Demos/JsonQueryDemo.cs
var fallbackPath = db.Products
    .Where(p => p.Metadata.Contains("\"brand\""))
    .ToList();
```

**Score:** 1/5 — no Rune-based operations (`Contains(Rune)`, `IndexOf(Rune)`, etc.).  
**Verdict:** Missing; Rune APIs are the preferred .NET 11 pattern for Unicode-safe text handling.

### 4. HMAC Single-Step Verification [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No HMAC verification code is present.

```csharp
// dotnet-net11-skill: efcore-showcase/Program.cs
using (var db = new OrderVaultContext(options))
{
    JsonQueryDemo.Run(db);
}
```

**Score:** 1/5 — no `HMACSHA256.Verify` or `CryptographicOperations.VerifyHmac`.  
**Verdict:** Missing; single-step verification should be used for safer auth/message checks.

### 5. FrozenDictionary Collection Expressions [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No frozen collections are used.

```csharp
// dotnet-net11-skill: Data/OrderVaultContext.cs
public DbSet<Customer> Customers => Set<Customer>();
public DbSet<Product> Products => Set<Product>();
public DbSet<Order> Orders => Set<Order>();
```

**Score:** 1/5 — no `FrozenDictionary<K,V>` collection-expression initialization.  
**Verdict:** Missing; .NET 11 collection expressions for frozen collections are not adopted.

### 6. Collection Expression with() Arguments [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No C# 15 collection-expression `with(...)` usage is present.

```csharp
// dotnet-net11-skill: Demos/ChangeTrackingDemo.cs
var allDirty = db.ChangeTracker
    .GetEntriesForState(added: true, modified: true, deleted: true, unchanged: false)
    .ToList();
```

**Score:** 1/5 — no evidence of `with(capacity: n)` style collection expressions.  
**Verdict:** Missing; modern C# collection-expression argument patterns are absent.

### 7. Union Type Usage [CRITICAL × 3]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No union/discriminated union type usage appears.

```csharp
// dotnet-net11-skill: Demos/ForeignKeyExclusionDemo.cs
public static class ForeignKeyExclusionDemo
{
    public static void Run(OrderVaultContext db) { /* ... */ }
}
```

**Score:** 1/5 — no `union` keyword or exhaustive union switching.  
**Verdict:** Missing; modern C# 15 union patterns are not used.

### 8. MediaTypeMap Usage [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No MIME mapping logic is present.

```csharp
// dotnet-net11-skill: efcore-showcase/Program.cs
Console.WriteLine("║   OrderVault — .NET 11 EF Core Showcase  ║");
```

**Score:** 1/5 — no `MediaTypeMap` APIs used.  
**Verdict:** Missing; MIME mapping feature not exercised.

### 9. DivisionRounding Modes [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No division-rounding APIs are used.

```csharp
// dotnet-net11-skill: Data/SeedData.cs (representative usage is EF seeding, no division APIs)
// (No int.Divide/DivRem with DivisionRounding usage in run-1 sources)
```

**Score:** 1/5 — no `DivisionRounding`-based APIs.  
**Verdict:** Missing; manual/legacy paths are effectively untested in this run.

### 10. System.Text.Json New Features [CRITICAL × 3]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No JSON serializer configuration or .NET 11 STJ enhancements are used.

```csharp
// dotnet-net11-skill: Demos/JsonQueryDemo.cs
// JSON is treated as text columns queried via EF, not via System.Text.Json settings.
var saleProducts = db.Products
    .Where(p => p.Tags.Contains("sale"))
    .OrderBy(p => p.Price)
    .ToList();
```

**Score:** 1/5 — no `JsonNamingPolicy.PascalCase`, type-level ignore, or `GetTypeInfo<T>()` usage.  
**Verdict:** Missing; .NET 11 JSON serializer capabilities are not demonstrated.

### 11. RegexOptions.AnyNewLine [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No regex usage appears.

```csharp
// dotnet-net11-skill: all C# sources under efcore-showcase
// (No Regex APIs used)
```

**Score:** 1/5 — no `RegexOptions.AnyNewLine`.  
**Verdict:** Missing.

### 12. File System New APIs [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

File handling is basic (`File.Exists`, `File.Delete`) and does not use new APIs.

```csharp
// dotnet-net11-skill: efcore-showcase/Program.cs
if (File.Exists(DbPath))
    File.Delete(DbPath);
```

**Score:** 1/5 — no `File.CreateHardLink`, `File.OpenNullHandle`, or `CreateAnonymousPipe`.  
**Verdict:** Missing; advanced cross-platform file APIs are not adopted.

### 13. Base64 Parity APIs [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No Base64 code appears.

```csharp
// dotnet-net11-skill: all run-1 efcore sources
// (No Base64 APIs used)
```

**Score:** 1/5 — no `Base64.EncodeToString/DecodeFromChars/GetEncodedLength`.  
**Verdict:** Missing.

### 14. Generic Interlocked Operations [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No interlocked enum operations are present.

```csharp
// dotnet-net11-skill: all run-1 efcore sources
// (No Interlocked APIs used)
```

**Score:** 1/5 — no generic `Interlocked.And/Or` on enums.  
**Verdict:** Missing.

### 15. BitArray.PopCount [LOW × 0]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No bit-array logic appears.

```csharp
// dotnet-net11-skill: all run-1 efcore sources
// (No BitArray APIs used)
```

**Score:** 1/5 — no `BitArray.PopCount()`.  
**Verdict:** Missing.

### 16. Native OpenTelemetry Tracing [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No ASP.NET Core project is present in this run.

```csharp
// dotnet-net11-skill: efcore-showcase/efcore-showcase.csproj
<OutputType>Exe</OutputType>
<TargetFramework>net11.0</TargetFramework>
```

**Score:** 1/5 — no `AddSource("Microsoft.AspNetCore")` or equivalent tracing setup.  
**Verdict:** Missing due absent webapi scenario.

### 17. OpenAPI Version [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No OpenAPI configuration exists in the available sources.

```csharp
// dotnet-net11-skill: run-1 has only efcore scenario sources
// (No OpenAPI configuration files/code)
```

**Score:** 1/5 — no `OpenApiSpecVersion.OpenApi3_2`.  
**Verdict:** Missing.

### 18. Dynamic Output Cache Policy Provider [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No output-caching policy provider code is present.

```csharp
// dotnet-net11-skill: run-1 efcore console project only
// (No IOutputCachePolicyProvider implementation)
```

**Score:** 1/5 — no dynamic provider-based output cache policy setup.  
**Verdict:** Missing.

### 19. Zstandard Response Compression [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No ASP.NET compression pipeline is implemented.

```csharp
// dotnet-net11-skill: no web API Program.cs in run-1
// (No AddResponseCompression / Zstandard provider options)
```

**Score:** 1/5 — no response/request Zstandard compression setup.  
**Verdict:** Missing.

### 20. Blazor EnvironmentBoundary Component [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No Blazor code is present.

```razor
// dotnet-net11-skill: run-1 contains no .razor files
```

**Score:** 1/5 — no `<EnvironmentBoundary ...>` usage.  
**Verdict:** Missing.

### 21. Blazor Label and DisplayName Components [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No Blazor forms/tables are present.

```razor
// dotnet-net11-skill: run-1 contains no .razor files
```

**Score:** 1/5 — no `<Label For>` / `<DisplayName For>`.  
**Verdict:** Missing.

### 22. QuickGrid OnRowClick [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No QuickGrid usage appears.

```razor
// dotnet-net11-skill: run-1 contains no .razor files
```

**Score:** 1/5 — no `OnRowClick` usage.  
**Verdict:** Missing.

### 23. RelativeToCurrentUri Navigation [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No Blazor navigation code is present.

```razor
// dotnet-net11-skill: run-1 contains no .razor files
```

**Score:** 1/5 — no `RelativeToCurrentUri` / URI helper usage.  
**Verdict:** Missing.

### 24. Blazor TempData Support [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No TempData support appears.

```razor
// dotnet-net11-skill: run-1 contains no .razor files
```

**Score:** 1/5 — no `[CascadingParameter] ITempData` usage.  
**Verdict:** Missing.

### 25. Blazor BasePath Component [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No Blazor host/layout code exists in run-1.

```razor
// dotnet-net11-skill: run-1 contains no .razor files
```

**Score:** 1/5 — no `<BasePath />` usage.  
**Verdict:** Missing.

### 26. EF Core GetEntriesForState [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

This is implemented directly and correctly using the .NET 11 API.

```csharp
// dotnet-net11-skill: Demos/ChangeTrackingDemo.cs
var modifiedEntries = db.ChangeTracker
    .GetEntriesForState(added: false, modified: true, deleted: false, unchanged: false)
    .ToList();
```

**Score:** 5/5 — explicit state-based retrieval via `GetEntriesForState` is present and primary.  
**Verdict:** Best-practice implementation; avoids unnecessary `DetectChanges` overhead versus legacy `Entries().Where(...)`.

### 27. EF Core RemoveDbContext [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

The code uses the dedicated removal API before re-registration.

```csharp
// dotnet-net11-skill: Demos/DbContextReplacementDemo.cs
services.RemoveDbContext<OrderVaultContext>();
services.AddDbContext<OrderVaultContext>(options =>
    options.UseSqlite("Data Source=:memory:"));
```

**Score:** 5/5 — clean remove/re-register flow follows intended .NET 11 design.  
**Verdict:** Best approach for test replacement scenarios; avoids stale/duplicate registration artifacts.

### 28. EF Core ExcludeForeignKeyFromMigrations [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 5 |
| **Mean** | **5.0** |

#### Analysis

This API is used directly in model configuration.

```csharp
// dotnet-net11-skill: Data/OrderVaultContext.cs
e.HasOne(oi => oi.Product)
 .WithMany()
 .HasForeignKey(oi => oi.ProductId)
 .ExcludeForeignKeyFromMigrations(true);
```

**Score:** 5/5 — direct use of the dedicated fluent API, plus verification demo.  
**Verdict:** Excellent and maintainable versus manual migration surgery.

### 29. EF Core JSON Query Functions [HIGH × 2]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 4 |
| **Mean** | **4.0** |

#### Analysis

The demo uses `EF.Functions.JsonPathExists(...)` in LINQ and handles provider fallback.

```csharp
// dotnet-net11-skill: Demos/JsonQueryDemo.cs
var jsonPathResults = db.Products
    .Where(p => EF.Functions.JsonPathExists(p.Metadata, "$.brand"))
    .ToList();
```

**Score:** 4/5 — strong adoption of built-in JSON query function, but only `JsonPathExists` is shown (not `JsonContains`).  
**Verdict:** Good modern usage; still slightly less complete than a full `JsonContains` + `JsonPathExists` coverage set.

### 30. SignalR ConfigureConnection [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No interactive server component configuration is present.

```csharp
// dotnet-net11-skill: run-1 efcore-only project
// (No SignalR server-component configuration)
```

**Score:** 1/5 — no `ConfigureConnection` callback usage.  
**Verdict:** Missing.

### 31. Blazor Virtualize Variable-Height Items [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No Blazor virtualization code is present.

```razor
// dotnet-net11-skill: run-1 contains no .razor files
```

**Score:** 1/5 — no variable-height `<Virtualize>` usage.  
**Verdict:** Missing.

### 32. Runtime Async Configuration [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

Project targets `net11.0` but does not configure runtime async features in project properties.

```xml
<!-- dotnet-net11-skill: efcore-showcase.csproj -->
<PropertyGroup>
  <TargetFramework>net11.0</TargetFramework>
</PropertyGroup>
```

**Score:** 1/5 — no `<Features>runtime-async=on</Features>` usage.  
**Verdict:** Missing.

### 33. ProcessExitStatus Usage [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No process-management code appears.

```csharp
// dotnet-net11-skill: all run-1 efcore sources
// (No Process API usage)
```

**Score:** 1/5 — no `process.ExitStatus` usage.  
**Verdict:** Missing.

### 34. OpenAPI Binary File Response [MEDIUM × 1]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No web API endpoints are present.

```csharp
// dotnet-net11-skill: run-1 efcore-only project
// (No endpoint metadata, no Produces<FileContentResult>)
```

**Score:** 1/5 — no binary response OpenAPI metadata implementation.  
**Verdict:** Missing.

### 35. Brotli and Compression Options [LOW × 0]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No compression options code appears.

```csharp
// dotnet-net11-skill: run-1 efcore-only project
// (No Brotli/Zstandard options configuration)
```

**Score:** 1/5 — no `BrotliCompressionOptions.WindowLog` usage.  
**Verdict:** Missing.

### 36. Vector Constants [LOW × 0]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 1 |
| **Mean** | **1.0** |

#### Analysis

No SIMD vector math constants are used.

```csharp
// dotnet-net11-skill: all run-1 efcore sources
// (No Vector<T>.Pi/E/Tau/etc usage)
```

**Score:** 1/5 — no vector constants adoption.  
**Verdict:** Missing.

### 37. Overall .NET 11 API Adoption Rate [CRITICAL × 3]

#### Scores Across Runs

| Run | dotnet-net11-skill |
|---|---|
| 1 | 2 |
| **Mean** | **2.0** |

#### Analysis

Adoption is concentrated in EF Core APIs, with broad gaps across console BCL, web API, and Blazor dimensions for this run.

```csharp
// dotnet-net11-skill: Program.cs (scope)
ChangeTrackingDemo.Run(db);
DbContextReplacementDemo.Run();
ForeignKeyExclusionDemo.Run(db);
JsonQueryDemo.Run(db);
```

**Score:** 2/5 — several EF Core .NET 11 APIs are implemented well, but most rubric dimensions are unimplemented due missing scenarios/features in run-1.  
**Verdict:** Partial success: strong EF Core specialization, weak overall breadth.

---

## Asset Usage Summary

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| dotnet-net11-skill | 1 | 7a1d4feb…84d7 | claude-opus-4.6 | — | — | ✅ |

---

## Raw Data References

- Per-run analysis: `reports/analysis-run-1.md`
- Verification data: `reports/verification-data.json`
- Score data: `reports/scores-data.json`
- Build notes: `reports/build-notes.md`
- Generation usage: `reports/generation-usage.json`
