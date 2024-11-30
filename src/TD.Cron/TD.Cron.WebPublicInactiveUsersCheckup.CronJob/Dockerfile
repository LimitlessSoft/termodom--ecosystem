FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TD.Cron/TD.Cron.WebPublicInactiveUsersCheckup.CronJob/TD.Cron.WebPublicInactiveUsersCheckup.CronJob.csproj", "src/TD.Cron/TD.Cron.WebPublicInactiveUsersCheckup.CronJob/"]
RUN dotnet restore "src/TD.Cron/TD.Cron.WebPublicInactiveUsersCheckup.CronJob/TD.Cron.WebPublicInactiveUsersCheckup.CronJob.csproj"
COPY . .
WORKDIR "/src/src/TD.Cron/TD.Cron.WebPublicInactiveUsersCheckup.CronJob"
RUN dotnet build "TD.Cron.WebPublicInactiveUsersCheckup.CronJob.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TD.Cron.WebPublicInactiveUsersCheckup.CronJob.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TD.Cron.WebPublicInactiveUsersCheckup.CronJob.dll"]
