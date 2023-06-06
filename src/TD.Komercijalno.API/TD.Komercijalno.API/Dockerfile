#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TD.Komercijalno.API/TD.Komercijalno.API.csproj", "TD.Komercijalno.API/"]
RUN dotnet restore "TD.Komercijalno.API/TD.Komercijalno.API.csproj"
COPY . .
WORKDIR "/src/TD.Komercijalno.API"
RUN dotnet build "TD.Komercijalno.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TD.Komercijalno.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TD.Komercijalno.API.dll"]