import Vault from 'hashi-vault-js'
import fs from 'fs'

process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0'

const vault = new Vault({
    https: true,
    baseUrl: 'http://vault.termodom.rs:8199/v1',
    timeout: 5000,
    proxy: false,
})

async function initializeVault() {
    try {
        const status = await vault.healthCheck()
        console.log('Vault Health Status:', status)

        const token = (await vault.loginWithUserpass('filip', '123456789'))
            .client_token

        console.log('Vault Token:', token)
    } catch (error) {
        console.error('Error connecting to Vault:', error)
    }
}

initializeVault()
