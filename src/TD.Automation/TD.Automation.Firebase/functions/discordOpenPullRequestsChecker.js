const { Client, Events, GatewayIntentBits } = require('discord.js')

const discordToken = process.env.DISCORD_TOKEN
const client = new Client({ intents: [GatewayIntentBits.Guilds] })
const privateTermodomEcosystemAnnouncementsChannel = '1312368860774535261'
const roleId = '1312038212063203480' // termodom--ecosystem role
const owner = 'LimitlessSoft'
const repo = 'termodom--ecosystem'

const fetchOpenPRs = async () => {
    const url = `https://api.github.com/repos/${owner}/${repo}/pulls?state=open`

    const headers = {
        Accept: 'application/vnd.github.v3+json',
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
        console.error('Failed to fetch PRs:', error)
    }
}

const discordOpenPullRequestsChecker = async () => {
    const oncePromise = client.once(Events.ClientReady, async () => {
        const channel = client.channels.cache.get(
            privateTermodomEcosystemAnnouncementsChannel
        )

        await fetchOpenPRs().then(async (prs) => {
            if (!prs) {
                await channel.send('Failed to fetch PRs')
                return
            }
            if (prs.length === 0) {
                // do nothing, everything is fine
            } else {
                await channel.send(
                    `<@&${roleId}> There are ${prs.length} open PRs in the termodom--ecosystem repo. Check them out at https://github.com/${owner}/${repo}/pulls`
                )
            }
        })
    })
    await client.login(discordToken)
}
module.exports = discordOpenPullRequestsChecker
