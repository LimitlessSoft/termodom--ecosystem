const {
    Client,
    Events,
    GatewayIntentBits,
    EmbedBuilder,
} = require('discord.js')
const {
    privateTermodomEcosystemAnnouncementsChannel,
    termodomEcosystemRoleId,
    organization,
    repo,
} = require('./constants')
const discordToken = process.env.DISCORD_TOKEN
const client = new Client({ intents: [GatewayIntentBits.Guilds] })

const fetchOpenPRs = async () => {
    const url = `https://api.github.com/repos/${organization}/${repo}/pulls?state=open`

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
    client.once(Events.ClientReady, async () => {
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
                const builder = new EmbedBuilder()
                    .setColor(0xffa500)
                    .setTitle(`Open Pull Requests`)
                    .setDescription(
                        `There are ${prs.length} open PRs in the termodom--ecosystem repo.`
                    )
                    .setURL(`https://github.com/${organization}/${repo}/pulls`)
                await channel.send({ embeds: [builder] })
            }
        })
    })
    await client.login(discordToken)
}
module.exports = discordOpenPullRequestsChecker
