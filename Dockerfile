# ---- BUILD STAGE ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything into container
COPY . .

# Restore dependencies
RUN dotnet restore MongoDockConnector.sln

# Publish the REPL app
RUN dotnet publish DBConnectorReplApp/DBConnectorReplApp.csproj -c Release -o /app/publish

# ---- RUNTIME STAGE ----
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "DBConnectorReplApp.dll"]
