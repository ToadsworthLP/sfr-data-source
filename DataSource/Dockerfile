# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./DataSource.csproj" --disable-parallel
RUN dotnet publish "./DataSource.csproj" -c release -o /app --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["dotnet", "DataSource.dll", "localhost:9192,localhost:9292,localhost:9392", "localhost:8081", "ack-all", "10", "10", "openmeteo", "weatherapi"]