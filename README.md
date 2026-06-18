# Petopia — Pet Adoption Platform Backend

The REST API backend for the Petopia pet adoption platform, built with ASP.NET Core 8.

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop)
- [dotnet-ef CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

## Getting Started

### 1. Configure environment

Copy the environment template and fill in your values:

```bash
cp .env.production .env
```

Copy the appsettings template and fill in your values:

```bash
cp Petopia.API/appsettings.Production.json Petopia.API/appsettings.Development.json
```

### 2. Start infrastructure services

Starts SQL Server, Redis, MinIO, and MeiliSearch via Docker.

```bash
docker-compose up -d
```

### 3. Create a development HTTPS certificate

```bash
dotnet dev-certs https -ep ./certificate.pfx -p <your-password> --trust
```

### 4. Apply database migrations

```bash
cd Petopia.Data
dotnet tool install --global dotnet-ef   # skip if already installed
dotnet ef database update
```

### 5. Run the API

```bash
cd Petopia.API
dotnet run -e ASPNETCORE_ENVIRONMENT=Development
```

## Service URLs

| Service                     | URL                                       |
| --------------------------- | ----------------------------------------- |
| Swagger (API docs)          | https://127.0.0.1:8888/swagger/index.html |
| MinIO (file storage)        | http://127.0.0.1:9001/browser             |
| MeiliSearch (search engine) | http://127.0.0.1:7700                     |

## Tech Stack

| Category        | Technology                  |
| --------------- | --------------------------- |
| API framework   | ASP.NET Core 8.0            |
| ORM             | Entity Framework Core 8.0   |
| Database        | SQL Server (Azure SQL Edge) |
| Cache           | Redis                       |
| File storage    | MinIO                       |
| Search          | MeiliSearch                 |
| Background jobs | Hangfire                    |

## Related Projects

| Project      | Repository                                           |
| ------------ | ---------------------------------------------------- |
| Front Office | https://github.com/Aaron-24DucAnh24/Petopia-Frontend |
