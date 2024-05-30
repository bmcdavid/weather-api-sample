FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
# https://stackoverflow.com/a/74939792
RUN apk add --no-cache icu-libs icu-data-full tzdata
ENV TZ=America/New_York
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
USER app
EXPOSE 80
EXPOSE 9000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG GIT_BRANCH="NOTSET"
ARG GIT_HASH="NOTSET"
ARG BUILD_NUMBER="NOTSET"
ENV BUILD_CONFIGURATION $BUILD_CONFIGURATION
ENV PATH $PATH:/root/.dotnet/tools # allows global tools to run later

WORKDIR /code
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["NuGet.Config", "."]
COPY [".editorconfig", "."]
COPY ["src/Api/*.csproj", "src/Api/"]
COPY ["src/Contracts/*.csproj", "src/Contracts/"]
RUN dotnet restore src/Api/Api.csproj
COPY ["src/Api/", "src/Api/"]
COPY ["src/Contracts/", "src/Contracts/"]
WORKDIR /code/src/Api
RUN dotnet build Api.csproj --no-restore -c $BUILD_CONFIGURATION -o /app/build -p:TreatWarningsAsErrors=true -p:GIT_BRANCH=${GIT_BRANCH} -p:GIT_HASH=${GIT_HASH} -p:BUILD_NUMBER=${BUILD_NUMBER}

# target test entrypoint with: docker build --target test
FROM build AS test
WORKDIR /code/tests
COPY ["tests/Tests/*.csproj", "Tests/"]
RUN dotnet restore Tests/Tests.csproj
COPY ["tests/Tests/", "Tests/"]

WORKDIR /code/tests/Tests
RUN dotnet build Tests.csproj -c $BUILD_CONFIGURATION --no-restore -p:TreatWarningsAsErrors=true

ENTRYPOINT dotnet test -c $BUILD_CONFIGURATION --no-restore --no-build --logger:trx --collect "XPlat Code Coverage" -p:ExcludeByFile=**/obj/**/*.cs

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish --no-restore /code/src/Api/Api.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Weather.Api.dll"]