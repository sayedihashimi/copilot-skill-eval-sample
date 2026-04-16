---
description: "Create a .NET 11 Blazor Web App — a task management application with modern forms, navigation, and UI components"
tools: ["search/changes", "search/codebase", "web/fetch", "read/problems", "read/terminalLastCommand"]
---

# TaskFlow — A .NET 11 Blazor Task Management App

## Product Overview

**TaskFlow** is a Blazor Web App for managing tasks. Users can create, view, edit, and organize tasks with priorities, statuses, tags, and notes. The app uses server-side rendering with interactive server components and emphasizes modern Blazor patterns: declarative form components, smart navigation, virtualized lists, flash messaging, and environment-aware UI.

## Technical Requirements

- **Framework**: .NET 11
- **Project Type**: Blazor Web App with Interactive Server render mode (`dotnet new blazor`)
- **Project Location**: `./samples/net11/blazor-showcase/`
- **Data Store**: In-memory (static list or concurrent dictionary in a scoped/singleton service) — no database
- **Authentication**: None
- **Dependencies**: No third-party NuGet packages unless a feature specifically requires one
- **Use the latest C# language version** and latest .NET APIs throughout. Prefer new, idiomatic approaches over legacy patterns wherever available.

## Domain Model

### TaskItem

| Field | Type | Constraints |
|-------|------|-------------|
| Id | int | Auto-generated |
| Title | string | Required, max 200 chars |
| Description | string? | Optional |
| Priority | enum | Low, Medium, High, Critical |
| Status | enum | Todo, InProgress, Done, Blocked |
| AssignedTo | string? | Optional |
| DueDate | DateOnly? | Optional |
| Tags | List\<string\> | For categorization |
| Notes | string? | Optional free-form text |
| CreatedAt | DateTime | Auto-set |
| UpdatedAt | DateTime? | Set on update |

Apply display name attributes on model properties so that form labels and table headers can read them automatically rather than using hardcoded strings.

## Pages & Navigation

| Route | Page | Purpose |
|-------|------|---------|
| `/` | Dashboard | Summary of task counts by status, environment-specific debug info |
| `/tasks` | Task List | Searchable, sortable grid with row-click navigation and flash messages |
| `/tasks/{id}` | Task Detail | View task details with section anchors (Details, Notes, History) |
| `/tasks/create` | Create Task | Form to create a new task |
| `/tasks/{id}/edit` | Edit Task | Form to edit an existing task |

## Feature Requirements

### 1. Environment-Aware UI

The layout should display a prominent debug/development banner that is only visible in non-production environments. Use a declarative component-based approach — not manual `if` checks against `IHostEnvironment`. Support both include and exclude patterns (e.g., show only in Development, or hide only in Production).

### 2. Smart Form Labels

All forms (create and edit) should use a component-based approach for `<label>` elements that:
- Automatically reads the display name from model attributes (`[Display]`, `[DisplayName]`) instead of hardcoded strings
- Supports both **nested** labels (label wraps the input for implicit association) and **non-nested** labels (label and input linked by for/id)
- Use the latest Blazor form label component available in .NET 11

### 3. Display Names in Tables

Table column headers on the task list should use a component that reads display names from model attributes, not hardcoded strings. This ensures column headers stay in sync with the model and support localization.

### 4. Data Grid with Row Click

The task list should use a `QuickGrid` component with:
- Columns for Title, Priority, Status, AssignedTo, DueDate
- **Row click handling**: Clicking any row navigates to the task detail page. The grid should use the built-in row-click event (not a button column workaround). Cursor styling should indicate rows are clickable.

### 5. Relative Navigation

Navigation between related pages (e.g., from task detail to edit) should use **relative-to-current-URI** navigation, not base-relative. This makes nested page structures work correctly. Use the latest navigation options available in .NET 11. Also use this approach in `NavLink` components where appropriate.

### 6. Section Anchor Links

The task detail page should have sections (Details, Notes, History) with anchor links that jump to each section. Use the latest NavigationManager extension for building URIs with hash fragments — not manual string concatenation.

### 7. Flash Messages Between Pages

After creating or updating a task, a success message should appear on the task list page. This message should:
- Persist across one navigation (set on page A, displayed on page B)
- Be automatically removed after being read (one-time display)
- Use the latest Blazor mechanism for temporary cross-page data (analogous to MVC's TempData)
- Support both "get and remove" and "peek without removing" semantics

### 8. Automatic Base Path

The root layout should use a component that automatically resolves the correct `<base href>` from the app's base URI, instead of hardcoding `<base href="/" />`. This ensures the app works correctly when hosted under a subpath.

### 9. Virtualized List with Variable-Height Items

The task list (or a notes/activity feed view) should use virtualization for efficient rendering of large lists. The virtualization should correctly handle items with different heights (e.g., tasks with short vs. long descriptions) by adapting to actual rendered sizes at runtime.

### 10. SignalR Connection Configuration

Configure the SignalR connection for interactive server components in `Program.cs`. Set options for:
- Closing the connection on authentication expiration
- Allowing stateful reconnects
- Custom application buffer size

### 11. IHostedService Note

Add a comment or note in the project explaining that .NET 11 now supports `IHostedService` in Blazor WebAssembly, enabling background services (e.g., periodic data refresh) to run in WASM apps.

## Build & Run

After creating the project:
1. Run `dotnet build` — must compile with zero errors
2. Run `dotnet run` — app must start and render pages
3. Navigate through the pages to verify they work
4. Fix any issues before considering the task complete
