# Aggregated Analysis: .NET 11 Feature Adoption Evaluation

**Runs:** 1 | **Configurations:** 2 | **Scenarios:** 4 | **Dimensions:** 38
**Date:** 2026-04-17 05:47 UTC

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
| no-skills | Baseline (default Copilot) | — | — |
| dotnet-net11-skill | dotnet-net11 Skill | — | dotnet-net11:dotnet-net11 |

### How It Works

1. **Generate** — For each configuration, Copilot CLI (`copilot --yolo`) is given a scenario prompt and generates a complete project from scratch. One scenario is randomly selected per run.
2. **Verify** — Each generated project is built (`dotnet build`), run, format-checked, and scanned for vulnerabilities.
3. **Analyze** — An AI judge reviews the source code of all configurations side-by-side and scores each across 38 quality dimensions.

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
| MEDIUM | ×1 | 15 |
| LOW | ×0.5 | 3 |

**Maximum possible weighted score: 302.5** (all dimensions scoring 5).
Scores shown as **mean ± standard deviation** across runs.

---

## Executive Summary

Mean dimension scores across runs (1–5 scale, **higher is better**). ± values show standard deviation across runs.

| Dimension [Tier] | no-skills | dotnet-net11-skill |
|---|---|---|
| Zstandard Compression Usage [CRITICAL] | 1.0 | 5.0 |
| BFloat16 Type Usage [HIGH] | 1.0 | 4.0 |
| Rune-Based String Operations [HIGH] | 1.0 | 5.0 |
| HMAC Single-Step Verification [HIGH] | 1.0 | 5.0 |
| FrozenDictionary Collection Expressions [HIGH] | 1.0 | 5.0 |
| Collection Expression with() Arguments [HIGH] | 1.0 | 5.0 |
| Union Type Usage [CRITICAL] | 1.0 | 5.0 |
| MediaTypeMap Usage [MEDIUM] | 1.0 | 5.0 |
| DivisionRounding Modes [MEDIUM] | 1.0 | 5.0 |
| System.Text.Json New Features [CRITICAL] | 1.0 | 3.0 |
| RegexOptions.AnyNewLine [MEDIUM] | 1.0 | 1.0 |
| File System New APIs [HIGH] | 1.0 | 1.0 |
| Base64 Parity APIs [MEDIUM] | 1.0 | 1.0 |
| Generic Interlocked Operations [MEDIUM] | 1.0 | 1.0 |
| BitArray.PopCount [LOW] | 1.0 | 1.0 |
| Native OpenTelemetry Tracing [HIGH] | 1.0 | 1.0 |
| OpenAPI Version [MEDIUM] | 1.0 | 1.0 |
| Dynamic Output Cache Policy Provider [HIGH] | 1.0 | 1.0 |
| Zstandard Response Compression [HIGH] | 1.0 | 1.0 |
| Blazor EnvironmentBoundary Component [HIGH] | 1.0 | 5.0 |
| Blazor Label and DisplayName Components [HIGH] | 1.0 | 5.0 |
| QuickGrid OnRowClick [HIGH] | 1.0 | 5.0 |
| RelativeToCurrentUri Navigation [MEDIUM] | 1.0 | 5.0 |
| Blazor TempData Support [HIGH] | 1.0 | 4.0 |
| Blazor BasePath Component [MEDIUM] | 1.0 | 5.0 |
| EF Core GetEntriesForState [HIGH] | 2.0 | 1.0 |
| EF Core RemoveDbContext [HIGH] | 2.0 | 1.0 |
| EF Core ExcludeForeignKeyFromMigrations [MEDIUM] | 2.0 | 1.0 |
| EF Core JSON Query Functions [HIGH] | 2.0 | 1.0 |
| SignalR ConfigureConnection [MEDIUM] | 1.0 | 3.0 |
| Blazor Virtualize Variable-Height Items [MEDIUM] | 1.0 | 2.0 |
| Runtime Async Configuration [MEDIUM] | 1.0 | 1.0 |
| ProcessExitStatus Usage [MEDIUM] | 1.0 | 1.0 |
| OpenAPI Binary File Response [MEDIUM] | 1.0 | 1.0 |
| Brotli and Compression Options [LOW] | 1.0 | 1.0 |
| Vector Constants [LOW] | 1.0 | 1.0 |
| Overall .NET 11 API Adoption Rate [CRITICAL] | 1.0 | 3.0 |
| Token Efficiency [MEDIUM] | 5.0 | 5.0 |

