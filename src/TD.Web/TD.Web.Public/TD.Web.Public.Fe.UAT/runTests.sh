#!/bin/bash

set -e

remove_existing_containers() {
    docker stop web-public-be web-public-fe selenium-driver >/dev/null 2>&1 || true
    docker rm web-public-be web-public-fe selenium-driver >/dev/null 2>&1 || true
    docker network rm test-network >/dev/null 2>&1 || true
}

remove_existing_containers

show_help() {
    cat << EOF
Usage: $0 [OPTIONS]

Options:
  -U, --username=<VAULT_USERNAME> Specify the Vault username (required)
  -P, --password=<VAULT_PASSWORD> Specify the Vault password (required)
  --help                      Show thfis help message
EOF
    exit 0
}

ARGS=$(getopt -o U:P: --long username:,password:,help -n "$0" -- "$@")

if [ $? -ne 0 ]; then
    exit 1
fi

eval set -- "$ARGS"

VAULT_USERNAME=""
VAULT_PASSWORD=""
VAULT_ADDR="http://45.79.250.225:8199"

export VAULT_ADDR

echo $VAULT_ADDR

while [[ $# -gt 0 ]]; do
    case "$1" in
        -U|--username)
            VAULT_USERNAME="$2"
            shift 2
            ;;
        -P|--password)
            VAULT_PASSWORD="$2"
            shift 2
            ;;
        --help)
            show_help
            ;;
        --)
            shift
            break
            ;;
        *)
            echo "Invalid option: $1"
            exit 1
            ;;
    esac
done

if [ -z "$VAULT_USERNAME" ] || [ -z "$VAULT_PASSWORD" ]; then
    echo "Error: --username, and --password arguments are required!"
    echo "Use --help for usage information."
    exit 1
fi


if ! vault login -tls-skip-verify -method=userpass username="$VAULT_USERNAME" password="$VAULT_PASSWORD" >/dev/null 2>&1; then
    echo "Error: Vault login failed!"
    exit 1
fi

docker network create test-network >/dev/null 2>&1

docker build -t limitlesssoft/termodom-web-public-api:temp -f src/TD.Web/TD.Web.Public/TD.Web.Public.Api/Dockerfile . >/dev/null 2>&1
docker run -d --name web-public-be --network test-network -p 8080:8080 \
    -e AllowedHosts="*" -e VAULT_URI="$VAULT_ADDR" -e VAULT_USERNAME="$VAULT_USERNAME" -e VAULT_PASSWORD="$VAULT_PASSWORD" -e VAULT_ENGINE="develop" -e VAULT_PATH="web/public/api" limitlesssoft/termodom-web-public-api:temp >/dev/null 2>&1

docker build -t limitlesssoft/termodom-web-public-fe:temp -f src/TD.Web/TD.Web.Public/TD.Web.Public.Fe/Dockerfile --build-arg "DEPLOY_ENV=develop" --build-arg "OVERRIDE_DEPLOY_ENV=http://web-public-be:8080" . >/dev/null 2>&1
docker run -d --name web-public-fe --network test-network -p 3000:3000 limitlesssoft/termodom-web-public-fe:temp >/dev/null 2>&1

cd src/TD.Web/TD.Web.Public/TD.Web.Public.Fe.UAT

export SELENIUM_SERVER=localhost
export PROJECT_URL=http://web-public-fe:3000

for browser in chrome firefox; do
    echo
    echo "Testing with $browser..."
    docker run -d --name selenium-driver --network test-network -p 4444:4444 selenium/standalone-$browser >/dev/null 2>&1
    
    sleep 10
    
    npm install >/dev/null 2>&1
    npm run test:dockerized-driver-$browser
    
    docker stop selenium-driver >/dev/null 2>&1 && docker rm selenium-driver >/dev/null 2>&1
done

remove_existing_containers