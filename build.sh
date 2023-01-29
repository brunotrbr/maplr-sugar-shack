
#!/bin/bash

## Building Maplr-api and TestMaplrSugarShack dockerfiles
echo "Building Maplr-api and TestMaplrSugarShack dockerfiles"

export DOCKER_BUILDKIT=1
cd maplr-api
docker build -t maplr_api .
cd ../TestMaplrSugarShack
docker build -t maplr_api_tests .
cd ../

echo "Builded images, run docker-compose up"