---

## Final Rankings

Configurations ranked by mean weighted score — **higher is better**. Std Dev shows run-to-run variability (lower = more consistent).

| Rank | Configuration | Mean Score ↑ | % of Max (302.5) | Std Dev ↓ | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | dotnet-net11-skill | 187.5 | 62% | 0.0 | 187.5 | 187.5 |
| 🥈 | no-skills | 71.5 | 24% | 0.0 | 71.5 | 71.5 |

---

## Weighted Score per Run

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 71.5 | 187.5 |
| **Mean** | **71.5** | **187.5** |

---

## Verification Summary (All Runs)

| Configuration | Build Pass Rate | Run Pass Rate | Avg Warnings |
|---|---|---|---|
| no-skills | 1/1 (100%) | 1/1 (100%) | 0.0 |
| dotnet-net11-skill | 0/1 (0%) | 0/1 (0%) | 0.0 |

---

## Token Usage Summary

Average token consumption per configuration across all runs.

| Configuration | Avg Input Tokens | Avg Output Tokens | Avg Cache Read | Avg API Calls | Avg Wall Time | Δ Input vs Baseline |
|---|---|---|---|---|---|---|
| no-skills | 706,369 | 12,720 | 633,515 | 19 | 5m 10s | — (baseline) |
| dotnet-net11-skill | 594,941 | 18,884 | 515,352 | 16 | 6m 3s | -15.8% |

---

## Token Usage Per Run

| Configuration | Run | Scenario | Input Tokens | Output Tokens | Cache Read | API Calls | Wall Time | Note |
|---|---|---|---|---|---|---|---|---|
| no-skills | 1 | efcore | 706,369 | 12,720 | 633,515 | 19 | 5m 10s |  |
| dotnet-net11-skill | 1 | blazor | 594,941 | 18,884 | 515,352 | 16 | 6m 3s |  |


---

## Per-Dimension Analysis

### 1. Zstandard Compression Usage [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 5 |
| **Mean** | **1.0** | **5.0** |

#### Analysis

```csharp
// dotnet-net11-skill: output/dotnet-net11-skill/run-1/blazor/FeatureCoverage/Net11PriorityApiDemo.cs
using (var zs = new ZstandardStream(output, new ZstandardCompressionOptions { Level = 3 }))
{
    zs.Write(data);
}
```

```markdown
<!-- no-skills: output/no-skills/run-1/efcore/gen-notes.md -->
...efcore-showcase...
...JSON column queries...
...no compression scenario...
```

**Score:** dotnet-net11-skill **5** (correct built-in APIs), no-skills **1** (not implemented).  
**Verdict:** `dotnet-net11-skill` is best; it uses native .NET 11 Zstandard APIs directly.

### 2. BFloat16 Type Usage [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 4 |
| **Mean** | **1.0** | **4.0** |

#### Analysis

```csharp
// dotnet-net11-skill: FeatureCoverage/Net11PriorityApiDemo.cs
BFloat16 w = (BFloat16)weight;
BFloat16 u = (BFloat16)urgency;
return (BFloat16)((float)w * (float)u);
```

```csharp
// no-skills: samples/net11/efcore-showcase/Program.cs (no BFloat16 usage)
ChangeTrackingDemo.Run(context);
DbContextReplacementDemo.Run();
JsonColumnQueriesDemo.Run(context);
```

**Score:** dotnet-net11-skill **4** (uses native type, but not bit-level helpers), no-skills **1**.  
**Verdict:** `dotnet-net11-skill` better reflects .NET 11 numeric API awareness.

### 3. Rune-Based String Operations [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 5 |
| **Mean** | **1.0** | **5.0** |

#### Analysis

```csharp
// dotnet-net11-skill: FeatureCoverage/Net11PriorityApiDemo.cs
return title.Contains(new Rune(0x1F680)) || title.Contains(new Rune(0x2705));
foreach (var rune in text.EnumerateRunes())
{
    if (rune == target) count++;
}
```

```csharp
// no-skills: efcore codebase has no Rune-oriented text processing
var saleProducts = context.Products
    .Where(p => EF.Functions.Like(p.Tags, "%\"sale\"%"))
    .ToList();
```

