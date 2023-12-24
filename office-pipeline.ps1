# =============
# Declaring the variables
# =============
$MainDir = Get-location

# Images name variables
$ImageName_OfficeApiDotnet = "limitlesssoft/termodom--office-api-dotnet:" + $env:BUILD_NUMBER

# Container name variables
$ContainerName_OfficeApiDotnet = 'office-api-dotnet'

# Container host port variables
$ContainerHostPort_OfficeApiDotnet = '59011:80'

# =============
# Pulling latest images
# =============
echo 'Pulling latest images'
echo 'Done pulling latest images'
#==============

# =============
# Stopping and removing containers
# =============
docker stop $ImageName_OfficeApiDotnet
docker rm $ImageName_OfficeApiDotnet
# =============

cd $MainDir/src/TD.Office/TD.Office.Public/TD.Office.Public.Api
dotnet build
dotnet publish -o obj/Docker/publish -c Release --runtime linux-x64 --self-contained False
docker build -f ./Dockerfile -t $ImageName_OfficeApiDotnet ./obj/Docker/publish
docker run -p $ContainerHostPort_OfficeApiDotnet -e JWT_ISSUER=$env:JWT_ISSUER -e POSTGRES_HOST=$env:POSTGRES_HOST -e POSTGRES_PORT=$env:POSTGRES_PORT -e POSTGRES_USER=$env:POSTGRES_USER -e POSTGRES_DATABASE_NAME=$env:POSTGRES_DATABASE_NAME -e POSTGRES_PASSWORD=$env:POSTGRES_PASSWORD -e JWT_AUDIENCE=$env:JWT_AUDIENCE -e JWT_KEY=$env:JWT_KEY --name $ContainerName_OfficeApiDotnet -m 1G --restart=unless-stopped -d $ImageName_OfficeApiDotnet

# cd $MainDir/src/TD.Web/TD.Web.Admin/TD.Web.Admin.Fe
# docker build -t $ImageName_WebFrontEndAdmin .
# docker run -p $ContainerHostPort_WebFrontEndAdmin --name $ContainerName_WebFrontEndAdmin --restart=unless-stopped -d $ImageName_WebFrontEndAdmin