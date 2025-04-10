FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TD.Office/TD.Office.MassSMS/TD.Office.MassSMS.Api/TD.Office.MassSMS.Api.csproj", "src/TD.Office/TD.Office.MassSMS/TD.Office.MassSMS.Api/"]
COPY ["src/TD.Common/TD.Common.Vault.DependencyInjection/TD.Common.Vault.DependencyInjection.csproj", "src/TD.Common/TD.Common.Vault.DependencyInjection/"]
COPY ["src/TD.Common/TD.Common.Vault/TD.Common.Vault.csproj", "src/TD.Common/TD.Common.Vault/"]
COPY ["src/TD.Office/TD.Office.MassSMS/TD.Office.MassSMS.Domain/TD.Office.MassSMS.Domain.csproj", "src/TD.Office/TD.Office.MassSMS/TD.Office.MassSMS.Domain/"]
COPY ["src/TD.Office/TD.Office.MassSMS/TD.Office.MassSMS.Contracts/TD.Office.MassSMS.Contracts.csproj", "src/TD.Office/TD.Office.MassSMS/TD.Office.MassSMS.Contracts/"]
COPY ["src/TD.Office/TD.Office.MassSMS/TD.Office.MassSMS.Repository/TD.Office.MassSMS.Repository.csproj", "src/TD.Office/TD.Office.MassSMS/TD.Office.MassSMS.Repository/"]
RUN dotnet restore "src/TD.Office/TD.Office.MassSMS/TD.Office.MassSMS.Api/TD.Office.MassSMS.Api.csproj"
COPY . .
WORKDIR "/src/src/TD.Office/TD.Office.MassSMS/TD.Office.MassSMS.Api"
RUN dotnet build "TD.Office.MassSMS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TD.Office.MassSMS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TD.Office.MassSMS.Api.dll"]
