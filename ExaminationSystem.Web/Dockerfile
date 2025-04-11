FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# 构建
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ExaminationSystem.Web/ExaminationSystem.Web.csproj", "ExaminationSystem.Web/"]
COPY ["ExaminationSystem.Application/ExaminationSystem.Application.csproj","ExaminationSystem.Application/"]
COPY ["ExaminationSystem.Model/ExaminationSystem.Model.csproj","ExaminationSystem.Model/"]
COPY ["ExaminationSystem.EntityFrameworkCore/ExaminationSystem.EntityFrameworkCore.csproj","ExaminationSystem.EntityFrameworkCore/"]
RUN dotnet restore "ExaminationSystem.Web/ExaminationSystem.Web.csproj"
COPY . .
WORKDIR "/src/ExaminationSystem.Web"
RUN dotnet build "ExaminationSystem.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 发布
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ExaminationSystem.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExaminationSystem.Web.dll"]
