FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/TD.Office/TD.Office.Public/TD.Office.Public.Api/TD.Office.Public.Api.csproj", "src/TD.Office/TD.Office.Public/TD.Office.Public.Api/"]
RUN dotnet restore "src/TD.Office/TD.Office.Public/TD.Office.Public.Api/TD.Office.Public.Api.csproj"
COPY . .
WORKDIR "/src/src/TD.Office/TD.Office.Public/TD.Office.Public.Api"
RUN dotnet build "TD.Office.Public.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TD.Office.Public.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TD.Office.Public.Api.dll"]