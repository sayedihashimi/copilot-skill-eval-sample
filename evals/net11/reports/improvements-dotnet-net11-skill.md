# Improvement Suggestions: dotnet-net11-skill

## Executive Summary
`dotnet-net11-skill` ranks first overall (166.5 vs 144.5 weighted) but underperforms badly on the priority ASP.NET Core/Blazor dimensions: all 10 focus dimensions scored **1.0** (same as baseline). The primary root cause is that the plugin instructions are extremely thin (`SKILL.md` is only “when to use / when not to use”) and do not direct the model to produce Web API or Blazor artifacts with required .NET 11 APIs.

Top opportunities:
1. Replace the minimal skill text with explicit, scenario-aware generation requirements.
2. Add hard requirements and code patterns for the 4 Web API focus dimensions.
3. Add hard requirements and code patterns for the 6 Blazor focus dimensions.
4. Add output/coverage checklist plus compactness rules to improve both adoption coverage and token efficiency.

## Current Performance Snapshot

| Dimension | Tier | Weight | dotnet-net11-skill | no-skills | Gap vs Baseline |
|---|---:|---:|---:|---:|---:|
| Zstandard Compression Usage | CRITICAL | 3.0 | 5.0 | 5.0 | 0.0 |
| BFloat16 Type Usage | HIGH | 2.0 | 4.0 | 4.0 | 0.0 |
| Rune-Based String Operations | HIGH | 2.0 | 5.0 | 4.0 | +1.0 |
| HMAC Single-Step Verification | HIGH | 2.0 | 5.0 | 5.0 | 0.0 |
| FrozenDictionary Collection Expressions | HIGH | 2.0 | 2.0 | 2.0 | 0.0 |
| Collection Expression with() Arguments | HIGH | 2.0 | 5.0 | 2.0 | +3.0 |
| Union Type Usage | CRITICAL | 3.0 | 5.0 | 2.0 | +3.0 |
| MediaTypeMap Usage | MEDIUM | 1.0 | 5.0 | 2.0 | +3.0 |
| DivisionRounding Modes | MEDIUM | 1.0 | 5.0 | 5.0 | 0.0 |
| System.Text.Json New Features | CRITICAL | 3.0 | 5.0 | 4.0 | +1.0 |
| RegexOptions.AnyNewLine | MEDIUM | 1.0 | 5.0 | 5.0 | 0.0 |
| File System New APIs | HIGH | 2.0 | 5.0 | 5.0 | 0.0 |
| Base64 Parity APIs | MEDIUM | 1.0 | 5.0 | 5.0 | 0.0 |
| Generic Interlocked Operations | MEDIUM | 1.0 | 5.0 | 5.0 | 0.0 |
| BitArray.PopCount | LOW | 0.5 | 5.0 | 5.0 | 0.0 |
| Native OpenTelemetry Tracing | HIGH | 2.0 | 1.0 | 1.0 | 0.0 |
| OpenAPI Version | MEDIUM | 1.0 | 1.0 | 1.0 | 0.0 |
| Dynamic Output Cache Policy Provider | HIGH | 2.0 | 1.0 | 1.0 | 0.0 |
| Zstandard Response Compression | HIGH | 2.0 | 1.0 | 1.0 | 0.0 |
| Blazor EnvironmentBoundary Component | HIGH | 2.0 | 1.0 | 1.0 | 0.0 |
| Blazor Label and DisplayName Components | HIGH | 2.0 | 1.0 | 1.0 | 0.0 |
| QuickGrid OnRowClick | HIGH | 2.0 | 1.0 | 1.0 | 0.0 |
| RelativeToCurrentUri Navigation | MEDIUM | 1.0 | 1.0 | 1.0 | 0.0 |
| Blazor TempData Support | HIGH | 2.0 | 1.0 | 1.0 | 0.0 |
| Blazor BasePath Component | MEDIUM | 1.0 | 1.0 | 1.0 | 0.0 |
| EF Core GetEntriesForState | HIGH | 2.0 | 1.0 | 1.0 | 0.0 |
| EF Core RemoveDbContext | HIGH | 2.0 | 1.0 | 1.0 | 0.0 |
| EF Core ExcludeForeignKeyFromMigrations | MEDIUM | 1.0 | 1.0 | 1.0 | 0.0 |
| EF Core JSON Query Functions | HIGH | 2.0 | 1.0 | 1.0 | 0.0 |
| SignalR ConfigureConnection | MEDIUM | 1.0 | 1.0 | 1.0 | 0.0 |
| Blazor Virtualize Variable-Height Items | MEDIUM | 1.0 | 1.0 | 1.0 | 0.0 |
| Runtime Async Configuration | MEDIUM | 1.0 | 1.0 | 1.0 | 0.0 |
| ProcessExitStatus Usage | MEDIUM | 1.0 | 2.0 | 2.0 | 0.0 |
| OpenAPI Binary File Response | MEDIUM | 1.0 | 1.0 | 1.0 | 0.0 |
| Brotli and Compression Options | LOW | 0.5 | 3.0 | 3.0 | 0.0 |
| Vector Constants | LOW | 0.5 | 1.0 | 1.0 | 0.0 |
| Overall .NET 11 API Adoption Rate | CRITICAL | 3.0 | 3.0 | 2.0 | +1.0 |
| Token Efficiency | MEDIUM | 1.0 | 1.0 | 5.0 | -4.0 |

