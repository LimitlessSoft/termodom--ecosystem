#!/bin/bash

# store usage message in a variable
REQUEST_HELP="See: $0 --help"

error() {
    echo "Running script failed"
    echo "$REQUEST_HELP"
    exit 1
}

# capture the arguments
ARGS=$(getopt -o a:s:t:h:p:U:P: --long application:,source:,target:,host:,port:,user:,password:,help -n "$0" -- "$@")

# check if getopt failed
if [ $? -ne 0 ]; then
    error
fi

# check if --help is passed
if [[ "$1" == "--help" ]]; then
    echo "Usage: $0 [options]"
    echo "Options:"
    echo "  -a, --application <application>  Application name(s) [web, tdoffice] (required)"
    echo "  -s, --source <source>            Source environment (release, develop) (required)"
    echo "  -t, --target <target>            Target environment (release, develop) (required)"
    echo "  -h, --host <host>                Database host (IP) (required)"
    echo "  -p, --port <port>                Database port (default: 5432)"
    echo "  -U, --user <user>                Database user (default: postgres)"
    echo "  -P, --password <password>        Database password (required)"
    echo "  --help                          Show this help message"
    exit 0
fi

# set the arguments to the positional parameters
eval set -- "$ARGS"

# set the default values
APPLICATIONS=()
SOURCE_ENV=""
TARGET_ENV=""
PGPORT=5432
PGUSER="postgres"

# loop through the arguments and set the values
while true; do
    case "$1" in
        -a|--application) APPLICATIONS+=("$2"); shift 2 ;;
        -s|--source) SOURCE_ENV="$2"; shift 2 ;;
        -t|--target) TARGET_ENV="$2"; shift 2 ;;
        -h|--host) PGHOST="$2"; shift 2 ;;
        -p|--port) PGPORT="$2"; shift 2 ;;
        -U|--user) PGUSER="$2"; shift 2 ;;
        -P|--password) PGPASSWORD="$2"; shift 2 ;;
        --) shift; break ;;
        *) echo "Invalid option: $1"; exit 1 ;;
    esac
done

if [[ -z "$APPLICATIONS" || -z "$SOURCE_ENV" || -z "$TARGET_ENV" || -z "$PGHOST" || -z "$PGPASSWORD" ]]; then
    error
fi

echo "====================================================================="
echo ""
echo "Applications: ${APPLICATIONS[@]}"
echo "Source Environment: $SOURCE_ENV"
echo "Target Environment: $TARGET_ENV"
echo "Host: $PGHOST"
echo "Port: $PGPORT"
echo "User: $PGUSER"
echo "Password: $PGPASSWORD"
echo ""
echo "====================================================================="
echo "Building Docker image..."
echo "====================================================================="
docker build -t db-migration-tool . || {
    echo "Docker build failed!"
    exit 1
}
echo "====================================================================="
echo ""

for APP in "${APPLICATIONS[@]}"; do
    echo "Starting db migration for $APP from $SOURCE_ENV to $TARGET_ENV..."
    docker run --rm \
        -e PGHOST="$PGHOST" \
        -e PGPORT="$PGPORT" \
        -e PGUSER="$PGUSER" \
        -e PGPASSWORD="$PGPASSWORD" \
        -e SOURCE_ENV="$SOURCE_ENV" \
        -e TARGET_ENV="$TARGET_ENV" \
        -e APPLICATION="$APP" \
        db-migration-tool  || {
            echo "Migration failed for $APP"
            exit 1
        }
echo "====================================================================="
echo ""
echo "Script executed"
echo "====================================================================="
done
