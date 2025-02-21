import Vault from 'hashi-vault-js'

const vault = new Vault({
    https: true,
    baseUrl: 'http://vault.termodom.rs:8199/v1',
    timeout: 5000,
    proxy: false,
})

function runOnVaultServer(func) {}

async function initializeVault() {
    try {
        const status = await vault.healthCheck()
        console.log('Vault Health Status:', status)

        const token = (await vault.loginWithUserpass('filip', '123456789'))
            .client_token

        runOnVaultServer(vault.listKVSecrets)

        vault.listKVSecrets(token).then((x) => console.log(x))
    } catch (error) {
        console.error('Error connecting to Vault:', error)
    }
}

initializeVault()
