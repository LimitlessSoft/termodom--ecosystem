# # =============
# # Declaring the variables
# # =============
# $MainDir = Get-location

# # Container name variables
# $ContainerName_WebPostgresDb = 'web-postgres-db'

# # Container host port variables
# $ContainerHostPort_WebPostgresDb = '59432:5432'

# # =============
# # Pulling latest images
# # =============
# echo 'Pulling latest images'
# docker pull postgres:latest
# echo 'Done pulling latest images'
# #==============

# # =============
# # Stopping and removing containers
# # =============
# docker stop $ContainerName_WebPostgresDb
# docker rm $ContainerName_WebPostgresDb
# # =============

# cd $MainDir
# docker run --name $ContainerName_WebPostgresDb -m 1G --restart=unless-stopped -p $ContainerHostPort_WebPostgresDb -e POSTGRES_PASSWORD=$env:POSTGRES_PASSWORD -d postgres:latest