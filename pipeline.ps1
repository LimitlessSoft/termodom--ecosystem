$MainDir = Get-location

docker stop termodom--tdoffice-api
docker rm termodom--tdoffice-api

docker stop termodom--tdoffice-fe-api
docker rm termodom--tdoffice-fe-api

docker stop termodom--office-server-api
docker rm termodom--office-server-api

cd $MainDir/src/TD.TDOffice/TD.TDOffice.Api
dotnet build
dotnet publish -o obj/Docker/publish -c Release --runtime linux-x64 --self-contained False
docker build -f ./Dockerfile -t limitlesssoft/termodom--tdoffice-api:$env:BUILD_NUMBER ./obj/Docker/publish
docker run -p 32778:80 -e ConnectionString_TDOffice=$env:ConnectionStrings_TDOffice --name termodom--tdoffice-api -m 1G --restart=always -d limitlesssoft/termodom--tdoffice-api:$env:BUILD_NUMBER

cd $MainDir/src/TD.FE/TD.FE.TDOffice/TD.FE.TDOffice.Api
dotnet build
dotnet publish -o obj/Docker/publish -c Release --runtime linux-x64 --self-contained False
docker build -f ./Dockerfile -t limitlesssoft/termodom--tdoffice-fe-api:$env:BUILD_NUMBER ./obj/Docker/publish
docker run -p 32779:80 -e API_HOST=$env:API_HOST --name termodom--tdoffice-fe-api -m 1G --restart=always -d limitlesssoft/termodom--tdoffice-fe-api:$env:BUILD_NUMBER

cd $MainDir/src/TD.OfficeServer/TD.OfficeServer.Api
dotnet build
dotnet publish -o obj/Docker/publish -c Release --runtime linux-x64 --self-contained False
docker build -f ./Dockerfile -t limitlesssoft/termodom--office-server-api:$env:BUILD_NUMBER ./obj/Docker/publish
docker run -p 32780:80 -e --name termodom--office-server-api -m 1G --restart=always -d limitlesssoft/termodom--office-server-api:$env:BUILD_NUMBER