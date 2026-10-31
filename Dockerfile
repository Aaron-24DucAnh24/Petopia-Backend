# syntax=docker/dockerfile:1

# --- Build -------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Restore first, from the project files only, so the layer is cached until a
# dependency changes.
COPY Petopia.API/Petopia.API.csproj Petopia.API/
COPY Petopia.BackgroundJobs/Petopia.BackgroundJobs.csproj Petopia.BackgroundJobs/
COPY Petopia.Business/Petopia.Business.csproj Petopia.Business/
COPY Petopia.DataLayer/Petopia.DataLayer.csproj Petopia.DataLayer/
COPY Petopia.Data/Petopia.Data.csproj Petopia.Data/
RUN dotnet restore Petopia.API/Petopia.API.csproj

COPY . .
RUN dotnet publish Petopia.API/Petopia.API.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish \
    /p:UseAppHost=false

# --- Runtime -----------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

# Serilog writes to Logs/ under the working directory, so it must be writable
# by the non-root user.
RUN groupadd --system --gid 1000 petopia \
    && useradd --system --uid 1000 --gid petopia --no-create-home petopia \
    && mkdir Logs \
    && chown petopia:petopia Logs

COPY --from=build /app/publish .

# Configuration is not baked into the image. Provide it at deploy time with
# env vars (e.g. ConnectionStrings__Database) or a mounted
# appsettings.<ASPNETCORE_ENVIRONMENT>.json. The listen address is set through
# the Kestrel key (not ASPNETCORE_URLS) because env vars override appsettings,
# so a mounted file that binds 127.0.0.1 can't make the API unreachable.
ENV Kestrel__Endpoints__Http__Url=http://+:9999 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_RUNNING_IN_CONTAINER=true

USER petopia
EXPOSE 9999
ENTRYPOINT ["dotnet", "Petopia.API.dll"]
