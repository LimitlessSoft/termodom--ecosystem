FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
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
Run ls
COPY --from=publish /app/publish .
Run ls
ENTRYPOINT ["dotnet", "TD.Web.Public.Api.dll"]