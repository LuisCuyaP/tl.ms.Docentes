# Consulte https://aka.ms/customizecontainer para aprender a personalizar su contenedor de depuración y cómo Visual Studio usa este Dockerfile para compilar sus imágenes para una depuración más rápida.

# Esta fase se usa cuando se ejecuta desde VS en modo rápido (valor predeterminado para la configuración de depuración)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Esta fase se usa para compilar el proyecto de servicio
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Docentes/Docentes.api/Docentes.api.csproj", "src/Docentes/Docentes.api/"]
COPY ["src/Docentes/Docentes.Infrastructure/Docentes.Infrastructure.csproj", "src/Docentes/Docentes.Infrastructure/"]
COPY ["src/Docentes/Docentes.Application/Docentes.Application.csproj", "src/Docentes/Docentes.Application/"]
COPY ["src/Docentes/Docentes.Domain/Docentes.Domain.csproj", "src/Docentes/Docentes.Domain/"]
RUN dotnet restore "./src/Docentes/Docentes.api/Docentes.api.csproj"
COPY . .
WORKDIR "/src/src/Docentes/Docentes.api"
RUN dotnet build "./Docentes.api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase se usa para publicar el proyecto de servicio que se copiará en la fase final.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Docentes.api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase se usa en producción o cuando se ejecuta desde VS en modo normal (valor predeterminado cuando no se usa la configuración de depuración)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Docentes.api.dll"]