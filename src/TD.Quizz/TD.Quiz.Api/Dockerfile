#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/TD.Quizz/TD.Quiz.Api/TD.Quiz.Api.csproj", "src/TD.Quizz/TD.Quiz.Api/"]
RUN dotnet restore "src/TD.Quizz/TD.Quiz.Api/TD.Quiz.Api.csproj"
COPY . .
WORKDIR "/src/src/TD.Quizz/TD.Quiz.Api"
RUN dotnet build "TD.Quiz.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TD.Quiz.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TD.Quiz.Api.dll"]