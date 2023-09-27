# PETOPIA - Pet Adoption Platform Backend

### Requirements

1. ASP.NET Core 7.0

2. Entity Framework Core 7.0

3. Docker

### Run

1. Install Docker, Docker-compose on Linux, MacOS or WSL on Windows

2. Setup database, storage and cache servers for the first time

```bash
docker-compose up
```

> Database, storage and cache are running now. If you want to start those servers later, run

```bash
docker start azuresql
docker start azurite
docker start redis
```

3. Create a development HTTPs certificate on your local machine, update "Kestrel" of "appsettings.Development.json"

4. Go to folder "Petopia.Data", run

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add init
dotnet ef database update
```

5. To start the program within the development environment, run by the debugger of VSCode or VS. For another way

- Linux:

```bash
  export ASPNETCORE_ENVIRONMENT=Development
  dotnet run
```

- Windows:

```bash
  set ASPNETCORE_ENVIRONMENT=Development
  dotnet run
```

> Now program is running at <<<https://127.0.0.1:4000>>> on your local machine.

### Tech stack

- ASP.NET Core 7.0

- Entity Framework core 7.0

- Docker

- SQL Server database

- Azure storage

- Redis cache
