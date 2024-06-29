FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/TD.Web/TD.Web.Admin/TD.Web.Admin.Api/TD.Web.Admin.Api.csproj", "src/TD.Web/TD.Web.Admin/TD.Web.Admin.Api/"]
RUN dotnet restore "src/TD.Web/TD.Web.Admin/TD.Web.Admin.Api/TD.Web.Admin.Api.csproj"
COPY . .
WORKDIR "/src/src/TD.Web/TD.Web.Admin/TD.Web.Admin.Api"
RUN dotnet build "TD.Web.Admin.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TD.Web.Admin.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TD.Web.Admin.Api.dll"]