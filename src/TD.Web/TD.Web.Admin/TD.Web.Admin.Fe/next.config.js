/** @type {import('next').NextConfig} */
const nextConfig = {
    async rewrites() {
        return [
            {
                source: `/${encodeURIComponent('podešavanja')}`,
                destination: '/podesavanja',
            }
        ]
    },
    publicRuntimeConfig: {
        API_BASE_URL_MAIN: "https://api-beta.termodom.rs"
    }
}

module.exports = nextConfig
