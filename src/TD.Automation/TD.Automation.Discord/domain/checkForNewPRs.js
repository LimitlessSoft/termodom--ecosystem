const privateTermodomEcosystemAnnouncementsChannel = '1312368860774535261'
const roleId = '1312038212063203480' // termodom--ecosystem role
const owner = 'LimitlessSoft'
const repo = 'termodom--ecosystem'

const fetchOpenPRs = async () => {
    console.log(`Fetching PRs of ${owner}/${repo}`)
    const url = `https://api.github.com/repos/${owner}/${repo}/pulls?state=open`

    console.log(`Setting headers with auth token ${process.env.GH_TOKEN}`)
    // Replace 'your_personal_access_token' with your token if needed
    const headers = {
        Accept: 'application/vnd.github.v3+json',
        // Uncomment the line below if using a personal access token
        Authorization: `token ${process.env.GH_TOKEN}`,
    }

    try {
        console.log('Fetching')
        const response = await fetch(url, { headers })

        console.log('Checking response')
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

module.exports = {
    checkForNewPRs: async function (discordClient) {
        console.log('Checking for new PRs')

        console.log('Getting channel')
        const channel = discordClient.channels.cache.get(
            privateTermodomEcosystemAnnouncementsChannel
        )

        await fetchOpenPRs().then(async (prs) => {
            console.log('PRs fetched')
            if (!prs) {
                console.error('Failed to fetch PRs')
                await channel.send('Failed to fetch PRs')
                return
            }
            if (prs.length === 0) {
                console.log('No open PRs')
                await channel.send('No open PRs')
            } else {
                console.log(`There are ${prs.length} open PRs`)
                await channel.send(
                    `<@&${roleId}> There are ${prs.length} open PRs in the termodom--ecosystem repo. Check them out at https://github.com/${owner}/${repo}/pulls`
                )
            }
        })
    },
}
