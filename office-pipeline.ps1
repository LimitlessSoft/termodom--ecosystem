# =============
# Declaring the variables
# =============
$MainDir = Get-location

# Images name variables
$ImageName_OfficePublicApiDotnet = "limitlesssoft/termodom--office-public-api-dotnet:" + $env:BUILD_NUMBER
$ImageName_OfficePublicFE = "limitlesssoft/termodom--office-public-fe:" + $env:BUILD_NUMBER

# Container name variables
$ContainerName_OfficePublicApiDotnet = 'office-public-api-dotnet'
$ContainerName_OfficePublicFE = 'office-public-fe'

# Container host port variables
$ContainerHostPort_OfficePublicApiDotnet = '59011:80'
$ContainerHostPort_OfficePublicFE = '4005:3000'

# =============
# Pulling latest images
# =============
echo 'Pulling latest images'
echo 'Done pulling latest images'
#==============

# =============
# Stopping and removing containers
# =============
docker stop $ContainerName_OfficePublicApiDotnet
docker rm $ContainerName_OfficePublicApiDotnet

docker stop $ContainerName_OfficePublicFE
docker rm $ContainerName_OfficePublicFE
# =============

cd $MainDir/src/TD.Office/TD.Office.Public/TD.Office.Public.Api
dotnet build
dotnet publish -o obj/Docker/publish -c Release --runtime linux-x64 --self-contained False
docker build -f ./Dockerfile -t $ImageName_OfficePublicApiDotnet ./obj/Docker/publish
docker run -p $ContainerHostPort_OfficePublicApiDotnet -e JWT_ISSUER=$env:JWT_ISSUER -e POSTGRES_HOST=$env:POSTGRES_HOST -e POSTGRES_PORT=$env:POSTGRES_PORT -e POSTGRES_USER=$env:POSTGRES_USER -e POSTGRES_DATABASE_NAME=$env:POSTGRES_DATABASE_NAME -e POSTGRES_PASSWORD=$env:POSTGRES_PASSWORD -e JWT_AUDIENCE=$env:JWT_AUDIENCE -e JWT_KEY=$env:JWT_KEY --name $ContainerName_OfficePublicApiDotnet -m 1G --restart=unless-stopped -d $ImageName_OfficePublicApiDotnet

cd $MainDir/src/TD.Office/TD.Office.Public/TD.Office.Public.Fe
docker build -t $ImageName_OfficePublicFE .
docker run -p $ContainerHostPort_OfficePublicFE --name $ContainerName_OfficePublicFE --restart=unless-stopped -d $ImageName_OfficePublicFE