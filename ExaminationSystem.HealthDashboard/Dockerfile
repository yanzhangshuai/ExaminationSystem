FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ExaminationSystem.HealthDashboard/ExaminationSystem.HealthDashboard.csproj", "ExaminationSystem.HealthDashboard/"]
RUN dotnet restore "ExaminationSystem.HealthDashboard/ExaminationSystem.HealthDashboard.csproj"
COPY . .
WORKDIR "/src/ExaminationSystem.HealthDashboard"
RUN dotnet build "./ExaminationSystem.HealthDashboard.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ExaminationSystem.HealthDashboard.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExaminationSystem.HealthDashboard.dll"]
