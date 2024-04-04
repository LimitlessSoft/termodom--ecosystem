FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS build
WORKDIR /app

COPY ["src/TD.Web/TD.Web.Cronjobs/TD.Web.Cronjobs.OrderStatusUpdater/TD.Web.Cronjobs.OrderStatusUpdater.csproj", "src/TD.Web/TD.Web.Cronjobs/TD.Web.Cronjobs.OrderStatusUpdater/"]
RUN dotnet restore "src/TD.Web/TD.Web.Cronjobs/TD.Web.Cronjobs.OrderStatusUpdater/TD.Web.Cronjobs.OrderStatusUpdater.csproj"
COPY . .
WORKDIR "/src/TD.Web/TD.Web.Cronjobs/TD.Web.Cronjobs.OrderStatusUpdater"
RUN dotnet build "TD.Web.Cronjobs.OrderStatusUpdater.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TD.Web.Cronjobs.OrderStatusUpdater.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TD.Web.Cronjobs.OrderStatusUpdater.dll"]