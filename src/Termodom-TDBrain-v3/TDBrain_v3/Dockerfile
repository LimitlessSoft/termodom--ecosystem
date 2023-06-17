#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TDBrain_v3/TDBrain_v3.csproj", "TDBrain_v3/"]
RUN dotnet restore "TDBrain_v3/TDBrain_v3.csproj"
COPY . .
WORKDIR "/src/TDBrain_v3"
RUN dotnet build "TDBrain_v3.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TDBrain_v3.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TDBrain_v3.dll"]