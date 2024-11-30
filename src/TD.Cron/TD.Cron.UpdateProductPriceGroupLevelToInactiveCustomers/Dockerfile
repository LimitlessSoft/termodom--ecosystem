FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/TD.Cron/TD.Cron.UpdateProductPriceGroupLevelToInactiveCustomers/TD.Cron.UpdateProductPriceGroupLevelToInactiveCustomers/TD.Cron.UpdateProductPriceGroupLevelToInactiveCustomers.csproj", "src/TD.Cron/TD.Cron.UpdateProductPriceGroupLevelToInactiveCustomers/"]
RUN dotnet restore "TD.Cron.UpdateProductPriceGroupLevelToInactiveCustomers.csproj"
COPY . .
WORKDIR "/src/src/TD.Cron.UpdateProductPriceGroupLevelToInactiveCustomers"
RUN dotnet publish "TD.Cron.UpdateProductPriceGroupLevelToInactiveCustomers.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TD.Cron.UpdateProductPriceGroupLevelToInactiveCustomers.dll"]