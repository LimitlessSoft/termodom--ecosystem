# =============
# Declaring the variables
# =============
$MainDir = Get-location

# Container name variables
$ContainerName_WebPostgresDb = 'web-postgres-db'

# Container host port variables
$ContainerHostPort_WebPostgresDb = '59432:5432'

# =============
# Pulling latest images
# =============
echo 'Pulling latest images'
docker pull postgres:latest
#==============

cd $MainDir
docker stop $ContainerName_WebPostgresDb
docker rm $ContainerName_WebPostgresDb
docker run --name $ContainerName_WebPostgresDb -m 1G -p $ContainerHostPort_WebPostgresDb -e POSTGRES_PASSWORD=x0rAo2DdhJA36rvT -d postgres:latest