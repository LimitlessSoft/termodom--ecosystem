const { onSchedule } = require('firebase-functions/v2/scheduler')
const discordOpenPullRequestsChecker = require('./discordOpenPullRequestsChecker')
const { defineSecret } = require('firebase-functions/params')
const GH_TOKEN = defineSecret('GH_TOKEN')
const DISCORD_TOKEN = defineSecret('DISCORD_TOKEN')

exports.discordOpenPullRequestsChecker = onSchedule(
    { secrets: [GH_TOKEN, DISCORD_TOKEN], schedule: '* */1 * * *' },
    async (event) => {
        await discordOpenPullRequestsChecker()
    }
)
