# Clinical Trials API

A .NET 8 RESTful API for managing clinical trials metadata, built following Clean Architecture principles.

## Project Structure
- **DPuchkovTestTask.API** - REST API endpoints and controllers
- **DPuchkovTestTask.Application** - Application business logic and use cases
- **DPuchkovTestTask.Domain** - Domain entities and business rules
- **DPuchkovTestTask.Infrastructure** - External concerns and implementations
- **DPuchkovTestTask.IntegrationTests** - Integration tests for the API

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop/)
- [Docker Compose](https://docs.docker.com/compose/install/)

## Getting Started

### Running with Docker Compose
1. Clone the repository:
```bash
git clone https://github.com/yourusername/dpuchkov-4create.git
cd dpuchkov-4create
```
2. Start the application and database:
```bash
docker compose up
```
3. The API will be available at `http://localhost:8080`.

### Running Locally

1. Install dependencies:
```bash
dotnet restore
```
2. Update the connection string in `appsettings.json` to point to your local PostgreSQL instance

3. Run the application:
```bash
cd src/DPuchkovTestTask.API
dotnet run
```

## Running Tests
```bash
dotnet test
```

## API Documentation
Once the application is running, you can access the Swagger documentation at:
- Docker: `http://localhost:8080/swagger`
- Local: `http://localhost:5000/swagger`

## Environment Variables
The application uses the following environment variables:
- `ASPNETCORE_ENVIRONMENT`: Application environment (Development/Production)
- `ConnectionStrings__DefaultConnection`: PostgreSQL connection string

## Docker Configuration
- The API runs on port 8080
- PostgreSQL database runs on port 5432
- Default database credentials:
  - Database: ClinicalTrialsDb
  - Username: postgres
  - Password: postgres

## Project Features
- Clean Architecture implementation
- REST API endpoints for clinical trials management
- JSON schema validation
- PostgreSQL database integration
- Swagger documentation
- Docker containerization
- Integration tests

## Technologies Used
- .NET 8
- Entity Framework Core
- PostgreSQL
- Docker
- Swagger/OpenAPI
- xUnit (for testing)

## Author
Dmytro Puchkov