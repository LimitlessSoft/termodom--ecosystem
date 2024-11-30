const { Client, Events, GatewayIntentBits } = require('discord.js')
const { checkForNewPRs } = require('./domain/checkForNewPRs')

console.log('Starting...')

const discordToken = process.env.DISCORD_TOKEN

console.log(`Initializing client with token: ${discordToken}`)
const client = new Client({ intents: [GatewayIntentBits.Guilds] })

console.log(`Registering event listener for ${Events.ClientReady}`)
client.once(Events.ClientReady, async () => {
    console.log('Client ready')

    await checkForNewPRs(client)

    console.log('Exiting application')
    process.exit(1)
})

console.log('Logging in...')
client.login(discordToken)
