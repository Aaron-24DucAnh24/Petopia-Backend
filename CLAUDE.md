# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run

**Prerequisites:** Docker must be running first.

```bash
# 1. Start infrastructure (SQL Server, Redis, MinIO, MeiliSearch)
docker-compose up

# 2. Apply EF Core migrations (run from Petopia.Data/)
dotnet tool install --global dotnet-ef
dotnet ef database update

# 3. Run the API (run from Petopia.API/)
dotnet run -e ASPNETCORE_ENVIRONMENT=Development
```

**Service URLs:**
- Swagger: `http://127.0.0.1:9999/swagger/index.html`
- MinIO console: `http://127.0.0.1:9001/browser`
- MeiliSearch: `http://127.0.0.1:7700`

There are no test projects in this repository.

## Architecture

Five-layer dependency chain: `Petopia.API` → `Petopia.Business` → `Petopia.DataLayer` → `Petopia.Data`. `Petopia.BackgroundJobs` sits alongside the API and also depends on `Petopia.Business`.

### Layer responsibilities

| Project | Role |
|---|---|
| `Petopia.API` | Controllers, middleware, DI wiring, Swagger config |
| `Petopia.Business` | Services, interfaces, models, validators, AutoMapper profiles |
| `Petopia.BackgroundJobs` | Hangfire jobs (cache warm-up, email, search indexing) |
| `Petopia.DataLayer` | Generic repository pattern; one `IBaseDataLayer<T>` per entity |
| `Petopia.Data` | EF Core `ApplicationDbContext`, entity classes, Fluent API configs, migrations |
| `Petopia.SeedingData` | One-off console app to seed the database |

### BaseService pattern

All business services extend `BaseService` and receive an `IServiceProvider` in their constructor. `BaseService` resolves and exposes:

- `UnitOfWork` — single entry point to all repositories
- `UserContext` — current authenticated user (Id, Email, Role) from JWT claims
- `Mapper` — AutoMapper instance
- `CacheManager` — Redis-backed cache with memory fallback
- `PagingAsync<TResult, TQuery>()` — handles pagination for EF `IQueryable`
- `ListPaging<TResult, TQuery>()` — handles pagination for in-memory `List`

### Unit of Work

`IUnitOfWork` (at `Petopia.Business/Data/`) exposes one property per entity repository and `SaveChange()` / `SaveChangesAsync()`. Always call `SaveChangesAsync()` after mutations; the EF context uses `NoTracking` by default, so entities that need updating must be queried with `.AsTracking()` first.

### Pagination convention

Paginated endpoints accept `PaginationRequestModel` (PageIndex, PageSize, OrderBy). For filtered endpoints use the generic `PaginationRequestModel<TFilter>` which adds a `Filter` property — extract filter values into local variables before building the `IQueryable` to avoid EF Core LINQ parameter translation errors.

### Error handling

Throw `DomainException` for business rule violations. `ExceptionHandlerMiddleware` maps known exception types to HTTP status codes and error codes defined in `DomainErrorCode` (10xxx = auth/user, 11xxx = pet, 13xxx = blog, 14xxx = payment).

### Authentication

JWT Bearer tokens. Access + Refresh tokens both expire in 7 days. The `UserContext` scoped service is populated from JWT claims on each request. Use `[Authorize]` for authenticated routes, `[AllowAnonymous]` for public routes, and `[AdminAuthorize]` / `[OrganizationAuthorize]` for role-restricted routes.

### Adding a new feature

1. Add entity + Fluent API config in `Petopia.Data`; create a migration from `Petopia.Data/`
2. Add data layer interface + implementation in `Petopia.DataLayer`; register in `Petopia.DataLayer/Extensions/ServiceExtension.cs`
3. Expose it on `IUnitOfWork` / `UnitOfWork` in `Petopia.Business/Data/`
4. Add request/response models in `Petopia.Business/Models/`; add AutoMapper mapping in `Petopia.Business/Utils/MappingProfiles.cs`
5. Add service interface + implementation in `Petopia.Business`; register in `Petopia.Business/Extensions/ServiceExtension.cs`
6. Add controller in `Petopia.API/Controllers/`

### External services (configured in `appsettings.Development.json`)

- **SQL Server** — primary database (Azure SQL Edge image in Docker)
- **Redis** — distributed cache; `ICacheManager` falls back to in-memory if unavailable
- **MinIO** — S3-compatible object storage for images (bucket: `image`)
- **MeiliSearch** — full-text search; indices are `pet` and `blog`
- **Hangfire** — background job scheduler backed by SQL Server
- **Braintree** — payment processing
- **Google** — OAuth and reCAPTCHA
