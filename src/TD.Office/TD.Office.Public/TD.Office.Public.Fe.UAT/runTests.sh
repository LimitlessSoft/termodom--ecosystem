#!/bin/bash

set -e

remove_existing_containers() {
    echo Removing existing containers
    docker stop office-public-be office-public-fe >/dev/null 2>&1 || true
    docker rm office-public-be office-public-fe >/dev/null 2>&1 || true
    docker rmi limitlesssoft/termodom-web-admin-api:temp limitlesssoft/termodom-web-admin-fe:temp >/dev/null 2>&1 || true

    echo Removing existing network
    docker network rm test-network >/dev/null 2>&1 || true
}

setup_containers() {
    if ! vault login -tls-skip-verify -method=userpass username="$VAULT_USERNAME" password="$VAULT_PASSWORD" >/dev/null 2>&1; then
        echo "Error: Vault login failed!"
        exit 1
    fi

    if ! docker ps >/dev/null 2>&1; then
        echo "Error: Docker command failed! try running it with sudo."
        exit 1
    fi

    remove_existing_containers

    echo Creating network...
    docker network create test-network >/dev/null 2>&1

    echo Building and running office-public-be container...
    if ! temp_output=$(docker build -t limitlesssoft/termodom-office-public-api:temp -f src/TD.Office/TD.Office.Public/TD.Office.Public.Api/Dockerfile . 2>&1 > /dev/null); then
      echo "$temp_output"
      echo Building failed! Check the logs above.
    fi
    docker run -d --name office-public-be \
      --network test-network \
      -p 8080:8080 \
      -e AllowedHosts="*" \
      -e VAULT_URI="$VAULT_ADDR" \
      -e VAULT_USERNAME="$VAULT_USERNAME" \
      -e VAULT_PASSWORD="$VAULT_PASSWORD" \
      -e VAULT_ENGINE="automation" \
      -e VAULT_PATH="office/public/api" \
      limitlesssoft/termodom-office-public-api:temp \
      >/dev/null 2>&1

    echo Building and running office-public-fe container...
    if ! temp_output=$(docker build -t limitlesssoft/termodom-office-public-fe:temp -f src/TD.Office/TD.Office.Public/TD.Office.Public.Fe/Dockerfile --build-arg "DEPLOY_ENV=automation" --build-arg "OVERRIDE_DEPLOY_ENV=http://office-public-be:8080" . 2>&1 > /dev/null); then
      echo "$temp_output"
      echo Building failed! Check the logs above.
    fi
    docker run -d --name office-public-fe \
      --network test-network \
      -p 3000:3000 \
      limitlesssoft/termodom-office-public-fe:temp \
      >/dev/null 2>&1
}

show_help() {
    cat << EOF
Usage: $0 [OPTIONS]

Options:
  -U, --username=<VAULT_USERNAME> Specify the Vault username (required)
  -P, --password=<VAULT_PASSWORD> Specify the Vault password (required)
  -S, --skip                      Skip Building and running docker containers (use if you want to run tests only and have the containers already running locally)
  --help                          Show this help message
EOF
    exit 0
}

ARGS=$(getopt -o U:,P:,S --long username:,password:,skip,help -n "$0" -- "$@")

if [ $? -ne 0 ]; then
    exit 1
fi

eval set -- "$ARGS"

VAULT_USERNAME=""
VAULT_PASSWORD=""
VAULT_ADDR="http://vault.termodom.rs:8199"
SKIP=false

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
        -S|--skip)
            SKIP=true
            shift
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

export VAULT_ADDR

if [ -z "$VAULT_USERNAME" ] || [ -z "$VAULT_PASSWORD" ]; then
    echo "Error: --username, and --password arguments are required!"
    echo "Use --help for usage information."
    exit 1
fi

docker stop selenium-driver >/dev/null 2>&1 && docker rm selenium-driver >/dev/null 2>&1

if [ "$SKIP" = "false" ] ; then
    setup_containers
fi
echo Running tests...
cd src/TD.Office/TD.Office.Public/TD.Office.Public.Fe.UAT

npm install >/dev/null 2>&1

export VAULT_USERNAME
export VAULT_PASSWORD
export SELENIUM_SERVER=localhost
export PROJECT_URL=http://office-public-fe:3000


for browser in chrome firefox; do
    docker stop selenium-driver >/dev/null 2>&1 && docker rm selenium-driver >/dev/null 2>&1
    
    echo Preparing selenium driver for $browser...
    docker run -d --name selenium-driver \
      --network test-network \
      -p 4444:4444 \
      selenium/standalone-$browser \
      >/dev/null 2>&1 
    
    echo Waiting for selenium $browser driver to start...
    while ! curl -sSL http://localhost:4444/wd/hub/status 2>/dev/null | jq -e '.value.ready' | grep -q true; do
        sleep 1
    done
    
    echo Running tests on $browser...
    output=$(FORCE_COLOR=1 npm run test:dockerized-driver-$browser 2>&1 | tee /dev/tty | tail -n 5)
    test_output+=("===== $browser =====")
    test_output+=("$(echo "$output")")
    test_output+=("")
    
    echo Cleaning up selenium driver...
    docker stop selenium-driver >/dev/null 2>&1 && docker rm selenium-driver >/dev/null 2>&1
done

echo # empty line
echo ===================
echo All tests finished!
echo ===================
echo # empty line
printf "%s\n" "${test_output[@]}"
echo ===================
echo ===================
if [ "$SKIP" = "false" ] ; then
    remove_existing_containers
fi