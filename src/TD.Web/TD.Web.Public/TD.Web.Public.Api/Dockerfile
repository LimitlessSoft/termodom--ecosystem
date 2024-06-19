FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/TD.Web/TD.Web.Public/TD.Web.Public.Api/TD.Web.Public.Api.csproj", "src/TD.Web/TD.Web.Public/TD.Web.Public.Api/"]
RUN dotnet restore "src/TD.Web/TD.Web.Public/TD.Web.Public.Api/TD.Web.Public.Api.csproj"
COPY . .
WORKDIR "/src/src/TD.Web/TD.Web.Public/TD.Web.Public.Api"
RUN dotnet build "TD.Web.Public.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TD.Web.Public.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TD.Web.Public.Api.dll"]