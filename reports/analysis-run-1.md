# Comparative Analysis: dotnet-webapi-skill, no-skills

This run contains **2 configurations** under `output/`: `dotnet-webapi-skill` and `no-skills`, each with the same scenario at `run-1/create-web-api/`. No `gen-notes.md` files were found, so configuration identity was inferred from directory names: `dotnet-webapi-skill` = skill-enabled generation, `no-skills` = baseline Copilot. Both outputs target `.NET 10`, but both are functionally incomplete for the requested contact-manager API.

## Executive Summary

| Dimension [Tier] | dotnet-webapi-skill | no-skills |
|---|---:|---:|
| Code Organization [MEDIUM] | 2 | 3 |
| Error Handling [MEDIUM] | 1 | 1 |
| Type Safety [MEDIUM] | 4 | 3 |
| Input Validation [MEDIUM] | 3 | 2 |
| Testing Support [MEDIUM] | 1 | 1 |
| Security Vulnerability Scan [CRITICAL] | 3 | 3 |
| Input Validation Coverage [CRITICAL] | 2 | 1 |
| Endpoint Completeness [CRITICAL] | 1 | 1 |
| Business Rule Implementation [CRITICAL] | 2 | 1 |
| Error Response Conformance [HIGH] | 1 | 1 |
| API Style [HIGH] | 4 | 2 |
| CancellationToken Propagation [CRITICAL] | 1 | 1 |
| Sealed Types [MEDIUM] | 5 | 2 |
| Documentation & API Discoverability [LOW] | 4 | 2 |

## 1. Code Organization [MEDIUM]

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

## 2. Error Handling [MEDIUM]

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

## 3. Type Safety [MEDIUM]

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

## 4. Input Validation [MEDIUM]

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

## 5. Testing Support [MEDIUM]

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

## 6. Security Vulnerability Scan [CRITICAL]

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

## 7. Input Validation Coverage [CRITICAL]

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

## 8. Endpoint Completeness [CRITICAL]

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

## 9. Business Rule Implementation [CRITICAL]

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

## 10. Error Response Conformance [HIGH]

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

## 11. API Style [HIGH]

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

## 12. CancellationToken Propagation [CRITICAL]

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

## 13. Sealed Types [MEDIUM]

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

## 14. Documentation & API Discoverability [LOW]

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

## Weighted Summary

Weights applied: `CRITICAL ×3`, `HIGH ×2`, `MEDIUM ×1`, `LOW ×0.5`.

| Configuration | Weighted Total |
|---|---:|
| dotnet-webapi-skill | **55.0** |
| no-skills | **40.0** |

## What All Versions Get Right

- Target modern runtime and language defaults (`net10.0`, nullable enabled).
- Define core contact-domain entities and DTOs (contacts, phones, emails, addresses, tags, notes).
- Include OpenAPI setup and SQLite EF Core dependency references.
- Avoid obvious high-risk anti-patterns like direct concatenated SQL in the inspected code.

## Summary: Impact of Skills

Most impactful differences were: **(1)** stronger DTO-level validation and type rigor (`required`, annotations), **(2)** sealed DTO usage, and **(3)** better API documentation readiness in `dotnet-webapi-skill`.  
Overall, `dotnet-webapi-skill` scores higher on code quality signals, but **both configurations fail the core scenario** because contact-management endpoints and full business workflows are not implemented in `run-1/create-web-api`.
