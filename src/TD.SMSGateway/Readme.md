Install node
Install pm2
Start application.js with:
	pm2 start -l /path-to-file-you-want-to-log --cron-restart="* * * * *"

which will start app each minute
