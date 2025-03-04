import WebPublicClient from 'td-web-public-client-node'
import { faker } from '@faker-js/faker/locale/sr_RS_latin'
import { vault } from './configs/vaultConfig.js'

export const PROJECT_URL =
    process.env.PROJECT_URL || 'https://develop.termodom.rs/'
export const WAIT_TIMEOUT = 5 * 1000
export const PUBLIC_API_CLIENT = new WebPublicClient('http://localhost:8080')
export const BUFFER = {
    images: Array.from({ length: 10 }, () =>
        faker.image.url({ width: 500, height: 500 })
    ),
}
export const {
    POSTGRES_HOST,
    POSTGRES_USER,
    POSTGRES_PORT,
    POSTGRES_PASSWORD,
    MINIO_HOST,
    MINIO_PORT,
    MINIO_ACCESS_KEY,
    MINIO_SECRET_KEY,
} = await vault.getSecret('web/public/api')

export const appSecrets = await vault.getSecrets('web')
