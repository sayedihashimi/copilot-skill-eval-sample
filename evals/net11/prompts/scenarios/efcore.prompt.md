---
description: "Create a .NET 11 EF Core project — an order management system demonstrating modern change tracking, context management, and JSON queries"
tools: ["search/changes", "search/codebase", "web/fetch", "read/problems", "read/terminalLastCommand"]
---

# OrderVault — A .NET 11 EF Core Order Management Demo

## Product Overview

**OrderVault** is a console application that models a simple order management system using Entity Framework Core on .NET 11. It sets up a realistic domain (Customers, Products, Orders, OrderItems), seeds sample data into an SQLite database, and runs a series of demos that exercise the latest EF Core capabilities. Each demo is self-contained: it performs operations, prints results, and verifies behavior.

## Technical Requirements

- **Framework**: .NET 11
- **Project Type**: Console application (`dotnet new console`)
- **Project Location**: `./samples/net11/efcore-showcase/`
- **Database**: SQLite via EF Core
- **NuGet Packages**: EF Core SQLite provider and design-time tools
- **Schema**: Code-first using `EnsureCreated()` (no migrations needed for this demo)
- **Code Organization**: Each demo in its own file under `Demos/`. `Program.cs` calls each demo sequentially.
- **Use the latest C# language version** and latest .NET APIs throughout. Prefer new, idiomatic approaches over legacy patterns wherever available.

## Domain Model

### Customer
- `Id` (int) — PK, auto-generated
- `Name` (string) — required
- `Email` (string) — required
- `Tier` (string) — e.g., "Gold", "Silver", "Bronze"
- `CreatedAt` (DateTime) — auto-set

### Product
- `Id` (int) — PK, auto-generated
- `Name` (string) — required
- `Price` (decimal) — required
- `Tags` (string) — a JSON column storing an array of tag strings (e.g., `["electronics", "sale"]`)
- `Metadata` (string) — a JSON column storing arbitrary key-value data

### Order
- `Id` (int) — PK, auto-generated
- `CustomerId` (int) — FK → Customer
- `OrderDate` (DateTime) — required
- `Status` (string) — e.g., "Pending", "Shipped", "Delivered", "Cancelled"
- `TotalAmount` (decimal) — required
- `Notes` (string?) — optional

### OrderItem
- `Id` (int) — PK, auto-generated
- `OrderId` (int) — FK → Order
- `ProductId` (int) — FK → Product
- `Quantity` (int) — required
- `UnitPrice` (decimal) — required

### Relationships
- Customer → Orders (one-to-many)
- Order → OrderItems (one-to-many)
- OrderItem → Product (many-to-one)

Configure using Fluent API.

## Feature Requirements

### 1. Efficient Change Tracking by State

Demonstrate a way to query tracked entities by their state (added, modified, deleted, unchanged) **without** triggering an automatic change detection pass. This is important for performance in long-lived contexts that track many entities. Compare this with the traditional approach that forces change detection on every call. Load several entities, modify some, delete some, and then query for just the modified ones efficiently. Print which entities are in each state.

### 2. Clean DbContext Replacement for Testing

Demonstrate how to cleanly remove an existing DbContext registration from the DI container and replace it with a different configuration. This is a common need in integration tests where you want to swap the production database provider with a test database. Show:
- Registering a context with one configuration
- Removing that registration cleanly (not just overriding/conflicting)
- Re-registering with a different configuration
- Resolving and verifying the new configuration is active

Also demonstrate registering a pooled context factory without needing to pass configuration inline (when configuration is provided elsewhere).

### 3. Excluding Foreign Keys from Migrations

Demonstrate how to configure a relationship so that its foreign key constraint is excluded from generated migrations. This is useful for application-enforced relationships or legacy schemas where you don't want EF to create the FK constraint in the database. Apply this to the OrderItem → Product relationship and show the Fluent API configuration.

### 4. JSON Column Queries

Demonstrate querying JSON data stored in columns using built-in EF Core functions:
- Check whether a JSON column contains a specific value
- Check whether a specific JSON path exists in a JSON column
- Note: These functions are designed for SQL Server. If running on SQLite, demonstrate the configuration and provide a working fallback (e.g., string-based `LIKE` queries) so the demo actually executes. Explain the SQL Server requirement in output.

## Demo Flow

1. Create a fresh SQLite database on each run (delete + recreate)
2. Seed sample data: at least 5 customers, 10 products with varied JSON tags/metadata, 15 orders with order items
3. Run each demo with clear output sections
4. Delete the database file on exit

## Output Format

Each demo section should print:
```
══════════════════════════════════════════
  [Section Name]
══════════════════════════════════════════
[output]

```

## Build & Run

After creating the project:
1. Run `dotnet build` — must compile with zero errors
2. Run `dotnet run` — all demos must execute and produce expected output
3. Fix any issues before considering the task complete