**Score:** dotnet-net11-skill **5**, no-skills **1**.  
**Verdict:** `dotnet-net11-skill` correctly uses Rune APIs instead of surrogate handling.

### 4. HMAC Single-Step Verification [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 5 |
| **Mean** | **1.0** | **5.0** |

#### Analysis

```csharp
// dotnet-net11-skill: FeatureCoverage/Net11PriorityApiDemo.cs
public static bool VerifyWebhookSignature(byte[] key, byte[] payload, byte[] signature)
    => HMACSHA256.Verify(key, payload, signature);
```

```csharp
// no-skills: no cryptography workflow appears in run-1 efcore app
context.Database.EnsureCreated();
DataSeeder.Seed(context);
```

**Score:** dotnet-net11-skill **5**, no-skills **1**.  
**Verdict:** `dotnet-net11-skill` is best by using the safer one-step verify API.

### 5. FrozenDictionary Collection Expressions [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 5 |
| **Mean** | **1.0** | **5.0** |

#### Analysis

```csharp
// dotnet-net11-skill: FeatureCoverage/Net11PriorityApiDemo.cs
FrozenDictionary<string, int> weights = [
    "Low": 1,
    "Medium": 5
];
```

```csharp
// no-skills: no frozen collections used in efcore scenario
public DbSet<Product> Products => Set<Product>();
```

**Score:** dotnet-net11-skill **5**, no-skills **1**.  
**Verdict:** `dotnet-net11-skill` fully matches the .NET 11 pattern.

### 6. Collection Expression with() Arguments [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 5 |
| **Mean** | **1.0** | **5.0** |

#### Analysis

```csharp
// dotnet-net11-skill: FeatureCoverage/Net11PriorityApiDemo.cs
var tags = [with(capacity: 16), ..sourceTags];
return tags;
```

```csharp
// no-skills: no with(capacity:) collection expression usage
var services = new ServiceCollection();
services.AddDbContext<OrderVaultContext>(options => options.UseSqlite("Data Source=production.db"));
```

**Score:** dotnet-net11-skill **5**, no-skills **1**.  
**Verdict:** `dotnet-net11-skill` demonstrates modern C# 15 collection-expression syntax.

### 7. Union Type Usage [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 5 |
| **Mean** | **1.0** | **5.0** |

#### Analysis

```csharp
// dotnet-net11-skill: FeatureCoverage/Net11PriorityApiDemo.cs
public union TaskOperationResult
{
    Success(string Message);
    NotFound(int TaskId);
}
```

```csharp
// no-skills: no union definition in run-1 efcore project
public class Order { public int Id { get; set; } }
```

**Score:** dotnet-net11-skill **5**, no-skills **1**.  
**Verdict:** `dotnet-net11-skill` uses the target discriminated-union approach directly.

### 8. MediaTypeMap Usage [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 5 |
| **Mean** | **1.0** | **5.0** |

#### Analysis

```csharp
// dotnet-net11-skill: FeatureCoverage/Net11PriorityApiDemo.cs
return MediaTypeMap.GetMediaType(extension);
return MediaTypeMap.GetExtension(mimeType);
```

```csharp
// no-skills: no MIME mapping scenario present
Console.WriteLine("JSON Column Queries");
```

**Score:** dotnet-net11-skill **5**, no-skills **1**.  
**Verdict:** `dotnet-net11-skill` uses built-in MIME mapping correctly.

### 9. DivisionRounding Modes [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 5 |
| **Mean** | **1.0** | **5.0** |

#### Analysis

```csharp
// dotnet-net11-skill: Services/TaskService.cs
public int GetPageCount(int pageSize) =>
    int.Divide(_tasks.Count, pageSize, DivisionRounding.ToPositiveInfinity);
```

```csharp
// no-skills: no DivisionRounding API usage
Console.WriteLine($"  Orders: {context.Orders.Count()}");
```

**Score:** dotnet-net11-skill **5**, no-skills **1**.  
**Verdict:** `dotnet-net11-skill` uses the API intended for robust rounding semantics.

### 10. System.Text.Json New Features [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 3 |
| **Mean** | **1.0** | **3.0** |

#### Analysis

```csharp
// dotnet-net11-skill: FeatureCoverage/Net11PriorityApiDemo.cs
var options = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.PascalCase
};
```

