# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository Overview

This is a **monorepo** for the Termodom business ecosystem containing multiple applications:

- **TD.Web** - Public-facing e-commerce platform (customer site and admin backoffice)
- **TD.Office** - Internal office management system with multiple modules (MassSMS, shipping, market review, etc.)
- **TD.Komercijalno** - API used to communicate with internal commercial platform
- **TD.TDOffice** - Legacy office system (not used anymore)
- **TD.OfficeServer** - Legacy backend orchestration layer for office applications (not used anymore)
- **TD.Cron** - Scheduled background jobs
- **TD.Core** - Core framework libraries used across all domains
- **TD.Common** - Cross-cutting concerns (Vault, Minio, environment configs)

## Technology Stack

### Backend (.NET)
- .NET 9.0 (primary), with some .NET 8.0 and .NET 7.0 projects
- ASP.NET Core Web API
- Entity Framework Core 9.0
- PostgreSQL (via Npgsql) for modern services
- Firebird SQL for legacy systems (TD.Komercijalno, TD.TDOffice)
- LSCore Framework (9.1.x) - custom framework providing Auth, Repository, Validation, Mapper, ApiClient abstractions
- Redis (StackExchange.Redis) for caching

### Frontend (Node.js)
- Next.js 13/14 with React 18.2
- TypeScript 5.2
- Material-UI (MUI) v5 (@mui/material, @mui/x-data-grid, @mui/x-date-pickers)
- Redux Toolkit + Zustand for state management
- React Hook Form + Yup for forms and validation
- Axios for HTTP
- ls-core-next - custom Next.js utilities
- Jest + React Testing Library for testing

### DevOps
- Docker for containerization
- Kubernetes for orchestration
- GitHub Actions for CI/CD
- Vault for secrets management
- Minio for object storage

## Common Commands

### Backend (.NET)

**Build and Run:**
```bash
# Restore dependencies for a specific project
dotnet restore src/TD.Komercijalno/TD.Komercijalno.Api/TD.Komercijalno.Api.csproj

# Build a specific project
dotnet build src/TD.Komercijalno/TD.Komercijalno.Api/TD.Komercijalno.Api.csproj

# Run a specific project
dotnet run --project src/TD.Komercijalno/TD.Komercijalno.Api/TD.Komercijalno.Api.csproj

# Build entire solution
dotnet build TD.sln
```

**Testing:**
```bash
# Run tests for a specific test project
dotnet test src/TD.Komercijalno/TD.Komercijalno.Tests/TD.Komercijalno.Tests.csproj

# Run tests with detailed output
dotnet test src/TD.Komercijalno/TD.Komercijalno.Tests/TD.Komercijalno.Tests.csproj --verbosity normal

# Other test projects:
dotnet test src/TD.Office/TD.Office.Public.Tests/TD.Office.Public.Tests.csproj
dotnet test src/TD.Web/TD.Web.Public/TD.Web.Public.Tests/TD.Web.Public.Tests.csproj
```

**Database Migrations:**
```bash
# Add migration (example for TD.Web.Common)
dotnet ef migrations add MigrationName --project src/TD.Web/TD.Web.Common/TD.Web.Common.DbMigrations

# Update database
dotnet ef database update --project src/TD.Web/TD.Web.Common/TD.Web.Common.DbMigrations
```

### Frontend (Node.js)

**Setup:**
```bash
# Install all workspace dependencies from root
npm install

# Install for a specific frontend project
cd src/TD.Office/TD.Office.Public/TD.Office.Public.Fe
npm install
```

**Development:**
```bash
# Run development server (from frontend project directory)
npm run dev

# Build for production
npm run build

# Start production server
npm start

# Lint
npm run lint
```

**Testing:**
```bash
# Run Jest tests
npm test

# Run tests in watch mode
npm run test:watch

# Run tests with coverage
npm run test:coverage

# Run tests in CI mode (used in GitHub Actions)
npm run test:ci
```

**UAT (Selenium) Tests:**
```bash
# From a .Fe.UAT directory (e.g., src/TD.Office/TD.Office.Public/TD.Office.Public.Fe.UAT)
npm run test:dockerized-driver-chrome
npm run test:dockerized-driver-firefox
```

