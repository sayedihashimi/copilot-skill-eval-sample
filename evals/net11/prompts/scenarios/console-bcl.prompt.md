---
description: "Create a .NET 11 console application — a developer toolkit with compression, cryptography, math, text processing, and file utilities"
tools: ["search/changes", "search/codebase", "web/fetch", "read/problems", "read/terminalLastCommand"]
---

# DevToolkit — A .NET 11 Developer Utilities Console Application

## Product Overview

**DevToolkit** is a command-line developer utilities application built on .NET 11. It provides a collection of practical tools that developers commonly need: file compression with multiple algorithms, cryptographic hashing and verification, text processing with full Unicode support, MIME type lookups, math utilities, and file system helpers. The app runs each utility as a self-contained demo, printing results to the console.

## Technical Requirements

- **Framework**: .NET 11
- **Project Type**: Console application (`dotnet new console`)
- **Project Location**: `./samples/net11/bcl-showcase/`
- **Dependencies**: No third-party NuGet packages — use only what ships with .NET 11
- **Code Organization**: Each utility area should be in its own file under a `Demos/` folder. `Program.cs` calls each demo with a labeled section header.
- **Use the latest C# language version** and latest .NET APIs throughout. Prefer new, idiomatic approaches over legacy patterns wherever available.

## Utility Modules

### 1. Compression Benchmark

Build a utility that compresses a block of sample text using every built-in compression algorithm available in .NET 11, including Zstandard. For each algorithm, print the original size, compressed size, and compression ratio. Also demonstrate decompression round-trip to verify data integrity. For Zstandard, show advanced options like quality level, window size, checksumming, and long-distance matching.

### 2. ML-Friendly Numeric Types

Build a utility that demonstrates the use of brain floating-point (BFloat16) arithmetic — the 16-bit float format used in ML/AI workloads. Create BFloat16 values, perform arithmetic (add, multiply, compare), serialize them to bytes and back using both big-endian and little-endian byte order, and show that they support standard .NET numeric interfaces. Also display the new SIMD vector constants (Pi, Tau, E, Epsilon, etc.).

### 3. Unicode Text Processor

Build a text processing utility that correctly handles supplementary Unicode characters (emoji, CJK, etc.) using Rune-based operations. The utility should:
- Search, replace, split, and trim strings using individual Unicode scalar values (not just `char`)
- Build strings with a StringBuilder that can append, retrieve, replace, and enumerate Unicode scalar values
- Enumerate text showing each character's position and byte length (important for surrogate pairs)
- Compare characters in a case-insensitive manner using proper culture-aware comparison

### 4. Cryptographic Hash Verifier

Build a utility that computes HMAC hashes for data and then verifies them in a single step. The verification should be constant-time (not vulnerable to timing attacks). Support multiple hash algorithms (SHA-256, SHA-384, SHA-512) and demonstrate both byte-array and stream-based verification. Also show an algorithm-agnostic verification approach where the algorithm is specified by name.

### 5. Immutable Lookup Tables

Build a utility that creates read-only, frozen lookup dictionaries using the most concise syntax available. Demonstrate creating frozen dictionaries with and without custom comparers (e.g., case-insensitive string keys). Show the most modern collection initialization syntax.

### 6. MIME Type Resolver

Build a utility that resolves MIME types from file extensions and vice versa using .NET's built-in MIME mapping (no third-party packages). Test with common file types: `.json`, `.pdf`, `.png`, `.html`, `.css`, `.js`, `.xml`. Show both extension→MIME and MIME→extension lookups, including span-based overloads for zero-allocation scenarios.

### 7. File System Utilities

Build a file system utility module that demonstrates:
- **Hard links**: Create a file, create a hard link to it, write through one path, read through the other to prove they reference the same data. Use both static and instance-based APIs. Clean up afterward.
- **Null handle**: Open a cross-platform null/dev-null file handle (without platform-specific paths) and write data to it to prove it discards silently.
- **Anonymous pipes**: Create an anonymous pipe pair with independently configurable async behavior for each end, query the handle type, and read/write through the pipe using random-access I/O on the non-seekable handles.

### 8. Integer Math Toolkit

Build a math utility that performs integer division using all available rounding modes: truncation, floor, ceiling, away-from-zero, and Euclidean. Show results for both positive and negative dividends (e.g., -7÷2 and 7÷2 under each mode). Demonstrate Euclidean remainder (always non-negative) and a combined quotient+remainder operation.

### 9. JSON Serialization Showcase

Build a utility that demonstrates modern JSON serialization features:
- Serialize and deserialize `IReadOnlySet<T>` (a set interface that should now work natively without workarounds)
- Use a PascalCase property naming policy
- Override the global naming policy on specific properties
- Apply a type-level "ignore when null" rule so all nullable properties on a class are omitted from output automatically (instead of annotating each property individually)
- Use the generic, cast-free way to retrieve type metadata from serializer options

### 10. Base64 Encoding Utilities

Build a utility that encodes and decodes data using the Base64 convenience APIs that now have full parity with Base64Url. Demonstrate `EncodeToString`, `DecodeFromChars`, `EncodeToChars`, and the length-calculation helpers.

### 11. Concurrent Flag Operations

Build a utility that atomically sets and clears bits on a flags enum using generic interlocked operations — directly on the enum type, without casting to `int`. Show that this works with any integer-backed enum.

### 12. Bit Counting

Build a utility that creates a `BitArray` and counts the number of set bits using the most efficient built-in method available (not a manual loop).

### 13. Universal Newline Regex

Build a utility that parses text containing mixed newline characters — `\r\n` (Windows), `\n` (Unix), `\u0085` (NEL), `\u2028` (line separator), `\u2029` (paragraph separator) — and demonstrates a regex mode that treats all Unicode newline sequences as line terminators for `^` and `$` anchors.

### 14. Process Exit Information

Build a utility that launches a child process, waits for it to exit, and then reads rich exit information including the exit code, whether it was cancelled, and the signal that killed it (on Unix).

### 15. Discriminated Union Types

Build a utility that defines a shape type (Circle, Rectangle, Triangle) as a discriminated union — a type where the compiler knows all possible cases and enforces exhaustive pattern matching with no default/wildcard case needed. Use the shape union in a switch expression to compute areas.

### 16. Collection Initialization with Constructor Arguments

Build a utility that creates collections using the most concise modern syntax, passing constructor arguments (like capacity hints or custom comparers) inline within the collection expression itself — without separate constructor calls.

## Output Format

When the application runs, each section should be clearly labeled:
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
