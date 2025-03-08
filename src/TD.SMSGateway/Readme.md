Install node
Install pm2
Start application.js with:
	pm2 start application.js --cron-restart="0 * * * *" -l /path-to-file-you-want-to-log

To run script on startup (pm2 resurrect - ressurect saved pm2 state which is saved inside run.ps1):
Task Scheduler can run the script with admin privileges silently or at startup.
Steps:

    Open Task Scheduler:

        Press Win + R, type taskschd.msc, and press Enter.

    Create a New Task:

        Click Action → Create Task.

        Name the task (e.g., Admin Script on Startup).

    Configure Settings:

        General Tab:

            Check Run with highest privileges (this grants admin rights).

            Select Configure for: Windows 10 or your OS version.

        Triggers Tab:

            Add a new trigger → At startup or At log on.

        Actions Tab:

            Add a new action → Start a program.

            Program/script: powershell.exe.

            Arguments:
            plaintext
            Copy

            -ExecutionPolicy Bypass -File "C:\Path\To\Your\Script.ps1"

        Conditions Tab:

            Uncheck unnecessary conditions (e.g., "Start only if on AC power").

    Save the Task:

        Click OK and enter admin credentials if prompted.

Result: The script will run with admin rights at startup/logon.