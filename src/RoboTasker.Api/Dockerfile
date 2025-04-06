FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

RUN mkdir -p /app/storage && chown -R app:app /app/storage

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/RoboTasker.Api/RoboTasker.Api.csproj", "src/RoboTasker.Api/"]
COPY ["src/RoboTasker.Infrastructure/RoboTasker.Infrastructure.csproj", "src/RoboTasker.Infrastructure/"]
COPY ["src/RoboTasker.Application/RoboTasker.Application.csproj", "src/RoboTasker.Application/"]
COPY ["src/RoboTasker.Domain/RoboTasker.Domain.csproj", "src/RoboTasker.Domain/"]
RUN dotnet restore "src/RoboTasker.Api/RoboTasker.Api.csproj"
COPY . .
WORKDIR "/src/src/RoboTasker.Api"
RUN dotnet build "RoboTasker.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RoboTasker.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RoboTasker.Api.dll"]
