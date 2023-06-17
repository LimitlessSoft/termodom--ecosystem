$MainDir = Get-location
$Counter = Get-Counter -Counter
cd $MainDir/src/Termodom-TDBrain-v3
docker stop termodom--td-brain
docker rm termodom--td-brain
docker build . -t limitlesssoft/termodom--td-brain:$Counter
docker run -p 32775:80 --name termodom--td-brain -d limitlesssoft/termodom--td-brain:$Counter