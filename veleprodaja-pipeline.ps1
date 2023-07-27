$MainDir = Get-location

docker stop termodom--web-veleprodaja-api
docker rm termodom--web-veleprodaja-api

cd $MainDir/src/WebApplications/TD.Web.Veleprodaja/TD.Web.Veleprodaja.Api
dotnet build
dotnet publish -o obj/Docker/publish -c Release --runtime linux-x64 --self-contained False
docker build -f ./Dockerfile -t limitlesssoft/termodom--web-veleprodaja-api:$env:BUILD_NUMBER ./obj/Docker/publish
docker run -p 33770:80 -e API_HOST=$env:API_HOST -e JWT_ISSUER=$env:WEB_VELEPRODAJA_JWT_ISSUER -e JWT_AUDIENCE=$env:WEB_VELEPRODAJA_JWT_AUDIENCE -e JWT_KEY=$env:WEB_VELEPRODAJA_JWT_KEY -e POSTGRES_HOST=$env:WEB_VELEPRODAJA_POSTGRES_HOST -e POSTGRES_PORT=$env:WEB_VELEPRODAJA_POSTGRES_PORT -e POSTGRES_PASSWORD=$env:WEB_VELEPRODAJA_POSTGRES_PASSWORD --name termodom--web-veleprodaja-api -m 1G --restart=always -d limitlesssoft/termodom--web-veleprodaja-api:$env:BUILD_NUMBER