---
description: "Create a .NET 11 ASP.NET Core Web API — a product catalog API with compression, caching, observability, and modern serialization"
tools: ["search/changes", "search/codebase", "web/fetch", "read/problems", "read/terminalLastCommand"]
---

# ProductHub — A .NET 11 Product Catalog Web API

## Product Overview

**ProductHub** is a RESTful product catalog API built on .NET 11 using ASP.NET Core Minimal APIs. It allows consumers to browse products, look up categories, resolve MIME types for file uploads, export data, and download catalog files. The API emphasizes modern best practices: built-in observability, efficient compression, dynamic caching, and clean JSON serialization.

## Technical Requirements

- **Framework**: .NET 11
- **Project Type**: ASP.NET Core Web API with Minimal APIs (`dotnet new webapi`)
- **Project Location**: `./samples/net11/webapi-showcase/`
- **Data Store**: In-memory (static collections or `ConcurrentDictionary`) — no database
- **Authentication**: None
- **Dependencies**: No third-party NuGet packages unless a feature specifically requires one
- **Code Organization**: `Models/`, `Services/`, `Endpoints/`, `Middleware/`
- **Use the latest C# language version** and latest .NET APIs throughout. Prefer new, idiomatic approaches over legacy patterns wherever available.

## Domain Model

### Product
- `Id` (int) — auto-generated
- `Name` (string) — required
- `Description` (string?) — optional
- `Price` (decimal) — required, must be > 0
- `Category` (string) — required
- `Tags` — a read-only set of strings for categorization
- `Metadata` (arbitrary JSON) — optional extensible metadata
- `CreatedAt` (DateTimeOffset) — auto-set on creation
- `UpdatedAt` (DateTimeOffset?) — set on update, null until first update

### Category
- `Name` (string) — required
- `Description` (string?) — optional

Implement full CRUD for Products and read-only listing for Categories.

## API Endpoints

| Method | Path | Description |
|--------|------|-------------|
| GET | /api/products | List all products, optionally filtered by category |
| GET | /api/products/{id} | Get a single product by ID |
| POST | /api/products | Create a new product |
| PUT | /api/products/{id} | Update an existing product |
| DELETE | /api/products/{id} | Delete a product |
| GET | /api/categories | List all categories |
| GET | /api/mime/{extension} | Look up the MIME type for a file extension |
| GET | /api/export/products | Export all products as a base64-encoded CSV string |
| GET | /api/download/catalog | Download the product catalog as a binary file |

## Feature Requirements

### 1. Observability — Built-in Tracing

The API should have OpenTelemetry tracing enabled using ASP.NET Core's native, built-in tracing support — without requiring any external instrumentation NuGet packages. The framework should automatically populate standard trace attributes (HTTP method, URL path, status code, server address). Use a console exporter during development for visibility.

### 2. OpenAPI Documentation

Generate OpenAPI documentation using the latest supported OpenAPI specification version available in .NET 11. The binary file download endpoint should properly describe its response as a binary octet-stream in the OpenAPI schema.

### 3. Dynamic Output Caching

Implement a dynamic output cache policy provider that resolves cache policies at runtime based on the policy name. For example, a "ShortCache" policy caches for 30 seconds and a "LongCache" policy caches for 5 minutes. Apply these policies to the product listing and category listing endpoints. The policy provider should be resolved through dependency injection, not statically configured.

### 4. Response Compression with Zstandard

Enable response compression including Zstandard (zstd) support. Also enable request decompression so the API can accept zstd-compressed request bodies. Configure the Zstandard compression quality level.

### 5. JSON Serialization

Configure JSON serialization with:
- **PascalCase** as the default property naming policy (using a built-in policy, not a custom one)
- **Per-property naming overrides**: Some specific properties should use camelCase even though the global policy is PascalCase
- **Type-level null handling**: DTO classes with many nullable properties should have a single type-level rule to omit all null values from output, rather than annotating each property individually
- **Read-only set support**: The `Tags` property should be typed as a read-only set interface and serialize/deserialize correctly without workarounds
- **Generic type metadata access**: Where type metadata is retrieved from serializer options, use the generic (cast-free) method

### 6. MIME Type Resolution

The `/api/mime/{extension}` endpoint should use .NET's built-in MIME type mapping to resolve extensions to MIME types and vice versa — without third-party packages. Support both extension→MIME and MIME→extension lookups.

### 7. Base64 Export

The `/api/export/products` endpoint should encode the CSV data using the Base64 convenience APIs that are now at parity with the Base64Url API surface (direct string encode/decode without going through `Convert`).

### 8. HTTP Client with Zstandard

If the API makes any outbound HTTP calls (or as a demo service), configure the HTTP client handler to automatically decompress Zstandard-encoded responses alongside GZip and Brotli.

## Build & Run

After creating the project:
1. Run `dotnet build` — must compile with zero errors
2. Run `dotnet run` — API must start and respond to requests
3. Test at least one endpoint to verify it works
4. Fix any issues before considering the task complete
