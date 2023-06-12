#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/TD.WebshopListener.Worker/TD.WebshopListener.Worker.csproj", "src/TD.WebshopListener.Worker/"]
RUN dotnet restore "src/TD.WebshopListener.Worker/TD.WebshopListener.Worker.csproj"
COPY . .
WORKDIR "/src/src/TD.WebshopListener.Worker"
RUN dotnet build "TD.WebshopListener.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TD.WebshopListener.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TD.WebshopListener.Worker.dll"]