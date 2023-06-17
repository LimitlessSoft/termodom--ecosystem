#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/TD.Komercijalno/TD.Komercijalno.Api/TD.Komercijalno.Api.csproj", "src/TD.Komercijalno/TD.Komercijalno.Api/"]
COPY ["src/TD.Core/TD.Core.Framework/TD.Core.Framework.csproj", "src/TD.Core/TD.Core.Framework/"]
COPY ["src/TD.Core/TD.Core.Contracts/TD.Core.Contracts.csproj", "src/TD.Core/TD.Core.Contracts/"]
COPY ["src/TD.Core/TD.Core.Domain/TD.Core.Domain.csproj", "src/TD.Core/TD.Core.Domain/"]
COPY ["src/TD.Core/TD.Core.Repository/TD.Core.Repository.csproj", "src/TD.Core/TD.Core.Repository/"]
COPY ["src/TD.Komercijalno/TD.Komercijalno.Domain/TD.Komercijalno.Domain.csproj", "src/TD.Komercijalno/TD.Komercijalno.Domain/"]
COPY ["src/TD.Komercijalno/TD.Komercijalno.Contracts/TD.Komercijalno.Contracts.csproj", "src/TD.Komercijalno/TD.Komercijalno.Contracts/"]
COPY ["src/TD.Komercijalno/TD.Komercijalno.Repository/TD.Komercijalno.Repository.csproj", "src/TD.Komercijalno/TD.Komercijalno.Repository/"]
RUN dotnet restore "src/TD.Komercijalno/TD.Komercijalno.Api/TD.Komercijalno.Api.csproj"
COPY . .
WORKDIR "/src/src/TD.Komercijalno/TD.Komercijalno.Api"
RUN dotnet build "TD.Komercijalno.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TD.Komercijalno.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TD.Komercijalno.Api.dll"]