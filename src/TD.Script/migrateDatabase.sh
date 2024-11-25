#!/bin/bash


ARGS=$(getopt -o a:s:t: --long application:,source:,target: -n "$0" -- "$@")
if [ $? -ne 0 ]; then
    echo "Error parsing arguments."
    echo "Usage: $0 --application <app> --source <env> --target <env>"
    exit 1
fi


eval set -- "$ARGS"

APPLICATIONS=()
SOURCE_ENV=""
TARGET_ENV=""

while true; do
    case "$1" in
        -a|--application) 
            APPLICATIONS+=("$2")
            shift 2 
            ;;
        -s|--source) 
            SOURCE_ENV="$2"
            shift 2 
            ;;
        -t|--target) 
            TARGET_ENV="$2"
            shift 2 
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

if [[ -z "$APPLICATIONS" || -z "$SOURCE_ENV" || -z "$TARGET_ENV" ]]; then
    echo "Missing required arguments."
    echo "Usage: $0 --application <app> [--application <app>] --source <env> --target <env>"
    exit 1
fi

echo "Applications: ${APPLICATIONS[@]}"
echo "Source Environment: $SOURCE_ENV"
echo "Target Environment: $TARGET_ENV"

echo "Building Docker image..."
docker build -t db-migration-tool . || {
    echo "Docker build failed!"
    exit 1
}

PG_HOST="192.168.0.17"
PG_PORT=5432
PG_USER="postgres"
PG_PASSWORD="password123"

for APP in "${APPLICATIONS[@]}"; do
    echo "Starting migration for $APP from $SOURCE_ENV to $TARGET_ENV..."
    docker run --rm \
        -e PG_HOST="$PG_HOST" \
        -e PG_PORT="$PG_PORT" \
        -e PG_USER="$PG_USER" \
        -e PG_PASSWORD="$PG_PASSWORD" \
        -e SOURCE_ENV="$SOURCE_ENV" \
        -e TARGET_ENV="$TARGET_ENV" \
        -e APPLICATION="$APP" \
        db-migration-tool || {
            echo "Migration failed for $APP"
            exit 1
        }
done
