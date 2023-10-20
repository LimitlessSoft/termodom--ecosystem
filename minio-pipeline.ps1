# =============
# Declaring the variables
# =============
$MainDir = Get-location

# Images name variables
$ImageName_Minio = "limitlesssoft/termodom--minio:" + $env:BUILD_NUMBER

# Container name variables
$ContainerName_Minio = 'td-minio'

# Ports
$Port1 = '9000:9000'
$Port2_1 = '9001'
$Port2_2 = ':9001'
$Port2 = $Port2_1+$Port2_2

# =============
# Pulling latest images
# =============
echo 'Pulling latest images'
docker pull minio/minio
echo 'Done pulling latest images'
#==============

# =============
# Stopping and removing containers
# =============
docker stop $ContainerName_Minio
docker rm $ContainerName_Minio
# =============

docker run -p $Port1 -p $Port2 --name $ContainerName_Minio -d --restart=unless-stopped -v C:/minio/data:/data -e MINIO_ROOT_USER=$env:MINIO_USER -e MINIO_ROOT_PASSWORD=$env:MINIO_PASSWORD quay.io/minio/minio server /data --console-address $Port2_2