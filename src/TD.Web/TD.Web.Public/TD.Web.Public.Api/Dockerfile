FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
Run ls
COPY ["src/TD.Web/TD.Web.Public/TD.Web.Public.Api/TD.Web.Public.Api.csproj", "src/TD.Web/TD.Web.Public/TD.Web.Public.Api/"]
Run ls
RUN dotnet restore "src/TD.Web/TD.Web.Public/TD.Web.Public.Api/TD.Web.Public.Api.csproj"
Run ls
COPY . .
Run ls
WORKDIR "/src/src/TD.Web/TD.Web.Public/TD.Web.Public.Api"
Run ls
RUN dotnet build "TD.Web.Public.Api.csproj" -c Release -o /app/build

FROM build AS publish
Run ls
RUN dotnet publish "TD.Web.Public.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
Run ls
COPY --from=publish /app/publish .
Run ls
ENTRYPOINT ["dotnet", "TD.Web.Public.Api.csproj"]