Evidence highlights from `analysis.md`:
- Priority dimensions all scored 1.0 with explicit “no Web API/Blazor infrastructure present”.
- Build quality is good (`build-notes.md`: build/run pass, zero warnings), so the main issue is **coverage/instruction quality**, not compile reliability.

## Improvement Suggestions

### 1. Replace minimal skill text with scenario-aware .NET 11 generation contract
- **Dimensions affected**: Native OpenTelemetry Tracing, OpenAPI Version, Dynamic Output Cache Policy Provider, Zstandard Response Compression, Blazor EnvironmentBoundary Component, Blazor Label and DisplayName Components, QuickGrid OnRowClick, RelativeToCurrentUri Navigation, Blazor TempData Support, Blazor BasePath Component, Overall .NET 11 API Adoption Rate
- **Current score → Target score**: focus dimensions **1.0 → 4.0+**; overall adoption **3.0 → 4.0+**
- **Problem**: Current skill content is too generic to steer model behavior into required API usage surfaces.
- **Root cause**: `SKILL.md` has no concrete API requirements, no scenario routing rules, and no completion checklist.
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-features\SKILL.md`):
  ```md
  ---
  name: net11-features
  description: Guidance on .NET 11 features and APIs.
  ---
  
  # .NET 11 Features
  
  ## When to Use
  
  - When writing code targeting .NET 11
  
  ## When Not to Use
  
  - When targeting .NET 10 or earlier
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-features\SKILL.md`):
  ```md
  ---
  name: net11-features
  description: Prescriptive .NET 11 API adoption guidance across BCL, ASP.NET Core, Blazor, and EF Core.
  ---
  
  # .NET 11 Features
  
  ## When to Use
  - Any project targeting `net11.0`
  - Any task explicitly asking for .NET 11 feature usage/adoption comparisons
  
  ## When Not to Use
  - Projects targeting .NET 10 or earlier
  
  ## Mandatory Generation Contract (net11)
  1. Prefer built-in .NET 11 APIs over manual workarounds or third-party packages.
  2. If the prompt asks for feature-adoption demo/evaluation coverage, include minimal compilable surfaces for:
     - console/BCL features
     - ASP.NET Core Web API features
     - Blazor features
     - EF Core features
  3. For each requested surface, include concrete code that uses the exact .NET 11 APIs listed below (not “TODO” comments).
  4. Do not replace missing framework surfaces with prose; provide runnable code.
  
  ## Required API Patterns (Do/Don't)
  ### ASP.NET Core (required when Web API surface exists)
  - DO: `AddOpenTelemetry().WithTracing(t => t.AddSource("Microsoft.AspNetCore").AddConsoleExporter())`
  - DO: `AddOpenApi(o => o.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_2)`
  - DO: `IOutputCachePolicyProvider` + DI registration (`AddSingleton<IOutputCachePolicyProvider, ...>`)
  - DO: `AddResponseCompression(); AddRequestDecompression();`
  - DO: `Configure<ZstandardCompressionProviderOptions>(o => o.CompressionOptions = new ZstandardCompressionOptions { ... })`
  - DON'T: external AspNetCore instrumentation package for tracing
  
  ### Blazor (required when Blazor surface exists)
  - DO: `<EnvironmentBoundary Include="Development">...</EnvironmentBoundary>`
  - DO: `<Label For="() => model.Name">...</Label>` and `<DisplayName For="() => model.Name" />`
  - DO: `<QuickGrid ... OnRowClick="@HandleRowClick">`
  - DO: `NavigateTo("edit", new NavigationOptions { RelativeToCurrentUri = true })`
  - DO: `[CascadingParameter] ITempData? TempData` with `Get/Peek/Keep`
  - DO: `<BasePath />` in root layout/head for Blazor Web App
  - DON'T: manual environment `if` checks or button-column workaround for row clicks
  
  ## Output Checklist (must satisfy before finalizing)
  - [ ] Uses native .NET 11 APIs for requested feature areas
  - [ ] Includes at least one concrete usage per requested dimension
  - [ ] Avoids legacy/manual patterns where .NET 11 has built-in alternatives
  - [ ] Keeps code concise and compile-ready
  ```

- **Expected impact**: Highest-impact correction; removes primary instruction gap. Estimated weighted gain: **+24 to +40** depending on how many currently-missing HIGH/MEDIUM dimensions become concrete implementations.

### 2. Add explicit Web API feature recipe block (hard requirements + mini skeleton)
- **Dimensions affected**: Native OpenTelemetry Tracing, OpenAPI Version, Dynamic Output Cache Policy Provider, Zstandard Response Compression
- **Current score → Target score**: **1.0 → 4.0–5.0**
- **Problem**: Analysis repeatedly shows no ASP.NET builder/services code in generated outputs for these dimensions.
- **Root cause**: Skill has no copy-pastable Web API template showing required registrations.
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-features\SKILL.md`):
  ```md
  ## When Not to Use
  
  - When targeting .NET 10 or earlier
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-features\SKILL.md`):
  ```md
  ## When Not to Use
  
  - When targeting .NET 10 or earlier
  
  ## Web API Recipe (.NET 11)
  Use this baseline when generating any Web API sample:
  
  ```csharp
  var builder = WebApplication.CreateBuilder(args);
  
  builder.Services.AddOpenTelemetry()
      .WithTracing(t => t
          .AddSource("Microsoft.AspNetCore")
          .AddConsoleExporter());
  
  builder.Services.AddOpenApi(o =>
  {
      o.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_2;
  });
  
  builder.Services.AddOutputCache();
  builder.Services.AddSingleton<IOutputCachePolicyProvider, DemoOutputCachePolicyProvider>();
  
  builder.Services.AddResponseCompression();
  builder.Services.AddRequestDecompression();
  builder.Services.Configure<ZstandardCompressionProviderOptions>(o =>
  {
      o.CompressionOptions = new ZstandardCompressionOptions { Quality = 6, WindowLog = 22 };
  });
  
  var app = builder.Build();
  app.MapOpenApi();
  app.Run();
  ```
  
  Do not use:
  - `OpenTelemetry.Instrumentation.AspNetCore` package
  - static-only `AddOutputCache(options => options.AddPolicy(...))` as the only policy mechanism
  ```

