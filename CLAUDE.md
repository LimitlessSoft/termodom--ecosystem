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

## Important Notes

- **LSCore Framework**: Understanding LSCore patterns is essential. It provides Repository, Validation, Mapper, Auth, and ApiClient abstractions used throughout the codebase.
- **Consistent Structure**: Once you understand one domain (e.g., TD.Komercijalno), you understand them all. The layering pattern is extremely consistent.
- **Microservices in Monorepo**: Services are logically separated but share code via libraries, enabling independent deployment while maximizing code reuse.
- **Legacy Support**: The `Old/` directory and Firebird-based systems (TD.TDOffice) are legacy codebases being migrated but still maintained.
- **Dual-Language**: Backend is .NET, frontend is Next.js/React. They communicate exclusively via REST APIs.
