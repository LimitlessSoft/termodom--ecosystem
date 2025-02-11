FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/TD.Cron/TD.Cron.WebPublicInactiveUsersCheckup.CronJob/TD.Cron.WebPublicInactiveUsersCheckup.CronJob.csproj", "src/TD.Cron/TD.Cron.WebPublicInactiveUsersCheckup.CronJob/"]
RUN dotnet restore "src/TD.Cron/TD.Cron.WebPublicInactiveUsersCheckup.CronJob/TD.Cron.WebPublicInactiveUsersCheckup.CronJob.csproj"
COPY . .
WORKDIR "/src/src/TD.Cron/TD.Cron.WebPublicInactiveUsersCheckup.CronJob"
RUN dotnet build "TD.Cron.WebPublicInactiveUsersCheckup.CronJob.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TD.Cron.WebPublicInactiveUsersCheckup.CronJob.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TD.Cron.WebPublicInactiveUsersCheckup.CronJob.dll"]
