# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/services/Lunches.Api/Lunches.Api.csproj", "src/services/Lunches.Api/"]
COPY ["src/Contracts/Contracts.csproj", "src/Contracts/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/ServiceDefaults/ServiceDefaults.csproj", "src/ServiceDefaults/"]

# the solution is using centralized package management. the Directory.Packages.props file contains the package versions
COPY ["Directory.Packages.props", "."]

# the Directory.Build.props file contains some common build properties, including TargetFramework
COPY ["Directory.Build.props", "Directory.Build.props"]

# this will just restore the packages
RUN dotnet restore "./src/services/Lunches.Api/Lunches.Api.csproj"

# this will copy the rest of the files
COPY . .
WORKDIR "/src/src/services/Lunches.Api"
RUN dotnet build "./Lunches.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Lunches.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lunches.Api.dll"]