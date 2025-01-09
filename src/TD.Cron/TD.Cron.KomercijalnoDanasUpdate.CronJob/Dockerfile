FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TD.Cron/TD.Cron.KomercijalnoDanasUpdate.CronJob/TD.Cron.KomercijalnoDanasUpdate.CronJob.csproj", "src/TD.Cron/TD.Cron.KomercijalnoDanasUpdate.CronJob/"]
RUN dotnet restore "src/TD.Cron/TD.Cron.KomercijalnoDanasUpdate.CronJob/TD.Cron.KomercijalnoDanasUpdate.CronJob.csproj"
COPY . .
WORKDIR "/src/src/TD.Cron/TD.Cron.KomercijalnoDanasUpdate.CronJob"
RUN dotnet build "TD.Cron.KomercijalnoDanasUpdate.CronJob.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TD.Cron.KomercijalnoDanasUpdate.CronJob.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TD.Cron.KomercijalnoDanasUpdate.CronJob.dll"]
