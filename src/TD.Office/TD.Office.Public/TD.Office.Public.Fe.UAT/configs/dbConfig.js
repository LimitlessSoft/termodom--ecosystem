import OfficeDbClientFactory from 'td-office-common-repository-node'
import { vaultClient } from './vaultConfig.js'

const { POSTGRES_HOST, POSTGRES_USER, POSTGRES_PASSWORD, POSTGRES_PORT } =
    await vaultClient.getSecret('office/public/api')

export const officeDbClientFactory = new OfficeDbClientFactory({
    host: POSTGRES_HOST,
    user: POSTGRES_USER,
    password: POSTGRES_PASSWORD,
    port: POSTGRES_PORT,
    database: 'automation_tdoffice',
})
