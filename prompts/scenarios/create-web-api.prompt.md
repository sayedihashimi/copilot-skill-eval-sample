---
mode: agent
description: "Create a Contact Manager API using .NET 10 ASP.NET Core Web API with SQLite"
tools: ["changes", "codebase", "fetch", "findTestFiles", "githubRepo", "problems", "runner", "selection", "terminalLastCommand", "terminalSelection", "usages", "visionScreenshot"]
---

# Create web api

## App Overview

**Nexus Contacts** is a contact management API that allows users to organize and manage their personal and professional contacts. The system manages **Contacts**, **Phone Numbers**, **Email Addresses**, **Addresses**, **Tags** (for grouping/categorizing contacts), and **Notes** (free-form notes attached to contacts). It is designed for individuals or small teams who need a centralized, searchable contact directory with tagging and filtering capabilities.

## Technical Requirements

- **Framework**: .NET 10, ASP.NET Core Web API
- **Database**: SQLite via Entity Framework Core (EF Core) with code-first migrations
- **Authentication**: None — no authentication or authorization required
- **Documentation**: OpenAPI/Swagger documentation enabled
- **Project Structure**: Structured with separation of concerns — **Models**, **DTOs**, **Services** (interface + implementation), and **API layer**
- **Validation**: Use Data Annotations and/or FluentValidation for input validation
- **Error Handling**: Global error handling middleware that returns consistent **ProblemDetails** responses
- **Project Location**: Create the project at `./src/Create web api/`

## Entities & Relationships

### Contact

| Field       | Type     | Constraints                          |
|-------------|----------|--------------------------------------|
| Id          | int      | PK, auto-generated                   |
| FirstName   | string   | Required, max length 100             |
| LastName    | string   | Required, max length 100             |
| Company     | string   | Optional, max length 200             |
| JobTitle    | string   | Optional, max length 150             |
| Birthday    | DateOnly | Optional, must be in the past        |
| IsFavorite  | bool     | Required, default false              |
| ImageUrl    | string   | Optional, max length 500             |
| Website     | string   | Optional, max length 500             |
| CreatedAt   | DateTime | Auto-set on creation (UTC)           |
| UpdatedAt   | DateTime | Auto-set on creation and update (UTC)|

- A Contact has **many** PhoneNumbers (one-to-many)
- A Contact has **many** EmailAddresses (one-to-many)
- A Contact has **many** Addresses (one-to-many)
- A Contact has **many** Notes (one-to-many)
- A Contact has **many** Tags (many-to-many via **ContactTag** join entity)

### PhoneNumber

| Field      | Type   | Constraints                                              |
|------------|--------|----------------------------------------------------------|
| Id         | int    | PK, auto-generated                                       |
| ContactId  | int    | FK → Contact.Id, required                                |
| Label      | string | Required; one of: Mobile, Home, Work, Other              |
| Number     | string | Required, max length 30                                  |
| IsPrimary  | bool   | Required, default false                                  |

### EmailAddress

| Field      | Type   | Constraints                                              |
|------------|--------|----------------------------------------------------------|
| Id         | int    | PK, auto-generated                                       |
| ContactId  | int    | FK → Contact.Id, required                                |
| Label      | string | Required; one of: Personal, Work, Other                  |
| Email      | string | Required, max length 254, must be valid email format     |
| IsPrimary  | bool   | Required, default false                                  |

### Address

| Field      | Type   | Constraints                                              |
|------------|--------|----------------------------------------------------------|
| Id         | int    | PK, auto-generated                                       |
| ContactId  | int    | FK → Contact.Id, required                                |
| Label      | string | Required; one of: Home, Work, Other                      |
| Street     | string | Required, max length 200                                 |
| City       | string | Required, max length 100                                 |
| State      | string | Optional, max length 100                                 |
| ZipCode    | string | Optional, max length 20                                  |
| Country    | string | Required, max length 100, default "USA"                  |

