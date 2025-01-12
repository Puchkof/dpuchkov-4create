# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files first to cache restore
COPY Directory.Build.props .
COPY src/DPuchkovTestTask.API/*.csproj ./src/DPuchkovTestTask.API/
COPY src/DPuchkovTestTask.Application/*.csproj ./src/DPuchkovTestTask.Application/
COPY src/DPuchkovTestTask.Domain/*.csproj ./src/DPuchkovTestTask.Domain/
COPY src/DPuchkovTestTask.Infrastructure/*.csproj ./src/DPuchkovTestTask.Infrastructure/

# Restore only the API project and its dependencies
RUN dotnet restore "src/DPuchkovTestTask.API/DPuchkovTestTask.API.csproj"

# Copy the rest of the source code
COPY src/. ./src/

# Build and publish
RUN dotnet publish "src/DPuchkovTestTask.API/DPuchkovTestTask.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Copy published files from build stage
COPY --from=build /app/publish .

# Create non-root user for security
RUN adduser --disabled-password --gecos "" appuser && chown -R appuser:appuser /app
USER appuser

EXPOSE 8080

ENTRYPOINT ["dotnet", "DPuchkovTestTask.API.dll"] 