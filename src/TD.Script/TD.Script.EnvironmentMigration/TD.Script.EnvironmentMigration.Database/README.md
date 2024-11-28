Make sure to run the following command to make the script executable:

```bash
chmod +x ./migrateDatabase.sh
```

Then run the script with the following command:

```bash
./migrateDatabase.sh -a <application> -s <source> -t <target> -h <host> -p <port> -U <user> -P <password> 
``` 

```txt
  -a, --application <application>  Application name(s) [web, tdoffice] (required)
  -s, --source <source>            Source environment (release, develop) (required)
  -t, --target <target>            Target environment (release, develop) (required)
  -h, --host <host>                Database host (IP) (required)
  -p, --port <port>                Database port (default: 5432)
  -U, --user <user>                Database user (default: postgres)
  -P, --password <password>        Database password (required)
  --help                          Show this help message
```