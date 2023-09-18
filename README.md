# Pet Adoption Platform Backend

### Requirements

1. ASP.NET Core 7.0

2. Entity Framework Core 7.0

3. Docker

### Run

1. Install Docker on Linux or WSL

2. Setup database, storage and cache servers

```bash
docker pull mcr.microsoft.com/azure-sql-edge
docker pull mcr.microsoft.com/azure-storage/azurite
docker pull redis
docker run --cap-add SYS_PTRACE -e 'ACCEPT_EULA=1' -e 'MSSQL_SA_PASSWORD=TicketBooking.database.v1' -p 1433:1433 --name azuresql -d mcr.microsoft.com/azure-sql-edge
docker run docker run -p 10000:10000 -p 10001:10001 -p 10002:10002 --name azurite mcr.microsoft.com/azure-storage/azurite
docker run -p 6379:6379 --name redis redis --requirepass "wlPydEzOygwQYh9HVGys9CO9VGoC4Oo7TAzCaBMBRtM="
```

3. Database and storage are running now. If you want to start those servers later, run

```bash
docker start azuresql
docker start azurite
docker start redis
```

4. Create a development HTTPs certificate on your local machine, update "Kestrel" of "app.setting.json"

5. Go to folder "PetAdoption.Data", run

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add init
dotnet ef database update
```

6. To start program within development environment, run by debugger of VSCode or VS. For another way

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

- MS SQL Server 

- Azure storage

- Redis cache
