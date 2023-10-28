/** @type {import('next').NextConfig} */
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
        // API_BASE_URL_MAIN: "https://api-public-beta.termodom.rs"
        API_BASE_URL_MAIN: "http://192.168.0.17:59002"
    }}

module.exports = nextConfig
