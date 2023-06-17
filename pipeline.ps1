cd /src/Termodom-TDBrain-v3
docker stop termodom--td-brain
docker rm termodom--td-brain
docker build . -t limitlesssoft/termodom--td-brain:%build.counter%
docker run -p 32775:80 --name termodom--td-brain -d limitlesssoft/termodom--td-brain:%build.counter%