## Architecture Patterns

### Project Structure (Per Domain)

Each domain follows a consistent layered architecture:

```
TD.[Domain]/
├── TD.[Domain].Api/              # Controllers, Program.cs, API endpoints
├── TD.[Domain].Contracts/        # DTOs, Requests, Enums, Interfaces (IManagers)
│   ├── Dtos/
│   ├── DtoMappings/
│   ├── Requests/
│   ├── Enums/
│   └── IManagers/
├── TD.[Domain].Domain/           # Business logic (Managers), Validators
│   ├── Managers/                 # Business logic implementations
│   └── Validators/               # Validation logic (LSCore.Validation)
├── TD.[Domain].Repository/       # Data access, EF DbContext
├── TD.[Domain].Client/           # API client for inter-service calls
├── TD.[Domain].DbMigrations/     # EF Core migrations
├── TD.[Domain].Fe/               # Next.js frontend (production)
│   ├── src/
│   │   ├── pages/               # Next.js pages/routes
│   │   ├── widgets/             # Reusable UI components
│   │   ├── features/            # Feature modules
│   │   ├── api/                 # API client code
│   │   ├── dtos/                # TypeScript DTOs
│   │   ├── helpers/             # Utilities
│   │   └── zStore/              # Zustand stores
├── TD.[Domain].Fe.UAT/           # Selenium UAT tests
└── TD.[Domain].Tests/            # Backend unit/integration tests
```

### Key Architectural Patterns

1. **Layered Architecture**: API → Domain (Managers) → Repository → Database
2. **Manager Pattern**: Business logic resides in Manager classes (similar to Service pattern)
3. **DTO Pattern**: Separate DTOs from Entities with explicit mappings (ValueInjecter)
4. **Repository Pattern**: Via LSCore.Repository abstractions
5. **Validation Pattern**: Declarative validators in Domain layer using LSCore.Validation
6. **Client Pattern**: Each API has a corresponding .Client project for typed inter-service communication
7. **Dependency Injection**: Extensive use via LSCore.DependencyInjection

### Frontend Patterns

- **Widget Pattern**: Reusable components in `widgets/` folder
- **Feature Pattern**: Business features grouped in `features/`
- **API Abstraction**: Centralized API clients in `api/` or `apis/`
- **State Management Split**:
  - Redux Toolkit for complex/global state
  - Zustand for simple/local state
- **Form Handling**: React Hook Form + Yup validation schemas

### Shared Libraries

**Backend:**
- **TD.Core**: Foundation used by all domains (TD.Core.Contracts, TD.Core.Domain, TD.Core.Repository)
- **TD.Common**: Cross-cutting utilities (Vault, Minio, PhoneValidator, Environments)
- **TD.Web.Common**: Shared between TD.Web.Public and TD.Web.Admin
- **TD.Office.Common**: Shared between all TD.Office modules

**Frontend (npm workspaces):**
- Projects with `.Node` suffix are npm workspace packages
- `td-web-common-contracts-node`, `td-web-common-domain-node`, `td-web-common-repository-node`
- `td-office-common-repository-node`
- `td-common-vault-node`, `td-common-minio-node`
- `td-cron-common-domain-node`

### Inter-Service Communication

Services communicate via typed client libraries (e.g., `TD.Komercijalno.Client`, `TD.Office.Public.Client`) using LSCore.ApiClient.Rest.

### Database Architecture

- **Separate databases per domain** to avoid tight coupling
- PostgreSQL for modern services (TD.Web, TD.Office)
- Firebird for legacy systems (TD.Komercijalno, TD.TDOffice)
- Each domain has its own `.DbMigrations` project with EF Core migrations

## Naming Conventions

**Project Suffixes:**
- `.Api` - ASP.NET Core API
- `.Contracts` - Data contracts/DTOs/interfaces
- `.Domain` - Business logic (Managers, Validators)
- `.Repository` - Data access layer
- `.Client` - API client library
- `.Fe` - Frontend (production Next.js app)
- `.Fe.UAT` - Frontend UAT tests (Selenium)
- `.Node` - Node.js library (npm workspace package)
- `.DbMigrations` - EF Core database migrations
- `.Tests` - Backend test project