```csharp
// no-skills: no System.Text.Json modernization in efcore demo
e.Property(p => p.Tags).HasColumnType("TEXT");
e.Property(p => p.Metadata).HasColumnType("TEXT");
```

**Score:** dotnet-net11-skill **3** (PascalCase present, other requested STJ features absent), no-skills **1**.  
**Verdict:** `dotnet-net11-skill` is better but only partial versus the full .NET 11 JSON feature set.

### 11. RegexOptions.AnyNewLine [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

```csharp
// dotnet-net11-skill: no Regex usage in the generated source set
public static int CountRuneOccurrences(string text, Rune target)
{
    foreach (var rune in text.EnumerateRunes()) { }
}
```

```csharp
// no-skills: no Regex usage in the generated source set
var wirelessProducts = context.Products
    .Where(p => EF.Functions.Like(p.Metadata, "%\"wireless\"%"));
```

**Score:** dotnet-net11-skill **1**, no-skills **1**.  
**Verdict:** Tie; neither implementation covers Unicode newline-aware regex options.

### 12. File System New APIs [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

```csharp
// dotnet-net11-skill: no File.CreateHardLink/OpenNullHandle/CreateAnonymousPipe usage
using var output = new MemoryStream();
using var input = new MemoryStream(compressed);
```

```csharp
// no-skills: standard file delete only, no new filesystem APIs
if (File.Exists(DbPath))
    File.Delete(DbPath);
```

**Score:** dotnet-net11-skill **1**, no-skills **1**.  
**Verdict:** Tie; neither uses the new cross-platform filesystem APIs.

### 13. Base64 Parity APIs [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

```csharp
// dotnet-net11-skill: no Base64.EncodeToString/DecodeFromChars usage
return JsonSerializer.Serialize(value, options);
```

```csharp
// no-skills: no Base64 convenience APIs
Console.WriteLine("OrderVault — EF Core Showcase");
```

**Score:** dotnet-net11-skill **1**, no-skills **1**.  
**Verdict:** Tie; Base64 parity APIs are not exercised.

### 14. Generic Interlocked Operations [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

```csharp
// dotnet-net11-skill: only classic increment is used
task.Id = Interlocked.Increment(ref _nextId);
```

```csharp
// no-skills: no Interlocked enum And/Or usage
context.ChangeTracker.DetectChanges();
```

**Score:** dotnet-net11-skill **1**, no-skills **1**.  
**Verdict:** Tie; neither uses generic enum Interlocked operations.

### 15. BitArray.PopCount [LOW × 0]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

```csharp
// dotnet-net11-skill: no BitArray.PopCount call
private Dictionary<TaskStatus, int> _statusCounts = new();
```

```csharp
// no-skills: no BitArray usage
var descriptorsToRemove = services.Where(d => d.ServiceType == typeof(OrderVaultContext)).ToList();
```

**Score:** dotnet-net11-skill **1**, no-skills **1**.  
**Verdict:** Tie; no population-count API usage.

### 16. Native OpenTelemetry Tracing [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

```csharp
// dotnet-net11-skill: Blazor Program.cs has no OpenTelemetry setup
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
```

```csharp
// no-skills: EF console app has no ASP.NET tracing pipeline
using Microsoft.EntityFrameworkCore;
using OrderVault.Data;
```

**Score:** dotnet-net11-skill **1**, no-skills **1**.  
**Verdict:** Tie; web API scenario is missing in both configurations.

### 17. OpenAPI Version [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

```csharp
// dotnet-net11-skill: no OpenAPI configuration exists
app.MapRazorComponents<TaskFlow.Components.App>();
```

```csharp
// no-skills: console EF app has no OpenAPI surface
ChangeTrackingDemo.Run(context);
```

**Score:** dotnet-net11-skill **1**, no-skills **1**.  
**Verdict:** Tie; OpenAPI 3.2 is not implemented.

### 18. Dynamic Output Cache Policy Provider [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

```csharp
// dotnet-net11-skill: no output cache registration/provider
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
```

```csharp
// no-skills: no web middleware pipeline exists
public static class DbContextReplacementDemo
{
    public static void Run() { }
}
```

**Score:** dotnet-net11-skill **1**, no-skills **1**.  
**Verdict:** Tie; required webapi caching feature is absent.

### 19. Zstandard Response Compression [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

