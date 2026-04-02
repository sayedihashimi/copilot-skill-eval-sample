# Aggregated Analysis: first-try

**Runs:** 1 | **Configurations:** 2 | **Scenarios:** 1 | **Dimensions:** 14
**Date:** 2026-04-02 04:40 UTC

---

## Overview

Evaluate how my custom skills improve code generation

---

## What Was Tested

### Scenarios

Each run generates one of the following application scenarios (randomly selected per run):

| Scenario | Description |
|---|---|
| create-web-api | Create a new .net 10 asp.net core web api that has endpoints to manage a contact list. store the data in a sqlite db. no auth or anything. I need the typical support that a contact manager app requires. |

### Configurations

Each configuration gives Copilot different custom skills or plugins. The **no-skills** baseline uses default Copilot with no custom instructions.

| Configuration | Description | Skills | Plugins |
|---|---|---|---|
| no-skills | Baseline (default Copilot) | — | — |
| dotnet-webapi-skill | dotnet-webapi-skill | dotnet-webapi | — |

### How It Works

1. **Generate** — For each configuration, Copilot CLI (`copilot --yolo`) is given a scenario prompt and generates a complete project from scratch. One scenario is randomly selected per run.
2. **Verify** — Each generated project is built (`dotnet build`), run, format-checked, and scanned for NuGet vulnerabilities.
3. **Analyze** — An AI judge reviews the source code of all configurations side-by-side and scores each across 14 quality dimensions.

Generation model: **—**
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
| CRITICAL | ×3 | 5 |
| HIGH | ×2 | 2 |
| MEDIUM | ×1 | 6 |

**Maximum possible weighted score: 125.0** (all dimensions scoring 5).
Scores shown as **mean ± standard deviation** across runs.

---

## Executive Summary

| Dimension [Tier] | no-skills | dotnet-webapi-skill |
|---|---|---|
| Code Organization [MEDIUM] | 3.0 | 2.0 |
| Error Handling [MEDIUM] | 1.0 | 1.0 |
| Type Safety [MEDIUM] | 3.0 | 4.0 |
| Input Validation [MEDIUM] | 2.0 | 3.0 |
| Testing Support [MEDIUM] | 1.0 | 1.0 |
| Security Vulnerability Scan [CRITICAL] | 3.0 | 3.0 |
| Input Validation Coverage [CRITICAL] | 1.0 | 2.0 |
| Endpoint Completeness [CRITICAL] | 1.0 | 1.0 |
| Business Rule Implementation [CRITICAL] | 1.0 | 2.0 |
| Error Response Conformance [HIGH] | 1.0 | 1.0 |
| API Style [HIGH] | 2.0 | 4.0 |
| CancellationToken Propagation [CRITICAL] | 1.0 | 1.0 |
| Sealed Types [MEDIUM] | 2.0 | 5.0 |
| Documentation & API Discoverability [MEDIUM] | 2.0 | 4.0 |

---

## Final Rankings

| Rank | Configuration | Mean Score | % of Max (125) | Std Dev | Min | Max |
|---|---|---|---|---|---|---|
| 🥇 | dotnet-webapi-skill | 57.0 | 46% | 0.0 | 57.0 | 57.0 |
| 🥈 | no-skills | 41.0 | 33% | 0.0 | 41.0 | 41.0 |

---

## Weighted Score per Run

| Run | no-skills | dotnet-webapi-skill |
|---|---|---|
| 1 | 41.0 | 57.0 |
| **Mean** | **41.0** | **57.0** |

---

## Verification Summary (All Runs)

| Configuration | Build Pass Rate | Run Pass Rate | Avg Warnings |
|---|---|---|---|
| no-skills | 1/1 (100%) | 1/1 (100%) | 0.0 |
| dotnet-webapi-skill | 1/1 (100%) | 1/1 (100%) | 0.0 |

---

## Consistency Analysis

| Configuration | Score σ | Most Consistent Dim (σ) | Most Variable Dim (σ) |
|---|---|---|---|

---

## Per-Dimension Analysis

### 1. Code Organization [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-webapi-skill |
|---|---|---|
| 1 | 3 | 2 |
| **Mean** | **3.0** | **2.0** |

#### Analysis

`dotnet-webapi-skill` has domain/data/DTO files, but endpoint/service folders are empty and `Program.cs` is still template weather code. `no-skills` has a more explicit layered skeleton (`Controllers`, `Services`, `Validators`, `Data`) but those folders are also empty.

```csharp
// dotnet-webapi-skill
// output/dotnet-webapi-skill/run-1/create-web-api/src/Create web api/Program.cs
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
var app = builder.Build();
app.MapGet("/weatherforecast", () => { ... });
app.Run();
```

```csharp
// no-skills
// output/no-skills/run-1/create-web-api/src/Create web api/NexusContacts/Program.cs
builder.Services.AddControllers();
builder.Services.AddOpenApi();
...
app.MapControllers();
```