**Folder Conventions:**
- `Managers/` - Business logic classes
- `Validators/` - Validation logic
- `Dtos/` - Data transfer objects
- `DtoMappings/` - DTO ↔ Entity mapping configurations
- `Requests/` - API request models
- `Entities/` - Database entity models
- `Enums/` - Enumerations
- `IManagers/` or `Interfaces/` - Interface definitions

## Development Workflow

### Monorepo Management

- **Backend**: Uses Visual Studio Solution (TD.sln) with project references
- **Frontend**: Uses npm workspaces defined in root `package.json`

### CI/CD

- Each deployable service has its own GitHub Actions workflow in `.github/workflows/`
- Workflows include separate jobs for tests, builds, and deployments
- UAT tests run via `test-automation-uat-*.yml` workflows
- Every deployable service has a Dockerfile
- Kubernetes manifests are in `k8s/` directory

### Testing Strategy

1. **Backend Unit/Integration Tests**: In `.Tests` projects using standard .NET test frameworks
2. **Frontend Unit Tests**: Jest + React Testing Library in `.Fe` projects
3. **UAT E2E Tests**: Selenium WebDriver in `.Fe.UAT` projects (Chrome and Firefox)
4. **Production Smoke Tests**: In `src/productionTests/`

## Development Best Practices

### Interface-Implementation Pattern

**CRITICAL: Always update interfaces when modifying implementations**

When modifying a Manager or Repository class, you MUST update its corresponding interface first or simultaneously:

1. **Naming Convention**: Interfaces are prefixed with `I`
   - Manager class: `NalogZaPrevozManager` → Interface: `INalogZaPrevozManager`
   - Repository class: `NalogZaPrevozRepository` → Interface: `INalogZaPrevozRepository`

2. **Update Order**:
   - **Step 1**: Update the interface definition in `Contracts/Interfaces/IManagers/` or `Contracts/Interfaces/IRepositories/`
   - **Step 2**: Update the implementation in `Domain/Managers/` or `Repository/`

3. **Common Mistake**:
   - ❌ Updating only the implementation without updating the interface
   - ✅ Always update both interface and implementation together

**Example - Adding a new method:**
```csharp
// STEP 1: Update interface first
// File: TD.Office.Public.Contracts/Interfaces/IManagers/INalogZaPrevozManager.cs
public interface INalogZaPrevozManager
{
    void UpdateStatus(long id, UpdateNalogZaPrevozStatusRequest request);
}

// STEP 2: Update implementation
// File: TD.Office.Public.Domain/Managers/NalogZaPrevozManager.cs
public class NalogZaPrevozManager : INalogZaPrevozManager
{
    public void UpdateStatus(long id, UpdateNalogZaPrevozStatusRequest request)
    {
        // Implementation
    }
}
```

### Adding New Fields to Entities or Creating New Entities

When adding a new field to an entity or entity itself, follow these steps in order:

1. **Update Entity Map**: Configure everything you need within entity map `Repository/EntityMappings/`
   - For fields with default values: Use `.HasDefaultValueSql("value")` for enums/primitives
   - Example: `builder.Property(x => x.Status).IsRequired().HasDefaultValueSql("0");`
4. **Create Migration**: Run `dotnet ef migrations add MigrationName --project [DbMigrations project]`

### Entity Configuration Patterns

**Default Values for Enums:**
```csharp
// In EntityMap
builder.Property(x => x.Status).IsRequired().HasDefaultValueSql("0");
```

**ValueInjecter Mapping:**
- Automatic property mapping works when property names and types match between Entity and DTO
- No explicit mapping configuration needed in most cases
- The mapper uses `ToMapped<TSource, TDest>()` and `ToMappedList<TSource, TDest>()`

### REST API Routing Conventions

