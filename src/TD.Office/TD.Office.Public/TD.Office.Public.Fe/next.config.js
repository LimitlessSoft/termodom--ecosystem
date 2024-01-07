/** @type {import('next').NextConfig} */

const getApiBaseUrlMain = () => {
    switch(process.env.DEPLOYMENT_ENVIRONMENT) {
        case 'stage':
            return 'https://api-office-stage.termodom.rs'
        case 'develop':
            return 'https://api-office-develop.termodom.rs'
        default:
            return 'error-loading-deployment-environment'
    }
}

const nextConfig = {
    publicRuntimeConfig: {
        API_BASE_URL_MAIN: getApiBaseUrlMain(),
    }}

module.exports = nextConfig
