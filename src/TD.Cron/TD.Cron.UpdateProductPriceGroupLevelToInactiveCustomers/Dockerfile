FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/TD.Cron/TD.Cron.UpdateProductPriceGroupLevelToInactiveCustomers", "src/TD.Cron/TD.Cron.UpdateProductPriceGroupLevelToInactiveCustomers"]
RUN dotnet restore "src/TD.Cron/TD.Cron.UpdateProductPriceGroupLevelToInactiveCustomers/TD.Cron.UpdateProductPriceGroupLevelToInactiveCustomers.csproj"
COPY . .
WORKDIR "src/TD.Cron/TD.Cron.UpdateProductPriceGroupLevelToInactiveCustomers"
RUN dotnet build "TD.Cron.UpdateProductPriceGroupLevelToInactiveCustomers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TD.Cron.UpdateProductPriceGroupLevelToInactiveCustomers.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TD.Cron.UpdateProductPriceGroupLevelToInactiveCustomers.dll"]