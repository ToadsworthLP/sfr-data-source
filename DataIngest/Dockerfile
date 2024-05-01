# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./DataIngest.csproj" --disable-parallel
RUN dotnet publish "./DataIngest.csproj" -c release -o /app --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["dotnet", "DataIngest.dll", "localhost:9192,localhost:9292,localhost:9392", "localhost:8081", "ingest", "openmeteo", "weatherapi", "Host=localhost:5432; Database=postgres; Username=postgres; Password=password", "10"]