**Score:** `dotnet-webapi-skill: 2/5`, `no-skills: 3/5`  
**Verdict:** `no-skills` is slightly better organized structurally, but both are incomplete because core behavior is not wired.

### 2. Error Handling [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-webapi-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

Neither configuration defines exception middleware, problem-details mapping, or endpoint-level error handling.

```csharp
// dotnet-webapi-skill Program.cs
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
```

```csharp
// no-skills Program.cs
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
```

**Score:** `dotnet-webapi-skill: 1/5`, `no-skills: 1/5`  
**Verdict:** Tie; both miss baseline API error-handling patterns.

### 3. Type Safety [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-webapi-skill |
|---|---|---|
| 1 | 3 | 4 |
| **Mean** | **3.0** | **4.0** |

#### Analysis

Both enable nullable reference types, but `dotnet-webapi-skill` uses `required` init-only properties and `DateTimeOffset` responses more consistently.

```csharp
// dotnet-webapi-skill DTO
public sealed record CreateContactRequest
{
    [Required, MaxLength(100)]
    public required string FirstName { get; init; }
    [Required, MaxLength(100)]
    public required string LastName { get; init; }
}
```

```csharp
// no-skills DTO
public record CreateContactDto(
    string FirstName,
    string LastName,
    string? Company,
    string? JobTitle
);
```

**Score:** `dotnet-webapi-skill: 4/5`, `no-skills: 3/5`  
**Verdict:** `dotnet-webapi-skill` is stronger on compile-time guarantees.

### 4. Input Validation [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-webapi-skill |
|---|---|---|
| 1 | 2 | 3 |
| **Mean** | **2.0** | **3.0** |

#### Analysis

`dotnet-webapi-skill` adds data annotations directly on request DTOs; `no-skills` mostly leaves DTOs unconstrained.

```csharp
// dotnet-webapi-skill
public sealed record CreateEmailAddressRequest
{
    [Required, MaxLength(254), EmailAddress]
    public required string Email { get; init; }
}
```

```csharp
// no-skills
public record CreateEmailAddressDto(
    string Label,
    string Email,
    bool IsPrimary = false
);
```

**Score:** `dotnet-webapi-skill: 3/5`, `no-skills: 2/5`  
**Verdict:** Skill-enabled output is better prepared for model validation, though not yet connected to real endpoints.

### 5. Testing Support [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-webapi-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

Neither configuration provides tests, interfaces, or implemented service abstractions for isolation.

```csharp
// no-skills Program.cs
builder.Services.AddControllers();
app.MapControllers();
```

```csharp
// dotnet-webapi-skill Program.cs
app.MapGet("/weatherforecast", () => { ... });
```

**Score:** `dotnet-webapi-skill: 1/5`, `no-skills: 1/5`  
**Verdict:** Tie; both lack test-ready architecture in actual code paths.

### 6. Security Vulnerability Scan [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-webapi-skill |
|---|---|---|
| 1 | 3 | 3 |
| **Mean** | **3.0** | **3.0** |

#### Analysis

No obvious SQL injection or hardcoded secrets were found in generated source. Both rely on EF Core package usage and no direct SQL strings in the inspected code.

```xml
<!-- dotnet-webapi-skill csproj -->
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="10.0.0-*" />
```

```xml
<!-- no-skills csproj -->
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="10.*-*" />
```

**Score:** `dotnet-webapi-skill: 3/5`, `no-skills: 3/5`  
**Verdict:** Tie; no critical vulnerability patterns surfaced, but missing full API implementation limits security confidence.

### 7. Input Validation Coverage [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-webapi-skill |
|---|---|---|
| 1 | 1 | 2 |
| **Mean** | **1.0** | **2.0** |

#### Analysis

Coverage is incomplete in both because request entry points are largely missing; however, `dotnet-webapi-skill` has broader DTO annotations prepared for multiple entities.

```csharp
// dotnet-webapi-skill Note DTO
public sealed record CreateNoteRequest
{
    [Required, MinLength(1), MaxLength(2000)]
    public required string Content { get; init; }
}
```

```csharp
// no-skills Note DTO
public record CreateNoteDto(
    string Content
);
```

**Score:** `dotnet-webapi-skill: 2/5`, `no-skills: 1/5`  
**Verdict:** `dotnet-webapi-skill` is better positioned, but neither validates all real input paths in a working API.

### 8. Endpoint Completeness [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-webapi-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

Required contact-manager CRUD endpoints are absent in both outputs.

```csharp
// dotnet-webapi-skill Program.cs
app.MapGet("/weatherforecast", () => { ... });
```

```csharp
// no-skills Program.cs
app.MapControllers();
// (no controller source files present)
```

**Score:** `dotnet-webapi-skill: 1/5`, `no-skills: 1/5`  
**Verdict:** Tie; both fail the core functional requirement.

