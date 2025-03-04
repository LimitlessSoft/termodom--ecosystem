import { generateWebDbClient } from 'td-web-common-repository-node'
import { vault } from './vaultConfig.js'

export const generateAutomationWebDbClient = async () => {
    const { POSTGRES_HOST, POSTGRES_USER, POSTGRES_PASSWORD, POSTGRES_PORT } =
        await vault.getSecret('web/public/api')

    return await generateWebDbClient({
        host: POSTGRES_HOST,
        user: POSTGRES_USER,
        password: POSTGRES_PASSWORD,
        port: POSTGRES_PORT,
        database: 'automation_web',
    })
}
