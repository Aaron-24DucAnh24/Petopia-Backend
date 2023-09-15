# Pet Adoption Platform Backend

### Requirements

1. ASP.NET Core 7.0

2. Entity Framework Core 7.0

3. Docker CLI

### Run

1. Install Docker on Linux or WSL

2. Make sure you stop your local SQL Server database service (if any)

3. Setup database server using Docker

```bash
docker pull mcr.microsoft.com/azure-sql-edge
docker run --cap-add SYS_PTRACE -e 'ACCEPT_EULA=1' -e 'MSSQL_SA_PASSWORD=TicketBooking.database.v1' -p 1433:1433 --name azuresql -d mcr.microsoft.com/azure-sql-edge
```

4. Database is running now. If you want to start database service later , run

```bash
docker start azuresql
```

5. Create a development HTTPs certificate on your local machine, update "Kestrel" of "app.setting.json"

6. Go to folder "PetAdoption.Data", run

```bash
dotnet ef migrations add init
dotnet ef database update
```

7. To start program within development environment, run by debugger of VSCode or VS. For another way

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

- Azure services
