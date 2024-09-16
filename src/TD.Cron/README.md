
### Create cron job using node
To create new cron job project using node, follow the steps below:
 - Create a new folder for the project with format `TD.Cron.[YourJobName]CronJob`
 - Add package.json file with the following content (no additional content needed):
   - ```"scripts": { "start": "node index.js" }```
 - add `index.js` file in your job folder
 - add `src/job.js` file in your job folder
 - add `td-cron-common-domain-node` with latest version to your `dependencies` in `package.json` file
 - run `npm install` to install the dependencies
 - inside `src/job.js` file, add the following content:
   - ```module.exports = executeJobAsync(async () => { // here write your job logic })```
 - multiple node shared libraries are available through the linked workspace, you can use them in your job

<span style="color:#ff4747; font-size: 1rem"> Important > docker image should be built from root of the mono-repo, not from the job folder!</span>