**Resource ID in URL Path:**
- Resource identifiers MUST be part of the URL path, not the request body
- Routes CANNOT have two adjoined static parts (e.g., `/resource/action`)
- Routes MUST include the resource ID when operating on a specific resource
- Use GET, PUT, POST and DELETE methods only
- Create or update resource should share same endpoint which will be of type PUT and accept request with nullable Id property. If Id is null, it will create resource, otherwise update it
- Endpoints updating resources property are of PUT method

**Correct Patterns:**
```csharp
// ✅ CORRECT - ID in path
[HttpPut]
[Route("/nalog-za-prevoz/{id}/status")]
public IActionResult UpdateStatus([FromRoute] long id, [FromBody] UpdateStatusRequest request)

[HttpGet]
[Route("/nalog-za-prevoz/{id}")]
public IActionResult GetSingle([FromRoute] LSCoreIdRequest request)
```

**Incorrect Patterns:**
```csharp
// ❌ WRONG - Two adjoined static parts
[HttpPut]
[Route("/nalog-za-prevoz/status")]
public IActionResult UpdateStatus([FromBody] UpdateStatusRequest request)
// Should be: /nalog-za-prevoz/{id}/status

// ❌ WRONG - ID in body instead of path
public class UpdateStatusRequest {
    public long Id { get; set; }  // Should be in URL path
    public Status Status { get; set; }
}
```

### Git Commit Workflow

**IMPORTANT: Never commit changes automatically**

When working with Claude Code on this project:

1. **Wait for Explicit Request**: Only create git commits when explicitly requested by the user
2. **Inform Before Committing**: After completing a task with changes, inform the user that changes are ready and wait for their instruction
3. **Ask if Unclear**: If it's uncertain whether a commit is needed, ask the user for confirmation
4. **User Controls Commits**: The user decides when and what to commit
5. **No Claude Attribution**: Never include "Claude" or "Co-Authored-By: Claude" in commit messages

**Examples:**

✅ **Correct - Wait for user request:**
```
Assistant: I've completed the changes to the nalog za prevoz files. The modifications are ready to be committed when you'd like.
User: commit the changes
Assistant: [Creates commit]
```

❌ **Incorrect - Automatic commit:**
```
Assistant: I've completed the changes and committed them automatically.
```

**Exception**: When the user explicitly uses commit-related commands like "quick commit" or "fast commit", proceed with the commit workflow as that is an explicit request.

### TD.Office Permission System

TD.Office uses an enum-based permission system with database persistence. When adding a new permission, modify these files in order:

**Backend Files (3 files):**

1. **Permission Group Constant** - `src/TD.Office/TD.Office.Common/TD.Office.Common.Contracts/LegacyConstants.cs`
   ```csharp
   public static class PermissionGroup
   {
       // Add new group constant (kebab-case string)
       public const string MojModul = "moj-modul";
   }
   ```

2. **Permission Enum** - `src/TD.Office/TD.Office.Common/TD.Office.Common.Contracts/Enums/Permission.cs`
   ```csharp
   // Add to NavBar group for navigation visibility
   // Add to module-specific group for granular control
   [PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
   [PermissionGroup(LegacyConstants.PermissionGroup.MojModul)]
   [Description("Moj Modul - pristup modulu")]
   MojModulRead,
   ```

3. **Controller Protection** - Apply `[Permissions]` attribute to controller
   ```csharp
   [LSCoreAuth]
   [Permissions(Permission.Access, Permission.MojModulRead)]
   public class MojModulController : ControllerBase
   ```

**Frontend Files (2 files):**

4. **Permission Constants** - `src/TD.Office/TD.Office.Public/TD.Office.Public.Fe/src/constants/permissions/permissionsConstants.js`
   ```javascript
   PERMISSIONS_GROUPS: {
       MOJ_MODUL: 'moj-modul',  // Must match backend kebab-case
   },
   USER_PERMISSIONS: {
       MOJ_MODUL: {
           READ: 'MojModulRead',  // Must match backend enum name
       },
   }
   ```

