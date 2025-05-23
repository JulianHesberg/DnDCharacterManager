FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY apps/ItemMicroservice/ItemMicroservice.sln ./
COPY SharedLibraries/ /SharedLibraries/
COPY apps/ItemMicroservice/src/ src/

RUN dotnet restore ItemMicroservice.sln
RUN dotnet publish src/ItemMicroservice.Presentation/ItemMicroservice.Presentation.csproj \
    -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /publish ./
EXPOSE 8080
ENTRYPOINT ["dotnet", "ItemMicroservice.Presentation.dll"]
