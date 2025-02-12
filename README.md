# Steal All The Cats API

## Table of Contents

- [Overview](#prerequisites)
- [Technologies Used](#technologies-used)
- [Features](#features)
- [Installation and Setup](#installation-and-setup)
- [Project Layout](#project-layout)
- [Testing](#testing)
- [Docker Setup](#docker-setup)

## Overview
This project is an ASP.NET Core Web API that "steals" 25 cat images from The Cat API (https://thecatapi.com/) and stores them in a Microsoft SQL Server database. It provides endpoints to fetch, store, and retrieve cat images, with pagination and filtering by tags.

## Technologies Used
- ASP.NET Core (.NET 8)
- Entity Framework Core
- Microsoft SQL Server
- HttpClient for API Requests
- Moq & xUnit for Unit Testing

## Features
This service supports the following operations:

- Fetch and store unique cat images from The Cat API.
- Retrieve cat images by ID.
- Paginated retrieval of cat images.
- Search for cats by tags.
- Cache cat images locally
- Background service that periodically updates database with image data
- API documentation via Swagger

## Installation and Setup

### 1. Clone the repository:   
```bash
git clone https://github.com/mouoent/StealAllTheCatsApi.git
cd StealAllTheCats/StealAllTheCats.API
```
### 2. Restore NuGet packages:
```bash
dotnet restore
```
### 3. Configure Environment Variables
#### 1. Keep `appsettings.json` as a template (committed, without secrets).
#### 2. Create `appsettings.Development.json` (ignored by Git, containing real credentials).
#### 3. Ensure `appsettings.Development.json` is added to `.gitignore`.

#### Example `appsettings.json` (Committed template):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "placeholder_connection_string"
  },
  "CatApiConfig": {
    "GetCatsEndpoint": "https://api.thecatapi.com/v1/images/search",
    "Key": "placeholder_api_key"
  }
}
```
#### Example `appsettings.Development.json` (Ignored in Git, contains real credentials):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=your_db;User Id=your_user;Password=your_password;"
  },
  "CatApiConfig": {
    "GetCatsEndpoint": "https://api.thecatapi.com/v1/images/search",
    "Key": "your-real-api-key"
  }
}
```
#### 4. Run Database Migrations
```bash
dotnet ef database update
```
#### 5. Get API key from [The Cat API](https://thecatapi.com/) and insert it into `appsettings.Development.json`

#### 6. Run the Project
```bash
dotnet run
```
The API will be available at `https://localhost:7159` and its documentation at `https://localhost:7159/swagger`

## API Endpoints
| Method | Endpoint | Description |
|--------|---------|-------------|
| **POST** | `/api/cats/fetch` | Fetch 25 new cat images and store them in the database. |
| **GET** | `/api/cats/{id}` | Retrieve a cat by its ID. |
| **GET** | `/api/cats?page=1&pageSize=10` | Retrieve paginated cats. |
| **GET** | `/api/cats-by-tag?tag=playful&page=1&pageSize=10` | Retrieve cats by tag with pagination. |
| **POST** | `/api/cats/cache-images` | Cache cat images locally. |
| **GET** | `/api/cats/{catId}/image` | Retrieve a cat's image. |

## Project Layout

    StealAllTheCats
    ├── docker-compose
    ├── StealAllTheCats.API
    │   ├── Endpoints 
    │   ├── Interfaces
    │   ├── Migrations           
    │   ├── Models           
    │   ├── Persistence           
    │   ├── Repositories           
    │   ├── Services
    │   ├── Dockerfile           
    │   └── Program.cs
    │   └── appsettings.json
    └── StealAllTheCats.Tests
        └── API
            └── CatServiceTests
                └── UniTests
                    ├── CatServiceUnitTestBase
                    └── CatServiceUnitTests                        

- Endpoints: Defines API routes and handlers.
- Interfaces: Contains service/repository contracts.
- Models: Entity and DTO models.
- Repositories: Data access logic.
- Services: Business logic implementation.
- Tests: Unit test suite.

## Testing
To run unit tests, use the following command:
```bash
# Run all tests
dotnet test
```

## Docker Setup 
### (Attempted, but Not Fully Functional)
A Docker setup was attempted for both the SQL database and the API, but issues arose preventing full functionality.
Some key challenges encountered:
- The database container failed to initialize properly with the expected schema.
- Networking issues between the API and the database container.
- Environment variables for connection strings not being recognized inside the containers.
Further refinements are needed to make the Docker implementation production-ready. If you wish to attempt a Docker setup, consider checking logs and ensuring correct network configurations between containers.
### Possible solutions and next steps
- Ensure the SQL Server container initializes correctly with migration scripts.
- Verify network configurations between the API and database containers.
- Use docker-compose to define dependencies and startup order.
- Check environment variable mappings in docker-compose.override.yml.
