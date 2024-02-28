/** @type {import('next').NextConfig} */

const getApiBaseUrlMain = () => {
    switch(process.env.DEPLOYMENT_ENVIRONMENT) {
        case 'stage':
            return 'https://api-admin-stage.termodom.rs'
        case 'develop':
            // return 'https://api-admin-develop.termodom.rs'
            return 'http://localhost:5219'
        default:
            return 'error-loading-deployment-environment'
    }
}

const nextConfig = {
    async rewrites() {
        return [
            {
                source: `/${encodeURIComponent('podešavanja')}`,
                destination: '/podesavanja',
            },
            {
                source: `/${encodeURIComponent('porudžbine')}`,
                destination: '/porudzbine',
            },
            {
                source: `/${encodeURIComponent('porudžbine')}/:id`,
                destination: '/porudzbine/[id]',
            }
        ]
    },
    publicRuntimeConfig: {
        API_BASE_URL_MAIN: getApiBaseUrlMain()
    }
}

module.exports = nextConfig
