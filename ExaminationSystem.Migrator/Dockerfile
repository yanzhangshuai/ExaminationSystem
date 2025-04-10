FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 4316

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ExaminationSystem.Migrator/ExaminationSystem.Migrator.csproj", "ExaminationSystem.Migrator/"]
COPY ["ExaminationSystem.Model/ExaminationSystem.Model.csproj","ExaminationSystem.Model/"]
COPY ["ExaminationSystem.EntityFrameworkCore/ExaminationSystem.EntityFrameworkCore.csproj","ExaminationSystem.EntityFrameworkCore/"]

RUN dotnet restore "ExaminationSystem.Migrator/ExaminationSystem.Migrator.csproj"
COPY . .
WORKDIR "/src/ExaminationSystem.Migrator"
RUN dotnet build "ExaminationSystem.Migrator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ExaminationSystem.Migrator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExaminationSystem.Migrator.dll"]
