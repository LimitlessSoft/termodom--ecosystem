/** @type {import('next').NextConfig} */

const getApiBaseUrlMain = () => {
    switch(process.env.DEPLOYMENT_ENVIRONMENT) {
        case 'stage':
            return 'https://api-stage.termodom.rs'
        case 'develop':
            return 'https://api-develop.termodom.rs'
        default:
            return 'error-loading-deployment-environment'
    }
}

const nextConfig = {
    async redirects() {
        return [
            {
                source: '/proizvodi',
                destination: '/',
                permanent: true
            }
        ]
    },
    async rewrites() {
        return [
            {
                source: '/',
                destination: '/proizvodi'
            }
        ]
    },
    publicRuntimeConfig: {
        API_BASE_URL_MAIN: getApiBaseUrlMain()
    }}
    
module.exports = nextConfig
