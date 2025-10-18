# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Application/Services/FlixHub.Api/FlixHub.Api.csproj", "Application/Services/FlixHub.Api/"]
COPY ["Application/Services/FlixHub.Core.Api/FlixHub.Core.Api.csproj", "Application/Services/FlixHub.Core.Api/"]
COPY ["Application/Services/FlixHub.Keycloak.Api/FlixHub.Keycloak.Api.csproj", "Application/Services/FlixHub.Keycloak.Api/"]
COPY ["Shared/FlixHub.Resources/FlixHub.Resources.csproj", "Shared/FlixHub.Resources/"]
COPY ["Shared/FlixHub.Notification/FlixHub.Notification.csproj", "Shared/FlixHub.Notification/"]
COPY ["Shared/FlixHub.Shared/FlixHub.Shared.csproj", "Shared/FlixHub.Shared/"]
COPY ["Shared/FlixHub.Caching/FlixHub.Caching.csproj", "Shared/FlixHub.Caching/"]
COPY ["Shared/FlixHub.Core/FlixHub.Core.csproj", "Shared/FlixHub.Core/"]
RUN dotnet restore "./Application/Services/FlixHub.Api/FlixHub.Api.csproj"
COPY . .
WORKDIR "/src/Application/Services/FlixHub.Api"
RUN dotnet build "./FlixHub.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FlixHub.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlixHub.Api.dll"]
