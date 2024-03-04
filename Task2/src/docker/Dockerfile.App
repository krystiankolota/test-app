#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WeatherDisplayer.Web/WeatherDisplayer.Web.csproj", "WeatherDisplayer.Web/"]
RUN dotnet restore "./WeatherDisplayer.Web/./WeatherDisplayer.Web.csproj"
COPY . .
WORKDIR "/src/WeatherDisplayer.Web"
RUN dotnet build "./WeatherDisplayer.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WeatherDisplayer.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherDisplayer.Web.dll"]