# Build stage

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /source

ENV ASPNETCORE_ENVIRONMENT=Production

COPY . .

RUN dotnet restore "./Petopia.API/Petopia.API.csproj" --disable-parallel

RUN dotnet publish "./Petopia.API/Petopia.API.csproj" -c release -o /app --no-restore

# Execute stage

FROM mcr.microsoft.com/dotnet/sdk:7.0

WORKDIR /app

COPY --from=build /app ./

EXPOSE 80

CMD dotnet Petopia.API.dll --urls https://0.0.0.0:80