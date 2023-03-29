docker stop termodom--postgres
docker rm termodom--postgres
docker run --name termodom--postgres -p 65432:5432 -e POSTGRES_PASSWORD=Plivanje123$ -d postgres

docker stop termodom-db--migrations
docker rm termodom--db-migrations
docker build --force-rm -t limitlesssoft/termodom--db-migrations -f DBMigrations/Dockerfile .
docker run --name termodom--db-migrations -p 65086:80 -d limitlesssoft/termodom--db-migrations