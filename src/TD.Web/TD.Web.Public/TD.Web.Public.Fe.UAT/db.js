import { vault } from './vaultConfig.js'
import { GenerateWebDbClient as GWC } from 'td-web-common-repository-node'

const { POSTGRES_HOST, POSTGRES_USER, POSTGRES_PORT, POSTGRES_PASSWORD } =
    await vault.getSecret('web/public/api')

export const GenerateWebDbClient = async () =>
    await GWC(
        POSTGRES_HOST,
        POSTGRES_USER,
        POSTGRES_PASSWORD,
        POSTGRES_PORT,
        'automation_web')