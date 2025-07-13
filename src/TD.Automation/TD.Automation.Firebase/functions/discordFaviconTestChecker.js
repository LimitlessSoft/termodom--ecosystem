const {
    Client,
    Events,
    GatewayIntentBits,
    EmbedBuilder,
} = require('discord.js')
const {
    organization,
    repo,
    privateTermodomEcosystemAnnouncementsChannel,
    termodomEcosystemRoleId,
} = require('./constants')

const discordToken = process.env.DISCORD_TOKEN

const client = new Client({
    intents: [GatewayIntentBits.Guilds, GatewayIntentBits.GuildMessages],
})

const fetchLastFaviconTest = async () => {
    const url = `https://api.github.com/repos/${organization}/${repo}/actions/workflows/_run-test-production-fav-icon.yml/runs`

    const headers = {
        Accept: 'application/vnd.github+json',
        Authorization: `token ${process.env.GH_TOKEN}`,
    }

    try {
        const response = await fetch(url, { headers })

        if (!response.ok) {
            throw new Error(
                `Error: ${response.status} - ${response.statusText}`
            )
        }

        return await response.json()
    } catch (error) {
        console.error('Failed to fetch actions:', error)
        return null
    }
}

const discordFaviconTestChecker = async () => {
    client.once(Events.ClientReady, async () => {
        try {
            const channel = client.channels.cache.get(
                privateTermodomEcosystemAnnouncementsChannel
            )

            const data = await fetchLastFaviconTest()
            const { workflow_runs } = data

            if (!data || !workflow_runs) {
                console.error('Invalid workflow data from GitHub')
                return
            }

            const latestRun = workflow_runs[0]

            if (latestRun.status !== 'completed') {
                console.log('Latest run is still in progress, skipping.')
                return
            }

            if (latestRun.conclusion === 'failure') {
                const failedTestUrl = `https://github.com/LimitlessSoft/termodom--ecosystem/actions/runs/${latestRun.id}`

                const embed = new EmbedBuilder()
                    .setColor('Red')
                    .setTitle('Production Test Coverage - Failed')
                    .setURL(failedTestUrl)
                    .setDescription(
                        'Test: "_run-test-production-fav-icon.yml" failed.'
                    )

                await channel.send({
                    content: `<@&${termodomEcosystemRoleId}>`,
                    embeds: [embed],
                })

                console.log('Sent failure embed.')
            }

            const messages = await channel.messages.fetch({ limit: 30 })

            const matchingMessages = messages.filter((msg) => {
                if (msg.embeds.length > 0) {
                    const embed = msg.embeds[0]
                    return embed.description?.includes(
                        '_run-test-production-fav-icon.yml'
                    )
                }
                return false
            })

            if (matchingMessages.size > 0) {
                for (const msg of matchingMessages.values()) {
                    await msg.react('✅')
                    console.log(`Reacted to message ID: ${msg.id}`)
                }
            } else {
                console.log('No matching previous messages found to react to.')
            }
        } catch (error) {
            console.error(
                'Unexpected error in discordFaviconTestChecker:',
                error
            )
        }
    })

    await client.login(discordToken)
}

module.exports = discordFaviconTestChecker
