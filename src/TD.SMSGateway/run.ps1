npm install -g pm2
pm2 -v
npm i

$APPLICATION_FILE = "application.js"
$LOG_FILE = "D:\_server_data_aleksa_termodom\sms_gateway\logs.txt"
$CRON_SCHEDULE = "0 * * * *"

# Application file and log file paths
$APPLICATION_FILE = "application.js"
$LOG_FILE = "D:\_server_data_aleksa_termodom\sms_gateway\logs.txt"
$CRON_SCHEDULE = "0 * * * *"

# Application name (used to check if the app is already running)
$APPLICATION_NAME = "sms-gateway" #Choose a meaningful name

# Stop and delete the application if it's already running
try {
    pm2 stop $APPLICATION_NAME
    pm2 delete $APPLICATION_NAME
}
catch {
    Write-Host "Error stopping or deleting existing application: $($_.Exception.Message)"
}

# Start the application with PM2 and cron-restart
try {
    pm2 start $APPLICATION_FILE --time --name $APPLICATION_NAME --cron-restart "$CRON_SCHEDULE" -l $LOG_FILE
    Write-Host "Application '$APPLICATION_NAME' started with PM2."
}
catch {
    Write-Host "Error starting application with PM2: $($_.Exception.Message)"
    exit
}

# Save PM2 state
try {
    pm2 save
    Write-Host "PM2 state saved."
}
catch {
    Write-Host "Error saving PM2 state: $($_.Exception.Message)"
}
