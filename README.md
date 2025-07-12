# PETOPIA - Pet Adoption Platform Backend

## This is the introduction to run API server locally on your machine

### Requirements
1. ASP.NET Core 7.0
2. Entity Framework Core 7.0
3. Docker

### Run

```bash
# Build and run docker containers
docker-compose up
docker-compose start

# Create a dev https certificate
dotnet dev-certs https -ep ./certificate.pfx -p HDJHFNVHYNDKSLFUEJDMF --trust

# Initialize database
dotnet tool install --global dotnet-ef
# // cd Petopia.Data
dotnet ef database update

# Run web API
# // cd ../Petopia.API
dotnet run -e ASPNETCORE_ENVIRONMENT=Development
```

### Management views
1. Web API: https://127.0.0.1:8888/swagger/index.html
2. File storage: http://127.0.0.1:9001/browser
3. Search engine: http://127.0.0.1:7700

### Tech stack
1. ASP.NET Core 7.0
2. Entity Framework Core 7.0
3. Docker
4. SQL Server database
5. Redis cache
6. Minio file storage
7. Meili search

### Other projects
1. Frontoffice: https://github.com/Aaron-24DucAnh24/Petopia-Frontend.git
2. Backoffice: https://github.com/Aaron-24DucAnh24/Petopia-Backoffice.git