5. **Navigation Menu** - `src/TD.Office/TD.Office.Public/TD.Office.Public.Fe/src/constants/navBar/navBarConstants.js`
   ```javascript
   {
       label: NAV_BAR_CONSTANTS.MODULE_LABELS.MOJ_MODUL,
       href: URL_CONSTANTS.MOJ_MODUL.INDEX,
       hasPermission: hasPermission(
           permissions,
           PERMISSIONS_CONSTANTS.USER_PERMISSIONS.MOJ_MODUL.READ
       ),
       icon: <SomeIcon />,
   }
   ```

**How Permission System Works:**
- `UserPermissionEntity` stores user-permission relationships in database
- `[Permissions(...)]` attribute on controllers validates access via LSCore middleware
- Frontend fetches permissions via `GET /permissions/{group}` API
- `usePermissions(group)` hook fetches permissions, `hasPermission()` helper checks them
- Navigation menu filters items where `hasPermission === false`

**Key Files Reference:**
- Permission enum: `TD.Office.Common.Contracts/Enums/Permission.cs`
- Permission groups: `TD.Office.Common.Contracts/LegacyConstants.cs`
- User-permission entity: `TD.Office.Common.Contracts/Entities/UserPermissionEntity.cs`
- Permissions controller: `TD.Office.Public.Api/Controllers/PermissionsController.cs`
- FE permission hook: `TD.Office.Public.Fe/src/hooks/usePermissionsHook.ts`
- FE permission helper: `TD.Office.Public.Fe/src/helpers/permissionsHelpers.ts`

### API Endpoint Permission Protection (CRITICAL)

**EVERY controller endpoint MUST be protected with appropriate permissions.** This is a security requirement that must never be skipped.

**Permission levels:**
- **Class-level `[Permissions]`**: Applied to all endpoints in the controller (typically READ permission)
- **Method-level `[Permissions]`**: Additional permission for specific operations (typically WRITE permissions)

**IMPORTANT: Write operations (PUT, POST, DELETE) that modify data MUST have separate WRITE permissions, not just READ permissions.**

**Correct pattern:**
```csharp
[LSCoreAuth]
[ApiController]
[Permissions(Permission.Access, Permission.ModuleRead)]  // Class-level: applies to all endpoints
public class MyController : ControllerBase
{
    [HttpGet]
    [Route("/my-resource")]
    public IActionResult GetMultiple() => Ok(manager.GetMultiple());  // Uses class-level READ permission

    [HttpGet]
    [Route("/my-resource/{Id}")]
    public IActionResult GetSingle([FromRoute] LSCoreIdRequest request) => Ok(manager.GetSingle(request));  // Uses class-level READ permission

    [HttpPut]
    [Route("/my-resource")]
    [Permissions(Permission.ModuleWrite)]  // Method-level: REQUIRES WRITE permission
    public IActionResult Save([FromBody] SaveRequest request)
    {
        manager.Save(request);
        return Ok();
    }

    [HttpDelete]
    [Route("/my-resource/{id}")]
    [Permissions(Permission.ModuleWrite)]  // Method-level: REQUIRES WRITE permission
    public IActionResult Delete([FromRoute] long id)
    {
        manager.Delete(id);
        return Ok();
    }
}
```

**Common mistakes to AVOID:**
```csharp
// ❌ WRONG - No write permission on PUT/DELETE
[HttpPut]
[Route("/my-resource")]
public IActionResult Save([FromBody] SaveRequest request)  // Anyone with READ can modify data!

// ❌ WRONG - Only class-level READ permission for write operations
[Permissions(Permission.Access, Permission.ModuleRead)]
public class MyController
{
    [HttpDelete]
    [Route("/my-resource/{id}")]
    public IActionResult Delete(long id)  // Only needs READ to delete!
}
```

**Checklist when creating controllers:**
1. ✅ Add `[LSCoreAuth]` attribute to require authentication
2. ✅ Add class-level `[Permissions(Permission.Access, Permission.ModuleRead)]` for READ operations
3. ✅ Add method-level `[Permissions(Permission.ModuleWrite)]` to ALL PUT/POST/DELETE methods
4. ✅ If module has different write levels (e.g., `Write` vs `EditAll`), use appropriate permission for each endpoint

### Print Pages Pattern

