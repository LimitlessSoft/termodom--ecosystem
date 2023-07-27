$MainDir = Get-location

docker stop termodom--web-veleprodaja-api
docker rm termodom--web-veleprodaja-api

cd $MainDir/src/WebApplications/TD.Web.Veleprodaja/TD.Web.Veleprodaja.Api
dotnet build
dotnet publish -o obj/Docker/publish -c Release --runtime linux-x64 --self-contained False
docker build -f ./Dockerfile -t limitlesssoft/termodom--web-veleprodaja-api:$env:BUILD_NUMBER ./obj/Docker/publish
docker run -p 33770:80 -e API_HOST=$env:API_HOST --name termodom--tdoffice-web-veleprodaja-api -m 1G --restart=always -d limitlesssoft/termodom--web-veleprodaja-api:$env:BUILD_NUMBER