- **Expected impact**: Directly targets 4 currently-failing dimensions; estimated weighted gain **+14 to +20**.

### 3. Add explicit Blazor feature recipe block (components + navigation + TempData)
- **Dimensions affected**: Blazor EnvironmentBoundary Component, Blazor Label and DisplayName Components, QuickGrid OnRowClick, RelativeToCurrentUri Navigation, Blazor TempData Support, Blazor BasePath Component
- **Current score → Target score**: **1.0 → 4.0–5.0**
- **Problem**: Analysis explicitly reports no `.razor` files/QuickGrid/NavigationManager/TempData/BasePath usage.
- **Root cause**: Skill lacks any Blazor-specific implementation examples.
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-features\SKILL.md`):
  ```md
  # .NET 11 Features
  
  ## When to Use
  
  - When writing code targeting .NET 11
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-features\SKILL.md`):
  ```md
  # .NET 11 Features
  
  ## When to Use
  
  - When writing code targeting .NET 11
  
  ## Blazor Recipe (.NET 11 Web App)
  Include the following patterns in Blazor samples:
  
  ```razor
  <!-- In root layout/head -->
  <BasePath />
  
  <EnvironmentBoundary Include="Development">
      <div class="debug-info">Debug tools enabled</div>
  </EnvironmentBoundary>
  
  <EditForm Model="@model">
      <Label For="() => model.Name">
          <InputText @bind-Value="model.Name" />
      </Label>
  </EditForm>
  
  <QuickGrid Items="@items.AsQueryable()" OnRowClick="@HandleRowClick">
      <PropertyColumn Property="x => x.Name" />
      <TemplateColumn Title="Name"><DisplayName For="() => model.Name" /></TemplateColumn>
  </QuickGrid>
  ```
  
  ```csharp
  [CascadingParameter] public ITempData? TempData { get; set; }
  
  void GoToEdit() =>
      Navigation.NavigateTo("edit", new NavigationOptions { RelativeToCurrentUri = true });
  ```
  
  Require `TempData.Get/Peek/Keep` usage for flash-message scenarios.
  ```

- **Expected impact**: Directly targets 6 currently-failing dimensions; estimated weighted gain **+18 to +26**.

### 4. Add concise “scoring-aware” coverage + brevity constraints in plugin metadata
- **Dimensions affected**: Overall .NET 11 API Adoption Rate, Token Efficiency (secondary: all low-coverage dimensions)
- **Current score → Target score**: Adoption **3.0 → 4.0+**; Token Efficiency **1.0 → 2.5–3.5**
- **Problem**: Skill spends many more tokens than baseline (+268% input tokens) while still missing many dimensions.
- **Root cause**: Metadata/description does not emphasize “minimal runnable examples per required feature” and does not constrain verbosity.
- **Suggested changes**:

  **Before** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\plugin.json`):
  ```json
  {
    "name": "dotnet-net11",
    "version": "0.1.0",
    "description": ".NET 11 features and APIs. Provides guidance on new C#, BCL, ASP.NET Core, Blazor, and EF Core capabilities introduced in .NET 11.",
    "skills": ["./skills/"]
  }
  ```

  **After** (`C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\plugin.json`):
  ```json
  {
    "name": "dotnet-net11",
    "version": "0.2.0",
    "description": "Prescriptive .NET 11 adoption plugin. Enforces concrete API usage with concise, compile-ready examples across BCL, ASP.NET Core, Blazor, and EF Core.",
    "skills": ["./skills/"]
  }
  ```

- **Expected impact**: Smaller than SKILL.md rewrite but useful reinforcement; expected weighted gain **+3 to +8** and improved consistency.

## Summary of Recommended Changes

| File | Recommended updates |
|---|---|
| `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\skills\net11-features\SKILL.md` | Replace minimal guidance with prescriptive contract; add Web API recipe; add Blazor recipe; add explicit do/don’t API patterns and completion checklist. |
| `C:\data\mycode\skills\net11-auto01\plugins\dotnet-net11\plugin.json` | Update description/version to reflect prescriptive behavior and concise, coverage-oriented generation goals. |

## Risks and Trade-offs
- Forcing multi-surface coverage can increase output size and may hurt Token Efficiency if unchecked; mitigate with “minimal compilable example” wording and strict checklist.
- Strong prescriptive templates may reduce flexibility for bespoke user requirements; mitigate by scoping requirements to “when feature-adoption/evaluation coverage is requested.”
- Over-emphasis on evaluation dimensions can bias outputs toward demo-style code; keep “compile-ready and concise” as a balancing constraint.
