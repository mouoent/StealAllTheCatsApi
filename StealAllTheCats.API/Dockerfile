# Use official .NET runtime for running the API
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Set up runtime environment
EXPOSE 8080
ENTRYPOINT ["dotnet", "StealAllTheCats.API.dll"]

# BUILD STAGE
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project files and restore dependencies
COPY StealAllTheCats.API/*.csproj ./
RUN dotnet restore

# Copy the remaining application files
COPY StealAllTheCats.API/. ./
RUN dotnet publish -c Release -o /out --no-restore

# Run the API using runtime image
FROM runtime AS final
WORKDIR /app
COPY --from=build /out .

CMD ["dotnet", "StealAllTheCats.API.dll"]
