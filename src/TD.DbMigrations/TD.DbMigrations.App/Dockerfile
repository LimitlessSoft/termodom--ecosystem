#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/TD.DbMigrations/TD.DbMigrations.App/TD.DbMigrations.App.csproj", "src/TD.DbMigrations/TD.DbMigrations.App/"]
COPY ["src/TD.Core/TD.Core.Contracts/TD.Core.Contracts.csproj", "src/TD.Core/TD.Core.Contracts/"]
COPY ["src/TD.Core/TD.Core.Framework/TD.Core.Framework.csproj", "src/TD.Core/TD.Core.Framework/"]
COPY ["src/TD.Webshop/TD.Webshop.Contracts/TD.Webshop.Contracts.csproj", "src/TD.Webshop/TD.Webshop.Contracts/"]
COPY ["src/TD.Webshop/TD.Webshop.DataContext/TD.Webshop.DataContext.csproj", "src/TD.Webshop/TD.Webshop.DataContext/"]
RUN dotnet restore "src/TD.DbMigrations/TD.DbMigrations.App/TD.DbMigrations.App.csproj"
COPY . .
WORKDIR "/src/src/TD.DbMigrations/TD.DbMigrations.App"
RUN dotnet build "TD.DbMigrations.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TD.DbMigrations.App.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TD.DbMigrations.App.dll"]