### Tag

| Field | Type   | Constraints                                   |
|-------|--------|-----------------------------------------------|
| Id    | int    | PK, auto-generated                            |
| Name  | string | Required, max length 50, unique (case-insensitive) |
| Color | string | Optional, max length 7 (hex color, e.g. #FF5733) |

### ContactTag (join entity)

| Field     | Type | Constraints                     |
|-----------|------|---------------------------------|
| ContactId | int  | FK → Contact.Id, composite PK   |
| TagId     | int  | FK → Tag.Id, composite PK       |

### Note

| Field     | Type     | Constraints                          |
|-----------|----------|--------------------------------------|
| Id        | int      | PK, auto-generated                   |
| ContactId | int      | FK → Contact.Id, required            |
| Content   | string   | Required, max length 2000            |
| CreatedAt | DateTime | Auto-set on creation (UTC)           |
| UpdatedAt | DateTime | Auto-set on creation and update (UTC)|

## Business Rules

1. **Contact name required**: Every contact must have both a FirstName and a LastName. If either is missing or whitespace-only, return **400 Bad Request**.
2. **Birthday in the past**: If a Birthday is provided, it must be a date before today. If a future date is supplied, return **400 Bad Request** with message "Birthday must be in the past."
3. **Unique primary phone**: A contact may have at most one PhoneNumber where IsPrimary is true. If a request would create a second primary phone number, return **409 Conflict** with message "Contact already has a primary phone number. Remove the existing primary designation first."
4. **Unique primary email**: A contact may have at most one EmailAddress where IsPrimary is true. If a request would create a second primary email, return **409 Conflict** with message "Contact already has a primary email address. Remove the existing primary designation first."
5. **Valid email format**: All EmailAddress.Email values must conform to a standard email format (contains @ and a domain). If invalid, return **400 Bad Request**.
6. **Tag name uniqueness**: Tag names are unique in a case-insensitive manner. Attempting to create a tag with a name that already exists (ignoring case) must return **409 Conflict** with message "A tag with this name already exists."
7. **Cascade delete contacts**: Deleting a contact must cascade-delete all associated PhoneNumbers, EmailAddresses, Addresses, Notes, and ContactTag associations.
8. **No duplicate tag assignment**: Assigning a tag to a contact that already has that tag must return **409 Conflict** with message "This tag is already assigned to the contact."
9. **Note length limit**: Note content must be between 1 and 2000 characters. Empty or over-length notes must return **400 Bad Request**.
10. **Phone number format**: Phone numbers must contain only digits, spaces, hyphens, parentheses, and an optional leading +. Max length 30 characters. Invalid formats must return **400 Bad Request** with message "Invalid phone number format."

## API Endpoints

### Contacts

| Method | Endpoint                        | Description                                                                                   |
|--------|---------------------------------|-----------------------------------------------------------------------------------------------|
| GET    | /api/contacts                   | List contacts with search, filter, and pagination. Supports query params: `search` (searches FirstName, LastName, Company, Email, Phone), `tagId`, `isFavorite`, `page` (default 1), `pageSize` (default 20, max 100) |
| GET    | /api/contacts/{id}              | Get a single contact by ID, including all phone numbers, emails, addresses, tags, and notes   |
| POST   | /api/contacts                   | Create a new contact                                                                          |
| PUT    | /api/contacts/{id}              | Update an existing contact's core fields                                                      |
| DELETE | /api/contacts/{id}              | Delete a contact and all associated data (cascade)                                            |
| POST   | /api/contacts/{id}/favorite     | Mark a contact as favorite (set IsFavorite = true)                                            |
| DELETE | /api/contacts/{id}/favorite     | Unmark a contact as favorite (set IsFavorite = false)                                         |

### Phone Numbers

| Method | Endpoint                                     | Description                                      |
|--------|----------------------------------------------|--------------------------------------------------|
| GET    | /api/contacts/{contactId}/phones              | List all phone numbers for a contact             |
| POST   | /api/contacts/{contactId}/phones              | Add a phone number to a contact                  |
| PUT    | /api/contacts/{contactId}/phones/{phoneId}    | Update an existing phone number                  |
| DELETE | /api/contacts/{contactId}/phones/{phoneId}    | Delete a phone number                            |

### Email Addresses

| Method | Endpoint                                      | Description                                     |
|--------|-----------------------------------------------|-------------------------------------------------|
| GET    | /api/contacts/{contactId}/emails               | List all email addresses for a contact          |
| POST   | /api/contacts/{contactId}/emails               | Add an email address to a contact               |
| PUT    | /api/contacts/{contactId}/emails/{emailId}     | Update an existing email address                |
| DELETE | /api/contacts/{contactId}/emails/{emailId}     | Delete an email address                         |

### Addresses

| Method | Endpoint                                          | Description                                  |
|--------|---------------------------------------------------|----------------------------------------------|
| GET    | /api/contacts/{contactId}/addresses                | List all addresses for a contact             |
| POST   | /api/contacts/{contactId}/addresses                | Add an address to a contact                  |
| PUT    | /api/contacts/{contactId}/addresses/{addressId}    | Update an existing address                   |
| DELETE | /api/contacts/{contactId}/addresses/{addressId}    | Delete an address                            |

### Notes

| Method | Endpoint                                      | Description                                     |
|--------|-----------------------------------------------|-------------------------------------------------|
| GET    | /api/contacts/{contactId}/notes                | List all notes for a contact (ordered by CreatedAt descending) |
| POST   | /api/contacts/{contactId}/notes                | Add a note to a contact                         |
| PUT    | /api/contacts/{contactId}/notes/{noteId}       | Update an existing note                         |
| DELETE | /api/contacts/{contactId}/notes/{noteId}       | Delete a note                                   |

### Tags

| Method | Endpoint                                      | Description                                     |
|--------|-----------------------------------------------|-------------------------------------------------|
| GET    | /api/tags                                      | List all tags                                   |
| POST   | /api/tags                                      | Create a new tag                                |
| PUT    | /api/tags/{id}                                 | Update a tag (name and/or color)                |
| DELETE | /api/tags/{id}                                 | Delete a tag (removes all ContactTag associations) |
| POST   | /api/contacts/{contactId}/tags/{tagId}         | Assign a tag to a contact                       |
| DELETE | /api/contacts/{contactId}/tags/{tagId}         | Remove a tag from a contact                     |

## Seed Data

The application **MUST** seed the database on startup with the exact data listed below. Ensure the seed data only runs when the database is empty to avoid duplicates.

### Tags

Seed these tags first (IDs 1–8):

| Id | Name        | Color   |
|----|-------------|---------|
| 1  | Family      | #E74C3C |
| 2  | Friends     | #3498DB |
| 3  | Work        | #2ECC71 |
| 4  | VIP         | #F39C12 |
| 5  | Vendors     | #9B59B6 |
| 6  | Networking  | #1ABC9C |
| 7  | College     | #E67E22 |
| 8  | Neighbors   | #34495E |

### Contacts

Seed these 20 contacts with IDs 1–20:

#### Contact 1 — Maria Santos
- **FirstName**: Maria, **LastName**: Santos, **Company**: Luminex Design Studio, **JobTitle**: Creative Director
- **Birthday**: 1985-06-15, **IsFavorite**: true, **Website**: https://luminexdesign.com, **ImageUrl**: https://example.com/photos/maria.jpg
- **Phones**: (Mobile) +1 (555) 100-1001 ★Primary, (Work) +1 (555) 100-1002
- **Emails**: (Personal) maria.santos@email.com ★Primary, (Work) maria@luminexdesign.com
- **Addresses**: (Work) 742 Evergreen Terrace, Portland, OR, 97201, USA
- **Tags**: Work, VIP, Friends
- **Notes**: "Key design partner for the rebrand project. Prefers communication via email."

#### Contact 2 — James O'Brien
- **FirstName**: James, **LastName**: O'Brien, **Company**: null, **JobTitle**: null
- **Birthday**: 1990-03-22, **IsFavorite**: true, **Website**: null, **ImageUrl**: null
- **Phones**: (Mobile) +1 (555) 200-2001 ★Primary, (Home) +1 (555) 200-2002
- **Emails**: (Personal) james.obrien92@email.com ★Primary
- **Addresses**: (Home) 158 Oak Lane, Seattle, WA, 98101, USA
- **Tags**: Family, VIP
- **Notes**: "Brother-in-law. Allergic to shellfish — remember for dinner invitations."

#### Contact 3 — Priya Patel
- **FirstName**: Priya, **LastName**: Patel, **Company**: Greenleaf Analytics, **JobTitle**: Data Scientist
- **Birthday**: null, **IsFavorite**: false, **Website**: https://priyapatel.dev, **ImageUrl**: null
- **Phones**: (Work) +1 (555) 300-3001 ★Primary, (Mobile) +1 (555) 300-3002
- **Emails**: (Work) priya@greenleafanalytics.com ★Primary, (Personal) priya.patel88@email.com
- **Addresses**: (Work) 900 Tech Boulevard, Suite 400, San Francisco, CA, 94105, USA
- **Tags**: Work, Networking
- **Notes**: "Met at DataConf 2024. Interested in collaborating on ML pipeline project."

#### Contact 4 — Carlos Rivera
- **FirstName**: Carlos, **LastName**: Rivera, **Company**: Rivera's Auto Body, **JobTitle**: Owner
- **Birthday**: 1978-11-03, **IsFavorite**: false, **Website**: null, **ImageUrl**: null
- **Phones**: (Work) +1 (555) 400-4001 ★Primary, (Mobile) +1 (555) 400-4002
- **Emails**: (Work) carlos@riverasautobody.com ★Primary
- **Addresses**: (Work) 2311 Industrial Parkway, Denver, CO, 80216, USA; (Home) 445 Maple Street, Denver, CO, 80220, USA
- **Tags**: Vendors
- **Notes**: "Reliable mechanic. Gives 10% discount on repeat visits."

#### Contact 5 — Amara Johnson
- **FirstName**: Amara, **LastName**: Johnson, **Company**: null, **JobTitle**: null
- **Birthday**: 1995-08-19, **IsFavorite**: true, **Website**: null, **ImageUrl**: https://example.com/photos/amara.jpg
- **Phones**: (Mobile) +1 (555) 500-5001 ★Primary
- **Emails**: (Personal) amara.j@email.com ★Primary
- **Addresses**: (Home) 67 Birchwood Drive, Austin, TX, 78701, USA
- **Tags**: Friends, College
- **Notes**: "College roommate. Runs the Austin marathon every year."

#### Contact 6 — Wei Zhang
- **FirstName**: Wei, **LastName**: Zhang, **Company**: Pacific Rim Imports, **JobTitle**: Supply Chain Manager
- **Birthday**: null, **IsFavorite**: false, **Website**: https://pacificrim-imports.com, **ImageUrl**: null
- **Phones**: (Work) +1 (555) 600-6001 ★Primary, (Mobile) +1 (555) 600-6002, (Other) +86 138-0013-8000
- **Emails**: (Work) wei.zhang@pacificrim.com ★Primary, (Personal) weizhang88@email.com
- **Addresses**: (Work) 1500 Harbor Boulevard, Long Beach, CA, 90802, USA
- **Tags**: Work, Vendors, Networking
- **Notes**: "Primary contact for overseas shipments. Speaks Mandarin and English fluently."

#### Contact 7 — Sofia Andersson
- **FirstName**: Sofia, **LastName**: Andersson, **Company**: null, **JobTitle**: null
- **Birthday**: 1988-12-01, **IsFavorite**: false, **Website**: null, **ImageUrl**: null
- **Phones**: (Mobile) +1 (555) 700-7001 ★Primary, (Home) +1 (555) 700-7002
- **Emails**: (Personal) sofia.andersson@email.com ★Primary
- **Addresses**: (Home) 234 Pine Street, Apt 5B, Chicago, IL, 60614, USA
- **Tags**: Friends, Neighbors
- **Notes**: null

#### Contact 8 — David Kim
- **FirstName**: David, **LastName**: Kim, **Company**: Apex Legal Group, **JobTitle**: Attorney
- **Birthday**: null, **IsFavorite**: true, **Website**: https://apexlegal.com, **ImageUrl**: null
- **Phones**: (Work) +1 (555) 800-8001 ★Primary, (Mobile) +1 (555) 800-8002
- **Emails**: (Work) dkim@apexlegal.com ★Primary, (Personal) david.kim.law@email.com
- **Addresses**: (Work) 50 Court Street, Suite 1200, Boston, MA, 02108, USA
- **Tags**: Work, VIP
- **Notes**: "Handles all business contracts. Retainer agreement renewed annually in January."

#### Contact 9 — Fatima Al-Rashid
- **FirstName**: Fatima, **LastName**: Al-Rashid, **Company**: Crescent Moon Bakery, **JobTitle**: Head Baker
- **Birthday**: 1983-04-10, **IsFavorite**: false, **Website**: null, **ImageUrl**: null
- **Phones**: (Work) +1 (555) 900-9001 ★Primary, (Mobile) +1 (555) 900-9002
- **Emails**: (Work) fatima@crescentmoonbakery.com ★Primary
- **Addresses**: (Work) 88 Main Street, Dearborn, MI, 48124, USA
- **Tags**: Vendors, Friends
- **Notes**: "Makes the best baklava in town. Order 48 hours in advance for large catering."

#### Contact 10 — Ryan Mitchell
- **FirstName**: Ryan, **LastName**: Mitchell, **Company**: null, **JobTitle**: Freelance Photographer
- **Birthday**: null, **IsFavorite**: false, **Website**: https://ryanmitchellphoto.com, **ImageUrl**: https://example.com/photos/ryan.jpg
- **Phones**: (Mobile) +1 (555) 101-0101 ★Primary
- **Emails**: (Personal) ryan@ryanmitchellphoto.com ★Primary
- **Addresses**: null
- **Tags**: Friends, Networking
- **Notes**: null

#### Contact 11 — Elena Vasquez
- **FirstName**: Elena, **LastName**: Vasquez, **Company**: Bright Horizons Academy, **JobTitle**: Principal
- **Birthday**: 1975-09-28, **IsFavorite**: false, **Website**: null, **ImageUrl**: null
- **Phones**: (Work) +1 (555) 111-1101, (Mobile) +1 (555) 111-1102 ★Primary
- **Emails**: (Work) evasquez@brighthorizons.edu ★Primary
- **Addresses**: (Work) 1200 Learning Lane, Miami, FL, 33101, USA
- **Tags**: Work, Family
- **Notes**: "Kids' school principal. Annual parent-teacher conference in October."

#### Contact 12 — Marcus Thompson
- **FirstName**: Marcus, **LastName**: Thompson, **Company**: null, **JobTitle**: null
- **Birthday**: null, **IsFavorite**: false, **Website**: null, **ImageUrl**: null
- **Phones**: (Mobile) +1 (555) 122-1201 ★Primary
- **Emails**: (Personal) marcus.t@email.com ★Primary
- **Addresses**: (Home) 789 Elm Court, Nashville, TN, 37201, USA
- **Tags**: Neighbors
- **Notes**: "Lives two doors down. Has a spare key to the house."

#### Contact 13 — Aiko Tanaka
- **FirstName**: Aiko, **LastName**: Tanaka, **Company**: Sakura Technologies, **JobTitle**: UX Researcher
- **Birthday**: 1992-02-14, **IsFavorite**: false, **Website**: https://aikotanaka.design, **ImageUrl**: null
- **Phones**: (Mobile) +1 (555) 133-1301 ★Primary, (Work) +1 (555) 133-1302
- **Emails**: (Personal) aiko.tanaka@email.com, (Work) atanaka@sakuratech.com ★Primary
- **Addresses**: (Work) 3000 Innovation Way, San Jose, CA, 95134, USA
- **Tags**: Work, Networking, College
- **Notes**: "Went to grad school together at Stanford. Great resource for usability testing."

#### Contact 14 — Liam O'Connor
- **FirstName**: Liam, **LastName**: O'Connor, **Company**: null, **JobTitle**: null
- **Birthday**: null, **IsFavorite**: false, **Website**: null, **ImageUrl**: null
- **Phones**: (Mobile) +1 (555) 144-1401 ★Primary, (Home) +1 (555) 144-1402
- **Emails**: (Personal) liam.oconnor@email.com ★Primary
- **Addresses**: (Home) 56 Clover Road, Philadelphia, PA, 19103, USA
- **Tags**: Family
- **Notes**: null

#### Contact 15 — Natasha Volkov
- **FirstName**: Natasha, **LastName**: Volkov, **Company**: Volkov & Associates CPA, **JobTitle**: Certified Public Accountant
- **Birthday**: 1980-07-04, **IsFavorite**: true, **Website**: https://volkovcpa.com, **ImageUrl**: null
- **Phones**: (Work) +1 (555) 155-1501 ★Primary, (Mobile) +1 (555) 155-1502
- **Emails**: (Work) natasha@volkovcpa.com ★Primary, (Personal) nvolkov@email.com
- **Addresses**: (Work) 400 Financial District Plaza, Suite 800, New York, NY, 10005, USA
- **Tags**: Work, VIP
- **Notes**: "Tax season crunch — best to reach her after April 15. Does both personal and business taxes."

#### Contact 16 — Omar Hassan
- **FirstName**: Omar, **LastName**: Hassan, **Company**: null, **JobTitle**: null
- **Birthday**: 1998-01-30, **IsFavorite**: false, **Website**: null, **ImageUrl**: null
- **Phones**: (Mobile) +1 (555) 166-1601 ★Primary
- **Emails**: (Personal) omar.hassan98@email.com ★Primary
- **Addresses**: (Home) 321 Willow Way, Minneapolis, MN, 55401, USA
- **Tags**: Friends, College
- **Notes**: "Study group buddy from undergrad. Now working in fintech."

#### Contact 17 — Grace Nwosu
- **FirstName**: Grace, **LastName**: Nwosu, **Company**: Emerald Health Clinic, **JobTitle**: Family Physician
- **Birthday**: null, **IsFavorite**: false, **Website**: null, **ImageUrl**: null
- **Phones**: (Work) +1 (555) 177-1701 ★Primary, (Mobile) +1 (555) 177-1702
- **Emails**: (Work) gnwosu@emeraldhealth.com ★Primary
- **Addresses**: (Work) 555 Wellness Drive, Atlanta, GA, 30301, USA
- **Tags**: Work
- **Notes**: "Family doctor. Office hours Mon–Fri 8am–5pm. Accepts walk-ins before 10am."

#### Contact 18 — Benjamin Clarke
- **FirstName**: Benjamin, **LastName**: Clarke, **Company**: Clarke Home Renovations, **JobTitle**: General Contractor
- **Birthday**: 1972-05-18, **IsFavorite**: false, **Website**: https://clarkehomereno.com, **ImageUrl**: null
- **Phones**: (Mobile) +1 (555) 188-1801 ★Primary, (Work) +1 (555) 188-1802
- **Emails**: (Work) ben@clarkehomereno.com ★Primary
- **Addresses**: (Work) 1010 Builder's Row, Phoenix, AZ, 85001, USA; (Home) 42 Desert Vista Lane, Scottsdale, AZ, 85251, USA
- **Tags**: Vendors, Neighbors
- **Notes**: "Did the kitchen remodel in 2023. Licensed and insured. Booked 3 months out typically."

#### Contact 19 — Isabelle Moreau
- **FirstName**: Isabelle, **LastName**: Moreau, **Company**: null, **JobTitle**: null
- **Birthday**: null, **IsFavorite**: false, **Website**: null, **ImageUrl**: null
- **Phones**: (Mobile) +1 (555) 199-1901 ★Primary
- **Emails**: (Personal) isabelle.moreau@email.com ★Primary
- **Addresses**: null
- **Tags**: Friends
- **Notes**: null

#### Contact 20 — Raj Krishnamurthy
- **FirstName**: Raj, **LastName**: Krishnamurthy, **Company**: NovaStar Consulting, **JobTitle**: Managing Partner
- **Birthday**: 1970-10-12, **IsFavorite**: true, **Website**: https://novastarconsulting.com, **ImageUrl**: https://example.com/photos/raj.jpg
- **Phones**: (Work) +1 (555) 200-2010 ★Primary, (Mobile) +1 (555) 200-2020, (Other) +91 98765-43210
- **Emails**: (Work) raj@novastarconsulting.com ★Primary, (Personal) raj.krishna@email.com, (Other) rkrishnamurthy@alumni.mit.edu
- **Addresses**: (Work) 8000 Towers Parkway, Suite 2200, Dallas, TX, 75201, USA; (Home) 15 Prestige Circle, Plano, TX, 75024, USA
- **Tags**: Work, VIP, Networking
- **Notes**: "Mentor and business advisor. Monthly check-in calls on the first Monday. MIT alumnus — connects well at alumni events."

### Seed Data Summary

The above data includes:
- **20 contacts**: mix of corporate and personal, 6 with favorites, 12 with birthdays, varied companies and job titles
- **35 phone numbers** across contacts with Mobile, Home, Work, and Other labels; primary designations set as marked
- **28 email addresses** across contacts with Personal, Work, and Other labels; primary designations set as marked
- **18 addresses** across contacts with Home, Work labels; includes multi-address contacts
- **8 tags** with hex colors
- **42 ContactTag** associations with overlapping assignments
- **16 notes** with varied content lengths

## HTTP File

Create a `.http` file in the same folder as `Program.cs`. This file should:

- Use a `@baseUrl` variable set to `http://localhost:`
- Include sample requests for **ALL** endpoints listed above
- Group requests by resource with comment headers
- Include realistic request bodies for POST and PUT operations
- Include query parameter examples for search/filter/pagination endpoints
- Use IDs that correspond to the seed data so requests work out of the box
- Include examples that test business rules (e.g., duplicate primary phone, invalid email, future birthday)

## Cross-Cutting Concerns

### Error Handling
- Global exception handler returning RFC 7807 **ProblemDetails** responses
- Business rule violations → **400 Bad Request** or **409 Conflict** with descriptive messages
- Not-found → **404 Not Found**

### Validation
- Validate all input DTOs
- Return **400 Bad Request** with structured validation error details

### Logging
- Use the built-in logger
- Log key operations at **Information** level
- Log errors at **Error** level

### OpenAPI / Swagger
- Swagger UI accessible at root or `/swagger`
- Descriptive operation summaries and response types

### Pagination
- Consistent pattern across all list endpoints
- Accept page number and page size with sensible defaults
- Return pagination metadata (total count, total pages, current page)

## Project Location

Create the project at: `./src/Create web api/`

The project should be a standalone project with no dependencies on other projects in this repository.
