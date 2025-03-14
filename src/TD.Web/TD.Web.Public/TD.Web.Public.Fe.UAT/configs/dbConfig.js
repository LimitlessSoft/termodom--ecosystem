import WebDbClientFactory from 'td-web-common-repository-node'
import { vaultClient } from './vaultConfig.js'

const { POSTGRES_HOST, POSTGRES_USER, POSTGRES_PASSWORD, POSTGRES_PORT } =
    await vaultClient.getSecret('web/public/api')

export const webDbClientFactory = new WebDbClientFactory({
    host: POSTGRES_HOST,
    user: POSTGRES_USER,
    password: POSTGRES_PASSWORD,
    port: POSTGRES_PORT,
    database: 'automation_web',
})
