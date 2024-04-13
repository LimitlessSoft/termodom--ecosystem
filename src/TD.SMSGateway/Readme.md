Install node
Install pm2
Start application.js with:
	pm2 start application.js --cron-restart="* * * * *" -l /path-to-file-you-want-to-log 

which will start app each minute
