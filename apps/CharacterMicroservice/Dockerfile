FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 1) Copy your solution file
COPY apps/CharacterMicroservice/CharacterMicroservice.sln ./

# 2) Copy SharedLibraries into container root
COPY SharedLibraries/ /SharedLibraries/

# 3) Copy your service code
COPY apps/CharacterMicroservice/src/ src/

# 4) Now restore will see ../SharedLibraries/MessageBroker/MessageBroker.csproj
RUN dotnet restore CharacterMicroservice.sln

# 5) Publish only the Presentation project
RUN dotnet publish src/CharacterMicroservice.Presentation/CharacterMicroservice.Presentation.csproj \
    -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /publish ./
EXPOSE 8080
ENTRYPOINT ["dotnet", "CharacterMicroservice.Presentation.dll"]
