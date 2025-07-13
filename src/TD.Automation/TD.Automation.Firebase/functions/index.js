const { onSchedule } = require('firebase-functions/v2/scheduler')
const discordOpenPullRequestsChecker = require('./discordOpenPullRequestsChecker')
const { defineSecret } = require('firebase-functions/params')
const discordFaviconTestChecker = require('./discordFaviconTestChecker')
const GH_TOKEN = defineSecret('GH_TOKEN')
const DISCORD_TOKEN = defineSecret('DISCORD_TOKEN')

exports.discordOpenPullRequestsChecker = onSchedule(
    {
        secrets: [GH_TOKEN, DISCORD_TOKEN],
        schedule: '0 8-23 * * *',
        timeZone: 'Europe/Belgrade',
    },
    async (event) => {
        await discordOpenPullRequestsChecker()
    }
)

exports.discordWeeklyTaskReviewsChecker = onSchedule(
    {
        secrets: [GH_TOKEN, DISCORD_TOKEN],
        schedule: '0 18 * * 0',
        timeZone: 'Europe/Belgrade',
    },
    async (event) => {
        await discordOpenPullRequestsChecker()
    }
)

exports.discordFaviconTestChecker = onSchedule(
    {
        secrets: [GH_TOKEN, DISCORD_TOKEN],
        schedule: '0 10 * * *',
        timeZone: 'Europe/Belgrade',
    },
    async (_event) => {
        await discordFaviconTestChecker()
    }
)
