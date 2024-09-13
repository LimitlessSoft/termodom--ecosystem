$MainDir = Get-location

docker stop termodom--komercijalno-api-TCMD-2024
docker rm termodom--komercijalno-api-TCMD-2024

docker stop termodom--komercijalno-api-TD-2024
docker rm termodom--komercijalno-api-TD-2024

docker stop termodom--komercijalno-api-SasaPdv-2024
docker rm termodom--komercijalno-api-SasaPdv-2024

docker stop termodom--komercijalno-api-TCMD-2023
docker rm termodom--komercijalno-api-TCMD-2023

cd $MainDir/src/TD.Komercijalno/TD.Komercijalno.Api
dotnet build
dotnet publish -o obj/Docker/publish -c Release --runtime linux-x64 --self-contained False
docker build -f ./Dockerfile -t limitlesssoft/termodom--komercijalno-api:$env:BUILD_NUMBER ./obj/Docker/publish
docker run -p 32776:80 -e ConnectionString_Komercijalno=$env:ConnectionStrings_KomercijalnoFransiza2024TCMD --name termodom--komercijalno-api-TCMD-2024 -m 1G --restart=always -d limitlesssoft/termodom--komercijalno-api:$env:BUILD_NUMBER

cd $MainDir/src/TD.Komercijalno/TD.Komercijalno.Api
dotnet build
dotnet publish -o obj/Docker/publish -c Release --runtime linux-x64 --self-contained False
docker build -f ./Dockerfile -t limitlesssoft/termodom--komercijalno-api:$env:BUILD_NUMBER ./obj/Docker/publish
docker run -p 32774:80 -e ConnectionString_Komercijalno=$env:ConnectionStrings_KomercijalnoTermodom2024 --name termodom--komercijalno-api-TD-2024 -m 1G --restart=always -d limitlesssoft/termodom--komercijalno-api:$env:BUILD_NUMBER

cd $MainDir/src/TD.Komercijalno/TD.Komercijalno.Api
dotnet build
dotnet publish -o obj/Docker/publish -c Release --runtime linux-x64 --self-contained False
docker build -f ./Dockerfile -t limitlesssoft/termodom--komercijalno-api:$env:BUILD_NUMBER ./obj/Docker/publish
docker run -p 32773:80 -e ConnectionString_Komercijalno=$env:ConnectionStrings_KomercijalnoFransiza2024SasaPdv --name termodom--komercijalno-api-SasaPdv-2024 -m 1G --restart=always -d limitlesssoft/termodom--komercijalno-api:$env:BUILD_NUMBER

cd $MainDir/src/TD.Komercijalno/TD.Komercijalno.Api
dotnet build
dotnet publish -o obj/Docker/publish -c Release --runtime linux-x64 --self-contained False
docker build -f ./Dockerfile -t limitlesssoft/termodom--komercijalno-api:$env:BUILD_NUMBER ./obj/Docker/publish
docker run -p 32775:80 -e ConnectionString_Komercijalno=$env:ConnectionStrings_KomercijalnoFransiza2023TCMD --name termodom--komercijalno-api-TCMD-2023 -m 1G --restart=always -d limitlesssoft/termodom--komercijalno-api:$env:BUILD_NUMBER

