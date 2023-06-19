$MainDir = Get-location

# cd $MainDir/src/Termodom-TDBrain-v3
# docker stop termodom--td-brain
# docker rm termodom--td-brain
# docker build . -t limitlesssoft/termodom--td-brain:$env:BUILD_NUMBER
# docker run -p 32775:80 --name termodom--td-brain -m 1G --restart=always -d limitlesssoft/termodom--td-brain:$env:BUILD_NUMBER

cd $MainDir/src/TD.Komercijalno/TD.Komercijalno.Api
dotnet build
dotnet publish -o obj/Docker/publish -c Release --runtime linux-x64 --self-contained False
docker stop termodom--komercijalno-api
docker rm termodom--komercijalno-api
docker build -f ./Dockerfile -t limitlesssoft/termodom--komercijalno-api:$env:BUILD_NUMBER ./obj/Docker/publish
docker run -p 32776:80 -e 'ConnectionString_Komercijalno=%env.ConnectionStrings.KomercijalnoFransiza2023TCMD%' --name termodom--komercijalno-api -m 1G --restart=always -d limitlesssoft/termodom--komercijalno-api:$env:BUILD_NUMBER

cd $MainDir/src/TD.WebshopListener/TD.WebshopListener.App
dotnet build
dotnet publish -o obj/Docker/publish -c Release --runtime linux-x64 --self-contained False
docker stop termodom--webshop-listener-app
docker rm termodom--webshop-listener-app
docker build -f ./Dockerfile -t limitlesssoft/termodom--webshop-listener-app:$env:BUILD_NUMBER ./obj/Docker/publish
docker run -p 32777:80 --name termodom--webshop-listener-app -m 1G --restart=always -d limitlesssoft/termodom--webshop-listener-app:$env:BUILD_NUMBER