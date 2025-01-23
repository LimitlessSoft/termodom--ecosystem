FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TD.Office/TD.Office.InterneOtpremnice.DbMigrations/TD.Office.InterneOtpremnice.DbMigrations.csproj", "src/TD.Office/TD.Office.InterneOtpremnice.DbMigrations/"]
RUN dotnet restore "src/TD.Office/TD.Office.InterneOtpremnice.DbMigrations/TD.Office.InterneOtpremnice.DbMigrations.csproj"
COPY . .
WORKDIR "/src/src/TD.Office/TD.Office.InterneOtpremnice.DbMigrations"
RUN dotnet build "TD.Office.InterneOtpremnice.DbMigrations.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TD.Office.InterneOtpremnice.DbMigrations.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TD.Office.InterneOtpremnice.DbMigrations.dll"]
