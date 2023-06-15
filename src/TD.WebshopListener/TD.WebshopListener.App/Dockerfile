#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/TD.WebshopListener/TD.WebshopListener.App/TD.WebshopListener.App.csproj", "src/TD.WebshopListener/TD.WebshopListener.App/"]
RUN dotnet restore "src/TD.WebshopListener/TD.WebshopListener.App/TD.WebshopListener.App.csproj"
COPY . .
WORKDIR "/src/src/TD.WebshopListener/TD.WebshopListener.App"
RUN dotnet build "TD.WebshopListener.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TD.WebshopListener.App.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TD.WebshopListener.App.dll"]