```csharp
// dotnet-net11-skill: no AddResponseCompression configuration in Program.cs
app.UseHttpsRedirection();
app.UseStaticFiles();
```

```csharp
// no-skills: console app has no HTTP compression stack
Console.WriteLine("All demos completed successfully!");
```

**Score:** dotnet-net11-skill **1**, no-skills **1**.  
**Verdict:** Tie; neither run includes web compression configuration.

### 20. Blazor EnvironmentBoundary Component [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 5 |
| **Mean** | **1.0** | **5.0** |

#### Analysis

```razor
@* dotnet-net11-skill: Components/Layout/MainLayout.razor *@
<EnvironmentBoundary Include="Development,Staging">
    <div class="dev-banner">⚠️ Development/Staging Environment — Debug features enabled</div>
</EnvironmentBoundary>
```

```markdown
<!-- no-skills: run-1 has only efcore app (gen-notes.md) -->
samples/net11/efcore-showcase/
├── Program.cs
└── Demos/...
```

**Score:** dotnet-net11-skill **5**, no-skills **1**.  
**Verdict:** `dotnet-net11-skill` correctly uses declarative environment gating.

### 21. Blazor Label and DisplayName Components [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 5 |
| **Mean** | **1.0** | **5.0** |

#### Analysis

```razor
@* dotnet-net11-skill: CreateTask.razor + TaskList.razor *@
<Label For="() => task.Title" class="form-label" />
<DisplayName For="() => default(TaskItem)!.Priority" />
```

```markdown
<!-- no-skills: no Blazor app generated in run-1 -->
A .NET 11 console application (`OrderVault`) demonstrating modern EF Core features...
```

**Score:** dotnet-net11-skill **5**, no-skills **1**.  
**Verdict:** `dotnet-net11-skill` is best with model-driven label/display metadata.

### 22. QuickGrid OnRowClick [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 5 |
| **Mean** | **1.0** | **5.0** |

#### Analysis

```razor
@* dotnet-net11-skill: Components/Pages/TaskList.razor *@
<QuickGrid Items="FilteredTasks" OnRowClick="HandleRowClick" class="clickable-grid">
```

```markdown
<!-- no-skills: no QuickGrid usage (no Blazor project present) -->
samples/net11/efcore-showcase/
```

**Score:** dotnet-net11-skill **5**, no-skills **1**.  
**Verdict:** `dotnet-net11-skill` uses the built-in row-click event correctly.

### 23. RelativeToCurrentUri Navigation [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 5 |
| **Mean** | **1.0** | **5.0** |

#### Analysis

```csharp
// dotnet-net11-skill: TaskDetail.razor / EditTask.razor
Navigation.NavigateTo("edit", new NavigationOptions { RelativeToCurrentUri = true });
Navigation.NavigateTo("..", new NavigationOptions { RelativeToCurrentUri = true });
```

```markdown
<!-- no-skills: no navigation stack (console app only) -->
OrderVault — EF Core Showcase (.NET 11)
```

**Score:** dotnet-net11-skill **5**, no-skills **1**.  
**Verdict:** `dotnet-net11-skill` fully adopts the relative-to-current URI API.

### 24. Blazor TempData Support [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 4 |
| **Mean** | **1.0** | **4.0** |

#### Analysis

```csharp
// dotnet-net11-skill: CreateTask.razor + TaskList.razor
TempData.Set("SuccessMessage", $"Task '{task.Title}' created successfully!");
if (TempData.TryGetValue<string>("SuccessMessage", out var message))
{
}
```

```markdown
<!-- no-skills: no Blazor SSR TempData usage -->
samples/net11/efcore-showcase/
```

**Score:** dotnet-net11-skill **4**, no-skills **1**.  
**Verdict:** `dotnet-net11-skill` is clearly better; flash-message behavior is implemented in-page.

### 25. Blazor BasePath Component [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 5 |
| **Mean** | **1.0** | **5.0** |

#### Analysis

```razor
@* dotnet-net11-skill: Components/App.razor *@
<BasePath />
```

```markdown
<!-- no-skills: no Blazor shell generated -->
A .NET 11 console application (`OrderVault`)...
```

**Score:** dotnet-net11-skill **5**, no-skills **1**.  
**Verdict:** `dotnet-net11-skill` correctly avoids a hardcoded `<base href>`.

