#!/bin/bash

set -e

show_help() {
    cat << EOF
Usage: $0 [OPTIONS]

Options:
  -a, --application=<APPLICATION> Specify the Application name(s) [web-public, web-admin, office-public] (optional / if not passed, tests will be ran for all applications)
  -U, --username=<VAULT_USERNAME> Specify the Vault username (required)
  -P, --password=<VAULT_PASSWORD> Specify the Vault password (required)
  --help                      Show this help message    
EOF
    exit 0
}

ARGS=$(getopt -o a:U:P: --long application:,username:,password:,help -n "$0" -- "$@")

if [ $? -ne 0 ]; then
    exit 1
fi

eval set -- "$ARGS"

APPLICATIONS=()
VAULT_USERNAME=""
VAULT_PASSWORD=""

while [[ $# -gt 0 ]]; do
    case "$1" in
        -a|--application) APPLICATIONS+=("$2"); shift 2 ;;
        -U|--username) VAULT_USERNAME="$2"; shift 2 ;;
        -P|--password) VAULT_PASSWORD="$2"; shift 2 ;;
        --help) show_help ;;
        --) shift; break ;;
        *) echo "Invalid option: $1"; exit 1 ;;
    esac
done

if [ ${#APPLICATIONS[@]} -eq 0 ]; then
    APPLICATIONS=("office-public" "web-public" "web-admin")
fi

if [ -z "$VAULT_USERNAME" ] || [ -z "$VAULT_PASSWORD" ]; then
    echo "Error: --username and --password arguments are required!"
    echo "Use --help for usage information."
    exit 1
fi

declare -A APP_SCRIPTS
APP_SCRIPTS["office-public"]="src/TD.Office/TD.Office.Public/TD.Office.Public.Fe.UAT/runTests.sh"
APP_SCRIPTS["web-public"]="src/TD.Web/TD.Web.Public/TD.Web.Public.Fe.UAT/runTests.sh"
APP_SCRIPTS["web-admin"]="src/TD.Web/TD.Web.Admin/TD.Web.Admin.Fe.UAT/runTests.sh"

for app in "${APPLICATIONS[@]}"; do
    echo
    script="${APP_SCRIPTS[$app]}"
    if [[ -n "$script" ]]; then
        if [[ -x "$script" ]]; then
            echo "Running ${app} UAT tests..."
            bash "$script" --username "$VAULT_USERNAME" --password "$VAULT_PASSWORD" || echo "Error: UAT tests for ${app} occurred some error."
        else
            echo "Error: Script '$script' for application '$app' is not executable or not found."
        fi
    else
        echo "Warning: No script found for application '$app'"
    fi
done
