FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TD.Office/TD.Office.PregledIUplataPazara/TD.Office.PregledIUplataPazara.Api/TD.Office.PregledIUplataPazara.Api.csproj", "src/TD.Office/TD.Office.PregledIUplataPazara/TD.Office.PregledIUplataPazara.Api/"]
RUN dotnet restore "src/TD.Office/TD.Office.PregledIUplataPazara/TD.Office.PregledIUplataPazara.Api/TD.Office.PregledIUplataPazara.Api.csproj"
COPY . .
WORKDIR "/src/src/TD.Office/TD.Office.PregledIUplataPazara/TD.Office.PregledIUplataPazara.Api"
RUN dotnet build "TD.Office.PregledIUplataPazara.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TD.Office.PregledIUplataPazara.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TD.Office.PregledIUplataPazara.Api.dll"]