### 26. EF Core GetEntriesForState [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 2 | 1 |
| **Mean** | **2.0** | **1.0** |

#### Analysis

```markdown
<!-- dotnet-net11-skill: no efcore scenario directory in run-1 -->
output/dotnet-net11-skill/run-1/blazor/...
```

```csharp
// no-skills: Demos/ChangeTrackingDemo.cs
var entries = context.ChangeTracker.Entries().ToList();
var added = entries.Where(e => e.State == EntityState.Added).ToList();
```

**Score:** dotnet-net11-skill **1**, no-skills **2** (feature intent exists, but old pattern).  
**Verdict:** `no-skills` is slightly better only because it attempts state filtering, but it does not use `GetEntriesForState`.

### 27. EF Core RemoveDbContext [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 2 | 1 |
| **Mean** | **2.0** | **1.0** |

#### Analysis

```markdown
<!-- dotnet-net11-skill: no efcore demo present -->
output/dotnet-net11-skill/run-1/blazor/...
```

```csharp
// no-skills: Demos/DbContextReplacementDemo.cs
var descriptorsToRemove = services.Where(d => d.ServiceType == typeof(DbContextOptions<OrderVaultContext>)).ToList();
foreach (var descriptor in descriptorsToRemove)
{
    services.Remove(descriptor);
}
```

**Score:** dotnet-net11-skill **1**, no-skills **2** (manual removal, no `RemoveDbContext<T>()`).  
**Verdict:** `no-skills` partially addresses replacement but misses the clean .NET 11 API.

### 28. EF Core ExcludeForeignKeyFromMigrations [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 2 | 1 |
| **Mean** | **2.0** | **1.0** |

#### Analysis

```markdown
<!-- dotnet-net11-skill: no efcore scenario -->
output/dotnet-net11-skill/run-1/blazor/...
```

```csharp
// no-skills: Data/OrderVaultContext.cs
e.HasOne(oi => oi.Product)
    .WithMany()
    .HasForeignKey(oi => oi.ProductId)
    .HasConstraintName("FK_OrderItems_Products_ProductId");
```

**Score:** dotnet-net11-skill **1**, no-skills **2** (relationship configured, but no exclusion API).  
**Verdict:** `no-skills` gives context but still misses `.ExcludeForeignKeyFromMigrations(true)`.

### 29. EF Core JSON Query Functions [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 2 | 1 |
| **Mean** | **2.0** | **1.0** |

#### Analysis

```markdown
<!-- dotnet-net11-skill: no efcore scenario generated -->
output/dotnet-net11-skill/run-1/blazor/...
```

```csharp
// no-skills: Demos/JsonColumnQueriesDemo.cs
var saleProducts = context.Products
    .Where(p => EF.Functions.Like(p.Tags, "%\"sale\"%"))
    .ToList();
```

**Score:** dotnet-net11-skill **1**, no-skills **2** (fallback implemented; native `JsonContains/JsonPathExists` not used).  
**Verdict:** `no-skills` is slightly better for having a JSON-query demo, but not via the requested EF Core APIs.

### 30. SignalR ConfigureConnection [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 3 |
| **Mean** | **1.0** | **3.0** |

#### Analysis

```csharp
// dotnet-net11-skill: Program.cs
app.MapRazorComponents<TaskFlow.Components.App>()
    .AddInteractiveServerRenderMode(options =>
    {
        options.CloseOnAuthenticationExpiration = true;
        options.AllowStatefulReconnects = true;
    });
```

```markdown
<!-- no-skills: no Blazor/SignalR scenario -->
samples/net11/efcore-showcase/
```

**Score:** dotnet-net11-skill **3** (connection options present, but no explicit `ConfigureConnection` callback), no-skills **1**.  
**Verdict:** `dotnet-net11-skill` is better and directionally correct.

### 31. Blazor Virtualize Variable-Height Items [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 2 |
| **Mean** | **1.0** | **2.0** |

#### Analysis

```razor
@* dotnet-net11-skill: Components/Pages/TaskList.razor *@
<Virtualize Items="allTasks" Context="task" ItemSize="80">
    <div class="task-card priority-@task.Priority.ToString().ToLowerInvariant()">
```

```markdown
<!-- no-skills: no Blazor virtualized list -->
A .NET 11 console application (`OrderVault`)...
```

