# =============
# Declaring the variables
# =============
$MainDir = Get-location

# Container name variables
$ContainerName_WebPostgresDb = 'web-postgres-db'

# Container host port variables
$ContainerHostPort_WebPostgresDb = '59432'

# =============
# Pulling latest images
# =============
echo 'Pulling latest images'
docker pull postgres:latest
#==============

cd $MainDir
if (docker stop $ContainerName_WebPostgresDb) { } else { }
if (docker rm $ContainerName_WebPostgresDb) { } else { }
docker run --name $ContainerName_WebPostgresDb -m 1G -p $ContainerHostPort_WebPostgresDb:5432 -e POSTGRES_PASSWORD=x0rAo2DdhJA36rvT -d postgres:latest