# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy .csproj files
COPY src/ItemMicroservice.Presentation/ItemMicroservice.Presentation.csproj ./ItemMicroservice.Presentation/
COPY src/ItemMicroservice.Application/ItemMicroservice.Application.csproj ./ItemMicroservice.Application/
COPY src/ItemMicroservice.Infrastructure/ItemMicroservice.Infrastructure.csproj ./ItemMicroservice.Infrastructure/
COPY src/ItemMicroservice.Domain/ItemMicroservice.Domain.csproj ./ItemMicroservice.Domain/

# Restore
RUN dotnet restore ./ItemMicroservice.Presentation/ItemMicroservice.Presentation.csproj

# Copy everything
COPY src/ ./ 

# Publish
WORKDIR /src/ItemMicroservice.Presentation
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ItemMicroservice.Presentation.dll"]
