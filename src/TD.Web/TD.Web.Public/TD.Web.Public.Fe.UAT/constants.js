import WebPublicClient from 'td-web-public-client-node'

export const PROJECT_URL =
    process.env.PROJECT_URL || 'https://develop.termodom.rs/'
export const ELEMENT_AWAITER_TIMEOUT = 30 * 1000
export const PUBLIC_API_CLIENT = new WebPublicClient('http://localhost:8080')
export const BUFFER_IMAGES_COUNT = 10