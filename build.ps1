
## Maplr-api and TestMaplrSugarShack dockerfiles build
write-host("Maplr-api and TestMaplrSugarShack dockerfiles build")

$Env:DOCKER_BUILDKIT = 1
Set-Location -Path "maplr-api"
docker build -t maplr_api .
Set-Location -Path "..\TestMaplrSugarShack"
docker build -t maplr_api_tests .
Set-Location -Path "..\"

write-host("Builded images, run docker-compose up")