### 9. Business Rule Implementation [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-webapi-skill |
|---|---|---|
| 1 | 1 | 2 |
| **Mean** | **1.0** | **2.0** |

#### Analysis

`dotnet-webapi-skill` includes stronger persistence-level constraints (unique tag name, relationship/cascade definitions). `no-skills` exposes model classes but lacks implemented data context/rules in source.

```csharp
// dotnet-webapi-skill Data/AppDbContext.cs
modelBuilder.Entity<Tag>(entity =>
{
    entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
    entity.HasIndex(e => e.Name).IsUnique();
});
```

```csharp
// no-skills Models/Contact.cs
[Required, MaxLength(100)]
public string FirstName { get; set; } = string.Empty;
```

**Score:** `dotnet-webapi-skill: 2/5`, `no-skills: 1/5`  
**Verdict:** `dotnet-webapi-skill` is better on domain constraints, but both miss operational business workflows.

### 10. Error Response Conformance [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-webapi-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

Neither output shows RFC 7807/problem-details or standardized error envelope usage.

```csharp
// dotnet-webapi-skill Program.cs
builder.Services.AddOpenApi();
```

```csharp
// no-skills Program.cs
builder.Services.AddControllers();
```

**Score:** `dotnet-webapi-skill: 1/5`, `no-skills: 1/5`  
**Verdict:** Tie; both omit consistent API error contract design.

### 11. API Style [HIGH × 2]

#### Scores Across Runs

| Run | no-skills | dotnet-webapi-skill |
|---|---|---|
| 1 | 2 | 4 |
| **Mean** | **2.0** | **4.0** |

#### Analysis

`dotnet-webapi-skill` uses Minimal API style (`MapGet`) while `no-skills` is configured for MVC controllers.

```csharp
// dotnet-webapi-skill Program.cs
app.MapGet("/weatherforecast", () => { ... });
```

```csharp
// no-skills Program.cs
builder.Services.AddControllers();
app.MapControllers();
```

**Score:** `dotnet-webapi-skill: 4/5`, `no-skills: 2/5`  
**Verdict:** `dotnet-webapi-skill` aligns better with modern .NET defaults, but style advantage does not compensate for missing contact endpoints.

### 12. CancellationToken Propagation [CRITICAL × 3]

#### Scores Across Runs

| Run | no-skills | dotnet-webapi-skill |
|---|---|---|
| 1 | 1 | 1 |
| **Mean** | **1.0** | **1.0** |

#### Analysis

No request handlers or data-access operations currently propagate `CancellationToken` in either output.

```csharp
// dotnet-webapi-skill Program.cs
app.MapGet("/weatherforecast", () => { ... });
```

```csharp
// no-skills Program.cs
app.MapControllers();
```

**Score:** `dotnet-webapi-skill: 1/5`, `no-skills: 1/5`  
**Verdict:** Tie; both miss a production-critical resiliency practice.

### 13. Sealed Types [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-webapi-skill |
|---|---|---|
| 1 | 2 | 5 |
| **Mean** | **2.0** | **5.0** |

#### Analysis

`dotnet-webapi-skill` consistently seals DTO records; `no-skills` uses unsealed records.

```csharp
// dotnet-webapi-skill DTO
public sealed record AddressResponse(
    int Id,
    int ContactId,
    string Label
);
```

```csharp
// no-skills DTO
public record AddressDto(
    int Id,
    string Label,
    string Street
);
```

**Score:** `dotnet-webapi-skill: 5/5`, `no-skills: 2/5`  
**Verdict:** `dotnet-webapi-skill` is clearly better on sealed-type usage and design intent signaling.

### 14. Documentation & API Discoverability [MEDIUM × 1]

#### Scores Across Runs

| Run | no-skills | dotnet-webapi-skill |
|---|---|---|
| 1 | 2 | 4 |
| **Mean** | **2.0** | **4.0** |

#### Analysis

`dotnet-webapi-skill` includes XML comments in DTOs and enables XML doc output; `no-skills` does not.

```xml
<!-- dotnet-webapi-skill csproj -->
<GenerateDocumentationFile>true</GenerateDocumentationFile>
<NoWarn>$(NoWarn);1591</NoWarn>
```

```csharp
// dotnet-webapi-skill DTO
/// <summary>Payload for creating a new contact.</summary>
public sealed record CreateContactRequest { ... }
```

**Score:** `dotnet-webapi-skill: 4/5`, `no-skills: 2/5`  
**Verdict:** `dotnet-webapi-skill` provides better API self-documentation.

---

## Raw Data References

- Per-run analysis: `reports/analysis-run-1.md`
- Verification data: `reports/verification-data.json`
- Score data: `reports/scores-data.json`
- Build notes: `reports/build-notes.md`
- Generation usage: `reports/generation-usage.json`
