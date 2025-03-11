import VaultManager from 'td-common-vault-node'

export const vaultClient = new VaultManager({
    engine: 'automation',
    username: process.env.VAULT_USERNAME,
    password: process.env.VAULT_PASSWORD,
    uri: `${process.env.VAULT_ADDR}/v1`,
    defaultPath: '',
    defaultSecret: 'office/public/api',
})
