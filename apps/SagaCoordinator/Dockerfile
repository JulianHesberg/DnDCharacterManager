# Use .NET SDK for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy and restore dependencies
COPY SagaCoordinator.sln ./
COPY src/ ./src/
RUN dotnet restore

# Build and publish
RUN dotnet publish src/SagaCoordinator.Presentation/SagaCoordinator.Presentation.csproj -c Release -o /publish

# Use lightweight runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "SagaCoordinator.Presentation.dll"]
