# Petopia — Pet Adoption Platform Backend

The REST API backend for the Petopia pet adoption platform, built with ASP.NET Core.

## Local development

Environment setup (infrastructure via Docker Compose, configuration, migrations, seeding) and run instructions for every Petopia service live in the umbrella repository:

**https://github.com/Aaron-24DucAnh24/Petopia#readme**

This repository doesn't ship its own `docker-compose.yml`; the shared one in the umbrella repo starts SQL Server, Redis, MinIO and MeiliSearch.

## Tech Stack

| Category        | Technology                  |
| --------------- | --------------------------- |
| API framework   | ASP.NET Core 7.0            |
| ORM             | Entity Framework Core 7.0   |
| Database        | SQL Server (Azure SQL Edge) |
| Cache           | Redis                       |
| File storage    | MinIO                       |
| Search          | MeiliSearch                 |
| Background jobs | Hangfire                    |

## Related Projects

| Project      | Repository                                           |
| ------------ | ---------------------------------------------------- |
| Umbrella     | https://github.com/Aaron-24DucAnh24/Petopia          |
| Front Office | https://github.com/Aaron-24DucAnh24/Petopia-Frontend |
| Chat Service | https://github.com/Aaron-24DucAnh24/Petopia-Chat     |
