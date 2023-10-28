# =============
# Declaring the variables
# =============
$MainDir = Get-location

# Images name variables
$ImageName_WebApiDotnet = "limitlesssoft/termodom--web-api-dotnet:" + $env:BUILD_NUMBER
$ImageName_WebFrontEndMain = "limitlesssoft/termodom--front-end-main:" + $env:BUILD_NUMBER
$ImageName_WebFrontEndAdmin = "limitlesssoft/termodom--front-end-admin:" + $env:BUILD_NUMBER

# Container name variables
$ContainerName_WebApiDotnet = 'web-api-dotnet'
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

cd $MainDir/src/TD.Web/TD.Web.Admin/TD.Web.Admin.Api
dotnet build
dotnet publish -o obj/Docker/publish -c Release --runtime linux-x64 --self-contained False
docker build -f ./Dockerfile -t $ImageName_WebApiDotnet ./obj/Docker/publish
docker run -p $ContainerHostPort_WebApiDotnet -e JWT_ISSUER=$env:JWT_ISSUER -e POSTGRES_HOST=$env:POSTGRES_HOST -e POSTGRES_PORT=$env:POSTGRES_PORT -e POSTGRES_PASSWORD=$env:POSTGRES_PASSWORD -e JWT_AUDIENCE=$env:JWT_AUDIENCE -e JWT_KEY=$env:JWT_KEY -e MINIO_HOST=$env:MINIO_HOST -e MINIO_ACCESS_KEY=$env:MINIO_ACCESS_KEY -e MINIO_SECRET_KEY=$env:MINIO_SECRET_KEY -e MINIO_PORT=$env:MINIO_PORT --name $ContainerName_WebApiDotnet -m 1G --restart=unless-stopped -d $ImageName_WebApiDotnet

cd $MainDir/src/TD.Web/TD.Web.Public/TD.Web.Public.Fe
docker build -t $ImageName_WebFrontEndMain .
docker run -p $ContainerHostPort_WebFrontEndMain --name $ContainerName_WebFrontEndMain --restart=unless-stopped -d $ImageName_WebFrontEndMain

cd $MainDir/src/TD.Web/TD.Web.Admin/TD.Web.Admin.Fe
docker build -t $ImageName_WebFrontEndAdmin .
docker run -p $ContainerHostPort_WebFrontEndAdmin --name $ContainerName_WebFrontEndAdmin --restart=unless-stopped -d $ImageName_WebFrontEndAdmin