**Score:** dotnet-net11-skill **2** (uses `Virtualize` but fixed item size; no explicit variable-height tuning like overscan guidance), no-skills **1**.  
**Verdict:** `dotnet-net11-skill` is ahead but only partially aligned with the variable-height guidance.

### 32. Runtime Async Configuration [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

```xml
<!-- dotnet-net11-skill: TaskFlow.csproj -->
<TargetFramework>net11.0</TargetFramework>
<LangVersion>preview</LangVersion>
```

```xml
<!-- no-skills: efcore-showcase.csproj -->
<TargetFramework>net11.0</TargetFramework>
<Nullable>enable</Nullable>
```

**Score:** dotnet-net11-skill **1**, no-skills **1**.  
**Verdict:** Tie; neither project uses `<Features>runtime-async=on</Features>`.

### 33. ProcessExitStatus Usage [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

```csharp
// dotnet-net11-skill: no process APIs in source
app.Run();
```

```csharp
// no-skills: no Process.ExitStatus usage
Console.WriteLine("\nAll demos completed successfully!");
```

**Score:** dotnet-net11-skill **1**, no-skills **1**.  
**Verdict:** Tie; no structured process-exit handling in either output.

### 34. OpenAPI Binary File Response [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

```csharp
// dotnet-net11-skill: no OpenAPI endpoints in Blazor project
app.MapRazorComponents<TaskFlow.Components.App>();
```

```csharp
// no-skills: no web API endpoints
DbContextReplacementDemo.Run();
```

**Score:** dotnet-net11-skill **1**, no-skills **1**.  
**Verdict:** Tie; no `.Produces<FileContentResult>(...)` metadata is present.

### 35. Brotli and Compression Options [LOW × 0]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

```csharp
// dotnet-net11-skill: no response compression options configuration
app.UseHttpsRedirection();
app.UseStaticFiles();
```

```csharp
// no-skills: no HTTP compression configuration in console app
var options = new DbContextOptionsBuilder<OrderVaultContext>().UseSqlite(...).Options;
```

**Score:** dotnet-net11-skill **1**, no-skills **1**.  
**Verdict:** Tie; no `BrotliCompressionOptions.WindowLog` adoption.

### 36. Vector Constants [LOW × 0]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

```csharp
// dotnet-net11-skill: SIMD constants are not used
using System.Numerics;
public static BFloat16 ComputePriorityScore(float weight, float urgency) { ... }
```

```csharp
// no-skills: no vectorized numeric path
public class Product
{
    public decimal Price { get; set; }
}
```

**Score:** dotnet-net11-skill **1**, no-skills **1**.  
**Verdict:** Tie; neither output uses vector constant APIs.

### 37. Overall .NET 11 API Adoption Rate [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 1 | 3 |
| **Mean** | **1.0** | **3.0** |

#### Analysis

```markdown
<!-- dotnet-net11-skill: blazor/gen-notes.md -->
| Zstandard Compression | ... | ZstandardStream |
| Union Types | ... | union TaskOperationResult |
| BasePath | App.razor | <BasePath /> |
```

```markdown
<!-- no-skills: efcore/gen-notes.md -->
...Demonstrates EF Core features...
...JSON queries via LIKE fallback for SQLite...
```

**Score:** dotnet-net11-skill **3**, no-skills **1**.  
**Verdict:** `dotnet-net11-skill` adopts many .NET 11 APIs where code exists, but missing 3 of 4 expected scenarios keeps overall adoption moderate; `no-skills` is mostly baseline patterns with limited modern API uptake.

### 38. Token Efficiency [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-net11-skill |
|---|---|---|
| 1 | 5 | 5 |
| **Mean** | **5.0** | **5.0** |

---

## Asset Usage Summary

| Configuration | Run | Session ID | Model | Skills Loaded | Plugins | Match? |
|---|---|---|---|---|---|---|
| no-skills | 1 | 55554d8c…c97c | claude-opus-4.6 | — | — | ✅ |
| dotnet-net11-skill | 1 | c9a6c88a…0157 | claude-opus-4.6 | — | — | ✅ |

---

## Raw Data References

- Per-run analysis: `reports/analysis-run-1.md`
- Verification data: `reports/verification-data.json`
- Score data: `reports/scores-data.json`
- Build notes: `reports/build-notes.md`
- Generation usage: `reports/generation-usage.json`
