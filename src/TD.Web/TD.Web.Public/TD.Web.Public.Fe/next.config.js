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
        // API_BASE_URL_MAIN: "https://public-api-beta.termodom.rs",
        API_BASE_URL_MAIN: "http://localhost:5039"
    }}

module.exports = nextConfig