When creating print-optimized pages in Next.js (e.g., `/pages/print/[feature]/[id].jsx`):

1. **Don't reuse editing widgets** - Widget components (in `widgets/`) are designed for interactive editing with MUI inputs, buttons, dialogs. They are too heavy for print pages.

2. **Use lightweight inline components** - Create simple render functions with inline styles or a `printStyles` object:
   ```javascript
   const printStyles = {
       page: { fontFamily: 'Arial', fontSize: '10px', padding: '10mm' },
       table: { width: '100%', borderCollapse: 'collapse', fontSize: '9px' },
       // ...
   }

   const renderSection = () => (
       <div style={printStyles.section}>
           <table style={printStyles.table}>...</table>
       </div>
   )
   ```

3. **Use CSS Grid for multi-column layouts** - Fits content on A4:
   ```javascript
   columns: { display: 'grid', gridTemplateColumns: '1fr 1fr 1fr', gap: '10px' }
   ```

4. **Handle number formatting safely** - API responses may return string values instead of numbers. Always convert before using `formatNumber`:
   ```javascript
   const safeFormat = (value) => {
       const num = Number(value)
       return formatNumber(isNaN(num) ? 0 : num)
   }
   ```

5. **Print page location**: `src/TD.Office/TD.Office.Public/TD.Office.Public.Fe/src/pages/print/`

### SpecifikacijaNovca Data Structure

Key properties from the API response (`ENDPOINTS_CONSTANTS.SPECIFIKACIJA_NOVCA.GET(id)`):

```javascript
{
    id: number,
    magacinId: string,
    datumUTC: string,
    komentar: string,
    racunar: {
        gotovinskiRacuni: number,
        virmanskiRacuni: number,
        kartice: number,
        racunarTrazi: string,        // Formatted label (e.g., "123,456.00 RSD")
        racunarTraziValue: number,   // Numeric value for calculations
        gotovinskePovratnice: number,
        virmanskePovratnice: number,
        ostalePovratnice: number,
        imaNefiskalizovanih: boolean
    },
    poreska: {
        fiskalizovaniRacuni: number,
        fiskalizovanePovratnice: number
    },
    specifikacijaNovca: {
        novcanice: [{ key: number, value: number }],  // key = denomination, value = count
        eur1: { komada: number, kurs: number },
        eur2: { komada: number, kurs: number },
        ostalo: [{ key: string, vrednost: number, komentar: string }]
    }
}
```

**Helper functions**: `src/TD.Office/TD.Office.Public/TD.Office.Public.Fe/src/widgets/SpecifikacijaNovca/helpers/SpecifikacijaHelpers.js`
- `getUkupnoGotovine(specifikacija)` - Calculates total cash from novcanice array
- `encryptSpecifikacijaNovcaId(id)` / `decryptSpecifikacijaNovcaId(encryptedId)` - ID obfuscation for URLs

### LSCore Repository Methods

**SoftDelete - Use the base class method:**

`LSCoreRepositoryBase<T>` already provides a `SoftDelete(long id)` method that sets `IsActive = false` and calls `Update()`. Do NOT create custom SoftDelete methods in repository implementations - they will hide the base method.

```csharp
// ✅ CORRECT - Use base method directly in manager
public void Delete(long id)
{
    repository.SoftDelete(id);  // Uses LSCoreRepositoryBase.SoftDelete
}

// ❌ WRONG - Don't create custom SoftDelete in repository
public class MyRepository : LSCoreRepositoryBase<MyEntity>
{
    public void SoftDelete(long id)  // This hides the base method!
    {
        var entity = Get(id);
        entity.IsActive = false;
        Update(entity);
    }
}
```

**Available base repository methods:**
- `Get(long id)` - Get entity by ID
- `GetMultiple()` - Get all active entities
- `Insert(entity)` - Insert new entity
- `Update(entity)` - Update existing entity
- `SoftDelete(long id)` - Set IsActive = false
- `HardDelete(long id)` - Permanently delete from database

### DTO Mapping Pattern

