const { Client, Events, GatewayIntentBits } = require('discord.js')
const {
    privateTermodomEcosystemAnnouncementsChannel,
    termodomEcosystemRoleId,
} = require('./constants')

const discordToken = process.env.DISCORD_TOKEN
const client = new Client({ intents: [GatewayIntentBits.Guilds] })

const discordWeeklyTaskReviewsChecker = async () => {
    client.once(Events.ClientReady, async () => {
        const channel = client.channels.cache.get(
            privateTermodomEcosystemAnnouncementsChannel
        )

        await channel.send(
            `<@&${termodomEcosystemRoleId}> If you already haven't, please review your tasks for the next week and update "Estimate Hours" and "Estimate finish (inclusive)" fields. Do not forget to close your '[Planning] Review next week tasks'.`
        )
    })

    await client.login(discordToken)
}

module.exports = discordWeeklyTaskReviewsChecker
