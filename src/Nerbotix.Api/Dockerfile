FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

RUN mkdir -p /app/storage && chown -R app:app /app/storage

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Nerbotix.Api/Nerbotix.Api.csproj", "src/Nerbotix.Api/"]
COPY ["src/Nerbotix.Infrastructure/Nerbotix.Infrastructure.csproj", "src/Nerbotix.Infrastructure/"]
COPY ["src/Nerbotix.Application/Nerbotix.Application.csproj", "src/Nerbotix.Application/"]
COPY ["src/Nerbotix.Domain/Nerbotix.Domain.csproj", "src/Nerbotix.Domain/"]
RUN dotnet restore "src/Nerbotix.Api/Nerbotix.Api.csproj"
COPY . .
WORKDIR "/src/src/Nerbotix.Api"
RUN dotnet build "Nerbotix.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Nerbotix.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nerbotix.Api.dll"]
