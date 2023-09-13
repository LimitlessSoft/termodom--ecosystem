# =============
# Declaring the variables
# =============
$MainDir = Get-location

# Images name variables
$ImageName_WebApiDotnet = "limitlesssoft/termodom--web-api-dotnet:" + $env:BUILD_NUMBER
$ImageName_WebFrontEndMain = "limitlesssoft/termodom--front-end-main:" + $env:BUILD_NUMBER
$ImageName_WebFrontEndAdmin = "limitlesssoft/termodom--front-end-admin:" + $env:BUILD_NUMBER

# Container name variables
$ContainerName_WebFrontEndMain = 'web-front-end-main'
$ContainerName_WebFrontEndAdmin = 'web-front-end-admin'

# Container host port variables
$ContainerHostPort_WebApiDotnet = '59001:80'
$ContainerHostPort_WebFrontEndMain = '4000:3000'
$ContainerHostPort_WebFrontEndAdmin = '4001:3000'

# =============
# Pulling latest images
# =============
echo 'Pulling latest images'
echo 'Done pulling latest images'
#==============

# =============
# Stopping and removing containers
# =============
docker stop $ContainerName_WebApiDotnet
docker rm $ContainerName_WebApiDotnet

docker stop $ContainerName_WebFrontEndMain
docker rm $ContainerName_WebFrontEndMain

docker stop $ContainerName_WebFrontEndAdmin
docker rm $ContainerName_WebFrontEndAdmin
# =============

cd $MainDir/src/WebApplications/TD.Web.Api
dotnet build
dotnet publish -o obj/Docker/publish -c Release --runtime linux-x64 --self-contained False
docker build -f ./Dockerfile -t $ImageName_WebApiDotnet ./obj/Docker/publish
docker run -p $ContainerHostPort_WebApiDotnet -e JWT_ISSUER=$env:JWT_ISSUER -e POSTGRES_HOST=$env:POSTGRES_HOST -e POSTGRES_PORT=$env:POSTGRES_PORT -e POSTGRES_PASSWORD=$env:POSTGRES_PASSWORD -e JWT_AUDIENCE=$env:JWT_AUDIENCE -e JWT_KEY=$env:JWT_KEY --name $ContainerName_WebApiDotnet -m 1G --restart=always -d $ImageName_WebApiDotnet

cd $MainDir/src/WebApplications/FrontEnd/termodom--fe-main
docker build -t $ImageName_WebFrontEndMain .
docker run -p $ContainerHostPort_WebFrontEndMain --name $ContainerName_WebFrontEndMain -d $ImageName_WebFrontEndMain

cd $MainDir/src/WebApplications/FrontEnd/termodom--fe-admin
docker build -t $ImageName_WebFrontEndAdmin .
docker run -p $ContainerHostPort_WebFrontEndAdmin --name $ContainerName_WebFrontEndAdmin -d $ImageName_WebFrontEndAdmin