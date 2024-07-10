# PETOPIA - Pet Adoption Platform Backend

## This is the introduction to run API server locally on your machine

### Requirements

1. ASP.NET Core 7.0

2. Entity Framework Core 7.0

3. Docker

4. Tensorflow

### Run

1. Install Docker, Docker-compose on Linux, MacOS or WSL on Windows

2. Setup database and cache servers for the first time

```bash
docker-compose up
```

> Database and cache are running now. To start those servers later, run

```bash
docker-compose start
```

> To stop

```bash
docker-compose stop
```

3. Create a development HTTPs certificate on your local machine

```bash
dotnet dev-certs https -ep ./certificate.pfx -p HDJHFNVHYNDKSLFUEJDMF --trust
```

4. Go to folder "Petopia.Data", run

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add Init
dotnet ef database update
```

> Initing essential data, go to folder "Petopia.SeedingData", run

```bash
dotnet run
```

5. To start the program within the development environment, run by the debugger of VSCode or VS. For another way

```bash
  dotnet run -e ASPNETCORE_ENVIRONMENT=Development
```

> Now program is running at <<<https://127.0.0.1:8888>>> and <<<http://127.0.0.1:9999>>> on your local machine.

### Protential improvements

- At the momment, the recognition feature only works on Window machines.

### Tech stack

- ASP.NET Core 7.0

- Entity Framework Core 7.0

- ML.NET

- Docker

- SQL Server database

- Redis cache

### Other projects

- Frontoffice: https://github.com/Aaron-24DucAnh24/Petopia-Frontend.git

- Backoffice: https://github.com/Aaron-24DucAnh24/Petopia-Backoffice.git