Use `ILSCoreMapper<TEntity, TDto>` interface for entity-to-DTO mappings. Mappers are placed in `Contracts/DtosMappings/[Feature]/` folder.

**Creating a mapper:**
```csharp
// File: TD.Office.Public.Contracts/DtosMappings/MyFeature/MyEntityDtoMapper.cs
using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;

public class MyEntityDtoMapper : ILSCoreMapper<MyEntity, MyDto>
{
    public MyDto ToMapped(MyEntity sender)
    {
        var dto = new MyDto();
        dto.InjectFrom(sender);  // Auto-maps matching properties
        // Manually map navigation properties or computed fields
        dto.RelatedName = sender.RelatedEntity?.Name ?? string.Empty;
        return dto;
    }
}
```

**Using mappers in managers:**
```csharp
using LSCore.Mapper.Domain;

// Single entity
var dto = entity.ToMapped<MyEntity, MyDto>();

// List of entities
var dtos = entities.ToMappedList<MyEntity, MyDto>();
```

### MUI Date Pickers with date-fns v3

When using `@mui/x-date-pickers` with `date-fns` v3.x, use the V3 adapter:

```javascript
// ✅ CORRECT - For date-fns v3
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFnsV3'
import { srLatn } from 'date-fns/locale/sr-Latn'

// ❌ WRONG - For date-fns v2 only
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns'
import { srLatn } from 'date-fns/locale'
```

### Frontend Click Handling for Grid/Calendar Components

When a grid cell can contain clickable items (chips, buttons), separate cell clicks from item clicks:

```javascript
// Day component with separate handlers
export const CalendarDay = ({ date, items, onCellClick, onItemClick, canEdit }) => {
    const handleCellClick = () => {
        if (onCellClick && date && canEdit) {
            onCellClick(date)  // Create new item
        }
    }

    const handleItemClick = (e, item) => {
        e.stopPropagation()  // Prevent cell click
        if (onItemClick) {
            onItemClick(item)  // Edit existing item
        }
    }

    return (
        <Box onClick={handleCellClick}>
            {items.map((item) => (
                <Chip
                    key={item.id}
                    onClick={(e) => handleItemClick(e, item)}
                />
            ))}
        </Box>
    )
}
```

This pattern ensures:
- Clicking the cell background creates a new item
- Clicking an item chip edits that specific item
- `e.stopPropagation()` prevents the cell click from firing when clicking an item

### TD.Office Database Migrations

For TD.Office.Common migrations, use the DbMigrations project as both project and startup project:

```bash
# Add migration
dotnet ef migrations add MigrationName \
    --project src/TD.Office/TD.Office.Common/TD.Office.Common.DbMigrations \
    --startup-project src/TD.Office/TD.Office.Common/TD.Office.Common.DbMigrations

# Apply migration
dotnet ef database update \
    --project src/TD.Office/TD.Office.Common/TD.Office.Common.DbMigrations \
    --startup-project src/TD.Office/TD.Office.Common/TD.Office.Common.DbMigrations
```

**Adding seed data in migrations:**
```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    // Create tables first...

    // Then seed data
    migrationBuilder.InsertData(
        table: "MyTable",
        columns: new[] { "Id", "Name", "IsActive", "CreatedAt", "CreatedBy" },
        values: new object[,]
        {
            { 1L, "Value1", true, DateTime.UtcNow, 0L },
            { 2L, "Value2", true, DateTime.UtcNow, 0L },
        });
}
```

## Important Notes

- **LSCore Framework**: Understanding LSCore patterns is essential. It provides Repository, Validation, Mapper, Auth, and ApiClient abstractions used throughout the codebase.
- **Consistent Structure**: Once you understand one domain (e.g., TD.Komercijalno), you understand them all. The layering pattern is extremely consistent.
- **Microservices in Monorepo**: Services are logically separated but share code via libraries, enabling independent deployment while maximizing code reuse.
- **Legacy Support**: The `Old/` directory and Firebird-based systems (TD.TDOffice) are legacy codebases being migrated but still maintained.
- **Dual-Language**: Backend is .NET, frontend is Next.js/React. They communicate exclusively via REST APIs.
