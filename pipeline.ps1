$MainDir = Get-location

# cd $MainDir/src/Termodom-TDBrain-v3
# docker stop termodom--td-brain
# docker rm termodom--td-brain
# docker build . -t limitlesssoft/termodom--td-brain:$env:BUILD_NUMBER
# docker run -p 32775:80 --name termodom--td-brain -m 1G --restart=always -d limitlesssoft/termodom--td-brain:$env:BUILD_NUMBER

docker stop termodom--komercijalno-api
docker rm termodom--komercijalno-api

docker stop termodom--webshop-listener-app
docker rm termodom--webshop-listener-app

docker stop termodom--tdoffice-api
docker rm termodom--tdoffice-api

docker stop termodom--tdoffice-fe-api
docker rm termodom--tdoffice-fe-api

cd $MainDir/src/TD.Komercijalno/TD.Komercijalno.Api
dotnet build
dotnet publish -o obj/Docker/publish -c Release --runtime linux-x64 --self-contained False
docker build -f ./Dockerfile -t limitlesssoft/termodom--komercijalno-api:$env:BUILD_NUMBER ./obj/Docker/publish
docker run -p 32776:80 -e ConnectionString_Komercijalno=$env:ConnectionStrings_KomercijalnoFransiza2023TCMD --name termodom--komercijalno-api -m 1G --restart=always -d limitlesssoft/termodom--komercijalno-api:$env:BUILD_NUMBER

cd $MainDir/src/TD.WebshopListener/TD.WebshopListener.App
dotnet build
dotnet publish -o obj/Docker/publish -c Release --runtime linux-x64 --self-contained False
docker build -f ./Dockerfile -t limitlesssoft/termodom--webshop-listener-app:$env:BUILD_NUMBER ./obj/Docker/publish
docker run -p 32777:80 --name termodom--webshop-listener-app -m 1G --restart=always -d limitlesssoft/termodom--webshop-listener-app:$env:BUILD_NUMBER

cd $MainDir/src/TD.TDOffice/TD.TDOffice.Api
dotnet build
dotnet publish -o obj/Docker/publish -c Release --runtime linux-x64 --self-contained False
docker build -f ./Dockerfile -t limitlesssoft/termodom--tdoffice-api:$env:BUILD_NUMBER ./obj/Docker/publish
docker run -p 32778:80 -e ConnectionString_TDOffice=$env:ConnectionStrings_TDOffice --name termodom--tdoffice-api -m 1G --restart=always -d limitlesssoft/termodom--tdoffice-api:$env:BUILD_NUMBER

cd $MainDir/src/TD.FE/TD.FE.TDOffice/TD.FE.TDOffice.Api
dotnet build
dotnet publish -o obj/Docker/publish -c Release --runtime linux-x64 --self-contained False
docker build -f ./Dockerfile -t limitlesssoft/termodom--tdoffice-fe-api:$env:BUILD_NUMBER ./obj/Docker/publish
docker run -p 32779:80 --name termodom--tdoffice-fe-api -m 1G --restart=always -d limitlesssoft/termodom--tdoffice-fe-api:$env:BUILD_NUMBER