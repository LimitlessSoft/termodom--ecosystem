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
const workflowPathname = '_run-test-production-fav-icon.yml'

const client = new Client({
    intents: [GatewayIntentBits.Guilds, GatewayIntentBits.GuildMessages],
})

const fetchFaviconTestRuns = async () => {
    const url = `https://api.github.com/repos/${organization}/${repo}/actions/workflows/${workflowPathname}/runs`

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

            const data = await fetchFaviconTestRuns()
            if (!data) {
                console.error('Invalid workflow data from GitHub')
                return
            }

            if (data.total_count === 0) {
                console.error(
                    'There is no run found for specified worfklow:',
                    workflowPathname
                )
                return
            }

            const latestRun = data.workflow_runs[0]

            if (latestRun.status !== 'completed') {
                console.log('Latest run is still in progress, skipping.')
                return
            }

            if (latestRun.conclusion === 'success') {
                const messages = await channel.messages.fetch({ limit: 30 })
                const matchingMessages = messages.filter((msg) =>
                    msg.embeds[0]?.description?.includes(workflowPathname)
                )

                if (!matchingMessages.size) {
                    console.log(
                        'No matching previous messages found to react to.'
                    )
                    return
                }

                for (const msg of matchingMessages.values()) {
                    await msg.react('✅')
                    console.log(`Reacted to message ID: ${msg.id}`)
                }
            }

            if (latestRun.conclusion === 'failure') {
                const embed = new EmbedBuilder()
                    .setColor('Red')
                    .setTitle('Production Test Coverage - Failed')
                    .setURL(latestRun.html_url)
                    .setDescription(`Test: "${workflowPathname}" failed.`)

                await channel.send({
                    content: `<@&${termodomEcosystemRoleId}>`,
                    embeds: [embed],
                })

                console.log('Sent failure embed.')
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
