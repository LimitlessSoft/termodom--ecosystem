import WebPublicClient from 'td-web-public-client-node'
import { faker } from '@faker-js/faker/locale/sr_RS_latin'

export const PROJECT_URL =
    process.env.PROJECT_URL || 'https://develop.termodom.rs/'
export const ELEMENT_AWAITER_TIMEOUT = 30 * 1000
export const PUBLIC_API_CLIENT = new WebPublicClient('http://localhost:8080')
export const BUFFER = {
    images: Array.from(
        { length: 10 },
        () =>
            `https://picsum.photos/${faker.number.int({ min: 300, max: 600 })}/${faker.number.int({ min: 300, max: 600 })}`
    ),
}
