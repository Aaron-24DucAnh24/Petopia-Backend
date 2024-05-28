# Build stage

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /source

COPY . .

WORKDIR /source/Petopia.Data

RUN dotnet new tool-manifest

RUN dotnet tool install --local dotnet-ef --version 7.0.11

RUN dotnet ef migrations add Init

RUN dotnet ef database update

WORKDIR /source

RUN dotnet restore "./Petopia.API/Petopia.API.csproj" --disable-parallel

RUN dotnet publish "./Petopia.API/Petopia.API.csproj" -c release -o /app --no-restore

# Execute stage

FROM mcr.microsoft.com/dotnet/sdk:7.0

WORKDIR /app

COPY --from=build /app ./

EXPOSE 8888

EXPOSE 9999

CMD ["dotnet", "Petopia.API.dll"]