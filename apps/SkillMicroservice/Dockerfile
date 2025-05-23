FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY apps/SkillMicroservice/SkillMicroservice.sln ./
COPY SharedLibraries/ /SharedLibraries/
COPY apps/SkillMicroservice/src/ src/

RUN dotnet restore SkillMicroservice.sln
RUN dotnet publish src/SkillMicroservice.Presentation/SkillMicroservice.Presentation.csproj \
    -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /publish ./
EXPOSE 8080
ENTRYPOINT ["dotnet", "SkillMicroservice.Presentation.dll"]
